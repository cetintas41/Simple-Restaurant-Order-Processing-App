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
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _ctx;

        public OrderService(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task CreateOrdersAsync(List<Order> orders)
        {
            await _ctx.Orders.AddRangeAsync(orders);
            await _ctx.SaveChangesAsync();
        }

        public async Task CancelOrdersAsync(List<int> orders)
        {
            var list = _ctx.Orders.Where(i => orders.Contains(i.Id) && i.Status == (int) OrderStatus.Waiting);

            if (list == null)
                return;

            _ctx.Orders.RemoveRange(list);
            await _ctx.SaveChangesAsync();
        }

        public IList<Order> GetOrdersByTableCode(string tableCode)
        {
            return _ctx.Orders
                 .Where(i => i.Customer.TableCode == tableCode)
                 .Include(i => i.Customer)
                 .Include(i => i.Menu)
                 .ThenInclude(i => i.MenuType)
                 .ToList();
        }

        public IList<Order> GetOrdersOfCustomer(int customerId)
        {
            return _ctx.Orders
                .Include(i => i.Menu)
                .Include(i => i.Customer)
                .Where(i => i.CustomerId == customerId)
                .ToList();
        }

        public IList<Order> GetOrdersOfWaiter(string waiterId, int branchId)
        {
            var codes = _ctx.Tables
                .Where(i => i.BranchId == branchId)
                .Select(i => i.Code)
                .ToList();

            return _ctx.Orders
                .Include(i => i.Customer)
                .Where(i => i.WaiterId == waiterId && codes.Contains(i.Customer.TableCode))
                .Include(i => i.Menu)
                .ToList();
        }

        public async Task SetOrdersToNextProgress(List<int> orders)
        {
            var list = _ctx.Orders.Where(i => orders.Contains(i.Id) && i.Status != (int) OrderStatus.Done);

            foreach (var order in list)
            {
                order.Status = order.Status == (int)OrderStatus.Waiting ? (int)OrderStatus.Preparing : (int)OrderStatus.Done;
            }

            _ctx.UpdateRange(list);
            await _ctx.SaveChangesAsync();
        }
    }
}
