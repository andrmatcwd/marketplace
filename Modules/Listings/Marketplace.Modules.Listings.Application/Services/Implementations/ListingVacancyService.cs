using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Application.Vacancies.Commands;
using Marketplace.Modules.Listings.Application.Vacancies.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class ListingVacancyService : IListingVacancyService
{
    private readonly IListingVacancyRepository _repository;

    public ListingVacancyService(IListingVacancyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<VacancyDto>> GetByListingIdAsync(int listingId, CancellationToken cancellationToken)
    {
        var vacancies = await _repository.GetByListingIdAsync(listingId, cancellationToken);
        return vacancies.Select(ToDto).ToList();
    }

    public async Task<VacancyDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var vacancy = await _repository.GetByIdAsync(id, cancellationToken);
        return vacancy is null ? null : ToDto(vacancy);
    }

    public async Task AddAsync(CreateVacancyCommand command, CancellationToken cancellationToken)
    {
        var vacancy = new ListingVacancy
        {
            ListingId = command.ListingId,
            Title = command.Title,
            Description = command.Description,
            EmploymentType = command.EmploymentType,
            SalaryText = command.SalaryText,
            LocationText = command.LocationText
        };

        await _repository.AddAsync(vacancy, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task EditAsync(EditVacancyCommand command, CancellationToken cancellationToken)
    {
        var vacancy = await _repository.GetByIdAsync(command.Id, cancellationToken);
        if (vacancy is null)
            throw new Exception($"Vacancy with id {command.Id} not found.");

        vacancy.Title = command.Title;
        vacancy.Description = command.Description;
        vacancy.EmploymentType = command.EmploymentType;
        vacancy.SalaryText = command.SalaryText;
        vacancy.LocationText = command.LocationText;

        _repository.Update(vacancy);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var vacancy = await _repository.GetByIdAsync(id, cancellationToken);
        if (vacancy is null)
            throw new Exception($"Vacancy with id {id} not found.");

        _repository.Remove(vacancy);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static VacancyDto ToDto(ListingVacancy v) => new()
    {
        Id = v.Id,
        ListingId = v.ListingId,
        Title = v.Title,
        Description = v.Description,
        EmploymentType = v.EmploymentType,
        SalaryText = v.SalaryText,
        LocationText = v.LocationText
    };
}
