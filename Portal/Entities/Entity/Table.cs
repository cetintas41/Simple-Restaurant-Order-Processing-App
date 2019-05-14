using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entity
{
    public class Table : EntityBase
    {
        public string Name { get; set; }

        public bool IsClosed { get; set; }

        public string Code { get; set; }

        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }
    }

    public class TableDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsClosed { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }
    }
}
