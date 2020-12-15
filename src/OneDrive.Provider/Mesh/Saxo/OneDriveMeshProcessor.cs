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
            return string.Join(",", new[] { entity.Properties[OneDriveVocabularies.DriveItem.Id], entity.Properties[OneDriveVocabularies.DriveItem.DriveOwnerUserId] });
        }

        public override string GetVocabularyProviderKey() =>
            OneDriveConstants.CodeOrigin.ToLowerInvariant();

        public override List<QueryResponse> RunQueries(IDictionary<string, object> config, string id, Core.Mesh.Properties properties)
        {
            var client = onedriveClientFactory.CreateNew(new OneDriveCrawlJobData(config));
            graphClient = client.graphClient;
            try
            {
                var values = id.Split(',');
                var itemId = values[0];
                var userId = values[1];

                DriveItem item = graphClient.Users[userId].Drive.Items[itemId].Request().GetAsync().Result;
                var drive = graphClient.Users[userId].Drives[item.ParentReference.DriveId].Request().GetAsync().Result;

                if (item != null)
                {
                    var stream = graphClient.Users[userId].Drive.Items[itemId].Content.Request().GetAsync().Result;

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
                        var result = client.ReplaceFile(drive, item, redacted);
                        if (result != null)
                            return new List<QueryResponse>() { new QueryResponse() { Content = $"Redacted {id}", StatusCode = HttpStatusCode.OK } };
                    }
                    else
                    {
                        var result = client.ReplaceLargeFile(drive, item, redacted);
                        if (result.UploadSucceeded)
                            return new List<QueryResponse>() { new QueryResponse() { Content = $"Redacted {id}", StatusCode = HttpStatusCode.OK } };
                    }
                }
            }
            catch (Exception ex)
            {
                this.AppContext.Container.GetLogger().Error(() => $"OD MESH Exception. ID {id}. Message: {ex.Message}", ex);
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
