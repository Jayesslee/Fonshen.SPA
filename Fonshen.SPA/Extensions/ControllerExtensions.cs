using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fonshen.SPA.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult Page(this Controller controller, object data, string view = "Default")
        {
            if (controller.Request.Query["_by_ajax"] == "1")
            {
                return controller.Json(data);
            }
            else
            {
                controller.ViewData["Page.Data"] = JsonConvert.SerializeObject(data);
                return controller.View(view);
            }
        }
    }
}