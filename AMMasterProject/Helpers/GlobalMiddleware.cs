using Microsoft.Extensions.Caching.Memory;
using AMMasterProject;

using System.Reflection;
using Newtonsoft.Json;
using AMMasterProject.ViewModel;
using System;

namespace AMMasterProject.Helpers
{
    public class GlobalMiddleware : IMiddleware
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MyDbContext _dbContext;

        private readonly WebsettingHelper _websettinghelper;
        public CompanySetupModel CompanySetup { get; set; }
        public LicenseAppSettingsModel LicenseAppSettingsModel { get; set; }

        public GlobalMiddleware(IMemoryCache memoryCache, MyDbContext dbContext, WebsettingHelper websettinghelper)
        {
            _memoryCache = memoryCache;
            _dbContext = dbContext;

            _websettinghelper = websettinghelper;
            CompanySetup = new CompanySetupModel();
        }


        //public static string GetVersion()
        //{
        //    //var versionInfo = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        //    //var version = new Version(versionInfo.InformationalVersion);

        //    //// Increment the build component by 1
        //    //var updatedVersion = new Version(version.Major, version.Minor, version.Build + 1);

        //    //return updatedVersion.ToString();

        //    //var version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        //    //return version;



        //    return "2.3.0 - 01-Nov-2023";
        //}


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Perform your global logic here

            string websetting = _websettinghelper.GetWebsettingJson("CompanySetupSettings");
            string licensesetting = _websettinghelper.GetWebsettingJson("LicenseAppSettings");

            if (websetting != null && !string.IsNullOrEmpty(websetting))
            {
                var json = JsonConvert.DeserializeObject<CompanySetupModel>(websetting);

                if (json != null)
                {
                    context.Items["CompanyName"] = json.CompanyName;
                    context.Items["CompanyLogo"] = json.Logo;
                    context.Items["Favicon"] = json.Favicon;
                    context.Items["AboutCompany"] = json.CompanyDescription;
                    context.Items["Address"] = json.CompanyAddress;
                    context.Items["SupportContact"] = json.SupportContact;
                    context.Items["SupportEmail"] = json.SupportEmail;
                    context.Items["IsMultiVendor"] = json.IsMultiVendor;
                    context.Items["VersionNumber"] = "2.6.5";
                    context.Items["VersionDate"] = "30-Jun-2024";

                  


                }

                // Rest of the code to set up the CompanySetup model
            }

            if (licensesetting != null && !string.IsNullOrEmpty(licensesetting))
            {
                var json = JsonConvert.DeserializeObject<LicenseAppSettingsModel>(licensesetting);

                if (json != null)
                {
                    context.Items["LicenseKey"] = json.LicenseKey;
                    context.Items["ActivationDate"] =json.ActivationDate!=null?  json.ActivationDate : null;
                    context.Items["ExpiryDate"] = json.ExpiryDate!=null ? json.ExpiryDate : null ;
                    context.Items["LicenseKeyForBrandRemoval"] = json.LicenseKeyForBrandRemoval;
                    context.Items["BrandRemovalActivationDate"] = json.BrandRemovalActivationDate!=null? json.BrandRemovalActivationDate:null;
                    context.Items["BrandRemovalExpiryDate"] = json.BrandRemovalActivationDate!= null?json.BrandRemovalActivationDate :null;
                  


                
                }

                // Rest of the code to set up the CompanySetup model
            }
            else
            {

                var licensesettings = new LicenseAppSettingsModel
                {
                    LicenseKey = "",
                    ActivationDate = null,
                    ExpiryDate = null,
                    LicenseKeyForBrandRemoval = "",
                    BrandRemovalActivationDate = null,
                    BrandRemovalExpiryDate = null
                };
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(licensesettings);

                _websettinghelper.UpdateWebsettingJson("LicenseAppSettings", jsonData);

            }







            ///call header links from cache no need to make database call again and again



            // Pass data to the view





            await next(context);
        }
    }
}
