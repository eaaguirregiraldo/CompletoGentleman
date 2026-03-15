using Microsoft.EntityFrameworkCore;
using BarberBook.Models;

namespace BarberBook.Data;

/// <summary>
/// Database context for BarberBook application.
/// Manages the connection to SQLite and provides access to entity sets.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Set of barbers in the system.
    /// </summary>
    public DbSet<Barber> Barbers { get; set; } = null!;

    /// <summary>
    /// Set of appointments in the system.
    /// </summary>
    public DbSet<Appointment> Appointments { get; set; } = null!;

    /// <summary>
    /// Set of configuration settings.
    /// </summary>
    public DbSet<Setting> Settings { get; set; } = null!;

    /// <summary>
    /// Configures the model and seeds initial data.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Barber entity
        modelBuilder.Entity<Barber>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Active)
                .HasDefaultValue(true);
        });

        // Configure Appointment entity
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ClientName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.ClientPhone)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.ClientEmail)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.AppointmentDateTime)
                .IsRequired();
            entity.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Foreign key relationship
            entity.HasOne(e => e.Barber)
                .WithMany(b => b.Appointments)
                .HasForeignKey(e => e.BarberId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint: same barber, same datetime
            entity.HasIndex(e => new { e.BarberId, e.AppointmentDateTime })
                .IsUnique();
        });

        // Configure Setting entity
        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Key)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(50);

            // Unique key constraint
            entity.HasIndex(e => e.Key)
                .IsUnique();
        });

        // Seed data for Barbers
        modelBuilder.Entity<Barber>().HasData(
            new Barber { Id = 1, Name = "Juan Pérez", Active = true },
            new Barber { Id = 2, Name = "Carlos García", Active = true },
            new Barber { Id = 3, Name = "Pedro Martínez", Active = true }
        );

        // Seed data for Settings
        modelBuilder.Entity<Setting>().HasData(
            new Setting { Id = 1, Key = "WorkStart", Value = "09:00" },
            new Setting { Id = 2, Key = "WorkEnd", Value = "19:00" },
            new Setting { Id = 3, Key = "SlotDurationMinutes", Value = "30" }
        );
    }
}
