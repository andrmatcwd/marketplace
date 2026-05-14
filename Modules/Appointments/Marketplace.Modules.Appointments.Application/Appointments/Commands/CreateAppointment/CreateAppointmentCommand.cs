using MediatR;

namespace Marketplace.Modules.Appointments.Application.Appointments.Commands.CreateAppointment;

public sealed record CreateAppointmentCommand(
    string  BusinessName,
    string  ContactName,
    string  Phone,
    string  Email,
    string  CategoryName,
    string  CityName,
    string? Address,
    string? Website,
    string? Description
) : IRequest<int>;
