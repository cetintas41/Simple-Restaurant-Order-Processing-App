using System.Collections.Generic;

namespace Entities.Entity
{
    public class Customer : EntityBase
    {
        public string Name { get; set; }

        public string TableCode { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class CustomerDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string TableCode { get; set; }
    }
}
