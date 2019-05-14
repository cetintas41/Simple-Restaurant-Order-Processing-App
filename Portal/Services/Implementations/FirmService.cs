using Entities;
using Entities.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class FirmService : IFirmService
    {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FirmService(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx, IHostingEnvironment hostingEnvironment)
        {
            _ctx = ctx;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task ChangeFirmAccessAsync(int firmId)
        {
            var firm = _ctx.Firms.First(i => i.Id == firmId);

            if (firm == null)
                return;

            firm.IsActive = firm.IsActive ? false : true;

            _ctx.Firms.Update(firm);
            await _ctx.SaveChangesAsync();
        }

        public async Task<int> CreateFirmAsync(Firm firm)
        {
            await _ctx.Firms.AddAsync(firm);
            await _ctx.SaveChangesAsync();

            return firm.Id;
        }

        public string GetOrCreateFirmFolder()
        {
            var firmFolderName = Guid.NewGuid().ToString();
            var path = Path.Combine(_hostingEnvironment.WebRootPath, string.Format("files\\firms\\{0}", firmFolderName));

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public async Task DeleteFirmAsync(int firmId)
        {
            var firm = _ctx.Firms.First(i => i.Id == firmId);

            if (firm == null)
                return;

            _ctx.Firms.Remove(firm);
            await _ctx.SaveChangesAsync();

        }

        public IList<Firm> GetActiveFirmsOfCity(int cityId)
        {
            return _ctx.Firms
                         .Where(i => i.IsActive)
                         .Include(i => i.Branches)
                         .Where(i => i.Branches.Any(x => x.IsActive && x.CityId == cityId))
                         .ToList();
        }

        public Firm GetFirmById(int firmId)
        {
            return _ctx.Firms.First(i => i.Id == firmId);
        }

        public string SaveFirmLogo(IFormFile file, string firmFolderFullpath)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(firmFolderFullpath, fileName);
            var fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);
            fs.Close();

            var firmFolder = firmFolderFullpath.Split('\\').Last();

            return string.Format("/files/firms/{0}/{1}", firmFolder, fileName);
        }

        public async Task UpdateFirmAsync(Firm firm)
        {
            _ctx.Firms.Update(firm);
            await _ctx.SaveChangesAsync();
        }

        public Firm GetFirmByFirmUserId(string userId)
        {
            return _ctx.Firms.Include(i => i.User).First(i => i.User.Id == userId);
        }
    }
}
