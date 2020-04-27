using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Interfaces;
using ProjectTracker.ViewModels;

namespace ProjectTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProjectInteractor _projectInteractor;
        private readonly IResourceInteractor _resourceInteractor;

        public HomeController(IProjectInteractor projectInteractor, IResourceInteractor resourceInteractor)
        {
            this._projectInteractor = projectInteractor;
            this._resourceInteractor = resourceInteractor;
        }

        public IActionResult Index()
        {
            var projects = _projectInteractor.GetProjectList(false);
            var resources = _resourceInteractor.GetListResources();

            var viewModel = new DashBoardModel()
            {
                ProjectLists = projects,
                ResourceLists = resources
            };

            return View(viewModel);
        }
    }
}