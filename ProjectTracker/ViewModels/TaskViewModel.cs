using ProjectTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectTracker.ViewModels
{
    public class TaskViewModel : SideMenu
    {
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<Task> InProgressTask { get; set; }
        public List<Task> CompletedTask { get; set; }

        public decimal? CompletedProgress { get; set; }

        public DateTime? DueDate { get; set; }

    }
}
