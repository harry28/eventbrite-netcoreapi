using FluentValidation;

namespace eventbrite.Commands.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        public CreateEventCommandValidator()
        {
            RuleFor(request => request.@event.name.html).NotEmpty()
                .WithMessage("Name cannot be null or a default value for the type.");
            RuleFor(request => request.@event.start.timezone).NotEmpty()
                .WithMessage("Start Date cannot be null or a default value for the type.");
            RuleFor(request => request.@event.end.timezone).NotEmpty()
                .WithMessage("End Date cannot be null or a default value for the type.");
            RuleFor(request => request.@event.currency).NotEmpty()
                .WithMessage("Currency cannot be null or a default value for the type.");
        }
    }
}
