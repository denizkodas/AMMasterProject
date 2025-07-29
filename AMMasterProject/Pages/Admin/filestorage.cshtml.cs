using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class filestorageModel : PageModel
    {
        #region Model
        private readonly WebsettingHelper _websettinghelper;
        public AWSSettingsModel aws { get; set; }

        public AzureSettingsModel azure { get; set; }

        public FireBaseStorageSettingsModel firebasestorage { get; set; }

        public FileStorageSettingsModel filestorage { get; set; }

        

        #endregion

        #region DI



        public filestorageModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            aws = new AWSSettingsModel();
            azure = new AzureSettingsModel();

        }


        #endregion
        public void OnGet()
        {

            //AWS

            var _awsSettings = _websettinghelper.GetWebsettingJson("AWSSettings");
            if (_awsSettings != null && !string.IsNullOrEmpty(_awsSettings))
            {
                var json = JsonConvert.DeserializeObject<AWSSettingsModel>(_awsSettings);

                if (json != null)
                {
                    aws = new AWSSettingsModel
                    {
                        AccessKey = json.AccessKey,
                        SecretKey = json.SecretKey,
                        URLPath = json.URLPath,
                        Bucket = json.Bucket
                    };

                }
            }


            //azure
            var _azureSettings = _websettinghelper.GetWebsettingJson("AzureSettings");
            if (_azureSettings != null && !string.IsNullOrEmpty(_azureSettings))
            {
                var json = JsonConvert.DeserializeObject<AzureSettingsModel>(_azureSettings);

                if (json != null)
                {
                    azure = new AzureSettingsModel
                    {
                        ConnectionString = json.ConnectionString,
                        ContainerName = json.ContainerName,
                        URLPath = json.URLPath,
                       
                    };

                }
            }



            //firebase storage
            var _firebasestorageSettings = _websettinghelper.GetWebsettingJson("FireBaseStorageSettings");
            if (_firebasestorageSettings != null && !string.IsNullOrEmpty(_firebasestorageSettings))
            {
                var json = JsonConvert.DeserializeObject<FireBaseStorageSettingsModel>(_firebasestorageSettings);

                if (json != null)
                {
                    firebasestorage = new FireBaseStorageSettingsModel
                    {
                        ServiceAcountMetaData = json.ServiceAcountMetaData,
                        
                    };

                }
            }


            //Local
            var _filestorageSettings = _websettinghelper.GetWebsettingJson("FileStorageSettings");
            if (_filestorageSettings != null && !string.IsNullOrEmpty(_filestorageSettings))
            {
                var json = JsonConvert.DeserializeObject<FileStorageSettingsModel>(_filestorageSettings);

                if (json != null)
                {
                    filestorage = new FileStorageSettingsModel
                    {
                        FileStorageType = json.FileStorageType
                       

                    };

                }
            }


        }

        public IActionResult OnPostAWS()
        {
            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(aws);

            string msg = _websettinghelper.UpdateWebsettingJson("AWSSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }
            else if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }
            else
            {
                TempData["success"] = msg;

            }

            return Redirect("/admin/filestorage#aws");
        }

        public IActionResult OnPostAzure()
        {
            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(azure);

            string msg = _websettinghelper.UpdateWebsettingJson("AzureSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }
            else if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }
            else
            {
                TempData["success"] = msg;

            }

            return Redirect("/admin/filestorage#azure");
        }

        public IActionResult OnPostFireBaseStorage()
        {
            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(firebasestorage);

            string msg = _websettinghelper.UpdateWebsettingJson("FireBaseStorageSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }
            else if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }
            else
            {
                TempData["success"] = msg;

            }

            return Redirect("/admin/filestorage#firebasestorage");
        }

        public IActionResult OnPostCDN()
        {
            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(filestorage);

            string msg = _websettinghelper.UpdateWebsettingJson("FileStorageSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }
            else if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }
            else
            {
                TempData["success"] = msg;

            }

            return Redirect("/admin/filestorage#cdn");
        }
    }
}
