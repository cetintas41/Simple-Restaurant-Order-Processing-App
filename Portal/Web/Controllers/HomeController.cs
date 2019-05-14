using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUserService _userService;
        private readonly IFirmService _firmService;
        private readonly ILogger<HomeController> _logger;

        // Login olan kullanıcı ve kullanıcının rolleri
        private ApplicationUserDto _currentUser => _userService.GetUserDtoById(User.GetUserId());
        private ApplicationUser _currentUser_ => _userService.GetUserById(User.GetUserId());
        private List<string> _currentUserRoles => _userService.GetUserRoles(User.GetUserId()).ToList();


        public HomeController(ILogger<HomeController> logger, IUserService userService, IFirmService firmService, UserManager<ApplicationUser> userManager,IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _firmService = firmService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId());

            if (User.Identity.IsAuthenticated)
            {
                var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result;
                if (isAdmin)
                    return RedirectToAction("Firms", "Admin");

                var isFirm = _userManager.IsInRoleAsync(user, "Firm").Result;
                if (isFirm)
                    return RedirectToAction("Branches", "Firm");

                var isBranch = _userManager.IsInRoleAsync(user, "Branch").Result;
                if (isBranch)
                    return RedirectToAction("Dashboard", "Branch");

                return RedirectToAction("Logout", "Account");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Language(string culture, string returnUrl = null)
        {

            Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Settings()
        {
            var model = new SettingsViewModel
            {
                CurrentUserRoles = _currentUserRoles,
                CurrentUser = _currentUser,
                CurrentFirmLogo = _currentUser.ImagePath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeLogo(IFormFile file)
        {
            if (file == null)
                return BadRequest("Logo seçilmedi");

            var firmFolder = string.Empty; var path = string.Empty; var logo = string.Empty; var oldFilepath = string.Empty;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var firm = _firmService.GetFirmById(_currentUser.FirmId);

            if (firm == null)
                return BadRequest("Firma bulunamadı");

            var isAdmin = _userManager.IsInRoleAsync(_currentUser_, "Admin").Result;

            if (isAdmin)
            {
                firmFolder = "admin";
                path = Path.Combine(_hostingEnvironment.WebRootPath, "files", firmFolder);
                logo = string.Format("/files/admin/{0}", fileName);
                oldFilepath = Path.Combine(path, firm.LogoPath.Split('/')[3]);
            }

            var isFirm = _userManager.IsInRoleAsync(_currentUser_, "Firm").Result;

            if (isFirm)
            {
                firmFolder = _firmService.GetFirmById(firm.Id).LogoPath.Split('/')[3];
                path = Path.Combine(_hostingEnvironment.WebRootPath, "files", "firms", firmFolder);
                logo = string.Format("/files/firms/{0}/{1}", firmFolder, fileName);
                oldFilepath = Path.Combine(path, firm.LogoPath.Split('/')[4]);
            }

            // yeni dosyayı kaydet
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var filePath = Path.Combine(path, fileName);
            var fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);
            fs.Close();

            // firma logo path güncelle
            firm.LogoPath = logo;
            await _firmService.UpdateFirmAsync(firm);

            // eski dosyayı sil
            try
            {
                if (System.IO.File.Exists(oldFilepath))
                    System.IO.File.Delete(oldFilepath);
            }
            catch (Exception e)
            {
                _logger.LogError(string.Format("{0} firmasının eski logosu silinemedi. Hata: {1}", firm.Name, e.InnerException));
                return BadRequest("Eski logo silinemedi");
            }

            _logger.LogInformation(string.Format("{0} firması logosunu güncelledi.", firm.Name, DateTime.Now));

            return Ok("Logo güncellendi");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Geçersiz veri");

            var user = _userService.GetUserById(User.GetUserId());

            var isPasswordChanged = await _userManager.CheckPasswordAsync(user, model.Password);

            if (isPasswordChanged)
                await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

            if (user.Email != model.Email)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                await _userManager.ChangeEmailAsync(user, model.Email, token);
            }

            if(user.PhoneNumber != model.Phone)
            {
                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.Phone);
                await _userManager.ChangePhoneNumberAsync(user, model.Phone, token);
            }

            if (user.Name != model.Name)
            {
                user.Name = model.Name;
                await _userManager.UpdateAsync(user);
            }

            _logger.LogInformation(string.Format("{0} kullanıcısınnı bilgileri güncellendi.", user.Name));

            return Ok("Bilgiler güncellendi");
        }
    }
}
