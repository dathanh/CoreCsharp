using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ServiceLayer.Interfaces
{
    public interface ITempUploadFileService
    {
        Task<string> SaveFile(IFormFile file, int imageType = 0);

        void RemoveFile(string file, int imageType = 0);

        string FilePath { get; set; }
    }
}