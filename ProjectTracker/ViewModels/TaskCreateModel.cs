using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProjectTracker.Entities;
using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ProjectTracker.ViewModels
{
    public class TaskCreateModel
    {
        public TaskCreateModel()
        {
            Files = new List<IFormFile>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name ="Task Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public List<LookUp.LookUpDetail> StatusDesc { get; set; }

        public int ProjectId { get; set; }

        [Display(Name = "Project Name")]
        public IQueryable<SelectListItem> ProjectName { get; set; }

        [Display(Name = "Effort Required")]
        public int? EffortId { get; set; }

        public List<LookUp.LookUpDetail> EffortDesc { get; set; }

        [Display(Name = "Resource Name")]
        public int? ResourceId { get; set; }

        public List<LookUp.LookUpDetail> ResourceDesc { get; set; }

        [Display(Name = "Browse File")]
        public List<IFormFile> Files { get; set; }          //uploaded files go here

        public List<Documents> ListFiles { get; set; }         //already uploaded files gets populated here

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }

        public TaskCreateModel MapToEntity (Task model)
        {

            var viewModel = new TaskCreateModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                StatusId = model.StatusId,
                CreatedDate = model.CreatedDate,
                ModifiedDate = model.ModifiedDate,
                ProjectId = model?.ProjectId ?? 0,
                EffortId = model.EffortId,
                ResourceId = model.ResourceId,
            };

            return viewModel;
        }




    }
}
