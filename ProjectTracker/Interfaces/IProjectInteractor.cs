using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectTracker.Entities;

namespace ProjectTracker.Interfaces
{
    public interface IProjectInteractor
    {
        List<Project> GetProjectList(bool activeOnly = true);

        List<Project> SearchProjectList(string search);

        Project GetProjectDetails(int id);

        (Project, string) CreateProject(Project model);

        (Project, string) UpdateProject(int id, Project model);

        IQueryable<Project> GetProjectName(int id);
    }
}
