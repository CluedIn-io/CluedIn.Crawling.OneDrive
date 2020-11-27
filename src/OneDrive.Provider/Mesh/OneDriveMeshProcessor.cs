using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Aspose.Slides;
using Aspose.Words;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Mesh;
using CluedIn.Core.Messages.Processing;
using CluedIn.Core.Messages.WebApp;
using CluedIn.Crawling.OneDrive.Core;
using CluedIn.Crawling.OneDrive.Infrastructure.Factories;
using CluedIn.Crawling.OneDrive.Vocabularies;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Threading;
using Aspose.Cells;
using ExecutionContext = CluedIn.Core.ExecutionContext;

namespace CluedIn.Providers.Mesh
{
    public class OneDrive_Command_MeshProcessor : SaxoBankRedactionMeshProcessor
    {
        private GraphServiceClient graphClient;


        public OneDrive_Command_MeshProcessor(ApplicationContext appContext) : base(appContext)
        {

        }

        public OneDrive_Command_MeshProcessor(ApplicationContext appContext, string editUrl, ActionType actionType, params EntityType[] entityType)
            : base(appContext, editUrl, ActionType.UPDATE, Core.Data.EntityType.Infrastructure.DirectoryItem)

        {

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
            if(graphClient == null)
            {
                graphClient = GetClient(config);
            }

            var item = graphClient.Drive.Items[id].Request().GetAsync().Result;
            var stream = graphClient.Drive.Items[id].Content.Request().GetAsync().Result;
            var extension = item.Name.Split('.').LastOrDefault();
            var redacted = Replace(stream, extension, properties.properties);
            var result = ReplaceLargeFile(config, item, redacted);
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

        private GraphServiceClient GetClient(IDictionary<string, object> config)
        {
            var onedriveCrawlJobData = new OneDriveCrawlJobData(config);

            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                               .Create(onedriveCrawlJobData.ClientID)
                               .WithAuthority($"https://login.microsoftonline.com/{onedriveCrawlJobData.Tenant}/v2.0")
                               .WithClientSecret(onedriveCrawlJobData.ClientSecret)
                               .Build();

            graphClient =
              new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
              {
                  var authResult = await confidentialClientApplication
                        .AcquireTokenForClient(new string[] { "https://graph.microsoft.com/.default" })
                        .ExecuteAsync();

                  requestMessage.Headers.Authorization =
                     new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
              })
              );

            return graphClient;
        }

        public override List<QueryResponse> Validate(ExecutionContext context, MeshDataCommand command, IDictionary<string, object> config, string id, MeshQuery query)
        {
            if (graphClient == null)
            {
                graphClient = GetClient(config);
            }

            var item = ActionExtensions.ExecuteWithRetry(() => { return graphClient.Drive.Items[id].Request().GetAsync().Result; });

            if (item == null)
                return new List<QueryResponse>() { new QueryResponse() { Content = null, StatusCode = System.Net.HttpStatusCode.InternalServerError } };

            //TODO search file content

            return new List<QueryResponse>()
            {
                new QueryResponse() { Content = JsonConvert.SerializeObject(item), StatusCode = System.Net.HttpStatusCode.OK }
            };

        }

        public UploadResult<DriveItem> ReplaceLargeFile(IDictionary<string, object> config, DriveItem item, Stream stream)
        {
            if (graphClient == null)
            {
                graphClient = GetClient(config);
            }

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

    public abstract class SaxoBankRedactionMeshProcessor : BaseMeshProcessor
    {
        public EntityType[] EntityType { get; }
        public string EditUrl { get; }
        public ActionType ActionType { get; }

        public SaxoBankRedactionMeshProcessor(ApplicationContext appContext) : base(appContext)
        {

        }

        public SaxoBankRedactionMeshProcessor(ApplicationContext appContext, string editUrl, ActionType actionType, params EntityType[] entityType)
            : base(appContext)
        {
            EntityType = entityType;
            EditUrl = editUrl;
            ActionType = actionType;
        }

        public override bool Accept(MeshDataCommand command, MeshQuery query, IEntity entity)
        {
            return command.ProviderId == this.GetProviderId() && query.Action == ActionType && EntityType.Contains(entity.EntityType);
        }

        public Stream Replace(Stream stream, string extension, List<Property> transforms)
        {
            Stream result = null;
            bool replaceWords = false;
            bool replaceCells = false;
            bool replaceCsv = false;
            bool replaceSlides = false;

            switch (extension)
            {
                case ".doc":
                case ".docm":
                case ".docx":
                case ".dot":
                case ".dotx":
                case ".dotm":
                case ".odt":
                case ".ott":
                case ".rtf":
                    replaceWords = true;
                    break;

                case ".xls":
                case ".xlsb":
                case ".xlsx":
                case ".xlsm":
                case ".xlt":
                case ".xltm":
                case ".xltx":
                case ".ods":
                case ".ots":
                case ".numbers":
                case ".nmbtemplate":
                    replaceCells = true;
                    break;

                case ".csv":
                    replaceCsv = true;
                    break;

                case ".pptx":
                case ".ppt":
                case ".pptm":
                case ".pot":
                case ".potm":
                case ".potx":
                case ".pps":
                case ".ppsm":
                case ".ppsx":
                case ".odp":
                case ".otp":
                    replaceSlides = true;
                    break;
            }

            if (replaceWords)
            {
                var doc = new Document(stream);
                foreach (var key in transforms)
                    doc.Range.Replace(key.name, key.value);
                doc.Save(result, Aspose.Words.SaveFormat.Docx);
            }
            else if (replaceCells)
            {
                var doc = new Aspose.Cells.Workbook(stream);
                foreach (var key in transforms)
                    doc.Replace(key.name, key.value);
                doc.Save(result, Aspose.Cells.SaveFormat.Xlsx);
            }
            else if (replaceCsv)
            {

            }
            else if (replaceSlides)
            {
                using (var slideshow = new Presentation(stream))
                {
                    foreach (var slide in slideshow.Slides)
                        foreach (var shape in slide.Shapes)
                        {
                            string str = shape.AlternativeText;
                            foreach (var key in transforms)
                            {
                                if (str.Contains(key.name))
                                {
                                    int idx = str.IndexOf(key.name);

                                    string strStartText = str.Substring(0, idx);
                                    string strEndText = str.Substring(idx + key.name.Length, str.Length - 1 - (idx + key.name.Length - 1));
                                    shape.AlternativeText = strStartText + key.value + strEndText;
                                }
                            }
                            slideshow.Save(result, Aspose.Slides.Export.SaveFormat.Pptx);
                        }
                }
            }
            return result;
        }
    }

}
