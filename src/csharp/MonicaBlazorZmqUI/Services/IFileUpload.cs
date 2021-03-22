using BlazorInputFile;
using System.Threading.Tasks;
namespace MonicaBlazorZmqUI.Services
{
    public interface IFileUpload
    {
        void DeleteExistingFiles();

        Task<string> UploadAsync(IFileListEntry file);

        Task<string> GetFileContentAsync(string uriString);
    }
}