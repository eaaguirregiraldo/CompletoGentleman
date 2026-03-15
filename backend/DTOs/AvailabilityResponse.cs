namespace BarberBook.DTOs;

/// <summary>
/// Response model for availability slots.
/// </summary>
public class AvailabilityResponse
{
    /// <summary>
    /// The barber ID.
    /// </summary>
    public int BarberId { get; set; }

    /// <summary>
    /// The date for which availability is being queried.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// List of available time slots.
    /// </summary>
    public List<TimeOnly> AvailableSlots { get; set; } = new();

    /// <summary>
    /// Indicates if the barber is available on this date.
    /// </summary>
    public bool IsAvailable { get; set; }
}
