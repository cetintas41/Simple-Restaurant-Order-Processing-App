using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entity
{
    public class Firm : EntityBase
    {
        public string Name { get; set; }

		public bool IsActive { get; set; }

        public string LogoPath { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }     
    }

    public class FirmDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
