using Entities;
using Entities.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBranchService
    {
        int GetBranchIdByTableCode(string tableCode);
        Task<int> CreateBranchAsync(Branch branch);
        Task ChangeBranchAccessAsync(int branchId);
        Task DeleteBranchAsync(int branchId, int firmId);
    }
}
