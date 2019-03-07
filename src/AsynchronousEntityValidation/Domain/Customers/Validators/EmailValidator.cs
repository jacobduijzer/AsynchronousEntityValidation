using FluentValidation;

namespace Domain.Customers.Validators
{
    public class EmailValidator
         : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor<string>(email => email).EmailAddress();
        }

        public static bool IsValid(string email) => new EmailValidator().Validate(email).IsValid;
    }
}
