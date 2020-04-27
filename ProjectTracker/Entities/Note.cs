using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Entities
{
    [Table("Note", Schema = "ptm")]
    public partial class Note
    {
        public Note()
        {
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        public int TaskId { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [InverseProperty("Note")]
        public virtual ICollection<Task> Task { get; set; }
    }
}
