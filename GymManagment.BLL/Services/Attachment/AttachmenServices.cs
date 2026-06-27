using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.BLL.Services.Attachment
{
    public class AttachmenServices : IAttachmenServices
    {
        private readonly long _maxFileSize = 5 * 1024 * 1024;
        private readonly ILogger<AttachmenServices> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtenstions = { ".jpg", ".jpeg", ".png", ".gif" };
        // stream : group of pixels , files ,audios , vides

        public AttachmenServices(ILogger<AttachmenServices> logger ,IWebHostEnvironment env)
        {
            _logger = logger;
          _env = env;
        }

        public  bool Delete(string fileName, string folderName)
        {
            var fullPath = Path.Combine(_env.ContentRootPath , folderName, fileName);
            try
            {

             if (!File.Exists(fullPath)) return false; 

              File.Delete(fullPath);
                return true;
            }
            catch (Exception ex )
            {
                _logger.LogError(ex, $"Faile To Delete Attachmet {fileName}");
                throw;
            }


        }

        public (Stream stream, string contentType)? GetFile(string fileName, string folderName)
        {
            if(string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName)) return null;

            var fullpath = Path.Combine(_env.ContentRootPath,folderName, fileName);
            if(!File.Exists(fullpath)) return null;

            var stream = new FileStream(fullpath, FileMode.Open, FileAccess.Read);
            var extention = Path.GetExtension(fullpath).ToLower();

            var contentType = extention switch
            {
                ".png" => "image/png",
                ".jpg" or "jpeg" => "image/jpeg",
                _=> "application/octet-stream"
            };

            return (stream, contentType);
        }

        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            // validation for size , exixt , read 
            if (fileStream == null || !fileStream.CanRead) return null;

            if (fileStream.Length == 0) return null;

            if (fileStream.Length > _maxFileSize )
            {
                _logger.LogError($"File rejected Too large {fileStream.Length} Bytes");
                return null;
            }
            // extenstion for file

            var extenstion = Path.GetExtension( fileName );
            if(string.IsNullOrWhiteSpace( extenstion) || !_allowedExtenstions.Contains(extenstion))
              {
                _logger.LogError($"File rejected Too large {extenstion} Not Allowed");
                return null;
            }; 
            var uploadFolder = Path.Combine(_env.ContentRootPath, folderName);
            Directory.CreateDirectory( uploadFolder );

            var storedFile = $"{Guid.NewGuid()}_{fileName}";

            var filePath = Path.Combine( uploadFolder, storedFile);
            try
            {

               using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

               await fileStream.CopyToAsync(fs, ct);
                return storedFile;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex,$"failed to upload file {fileName}");
                return null;
            }








        }


    }
}
