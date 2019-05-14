using Entities;
using Entities.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext _ctx;
        private readonly IHostingEnvironment _hostingEnvironment;

        public MenuService(ApplicationDbContext ctx, IHostingEnvironment hostingEnvironment)
        {
            _ctx = ctx;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task CreateMenuAsync(Menu menu)
        {
            await _ctx.Menus.AddAsync(menu);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteMenuAsync(int menuId, int branchId)
        {
            var menu = _ctx.Menus.Include(i => i.MenuType).FirstOrDefault(i => i.Id == menuId && i.MenuType.BranchId == branchId);

            if (menu == null)
                return;

            _ctx.Menus.Remove(menu);
            await _ctx.SaveChangesAsync();
        }

        public void DeleteMenuImage(int menuId)
        {
            var imagePath = _ctx.Menus.First(i => i.Id == menuId).ImagePath;

            imagePath = imagePath.Substring(1).Replace('/', '\\');

            var path = Path.Combine(_hostingEnvironment.WebRootPath, imagePath);

            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }

        }

        public Menu GetMenuById(int menuId, int branchId)
        {
            return _ctx.Menus.Include(i => i.MenuType).FirstOrDefault(i => i.MenuType.BranchId == branchId && i.Id == menuId);
        }

        public IList<Menu> GetMenusOfBranchFiltered(int branchId, string keyword, string sortBy, int currentPage, int topRecords)
        {
            keyword = keyword ?? ""; sortBy = sortBy ?? "";

            var total = _ctx.Menus.Include(i => i.MenuType).Where(i => i.MenuType.BranchId == branchId).Count();

            currentPage = topRecords == total ? 1 : currentPage;

            switch (sortBy.ToLower())
            {
                case "asc":
                    return _ctx.Menus
                        .Include(i => i.MenuType)
                        .Where(i => i.Name.Contains(keyword) && i.MenuType.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .OrderBy(i => i.Name)
                        .ToList();
                case "desc":
                    return _ctx.Menus
                         .Include(i => i.MenuType)
                         .Where(i => i.Name.Contains(keyword) && i.MenuType.BranchId == branchId)
                         .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                         .Take(Convert.ToInt32(topRecords))
                         .OrderByDescending(i => i.Name)
                         .ToList();
                default:
                    return _ctx.Menus
                        .Include(i => i.MenuType)
                        .Where(i => i.Name.Contains(keyword) && i.MenuType.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .ToList();
            }
        }

        public int GetTotalMenusCountFiltered(int branchId, string kw)
        {
            kw = kw ?? "";
            return _ctx.Menus.Include(i => i.MenuType).Where(i => i.MenuType.BranchId == branchId && i.Name.Contains(kw)).Count();
        }

        public string SaveMenuImage(string firmFolderName, IFormFile file)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, string.Format("files\\firms\\{0}\\menus", firmFolderName));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(path, fileName);

            var fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);
            fs.Close();

            return string.Format("/files/firms/{0}/menus/{1}", firmFolderName, fileName);
        }

        public async Task UpdateMenuAsync(Menu menu)
        {
            _ctx.Menus.Update(menu);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdatePricesByRateAsync(int rate, int branchId)
        {
            var menus = _ctx.Menus.Include(i => i.MenuType).Where(i => i.MenuType.BranchId == branchId).ToList();

            if (menus.Count == 0)
                return;

            if(rate > 0)
            {
                //zam
                foreach (var item in menus)
                {
                    item.Price = Math.Round(item.Price * (1 + ((decimal)Math.Abs(rate) / 100)), 2);
                }
            }
            else
            {
                //indirim
                foreach (var item in menus)
                {
                    item.Price = Math.Round(item.Price * (1 - (decimal)Math.Abs(rate) / 100), 2);
                }

            }

            _ctx.Menus.UpdateRange(menus);
            await _ctx.SaveChangesAsync();
        }
    }
}
