using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entity
{
    public class MenuType : EntityBase
    {
        public string Name { get; set; }

        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }

    public class MenuTypeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
