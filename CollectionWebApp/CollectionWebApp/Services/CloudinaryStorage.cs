using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CollectionWebApp.Services
{
    public class CloudinaryStorage : IImageStorage
    {
        public string ImagePlaceholderUrl { get; init; }

        private readonly string CloudName = null!;
        private readonly string ApiKey = null!;
        private readonly string ApiSecret = null!;

        private Account account = null!;
        private Cloudinary cloudinary = null!;


        public CloudinaryStorage(IConfiguration configuration)
        {
            var section = configuration.GetSection("Cloudinary");
            ApiKey = section.GetValue<string>("ApiKey");
            ApiSecret = section.GetValue<string>("ApiSecret");
            CloudName = section.GetValue<string>("CloudName");
            ImagePlaceholderUrl = section.GetValue<string>("ImagePlaceholderUrl");
            
            account = new Account(CloudName, ApiKey, ApiSecret);
            cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream)
                };
                var result = await cloudinary.UploadAsync(uploadParams);
                return result.Url.AbsoluteUri;
            }
        }
    }
}
