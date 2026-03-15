namespace BarberBook.Models;

/// <summary>
/// Represents a barber in the system.
/// </summary>
public class Barber
{
    /// <summary>
    /// Unique identifier for the barber.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the barber.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the barber is currently active and available for appointments.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Navigation property for appointments.
    /// </summary>
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
