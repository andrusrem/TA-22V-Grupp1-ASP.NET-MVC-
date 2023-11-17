using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public class ImageService
    {
        private readonly ApplicationDbContext _context;

        // //public IActionResult Image(int id)
        // {
        //     var path = "/Users/andrusremets/TA-22V-Grupp1/KooliProjekt/Images/" + id + ".jpg";
        //     var stream = System.IO.File.OpenRead(path);
        //     return File(stream, "image/jpeg");
        // }

        public string GetImagesDir()
        {
            return System.IO.Path.Join( System.IO.Directory.GetCurrentDirectory(), "Images");
        }

        public string GetImagePath(int Id)
        {
            return System.IO.Path.Join(GetImagesDir(), Id + ".jpg");
        }

        public FileStream ReadImage(int Id)
        {
            return System.IO.File.OpenRead(GetImagePath(Id));
        }

        public async Task WriteImage(int Id, System.IO.Stream stream)
        {
            using(var fileStream = new FileStream(GetImagePath(Id), FileMode.CreateNew))
            {
                await stream.CopyToAsync(fileStream);
            }
        }

        public async Task UpdateImage(int Id, System.IO.Stream stream)
        {
            using(var fileStream = new FileStream(GetImagePath(Id), FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}
    