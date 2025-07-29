using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using UAParser;

namespace AMMasterProject.Controllers
{

    [Route("controller/[controller]/{action}")]
    [Controller]
    public class UserController : Controller
    {

        #region DI


        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userhelper;
        private readonly NotificationHelper _notificationhelper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;
        public UserController(MyDbContext context, IMemoryCache cache, UserHelper userhelper, NotificationHelper notificationHelper, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = context;
            _cache = cache;
            _userhelper = userhelper;
            _notificationhelper = notificationHelper;
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion


        #region Seller

        //public IActionResult SellerHome()
        //{
        //    List<SellerViewModel> sellerlist = (from up in _userhelper.SellerList()
        //                                        join pb in _dbContext.ProductBasicV2s on up.ProfileId equals pb.ProfileId
        //                                        where pb.IsAdminApproved && pb.IsPublish
        //                                        select new SellerViewModel
        //                                        {
        //                                            BusinessUrlpath = up.BusinessUrlpath,
        //                                            Image = up.Image,
        //                                            BusinessName = up.BusinessName,
        //                                            ProfileId = up.ProfileId,
        //                                            ProductTotal = 0
        //                                        }).ToList();

        //    return PartialView("/Pages/User/_sellerHomeView", sellerlist);
        //}




        [HttpPost]
        public IActionResult BusinessUrlPathValidation(string businessurlpath)
        {
            Guid profileguid = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {
                //loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
                profileguid = Guid.Parse(User.FindFirst("UserGUID")?.Value ?? "0");
            }


            string message = _userhelper.BusinessURLValidation(businessurlpath, profileguid);

            if (message == "exist")
            {
                message = "It already exist. Try another name.";
            }
            else
            {
                message = "Name is available";
            }

            return new JsonResult(message);
        }


        #endregion

        #region FollowSeller

        [HttpPost]
        public IActionResult followuser(int loginid, int sellerid)
        {
            try
            {

                string message = _userhelper.followuser(loginid, sellerid);


                return new JsonResult(message);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting followuser" + ex.Message);
            }



        }

        public IActionResult FollowUserStatus(int loginid, int sellerid)
        {
            try
            {
                string message = _userhelper.followuserstatus(loginid, sellerid);
                bool isFollowing = message == "Un Follow";

                return new JsonResult(new { isFollowing = isFollowing, message = message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting follow user status: " + ex.Message);
            }
        }
        #endregion


        #region UserViews





        public IActionResult UserByGUID(Guid ProfileGUID)
        {
            SellerViewModel seller = _userhelper.SellerByGUID(ProfileGUID);


            return PartialView("/Pages/User/_sellerbyguid.cshtml", seller);

        }



        #endregion


        #region ContactView
        public IActionResult ContactNumberForm(Guid profileguid)
        {
            ContactModel contact = _userhelper.Contact(profileguid);


            return PartialView("/Pages/User/_contactnumber.cshtml", contact);

        }

        #endregion


        #region Announcement


        public IActionResult AnnouncementMetdata(int announcementid)
        {

            // Retrieve the profileid from the claim
            int profileid = int.Parse(User.FindFirst("UserID").Value);
            _notificationhelper.AnnouncementMetaDataPost(profileid, announcementid);


            return Content("success");

        }


        #endregion

        #region AccountDelete

        public IActionResult AccountDelete(int profileid, string? userType, string? routeType)
        {
            string message = _userhelper.AccountDelete(profileid);

            if (string.IsNullOrEmpty(userType)) // user's own account
            {
                TempData["success"] = "Your account has been deleted";
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToPage("/Index");
            }
            else
            {
                TempData["success"] = "User account has been deleted";

                return RedirectToPage("/Admin/Index");
                //return RedirectToPage("/pages/admin/usermanagement", new { type = routeType });
            }
        }
        #endregion


        #region SellerSocialMediaAccountUpdate

        [HttpPost]
        public IActionResult socialMediaAccountUpdate(int socialmediaid, string url)
        {
            try
            {
                int profileid = int.Parse(User.FindFirst("UserID").Value);
                string message = _userhelper.socialMediametadata(socialmediaid, profileid, url);


                return new JsonResult(message);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting socialMediaAccountUpdate" + ex.Message);
            }



        }

        [HttpPost]
        public IActionResult socialMediaAccountDelete(int Sellersocialmediaid)
        {
            try
            {
                int profileid = int.Parse(User.FindFirst("UserID").Value);
                string message = _userhelper.SellersocialMediaDelete(Sellersocialmediaid, profileid);


                return new JsonResult(message);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting SellersocialMediaDelete" + ex.Message);
            }



        }
        #endregion


        #region AvailabilitySetup
        [HttpPost]
        public IActionResult availabilitySetup(string day, bool IsDayEnable, bool iscustomtime, string fromtime, string totime)
        {
            try
            {
                string message = "";

                if (iscustomtime == true && (fromtime == null || totime == null))
                {
                    return new JsonResult("select from time and to time");
                }

                int profileid = int.Parse(User.FindFirst("UserID").Value);
                message = _userhelper.availabilitySetupmetadata(profileid, day, IsDayEnable, iscustomtime, fromtime, totime);




                return new JsonResult(message);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting availabilitySetup" + ex.Message);
            }



        }
        #endregion



        #region LastSeen
        public void LastSeenUpdate()
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                int profileid = int.Parse(User.FindFirst("UserID").Value);
                _userhelper.UserOtherMetaDataUpdate(profileid, "", "LastSeen");
            }
            else
            {
                // Handle the case when the user is not authenticated, e.g., return an error or redirect to a login page.
                // You can also log this event for auditing purposes.
                // Example: return Unauthorized();
            }
        }
        #endregion


        #region UserInactive

        [HttpPost]
        public IActionResult UserInactive(string updatetype,string offlinetype,  DateTime? startdate, DateTime? enddate, bool availableforchat, bool availableforsearch)
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {

                int profileid = int.Parse(User.FindFirst("UserID").Value);

              


                if (offlinetype == "hour" && updatetype=="update")
                {
                  

                    startdate = DateTime.Now;
                    enddate = DateTime.Now.AddHours(1);
                }
                else if (offlinetype == "day" && updatetype == "update")
                {
                  
                    startdate = DateTime.Now;
                    enddate = DateTime.Now.AddDays(1);
                }
                else if (offlinetype == "week" && updatetype == "update")
                {
                   
                    startdate = DateTime.Now;
                    enddate = DateTime.Now.AddDays(7);
                }
                else if (offlinetype == "week" && updatetype == "update")
                {
                  

                    startdate = DateTime.Now;
                    enddate = DateTime.Now.AddDays(30);
                }
                else if (offlinetype == "customize" && updatetype == "update")
                {
                    

                   //startdate = DateTime.Now;
                   //enddate = DateTime.Now.AddDays(30);
                }

                else if (offlinetype == "forever" && updatetype == "update")
                {
                    
                    startdate = DateTime.Now;
                    enddate = DateTime.Now.AddYears(30);
                    availableforchat = false;
                    availableforsearch = false;

                }
                else if (offlinetype == "online" && updatetype == "update")
                {



                

                    startdate = null;
                    enddate = null;
                    availableforchat = true;
                    availableforsearch = true;
                }
                else if (offlinetype != "online" && updatetype == "validate")
                {
                    if(enddate< DateTime.Now)
                    {

                        offlinetype = "online";
                    

                        startdate = null;
                        enddate = null;
                        availableforchat = true;
                        availableforsearch = true;
                    }
                    


                }

                var userInactive = new UserInActive
                {
                    OffLineType = offlinetype,
                  
                    StartDate = startdate,
                    EndDate = enddate,
                    Availableforchat = availableforchat,
                    Availableforsearch = availableforsearch,
                    LastUpdated =DateTime.Now
                };

                // Convert UserAgentMetaData to JSON string
                string jsonUserInActive = JsonConvert.SerializeObject(userInactive);



                _userhelper.UserOtherMetaDataUpdate(profileid, jsonUserInActive, "UserInActive");


                return Json(new {success=true, offlinetype});
            }
            else
            {
                // Handle the case when the user is not authenticated, e.g., return an error or redirect to a login page.
                // You can also log this event for auditing purposes.
                // Example: return Unauthorized();

                return Json(new { success = false, message="User is not authenticated" });
            }
        }
        #endregion


        #region GetAvatars
        public IActionResult GetAvatars()
        {
            var maleAvatars = new List<AvatarList>();
            var femaleAvatars = new List<AvatarList>();

            // Assuming the avatars are stored in wwwroot/avatar/males/ and wwwroot/avatar/females/
            var maleDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "avatar", "males");
            var femaleDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "avatar", "females");

            // Get all files in the male directory
            var maleAvatarFiles = Directory.GetFiles(maleDirectory);
            maleAvatars.AddRange(maleAvatarFiles.Select(file => new AvatarList { ImagePath = $"/avatar/males/{Path.GetFileName(file)}" }));

            // Get all files in the female directory
            var femaleAvatarFiles = Directory.GetFiles(femaleDirectory);
            femaleAvatars.AddRange(femaleAvatarFiles.Select(file => new AvatarList { ImagePath = $"/avatar/females/{Path.GetFileName(file)}" }));

            // Create and return a GenderModel object
            var genderModel = new GenderModel
            {
                malelist = maleAvatars,
                femalelist = femaleAvatars
            };

            return PartialView("/Pages/shared/_avatarlist.cshtml", genderModel);
        }

        #endregion

    }
}
