using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;
using Marketplace.Modules.Listings.Application.Listings.Commands.CreateListing;
using Marketplace.Modules.Listings.Application.Listings.Commands.DeleteListing;
using Marketplace.Modules.Listings.Application.Listings.Commands.EditListing;
using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetById;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;
using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;
using MediatR;

namespace Marketplace.Web.Services.Listing;

public sealed class ListingCatalogService : IListingCatalogService
{
    private readonly IMediator mediator;
    
    public ListingCatalogService(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<IReadOnlyList<CategoryViewModel>> GetCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        var filter = new CategoryFilter
        {
        };

        var result = await mediator.Send(new GetCategoriesByFilterQuery(filter), cancellationToken);

        return (IReadOnlyList<CategoryViewModel>)Task.FromResult(result.Items.Select(x => new CategoryViewModel
        {
            Value = x.Id.ToString(),
            Label = x.Name
        }).ToList());
    }

    public Task<PagedResult<ListingViewModel>> GetListingsAsync(
        ListingsFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        var filter = new ListingFilter
        {
            IsActive = true,
            OrderBy = request.SortBy
        };

        var listings = mediator.Send(new GetListingsByFilterQuery(filter), cancellationToken).Result;
        
        return Task.FromResult(new PagedResult<ListingViewModel>
        {
            Items = listings.Items.Select(x => new ListingViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                // Category = x.Category.Name,
                Price = x.Price,
                // Currency = x.Currency,
                // City = x.Location?.City,
                // AddressLine = x.Location?.AddressLine,
                // Latitude = x.Location?.Latitude,
                // Longitude = x.Location?.Longitude,
                // Rating = x.Rating,
                // IsOnline = x.IsOnline,
                // IsOffline = x.IsOffline,
                // ImageUrl = x.ImageUrl,
                // ImageUrls = x.ImageUrls
            }).ToList(),
            Page = listings.Page,
            PageSize = listings.PageSize,
            TotalItems = listings.TotalCount,
            TotalPages = listings.TotalPages
        });
    }

    public async Task<ListingViewModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await mediator.Send(new GetListingByIdQuery(id), cancellationToken);
        return new ListingViewModel
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            // Category = item.Category.Name,
            Price = item.Price,
            // Currency = item.Currency,
            // City = item.Location?.City,
            // AddressLine = item.Location?.AddressLine,
            // Latitude = item.Location?.Latitude,
            // Longitude = item.Location?.Longitude,
            // Rating = item.Rating,
            // IsOnline = item.IsOnline,
            // IsOffline = item.IsOffline,
            // ImageUrl = item.ImageUrl,
            // ImageUrls = item.ImageUrls
        };
    }

    public async Task CreateAsync(ListingViewModel model, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new CreateListingCommand(
            model.Title,
            model.Description,
            model.Price,
            "test-seller-id",
            false
        ), cancellationToken);
    }

    public async Task UpdateAsync(ListingViewModel model, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new EditListingCommand(
            model.Title,
            model.Description,
            model.Price,
            "test-seller-id",
            false
        ), cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteListingCommand(id), cancellationToken);
    }
}