using Entities;
using Entities.Entity;
using Microsoft.EntityFrameworkCore;
using MlkPwgen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class TableService : ITableService
    {
        private readonly ApplicationDbContext _ctx;

        public TableService(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task CreateTableAsync(Table table)
        {
            await _ctx.Tables.AddAsync(table);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteTableAsync(int tableId, int branchId)
        {
            var table = _ctx.Tables.First(i => i.Id == tableId && i.BranchId == branchId);

            _ctx.Tables.Remove(table);
            await _ctx.SaveChangesAsync();

        }

        public string GenerateTableCode(int len)
        {
            return PasswordGenerator.Generate(length: len, allowed: Sets.Alphanumerics).ToUpper();
        }

        public Table GetTableOfBranch(string tableCode, int branchId)
        {
            return _ctx.Tables.First(i => i.Code == tableCode && i.BranchId == branchId);
        }

        public Table GetTableById(int tableId, int branchId)
        {
            return _ctx.Tables.FirstOrDefault(i => i.Id == tableId && i.BranchId == branchId);
        }

        public IList<Table> GetTablesOfBranch(int branchId)
        {
            return _ctx.Tables.Where(i => i.BranchId == branchId).ToList();
        }

        public IList<Table> GetTablesOfBranchFiltered(int branchId, string keyword, string sortBy, int currentPage, int topRecords)
        {
            keyword = keyword ?? ""; sortBy = sortBy ?? "";

            var total = _ctx.Tables.Where(i => i.BranchId == branchId).Count();

            currentPage = topRecords == total ? 1 : currentPage;

            switch (sortBy.ToLower())
            {
                case "asc":
                    return _ctx.Tables
                        .Where(i => i.Name.Contains(keyword) && i.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .OrderBy(i => i.Name)
                        .ToList();
                case "desc":
                    return _ctx.Tables
                        .Where(i => i.Name.Contains(keyword) && i.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .OrderByDescending(i => i.Name)
                        .ToList();
                default:
                    return _ctx.Tables
                        .Where(i => i.Name.Contains(keyword) && i.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .ToList();
            }
        }

        public decimal GetTableTotal(int tableId)
        {
            var tableCode = _ctx.Tables.FirstOrDefault(i => i.Id == tableId).Code;

            if (string.IsNullOrEmpty(tableCode))
                return 0;

            return _ctx.Orders
                .Include(i => i.Customer)
                .Include(i => i.Menu)
                .Where(i => i.Customer.TableCode == tableCode)
                .Sum(i => i.Menu.Price);
        }

        public int GetTotalTablesCountFiltered(int branchId, string kw)
        {
            kw = kw ?? "";
            return _ctx.Tables.Where(i => i.BranchId == branchId && i.Name.Contains(kw)).Count();
        }

        public async Task ReleaseTableAsync(int tableId, int branchId)
        {
            var table = _ctx.Tables.FirstOrDefault(i => i.Id == tableId && i.BranchId == branchId);

            if (table == null)
                return;

            table.IsClosed = false;
            table.Code = GenerateTableCode(6);

            _ctx.Tables.Update(table);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateTableAsync(Table table)
        {
            _ctx.Tables.Update(table);
            await _ctx.SaveChangesAsync();
        }

        public async Task CloseTableAsync(int tableId, string waiterId)
        {
            var table = _ctx.Tables.First(i => i.Id == tableId);

            if (table == null)
                return;

            table.IsClosed = true;
            _ctx.Tables.Update(table);

            var orders = _ctx.Orders
                .Include(i => i.Customer)
                .Where(i => i.Customer.TableCode == table.Code)
                .ToList();

            foreach (var order in orders)
            {
                order.WaiterId = waiterId;
            }

            _ctx.UpdateRange(orders);
            await _ctx.SaveChangesAsync();
              
        }

        public IList<Table> GetWaitingTables(int branchId)
        {
            var tableCodes = _ctx.Tables
                .Where(i => i.BranchId == branchId && !i.IsClosed)
                .Select(i => i.Code)
                .ToList();

            var codes = _ctx.Orders
                .Include(i => i.Customer)
                .Where(i => tableCodes.Contains(i.Customer.TableCode))
                .Select(i => i.Customer.TableCode)
                .ToList();

            return _ctx.Tables.Where(i => codes.Contains(i.Code) && !i.IsClosed && i.BranchId == branchId).ToList();

        }
    }
}
