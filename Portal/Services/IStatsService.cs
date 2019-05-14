using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IStatsService
    {
        Dictionary<string, int> GetTableCountStatsOfWaiter(string waiterId, DateTime start, DateTime end);
        Dictionary<string, decimal> GetIncomeStatsOfWaiter(string waiterId, DateTime start, DateTime end);
        int GetTotalTableStatsOfBranch(int branchId, int type);
    }
}
