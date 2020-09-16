using System;
using System.Linq;
using DocsStorageWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace DocsStorageWebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocRepository documents;

        private const int countElemetsInPage = 10;

        private readonly Logger logger;

        public DocumentController(IDocRepository repository)
        {
            documents = repository;
            logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet("document/{id}")]
        public IActionResult GetDocument(int id)
        {
            logger.Info(BuildInfoMessage(nameof(GetDocument), $"{nameof(id)} = {id}"));

            try
            {
                var doc = documents.GetDocument(id);

                return new JsonResult(doc);
            }
            catch (Exception ex)
            {
                logger.Error(BuildErrorMessage(nameof(GetDocument), ex, $"{nameof(id)} = {id}"));
                return StatusCode(500);
            }
        }

        [HttpGet("documentsList/{numPage}")]
        public IActionResult GetDocuments(int numPage)
        {
            logger.Info(BuildInfoMessage(nameof(GetDocuments), $"{nameof(numPage)} = {numPage}"));

            try
            {
                var offsetCount = countElemetsInPage * numPage;
                var doc = documents.GetDocuments(offsetCount, countElemetsInPage);
                return new JsonResult(doc);
            }
            catch (Exception ex)
            {
                logger.Error(BuildErrorMessage(nameof(GetDocuments), ex, $"{nameof(numPage)} = {numPage}"));
                return StatusCode(500);
            }
        }

        [HttpGet("documents")]
        public IActionResult GetDocumentByName(string name)
        {
            logger.Info(BuildInfoMessage(nameof(GetDocumentByName), $"{nameof(name)} = {name}"));
            try
            {
                var docs = documents.Documents.Where(x => x.DocumentName.ToLower().Contains(name.ToLower())).ToList();

                return new JsonResult(docs);
            }
            catch (Exception ex)
            {
                logger.Error(BuildErrorMessage(nameof(GetDocumentByName), ex, $"{nameof(name)} = {name}"));
                return StatusCode(500);
            }
        }

        [HttpGet("download/{id}")]
        public IActionResult DownloadDocumentFile(int id)
        {
            logger.Info(BuildInfoMessage(nameof(DownloadDocumentFile), $"{nameof(id)} = {id}"));

            try
            {
                var file = documents.GetDownloadableFile(id);
                
                if (file == null)
                    return StatusCode(404);

                return File(file.DocumentData, "application/zip", $"{file.DocumentName}.zip");
            }
            catch (Exception ex)
            {
                logger.Error(BuildErrorMessage(nameof(DownloadDocumentFile), ex, $"{nameof(id)} = {id}"));
                return StatusCode(500);
            }
 
        }

        [HttpPost("[action]")]
        public IActionResult Save([FromForm] Document document)
        {
            var paramsInfo = new string[] {$"{nameof(document.DocumentID)} = {document.DocumentID}",
                                                       $"{nameof(document.DocumentName)} = {document.DocumentName}",
                                                       $"{nameof(document.Author)} = {document.Author}",
                                                       $"{nameof(document.Description)} = {document.Description}",
                                                       $"{nameof(document.Exstension)} = {document.Exstension}",
                                                       $"{nameof(document.LoadDate)} = {document.LoadDate}" };
            
            logger.Info(BuildInfoMessage(nameof(Save), paramsInfo));

            try
            {
                documents.Save(document);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(BuildErrorMessage(nameof(Save), ex, paramsInfo));

                return StatusCode(500);
            }
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            logger.Info(BuildInfoMessage(nameof(Delete), $"{nameof(id)} = {id}"));

            try
            {
                documents.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(BuildErrorMessage(nameof(Delete), ex, $"{nameof(id)} = {id}"));

                return StatusCode(500);
            }
        }

        private string BuildInfoMessage(string actionName, params string[] paramsAction)
        {
            return $"Controller: Document, Action: {actionName}, Params: {string.Join(", ", paramsAction)}";
        }

        private string BuildErrorMessage(string actionName, Exception ex, params string[] paramsAction)
        {
            return $"Controller: Document, Action: {actionName}, Params: {string.Join(", ", paramsAction)}, Error: {ex.InnerException?.Message}, {ex.Message}, StackTrace: {Environment.NewLine}{ex.InnerException?.StackTrace}{Environment.NewLine}{ex.StackTrace}";
        }
    }
}