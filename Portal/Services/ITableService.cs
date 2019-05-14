using Entities.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITableService
    {
        Task CloseTableAsync(int tableId, string waiterId);
        Task CreateTableAsync(Table table);
        string GenerateTableCode(int len);
        IList<Table> GetTablesOfBranch(int branchId);
        Task ReleaseTableAsync(int tableId, int branchId);
        decimal GetTableTotal(int tableId);
        Table GetTableById(int tableId, int branchId);
        Table GetTableOfBranch(string tableCode, int branchId);
        int GetTotalTablesCountFiltered(int branchId, string kw);
        IList<Table> GetTablesOfBranchFiltered(int branchId, string keyword, string sortBy, int currentPage, int topRecords);
        Task UpdateTableAsync(Table table);
        Task DeleteTableAsync(int tableId, int branchId);
        IList<Table> GetWaitingTables(int branchId);
    }
}
