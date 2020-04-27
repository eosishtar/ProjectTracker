using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Entities
{
    [Table("Effort", Schema = "ptm")]
    public partial class Effort
    {
        public Effort()
        {
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [Column(TypeName = "money")]
        public decimal ValueInMinutes { get; set; }

        [InverseProperty("Effort")]
        public virtual ICollection<Task> Task { get; set; }
    }
}
