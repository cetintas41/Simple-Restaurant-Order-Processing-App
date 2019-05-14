using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entity
{
    public class Menu : EntityBase
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImagePath { get; set; }

        [ForeignKey("MenuType")]
        public int MenuTypeId { get; set; }
        public virtual MenuType MenuType { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class MenuDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImagePath { get; set; }

        public string MenuType { get; set; }

        public string MenuTypeId { get; set; }
    }
}
