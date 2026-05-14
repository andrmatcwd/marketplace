using Marketplace.Modules.Listings.Application.Rentals.Commands;
using Marketplace.Modules.Listings.Application.Rentals.Dtos;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class ListingRentalService : IListingRentalService
{
    private readonly IListingRentalRepository _repository;

    public ListingRentalService(IListingRentalRepository repository)
    {
        _repository = repository;
    }

    public async Task<RentalAdminDto?> GetByListingIdAsync(int listingId, CancellationToken cancellationToken)
    {
        var rental = await _repository.GetByListingIdAsync(listingId, cancellationToken);
        return rental is null ? null : ToDto(rental);
    }

    public async Task SaveAsync(SaveRentalCommand command, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByListingIdAsync(command.ListingId, cancellationToken);

        if (existing is not null)
        {
            existing.Price = command.Price;
            existing.Rooms = command.Rooms;
            existing.Area = command.Area;
            existing.Floor = command.Floor;
            existing.Features = command.Features;

            _repository.Update(existing);
        }
        else
        {
            var rental = new ListingRental
            {
                ListingId = command.ListingId,
                Price = command.Price,
                Rooms = command.Rooms,
                Area = command.Area,
                Floor = command.Floor,
                Features = command.Features
            };

            await _repository.AddAsync(rental, cancellationToken);
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByListingIdAsync(int listingId, CancellationToken cancellationToken)
    {
        var rental = await _repository.GetByListingIdAsync(listingId, cancellationToken);
        if (rental is null)
            throw new Exception($"Rental for listing id {listingId} not found.");

        _repository.Remove(rental);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<RentalRoomAdminDto?> GetRoomByIdAsync(int id, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(id, cancellationToken);
        return room is null ? null : ToRoomDto(room);
    }

    public async Task AddRoomAsync(CreateRentalRoomCommand command, CancellationToken cancellationToken)
    {
        var room = new ListingRentalRoom
        {
            RentalId = command.RentalId,
            Title = command.Title,
            Description = command.Description,
            Price = command.Price,
            Area = command.Area,
            Guests = command.Guests,
            Beds = command.Beds,
            Amenities = command.Amenities,
            ImageUrls = command.ImageUrls
        };

        await _repository.AddRoomAsync(room, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task EditRoomAsync(EditRentalRoomCommand command, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(command.Id, cancellationToken);
        if (room is null)
            throw new Exception($"Rental room with id {command.Id} not found.");

        room.Title = command.Title;
        room.Description = command.Description;
        room.Price = command.Price;
        room.Area = command.Area;
        room.Guests = command.Guests;
        room.Beds = command.Beds;
        room.Amenities = command.Amenities;
        room.ImageUrls = command.ImageUrls;

        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRoomAsync(int id, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(id, cancellationToken);
        if (room is null)
            throw new Exception($"Rental room with id {id} not found.");

        _repository.RemoveRoom(room);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetRoomImagesAsync(int roomId, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(roomId, cancellationToken);
        if (room is null) return Array.Empty<string>();
        return room.ImageUrls.AsReadOnly();
    }

    public async Task AddRoomImageAsync(int roomId, string url, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(roomId, cancellationToken);
        if (room is null) throw new Exception($"Room {roomId} not found.");
        room.ImageUrls.Add(url);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<string?> DeleteRoomImageAsync(int roomId, string url, CancellationToken cancellationToken)
    {
        var room = await _repository.GetRoomByIdAsync(roomId, cancellationToken);
        if (room is null) return null;
        if (!room.ImageUrls.Remove(url)) return null;
        await _repository.SaveChangesAsync(cancellationToken);
        return url;
    }

    private static RentalAdminDto ToDto(ListingRental r) => new()
    {
        Id = r.Id,
        ListingId = r.ListingId,
        Price = r.Price,
        Rooms = r.Rooms,
        Area = r.Area,
        Floor = r.Floor,
        Features = r.Features.AsReadOnly(),
        RoomOptions = r.RoomOptions.Select(ToRoomDto).ToList()
    };

    private static RentalRoomAdminDto ToRoomDto(ListingRentalRoom r) => new()
    {
        Id = r.Id,
        RentalId = r.RentalId,
        Title = r.Title,
        Description = r.Description,
        Price = r.Price,
        Area = r.Area,
        Guests = r.Guests,
        Beds = r.Beds,
        Amenities = r.Amenities.AsReadOnly(),
        ImageUrls = r.ImageUrls.AsReadOnly()
    };
}
