using BarberBook.DTOs;

namespace BarberBook.Interfaces;

/// <summary>
/// Service interface for availability operations.
/// </summary>
public interface IAvailabilityService
{
    /// <summary>
    /// Gets available time slots for a barber on a specific date.
    /// </summary>
    /// <param name="barberId">The barber ID.</param>
    /// <param name="date">The date to check.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Availability response with available slots.</returns>
    Task<AvailabilityResponse> GetAvailableSlotsAsync(int barberId, DateOnly date, CancellationToken ct = default);
}
