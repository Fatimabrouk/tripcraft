using FluentValidation;

namespace TripCraft.Application.Trips;

public class CreateTripValidator : AbstractValidator<CreateTripRequest>
{
    public CreateTripValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be on or after the start date.");
    }
}

public class UpdateTripValidator : AbstractValidator<UpdateTripRequest>
{
    public UpdateTripValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be on or after the start date.");
    }
}