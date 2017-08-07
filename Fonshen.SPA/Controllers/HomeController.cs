using Fonshen.SPA.Extensions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fonshen.SPA.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var data = new
            {
                content = "index"
            };
            return this.Page(data);
        }

        public IActionResult About(int id)
        {
            var data = new
            {
                content = "about",
                page = id
            };
            return this.Page(data);
        }

        public IActionResult Contact()
        {
            var data = new
            {
                content = "contact"
            };
            return this.Page(data);
        }
    }
}
