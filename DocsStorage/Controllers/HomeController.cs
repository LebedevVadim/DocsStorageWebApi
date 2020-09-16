using DocsStorage.Helpers;
using DocsStorage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Net;

namespace DocsStorage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiHelper apiHelper;

        public HomeController(ApiHelper helper)
        {
            apiHelper = helper;
        }

        public IActionResult Index()
        {
            try
            {
                var documents = apiHelper.GetDocuments(0);

                return View(documents);
            }
            catch (Exception ex)
            {
                return View("_Error", $"{ex.Message}");
            }
        }

        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("_Error", $"{ex.Message}");
            }
        }

        [Route("[controller]/[action]/{numPage}")]
        public IActionResult GetItems(int numPage)
        {
            try
            {
                var documents = apiHelper.GetDocuments(numPage);

                if (documents.Any())
                    return View(documents);
                else
                    return NotFound();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [Route("[controller]/[action]")]
        public IActionResult GetItemsByName(string name)
        {
            try
            {
                var documents = apiHelper.GetDocuments(name);
                return View(nameof(GetItems), documents);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Create([FromForm] Document document)
        {
            try
            {
                var response = apiHelper.SaveDocument(document);
                if (response.StatusCode == HttpStatusCode.OK)
                    return RedirectToAction(nameof(Index));
                else
                    return View("_Error", $"{response.StatusCode}{Environment.NewLine}{response.ErrorMessage}");
            }
            catch (Exception ex)
            {
                return View("_Error", $"{ex.Message}");
            }
        }

    }
}