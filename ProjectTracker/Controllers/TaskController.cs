using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using ProjectTracker.Entities;
using ProjectTracker.Helpers;
using ProjectTracker.Interfaces;
using ProjectTracker.Managers;
using ProjectTracker.Models;
using ProjectTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaskPT = ProjectTracker.Entities.Task;

namespace ProjectTracker.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskInteractor _interactor;
        private readonly IProjectInteractor _projectInteractor;
        private readonly FileUploadManager _fileUpload;
        private readonly CacheManager _cacheHelper;
        private readonly IMemoryCache _cache;

        private bool clearData = true;

        public TaskController(ITaskInteractor interactor, IProjectInteractor projectInteractor, 
            FileUploadManager fileUpload, CacheManager cacheHelper, IMemoryCache cache)
        {
            this._interactor = interactor;
            this._projectInteractor = projectInteractor;
            this._fileUpload = fileUpload;
            this._cacheHelper = cacheHelper;
            this._cache = cache;
        }

        // GET: Task
        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _interactor.GetProjectTasks((int)id);

            var inProgTasks = new List<TaskPT>();
            var completedTasks = new List<TaskPT>();
            var activeProjects = _projectInteractor.GetProjectList() ?? new List<Project>();

            if (result.Count == 0)
            {
                var projName = _projectInteractor.GetProjectDetails((int)id);

                var emptyModel = new TaskViewModel()
                {
                    InProgressTask = inProgTasks,
                    CompletedTask = completedTasks,
                    ProjectId = (int)id,
                    ProjectName = projName?.Name,
                    CompletedProgress = 0,
                    DueDate = null,
                    projectLists = activeProjects
                };

                return View(emptyModel);
            }

            foreach (var item in result)
            {
                //truncate the desc for the view
                if (item.Name.Length > 20)
                {
                    item.Name= string.Concat(item.Name.Substring(0, 20), "...");
                }

                if (item.Description != null)
                {
                    if (item.Description.Length > 20)
                    {
                        item.Description = string.Concat(item.Description.Substring(0, 20), "...");
                    }
                }

                if (item.StatusId == (int)StatusId.Done)
                {
                    completedTasks.Add(item);
                }
                else
                {
                    inProgTasks.Add(item);
                }
            }

            //get values for progress bar
            var queryable = result.AsQueryable();
            var totalProjectMinutes = queryable.Sum(x => x.Effort.ValueInMinutes);
            var completedProjectMiniutes = queryable.Where(x => x.StatusId == (int)StatusId.Done).Sum(x => x.Effort.ValueInMinutes);
            var currentProgressValue = completedProjectMiniutes == 0 ? 0 : Math.Ceiling((completedProjectMiniutes / totalProjectMinutes) * 100);   //round the value

            var viewModel = new TaskViewModel()
            {
                InProgressTask = inProgTasks,
                CompletedTask = completedTasks,
                ProjectId = result[0].Project?.Id,
                ProjectName = result[0].Project?.Name,
                CompletedProgress = currentProgressValue,
                DueDate = result[0].Project?.DueDate,
                projectLists = activeProjects
            };

            return View(viewModel);
        }



        // GET: Task/Create
        public IActionResult Create(int id)
        {
            var model = this.GetNewTaskModel(id);
            this.AssignLookUps(_projectInteractor.GetProjectName(model.ProjectId), model);
            model.StatusId = (int)StatusId.New;

            return View(model);
        }


        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ProjectId,Name,Description,EffortId,ResourceId,StatusId,Files")] TaskCreateModel viewModel)
        {

            try
            {
                if (!ModelIsValid(viewModel))
                {
                    this.AssignLookUps(_projectInteractor.GetProjectName(viewModel.ProjectId), viewModel);

                    return View(viewModel);
                }

                var model = new TaskPT()
                {
                    //Id = 0,
                    ProjectId = viewModel.ProjectId,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    EffortId = (int)viewModel.EffortId,
                    StatusId = viewModel.StatusId,
                    ResourceId = (int)viewModel.ResourceId
                };

                //Save files to database
                var documents = new List<Documents>();

                if (viewModel.Files.Count() > 0)
                {
                    foreach (IFormFile item in viewModel.Files)
                    {
                        if (item.Length > 0)
                        {
                            string fileName = Path.GetFileName(item.FileName);
                            string fileType = Path.GetExtension(item.FileName);

                            documents.Add(new Documents()
                            {
                                FileName = fileName,
                                FileType = fileType,
                                Task = model,
                                TaskId = model.Id,
                            });
                        }
                    }

                    //link documents to model
                    model.Documents = documents;
                }

                var result = _interactor.CreateTask(model);
                if (result.Item1 == true)
                {
                    var files = _fileUpload.UploadDocuments(viewModel.Files, viewModel.ProjectId).Result;

                    TempData["success"] = $"'{viewModel.Name}' was added!";
                    return RedirectToAction("Index", "Task", new { id = viewModel.ProjectId });
                }

                TempData.Remove("fail");
                TempData["fail"] = $"Error: {result.Item2}";
                return View();
            }
            catch (Exception ex)
            {
                TempData["fail"] = $"Error: {ex.Message}";
                return View();
            }
        }


        [NonAction]
        private TaskCreateModel GetNewTaskModel(int projectId)
        {
            if (clearData)
            {
                TempData.Remove("fail");
                TempData.Remove("success");
            }

            var model = new TaskCreateModel();

            model.ProjectId = projectId;
            model.StatusId = (int)StatusId.New;

            return model;
        }

        [NonAction]
        private bool ModelIsValid(TaskCreateModel model)
        {
            TempData.Remove("fail");

            if (model.EffortId <= 0 || model.EffortId == null)
            {
                TempData["fail"] = "Please select the effort that is required.";
                return false;
            }
            ModelState.Remove("EffortDesc");

            if (model.ResourceId <= 0 || model.ResourceId == null)
            {
                TempData["fail"] = "Please select the resource that is required.";
                return false;
            }
            ModelState.Remove("ResourceDesc");

            if (!ModelState.IsValid)
            {
                TempData["fail"] = "Please complete all fields.";
                return false;
            }

            return true;
        }


        // GET: Task/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = _interactor.GetTaskDetails(id);

            if (task == null)
            {
                return NotFound();
            }

            var model = GetNewTaskModel(task.ProjectId);
            model.MapToEntity(task);
            model.Id = task.Id;
            model.Name = task.Name;
            model.Description = task?.Description;
            model.EffortId = task.EffortId;
            model.ResourceId = task.ResourceId;
            model.CreatedDate = task.CreatedDate;
            model.ModifiedDate = task.ModifiedDate;
            model.StatusId = task.StatusId;

            model.ListFiles = (task.Documents.Count > 0) ? task.Documents.ToList() : new List<Documents>();

            this.AssignLookUps(_projectInteractor.GetProjectName(model.ProjectId), model);

            return View(model);
        }



        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ProjectId,Name,Description,EffortId,ResourceId,StatusId,Files")] TaskCreateModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            try
            {
                if (!ModelIsValid(viewModel))
                {
                    this.AssignLookUps(_projectInteractor.GetProjectName(viewModel.ProjectId), viewModel);

                    return View(viewModel);
                }

                var model = new TaskPT()
                {
                    Id = viewModel.Id,
                    ProjectId = viewModel.ProjectId,
                    Name = viewModel.Name,
                    Description = viewModel.Description?.TruncateAndTrim(255),
                    EffortId = (int)viewModel.EffortId,
                    StatusId = viewModel.StatusId,
                    ResourceId = (int)viewModel.ResourceId
                };

                //Save files to database
                var documents = new List<Documents>();

                if (viewModel.Files.Count() > 0)
                {
                    foreach (IFormFile item in viewModel.Files)
                    {
                        if (item.Length > 0)
                        {
                            string fileName = Path.GetFileName(item.FileName);
                            string fileType = Path.GetExtension(item.FileName);

                            documents.Add(new Documents()
                            {
                                FileName = fileName,
                                FileType = fileType,
                                Task = model,
                                TaskId = viewModel.Id,
                            });
                        }
                    }

                    //link documents to model
                    model.Documents = documents;
                }

                var result = _interactor.UpdateTask(id, model);
                if (result.Item1 == true)
                {
                    var files = _fileUpload.UploadDocuments(viewModel.Files, viewModel.ProjectId).Result;

                    TempData["success"] = $"'{viewModel.Name}' was updated!";
                    return RedirectToAction("Index", "Task", new { id = viewModel.ProjectId });
                }

                TempData.Remove("fail");
                TempData["fail"] = $"Error: {result.Item2}";
                return View();
            }
            catch (Exception ex)
            {
                TempData["fail"] = $"Error: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody]int id)
        {
            try
            {
                var res = await this._interactor.DeleteAttachment(id);
                if (res == true)
                {
                    clearData = false;
                    TempData["success"] = "Attachment successfully deleted";
                    return Json(new { isok = true, message = "Attachment successfully deleted" });
                }
                else
                {
                    TempData["fail"] = "An error occured when deleting the attachment";
                    return Json(new { isok = false, message = "An error occured when deleting the attachment" });
                }
            }
            catch (Exception ex)
            {
                return this.RedirectToAction("Error", "Home");
            }
        }

        [NonAction]
        public async Task<TaskCreateModel> AssignLookUps(IQueryable<Project> projects, TaskCreateModel model)
        {
            LookUp lookups = null;
            try
            {
                lookups = await _cacheHelper.CheckLookupCache(_cache);
            }
            catch (Exception ex)
            {
                var error = ex;
            }

            model.EffortDesc = lookups.Efforts;
            model.ResourceDesc = lookups.Resources;
            model.StatusDesc = lookups.Statuses;

            model.ProjectName = projects
                .Select(y => new SelectListItem
                {
                    Value = y.Id.ToString(),
                    Text = $"{y.Description}"
                });


            return model;
        }


    }
}
