using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using Infrastructure.EF.Entities;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System.IO;

namespace WebApi.Utilities
{
    public static class ImageManagement
    {
        public static async void SaveImage(UserEntity user, IFormFile image, string FileName, IWebHostEnvironment hostEnvironment)
        {
            string dir = Path.Combine(hostEnvironment.ContentRootPath, "Uploads", user.UserName);
            string mini = "mini" + FileName;
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, FileName + ".png");
            string path2 = Path.Combine(dir, mini + ".png");
            Size size = new Size(300, 300);
            ISupportedImageFormat format = new PngFormat { Quality = 70 };
            //using (Stream fileStream = new FileStream(path, FileMode.Create))
            //{
            //    await image.CopyToAsync(fileStream);
            //}
            using (Stream imageStream = image.OpenReadStream())
            {
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                {
                    imageFactory.Load(imageStream)
                                .Save(path);
                    imageFactory.Load(imageStream)
                                .Resize(size)
                                .BackgroundColor(Color.Transparent)
                                .Format(format)
                                .Save(path2);
                }
            }
        }
        public static void DeleteImage(UserEntity user, IEnumerable<string> FileNames, IWebHostEnvironment hostEnvironment)
        {
            string dir = Path.Combine(hostEnvironment.ContentRootPath, "Uploads", user.UserName);
            Directory.CreateDirectory(dir);
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                foreach (var item in FileNames)
                {
                    if (file.Contains(item))
                        System.IO.File.Delete(file);
                    if (file.Contains("mini" + item))
                        System.IO.File.Delete(file);
                }
            }
        }
    }
}
