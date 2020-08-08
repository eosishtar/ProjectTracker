using Microsoft.EntityFrameworkCore;
using ProjectTracker.Entities;
using ProjectTracker.Interfaces;
using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPT = ProjectTracker.Entities.Task;

namespace ProjectTracker.Interactors
{
    public class TaskInteractor : ITaskInteractor
    {
        private readonly PTContext _db;
        private readonly FileUploadManager _fileManager;
        private readonly IEmailInteractor _emailServer;

        public TaskInteractor(PTContext db, FileUploadManager fileManager, IEmailInteractor emailInteractor)
        {
            this._db = db;
            this._fileManager = fileManager;
            this._emailServer = emailInteractor;
        }

        public List<TaskPT> GetProjectTasks(int? projectId)
        {
            return _db.Task.Include(x => x.Project)
                           .Include(x => x.Effort)
                           .Include(x => x.Status)
                           .Where(x => x.ProjectId == projectId).OrderBy(x => x.CreatedDate).ToList();
        }

        public bool TaskExists(string taskName)
        {
            return _db.Task.Any(x => x.Name == taskName);
        }

        public (bool, string) CreateTask(TaskPT task)
        {
            if (!TaskExists(task.Name))
            {
                var now = DateTime.Now;

                task.StatusId = (int)StatusId.New;
                task.CreatedDate = now;
                task.ModifiedDate = now;

                _db.Task.Add(task);
                _db.SaveChanges();

                return (true, null);
            }

            return (false, $"A task '{task.Name}' already exists.");
        }


        public (bool, string) UpdateTask(int id, TaskPT task)
        {
            var result = _db.Task.Where(x => x.Id == id).FirstOrDefault();

            if (result != null)
            {
                //result.Project = task.ProjectId       //shouldnt be able to switch the task to another project
                result.Name = task.Name;
                result.Description = task.Description;
                result.StatusId = task.StatusId;
                result.ModifiedDate = DateTime.Now;
                result.EffortId = task.EffortId;

                _db.Task.Update(result);

                if (task.Documents.Count > 0)
                {
                    _db.Documents.UpdateRange(task.Documents);
                }
                _db.SaveChanges();

                //the resource has changed, notify them
                if (result.ResourceId != task.ResourceId)
                {
                    var email = new EmailModel()
                    {
                        ToEmails = new string[] { "mlang1986@gmail.com" },
                        Message = "You have been assigned a new task"
                    };

                    _emailServer.SendEmail(email);
                }

                return (true, null);
            }

            return (false, $"Task id '{task.Id}' could not be found.");
        }

        public TaskPT GetTaskDetails(int? id)
        {
            if (id != null)
            {
                return _db.Task.Include(x => x.Resource)
                               .Include(x => x.Documents)
                               .Include(x => x.Status)
                               .Where(x => x.Id == id).FirstOrDefault();
            }

            return null;
        }

        public Task<bool> DeleteAttachment(int id)
        {
            var fileName = _db.Documents.Include(x => x.Task)
                                        .Where(x => x.Id == id)
                                        .AsNoTracking()     //useful when the results are used in a read-only scenario
                                        .FirstOrDefault();

            if (fileName != null)
            {
                //delete from database
                var file = new Documents { Id = id };
                _db.Remove(file);
                _db.SaveChanges();

                //delete actual file
                _fileManager.DeleteFile(fileName.Task.ProjectId, fileName.FileName);

                return System.Threading.Tasks.Task.FromResult(true);
            }

            return System.Threading.Tasks.Task.FromResult(false);
        }
    }
}
