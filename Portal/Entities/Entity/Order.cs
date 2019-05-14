using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entity
{
    public class Order : EntityBase
    {
        [ForeignKey("Menu")]
        public int MenuId  { get; set; }
        public virtual Menu Menu { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("Waiter")]
        public string WaiterId { get; set; }
        public virtual ApplicationUser Waiter { get; set; }

        public int Status  { get; set; }

        public string Note { get; set; }
    }

    public enum OrderStatus
    {
        Waiting = 0x0,
        Preparing = 0x1,
        Done = 0x2
    }
}
