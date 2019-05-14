using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Entities.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace API.Controllers
{
    // api/customer
    [ApiController, Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        #region initial
        private readonly ITableService _tableService;
        private readonly ICustomerService _customerService;
        private readonly ICountryService _countryService;
        private readonly IMenuTypeService _menuTypeService;
        private readonly IFirmService _firmService;
        private readonly IOrderService _orderService;
        private readonly IBranchService _branchService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IBranchService branchService, ICountryService countryService, ILogger<CustomerController> logger, ITableService tableService, ICustomerService customerService, IMenuTypeService menuTypeService, IFirmService firmService, IOrderService orderService)
        {
            _logger = logger;
            _tableService = tableService;
            _customerService = customerService;
            _menuTypeService = menuTypeService;
            _firmService = firmService;
            _branchService = branchService;
            _countryService = countryService;
            _orderService = orderService;
        }

        #endregion

        // api/customer/location
        [HttpPost, Route("location")]
        public IActionResult Location([FromBody] LocationModel model)
        {
            // Uygulama başlatıldığında önce lokasyon seçilir. 
            // ülke isimlerinin dil bazlı farklı olması nedeniyle (USA, ABD gibi)
            // istemci seçilen dili model içinde belirtmelidir.
            var language = _countryService.GetLanguageByCode(model.Language ?? "tr");

            if (language == null)
                return BadRequest();

            // İstemci şube seçmek için önce ülke ve şehir seçimi yapmalıdır.
            // aşağıdaki listeden seçilen şehrin firma ve şubeleri istemciye cevap olarak dönülmektedir.
            var locations = _countryService.GetLocations()
                .Select(i => new
                {
                    Id = i.Id.ToString().Encrypt(),
                    Name = i.GetName(language.Id),
                    Cities = i.Cities.Select(l => new { Id= l.Id.ToString().Encrypt(), l.Name })
                });

            var data = new
            {
                Locations = locations
            };

            _logger.LogInformation(string.Format("MAC: {0}\nIP: {1}\nURL: {2}", model.MacAddress, model.IPAddress, "api/customer/location"));
            return Ok(data);
        }

        // api/customer/firms
        [HttpPost, Route("firms")]
        public IActionResult Firms([FromBody] FirmsModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // istemci tarafından seçilen şehirlerde yer alan
            // aktif firmalar, aktif şubeleri ile birlikte istemciye cevap olarak dönülür.
            var id = Convert.ToInt32(model.CityId.Decrypt());
            var firms = _firmService.GetActiveFirmsOfCity(id)
                .Select(i => new
                {
                    Id = i.Id.ToString().Encrypt(),
                    i.Name,
                    i.LogoPath,
                    Branches = i.Branches
                                    .Where(l => l.IsActive)
                                    .Select(x => new { Id = x.Id.ToString().Encrypt(), x.Name })
                                    .ToList()

                }).ToList();

            var data = new
            {
                Firms = firms
            };

            _logger.LogInformation(string.Format("MAC: {0}\nIP: {1}\nURL: {2}", model.MacAddress, model.IPAddress, "api/customer/firms"));
            return Ok(data);
        }

        // api/customer/connect
        [HttpPost, Route("connect")]
        public IActionResult Connect([FromBody] ConnectModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Geçersiz model\nMAC: {0}\nIP: {1}\nURL: {2}", model.MacAddress, model.IPAddress, "api/customer/connect"));
                return BadRequest();
            }

            // İstemci tafından seçilen şubenin id ve masa koduna göre
            // istemci ilgili şubenin ilgili masasına bağlanır
            // varsa mevcut müşteriler ve siparişlerini görebilir
            // veya yeni sipariş verebilir.
            int _branchId = Convert.ToInt32(model.BranchId.Decrypt());
            var currency = _countryService.GetCurrencyByBranchId(_branchId);
            var table = _tableService.GetTableOfBranch(model.Code.ToUpper(), _branchId);
            var lang = model.Language ?? "tr";

            if (table == null)
            {
                _logger.LogError(string.Format("Masa bulunamadı\nMAC: {0}\nIP: {1}\nURL: {2}", model.MacAddress, model.IPAddress, "api/customer/connect"));
                return BadRequest();
            }

            // yeni sipariş verebilmek için menü tipleri ve menüler listesi
            // istemciye cevap olarak dönülür.
            var menuTypes = _menuTypeService.GetMenuTypesOfBranch(_branchId)
               .Select(i => new
               {
                   Id = i.Id.ToString().Encrypt(),
                   i.Name,
                   Menus = i.Menus.Select(l => new
                   {
                       Id = l.Id.ToString().Encrypt(),
                       l.Name,
                       l.ImagePath,
                       Price = string.Format("{0} {1}", l.Price, currency)

                   }).ToList()

               }).ToList();

            // keza masada yer alan (varsa) müşteriler ve siparişleri de öyle..
            var customers = _customerService.GetCustomersByTableCode(model.Code)
                .Select(i => new
                {
                    Id = i.Id.ToString().Encrypt(),
                    i.Name,
                    i.TableCode,
                    Orders = i.Orders.Select(k => new
                    {
                        Id = k.Id.ToString().Encrypt(),
                        Name = k.Menu == null ? "" : k.Menu.Name,
                        Status = Extensions.OrderStatusTranslator(k.Status, lang),
                        Price = k.Menu == null ? string.Format("{0} {1}", 0.0M, currency)  : string.Format("{0} {1}", k.Menu.Price, currency)

                    }).ToList()

                }).ToList();

           
            var data = new
            {
                model.Code,
                Customers = customers,
                MenuTypes = menuTypes
            };

            _logger.LogInformation(string.Format("MAC: {0}\nIP: {1}\nURL: {2}", model.MacAddress, model.IPAddress, "api/customer/connect"));
            return Ok(data);
        }

        // api/customer/new
        [HttpPost, Route("new")]
        public async Task<IActionResult> Create([FromBody] CreateNewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Geçersiz model\nMAC: {0}\nIP: {1}\nURL: {2}", model.MacAddress, model.IPAddress, "api/customer/new"));
                return BadRequest();
            }

            var list = new List<Order>();
            var lang = model.Language ?? "tr";

            // istek yapan müşteri bu masada ise müşterinin id sini getirir.
            var customerId = await _customerService.GetOrCreateCustomerAsync(model.CustomerName.FirstCharToUpper(), model.TableCode.ToUpper());

            // model içindeki seçilen yeni siparişleri listeye doldurur.
            foreach (var order in model.Orders)
            {
                // sipariş için seçilen adet kadar döner
                for(int i = 1; i <= order.Piece; i++)
                {
                    list.Add(new Order
                    {
                        CustomerId = customerId,
                        DateCreated = DateTime.Now,
                        MenuId = Convert.ToInt32(order.MenuId.Decrypt()),
                        Note = order.Note,
                        Status = (int)OrderStatus.Waiting
                    });
                }
               
            }

           // gönderilen yeni siparişleri liste içerisinden ayıklayıp kaydeder.
            await _orderService.CreateOrdersAsync(list);

            // Şubenin bulunduğu ülkenin para birimini çeker.
            var _branchId = _branchService.GetBranchIdByTableCode(model.TableCode);
            var currency = _countryService.GetCurrencyByBranchId(_branchId);

            // geriye masadaki tüm siparişleri döndürür.
            var orders = _orderService.GetOrdersByTableCode(model.TableCode)
                .Select(i => new
                {
                    OrderId = i.Id.ToString().Encrypt(),
                    OrderNote = i.Note,
                    OrderStatus = Extensions.OrderStatusTranslator(i.Status, lang),
                    MenuPrice = string.Format("{0} {1}", i.Menu.Price, currency),
                    MenuImage = i.Menu.ImagePath,
                    MenuType = i.Menu.MenuType.Name,
                    MenuName = i.Menu.Name,
                    CustomerName = _customerService.GetCustomerById(i.CustomerId).Name,
                    CustomerId = i.CustomerId.ToString().Encrypt(),

                }).ToList();
               

            var data = new
            {
                Orders = orders
            };

            _logger.LogInformation(string.Format("MAC: {0}\nIP: {1}\nURL: {2}", model.MacAddress, model.IPAddress, "api/customer/new"));
            return Ok(data);
        }

        // api/customer/cancel
        [HttpPost, Route("cancel")]
        public async Task<IActionResult> DeleteOrder([FromBody] DeleteOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format("Geçersiz Model\nMAC: {0}\nIP: {1}\nURL: {2}", model.MacAddress, model.IPAddress, "api/customer/cancel"));
                return BadRequest();
            }

            // Müsteri isterse 'Bekliyor' statusündeki siparişlerini iptal edebilir.
            var list = model.Orders.Select(i => Convert.ToInt32(i.Decrypt())).ToList();
            
            await _orderService.CancelOrdersAsync(list);

            _logger.LogInformation(string.Format("MAC: {0}\nIP: {1}\nURL: {2}", model.MacAddress, model.IPAddress, "api/customer/cancel"));

            return Ok();
        }
    }
}