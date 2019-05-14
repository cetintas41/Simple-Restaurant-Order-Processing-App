using Entities;
using Entities.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ViewModels
    {
    }

    public class LayoutViewModel
    {
        public ApplicationUserDto CurrentUser { get; set; }
        public string CurrentFirmLogo { get; set; }
        public List<string> CurrentUserRoles { get; set; }
        public PaginationModel Pagination { get; set; }

    }

    public class AdminFirmsViewModel : LayoutViewModel
    {
        public List<ApplicationUserDto> FirmUsers { get; set; }
    }

    public class FirmDashboardViewModel : LayoutViewModel
    {
        public List<ApplicationUserDto> BranchUsers { get; set; }
    }

    public class BranchDashboardViewModel : LayoutViewModel
    {
        public List<TableDto> Tables { get; set; }
    }

    public class TableSummaryViewModel
    {
        public List<TableSummaryModel> Summary { get; set; }
        public decimal Total { get; set; }
    }

  
    public class WaitersViewModel : LayoutViewModel
    {
        public List<ApplicationUserDto> Waiters { get; set; }
    }

    public class MenusViewModel : LayoutViewModel
    {
        public List<MenuTypeDto> MenuTypes { get; set; }
        public List<MenuDto> Menus { get; set; }

    }

    public class MenuTypesViewModel : LayoutViewModel
    {
        public List<MenuTypeDto> MenuTypes { get; set; }
    }
    
    public class TablesViewModel : LayoutViewModel
    {
        public List<TableDto> Tables { get; set; }
    }

    public class WaiterStatsViewModel : LayoutViewModel
    {
        public Dictionary<string,int> TableStats { get; set; }
        public Dictionary<string,decimal> IncomeStats { get; set; }
        public Dictionary<string, string> Colors { get; set; }
        public string WaiterName { get; set; }
    }

    public class StatsViewModel : LayoutViewModel
    {
        public int MontlyTotalTable { get; set; }
        public int WeeklyTotalTable { get; set; }
        public int DailyTotalTable { get; set; }
    }


    public class SettingsViewModel : LayoutViewModel
    {

    }

    public class ResetPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required, Compare("NewPassword")]
        public string NewPasswordAgain { get; set; }
    }
}
