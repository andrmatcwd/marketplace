using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers
{
    public class ServicesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
