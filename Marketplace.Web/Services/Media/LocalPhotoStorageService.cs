namespace Marketplace.Web.Services.Media;

public sealed class LocalPhotoStorageService : IPhotoStorageService
{
    private static readonly HashSet<string> AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private const long MaxBytes = 10 * 1024 * 1024; // 10 MB

    private readonly IWebHostEnvironment _env;

    public LocalPhotoStorageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
    {
        if (file.Length == 0) throw new ArgumentException("File is empty.");
        if (file.Length > MaxBytes) throw new ArgumentException("File exceeds 10 MB limit.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext))
            throw new ArgumentException($"File type '{ext}' is not allowed.");

        var dir = Path.Combine(_env.WebRootPath, "uploads", folder);
        Directory.CreateDirectory(dir);

        var fileName = $"{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(dir, fileName);

        await using var stream = File.Create(fullPath);
        await file.CopyToAsync(stream, cancellationToken);

        return $"/uploads/{folder}/{fileName}";
    }

    public void Delete(string url)
    {
        if (string.IsNullOrWhiteSpace(url) || !url.StartsWith("/uploads/")) return;

        var rel = url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var abs = Path.Combine(_env.WebRootPath, rel);
        if (File.Exists(abs)) File.Delete(abs);
    }
}
