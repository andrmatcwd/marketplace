using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers.Admin
{
    public class CategoriesAdminController : Controller
    {
        // GET: CategoriesAdminController
        public ActionResult Index()
        {
            return View("~/Views/Admin/Categories/Index.cshtml");
        }

    }
}
