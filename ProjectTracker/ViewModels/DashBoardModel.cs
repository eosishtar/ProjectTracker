using ProjectTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.ViewModels
{
    public class DashBoardModel 
    {
        public List<Project> ProjectLists { get; set; }
        public List<Resource> ResourceLists { get; set; }
    }
}
