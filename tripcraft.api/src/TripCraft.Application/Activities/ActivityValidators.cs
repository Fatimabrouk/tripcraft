using FluentValidation;

namespace TripCraft.Application.Activities;

public class CreateActivityValidator : AbstractValidator<CreateActivityRequest>
{
    public CreateActivityValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Notes).MaximumLength(1000);
        RuleFor(x => x.Day).GreaterThanOrEqualTo(1);
        RuleFor(x => x.TimeSlot)
            .Must(v => TimeOnly.TryParse(v, out _))
            .When(x => !string.IsNullOrWhiteSpace(x.TimeSlot))
            .WithMessage("Time slot must be a valid time such as 09:30.");
    }
}

public class UpdateActivityValidator : AbstractValidator<UpdateActivityRequest>
{
    public UpdateActivityValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Notes).MaximumLength(1000);
        RuleFor(x => x.Day).GreaterThanOrEqualTo(1);
        RuleFor(x => x.TimeSlot)
            .Must(v => TimeOnly.TryParse(v, out _))
            .When(x => !string.IsNullOrWhiteSpace(x.TimeSlot))
            .WithMessage("Time slot must be a valid time such as 09:30.");
    }
}