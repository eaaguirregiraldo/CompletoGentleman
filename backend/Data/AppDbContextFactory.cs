using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BarberBook.Data;

namespace BarberBook.Data;

/// <summary>
/// Factory for creating AppDbContext instances at design time (for migrations).
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Creates a new instance of AppDbContext for design-time operations.
    /// </summary>
    /// <param name="args">Arguments provided by the design-time host.</param>
    /// <returns>A new AppDbContext instance.</returns>
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Default connection string for design-time
        optionsBuilder.UseSqlite("Data Source=barberbook.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}
