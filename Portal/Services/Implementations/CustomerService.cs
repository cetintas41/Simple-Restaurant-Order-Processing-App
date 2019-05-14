using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Entity;
using Microsoft.EntityFrameworkCore;

namespace Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _ctx;

        public CustomerService(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<int> GetOrCreateCustomerAsync(string customerName, string tableCode)
        {
            var customer = _ctx.Customers.First(i => i.Name == customerName && i.TableCode == tableCode);

            if(customer == null)
            {
                // yeni bir müşteri
                customer = new Customer
                {
                    Name = customerName,
                    TableCode = tableCode,
                    DateCreated = DateTime.Now
                };

                await _ctx.Customers.AddAsync(customer);
                await _ctx.SaveChangesAsync();
            }

            return customer.Id;
        }

        public Customer GetCustomerById(int customerId)
        {
            return _ctx.Customers.First(i => i.Id == customerId);
        }

        public IList<Customer> GetCustomersByTableCode(string tableCode)
        {
            return _ctx.Customers
                .Where(i => i.TableCode == tableCode)
                .Include(i => i.Orders)
                .ThenInclude(l => l.Menu)
                .ToList();


        }
    }
}
