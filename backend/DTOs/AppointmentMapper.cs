using BarberBook.Models;

namespace BarberBook.DTOs;

/// <summary>
/// Static mapper class for converting between entities and DTOs.
/// </summary>
public static class AppointmentMapper
{
    /// <summary>
    /// Converts an Appointment entity to AppointmentDto.
    /// </summary>
    public static AppointmentDto ToDto(Appointment appointment)
    {
        return new AppointmentDto
        {
            Id = appointment.Id,
            ClientName = appointment.ClientName,
            ClientPhone = appointment.ClientPhone,
            ClientEmail = appointment.ClientEmail,
            BarberId = appointment.BarberId,
            BarberName = appointment.Barber?.Name,
            AppointmentDateTime = appointment.AppointmentDateTime,
            Status = appointment.Status.ToString()
        };
    }

    /// <summary>
    /// Converts a Barber entity to BarberDto.
    /// </summary>
    public static BarberDto ToDto(Barber barber)
    {
        return new BarberDto
        {
            Id = barber.Id,
            Name = barber.Name,
            Active = barber.Active
        };
    }

    /// <summary>
    /// Converts a CreateAppointmentRequest to an Appointment entity.
    /// </summary>
    public static Appointment ToEntity(CreateAppointmentRequest request)
    {
        return new Appointment
        {
            ClientName = request.ClientName,
            ClientPhone = request.ClientPhone,
            ClientEmail = request.ClientEmail,
            BarberId = request.BarberId,
            AppointmentDateTime = request.AppointmentDateTime,
            Status = AppointmentStatus.Pending
        };
    }
}
