namespace Marketplace.Web.Services.Media;

public interface IPhotoStorageService
{
    Task<string> SaveAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);
    void Delete(string url);
}
