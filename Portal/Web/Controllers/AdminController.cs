using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        #region initial
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFirmService _firmService;
        private readonly IBranchService _branchService;
        private readonly IUserService _userService;
        private readonly ICountryService _countryService;
        private readonly ILogger<AdminController> _logger;

        // Login olan kullanıcı ve kullanıcının rolleri
        private ApplicationUserDto _currentUser => _userService.GetUserDtoById(User.GetUserId());
        private List<string> _currentUserRoles => _userService.GetUserRoles(User.GetUserId()).ToList();

        public AdminController(
            ILogger<AdminController> logger,
            IUserService userService,
            ICountryService countryService,
            IBranchService branchService, 
            IFirmService firmService, 
            UserManager<ApplicationUser> userManager)
        {
            _firmService = firmService;
            _userManager = userManager;
            _branchService = branchService;
            _userManager = userManager;
            _countryService = countryService;
            _userService = userService;
            _logger = logger;
        }

        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("Firms", "Admin");
        }

        public IActionResult Firms(string s, string kw, int p = 1, int t = 25)
        {
            // Kayıtlı firmaların 'Firm' rolüne sahip kullanıcılarının toplam sayısı
            var firmUserCount = _userService.GetTotalFirmUserCountFiltered(kw);

            // Kayıtlı firmaların 'Firm' rolüne sahip kullanıcıları
            var firmUsers = _userService.GetFirmUsersFiltered(kw, s, p, t)
             .Select(i => new ApplicationUserDto
             {
                 Id = i.Id.Encrypt(),
                 Name = i.Name,
                 Email = i.Email,
                 IsActive = i.IsActive,
                 Phone = i.PhoneNumber

             }).ToList();

            var pagination = new PaginationModel()
            {
                Count = firmUserCount,
                Page = p,
                Top = t,
                SortBy = s,
                Keyword = kw
            };

            var model = new AdminFirmsViewModel()
            {
                FirmUsers = firmUsers,
                Pagination = pagination,
                CurrentUserRoles = _currentUserRoles,
                CurrentUser = _currentUser,
                CurrentFirmLogo = _currentUser.ImagePath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFirm(CreateFirmModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Geçersiz veri");

            // Sisteme kaydolacak her firma için bir klasör yaratılır/getirilir.
            var firmFolderFullpath = _firmService.GetOrCreateFirmFolder();

            // Firmanın logosu firmanın dizinine kaydedilir.
            var firmLogoPath = _firmService.SaveFirmLogo(model.Logo, firmFolderFullpath);
   
            // Firma için 'Firm' rolüne sahip bir yönetici kullanıcı oluşturulur.
            var user = new ApplicationUser
            {
                DateCreated = DateTime.Now,
                IsActive = true,
                Email = model.Email,
                EmailConfirmed = true,
                BranchId = null,
                Name = model.Name,
                ImagePath = null,
                NormalizedEmail = model.Email,
                NormalizedUserName = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true
            };

            await _userService.CreateUserAsync(user, model.Password, "Firm");

            // Firma nesnesi oluşturulur. 
            var firm = new Firm
            {
                DateCreated = DateTime.Now,
                IsActive = true,
                LogoPath = firmLogoPath,
                Name = model.Name,
                User = user
            };

            await _firmService.CreateFirmAsync(firm);

            _logger.LogInformation(string.Format("{0} Firması ve #{1} Firma Kullanıcısı oluşturuldu.", firm.Name, user.Id));

            return Ok("Firma oluşturuldu");
        }

       
        [HttpPost]
        public async Task<IActionResult> ChangeAccess(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Geçersiz veri");

            // 'Firm' rolündeki firma yöneticisinin sisteme erişim hakkı bloke edilebilir
            // veya mevcut blokaj kaldırılabilir.
            var id = userId.Decrypt();
            await _userService.ChangeUserAccessAsync(id);

            _logger.LogInformation(string.Format("#{0} Firma Kullanıcısının erişim hakkı güncellendi.", id));

            return Ok("Erişim hakkı güncellendi");

        }

        [HttpPost]
        public async Task<IActionResult> DeleteFirmUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Geçersiz veri");

            // 'Firm' rolündeki firma yöneticisini siler.
            // Firmaya bağlı diğer veriler silinmez.
            var userId = id.Decrypt();
            await _userService.DeleteUserById(userId);

            _logger.LogInformation(string.Format("#{0} Firma Kullanıcısı silindi.", userId));

            return Ok("Firma kullanıcısı silindi");

        }

        
    }
}