using Entities.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IMenuService
    {
        void DeleteMenuImage(int menuId);
        Task CreateMenuAsync(Menu menu);
        IList<Menu> GetMenusOfBranchFiltered(int branchId, string keyword, string sortBy, int currentPage, int topRecords);
        Task DeleteMenuAsync(int menuId, int branchId);
        Task UpdateMenuAsync(Menu menu);
        Menu GetMenuById(int menuId, int branchId);
        int GetTotalMenusCountFiltered(int branchId, string kw);
        Task UpdatePricesByRateAsync(int rate, int branchId);
        string SaveMenuImage(string firmFolderName, IFormFile file);
    }
}
