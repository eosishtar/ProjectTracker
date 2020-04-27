using ProjectTracker.Entities;
using ProjectTracker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Interactors
{
    public class ProjectInteractor : IProjectInteractor
    {
        private readonly PTContext _db;

        public ProjectInteractor(PTContext db)
        {
            this._db = db;
        }

        public List<Project> GetProjectList(bool activeOnly = true)
        {
            if (activeOnly)
            {
                return _db.Project.Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
            }
            else
            {
                return _db.Project.OrderBy(x => x.Name).ToList();
            }
        }

        public List<Project> SearchProjectList(string search)
        {
            return _db.Project.Where(x => x.Name.StartsWith(search)).OrderBy(x => x.Name).ToList();
        }

        public (Project, string) CreateProject (Project model)
        {
            var projectExist = _db.Project.Any(x => x.Name == model.Name);

            if (!projectExist)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Completed = false;        //this will be auto set once all tasks are done.

                _db.Project.Add(model);
                _db.SaveChanges();

                return (model, null);
            }
            return (null, $"Project '{model.Name}' already exists.");
        }

        public (Project, string) UpdateProject(int id, Project model)
        {
            var result = _db.Project.Where(x => x.Id== id).FirstOrDefault();

            if (result != null)
            {
                result.Name = model.Name;
                result.Description = model.Description;
                result.IsActive = model.IsActive;
                result.DueDate = model.DueDate;
                result.ModifiedDate = DateTime.Now;
                result.Completed = false;

                _db.Project.Update(result);
                _db.SaveChanges();

                return (model, null);
            }
            return (null, $"Project '{model.Name}' not found.");
        }

        public Project GetProjectDetails(int projectId)
        {
            return _db.Project.Where(x => x.Id == projectId).FirstOrDefault();
        }

        public IQueryable<Project> GetProjectName(int projectId)
        {
            return _db.Project
                .Where(x => x.IsActive & x.Id == projectId)
                .OrderBy(x => x.Name)
                .ToList()
                .AsQueryable();
        }
    }
}
