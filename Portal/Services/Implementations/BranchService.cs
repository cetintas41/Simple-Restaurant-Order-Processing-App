using Entities;
using Entities.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class BranchService : IBranchService
    {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;

        public BranchService(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        public async Task ChangeBranchAccessAsync(int branchId)
        {
            var branch = _ctx.Branches.First(i => i.Id == branchId);

            if (branch == null)
                return;

            branch.IsActive = branch.IsActive ? false : true;

            _ctx.Branches.Update(branch);

            await _ctx.SaveChangesAsync();
        }

        public async Task<int> CreateBranchAsync(Branch branch)
        {
            try
            {
                await _ctx.Branches.AddAsync(branch);
                await _ctx.SaveChangesAsync();

                return branch.Id;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task DeleteBranchAsync(int branchId, int firmId)
        {
            var branch = _ctx.Branches.First(i => i.Id == branchId && i.FirmId == firmId);

            if (branch == null)
                return;

            _ctx.Branches.Remove(branch);

            await _ctx.SaveChangesAsync();
        }

        public int GetBranchIdByTableCode(string tableCode)
        {
            return _ctx.Tables.First(i => i.Code == tableCode).BranchId;
        }
    }
}
