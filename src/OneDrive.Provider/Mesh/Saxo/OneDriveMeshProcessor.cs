using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Mesh;
using CluedIn.Core.Messages.Processing;
using CluedIn.Core.Messages.WebApp;
using CluedIn.Crawling.OneDrive.Core;
using CluedIn.Crawling.OneDrive.Infrastructure.Factories;
using CluedIn.Crawling.OneDrive.Vocabularies;
using CluedIn.SaxoBank.Common;
using Microsoft.Graph;

namespace CluedIn.Providers.Mesh
{
    public class OneDriveSaxoBankRedactionMeshProcessor : SaxoBankRedactionMeshProcessor
    {
        private GraphServiceClient graphClient;

        private IOneDriveClientFactory onedriveClientFactory = null;

        public OneDriveSaxoBankRedactionMeshProcessor(ApplicationContext appContext, IOneDriveClientFactory onedriveClientFactory)
            : base(appContext, CluedIn.Core.Data.EntityType.Infrastructure.DirectoryItem)
        {
            this.onedriveClientFactory = onedriveClientFactory;
        }

        public override Guid GetProviderId() =>
          OneDriveConstants.ProviderId;

        public override string GetLookupId(IEntity entity)
        {
            return string.Join("|", new[] { entity.Properties[OneDriveVocabularies.DriveItem.Id], entity.Properties[OneDriveVocabularies.DriveItem.DriveOwnerUserId], entity.Properties["saxo.file.path"] });
        }

        public override string GetVocabularyProviderKey() =>
            OneDriveConstants.CodeOrigin.ToLowerInvariant();

        public override List<QueryResponse> RunQueries(IDictionary<string, object> config, string id, Core.Mesh.Properties properties)
        {
            var client = onedriveClientFactory.CreateNew(new OneDriveCrawlJobData(config));
            graphClient = client.graphClient;
            try
            {
                var values = id.Split('|');
                var itemId = values[0];
                var userId = values[1];
                string driveId = null;

                try
                {
                    driveId = values[2].Split('/')[2];
                }
                catch (Exception ex)
                {
                    this.AppContext.Container.GetLogger().Error(() => $"OD MESH Exception. Could not get drive id. ID {id}. Message: {ex.Message}", ex);
                }

                Drive drive = null;
                DriveItem item = null;
                var useDriveId = false;

                if (!string.IsNullOrWhiteSpace(driveId))
                {
                    try
                    {
                        drive = graphClient.Drives[driveId].Request().GetAsync().Result;
                        item = graphClient.Drives[driveId].Items[itemId].Request().GetAsync().Result;
                        useDriveId = true;
                    }
                    catch (Exception ex)
                    {
                        this.AppContext.Container.GetLogger().Error(() => $"OD MESH Exception. Could not get item by drive id. ID {id}. Message: {ex.Message}", ex);
                    }
                }

                try
                {
                    if (item == null)
                        item = graphClient.Users[userId].Drive.Items[itemId].Request().GetAsync().Result;

                    if (drive == null)
                        drive = graphClient.Users[userId].Drives[item.ParentReference.DriveId].Request().GetAsync().Result;
                }
                catch (Exception ex)
                {
                    this.AppContext.Container.GetLogger().Error(() => $"OD MESH Exception. Could not get item by user id. ID {id}. Message: {ex.Message}", ex);
                }


                if (item == null || drive == null)
                {
                    this.AppContext.Container.GetLogger().Error(() => $"OD MESH Exception. Item or drive null. Ending MESH.");
                    return new List<QueryResponse>() { new QueryResponse() { Content = "Item or drive null. Ending MESH.", StatusCode = HttpStatusCode.InternalServerError } };
                }


                Stream stream = null;

                if (useDriveId)
                {
                    stream = graphClient.Drives[driveId].Items[itemId].Content.Request().GetAsync().Result;
                }

                if (!useDriveId || stream == null || stream.Length == 0)
                {
                    stream = graphClient.Users[userId].Drive.Items[itemId].Content.Request().GetAsync().Result;
                }

                if (stream == null || stream.Length == 0)
                {
                    this.AppContext.Container.GetLogger().Warn(() => $"OD MESH Exception. Stream is null. DriveID: {useDriveId}. ID {id}.");
                    return new List<QueryResponse>() { new QueryResponse() { Content = "Stream is empty", StatusCode = HttpStatusCode.InternalServerError } };
                }

                var temporaryStream = new MemoryStream();

                stream.CopyTo(temporaryStream);
                stream.Position = 0;

                temporaryStream.Position = 0;

                var extension = "";
                var index = item.Name.LastIndexOf('.');
                if (index >= 0)
                    extension = item.Name.Substring(index);

                var redacted = Replace(temporaryStream, extension, properties.properties);

                if (redacted == null || redacted.Length == 0)
                    return new List<QueryResponse>() { new QueryResponse() { Content = "Replaced stream is empty", StatusCode = HttpStatusCode.InternalServerError } };

                redacted.Position = 0;

                if (item.Size < 4000000)
                {
                    var result = client.ReplaceFile(userId, drive, item, redacted);
                    if (result != null)
                        return new List<QueryResponse>() { new QueryResponse() { Content = $"Redacted {id}", StatusCode = HttpStatusCode.OK } };
                }
                else
                {
                    var result = client.ReplaceLargeFile(userId, drive, item, redacted);
                    if (result.UploadSucceeded)
                        return new List<QueryResponse>() { new QueryResponse() { Content = $"Redacted {id}", StatusCode = HttpStatusCode.OK } };
                }

            }
            catch (Exception ex)
            {
                this.AppContext.Container.GetLogger().Error(() => $"OD MESH Exception. ID {id}. Message: {ex.Message}", ex);
                return new List<QueryResponse>() { new QueryResponse() { Content = $"OD MESH Fail: ID: {id}. Ex: {ex.Message};", StatusCode = HttpStatusCode.InternalServerError } };
            }
            return new List<QueryResponse>() { new QueryResponse() { Content = $"OD MESH Fail: ID: {id};", StatusCode = HttpStatusCode.InternalServerError } };
        }

        public override List<QueryResponse> Validate(ExecutionContext context, MeshDataCommand command, IDictionary<string, object> config, string id, MeshQuery query)
        {
            return new List<QueryResponse>();
        }

        public override void DoProcess(ExecutionContext context, MeshDataCommand command, IDictionary<string, object> jobData, MeshQuery query)
        {
            return;
        }

        public override List<RawQuery> GetRawQueries(IDictionary<string, object> config, IEntity entity, Core.Mesh.Properties properties)
        {
            return new List<RawQuery>()
            {
                new RawQuery(){ Query = $"Redaction query for {entity.Uri} (ItemId {entity.Codes.FirstOrDefault().Value}) with transforms {JsonUtility.Serialize(properties)}"}
            };
        }
    }
}
