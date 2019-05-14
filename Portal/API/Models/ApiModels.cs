using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ApiModels
    {
        public string IPAddress  { get; set; }
        public string MacAddress { get; set; }
    }

    public class ErrorModel
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class LoginModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class ConnectModel : ApiModels
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string BranchId { get; set; }
        public string Language { get; set; }
    }
    
    public class CreateNewModel : ApiModels
    {
        [Required]
        public List<OrderModel> Orders { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string TableCode  { get; set; }
        public string Language { get; set; }
    }

    public class OrderModel
    {
        public string MenuId { get; set; }
        public int Piece { get; set; }
        public string Note { get; set; }
    }

    public class CreateOrderModel
    {
        [Required]
        public List<OrderModel> Orders { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string TableCode { get; set; }
    }

    public class GetOrdersModel
    {
        [Required]
        public string WaiterId { get; set; }
        [Required]
        public string BranchId { get; set; }
        public string Language { get; set; }
    }

    public class CloseTableModel
    {
        [Required]
        public string WaiterId { get; set; }
        [Required]
        public string TableId { get; set; }
    }

    public class FirmsModel : ApiModels
    {
        [Required]
        public string CityId { get; set; }
    }

    public class LocationModel : ApiModels
    {
        public string Language { get; set; }
    }

    public class DeleteOrderModel : ApiModels
    {
        [Required]
        public List<string> Orders { get; set; }

    }

    public class WaitingTablesModel : ApiModels
    {
        [Required]
        public string BranchId { get; set; }
    }
}
