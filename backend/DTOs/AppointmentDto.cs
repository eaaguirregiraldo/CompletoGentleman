namespace BarberBook.DTOs;

/// <summary>
/// Data Transfer Object for Appointment entity.
/// </summary>
public class AppointmentDto
{
    /// <summary>
    /// Unique identifier for the appointment.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Client's full name.
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// Client's phone number.
    /// </summary>
    public string ClientPhone { get; set; } = string.Empty;

    /// <summary>
    /// Client's email address.
    /// </summary>
    public string ClientEmail { get; set; } = string.Empty;

    /// <summary>
    /// Barber ID.
    /// </summary>
    public int BarberId { get; set; }

    /// <summary>
    /// Barber name (for display purposes).
    /// </summary>
    public string? BarberName { get; set; }

    /// <summary>
    /// Date and time of the appointment.
    /// </summary>
    public DateTime AppointmentDateTime { get; set; }

    /// <summary>
    /// Current status of the appointment.
    /// </summary>
    public string Status { get; set; } = "Pending";
}
