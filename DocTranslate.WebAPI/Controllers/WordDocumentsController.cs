using DocTranslate.WebAPI.Attributes;
using DocTranslate.WebAPI.Helpers;
using DocTranslate.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace DocTranslate.WebAPI.Controllers
{
    public class WordDocumentsController : ApiController
    {
        private static readonly string ServerUploadFolder = HostingEnvironment.MapPath("~/Uploads");
        private static readonly string ServerSaveFolder = HostingEnvironment.MapPath("~/Saves");

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

            string originalName = result.Names.First().Trim(new char[] { '\\', '"' });
            string fileName = result.FileNames.First();
            //string ext = result.FileExtensions.First();
            result.FullFileName = Path.Combine(ServerUploadFolder, originalName);

            File.Move(fileName, result.FullFileName);

            return Ok(result);
        }

        public IHttpActionResult GetContentDocument(string docPath)
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
                    return NotFound();
            }

            string temp = Regex.Replace(read.GetText(fileName), @"\.\s?\n", "¿¿");
            temp = Regex.Replace(temp, "\n", " ");

            List<string> paragraphtemp = temp
                .Split(new string[] { "¿¿" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            List<Paragraph> paragraphs = new List<Paragraph>();
            for (int id = 0; id < paragraphtemp.Count; id++)
            {
                Paragraph p = new Paragraph();
                p.ParagraphId = id;
                p.OriginLanguage = paragraphtemp[id];
                p.TargetLanguage = string.Empty;
                paragraphs.Add(p);
            }

            string json = JsonConvert.SerializeObject(paragraphs);
            StreamWriter sw = new StreamWriter(Path.Combine(ServerSaveFolder, docPath.Substring(docPath.LastIndexOf("\\") + 1)).Replace(fileExtension, ".json"), true);
            sw.Write(json);
            sw.Close();

            return Ok(paragraphs);
            //return read.GetText(fileName);
        }
    }
}
