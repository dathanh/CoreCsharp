using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Framework.DomainModel.Entities.Common;
using Framework.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class TempUploadFileService : ITempUploadFileService
    {
        private readonly IHostingEnvironment _env;
        private readonly IResizeImage _resizeImage;
        private readonly IConfiguration _configuration;

        private const string bucketName = "ompalo";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;
        private static IAmazonS3 s3Client;

        public TempUploadFileService(IHostingEnvironment env, IResizeImage resizeImage, IConfiguration configuration)
        {
            _env = env;
            _resizeImage = resizeImage;
            _configuration = configuration;
        }
        public async Task<string> SaveFile(IFormFile file, int imageType = 0)
        {

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = _env.WebRootPath + FilePath;
            var filePath = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (file.ContentType.ToLower().StartsWith("image/"))
            {
                Image img = Image.FromStream(file.OpenReadStream(), true, true);
                ImageHelper.RotateImageByExifOrientationData(img);
                img.Save(filePath, ImageFormat.Jpeg);
            }
            else
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            CloneFileForCategory(filePath, imageType);
            await UploadImageToS3(filePath, fileName);
            DeleteCloneFileWithType(filePath, imageType);

            return fileName;
        }

        private void CloneFileForCategory(string filePath, int type)
        {
            if (type == (int)UploadImageType.VideoFile)
            {
                return;
            }
            var width = 200;
            switch (type)
            {
                case (int)UploadImageType.CategoryImage:
                    width = 200;
                    break;
                case (int)UploadImageType.BackgroundTopBanner:
                    width = 1920;
                    break;
                case (int)UploadImageType.VideoImage:
                    width = 400;
                    break;
                case (int)UploadImageType.SeriesImage:
                    width = 1300;
                    break;
                //
                case (int)UploadImageType.CommingSoon:
                    width = 354;
                    break;
                case (int)UploadImageType.CategoryBackground:
                    width = 400;
                    break;
                case (int)UploadImageType.PlaylistBackground:
                    width = 800;
                    break;
                case (int)UploadImageType.ConfigBackground:
                    width = 1920;
                    break;
            }

            var originFileName = Path.GetFileNameWithoutExtension(filePath);
            var originExt = Path.GetExtension(filePath);
            var path = Path.GetDirectoryName(filePath);
            var saveFileName = originFileName + originExt;
            var saveFile = Path.Combine(path, saveFileName);
            _resizeImage.ResizeImageByWidthAndSave(filePath, width, saveFile);
            //var smallFileName = originFileName + "_small" + originExt;
            //var smallFile = Path.Combine(path, smallFileName);
            //_resizeImage.ResizeImageByWidthAndSave(filePath, 200, smallFile);
            //var mediumFileName = originFileName + "_medium" + originExt;
            //var mediumFile = Path.Combine(path, mediumFileName);
            //_resizeImage.ResizeImageByWidthAndSave(filePath, 900, mediumFile);
            //var largeFileName = originFileName + "_large" + originExt;
            //var largeFile = Path.Combine(path, largeFileName);
            //_resizeImage.ResizeImageByWidthAndSave(filePath, 1400, largeFile);
        }

        public async void RemoveFile(string file, int imageType = 0)
        {
            if (!string.IsNullOrWhiteSpace(file))
            {
                var path = _env.WebRootPath + FilePath;
                var fileName = Path.GetFileName(file);

                var filePath = Path.Combine(path, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    await DeleteFileInS3("UploadTemp/" + fileName);
                }

                DeleteCloneFileWithType(filePath, imageType);
            }
        }

        private void DeleteCloneFileWithType(string filePath, int type)
        {
            var originFileName = Path.GetFileNameWithoutExtension(filePath);
            var originExt = Path.GetExtension(filePath);
            var path = Path.GetDirectoryName(filePath);
            var saveFileName = originFileName + originExt;
            var saveFile = Path.Combine(path, saveFileName);

            if (File.Exists(saveFile))
            {
                File.Delete(saveFile);
            }
            //var smallFileName = originFileName + "_small" + originExt;
            //var smallFile = Path.Combine(path, smallFileName);

            //if (File.Exists(smallFile))
            //{
            //    File.Delete(smallFile);
            //}

            //var mediumFileName = originFileName + "_medium" + originExt;

            //if (File.Exists(mediumFileName))
            //{
            //    File.Delete(mediumFileName);
            //}

            //var largeFileName = originFileName + "_large" + originExt;

            //if (File.Exists(largeFileName))
            //{
            //    File.Delete(largeFileName);
            //}
        }

        public string FilePath { get; set; }
        private async Task UploadImageToS3(string filePath, string fileName)
        {
            s3Client = new AmazonS3Client(_configuration["AwsAccessKey"], _configuration["AwsSecretKey"], bucketRegion);

            var fileTransferUtility = new TransferUtility(s3Client);
            var fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                FilePath = filePath,
                CannedACL = S3CannedACL.PublicRead,
                Key = "UploadTemp/" + fileName,
            };

            await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
        }
        private async Task DeleteFileInS3(string fileName)
        {
            s3Client = new AmazonS3Client(_configuration["AwsAccessKey"], _configuration["AwsSecretKey"], bucketRegion);
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            await s3Client.DeleteObjectAsync(deleteObjectRequest);
        }
    }
}
