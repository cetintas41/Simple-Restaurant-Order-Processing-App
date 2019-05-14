using Entities.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IMenuTypeService
    {
        MenuType GetMenuTypeById(int menuTypeId, int branchId);
        Task UpdateMenuTypeAsync(MenuType menutype);
        Task CreateMenuTypeAsync(MenuType menutype);
        IList<MenuType> GetMenuTypes(int branchId);
        IList<MenuType> GetMenuTypesOfBranchFiltered(int branchId, string keyword, string sortBy, int currentPage, int topRecords);
        IList<MenuType> GetMenuTypesOfBranch(int branchId);
        int GetTotalMenuTypesCountFiltered(int branchId, string kw);
        Task DeleteMenuTypeAsync(int menuTypeId, int branchId);
    }
}
