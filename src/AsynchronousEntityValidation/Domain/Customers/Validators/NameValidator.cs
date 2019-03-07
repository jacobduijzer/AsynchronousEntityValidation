using FluentValidation;

namespace Domain.Customers.Validators
{
    public class NameValidator
        : AbstractValidator<string>
    {
        public NameValidator()
        {
            RuleFor<string>(name => name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2);
        }

        public static bool IsValid(string name) => new NameValidator().Validate(name).IsValid;
    }
}
