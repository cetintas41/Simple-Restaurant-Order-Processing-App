using Entities;
using Entities.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "Branch")]
    public class BranchController : Controller
    {
        #region
        private readonly UserManager<ApplicationUser> _userManager;
      
        private readonly IFirmService _firmService;
        private readonly ITableService _tableService;
        private readonly IMenuService _menuService;
        private readonly IMenuTypeService _menuTypeService;
        private readonly IDocumentService _documentService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IStatsService _statService;
        private readonly ILogger<BranchController> _logger;

        // Login olan kullanıcı ve kullanıcının rolleri
        private ApplicationUserDto _currentUser => _userService.GetUserDtoById(User.GetUserId());
        private List<string> _currentUserRoles => _userService.GetUserRoles(User.GetUserId()).ToList();
        private Firm _currentFirm => _firmService.GetFirmById(_currentUser.FirmId);

        public BranchController
            (
            IStatsService statService,
            IUserService userService, 
            ICustomerService customerService, 
            IOrderService orderService, 
            IDocumentService documentService, 
            IMenuTypeService menuTypeService,
            IMenuService menuService, 
            ITableService tableService,
            IFirmService firmService,
            UserManager<ApplicationUser> userManager, 
            ILogger<BranchController> logger
            )
        {
            _firmService = firmService;
            _userManager = userManager;
            _tableService = tableService;
            _menuService = menuService;
            _menuTypeService = menuTypeService;
            _documentService = documentService;
            _orderService = orderService;
            _customerService = customerService;
            _userService = userService;
            _logger = logger;
            _statService = statService;
        }

        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard", "Branch");
        }

        public IActionResult Dashboard()
        {
            // Şubedeki kayıtlı masalar ve doluluk durumları
            var tables = _tableService.GetTablesOfBranch(_currentUser.BranchId)
                .Select(i => new TableDto
                {
                    Total = _tableService.GetTableTotal(i.Id),
                    Id = i.Id.ToString().Encrypt(),
                    Name = i.Name,
                    Code = i.Code,
                    IsClosed = i.IsClosed

                }).ToList();

            var model = new BranchDashboardViewModel()
            {
                Tables = tables,
                CurrentUserRoles = _currentUserRoles,
                CurrentUser = _currentUser,
                CurrentFirmLogo = _currentFirm.LogoPath
                 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWaiter(CreateWaiterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Geçersiz veri");

            // Mevcut şubeye ait bir garson tanımlar.
            var user = new ApplicationUser
            {
                DateCreated = DateTime.Now,
                IsActive = true,
                Email = model.Email,
                EmailConfirmed = true,
                BranchId = _currentUser.BranchId,
                Name = model.Name,
                NormalizedEmail = model.Email,
                NormalizedUserName = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true
            };

            await _userService.CreateUserAsync(user, model.Password, "Waiter");

            _logger.LogInformation(string.Format("{0} şubesi {1} isimli bir garson oluşturdu.", _currentUser.Name, user.Name));

            return Ok("Garson kaydedildi");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable(string tableName)
        {
            if(string.IsNullOrEmpty(tableName))
                return BadRequest("Geçersiz veri");

            // Mevcut şubeye ait bir masa tanımlar.
            var table = new Table
            {
                BranchId = _currentUser.BranchId,
                Code = _tableService.GenerateTableCode(6),
                DateCreated = DateTime.Now,
                IsClosed = false,
                Name = tableName,
            };

            await _tableService.CreateTableAsync(table);

            _logger.LogInformation(string.Format("{0} şubesi {1} isimli bir masa oluşturdu.", _currentUser.Name, table.Name));

            return Ok("Masa oluşturuldu");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu(CreateMenuModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest("Geçersiz veri");

            // Menü görselini firmanın dizinine kaydet
            var firmFolderName = _currentFirm.LogoPath.Split('/')[3];
            var imagePath = _menuService.SaveMenuImage(firmFolderName, model.Image);

            var menuTypeId = Convert.ToInt32(model.MenuType.Decrypt());
            var price = Convert.ToDecimal(model.Price.Replace('.', ','));

            // Menü nesnesini oluştur.
            var menu = new Menu
            {
                DateCreated = DateTime.Now,
                ImagePath = imagePath,
                Name = model.Name,
                MenuTypeId = menuTypeId,
                Price = price
            };

            await _menuService.CreateMenuAsync(menu);

            _logger.LogInformation(string.Format("{0} şubesi {1} isimli bir menü oluşturdu.", _currentUser.Name, menu.Name));

            return Ok("Menü oluşturuldu");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuType(string menuTypeName)
        {
            if (string.IsNullOrEmpty(menuTypeName))
                return BadRequest("Geçersiz veri");

            var menutype = new MenuType
            {
                BranchId = _currentUser.BranchId,
                Name = menuTypeName,
                DateCreated = DateTime.Now
            };

            await _menuTypeService.CreateMenuTypeAsync(menutype);

            _logger.LogInformation(string.Format("{0} şubesi {1} isimli bir menü tipi oluşturdu.", _currentUser.Name, menutype.Name));

            return Ok("Menü tipi oluşturuldu");
        }

        public IActionResult Download(int id)
        {
            if (id == 0) return BadRequest("Geçersiz veri");

            if(id == (int)DocumentType.CreateWaiterForm)
            {
                // Garson oluşturma formunu indirir.
                var headers = new string[]
                {
                   "Adı Soyadı",
                   "E-posta",
                   "Telefon",
                   "Şifre"
                };

                byte[] file = _documentService.DownloadCreateUserForm(headers);
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GarsonOlusturmaFormu.xlsx");

            }

            return BadRequest("Geçersiz istek");
        }

        [HttpPost]
        public async Task<IActionResult> CreateWaiters(IFormFile file)
        {
            if (file == null)
                return BadRequest("Geçersiz dosya");

            // Excel dosyasından gelen veriler ile şubeye ait garsonlar oluşturur.

            await _userService.CreateWaitersAsync(file, _currentUser.BranchId);

            _logger.LogInformation(string.Format("{0} şubesi garsonlar oluşturdu.", _currentUser.Name));

            return Ok("Garsonlar oluşturuldu");
        }

        [HttpPost]
        public async Task<IActionResult> ReleaseTable(string tableId)
        {
            if (string.IsNullOrEmpty(tableId))
                return BadRequest("Geçersiz veri");

            // Masadaki hesap ödendiğinde Şube kullanıcısı masayı serbest bırakır.
            // serbest bırakılan masa boşa çıkar ve yeni müşterilere hizmet etmek için hazır duruma gelir.

            var table_id = Convert.ToInt32(tableId.Decrypt());

            await _tableService.ReleaseTableAsync(table_id, _currentUser.BranchId);

            _logger.LogInformation(string.Format("{0} şubesi #{1} masadaki ödemeleri onayladı.", _currentUser.Name, table_id, DateTime.Now));

            return Ok("Onaylandı");
        }

        [HttpPost]
        public IActionResult TableSummary(string tableId)
        {
            decimal tableTotal = 0;
            string orderList = string.Empty;
            var summary = new List<TableSummaryModel>();

            var table_id = Convert.ToInt32(tableId.Decrypt());

            // masa kodu: masa, o an masada yer alan müşteriler, müşterilerin siparişleri, siparişleri karşılayan garson
            // arasındaki bağlantıyı sağlayan unique bir değerdir.
            var tableCode = _tableService.GetTableById(table_id, _currentUser.BranchId).Code;

            // masada o an yer alan müşterileri getirir.
            var customers = _customerService.GetCustomersByTableCode(tableCode);

            // her bir müşteri için adisyonu hazırlar.
            foreach (var customer in customers)
            {
                var orders = _orderService.GetOrdersOfCustomer(customer.Id);

                foreach (var order in orders)
                {
                    orderList += (string.Format("{0} ({1} {2}), ", order.Menu.Name, order.Menu.Price, "TL"));
                }

                summary.Add(new TableSummaryModel
                {
                    CustomerName = customer.Name,
                    CustomerTotal = orders.Sum(i => i.Menu.Price),
                    Orders = orderList == "" ? orderList : orderList.Substring(0, orderList.Length - 2)
                });

                tableTotal += orders.Sum(i => i.Menu.Price);
                orderList = string.Empty;

            }

            var model = new TableSummaryViewModel
            {
                Summary = summary,
                Total = tableTotal
            };

            return PartialView(model);
        }

        public IActionResult Waiters(string s, string kw, int p = 1, int t = 25)
        {
            // Şubede kayıtlı garsonların sayısı
            var waiterCount = _userService.GetTotalWaitersCountFiltered(_currentUser.BranchId, kw);

            // Şubede kayıtlı garsonlar
            var waiters = _userService.GetWaitersOfBranchFiltered(_currentUser.BranchId, kw, s, p, t)
                .Select(i => new ApplicationUserDto
                {
                    Id = i.Id.Encrypt(),
                    Name = i.Name,
                    IsActive = i.IsActive,
                    Email = i.Email,
                    Phone = i.PhoneNumber

                }).ToList();

            var pagination = new PaginationModel()
            {
                Count = waiterCount,
                Page = p,
                Top = t,
                SortBy = s,
                Keyword = kw
            };

            var model = new WaitersViewModel
            {
                Waiters = waiters,
                Pagination = pagination,
                CurrentUserRoles = _currentUserRoles,
                CurrentUser = _currentUser,
                CurrentFirmLogo = _currentFirm.LogoPath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWaiter(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Geçersiz veri");

            // Şubeye ait olan bir garsonu siler.
            var waiterId = id.Decrypt();
            await _userService.DeleteUserById(waiterId);

            _logger.LogInformation(string.Format("{0} şubesi #{1} garsonu sildi.", _currentUser.Name, waiterId));

            return Ok("Garson silindi");

        }

        public async Task<IActionResult> ChangeAccess(string waiterId)
        {
            if (string.IsNullOrEmpty(waiterId))
                return BadRequest("Geçersiz veri");

            // Şubeye ait olan garsonun sisteme erişimi bloke edilir veya blokaj kaldırılır.
            var waiter_id = waiterId.Decrypt();
            await _userService.ChangeUserAccessAsync(waiter_id);

            _logger.LogInformation(string.Format("{0} şubesi {1} ID'li garsonun erişim hakkını güncelledi.", _currentUser.Name, waiter_id));

            return Ok("Erişim hakkı güncellendi");
        }

        public IActionResult Menus(string s, string kw, int p = 1, int t = 25)
        {
            // Şubeye ait menülerin sayısı
            var menuCount = _menuService.GetTotalMenusCountFiltered(_currentUser.BranchId, kw);

            // Şubeye ait menuler
            var menus = _menuService.GetMenusOfBranchFiltered(_currentUser.BranchId, kw, s, p, t)
                .Select(i => new MenuDto
                {
                    MenuTypeId = i.MenuTypeId.ToString().Encrypt(),
                    Id = i.Id.ToString().Encrypt(),
                    ImagePath = i.ImagePath,
                    Name = i.Name,
                    Price = i.Price,
                    MenuType = i.MenuType.Name

                }).ToList();

            // Şubeye ait menü tipleri
            var menuTypes = _menuTypeService.GetMenuTypes(_currentUser.BranchId)
               .Select(i => new MenuTypeDto
               {
                   Id = i.Id.ToString().Encrypt(),
                   Name = i.Name

               }).ToList();

            var pagination = new PaginationModel()
            {
                Count = menuCount,
                Page = p,
                Top = t,
                SortBy = s,
                Keyword = kw
            };

            var model = new MenusViewModel
            {
                MenuTypes = menuTypes,
                Menus = menus,
                Pagination = pagination,
                CurrentUserRoles = _currentUserRoles,
                CurrentUser = _currentUser,
                CurrentFirmLogo = _currentFirm.LogoPath
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenu(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Geçersiz veri");

            var menu_id = Convert.ToInt32(id.Decrypt());

            // menüye ait görseli sil.
            _menuService.DeleteMenuImage(menu_id);

            // menüyü sil
            await _menuService.DeleteMenuAsync(menu_id, _currentUser.BranchId);

            _logger.LogInformation(string.Format("{0} şubesi #{1} menüyü sildi.", _currentUser.Name, menu_id));

            return Ok("Menü silindi");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMenu(UpdateMenuModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Geçersiz veri");

            string imagePath = string.Empty;
            var menu_id = Convert.ToInt32(model.Id.Decrypt());

            if (model.Image != null)
            {
                // yeni görseli kaydet
                var firmFolder = _currentFirm.LogoPath.Split('/')[3];
                imagePath = _menuService.SaveMenuImage(firmFolder, model.Image);

                // eski görseli sil
                _menuService.DeleteMenuImage(menu_id);
            }

            var price = Convert.ToDecimal(model.Price.Replace('.', ','));
            var menutypeId = Convert.ToInt32(model.MenuTypeId.Decrypt());

            // Menü bilgilerini güncelle
            var menu = _menuService.GetMenuById(menu_id, _currentUser.BranchId);

            menu.Name = model.Name;
            menu.Price = price;
            menu.MenuTypeId = menutypeId;

            if (!string.IsNullOrEmpty(imagePath))
                menu.ImagePath = imagePath;

            await _menuService.UpdateMenuAsync(menu);

            _logger.LogInformation(string.Format("{0} şubesi {1} isimli menüyü güncelledi.", _currentUser.Name, menu.Name));

            return Ok("Menü güncellendi");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrices(int rate)
        {
            if (rate == 0)
                return BadRequest("Geçersiz veri");

            // tüm ürünlerin/menülerin fiyatlarına toplu olarak zam veya indirim uygulanır.
            await _menuService.UpdatePricesByRateAsync(rate, _currentUser.BranchId);

            _logger.LogInformation(string.Format("{0} şubesi, tüm fiyatları {1}% oranında güncelledi.", _currentUser.Name, rate));

            return Ok("Fiyatlar güncellendi");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWaiter(UpdateWaiterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Geçersiz veri");

            // Seçilen garson bilgilerinde güncelleme yapılır.

            var waiter = _userService.GetUserById(model.Id.Decrypt());

            if(waiter.Email != model.Email)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(waiter, model.Email);
                await _userManager.ChangeEmailAsync(waiter, model.Email, token);
            }

            if(waiter.PhoneNumber != model.PhoneNumber)
            {
                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(waiter, model.PhoneNumber);
                await _userManager.ChangePhoneNumberAsync(waiter, model.PhoneNumber, token);
            }

            _logger.LogInformation(string.Format("{0} şubesi {1} isimli garson bilgilerini güncelledi.", _currentUser.Name, waiter.Name));


            return Ok("Garson bilgileri güncellendi");
        }

        public IActionResult MenuTypes(string s, string kw, int p = 1, int t = 25)
        {
            // Şubede tanımlı menü tipleri adedi
            var menutypeCount = _menuTypeService.GetTotalMenuTypesCountFiltered(_currentUser.BranchId, kw);

            // Şubede tanımlı menü tipleri
            var menutypes = _menuTypeService.GetMenuTypesOfBranchFiltered(_currentUser.BranchId, kw, s, p, t)
                .Select(i => new MenuTypeDto
                {
                    Id = i.Id.ToString().Encrypt(),
                    Name = i.Name

                }).ToList();
          
            var pagination = new PaginationModel()
            {
                Count = menutypeCount,
                Page = p,
                Top = t,
                SortBy = s,
                Keyword = kw
            };

            var model = new MenuTypesViewModel
            {
                MenuTypes = menutypes,
                Pagination = pagination,
                CurrentUserRoles = _currentUserRoles,
                CurrentUser = _currentUser,
                CurrentFirmLogo = _currentFirm.LogoPath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMenuType(string mtId, string mtName)
        {
            if (string.IsNullOrEmpty(mtName) || string.IsNullOrEmpty(mtId))
                return BadRequest("Geçersiz veri");

            // Menü tipini günceller.
            var menutype_id = Convert.ToInt32(mtId.Decrypt());
            var menutype = _menuTypeService.GetMenuTypeById(menutype_id, _currentUser.BranchId);
            menutype.Name = mtName;

            await _menuTypeService.UpdateMenuTypeAsync(menutype);

            _logger.LogInformation(string.Format("{0} şubesi {1} isimli menü tipi bilgilerini güncelledi.", _currentUser.Name, menutype.Name));

            return Ok("Menü tipi güncellendi");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenuType(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Geçersiz veri");

            // Seçilen menü tipini siler.
            var menutype_id = Convert.ToInt32(id.Decrypt());

            await _menuTypeService.DeleteMenuTypeAsync(menutype_id, _currentUser.BranchId);

            _logger.LogInformation(string.Format("{0} şubesi #{1} menü tipini sildi.", _currentUser.Name, menutype_id));

            return Ok("Menü tipi silindi");

        }

        public IActionResult Tables(string s, string kw, int p = 1, int t = 25)
        {
            // Şubede tanımlı masa adedi
            var tableCount = _tableService.GetTotalTablesCountFiltered(_currentUser.BranchId, kw);

            // Şubede tanımlı masalar
            var tables = _tableService.GetTablesOfBranchFiltered(_currentUser.BranchId, kw, s, p, t)
                .Select(i => new TableDto
                {
                    Id = i.Id.ToString().Encrypt(),
                    Name = i.Name,
                    Code = i.Code,
                    IsClosed = i.IsClosed

                }).ToList();

            var pagination = new PaginationModel()
            {
                Count = tableCount,
                Page = p,
                Top = t,
                SortBy = s,
                Keyword = kw
            };

            var model = new TablesViewModel
            {
                Tables = tables,
                Pagination = pagination,
                CurrentUserRoles = _currentUserRoles,
                CurrentUser = _currentUser,
                CurrentFirmLogo = _currentFirm.LogoPath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTable(UpdateTableModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Geçersiz veri");

            // masa günceller.
            var table_id = Convert.ToInt32(model.Id.Decrypt());
            var table = _tableService.GetTableById(table_id, _currentUser.BranchId);
            table.Name = model.Name;

            await _tableService.UpdateTableAsync(table);

            _logger.LogInformation(string.Format("{0} şubesi {1} isimli masanın bilgilerini güncelledi.", _currentUser.Name, table.Name));

            return Ok("Masa güncellendi");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTable(string id)
        {
            if(string.IsNullOrEmpty(id))
                return BadRequest("Geçersiz veri");

            // Masa silme
            var table_id = Convert.ToInt32(id.Decrypt());

            await _tableService.DeleteTableAsync(table_id, _currentUser.BranchId);

            _logger.LogInformation(string.Format("{0} şubesi #{1} masayı sildi.", _currentUser.Name, table_id));

            return Ok("Masa silindi");
        }

        public IActionResult GetWaiters()
        {
            // Şubede tanımlı tüm garsonların isim ve ID lerini döndürür.
            var waitersCount = _userService.GetTotalWaitersCountFiltered(_currentUser.BranchId, null);

            var waiters = _userService.GetWaitersOfBranchFiltered(_currentUser.BranchId, null, "asc", 1, waitersCount)
                .Select(i => new { ID = i.Id.Encrypt(), NAME = i.Name }).ToList();

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(waiters);
            return Ok(data);

        }

        [HttpPost]
        public IActionResult WaiterStats(WaiterStatsModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Geçersiz veri"); // to do: burası daha sonra hata sayfasına yönlendirilecek.

            // Bakılan masa adedi
            var tableStats = _statService.GetTableCountStatsOfWaiter(model.WaiterId.Decrypt(), model.Start, model.End);

            // Kazandırdığı ciro
            var incomeStats = _statService.GetIncomeStatsOfWaiter(model.WaiterId.Decrypt(), model.Start, model.End);

            // garson adı
            var waiterName = _userService.GetUserById(model.WaiterId.Decrypt()).Name;
            // grafiklerde kullanılacak renk dolguları
            var rand = new Random();
            var colors = new Dictionary<string, string>();
            for(int i = 0; i < tableStats.Count(); i++)
            {
                var r = rand.Next(0, 255);
                var g = rand.Next(0, 255);
                var b = rand.Next(0, 255);

                var bgColor = string.Format("rgba({0},{1},{2},0.2)", r,g,b);
                var borderColor = string.Format("rgba({0},{1},{2},1)", r,g,b);
                colors.Add(bgColor, borderColor);
            }

            var vmodel = new WaiterStatsViewModel
            {
                WaiterName = waiterName,
                Colors = colors,
                IncomeStats = incomeStats,
                TableStats = tableStats,
                CurrentFirmLogo = _currentFirm.LogoPath,
                CurrentUserRoles = _currentUserRoles,
                CurrentUser = _currentUser
            };

            return View(vmodel);

        }

        public IActionResult Stats()
        {
            // Günlük - Haftalık - Aylık olarak bakılan toplam masa adedi
            var monthlyTotalTable = _statService.GetTotalTableStatsOfBranch(_currentUser.BranchId, (int) StatsType.Monthly);
            var weeklyTotalTable = _statService.GetTotalTableStatsOfBranch(_currentUser.BranchId, (int)StatsType.Weekly);
            var dailyTotalTable = _statService.GetTotalTableStatsOfBranch(_currentUser.BranchId, (int)StatsType.Daily);

            // Toplam Gelir Günlük - Aylık - Yıllık

            // Toplam Müşteri Günlük - Aylık - Yıllık

            var model = new StatsViewModel()
            {
                MontlyTotalTable = monthlyTotalTable,
                DailyTotalTable = dailyTotalTable,
                WeeklyTotalTable = weeklyTotalTable,
                CurrentFirmLogo = _currentFirm.LogoPath,
                CurrentUserRoles = _currentUserRoles,
                CurrentUser = _currentUser
            };

            return View(model);
        }

    }
}