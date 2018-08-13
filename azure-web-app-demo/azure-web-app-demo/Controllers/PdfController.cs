using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using azure_web_app_demo.Models;
using azure_web_app_demo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace azure_web_app_demo.Controllers
{
    public class PdfController : Controller
    {
        private readonly PdfStorage pdfStorage;

        public PdfController(PdfStorage pdfStorage)
        {
            this.pdfStorage = pdfStorage;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile pdf)
        {
            if (pdf != null)
            {
                using (var stream = pdf.OpenReadStream())
                {
                    var pdfId = await pdfStorage.SavePdf(stream);
                    return RedirectToAction("Show", new { id = pdfId });
                }
            }

            return View();
        }

        public ActionResult Show(string id)
        {
            var model = new ShowModel { Uri = pdfStorage.UriFor(id) };
            return View(model);
        }
    }
}