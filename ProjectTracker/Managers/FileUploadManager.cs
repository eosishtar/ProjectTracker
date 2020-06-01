using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using ProjectTracker.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker
{
    public class FileUploadManager
    {
        private readonly Settings _settings;

        public FileUploadManager(Settings settings)
        {
            this._settings = settings;
        }

        public async Task<IEnumerable<string>> UploadDocuments(List<IFormFile> documents, int projectId)
        {
            //https://stackoverflow.com/questions/17128196/file-upload-as-part-of-form-with-other-fields/17128313
            var result = new List<string>();

            if (documents.Count() > 0)
            {
                foreach (IFormFile item in documents)
                {
                    if (item.Length > 0)
                    {
                        CreateDirectory(Path.Combine(_settings.DocumentsPath, projectId.ToString()));

                        string fileName = Path.GetFileName(item.FileName);
                        string fileType = Path.GetExtension(item.FileName);
                        string fullPath = Path.Combine(_settings.DocumentsPath, projectId.ToString(), fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            //save file to location
                            await item.CopyToAsync(stream);
                        }

                        result.Add(fullPath);
                    }
                }
            }

            return result;
        }

        private void CreateDirectory(string fullPath)
        {
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        public List<IFormFile> GetFiles(int projectId, string[] files)
        {
            var listFiles = new List<IFormFile>();

            foreach (var item in files)
            {
                using (var stream = File.OpenRead(Path.Combine(_settings.DocumentsPath, projectId.ToString(), item)))
                {
                    var ext = Path.GetExtension((Path.Combine(_settings.DocumentsPath, projectId.ToString(), item)).ToLowerInvariant());

                    var file = new FormFile(stream, 0, stream.Length, stream.Name, Path.GetFileName(stream.Name))
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = GetContentType()[ext]
                    };

                    listFiles.Add(file);
                }
            }

            return listFiles;
        }

        public Dictionary<string, string> GetContentType()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
            };
        }

        public void DeleteFile(int projectId, string fileName)
        {
            string path = Path.Combine(_settings.DocumentsPath, projectId.ToString(), fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

}
