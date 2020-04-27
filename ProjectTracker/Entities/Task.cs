using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Entities
{
    [Table("Task", Schema = "ptm")]
    public partial class Task
    {
        public Task()
        {
            Documents = new HashSet<Documents>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public int EffortId { get; set; }
        public int StatusId { get; set; }
        public int ResourceId { get; set; }
        public int? NoteId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("EffortId")]
        [InverseProperty("Task")]
        public virtual Effort Effort { get; set; }
        [ForeignKey("NoteId")]
        [InverseProperty("Task")]
        public virtual Note Note { get; set; }
        [ForeignKey("ProjectId")]
        [InverseProperty("Task")]
        public virtual Project Project { get; set; }
        [ForeignKey("ResourceId")]
        [InverseProperty("Task")]
        public virtual Resource Resource { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("Task")]
        public virtual Status Status { get; set; }
        [InverseProperty("Task")]
        public virtual ICollection<Documents> Documents { get; set; }
    }
}
