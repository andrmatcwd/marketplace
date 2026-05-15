using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Models.Vacancies;
using Marketplace.Web.Navigation;
using MediatR;

namespace Marketplace.Web.Services.Vacancies;

public sealed class VacanciesService : IVacanciesService
{
    private readonly IMediator _mediator;
    private readonly ICatalogVmMapper _mapper;
    private readonly ICatalogUrlBuilder _urlBuilder;
    private readonly ICatalogBreadcrumbBuilder _breadcrumbBuilder;

    public VacanciesService(
        IMediator mediator,
        ICatalogVmMapper mapper,
        ICatalogUrlBuilder urlBuilder,
        ICatalogBreadcrumbBuilder breadcrumbBuilder)
    {
        _mediator = mediator;
        _mapper = mapper;
        _urlBuilder = urlBuilder;
        _breadcrumbBuilder = breadcrumbBuilder;
    }

    public async Task<VacanciesPageVm> GetVacanciesPageAsync(
        string culture,
        VacanciesFilterVm filter,
        CancellationToken cancellationToken)
    {
        filter.Page = filter.Page < 1 ? 1 : filter.Page;
        filter.PageSize = filter.PageSize <= 0 ? 12 : Math.Min(filter.PageSize, 60);
        filter.ResetUrl = _urlBuilder.BuildVacanciesUrl(culture);

        var citiesTask = _mediator.Send(new GetCatalogCitiesQuery(), cancellationToken);
        var employmentTypesTask = _mediator.Send(new GetVacancyEmploymentTypesQuery(), cancellationToken);
        await Task.WhenAll(citiesTask, employmentTypesTask);
        var cities = citiesTask.Result;
        var employmentTypes = employmentTypesTask.Result;

        filter.Cities = cities
            .Select(x => _mapper.MapFilterOption(x.Slug, x.Name))
            .ToList();

        filter.EmploymentTypes = employmentTypes
            .Select(x => _mapper.MapFilterOption(x, x))
            .ToList();

        int? cityId = null;
        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            var city = cities.FirstOrDefault(x =>
                string.Equals(x.Slug, filter.City, StringComparison.OrdinalIgnoreCase));
            cityId = city?.Id;
        }

        var vacancyFilter = new VacancyListingFilter
        {
            Search = filter.Search,
            CityId = cityId,
            EmploymentType = filter.EmploymentType,
            Page = filter.Page,
            PageSize = filter.PageSize
        };

        var vacanciesTask = _mediator.Send(new GetPublicVacanciesQuery(vacancyFilter), cancellationToken);
        var countTask = _mediator.Send(new CountPublicVacanciesQuery(vacancyFilter), cancellationToken);
        await Task.WhenAll(vacanciesTask, countTask);
        var vacancies = vacanciesTask.Result;
        var totalCount = countTask.Result;

        var vacancyCards = vacancies
            .Select(v => new VacancyCardVm
            {
                Title = v.Title,
                Description = v.Description,
                EmploymentType = v.EmploymentType,
                SalaryText = v.SalaryText,
                LocationText = v.LocationText,
                ListingTitle = v.ListingTitle,
                ListingUrl = _urlBuilder.BuildListingUrl(
                    culture, v.CitySlug, v.CategorySlug, v.SubCategorySlug, v.ListingSlug, v.ListingId),
                CityName = v.CityName,
                CategoryName = v.CategoryName,
                SubCategoryName = v.SubCategoryName
            })
            .ToList();

        var pagination = BuildPagination(culture, filter, totalCount);

        return new VacanciesPageVm
        {
            Culture = culture,
            Hero = new PageHeroVm
            {
                Title = "Job Vacancies",
                Description = "Browse open positions from service providers on the marketplace.",
                Breadcrumbs = BuildBreadcrumbs(culture)
            },
            SeoIntro = new SeoIntroVm
            {
                Title = "Job Vacancies",
                Text = "<p>Find employment opportunities posted by businesses and specialists. Use the filters to narrow results by city or employment type.</p>"
            },
            Filter = filter,
            Vacancies = vacancyCards,
            Pagination = pagination,
            TotalCount = totalCount
        };
    }

    private IReadOnlyCollection<BreadcrumbItemVm> BuildBreadcrumbs(string culture)
    {
        return new List<BreadcrumbItemVm>
        {
            new() { Title = "Home", Url = _urlBuilder.BuildHomeUrl(culture) },
            new() { Title = "Vacancies" }
        };
    }

    private PaginationVm BuildPagination(string culture, VacanciesFilterVm filter, int totalCount)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        if (totalPages <= 1)
            return new PaginationVm { CurrentPage = filter.Page, TotalPages = totalPages };

        var pages = BuildPageWindow(filter.Page, totalPages);

        var items = pages.Select(page =>
        {
            if (page is null)
                return new PaginationItemVm { IsEllipsis = true };

            return new PaginationItemVm
            {
                Page = page.Value,
                Url = AppendQuery(_urlBuilder.BuildVacanciesUrl(culture, page.Value), filter),
                IsActive = page.Value == filter.Page,
                IsEllipsis = false
            };
        }).ToList();

        return new PaginationVm
        {
            CurrentPage = filter.Page,
            TotalPages = totalPages,
            PrevUrl = filter.Page > 1
                ? AppendQuery(_urlBuilder.BuildVacanciesUrl(culture, filter.Page - 1), filter)
                : null,
            NextUrl = filter.Page < totalPages
                ? AppendQuery(_urlBuilder.BuildVacanciesUrl(culture, filter.Page + 1), filter)
                : null,
            Items = items
        };
    }

    private static IReadOnlyCollection<int?> BuildPageWindow(int currentPage, int totalPages)
    {
        var pages = new List<int?>();

        if (totalPages <= 7)
        {
            for (var i = 1; i <= totalPages; i++) pages.Add(i);
            return pages;
        }

        pages.Add(1);
        var start = Math.Max(2, currentPage - 1);
        var end = Math.Min(totalPages - 1, currentPage + 1);
        if (start > 2) pages.Add(null);
        for (var i = start; i <= end; i++) pages.Add(i);
        if (end < totalPages - 1) pages.Add(null);
        pages.Add(totalPages);
        return pages;
    }

    private static string AppendQuery(string basePath, VacanciesFilterVm filter)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(filter.Search))
            parts.Add($"search={Uri.EscapeDataString(filter.Search)}");
        if (!string.IsNullOrWhiteSpace(filter.City))
            parts.Add($"city={Uri.EscapeDataString(filter.City)}");
        if (!string.IsNullOrWhiteSpace(filter.EmploymentType))
            parts.Add($"employmentType={Uri.EscapeDataString(filter.EmploymentType)}");
        if (filter.PageSize > 0 && filter.PageSize != 12)
            parts.Add($"pageSize={filter.PageSize}");
        return parts.Count > 0 ? $"{basePath}?{string.Join("&", parts)}" : basePath;
    }
}
