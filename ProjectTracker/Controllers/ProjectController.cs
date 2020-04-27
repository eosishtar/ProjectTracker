using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Entities;
using ProjectTracker.Interfaces;
using ProjectTracker.Models;
using ProjectTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProjectTracker.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectInteractor _projectInteractor;

        public ProjectController(IProjectInteractor projectInteractor)
        {
            this._projectInteractor = projectInteractor;
        }

        public IActionResult Index([FromQuery]string search)
        {
            var projects = new List<Entities.Project>();

            if (string.IsNullOrEmpty(search))
            {
                projects = _projectInteractor.GetProjectList();
            }
            else
            {
                projects = _projectInteractor.SearchProjectList(search);
            }

            //TempData.Remove("success");
            return View(projects);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("Name", "Description", "IsActive", "DueDate")] ProjectPostModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["fail"] = "An error occurred.";
                return View(this.Read(viewModel));
            }

            if (viewModel.DueDate <= DateTime.Now)
            {
                TempData["fail"] = "Due date cannot be set before or on today.";
                return View(this.Read(viewModel));
            }

            try
            {
                var model = new Project()
                {
                    Id = 0,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    IsActive = viewModel.IsActive,
                    DueDate = viewModel.DueDate,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                //save to database
                var result = _projectInteractor.CreateProject(model);
                TempData.Remove("fail");

                if (result.Item1 != null)
                {
                    TempData["success"] = $"'{viewModel.Name}' was created!";
                    return RedirectToAction("Index", "Home", "success");
                }

                TempData["fail"] = $"Error: {result.Item2}";
                return View();

            }
            catch (Exception ex)
            {
                TempData["fail"] = $"Error: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            if (id < 1)
                return NotFound();

            try
            {
                var model = _projectInteractor.GetProjectDetails(id) ?? new Project();
                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Name", "Description", "IsActive", "DueDate")] ProjectPostModel viewModel)
        {

            if (id < 1)
                return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["fail"] = "An error occurred.";
                return View(this.Read(viewModel));
            }

            TempData.Remove("fail");

            try
            {
                var model = new Project()
                {
                    Id = id,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    IsActive = viewModel.IsActive,
                    DueDate = viewModel.DueDate,
                    ModifiedDate = DateTime.Now
                };

                var result = _projectInteractor.UpdateProject(id, model);
                if (result.Item1 != null)
                {
                    TempData["success"] = $"'{viewModel.Name}' was saved!";
                }

                return RedirectToAction("Index", "Home", "success");
            }
            catch (Exception ex)
            {
                TempData["fail"] = $"Error: {ex.Message}";
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        private Project Read(ProjectPostModel model)
        {
            var viewModel = new Project()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                DueDate = model.DueDate,
                IsActive = model.IsActive
            };

            return viewModel;
        }
    }
}
