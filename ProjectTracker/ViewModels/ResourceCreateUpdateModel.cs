using ProjectTracker.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.ViewModels
{
    public class ResourceCreateUpdateModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }

        [Display(Name = "Title")]
        public string Description { get; set; }

        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
        [ValidateContactNumberAttribute("Invalid contact number", false)]
        public string ContactNumber { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Display(Name = "Active")]
        public bool IsActive  { get; set; }
    }
}
