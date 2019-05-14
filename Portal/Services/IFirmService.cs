using Entities;
using Entities.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IFirmService
    {
        Firm GetFirmByFirmUserId(string userId);
        Task<int> CreateFirmAsync(Firm firm);
        Task ChangeFirmAccessAsync(int firmId);
        Task DeleteFirmAsync(int firmId);
        Firm GetFirmById(int firmId);
        IList<Firm> GetActiveFirmsOfCity(int cityId);
        Task UpdateFirmAsync(Firm firm);
        string GetOrCreateFirmFolder();
        string SaveFirmLogo(IFormFile file, string firmFolderFullpath);
    }
}
