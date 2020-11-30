using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Newtonsoft.Json;

namespace CluedIn.Providers.Mesh
{
    public class OneDrive_Command_MeshProcessor : SaxoBankRedactionMeshProcessor
    {
        private GraphServiceClient graphClient;

        private IOneDriveClientFactory _onedriveClientFactory = null;

        public OneDrive_Command_MeshProcessor(ApplicationContext appContext, IOneDriveClientFactory onedriveClientFactory)
            : base(appContext, CluedIn.Core.Data.EntityType.Infrastructure.DirectoryItem)
        {
            this._onedriveClientFactory = onedriveClientFactory;
        }

        public override Guid GetProviderId() =>
          OneDriveConstants.ProviderId;

        public override string GetLookupId(IEntity entity)
        {
            return entity.Properties[OneDriveVocabularies.DriveItem.Id];
        }

        public override string GetVocabularyProviderKey() =>
            OneDriveConstants.CodeOrigin.ToLowerInvariant();

        public override List<QueryResponse> RunQueries(IDictionary<string, object> config, string id, Core.Mesh.Properties properties)
        {
            graphClient = _onedriveClientFactory.CreateNew(new OneDriveCrawlJobData(config)).graphClient;
            var item = graphClient.Drive.Items[id].Request().GetAsync().Result;
            var stream = graphClient.Drive.Items[id].Content.Request().GetAsync().Result;
            var extension = item.Name.Split('.').LastOrDefault();
            var redacted = Replace(stream, extension, properties.properties);
            var result = ReplaceLargeFile(item, redacted);
            if (result.UploadSucceeded)
                return new List<QueryResponse>()
                    {
                        new QueryResponse() { Content = null, StatusCode = System.Net.HttpStatusCode.OK }
                    };

            return new List<QueryResponse>()
            {
                new QueryResponse() { Content = null, StatusCode = System.Net.HttpStatusCode.InternalServerError }
            };
        }

        public override List<QueryResponse> Validate(ExecutionContext context, MeshDataCommand command, IDictionary<string, object> config, string id, MeshQuery query)
        {
            var item = ActionExtensions.ExecuteWithRetry(() => { return graphClient.Drive.Items[id].Request().GetAsync().Result; });

            if (item == null)
                return new List<QueryResponse>() { new QueryResponse() { Content = null, StatusCode = System.Net.HttpStatusCode.InternalServerError } };

            //TODO search file content

            return new List<QueryResponse>()
            {
                new QueryResponse() { Content = JsonConvert.SerializeObject(item), StatusCode = System.Net.HttpStatusCode.OK }
            };

        }

        public UploadResult<DriveItem> ReplaceLargeFile(DriveItem item, Stream stream)
        {
            UploadResult<DriveItem> uploadResult = null;
            using (var fileStream = (FileStream)stream)
            {
                var uploadProps = new DriveItemUploadableProperties
                {
                    ODataType = null,
                    AdditionalData = new Dictionary<string, object>
                    {
                        { "@microsoft.graph.conflictBehavior", "replace" }
                    }
                };

                var uploadSession = graphClient.Drive.Root
                    .ItemWithPath(item.Name)
                    .CreateUploadSession(uploadProps)
                    .Request()
                    .PostAsync();

                int maxSliceSize = 320 * 1024;
                var fileUploadTask =
                    new LargeFileUploadTask<DriveItem>(uploadSession.Result, fileStream, maxSliceSize);

                IProgress<long> progress = new Progress<long>(slice =>
                {
                    Console.WriteLine($"Uploaded {slice} bytes of {fileStream.Length} bytes");
                });

                try
                {
                    uploadResult = fileUploadTask.UploadAsync(progress).Result;

                    if (uploadResult.UploadSucceeded)
                    {
                        Console.WriteLine($"Upload complete, item ID: {uploadResult.ItemResponse.Id}");
                    }
                    else
                    {
                        Console.WriteLine("Upload failed");
                    }
                }
                catch (ServiceException ex)
                {
                    Console.WriteLine($"Error uploading: {ex.ToString()}");
                }
            }
            return uploadResult;
        }

        public override void DoProcess(ExecutionContext context, MeshDataCommand command, IDictionary<string, object> jobData, MeshQuery query)
        {
            return;
        }

        public override List<RawQuery> GetRawQueries(IDictionary<string, object> config, IEntity entity, Core.Mesh.Properties properties)
        {
            return new List<RawQuery>();
        }
    }
}
