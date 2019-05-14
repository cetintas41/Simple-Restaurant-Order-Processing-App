using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace API.Controllers
{
    [ApiController, Route("api/auth")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AccountController(IUserService userService, ILogger<AccountController> logger, IConfiguration configuration)
        {
            _userService = userService;
            _logger = logger;
            _configuration = configuration;
        }

        // api/auth/index
        [AllowAnonymous, HttpGet, Route("index")]
        public IActionResult Index()
        {
            return Ok(DateTime.Now.Date.ToLongDateString());
        }

        // api/auth/login
        [HttpPost, AllowAnonymous, Route("login")]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("{0} kullanıcısı hatalı veri gönderdi. [Tarih:{1}]", model.Email, DateTime.Now.ToLongDateString()));
                return BadRequest();
            }

            var user = _userService.GetUserByCredentials(model.Email, model.Password);

            if (user == null)
            {
                _logger.LogError(string.Format("{0} kullanıcısı bulunamadı. [Tarih:{1}]", model.Email, DateTime.Now.ToLongDateString()));
                return Unauthorized();
            }

            var isWaiter = _userService.GetUserRoles(user.Id).Contains("Waiter");

            if (!isWaiter)
            {
                _logger.LogError(string.Format("{0} kullanıcısı Garson değil. [Tarih:{1}]", model.Email, DateTime.Now.ToLongDateString()));
                return Unauthorized();
            }

            if (!user.IsActive)
            {
                _logger.LogError(string.Format("{0} kullanıcısı pasifize edilmiş. [Tarih:{1}]", model.Email, DateTime.Now.ToLongDateString()));
                return Unauthorized();
            }

            if (!user.Branch.IsActive)
            {
                _logger.LogError(string.Format("{0} kullanıcısının şubesi pasifize edilmiş. [Tarih:{1}]", model.Email, DateTime.Now.ToLongDateString()));
                return Unauthorized();
            }

            if (!user.Branch.Firm.IsActive)
            {
                _logger.LogError(string.Format("{0} kullanıcısının firması pasifize edilmiş. [Tarih:{1}]", model.Email, DateTime.Now.ToLongDateString()));
                return Unauthorized();
            }

            var roles = _userService.GetUserRoles(user.Id);

            if (roles == null || roles.Count == 0 || !roles.Contains("Waiter"))
            {
                _logger.LogError(string.Format("{0} kullanıcısı geçerli bir role sahip değil. [Tarih:{1}]", model.Email, DateTime.Now.ToLongDateString()));
                return BadRequest();
            }

            // user verified
            _logger.LogInformation(string.Format("{0} kullanıcısı doğrulandı. [Tarih:{1}]", model.Email, DateTime.Now.ToLongDateString()));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                  issuer: _configuration["Issuer"],
                  audience: _configuration["Audience"],
                  claims: claims,
                  expires: DateTime.UtcNow.AddHours(7),
                  notBefore: DateTime.UtcNow,
                  signingCredentials: new SigningCredentials(
                      new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SigningKey"])),
                      SecurityAlgorithms.HmacSha256)
              );

            var data = new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                user.Id,
                user.Email,
                user.Name,
                user.PhoneNumber,
                BranchId = user.BranchId.ToString().Encrypt(),
                BranchName = user.Branch.Name,
                FirmName = user.Branch.Firm.Name,
                FirmLogo = user.Branch.Firm.LogoPath
            };

            return Ok(data);
        }
    }
}