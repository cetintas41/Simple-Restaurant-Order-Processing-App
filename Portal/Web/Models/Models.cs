using Entities.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Models
    {
    }

    public class LoginModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string PasswordConfirm { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Role { get; set; }
    }

    public class CreateFirmModel
    {
        [Required]
        public IFormFile Logo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email  { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string PasswordConfirm  { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }

    public class CreateBranchModel
    {
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string PasswordConfirm { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
    }

    public class CreateWaiterModel
    {
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string PasswordConfirm { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }

    public class CreateMenuModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string MenuType  { get; set; }
        [Required]
        public string Price  { get; set; }
        [Required]
        public IFormFile Image  { get; set; }
    }

    public class TableSummaryModel
    {
        public string CustomerName { get; set; }
        public decimal CustomerTotal { get; set; }
        public string Orders { get; set; }
    }

    public class PaginationModel
    {
        public int Count { get; set; }
        public int Page { get; set; }
        public int Top { get; set; }
        public string SortBy { get; set; }
        public string Keyword { get; set; }
    }

    public class UpdateMenuModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string MenuTypeId { get; set; }
        [Required]
        public string Price { get; set; }

        public IFormFile Image { get; set; }
    }

    public class UpdateWaiterModel
    {
        [Required]
        public string Id { get; set; }   
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }

    public class UpdateTableModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
   
    public class WaiterStatsModel
    {
        [Required]
        public string WaiterId { get; set; }

        [Required]
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }

    public class PaginationPartialModel
    {
        public PaginationModel Pagination { get; set; }
        public string Url { get; set; }
    }

    public class SortPartialModel
    {
        public PaginationModel Pagination { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string ColumnName { get; set; }

    }

    public class UpdateUserModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        public string NewPasswordAgain { get; set; }
    }
}
