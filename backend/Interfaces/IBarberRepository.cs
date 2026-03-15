using BarberBook.Models;

namespace BarberBook.Interfaces;

/// <summary>
/// Repository interface for Barber entity operations.
/// </summary>
public interface IBarberRepository
{
    /// <summary>
    /// Gets all active barbers asynchronously.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of active barbers.</returns>
    Task<IReadOnlyList<Barber>> GetAllActiveAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets a barber by ID asynchronously.
    /// </summary>
    /// <param name="id">The barber ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The barber if found, otherwise null.</returns>
    Task<Barber?> GetByIdAsync(int id, CancellationToken ct = default);
}
