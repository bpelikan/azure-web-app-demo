using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using azure_web_app_demo.Models;
using azure_web_app_demo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace azure_web_app_demo.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageStorage imageStorage;
        private readonly ImageAnalysisStore imageAnalysisStore;

        public ImageController(ImageStorage imageStorage, ImageAnalysisStore imageAnalysisStore)
        {
            this.imageStorage = imageStorage;
            this.imageAnalysisStore = imageAnalysisStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile image)
        {
            if (image != null)
            {
                using (var stream = image.OpenReadStream())
                {
                    var imageId = await imageStorage.SaveImage(stream, image.FileName);
                    return RedirectToAction("Show", new { id = imageId });
                }
            }

            return View();
        }

        public ActionResult Show(string id)
        {
            var model = new ShowModel
            {
                Uri = imageStorage.UriFor(id),
                Analysis = ""
            };

            var analysis = imageAnalysisStore.GetImageAnalysis(id);
            if (analysis != null)
            {
                model.Analysis = JsonConvert.SerializeObject(analysis, Formatting.Indented);
            }

            return View(model);
        }
    }
}