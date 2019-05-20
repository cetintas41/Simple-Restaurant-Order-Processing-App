using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Web.Models;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        #region initial
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<AccountController> _logger;

        public AccountController
            (
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IHostingEnvironment hostingEnvironment,
            ILogger<AccountController> logger
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        #endregion

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> AccessDenied()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("{0} kullanıcısı {1} şifresi ile sisteme erişemedi. Tarih: {2}", model.Email, model.Password, DateTime.Now));

                ModelState.AddModelError("LoginError", "Geçersiz E-posta ve/veya Şifre");
                return View();
            }

            var user = _userManager.Users
                .Include(i => i.Branch)
                .ThenInclude(i => i.Firm)
                .FirstOrDefault(i => i.IsActive && i.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("LoginError", "Kullanıcı bulunamadı");
                return View();
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("LoginError", "Hesabınız bloke edilmiş");
                return View();
            }

            var isActive = user.Branch == null ? true : user.Branch.IsActive;
            if (!isActive)
            {
                ModelState.AddModelError("LoginError", "Şubeniz bloke edilmiş");
                return View();
            }

            var isActive2 = user.Branch == null ? true : user.Branch.Firm.IsActive;
            if (!isActive2)
            {
                ModelState.AddModelError("LoginError", "Firmanız bloke edilmiş");
                return View();
            }

            var isWaiter = _userManager.IsInRoleAsync(user, "Waiter").Result;

            if (isWaiter)
            {
                ModelState.AddModelError("LoginError", "Web üzerinden giriş izniniz yok");
                return View();
            }

            var result = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;

            if (result.Succeeded)
            {
                _logger.LogInformation(string.Format("{0} kullanıcısı {1} şifresi ile sisteme giriş yaptı. Tarih: {2}", model.Email, model.Password, DateTime.Now));

                // Giriş başarılı...
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email)
                };

                await _userManager.AddClaimsAsync(user, claims);

                Log.Information(string.Format("{0} user logged on {1}", user.Email, DateTime.Now));
                return RedirectToLocal(user, returnUrl);
            }

            if (result.IsLockedOut)
            {

                ModelState.AddModelError("LoginError", "Hesap kilitlendi");
                Log.Information(string.Format("{0} user account is locked on {1}", user.Email, DateTime.Now));
                return View();
            }

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("LoginError", "Giriş izniniz yok");
                return View();
            }

            if (result.RequiresTwoFactor)
            {
                ModelState.AddModelError("LoginError", "RequiresTwoFactorAuth");
                return View();
            }

            ModelState.AddModelError("LoginError", "Geçersiz E-posta ve/veya Şifre");
            return View();
        }

        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model, string returnUrl = null)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("{0} kullanıcısı {1} şifresi ile sisteme kayıt olurken bir hata meydana geldi. Tarih: {2}", model.Email, model.Password, DateTime.Now));

                ModelState.AddModelError("RegisterError", "Geçersiz veri girişi");
                return View();
            }

            var user = _userManager.Users.FirstOrDefault(i => i.Email == model.Email);

            if (user != null)
            {
                ModelState.AddModelError("RegisterError", "Bu e-posta adresi kullanımda");
                return View();
            }

            user = new ApplicationUser()
            {
                Email = model.Email,
                EmailConfirmed = true,
                UserName = model.Email,
                Name = model.Name,
                IsActive = true,
                NormalizedEmail = model.Email,
                NormalizedUserName = model.Email,
                PhoneNumber = model.Phone,
                PhoneNumberConfirmed = true,
                BranchId = null,
                ImagePath = "/files/admin/admin.png",
                DateCreated = DateTime.Now
                 
            };

            var result = _userManager.CreateAsync(user, model.Password).Result;

            if (result.Succeeded)
            {
                // Kayıt başarılı...
                await _userManager.AddToRoleAsync(user, model.Role);

                /*
                await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email)
                };

                await _userManager.AddClaimsAsync(user, claims);
                */

                _logger.LogInformation(string.Format("{0} kullanıcısı {1} şifresi ile sisteme kayıt oldu. Tarih: {2}", model.Email, model.Password, DateTime.Now));

                return Ok("Kayıt başarılı");
            }
            else
            {
                _logger.LogError(string.Format("{0} kullanıcısı {1} şifresi ile sisteme kayıt olurken bir hata meydana geldi. Tarih: {2}", model.Email, model.Password, DateTime.Now));

                return BadRequest("Kayıt başarısız");
            }
        }

        private ActionResult RedirectToLocal(ApplicationUser user, string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result;
                if (isAdmin)
                {
                    return RedirectToAction("Firms", "Admin");
                }

                var isFirm = _userManager.IsInRoleAsync(user, "Firm").Result;
                if (isFirm)
                {
                    return RedirectToAction("Branches", "Firm");
                }

                var isBranch = _userManager.IsInRoleAsync(user, "Branch").Result;
                if (isBranch)
                {
                    return RedirectToAction("Dashboard", "Branch");
                }

                return RedirectToAction("Logout", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains('@'))
                return BadRequest("Geçersiz e-posta");

            var user = _userManager.Users.First(i => i.Email == email && i.IsActive);

            if(user == null)
                return BadRequest("Kullanıcı bulunamadı");

            var isWaiter = await _userManager.IsInRoleAsync(user, "Waiter");

            if (isWaiter)
                return BadRequest("Kullanıcı bulunamadı");

            string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;

            SmtpClient client = new SmtpClient
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                Credentials = new NetworkCredential("*****", "*****"),
                Timeout = 10000,
                EnableSsl = true
            };

            var body = "Merhaba" + "&nbsp;<strong>" + user.Name + "</strong><br /><br />" +
                   "Yeni şifrenizi belirlemek için " + "&nbsp;<a href='" + Url.Action("ResetPassword", "Account", new { email , token }, "https") +
                   "'>" + "buraya tıklayın" + "</a>";

            MailMessage mail = new MailMessage()
            {
                Subject = "Hesap Kurtarma",
                Body = body,
                IsBodyHtml = true,
                Sender = new MailAddress("support@maestro.app"),
                BodyEncoding = Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                From = new MailAddress("support@maestro.app")

            };

            mail.To.Add(new MailAddress(email));

            client.Send(mail);

            return Ok("Hesap kurtarma linki e-posta adresinize gönderildi.");
        }

        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest("Geçersiz veri");

            var model = new ResetPasswordViewModel()
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Geçersiz veri");

            var user = _userManager.Users.First(i => i.Email == model.Email && i.IsActive);

            if (user == null)
                return BadRequest("Kullanıcı bulunamadı");

            await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            return Ok("Şifreniz güncellendi.");
        }
    }
}
