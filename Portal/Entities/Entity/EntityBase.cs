using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entity
{
    public class EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
