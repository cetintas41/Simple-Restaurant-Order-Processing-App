using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Implementations
{
    public enum StatsType
    {
        Daily, Monthly, Weekly
    }

    public class StatsService : IStatsService
    {
        private readonly ApplicationDbContext _ctx;

        public StatsService(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public int GetTotalTableStatsOfBranch(int branchId, int type)
        {
            var start = DateTime.Now;
            var end = DateTime.Now;

            switch (type)
            {
                case (int) StatsType.Daily:
                    start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    end = DateTime.Now;
                    break;
                case (int)StatsType.Weekly:
                    start = DateTime.Now.AddDays(DateTime.Now.DayOfWeek - DayOfWeek.Monday).Date;
                    end = start.AddDays(7).AddSeconds(-1);
                    break;
                case (int)StatsType.Monthly:
                    start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    end = start.AddMonths(1).AddDays(-1);
                    break;
                default:
                    return 0;
            }

            return _ctx.Orders
                .Include(i => i.Waiter)
                .Where(i => i.DateCreated >= start && i.DateCreated <= end && i.Waiter.BranchId == branchId)
                .Include(i => i.Customer)
                .Select(i => i.Customer.TableCode)
                .Distinct()
                .Count();
        }

        public Dictionary<string, decimal> GetIncomeStatsOfWaiter(string waiterId, DateTime start, DateTime end)
        {
            Extensions.CheckDates(ref start, ref end);
            Dictionary<string, decimal> list = new Dictionary<string, decimal>();

            var date_x_price = _ctx.Orders
                .Where(i => i.DateCreated >= start && i.DateCreated <= end && i.WaiterId == waiterId)
                .OrderBy(i => i.DateCreated)
                .Include(i => i.Menu)
                .Select(i => new { DATE = i.DateCreated.ToString("dd/MM/yyyy"), PRICE = i.Menu.Price });

            foreach (var item in date_x_price)
            {
                if (!list.Keys.Contains(item.DATE))
                {
                    var date = item.DATE;
                    var sum = date_x_price.Where(i => i.DATE == date).Select(i => i.PRICE).Sum();
                    list.Add(date, sum);
                }
            }


            return list;
        }

        public Dictionary<string, int> GetTableCountStatsOfWaiter(string waiterId, DateTime start, DateTime end)
        {
            Extensions.CheckDates(ref start, ref end);
            Dictionary<string, int> list = new Dictionary<string, int>();

            var date_x_code = _ctx.Customers
                 .Where(i => i.DateCreated >= start && i.DateCreated <= end)
                 .OrderBy(i => i.DateCreated)
                 .Include(i => i.Orders)
                 .Where(i => i.Orders.Any(l => l.WaiterId == waiterId))
                 .Select(i => new { DATE = i.DateCreated.ToString("dd/MM/yyyy"), CODE = i.TableCode });                 
               
            foreach (var item in date_x_code)
            {
                if (!list.Keys.Contains(item.DATE))
                {
                    var date = item.DATE;
                    var count = date_x_code.Where(i => i.DATE == date).Select(i => i.CODE).Distinct().Count();
                    list.Add(date, count);
                }
            }
            return list;
        }
    }
}
