using Domain.Customers;
using Domain.Shared;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Customers
{
    public class CustomerShould
    {
        private readonly Mock<ICustomerUniquenessChecker> _mockCustomerUniqenessChecker;

        public CustomerShould()
        {
            _mockCustomerUniqenessChecker = new Mock<ICustomerUniquenessChecker>();
            _mockCustomerUniqenessChecker
                .Setup(x => x.IsUnique(It.Is<Customer>(y => y.Email == "alreadyexisting@somedomain.ext")))
                .ThrowsAsync(new BusinessRuleValidationException("Customer with this email already exists."));
            _mockCustomerUniqenessChecker
                .Setup(x => x.IsUnique(It.Is<Customer>(y => y.Email == "someemail@somedomain.ext")))
                .ReturnsAsync(true);
        }

        [Fact]
        public async Task CreateNewCustomerWhenUnique()
        {
            var result = await Customer.CreateNew("someemail@somedomain.ext", "Some Name", _mockCustomerUniqenessChecker.Object);
            result.Should().NotBeNull().And.BeOfType<Customer>();
            result.Email.Should().Be("someemail@somedomain.ext");
            result.Name.Should().Be("Some Name");
        }

        [Fact]
        public void ThrowWhenEmailAlreadyExisting()
        {
            Func<Task> task = async () =>
                await Customer.CreateNew(
                    "alreadyexisting@somedomain.ext",
                    "Already Existing",
                    _mockCustomerUniqenessChecker.Object);

            task.Should().Throw<BusinessRuleValidationException>().WithMessage("Customer with this email already exists.");
        }

        [Theory]
        [InlineData("alreadyexisting@somedomain.")]
        [InlineData("@somedomain.ext")]
        public void ThrowWhenEmailIsInvalid(string invalidEmail)
        {
            Func<Task> task = async () =>
                await Customer.CreateNew(
                    invalidEmail,
                    "Already Existing",
                    _mockCustomerUniqenessChecker.Object);

            task.Should().Throw<BusinessRuleValidationException>().WithMessage($"This emailaddress is not valid: {invalidEmail}");
        }

        [Theory]
        [InlineData("a")]
        [InlineData("")]
        public void ThrowWhenNameIsInvalid(string invalidName)
        {
            Func<Task> task = async () =>
                await Customer.CreateNew(
                    "alreadyexisting@somedomain.ext",
                    invalidName,
                    _mockCustomerUniqenessChecker.Object);

            task.Should().Throw<BusinessRuleValidationException>().WithMessage($"This name is not valid: {invalidName}");
        }
    }
}
