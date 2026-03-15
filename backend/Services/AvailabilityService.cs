using BarberBook.DTOs;
using BarberBook.Interfaces;
using Microsoft.Extensions.Logging;

namespace BarberBook.Services;

/// <summary>
/// Service implementation for availability operations.
/// </summary>
public class AvailabilityService(
    IAppointmentRepository appointmentRepository,
    IBarberRepository barberRepository,
    ILogger<AvailabilityService> logger) : IAvailabilityService
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IBarberRepository _barberRepository = barberRepository;
    private readonly ILogger<AvailabilityService> _logger = logger;

    // Business hours configuration
    private static readonly TimeOnly StartTime = new(9, 0);  // 9:00 AM
    private static readonly TimeOnly EndTime = new(19, 0);   // 7:00 PM
    private static readonly int SlotDurationMinutes = 30;

    /// <inheritdoc />
    public async Task<AvailabilityResponse> GetAvailableSlotsAsync(int barberId, DateOnly date, CancellationToken ct = default)
    {
        _logger.LogInformation("Getting available slots for barber {BarberId} on {Date}", barberId, date);

        // Check if barber exists and is active
        var barber = await _barberRepository.GetByIdAsync(barberId, ct);
        if (barber == null || !barber.Active)
        {
            _logger.LogWarning("Barber {BarberId} not found or inactive", barberId);
            return new AvailabilityResponse
            {
                BarberId = barberId,
                Date = date,
                IsAvailable = false,
                AvailableSlots = new List<TimeOnly>()
            };
        }

        // Allow appointments on weekends (removed restriction for demo purposes)
        // If needed, business hours can be read from Settings table

        // Get existing appointments for the date
        var appointments = await _appointmentRepository.GetByBarberAndDateAsync(barberId, date, ct);
        
        // Filter out cancelled/no-show appointments
        var bookedSlots = appointments
            .Where(a => a.Status != Models.AppointmentStatus.NoShow)
            .Select(a => TimeOnly.FromDateTime(a.AppointmentDateTime))
            .ToHashSet();

        // Generate available slots
        var availableSlots = new List<TimeOnly>();
        var currentTime = StartTime;
        
        while (currentTime < EndTime)
        {
            if (!bookedSlots.Contains(currentTime))
            {
                availableSlots.Add(currentTime);
            }
            currentTime = currentTime.AddMinutes(SlotDurationMinutes);
        }

        _logger.LogInformation("Found {Count} available slots for barber {BarberId}", availableSlots.Count, barberId);

        return new AvailabilityResponse
        {
            BarberId = barberId,
            Date = date,
            IsAvailable = true,
            AvailableSlots = availableSlots
        };
    }
}
