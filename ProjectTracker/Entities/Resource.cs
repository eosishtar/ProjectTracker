using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectTracker.Entities
{
    [Table("Resource", Schema = "ptm")]
    public partial class Resource
    {
        public Resource()
        {
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        public string ContactNumber { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [InverseProperty("Resource")]
        public virtual ICollection<Task> Task { get; set; }
    }
}
