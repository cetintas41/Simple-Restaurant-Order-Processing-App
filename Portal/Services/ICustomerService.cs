using Entities.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICustomerService
    {
        IList<Customer> GetCustomersByTableCode(string tableCode);
        Task<int> GetOrCreateCustomerAsync(string customerName, string tableCode);
        Customer GetCustomerById(int customerId);
    }
}
