using BarberBook.DTOs;
using BarberBook.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BarberBook.Controllers;

/// <summary>
/// API controller for Barber operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BarbersController : ControllerBase
{
    private readonly IBarberService _barberService;
    private readonly IAvailabilityService _availabilityService;
    private readonly ILogger<BarbersController> _logger;

    public BarbersController(
        IBarberService barberService,
        IAvailabilityService availabilityService,
        ILogger<BarbersController> logger)
    {
        _barberService = barberService;
        _availabilityService = availabilityService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all active barbers.
    /// </summary>
    /// <returns>List of active barbers.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BarberDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<BarberDto>>> GetAll(CancellationToken ct)
    {
        _logger.LogInformation("Fetching all active barbers");
        var barbers = await _barberService.GetAllAsync(ct);
        return Ok(barbers);
    }

    /// <summary>
    /// Gets a barber by ID.
    /// </summary>
    /// <param name="id">The barber ID.</param>
    /// <returns>The barber if found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BarberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BarberDto>> GetById(int id, CancellationToken ct)
    {
        _logger.LogInformation("Fetching barber with ID {BarberId}", id);
        var barber = await _barberService.GetByIdAsync(id, ct);
        
        if (barber == null)
        {
            _logger.LogWarning("Barber with ID {BarberId} not found", id);
            return NotFound(new { message = $"Barber with ID {id} not found" });
        }
        
        return Ok(barber);
    }

    /// <summary>
    /// Gets available time slots for a barber on a specific date.
    /// </summary>
    /// <param name="id">The barber ID.</param>
    /// <param name="date">The date to check (format: yyyy-MM-dd).</param>
    /// <returns>Available time slots.</returns>
    [HttpGet("{id}/availability")]
    [ProducesResponseType(typeof(AvailabilityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AvailabilityResponse>> GetAvailability(
        int id, 
        [FromQuery] string date, 
        CancellationToken ct)
    {
        _logger.LogInformation("Fetching availability for barber {BarberId} on {Date}", id, date);
        
        if (!DateOnly.TryParse(date, out var parsedDate))
        {
            _logger.LogWarning("Invalid date format: {Date}", date);
            return BadRequest(new { message = "Invalid date format. Use yyyy-MM-dd" });
        }

        if (parsedDate < DateOnly.FromDateTime(DateTime.Now))
        {
            _logger.LogWarning("Cannot check availability for past date: {Date}", date);
            return BadRequest(new { message = "Cannot check availability for past dates" });
        }

        var availability = await _availabilityService.GetAvailableSlotsAsync(id, parsedDate, ct);
        
        return Ok(availability);
    }
}
