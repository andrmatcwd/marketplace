using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        // GET: CategoriesController
        public ActionResult Index()
        {
            return View();
        }

    }
}
