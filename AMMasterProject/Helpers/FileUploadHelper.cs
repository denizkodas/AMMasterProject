using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using AMMasterProject.ViewModel;
using Azure.Storage.Blobs;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace AMMasterProject.Helpers
{
    public class FileUploadHelper
    {
        private readonly MyDbContext _dbContext;
        private readonly WebsettingHelper _websettinghelper;

        private readonly IWebHostEnvironment _hostingEnvironment;
        public FileUploadHelper(MyDbContext context, WebsettingHelper websettinghelper, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = context;
            _websettinghelper = websettinghelper;
            _hostingEnvironment = hostingEnvironment;

        }

        #region AWSKey



        private static HashSet<string> validExtensions = new HashSet<string>
        {
        ".jpg", ".jpeg", ".gif", ".png", ".pdf", ".docx", ".pptx", ".xlsx",
        ".doc", ".xls", ".ppt", ".mp4", ".ico", "x-icon", "x-ico", ".zip"
        };
        public async Task<string> AwsUpload(IFormFile postedFile)
        {


            string accessKey = "";
            string secretKey = "";
            string bucketName = "";
            string urlPath = "";

            var _awsSettings = _websettinghelper.GetWebsettingJson("AWSSettings");
            if (_awsSettings != null && !string.IsNullOrEmpty(_awsSettings))
            {
                var json = JsonConvert.DeserializeObject<AWSSettingsModel>(_awsSettings);

                if (json != null)
                {
                    accessKey = json.AccessKey.Trim();
                    secretKey = json.SecretKey.Trim();
                    bucketName = json.Bucket.Trim();
                    urlPath = json.URLPath.Trim();
                }
            }

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast2,
                ServiceURL = "https://s3.us-east-2.amazonaws.com"

                
            };

            using (var client = new AmazonS3Client(credentials, config))
            using (var stream = postedFile.OpenReadStream())
            {
                var ext = Path.GetExtension(postedFile.FileName).ToLower();
                //var validExtensions = new HashSet<string> { ".jpg", ".jpeg", ".gif", ".png", ".pdf", ".docx", ".pptx", ".xlsx", ".doc", ".xls", ".ppt", ".mp4", ".ico", "x-icon", "x-ico", ".zip" };

                if (!validExtensions.Contains(ext))
                {
                    return "This file type is not supported.";
                }

                //var uploadFileName = $"{Guid.NewGuid()}{ext}";

                // Concatenate the GUID with the file name (without extension)
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(postedFile.FileName.Replace("_", ""));
                var uploadFileName = $"{postedFile.FileName}_{Guid.NewGuid()}{ext}";

                var request = new PutObjectRequest
                {
                    InputStream = stream,
                    BucketName = bucketName,
                    ContentType = GetContentType(ext),
                    CannedACL = S3CannedACL.PublicRead,
                    Key = uploadFileName
                };

                var response = await client.PutObjectAsync(request);

                return urlPath + uploadFileName;
            }
        }

        #endregion


        #region AzureBlob

        public async Task<string> AzureUpload(IFormFile postedFile)
        {
            string connectionString = "";
            string containerName = "images"; // Replace with your container name
            string urlPath = ""; // Replace with your storage account URL

            //azure
            var _azureSettings = _websettinghelper.GetWebsettingJson("AzureSettings");
            if (_azureSettings != null && !string.IsNullOrEmpty(_azureSettings))
            {
                var json = JsonConvert.DeserializeObject<AzureSettingsModel>(_azureSettings);

                if (json != null)
                {
                    connectionString = json.ConnectionString.Trim();
                    containerName = json.ContainerName.Trim();
                    urlPath = json.URLPath.Trim();
                }
            }

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var ext = Path.GetExtension(postedFile.FileName).ToLower();
            //var validExtensions = new HashSet<string> { ".jpg", ".jpeg", ".gif", ".png", ".pdf", ".docx", ".pptx", ".xlsx", ".doc", ".xls", ".ppt", ".mp4", ".ico", "x-icon", "x-ico", ".zip" };

            if (!validExtensions.Contains(ext))
            {
                return "This file type is not supported.";
            }

            //var uploadFileName = $"{Guid.NewGuid()}{ext}";
            // Concatenate the GUID with the file name (without extension)
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(postedFile.FileName.Replace("_", ""));
            var uploadFileName = $"{postedFile.FileName}_{Guid.NewGuid()}{ext}";

            BlobClient blobClient = containerClient.GetBlobClient(uploadFileName);

            using (var stream = postedFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return urlPath + containerName + "/" + uploadFileName;
        }
        #endregion

        #region local

        public async Task<string> LocalUpload(IFormFile postedFile)
        {
            if (postedFile == null || postedFile.Length == 0)
            {
                return "No file selected.";
            }

            // Define the folder path where you want to save the file
            var folderPath = "cdn";

            // Get the wwwroot path
            var wwwrootPath = _hostingEnvironment.WebRootPath;

            // Combine the wwwroot path and folder path to get the upload folder path
            var uploadFolderPath = Path.Combine(wwwrootPath, folderPath);

            // Create the upload folder if it doesn't exist
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            var ext = Path.GetExtension(postedFile.FileName).ToLower();
            //var validExtensions = new HashSet<string> { ".jpg", ".jpeg", ".gif", ".png", ".pdf", ".docx", ".pptx", ".xlsx", ".doc", ".xls", ".ppt", ".mp4", ".ico", "x-icon", "x-ico", ".zip" };

            if (!validExtensions.Contains(ext))
            {
                return "This file type is not supported.";
            }

            // Create a unique file name
            //var uploadFileName = $"{Guid.NewGuid()}{Path.GetExtension(postedFile.FileName)}";

            //var uploadFileName = $"{Guid.NewGuid()}{ext}";
            // Concatenate the GUID with the file name (without extension)
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(postedFile.FileName.Replace("_", ""));
            var uploadFileName = $"{postedFile.FileName}_{Guid.NewGuid()}{ext}";

            // Combine the upload folder path and file name to get the full file path
            var filePath = Path.Combine(uploadFolderPath, uploadFileName);

            // Save the file to the local folder
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await postedFile.CopyToAsync(fileStream);
            }

            // Combine the folder path and file name to get the return path
            var returnPath = $"/{folderPath}/{uploadFileName}";

            return returnPath;
        }
        #endregion

        #region firebase
        public async Task<string> FirebaseUpload(IFormFile postedFile)
        {
            string projectId = "";
            string bucketName = ""; // Replace with your container name
            string urlPath = "https://firebasestorage.googleapis.com/v0/b/"; // Replace with your storage account URL

            // Get Firebase Storage settings from your configuration
            var _firebaseSettings = _websettinghelper.GetWebsettingJson("FireBaseSettings");

            if (!string.IsNullOrEmpty(_firebaseSettings))
            {
                var json = JsonConvert.DeserializeObject<FireBaseSettingsModel>(_firebaseSettings);

                if (json != null)
                {
                    projectId = json.projectId.Trim();
                    bucketName = json.storageBucket.Trim();
                }
            }

            var ext = Path.GetExtension(postedFile.FileName).ToLower();
            //var validExtensions = new HashSet<string> { ".jpg", ".jpeg", ".gif", ".png", ".pdf", ".docx", ".pptx", ".xlsx", ".doc", ".xls", ".ppt", ".mp4", ".ico", "x-icon", "x-ico", ".zip" };

            if (!validExtensions.Contains(ext))
            {
                return "This file type is not supported.";
            }

            //var uploadFileName = $"{Guid.NewGuid()}{ext}";

            // Concatenate the GUID with the file name (without extension)
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(postedFile.FileName.Replace("_", ""));
            var uploadFileName = $"{postedFile.FileName}_{Guid.NewGuid()}{ext}";


            // Set the file path in Firebase Storage
            string filePath = $"{uploadFileName}";

            // Create GoogleCredential from JSON string

            var _firebaseStorageSettings = _websettinghelper.GetWebsettingJson("FireBaseStorageSettings");

            var storageSettings = JsonConvert.DeserializeObject<FireBaseStorageSettingsModel>(_firebaseStorageSettings);





            var credential = GoogleCredential.FromJson(storageSettings.ServiceAcountMetaData);

            // Create a StorageClient using the GoogleCredential
            var storageClient = StorageClient.Create(credential);

            // Upload the file to Firebase Storage
            using (var stream = postedFile.OpenReadStream())
            {
                storageClient.UploadObject(bucketName, filePath, null, stream);
            }


            //// Create the URL with the desired format
            var fileUrl = $"{urlPath}{bucketName}/o/{uploadFileName}?alt=media";




            return fileUrl;
        }


        #endregion

        #region SingleUpload


        public async Task<string> UploadFileAsync(IFormFile postedFile)
        {

            string fileurlpath = "";
            //check which is enable aws, azure or local and call the accordingly
            var _filestorageSettings = _websettinghelper.GetWebsettingJson("FileStorageSettings");
            if (_filestorageSettings != null && !string.IsNullOrEmpty(_filestorageSettings))
            {
                var json = JsonConvert.DeserializeObject<FileStorageSettingsModel>(_filestorageSettings);

                if (json != null)
                {
                    if (json.FileStorageType == "aws")
                    {
                        fileurlpath = await AwsUpload(postedFile);
                    }
                    else if (json.FileStorageType == "azure")
                    {
                        fileurlpath = await AzureUpload(postedFile);
                    }
                    else if (json.FileStorageType == "local")
                    {
                        fileurlpath = await LocalUpload(postedFile);
                    }
                    else if (json.FileStorageType == "firebase")
                    {
                        fileurlpath = await FirebaseUpload(postedFile);
                    }

                }
            }


            return fileurlpath;
        }
        #endregion





        #region FileTypeValidation


        private string GetContentType(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".ppt":
                    return "application/vnd.ms-powerpoint";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".mp4":
                    return "video/mp4";
                default:
                    return "application/octet-stream";
            }
        }
        #endregion

        #region Icons
        public static string icons(string filename)
        {
            var ext = Path.GetExtension(filename).ToLower();
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif")
            {
                return "view-label file-png";
            }
            else if (ext == ".pdf")
            {
                return "view-label file-pdf";
            }
            else if (ext == ".doc" || ext == ".docx")
            {
                return "view-label file-doc";
            }
            else if (ext == ".ppt" || ext == ".pptx")
            {
                return "view-label file-pptx";
            }
            else if (ext == ".txt")
            {
                return "view-label file-txt";
            }
            else
            {
                return "view-label file-png";
            }
        }
        #endregion
    }
}