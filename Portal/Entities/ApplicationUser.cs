using Entities.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public string ImagePath { get; set; }

        [ForeignKey("Branch")]
        public int? BranchId { get; set; }
        public virtual Branch Branch { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual Firm Firm { get; set; }

    }

    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string _Id { get; set; }
        public string Name { get; set; }
        public int BranchId { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Phone { get; set; }
        public int FirmId { get; set; }
        public string ImagePath { get; set; }

    }
}
