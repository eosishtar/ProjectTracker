using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.ViewModels
{
    public class ProjectPostModel
    {
        public int Id { get; set;  }

        [Display(Name = "Project Name")]
        [Required(ErrorMessage = "Project Name is required.")]
        public string Name { get; set; }

        [Display(Name = "Project Description")]
        public string Description { get; set; }

        [Display(Name = "Project Active")]
        [Required(ErrorMessage = "Active is required.")]
        public bool IsActive { get; set; }

        [Display(Name = "Date Due")]
        [Required(ErrorMessage = "Date Due required")]
        public DateTime DueDate { get; set; }
    }
}
