using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.Implementations;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "Firm")]
    public class FirmController : Controller
    {
        #region initial
        private readonly IFirmService _firmService;
        private readonly IBranchService _branchService;
        private readonly ICountryService _countryService;
        private readonly IDocumentService _documentService;
        private readonly IUserService _userService;
        private readonly ILogger<FirmController> _logger;

        // Login olan kullanıcı ve kullanıcının rolleri
        private ApplicationUserDto _currentUser => _userService.GetUserDtoById(User.GetUserId());
        private List<string> _currentUserRoles => _userService.GetUserRoles(User.GetUserId()).ToList();
        private Firm _currentFirm => _firmService.GetFirmByFirmUserId(User.GetUserId());


        public FirmController
            (
             ICountryService countryService,
            IUserService userService, 
            IDocumentService documentService, 
            IBranchService branchService, 
            IFirmService firmService, 
            ILogger<FirmController> logger
            )
        {
            _firmService = firmService;
            _branchService = branchService;
            _documentService = documentService;
            _userService = userService;
            _logger = logger;
            _countryService = countryService;
        }
        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard", "Firm");
        }

        public IActionResult GetCountries(string lang = "tr")
        {
            // Sisteme tanımlı tüm ülkeler.
            var langId = _countryService.GetLanguageByCode(lang).Id;
            var countries = _countryService.GetCountries()
                .Select(i => new { ID = i.Id.ToString().Encrypt(), NAME = i.GetName(langId) }).ToList();

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(countries.OrderBy(i => i.NAME));
            return Ok(data);

        }

        public IActionResult GetCities(string countryId)
        {
            if (string.IsNullOrEmpty(countryId))
                return BadRequest("Geçersiz veri");

            // Seçilen ülkenin şehir/eyalet lerini getirir.
            var cities = _countryService.GetCitiesOfCountry(Convert.ToInt32(countryId.Decrypt()))
                .Select(i => new { ID = i.Id.ToString().Encrypt(), NAME = i.Name }).ToList();

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(cities.OrderBy(i => i.NAME));
            return Ok(data);

        }


        public IActionResult Branches(string s, string kw, int p = 1, int t = 25)
        {
            // Firmanın her bir şubesinin 'Branch' rolündeki kullanıcıların toplam sayısı
            var branchUsersCount = _userService.GetTotalBranchUserCountFiltered(_currentFirm.Id, kw);
           
            // Firmanın her bir şubesinin 'Branch' rolündeki kullanıcılar
            var branchUsers = _userService.GetBranchUsersFiltered(_currentFirm.Id, kw, s, p, t)
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
                Count = branchUsersCount,
                Page = p,
                Top = t,
                SortBy = s,
                Keyword = kw
            };

            var model = new FirmDashboardViewModel()
            {
                BranchUsers = branchUsers,
                Pagination = pagination,
                CurrentUserRoles = _currentUserRoles,
                CurrentUser = _currentUser,
                CurrentFirmLogo = _currentFirm.LogoPath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch(CreateBranchModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Geçersiz veri");

            // Yeni bir şube tanımlar.
            var branch = new Branch
            {
                DateCreated = DateTime.Now,
                FirmId = _currentFirm.Id,
                IsActive = true,
                Name = model.Name,
                CityId = Convert.ToInt32(model.City.Decrypt())
            };

            var branchId = await _branchService.CreateBranchAsync(branch);

            // Tanımlanan şubeye ait bir yönetici kullanıcı tanımlar.
            var user = new ApplicationUser
            {
                DateCreated = DateTime.Now,
                IsActive = true,
                Email = model.Email,
                EmailConfirmed = true,
                BranchId = branchId,
                Name = model.Name,
                NormalizedEmail = model.Email,
                NormalizedUserName = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true
            };

            await _userService.CreateUserAsync(user, model.Password, "Branch");

            _logger.LogInformation(string.Format("#{0} firması {1} şubesi ve {2} şube kullanıcısı oluşturdu.", _currentUser.Name, branch.Name, user.Name));

            return Ok("Şube kaydedildi");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBranchUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Geçersiz veri");

            // Şube kullanıcısını siler, şubenin kendisi silinmez.
            var userId = id.Decrypt();
            await _userService.DeleteUserById(userId);

            _logger.LogInformation(string.Format("{0} firması #{1} şube kullanıcısını sildi.", _currentUser.Name, userId));

            return Ok("Şube Kullanıcısı silindi");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAccess(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Geçersiz veri");

            // Şube kullanıcısının sisteme erişimini bloke eder veya blokajı kaldırır.
            var id = userId.Decrypt();
            await _userService.ChangeUserAccessAsync(id);

            _logger.LogInformation(string.Format("{0} firması #{1} şube kullanıcısının erişim hakkını güncelledi.", _currentUser.Name, id));

            return Ok("Erişim hakkı güncellendi");

        }

        public IActionResult Download(int id)
        {
            if (id == 0) return BadRequest("Geçersiz veri");

            if (id == (int)DocumentType.CreateBranchForm)
            {
                // Şube Oluşturma Excel formunu indirir.
                var headers = new string[]
                {
                   "Şube Adı",
                   "E-posta",
                   "Telefon",
                   "Şifre"
                };

                byte[] file = _documentService.DownloadCreateUserForm(headers);
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SubeOlusturmaFormu.xlsx");

            }

            return BadRequest("Geçersiz istek");
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranches(IFormFile file)
        {
            if (file == null)
                return BadRequest("Geçersiz dosya");

            // Excel dosyasından verileri ayıklar ve Şube kullanıcılarını oluşturur.
            await _userService.CreateBranchUsersAsync(file, _currentFirm.Id);

            _logger.LogInformation(string.Format("{0} firması şubeler oluşturdu.", _currentUser.Name));

            return Ok("Şubeler oluşturuldu");
        }
    }
}