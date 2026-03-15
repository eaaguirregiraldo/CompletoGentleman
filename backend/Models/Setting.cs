namespace BarberBook.Models;

/// <summary>
/// Represents a configuration setting stored in the database.
/// </summary>
public class Setting
{
    /// <summary>
    /// Unique identifier for the setting.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Key that identifies the setting (e.g., "WorkStart", "WorkEnd").
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Value of the setting (e.g., "09:00", "19:00").
    /// </summary>
    public string Value { get; set; } = string.Empty;
}
