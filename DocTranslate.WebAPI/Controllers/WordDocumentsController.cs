using DocTranslate.WebAPI.Attributes;
using DocTranslate.WebAPI.Helpers;
using DocTranslate.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace DocTranslate.WebAPI.Controllers
{
    public class WordDocumentsController : ApiController
    {
        private static readonly string ServerUploadFolder = HostingEnvironment.MapPath("~/Uploads");

        [ValidateMimeMultipartContentFilter]
        public async Task<IHttpActionResult> PostUploadSingleFile()
        {
            var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            FileResult result = new FileResult
            {
                FileNames = streamProvider.FileData.Select(entry => entry.LocalFileName),
                Names = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName),
                ContentTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType),
                Description = streamProvider.FormData["description"],
                CreatedTimestamp = DateTime.UtcNow,
                UpdatedTimestamp = DateTime.UtcNow,
                DownloadLink = "TODO, will implement when file is persisited",
                FileExtensions = streamProvider.FileData.Select(entry =>
                    entry.Headers.ContentDisposition.FileName.Substring(
                        entry.Headers.ContentDisposition.FileName.LastIndexOf(".")).Trim('"'))
            };

            string fileName = result.FileNames.First();
            string ext = result.FileExtensions.First();
            result.FullFileName = fileName + ext;

            File.Move(fileName, result.FullFileName);

            return Ok(result);
        }

        public IEnumerable<string> GetContentDocument(string docPath)
        {
            string fileName = Path.Combine(ServerUploadFolder, docPath.Substring(docPath.LastIndexOf("\\") + 1));

            string fileExtension = docPath.Substring(docPath.LastIndexOf("."));

            IReadable read;
            switch (fileExtension)
            {
                case ".doc":
                case ".docx":
                    read = new WordDocumentManager();
                    break;
                case ".pdf":
                    read = new PdfDocumentManager();
                    break;
                default:
                    return new List<string> { "Invalid document" };
            }

            string temp = read.GetText(fileName);

            List<string> texts = temp                
                .Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return texts;
            //return read.GetText(fileName);
        }
    }
}
