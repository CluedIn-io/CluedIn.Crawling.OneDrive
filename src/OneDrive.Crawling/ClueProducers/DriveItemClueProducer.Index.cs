using System;
using System.IO;
using System.Security.Cryptography;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.FileTypes;
using CluedIn.Core.IO;
using CluedIn.Crawling.OneDrive.Core;
using CluedIn.Crawling.OneDrive.Core.Models;
using CluedIn.Crawling.OneDrive.Vocabularies;
using Microsoft.Graph;

namespace CluedIn.Crawling.OneDrive.ClueProducers
{
    public partial class DriveItemClueProducer : BaseClueProducer<CluedInDriveItem>
    {      
        private void Index([NotNull] CluedInDriveItem value, [NotNull] string webUrl, [NotNull] Clue clue)
        {
            var data = clue.Data;

            string hash;

            if (value.Size <= CluedIn.Core.Constants.MaxFileIndexingFileSize)
            {
                using (var tempFile = new TemporaryFile(value.Name))
                {
                    Stream file = System.Net.WebRequest.Create(webUrl).GetResponse().GetResponseStream();
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

                    data.EntityData.Codes.Add(new EntityCode(EntityType.Files.File, OneDriveConstants.CodeOrigin, hash));

                    MimeType mimeType = tempFile.FileInfo.ToMimeType();

                    if (value.Name != null)
                        data.EntityData.DocumentFileName = value.Name;

                    data.EntityData.DocumentSize = tempFile.FileInfo.Length;
                    data.EntityData.DocumentMimeType = mimeType.Code;
                    data.EntityData.Properties[OneDriveVocabularies.File.Hash] = hash;

                    FileCrawlingUtility.IndexFile(tempFile, clue.Data, clue, state, appContext);
                }
            }
            else
            {
                data.EntityData.DocumentFileName = value.Name;
                data.EntityData.DocumentSize = value.Size;
            }
        }
    }
}
