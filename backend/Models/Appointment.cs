namespace BarberBook.Models;

/// <summary>
/// Represents the status of an appointment.
/// </summary>
public enum AppointmentStatus
{
    /// <summary>
    /// Appointment is pending confirmation.
    /// </summary>
    Pending,

    /// <summary>
    /// Appointment has been confirmed.
    /// </summary>
    Confirmed,

    /// <summary>
    /// Service has been completed.
    /// </summary>
    Completed,

    /// <summary>
    /// Client did not show up.
    /// </summary>
    NoShow
}

/// <summary>
/// Represents a scheduled appointment between a client and a barber.
/// </summary>
public class Appointment
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
    /// Foreign key to the barber.
    /// </summary>
    public int BarberId { get; set; }

    /// <summary>
    /// Navigation property to the barber.
    /// </summary>
    public Barber? Barber { get; set; }

    /// <summary>
    /// Date and time of the appointment.
    /// </summary>
    public DateTime AppointmentDateTime { get; set; }

    /// <summary>
    /// Current status of the appointment.
    /// </summary>
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
}
