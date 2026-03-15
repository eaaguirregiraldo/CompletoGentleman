using Microsoft.EntityFrameworkCore;
using BarberBook.Data;
using BarberBook.Interfaces;
using BarberBook.Models;

namespace BarberBook.Repositories;

/// <summary>
/// Repository implementation for Barber entity operations.
/// </summary>
public class BarberRepository(AppDbContext context) : IBarberRepository
{
    private readonly AppDbContext _context = context;

    /// <inheritdoc />
    public async Task<IReadOnlyList<Barber>> GetAllActiveAsync(CancellationToken ct = default)
    {
        return await _context.Barbers
            .Where(b => b.Active)
            .OrderBy(b => b.Name)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<Barber?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Barbers
            .Include(b => b.Appointments)
            .FirstOrDefaultAsync(b => b.Id == id, ct);
    }
}
