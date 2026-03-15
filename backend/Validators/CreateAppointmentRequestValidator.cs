using BarberBook.DTOs;
using FluentValidation;

namespace BarberBook.Validators;

/// <summary>
/// FluentValidation validator for CreateAppointmentRequest.
/// </summary>
public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("Client name is required")
            .MinimumLength(2).WithMessage("Client name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Client name cannot exceed 100 characters");

        RuleFor(x => x.ClientPhone)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\+?[\d\s\-()]+$").WithMessage("Invalid phone number format")
            .MinimumLength(7).WithMessage("Phone number must be at least 7 digits")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters");

        RuleFor(x => x.ClientEmail)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Email cannot exceed 256 characters");

        RuleFor(x => x.BarberId)
            .GreaterThan(0).WithMessage("Barber ID must be a positive number");

        RuleFor(x => x.AppointmentDateTime)
            .NotEmpty().WithMessage("Appointment date and time is required")
            .Must(BeInTheFuture).WithMessage("Appointment must be scheduled for a future date and time")
            .Must(BeWithinBusinessHours).WithMessage("Appointment must be between 9:00 AM and 7:00 PM");
            // Removed weekend restriction for demo purposes
    }

    private static bool BeInTheFuture(DateTime dateTime)
    {
        return dateTime > DateTime.Now;
    }

    private static bool BeWithinBusinessHours(DateTime dateTime)
    {
        var time = TimeOnly.FromDateTime(dateTime);
        return time >= new TimeOnly(9, 0) && time < new TimeOnly(19, 0);
    }

    private static bool BeOnValidDay(DateTime dateTime)
    {
        var dayOfWeek = dateTime.DayOfWeek;
        return dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday;
    }
}
