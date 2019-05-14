using Entities;
using Entities.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class MenuTypeService : IMenuTypeService
    {
        private readonly ApplicationDbContext _ctx;

        public MenuTypeService(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task CreateMenuTypeAsync(MenuType menutype)
        {
            await _ctx.MenuTypes.AddAsync(menutype);
            await _ctx.SaveChangesAsync();
        }

        public IList<MenuType> GetMenuTypesOfBranchFiltered(int branchId, string keyword, string sortBy, int currentPage, int topRecords)
        {
            keyword = keyword ?? ""; sortBy = sortBy ?? "";

            var total = _ctx.MenuTypes.Where(i => i.BranchId == branchId).Count();

            currentPage = topRecords == total ? 1 : currentPage;

            switch (sortBy.ToLower())
            {
                case "asc":
                    return _ctx.MenuTypes
                        .Where(i => i.Name.Contains(keyword) && i.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .OrderBy(i => i.Name)
                        .ToList();
                case "desc":
                    return _ctx.MenuTypes
                        .Where(i => i.Name.Contains(keyword) && i.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .OrderByDescending(i => i.Name)
                        .ToList();
                default:
                    return _ctx.MenuTypes
                        .Where(i => i.Name.Contains(keyword) && i.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .ToList();
            }
        }

        public IList<MenuType> GetMenuTypes(int branchId)
        {
            return _ctx.MenuTypes.Where(i => i.BranchId == branchId).ToList();
        }

        public async Task UpdateMenuTypeAsync(MenuType menutype)
        {
            _ctx.MenuTypes.Update(menutype);
            await _ctx.SaveChangesAsync();
        }

        public int GetTotalMenuTypesCountFiltered(int branchId, string kw)
        {
            kw = kw ?? "";
            return _ctx.MenuTypes.Where(i => i.BranchId == branchId && i.Name.Contains(kw)).Count();
        }

        public async Task DeleteMenuTypeAsync(int menuTypeId, int branchId)
        {
            var type = _ctx.MenuTypes.FirstOrDefault(i => i.Id == menuTypeId && i.BranchId == branchId);

            if (type == null)
                return;

            _ctx.MenuTypes.Remove(type);
            await _ctx.SaveChangesAsync();
        }

        public IList<MenuType> GetMenuTypesOfBranch(int branchId)
        {
            return _ctx.MenuTypes
                .Where(i => i.BranchId == branchId)
                .Include(i => i.Menus)
                .ToList();
        }

        public MenuType GetMenuTypeById(int menuTypeId, int branchId)
        {
            return _ctx.MenuTypes.First(i => i.Id == menuTypeId && i.BranchId == branchId);
        }
    }
}
