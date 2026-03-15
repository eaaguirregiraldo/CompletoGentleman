using BarberBook.DTOs;

namespace BarberBook.Interfaces;

/// <summary>
/// Service interface for Barber operations.
/// </summary>
public interface IBarberService
{
    /// <summary>
    /// Gets all active barbers.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of active barbers as DTOs.</returns>
    Task<IReadOnlyList<BarberDto>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets a barber by ID.
    /// </summary>
    /// <param name="id">The barber ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The barber as DTO if found, otherwise null.</returns>
    Task<BarberDto?> GetByIdAsync(int id, CancellationToken ct = default);
}
