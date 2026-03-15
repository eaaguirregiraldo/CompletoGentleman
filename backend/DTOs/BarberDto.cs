namespace BarberBook.DTOs;

/// <summary>
/// Data Transfer Object for Barber entity.
/// </summary>
public class BarberDto
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
    /// Indicates whether the barber is currently active.
    /// </summary>
    public bool Active { get; set; }
}
