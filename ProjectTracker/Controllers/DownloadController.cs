using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Entities;
using ProjectTracker.Interfaces;
using ProjectTracker.Models;

namespace ProjectTracker.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITaskInteractor _taskInteractor;
        private readonly Settings _settings;
        private readonly FileUploadManager _fileManager;

        public DownloadController(IHostingEnvironment hostingEnvironment, ITaskInteractor taskInteractor, Settings settings, FileUploadManager fileManager)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._taskInteractor = taskInteractor;
            this._settings = settings;
            this._fileManager = fileManager;
        }


        public IActionResult DownloadFile([FromBody]DownloadFileModel model)
        {
            try
            {
                var file = _taskInteractor.GetTaskDetails(model.TaskId);

                if (file == null)
                {
                    return null;
                }

                if (file.Documents.Count() == 0)
                {
                    return null;
                }

                var fileDetails = file.Documents.Where(x => x.Id == model.ItemId).FirstOrDefault();
                string filepath = Path.Combine(_settings.DocumentsPath, fileDetails.Task.ProjectId.ToString(), fileDetails.FileName);

                if (!System.IO.File.Exists(filepath))
                {
                    throw new ArgumentException("Invalid file name or file does not exist!");
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                var fs = new FileStream(filepath, FileMode.Open);
                var ms = new MemoryStream();
                ms.CopyTo(fs);


                var ext = Path.GetExtension(filepath).ToLowerInvariant();
                string contentType = _fileManager.GetContentType()[ext];

                Response.ContentType = contentType;
                Response.Headers.Add("Content-Disposition", "attachment; filename=" + fileDetails.FileName);

                //return File(memory, contentType);
                //return File(fileBytes, "application/force-download", fileDetails.FileName);
                return PhysicalFile(filepath, "application/force-download", fileDetails.FileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }



    }
}