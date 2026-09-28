using FluentValidation;

namespace TripCraft.Application.Stops;

public class CreateStopValidator : AbstractValidator<CreateStopRequest>
{
    public CreateStopValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Address).MaximumLength(300);
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);      // physically valid coordinates
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
    }
}

public class UpdateStopValidator : AbstractValidator<UpdateStopRequest>
{
    public UpdateStopValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Address).MaximumLength(300);
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
    }
}