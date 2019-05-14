using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;

namespace Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser>  _userManager;
        private readonly ApplicationDbContext _ctx;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx)
        {
            _userManager = userManager;
            _ctx = ctx;
        }

        public async Task ChangeUserAccessAsync(string userId)
        {
            var user = _userManager.Users.First(i => i.Id == userId);

            if (user == null)
                return;

            user.IsActive = user.IsActive ? false : true;

            await _userManager.UpdateAsync(user);
        }

        public async Task CreateBranchUsersAsync(IFormFile file, int firmId)
        {
            try
            {
                using (var fs = file.OpenReadStream())
                {
                    var workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);

                    for (int row = 1; row < sheet.LastRowNum + 1; row++)
                    {
                        var name = sheet.GetRow(row).GetCell(0)?.ToString().Trim() ?? "";
                        var email = sheet.GetRow(row).GetCell(1)?.ToString().Trim() ?? "";
                        var phone = sheet.GetRow(row).GetCell(2)?.ToString().Trim() ?? "";
                        var password = sheet.GetRow(row).GetCell(3)?.ToString() ?? "";

                        var branch = new Branch
                        {
                            DateCreated = DateTime.Now,
                            IsActive = true,
                            FirmId = firmId,
                            Name = name
                        };

                        _ctx.Branches.Add(branch);

                        var user = new ApplicationUser()
                        {
                            ImagePath = null,
                            Email = email,
                            EmailConfirmed = true,
                            PhoneNumber = phone,
                            IsActive = true,
                            Name = name,
                            NormalizedEmail = email,
                            NormalizedUserName = email,
                            UserName = email,
                            DateCreated = DateTime.Now,
                            Branch = branch

                        };

                        await _userManager.CreateAsync(user, password);
                        await _userManager.AddToRoleAsync(user, "Branch");
                    }

                    _ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return;
            }
        }

        public async Task CreateUserAsync(ApplicationUser user, string password, string role)
        {
            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task CreateWaitersAsync(IFormFile file, int branchId)
        {
            try
            {
                using (var fs = file.OpenReadStream())
                {
                    var workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);

                    for (int row = 1; row < sheet.LastRowNum + 1; row++)
                    {

                        var name = sheet.GetRow(row).GetCell(0)?.ToString().Trim() ?? "";
                        var email = sheet.GetRow(row).GetCell(1)?.ToString().Trim() ?? "";
                        var phone = sheet.GetRow(row).GetCell(2)?.ToString().Trim() ?? "";
                        var password = sheet.GetRow(row).GetCell(3)?.ToString() ?? "";

                        var user = new ApplicationUser()
                        {
                            ImagePath = null,
                            Email = email,
                            EmailConfirmed = true,
                            PhoneNumber = phone,
                            IsActive = true,
                            Name = name,
                            NormalizedEmail = email,
                            NormalizedUserName = email,
                            UserName = email,
                            DateCreated = DateTime.Now,
                            BranchId = branchId

                        };

                        await _userManager.CreateAsync(user, password);
                        await _userManager.AddToRoleAsync(user, "Waiter");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                return;
            }
        }

        public async Task DeleteUserById(string userId)
        {
            var user = _userManager.Users.First(i => i.Id == userId);

            if (user == null)
                return;

            await _userManager.DeleteAsync(user);

        }

        public IList<ApplicationUser> GetBranchUsersFiltered(int firmId, string keyword, string sortBy, int currentPage, int topRecords)
        {
            keyword = keyword ?? ""; sortBy = sortBy ?? "";

            var branchIds = _ctx.Branches.Where(i => i.FirmId == firmId).Select(i => i.Id).ToList();

            var branches = _userManager.GetUsersInRoleAsync("Branch").Result
                .Where(i => branchIds.Contains(i.BranchId ?? 0))
                .ToList();

            int total = branches.Count();

            currentPage = topRecords == total ? 1 : currentPage;

            switch (sortBy.ToLower())
            {
                case "asc":
                    return branches
                        .Where(i => i.Name.Contains(keyword) && branchIds.Contains(i.BranchId??0))
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .OrderBy(i => i.Name)
                        .ToList();
                case "desc":
                    return branches
                          .Where(i => i.Name.Contains(keyword) && branchIds.Contains(i.BranchId??0))
                          .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                          .Take(Convert.ToInt32(topRecords))
                          .OrderByDescending(i => i.Name)
                          .ToList();
                default:
                    return branches
                        .Where(i => i.Name.Contains(keyword) && branchIds.Contains(i.BranchId??0))
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .ToList();
            }
        }

        public IList<ApplicationUser> GetFirmUsersFiltered(string keyword, string sortBy, int currentPage, int topRecords)
        {
            keyword = keyword ?? ""; sortBy = sortBy ?? "";

            var users = _userManager.GetUsersInRoleAsync("Firm").Result.ToList();

            int total = users.Count();

            currentPage = topRecords == total ? 1 : currentPage;

            switch (sortBy.ToLower())
            {
                case "asc":
                    return users
                        .Where(i => i.Name.Contains(keyword))
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .OrderBy(i => i.Name)
                        .ToList();
                case "desc":
                    return users
                        .Where(i => i.Name.Contains(keyword))
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .OrderByDescending(i => i.Name)
                        .ToList();
                default:
                    return users
                        .Where(i => i.Name.Contains(keyword))
                         .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                         .Take(Convert.ToInt32(topRecords))
                         .ToList();
            }
        }

        public int GetTotalBranchUserCountFiltered(int firmId, string kw)
        {
            kw = kw ?? "";

            var branchIds = _ctx.Branches.Where(i => i.FirmId == firmId).Select(i => i.Id).ToList();

            return _userManager.GetUsersInRoleAsync("Branch").Result.AsQueryable()
                   .Where(i => i.Name.Contains(kw) && branchIds.Contains(i.BranchId??0)).Count();
        }

        public int GetTotalFirmUserCountFiltered(string kw)
        {
            kw = kw ?? "";
            return _userManager.GetUsersInRoleAsync("Firm").Result.AsQueryable()
                   .Where(i => i.Name.Contains(kw)).Count();
        }

        public int GetTotalWaitersCountFiltered(int branchId, string kw)
        {
            kw = kw ?? "";
            return _userManager.GetUsersInRoleAsync("Waiter").Result.Where(i => i.BranchId == branchId && i.Name.Contains(kw)).Count();
        }

        public ApplicationUser GetUserByCredentials(string email, string password)
        {
            return _userManager.Users.Include(i => i.Branch).ThenInclude(i => i.Firm).First(i => i.Email == email);
        }

        public ApplicationUser GetUserById(string userId)
        {
            return _userManager.Users.First(i => i.Id == userId);
        }

        public ApplicationUserDto GetUserDtoById(string userId)
        {
            var user = _userManager.Users.Where(i => i.Id == userId)
               .Include(i => i.Branch)
               .Select(i => new ApplicationUserDto
               {
                   _Id = i.Id,
                   Id = i.Id.Encrypt(),
                   Name = i.Name,
                   ImagePath = i.ImagePath,
                   BranchId = i.BranchId ?? 0,
                   Email = i.Email,
                   IsActive = i.IsActive,
                   Phone = i.PhoneNumber,
                   FirmId = i.Branch == null ? 0 : i.Branch.FirmId,

               }).FirstOrDefault();
            return user;
        }

        public IList<string> GetUserRoles(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(i => i.Id == userId);

            return _userManager.GetRolesAsync(user).Result.ToList();
        }

        public IList<ApplicationUser> GetWaitersOfBranchFiltered(int branchId, string keyword, string sortBy, int currentPage, int topRecords)
        {
            keyword = keyword ?? ""; sortBy = sortBy ?? "";

            var waiters = _userManager.GetUsersInRoleAsync("Waiter").Result.Where(i => i.BranchId == branchId).ToList();

            var total = waiters.Count();

            currentPage = topRecords == total ? 1 : currentPage;


            switch (sortBy.ToLower())
            {
                case "asc":
                    return waiters.AsQueryable()
                        .Include(i => i.Branch)
                        .Where(i => i.Name.Contains(keyword) && i.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .OrderBy(i => i.Name)
                        .ToList();
                case "desc":
                    return waiters.AsQueryable()
                        .Include(i => i.Branch)
                        .Where(i => i.Name.Contains(keyword) && i.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .OrderByDescending(i => i.Name)
                        .ToList();
                default:
                    return waiters.AsQueryable()
                        .Include(i => i.Branch)
                        .Where(i => i.Name.Contains(keyword) && i.BranchId == branchId)
                        .Skip(Convert.ToInt32((currentPage - 1) * topRecords))
                        .Take(Convert.ToInt32(topRecords))
                        .ToList();
            }
        }
    }
}
