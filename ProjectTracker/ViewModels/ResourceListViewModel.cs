using ProjectTracker.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.ViewModels
{
    public class ResourceListViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Resource Name")]
        public string Name { get; set; }

        public string Title { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }


        public List<ResourceListViewModel> MapToEntity(List<Resource> resources)
        {
            List<ResourceListViewModel> viewModel = new List<ResourceListViewModel>();

            foreach (var resource in resources)
            {
                viewModel.Add(new ResourceListViewModel()
                {
                    Id = resource.Id,
                    Name = resource.Name,
                    Title = resource.Title,
                    IsActive = resource.IsActive
                });
            }

            return viewModel;
        }
    }
}
