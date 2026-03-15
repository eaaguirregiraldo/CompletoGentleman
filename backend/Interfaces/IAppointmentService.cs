using BarberBook.DTOs;

namespace BarberBook.Interfaces;

/// <summary>
/// Service interface for Appointment operations.
/// </summary>
public interface IAppointmentService
{
    /// <summary>
    /// Creates a new appointment.
    /// </summary>
    /// <param name="request">The appointment creation request.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created appointment as DTO.</returns>
    Task<AppointmentDto> CreateAsync(CreateAppointmentRequest request, CancellationToken ct = default);

    /// <summary>
    /// Validates if an appointment can be created.
    /// </summary>
    /// <param name="request">The appointment creation request.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>True if valid, otherwise false.</returns>
    Task<bool> ValidateAsync(CreateAppointmentRequest request, CancellationToken ct = default);
}
