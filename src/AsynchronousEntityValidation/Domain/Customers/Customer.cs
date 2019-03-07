using Domain.Customers.Validators;
using Domain.Shared;
using System.Threading.Tasks;

namespace Domain.Customers
{
    public class Customer
    {
        private Customer(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; private set; }
        public string Name { get; private set; }

        public static async Task<Customer> CreateNew(
            string email,
            string name,
            ICustomerUniquenessChecker customerUniquenessChecker)
        {
            if (!EmailValidator.IsValid(email))
                throw new BusinessRuleValidationException($"This emailaddress is not valid: {email}");

            if (!NameValidator.IsValid(name))
                throw new BusinessRuleValidationException($"This name is not valid: {name}");

            var customer = new Customer(email, name);
            var isUnique = await customerUniquenessChecker.IsUnique(customer).ConfigureAwait(false);
            if (!isUnique)
                throw new BusinessRuleValidationException("Customer with this email already exists.");

            return customer;
        }
    }
}
