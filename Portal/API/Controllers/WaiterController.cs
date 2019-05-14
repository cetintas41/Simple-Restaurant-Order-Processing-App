using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController, Route("api/waiter")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WaiterController : ControllerBase
    {
        #region initial
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;
        private readonly ICountryService _countryService;
        private readonly ITableService _tableService;

        public WaiterController(ICountryService countryService, ILogger<AccountController> logger, IOrderService orderService, ITableService tableService)
        {
            _logger = logger;
            _orderService = orderService;
            _tableService = tableService;
            _countryService = countryService;
        }
        #endregion

        // api/waiter/waitingtables
        [HttpPost, Route("waitingtables")]
        public IActionResult WaitingTables([FromBody] WaitingTablesModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // Garsonlar için şubedeki hizmet bekleyen masaların listesini
            // döndürür.
            // Bekleyen masalarda müşteriler siparişlerini vermiş
            // fakat siparişler henüz bir garson tarafından karşılanmamıştır.
            // Garson masayı kapattıktan sonra bu masadaki siparişler sadece ilgili garsonun önüne düşebilir.
          
            var branch_id = Convert.ToInt32(model.BranchId.Decrypt());
            var tables = _tableService.GetWaitingTables(branch_id)
                .Select(i => new
                {
                   Id = i.Id.ToString().Encrypt(),
                   i.Name

                }).ToList();

            var data = new
            {
                FreeTables = tables
            };

            _logger.LogInformation(string.Format("MAC: {0}\nIP: {1}\nURL: {2}", model.MacAddress, model.IPAddress, " api/waiter/waitingtables"));
            return Ok(data);
        }

        // api/waiter/orders
        [HttpPost, Route("orders")]
        public IActionResult GetOrders(GetOrdersModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // istemci garson kullanım dilini model içinde belirtilmelidir.
            // default olarak Türkçe varsayılmıştır.
            var lang = model.Language ?? "tr";

            // istemci garsonun bağlı olduğu şube
            var branchId = Convert.ToInt32(model.BranchId.Decrypt());
            // bu şubede kullanılan para birimi
            var currency = _countryService.GetCurrencyByBranchId(branchId);

            // istemci garsonun şubede sorumlu olduğu AKTİF sipariş listesini döndürür.
            var orders = _orderService.GetOrdersOfWaiter(model.WaiterId, branchId)
                .Select(i => new
                {
                   Id = i.Id.ToString().Encrypt(),
                   i.Note,
                   Status = Extensions.OrderStatusTranslator(i.Status, lang),
                   MenuName = i.Menu.Name,
                   MenuPrice = string.Format("{0} {1}", i.Menu.Price, currency),
                   MenuImage = i.Menu.ImagePath,

                }).ToList();


            var data = new
            {
                Orders = orders
            };

            _logger.LogInformation(string.Format("{0} ID'li garsonun sorumlu olduğu siparişler getirildi. Tarih: {1}", model.WaiterId, DateTime.Now));

            return Ok(data);
        }

        // api/waiter/closetable
        [HttpPost, Route("closetable")]
        public async Task<IActionResult> CloseTable(CloseTableModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // istemci garson bekleyen masaları kapatır
            // kapattığı masalardaki siparişleri başka bir garson görüntüleyemez.
            // kısaca kapatılan masaya  sadece ilgili garson hizmet verebilr.
            var tableId = Convert.ToInt32(model.TableId.Decrypt());

            await _tableService.CloseTableAsync(tableId, model.WaiterId);

            _logger.LogInformation(string.Format("{0} ID'li masa {1} ID'li garson tarafından kapatıldı. Tarih: {2}", Convert.ToInt32(model.TableId.Decrypt()), model.WaiterId, DateTime.Now));

            return Ok();

        }

        // api/waiter/setnext
        [HttpPost, Route("setnext")]
        public IActionResult SetNext([FromBody] List<string> orders)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            // istemci garson siparişlerin mevcut durumlarını güncelleyerek
            // müşteriyi sipariş süreci ile ilgili bilgilendirmiş olur.
            var list = orders.Select(i => Convert.ToInt32(i.Decrypt())).ToList();

            _orderService.SetOrdersToNextProgress(list);

            _logger.LogInformation(string.Format("{0} adet siparişin durumu güncellendi. Tarih: {1}", orders.Count(), DateTime.Now));

            return Ok();
        }
    }
}