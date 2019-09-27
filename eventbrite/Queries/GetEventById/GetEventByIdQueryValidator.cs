using FluentValidation;

namespace eventbrite.Queries.GetEventById
{
    public class GetEventByIdQueryValidator:AbstractValidator<GetEventByIdQuery>
    {
        public GetEventByIdQueryValidator()
        {
            RuleFor(request => request.EventId).NotEmpty()
                .WithMessage("Event Id cannot be null or a default value for the type.");
        }
    }
}
