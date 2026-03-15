using BarberBook.Data;
using BarberBook.DTOs;
using BarberBook.Interfaces;
using BarberBook.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BarberBook.Services;

/// <summary>
/// Service implementation for Appointment operations.
/// </summary>
public class AppointmentService(
    AppDbContext dbContext,
    IAppointmentRepository appointmentRepository,
    IBarberRepository barberRepository,
    ILogger<AppointmentService> logger) : IAppointmentService
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IBarberRepository _barberRepository = barberRepository;
    private readonly ILogger<AppointmentService> _logger = logger;

    /// <inheritdoc />
    public async Task<AppointmentDto> CreateAsync(CreateAppointmentRequest request, CancellationToken ct = default)
    {
        _logger.LogInformation("Creating appointment for client {ClientName} with barber {BarberId} at {DateTime}",
            request.ClientName, request.BarberId, request.AppointmentDateTime);

        // Use transaction to ensure atomicity and prevent race conditions
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct);
        
        try
        {
            // Check if barber exists and is active (inside transaction)
            var barber = await _barberRepository.GetByIdAsync(request.BarberId, ct);
            if (barber == null || !barber.Active)
            {
                _logger.LogWarning("Failed to create appointment: Barber {BarberId} not found or inactive", request.BarberId);
                throw new InvalidOperationException($"Barber with ID {request.BarberId} not found or is inactive");
            }

            // Check for conflicting appointments (inside transaction)
            var date = DateOnly.FromDateTime(request.AppointmentDateTime);
            var existingAppointments = await _appointmentRepository.GetByBarberAndDateAsync(request.BarberId, date, ct);
            
            var requestedTime = TimeOnly.FromDateTime(request.AppointmentDateTime);
            var hasConflict = existingAppointments.Any(a => 
                TimeOnly.FromDateTime(a.AppointmentDateTime) == requestedTime &&
                a.Status != AppointmentStatus.NoShow);

            if (hasConflict)
            {
                _logger.LogWarning("Failed to create appointment: Time slot already booked for barber {BarberId} at {Time}",
                    request.BarberId, requestedTime);
                throw new InvalidOperationException($"The requested time slot is already booked");
            }

            // Create the appointment
            var appointment = AppointmentMapper.ToEntity(request);
            appointment.Barber = barber;
            
            var created = await _appointmentRepository.AddAsync(appointment, ct);
            
            await transaction.CommitAsync(ct);
            
            _logger.LogInformation("Appointment created successfully with ID {AppointmentId}", created.Id);
            
            return AppointmentMapper.ToDto(created);
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 2067) // UNIQUE constraint failed
        {
            await transaction.RollbackAsync(ct);
            _logger.LogWarning("Failed to create appointment: Concurrent conflict detected for barber {BarberId} at {DateTime}",
                request.BarberId, request.AppointmentDateTime);
            throw new InvalidOperationException($"The requested time slot is already booked (concurrent request)");
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ValidateAsync(CreateAppointmentRequest request, CancellationToken ct = default)
    {
        // Check if barber exists and is active
        var barber = await _barberRepository.GetByIdAsync(request.BarberId, ct);
        if (barber == null || !barber.Active)
        {
            return false;
        }

        // Check if the appointment date is in the future
        if (request.AppointmentDateTime <= DateTime.Now)
        {
            return false;
        }

        // Check for conflicting appointments
        var date = DateOnly.FromDateTime(request.AppointmentDateTime);
        var existingAppointments = await _appointmentRepository.GetByBarberAndDateAsync(request.BarberId, date, ct);
        
        var requestedTime = TimeOnly.FromDateTime(request.AppointmentDateTime);
        var hasConflict = existingAppointments.Any(a => 
            TimeOnly.FromDateTime(a.AppointmentDateTime) == requestedTime &&
            a.Status != AppointmentStatus.NoShow);

        return !hasConflict;
    }
}
