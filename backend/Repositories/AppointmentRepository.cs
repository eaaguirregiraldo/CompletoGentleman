using Microsoft.EntityFrameworkCore;
using BarberBook.Data;
using BarberBook.Interfaces;
using BarberBook.Models;

namespace BarberBook.Repositories;

/// <summary>
/// Repository implementation for Appointment entity operations.
/// </summary>
public class AppointmentRepository(AppDbContext context) : IAppointmentRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<Appointment?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Appointments
            .Include(a => a.Barber)
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Appointment>> GetByBarberAndDateAsync(
        int barberId,
        DateOnly date,
        CancellationToken ct = default)
    {
        var startOfDay = date.ToDateTime(TimeOnly.MinValue);
        var endOfDay = date.ToDateTime(TimeOnly.MaxValue);

        return await _context.Appointments
            .Where(a => a.BarberId == barberId
                && a.AppointmentDateTime >= startOfDay
                && a.AppointmentDateTime < endOfDay)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<Appointment> AddAsync(Appointment appointment, CancellationToken ct = default)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(ct);
        return appointment;
    }
}
