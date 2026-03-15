using BarberBook.DTOs;
using BarberBook.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BarberBook.Controllers;

/// <summary>
/// API controller for Appointment operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IValidator<CreateAppointmentRequest> _validator;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(
        IAppointmentService appointmentService,
        IValidator<CreateAppointmentRequest> validator,
        ILogger<AppointmentsController> logger)
    {
        _appointmentService = appointmentService;
        _validator = validator;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new appointment.
    /// </summary>
    /// <param name="request">The appointment creation request.</param>
    /// <returns>The created appointment.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AppointmentDto>> Create(
        [FromBody] CreateAppointmentRequest request, 
        CancellationToken ct)
    {
        _logger.LogInformation("Received appointment creation request for barber {BarberId}", request.BarberId);

        // Validate the request with FluentValidation
        var validationResult = await _validator.ValidateAsync(request, ct);
        
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed: {Errors}", 
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                
            return BadRequest(new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                title = "Validation Error",
                status = 400,
                errors = validationResult.Errors.ToDictionary(
                    e => e.PropertyName,
                    e => new[] { e.ErrorMessage })
            });
        }

        try
        {
            var appointment = await _appointmentService.CreateAsync(request, ct);
            _logger.LogInformation("Appointment created successfully with ID {AppointmentId}", appointment.Id);
            
            return CreatedAtAction(
                nameof(GetById), 
                new { id = appointment.Id }, 
                appointment);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Failed to create appointment: {Message}", ex.Message);
            
            return Conflict(new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                title = "Conflict",
                status = 409,
                detail = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets an appointment by ID.
    /// </summary>
    /// <param name="id">The appointment ID.</param>
    /// <returns>The appointment if found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> GetById(int id, CancellationToken ct)
    {
        _logger.LogInformation("Fetching appointment with ID {AppointmentId}", id);
        
        // This would require adding a GetById method to IAppointmentService
        // For now, returning NotFound as this is not in the Phase 2 scope
        return NotFound(new { message = $"Appointment with ID {id} not found" });
    }
}
