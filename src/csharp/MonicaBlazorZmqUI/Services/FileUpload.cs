using BlazorInputFile;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
namespace MonicaBlazorZmqUI.Services
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string UploadPath;

        public FileUpload(IWebHostEnvironment env)
        {
            _environment = env;
            UploadPath = Path.Combine(_environment.ContentRootPath, "Upload");
        }

        public void DeleteExistingFiles()
        {
            var files = Directory.GetFiles(UploadPath);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        public async Task<string> UploadAsync(IFileListEntry fileEntry)
        {
            var filePath = Path.Combine(UploadPath, fileEntry.Name);
            var ms = new MemoryStream();
            await fileEntry.Data.CopyToAsync(ms);

            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                ms.WriteTo(file);
            }

            return filePath;
        }

        public async Task<string> GetFileContentAsync(string uriString)
        {
            // Lesson learnt - always check for a valid URI
            if (Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
            {
                Uri uri = new Uri(uriString);
                string filePath = _environment.WebRootPath + uri.LocalPath.Replace("/","\\");
                if (File.Exists(filePath))
                {
                    var fileContent = File.ReadAllText(filePath);
                    return fileContent;
                }
            }

            return null;
        }
    }
}