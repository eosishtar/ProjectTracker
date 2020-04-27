using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Entities
{
    [Table("Status", Schema = "ptm")]
    public partial class Status
    {
        public Status()
        {
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [Required]
        [StringLength(10)]
        public string Inactive { get; set; }

        [InverseProperty("Status")]
        public virtual ICollection<Task> Task { get; set; }
    }
}
