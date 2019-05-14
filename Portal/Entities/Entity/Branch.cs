using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entity
{
    public class Branch : EntityBase
    {
        [ForeignKey("Firm")]
        public int FirmId { get; set; }
        public Firm Firm { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Table> Tables { get; set; }
    }

    public class BranchDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
