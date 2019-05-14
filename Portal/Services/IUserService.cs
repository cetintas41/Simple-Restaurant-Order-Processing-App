using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        ApplicationUser GetUserById(string userId);
        ApplicationUser GetUserByCredentials(string email, string password);
        ApplicationUserDto GetUserDtoById(string userId);
        IList<string> GetUserRoles(string userId);
        int GetTotalFirmUserCountFiltered(string kw);
        int GetTotalBranchUserCountFiltered(int firmId, string kw);
        IList<ApplicationUser> GetFirmUsersFiltered(string keyword, string sortBy, int currentPage, int topRecords);
        IList<ApplicationUser> GetBranchUsersFiltered(int firmId, string keyword, string sortBy, int currentPage, int topRecords);
        IList<ApplicationUser> GetWaitersOfBranchFiltered(int branchId, string keyword, string sortBy, int currentPage, int topRecords);
        Task CreateUserAsync(ApplicationUser user, string password, string role);
        Task ChangeUserAccessAsync(string userId);
        Task DeleteUserById(string userId);
        Task CreateBranchUsersAsync(IFormFile file, int firmId);
        Task CreateWaitersAsync(IFormFile file, int branchId);
        int GetTotalWaitersCountFiltered(int branchId, string kw);
    }
}
