using BarberBook.Models;

namespace BarberBook.Interfaces;

/// <summary>
/// Repository interface for Appointment entity operations.
/// </summary>
public interface IAppointmentRepository
{
    /// <summary>
    /// Gets an appointment by ID asynchronously.
    /// </summary>
    /// <param name="id">The appointment ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The appointment if found, otherwise null.</returns>
    Task<Appointment?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Gets all appointments for a specific barber on a specific date.
    /// </summary>
    /// <param name="barberId">The barber ID.</param>
    /// <param name="date">The date to query.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of appointments.</returns>
    Task<IReadOnlyList<Appointment>> GetByBarberAndDateAsync(int barberId, DateOnly date, CancellationToken ct = default);

    /// <summary>
    /// Adds a new appointment asynchronously.
    /// </summary>
    /// <param name="appointment">The appointment to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created appointment with generated ID.</returns>
    Task<Appointment> AddAsync(Appointment appointment, CancellationToken ct = default);
}
