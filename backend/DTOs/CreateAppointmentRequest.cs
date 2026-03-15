using System.ComponentModel.DataAnnotations;

namespace BarberBook.DTOs;

/// <summary>
/// Request model for creating a new appointment.
/// </summary>
public class CreateAppointmentRequest
{
    /// <summary>
    /// Client's full name.
    /// </summary>
    [Required(ErrorMessage = "Client name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Client name must be between 2 and 100 characters")]
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// Client's phone number.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string ClientPhone { get; set; } = string.Empty;

    /// <summary>
    /// Client's email address.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string ClientEmail { get; set; } = string.Empty;

    /// <summary>
    /// Barber ID for the appointment.
    /// </summary>
    [Required(ErrorMessage = "Barber ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Barber ID must be a positive number")]
    public int BarberId { get; set; }

    /// <summary>
    /// Requested date and time for the appointment.
    /// </summary>
    [Required(ErrorMessage = "Appointment date and time is required")]
    public DateTime AppointmentDateTime { get; set; }
}
