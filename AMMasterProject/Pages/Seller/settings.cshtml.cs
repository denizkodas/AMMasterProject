using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Seller
{

    [Authorize(Policy = "Seller")]
    [BindProperties]
    public class settingsModel : PageModel
    {
        #region DI
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;
       
        public UserDeviceViewModel UserDevice { get; set; }

        public UserGeneralView usergeneralview { get; set; }


        public settingsModel(MyDbContext context, UserHelper userHelper)
        {
            _dbContext = context;
            _userHelper = userHelper;

        }
        #endregion
        #region DataPopulate    

        
        #endregion

        public void DeviceDetails()
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            string useragentModel = _userHelper.GetUserAgentAsJson(userAgent);

            UserAgentMetaData userAgentMetaData = JsonConvert.DeserializeObject<UserAgentMetaData>(useragentModel);

            UserAgentMetaData useragent = new UserAgentMetaData()
            {
                Browser = userAgentMetaData.Browser,
                IP = userAgentMetaData.IP,
                OperatingSystem = userAgentMetaData.OperatingSystem,
                DeviceType = userAgentMetaData.DeviceType,
            };

            Guid profileguid = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {
                profileguid = Guid.Parse(User.FindFirst("UserGUID")?.Value ?? Guid.Empty.ToString());
            }

            usergeneralview = _userHelper.UserGeneralByGUID(profileguid);

            UserDevice = new UserDeviceViewModel()
            {
                UserAgent = useragent,
                ListUserDevice = usergeneralview?.userothermetadata?.UserAgentMetaData ?? new List<UserAgentMetaData>()
            };



            /* return Json(model);*/ // Return the serialized UserDeviceViewModel instead of useragentModel
        }
        public void OnGet()
        {
            
            DeviceDetails();
        }
    }
}
