using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Entities
{
    [Table("Documents", Schema = "ptm")]
    public partial class Documents
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        [Required]
        [StringLength(50)]
        public string FileName { get; set; }
        [Required]
        [StringLength(10)]
        public string FileType { get; set; }

        [ForeignKey("TaskId")]
        [InverseProperty("Documents")]
        public virtual Task Task { get; set; }

    }
}
