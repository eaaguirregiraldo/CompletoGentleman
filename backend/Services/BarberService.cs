using BarberBook.DTOs;
using BarberBook.Interfaces;

namespace BarberBook.Services;

/// <summary>
/// Service implementation for Barber operations.
/// </summary>
public class BarberService(IBarberRepository barberRepository) : IBarberService
{
    private readonly IBarberRepository _barberRepository = barberRepository;

    /// <inheritdoc />
    public async Task<IReadOnlyList<BarberDto>> GetAllAsync(CancellationToken ct = default)
    {
        var barbers = await _barberRepository.GetAllActiveAsync(ct);
        return barbers.Select(AppointmentMapper.ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<BarberDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var barber = await _barberRepository.GetByIdAsync(id, ct);
        return barber == null ? null : AppointmentMapper.ToDto(barber);
    }
}
