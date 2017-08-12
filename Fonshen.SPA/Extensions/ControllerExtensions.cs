using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Fonshen.SPA.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult Page(this Controller Controller, object data, string view = "Default")
        {
            var cache = Controller.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            var controller = Controller.HttpContext.GetRouteValue("controller");
            var action = Controller.HttpContext.GetRouteValue("action");
            var css = Controller.GetLastFile(cache, "css", $"{controller}/{action}.css");
            var js = Controller.GetLastFile(cache, "js", $"{controller}/{action}.js");
            var result = new
            {
                data = data,
                css = css,
                js = js
            };
            if (Controller.Request.Query["_by_ajax"] == "1")
            {
                return Controller.Json(result);
            }
            else
            {
                css = Controller.GetLastFile(cache, "css", "Basic.css");
                js = Controller.GetLastFile(cache, "js", "Basic.js");
                Controller.ViewData["Fonshen.SPA"] = $@"<link href=""{css}"" rel=""stylesheet"" type=""text/css"" />
    <script src=""{js}""></script>
    <script>
        Page.Raw={JsonConvert.SerializeObject(result)};
    </script>";
                return Controller.View(view);
            }
        }

        private static string GetLastFile(this Controller controller, IMemoryCache cache, string folder, string fileName)
        {
            string key = $"AppendVersion.{folder}.{fileName}";
            string value;
            if (!cache.TryGetValue(key, out value))
            {
                var _env = controller.HttpContext.RequestServices.GetRequiredService<IHostingEnvironment>();
                value = $"/{folder}/{fileName}";
                var options = new MemoryCacheEntryOptions().AddExpirationToken(_env.WebRootFileProvider.Watch(value));
                var fileInfo = _env.WebRootFileProvider.GetFileInfo(value);

                if (fileInfo.Exists)
                {
                    value += fileInfo.LastModified.ToLocalTime().ToString("?yyMMddHHmmss");
                }
                cache.Set(key, value, options);
            }
            return value;
        }
    }
}