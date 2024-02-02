using KooliProjekt.Data;
namespace KooliProjekt.Services

{
    public interface IImageService
    {
        string GetImagesDir();
        string GetImagePath(int Id);
        Stream ReadImage(int Id);
        Task WriteImage(int Id, System.IO.Stream stream);
        Task UpdateImage(int Id, System.IO.Stream stream);
    }
}
