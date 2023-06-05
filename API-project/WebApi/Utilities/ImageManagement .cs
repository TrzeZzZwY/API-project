using Infrastructure.EF.Entities;
using Microsoft.Extensions.Hosting;

namespace WebApi.Utilities
{
    public static class ImageManagement
    {
        public static async void SaveImage(UserEntity user, IFormFile image, string FileName, IWebHostEnvironment hostEnvironment)
        {
            string dir = Path.Combine(hostEnvironment.ContentRootPath, "Uploads", user.UserName);
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, FileName + Path.GetExtension(image.FileName));
            using (Stream fileStream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
        }
        public static void DeleteImage(UserEntity user, IEnumerable<string> FileNames, IWebHostEnvironment hostEnvironment)
        {
            string dir = Path.Combine(hostEnvironment.ContentRootPath, "Uploads", user.UserName);
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                foreach (var item in FileNames)
                {
                    if (file.Contains(item))
                        System.IO.File.Delete(file);
                }
            }
        }
    }
}
