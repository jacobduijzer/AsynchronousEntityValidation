using System.Threading.Tasks;

namespace Domain.Customers
{
    public interface ICustomerUniquenessChecker
    {
        Task<bool> IsUnique(Customer customer);
    }
}
