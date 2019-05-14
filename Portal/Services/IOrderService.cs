using Entities.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IOrderService
    {
        Task SetOrdersToNextProgress(List<int> orders);
        IList<Order> GetOrdersOfWaiter(string waiterId, int branchId);
        IList<Order> GetOrdersOfCustomer(int customerId);
        Task CreateOrdersAsync(List<Order> orders);
        Task CancelOrdersAsync(List<int> orders);
        IList<Order> GetOrdersByTableCode(string tableCode);
    }
}
