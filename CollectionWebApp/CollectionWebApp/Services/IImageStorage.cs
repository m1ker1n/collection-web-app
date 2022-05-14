namespace CollectionWebApp.Services
{
    public interface IImageStorage
    {
        public string ImagePlaceholderUrl { get; init; }
        public Task<string> UploadFileAsync(IFormFile file, string fileName);
    }
}
