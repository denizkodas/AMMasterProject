using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.User
{
    [Authorize(Policy = "AllUsers")]
    [BindProperties]
    public class accountcontrolModel : PageModel
    {
        #region DI
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;
        //public ClientProfileModel UserProfile { get; set; }
        public UserDeviceViewModel UserDevice { get; set; }
        public UserGeneralView usergeneralview { get; set; }



        public accountcontrolModel(MyDbContext context, UserHelper userHelper)
        {
            _dbContext = context;
            _userHelper = userHelper;

        }
        #endregion
        #region DataPopulate    

        public void setup()
        {

            Guid ProfileGUID = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {

                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value);
                // continue with loginid variable
            }

            usergeneralview = _userHelper.UserGeneralByGUID(ProfileGUID);

            //clientview = _userHelper.ClientByGUID(ProfileGUID);

            //if (clientview != null)
            //{
            //    UserProfile = new ClientProfileModel
            //    {
            //        ProfileGuid = clientview.ProfileGuid,
            //        ProfileImage = clientview.Image,
            //        Firstname = clientview.FirstName,
            //        Lastname = clientview.LastName,
            //        Email = clientview.Email,
            //        ClientDisplayName = clientview.Displayname,
            //        About = clientview.About,
            //        Contactnumber = clientview.Contact,


            //    };
            //}

        }
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

            ClientViewModel clientview = _userHelper.ClientByGUID(profileguid);

            UserDevice = new UserDeviceViewModel()
            {
                UserAgent = useragent,
                ListUserDevice = clientview?.userothermetadata?.UserAgentMetaData ?? new List<UserAgentMetaData>()
            };



            /* return Json(model);*/ // Return the serialized UserDeviceViewModel instead of useragentModel
        }

      
        public void OnGet()
        {
            setup();
            DeviceDetails();
        }
    }
}
