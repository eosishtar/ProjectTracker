using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Entities;
using ProjectTracker.Interfaces;
using ProjectTracker.ViewModels;
using System;
using System.Collections.Generic;

namespace ProjectTracker.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IResourceInteractor _interactor;

        public ResourceController(IResourceInteractor interactor)
        {
            this._interactor = interactor;
        }

        public IActionResult Index()
        {
            var result = _interactor.GetListResources();

            var viewModel = new ResourceListViewModel().MapToEntity(result);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ResourceCreateUpdateModel()
            {
                IsActive = true
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name, Description, ContactNumber, EmailAddress, IsActive")] ResourceCreateUpdateModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var model = new Resource()
                {
                    Id = 0,
                    Name = viewModel.Name,
                    Title = viewModel.Description,
                    ContactNumber = viewModel.ContactNumber,
                    EmailAddress = viewModel.EmailAddress,
                    IsActive = viewModel.IsActive
                };

                var result = _interactor.CreateResource(model);

                if (result.Item1 == true)
                {
                    TempData["success"] = $"'{viewModel.Description}' was added!";
                    return RedirectToAction("Index");
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
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, [Bind("Id, Name, Description, ContactNumber, EmailAddress, IsActive")] ResourceCreateUpdateModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var model = new Resource()
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    Title = viewModel.Description,
                    ContactNumber = viewModel.ContactNumber,
                    EmailAddress = viewModel.EmailAddress,
                    IsActive = viewModel.IsActive
                };

                var result = _interactor.UpdateResource(viewModel.Id, model);

                if (result.Item1 == true)
                {
                    TempData["success"] = $"'{viewModel.Description}' was updated!";
                    return RedirectToAction("Index");
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

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = _interactor.GetResourceDetails(id);

            if (resource == null)
            {
                return NotFound();
            }

            var model = new ResourceCreateUpdateModel()
            {
                Id = resource.Id,
                Name = resource.Name,
                Description = resource.Title,
                ContactNumber = resource.ContactNumber,
                EmailAddress = resource.EmailAddress,
                IsActive = resource.IsActive
            };
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete([FromBody]int id)
        {
            try
            {
                var result = _interactor.DeleteResource((int)id);
                
                if (result.Item1 == true)
                {
                    TempData["success"] = $"Resource successfully updated!";
                    return Json(new { isok = true, message = "Resource successfully updated!"});
                }
                else
                {
                    TempData["fail"] = $"Error: {result.Item2}";
                    return Json(new { isok = false, message = $"Error: {result.Item2}" });
                }
            }
            catch (Exception ex)
            {
                TempData["fail"] = $"Error: {ex.Message}";
                return View();
            }
        }

    }
}