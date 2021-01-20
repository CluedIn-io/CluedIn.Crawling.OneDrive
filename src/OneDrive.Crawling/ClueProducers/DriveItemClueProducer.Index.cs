using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CluedIn.ContentExtraction.Aspose.ContentExtraction.AsposeExtractors;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.FileTypes;
using CluedIn.Core.IO;
using CluedIn.Crawling.ContentExtraction;
using CluedIn.Crawling.FileIndexing;
using CluedIn.Crawling.OneDrive.Core;
using CluedIn.Crawling.OneDrive.Core.Models;
using CluedIn.Crawling.OneDrive.Vocabularies;

namespace CluedIn.Crawling.OneDrive.ClueProducers
{
    public partial class DriveItemClueProducer : BaseClueProducer<CluedInDriveItem>
    {
        private void Index([NotNull] CluedInDriveItem input, [NotNull] string webUrl, [NotNull] Clue clue)
        {
            this.state.Status.Ping();


            var data = clue.Data;
            var value = input.DriveItem;

            string hash;

            if (value.Size <= Constants.MaxFileIndexingFileSize && value.Size > 1)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var settings = new FileIndexingSettings();

                        settings.ExtractContents = true;
                        settings.GenerateThumbnail = false;

                        settings.ContentExtractionTimeout = 500;
                        settings.FilterContentExtractors = extractors => extractors.Where(e => e.Name != "Tika" && e.Name != "POI");
                        settings.ShouldSkipMultipleExtractors = (extractor, results) => (extractor.Name == "Tika" || extractor.Name == "POI") ? false : true;
                        var contentExtractor = this.appContext.Container.ResolveAll<IContentExtractor>().FirstOrDefault(c => c is AsposeContentExtractor);


                        using (var tempFile = new TemporaryFile(value.Name))
                        {
                            using (var webClient = new WebClient())
                            {
                                Stream file = new MemoryStream(webClient.DownloadData(webUrl));
                                using (var md5 = MD5.Create())
                                {
                                    using (var stream = file)
                                    {
                                        var hashBytes = md5.ComputeHash(stream);

                                        hash = BitConverter.ToString(hashBytes);

                                        using (var fileStream = System.IO.File.Create(tempFile.FilePath))
                                        {
                                            file.Seek(0, SeekOrigin.Begin);
                                            file.CopyTo(fileStream);
                                        }
                                    }
                                }
                                file.Close();
                            }

                            if (value.Name != null)
                                data.EntityData.DocumentFileName = value.Name;

                            data.EntityData.DocumentSize = tempFile.FileInfo.Length;
                            data.EntityData.Properties[OneDriveVocabularies.File.Hash] = hash;

                            appContext.Container.GetLogger().Info(() => $"Indexing file from OneDrive. File: {value.Name}");

                            var indexingResults = FileCrawlingUtility.IndexFile(appContext, state, clue, tempFile, settings);
                            if (indexingResults != null && indexingResults.IsContentExtractionSuccessful)
                            {
                                appContext.Container.GetLogger().Info(() => $"Indexed file from OneDrive. File: {value.Name}");
                            }
                            else
                            {
                                appContext.Container.GetLogger().Error(() => $"Could not index file from OneDrive. File: {value.Name}");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        appContext.Container.GetLogger().Error(() => ex.Message, ex);
                    }
                });
            }
            else
            {
                data.EntityData.DocumentFileName = value.Name;
                data.EntityData.DocumentSize = value.Size;
            }

        }
    }
}
