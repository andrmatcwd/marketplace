using Marketplace.Web.Models.Common;
using Marketplace.Web.Seo;
using Marketplace.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string culture,
            CancellationToken cancellationToken)
        {
            var vm = await _catalogService.GetCatalogIndexPageAsync(culture, cancellationToken);

            this.SetSeo(new PageSeoData
            {
                Title = "Каталог послуг",
                Description = "Знайдіть послуги за містом, категорією та підкатегорією.",
                CanonicalUrl = Url.Action("Index", "Catalog", null, Request.Scheme),
                Robots = "index,follow"
            });

            return View(vm);
        }

        public async Task<IActionResult> City(
            string culture,
            string city,
            [FromQuery] BaseFilter filter,
            CancellationToken cancellationToken)
        {
            var vm = await _catalogService.GetCityPageAsync(
                culture, city, filter, cancellationToken);
            if (vm is null) return NotFound();

            this.SetSeo(new PageSeoData
            {
                Title = $"Послуги у {vm.CityName}",
                Description = $"Каталог послуг у {vm.CityName}.",
                CanonicalUrl = Url.Action("City", "Catalog", new { city = vm.CitySlug }, Request.Scheme),
                Robots = "index,follow"
            });

            return View(vm);
        }

        public async Task<IActionResult> Category(
            string culture,
            string city,
            string category,
            [FromQuery] BaseFilter filter,
            CancellationToken cancellationToken)
        {
            var vm = await _catalogService.GetCategoryPageAsync(
                culture, city, category, filter, cancellationToken);
            if (vm is null) return NotFound();

            this.SetSeo(new PageSeoData
            {
                Title = $"{vm.CategoryName} у {vm.CityName}",
                Description = $"{vm.CategoryName} у {vm.CityName}. Каталог послуг.",
                CanonicalUrl = Url.Action("Category", "Catalog", new
                {
                    city = vm.CitySlug,
                    category = vm.CategorySlug
                }, Request.Scheme),
                Robots = "index,follow"
            });

            return View(vm);
        }

        public async Task<IActionResult> Subcategory(
            string culture,
            string city,
            string category,
            string subcategory,
            [FromQuery] BaseFilter filter,
            CancellationToken cancellationToken)
        {
            var vm = await _catalogService.GetSubCategoryPageAsync(
                culture, city, category, subcategory, filter, cancellationToken);
            if (vm is null) return NotFound();

            this.SetSeo(new PageSeoData
            {
                Title = $"{vm.SubCategoryName} у {vm.CityName}",
                Description = $"{vm.SubCategoryName} у {vm.CityName}. Перелік виконавців і компаній.",
                CanonicalUrl = Url.Action("Subcategory", "Catalog", new
                {
                    city = vm.CitySlug,
                    category = vm.CategorySlug,
                    subcategory = vm.SubCategorySlug
                }, Request.Scheme),
                Robots = "index,follow"
            });

            return View(vm);
        }
    }
}
