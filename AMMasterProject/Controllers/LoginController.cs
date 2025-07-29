//using Amazon.Runtime.Internal.Util;
using AMMasterProject.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using AMMasterProject.ViewModel;

using Microsoft.AspNetCore.Authentication.Google;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Facebook;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Web;
using static Google.Apis.Auth.OAuth2.Web.AuthorizationCodeWebApp;
using Microsoft.EntityFrameworkCore;
using PayPal.Api;
using UAParser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
//using Microsoft.VisualBasic;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AMMasterProject.Controllers
{

    [Route("controller/[controller]/{action}")]
    [Controller]
    public class LoginController : Controller
    {
        #region DI

        private readonly WebsettingHelper _websettinghelper;
        private readonly MyDbContext _dbContext;
        private readonly NotificationHelper _notificationHelper;
        private readonly MembershipHelper _membershiphelper;
        private readonly OrderHelper _orderHelper;
        private readonly UserHelper _userHelper;
        private readonly GlobalHelper _globalhelper;

        private readonly IOptionsMonitor<GoogleOptions> _googleOptions;
        private readonly IOptionsMonitor<FacebookOptions> _facebookOptions;


      

        public LoginController(MyDbContext context, NotificationHelper notificationHelper, MembershipHelper membershiphelper, OrderHelper orderHelper, IOptionsMonitor<GoogleOptions> googleOptions, IOptionsMonitor<FacebookOptions> facebookOptions, WebsettingHelper websettinghelper, UserHelper userhelper, GlobalHelper globalhelper)
        {
            _dbContext = context;
            _notificationHelper = notificationHelper;
            _membershiphelper = membershiphelper;
            _orderHelper = orderHelper;
            _googleOptions = googleOptions;
            _facebookOptions = facebookOptions;
            _websettinghelper = websettinghelper;
            _userHelper = userhelper;
            _globalhelper = globalhelper;   
        }
        #endregion

        #region AccessDenied
        public IActionResult AccessDenied()
        {

            //ViewBag.MyString = myString;
            return RedirectToPage("/Error", new { Title = "Access Denied", Message = "You do not have permission to access this feature" });
        }
        #endregion


        #region PartialViews

       

        public IActionResult LoginView()
        {
            var model = _userHelper.registrationsetting();

            //ViewBag.MyString = myString;
            return PartialView("/Pages/Login/_Login.cshtml", model);
        }


        public IActionResult RegisterView()
        {
            var model = _userHelper.registrationsetting();

            //ViewBag.MyString = myString;
            return PartialView("/Pages/Login/_Register.cshtml", model);
        }


        public IActionResult ChangePasswordView()
        {

            PasswordModel model = new PasswordModel();
            //ViewBag.MyString = myString;
            return PartialView("/Pages/Login/_changepassword.cshtml", model);
        }
        #endregion
        #region Register


        [HttpPost]
        public async Task<string> RegisterCreateAccount(string UserName, string Type, string loginchannel, string FirstName, string LastName, string Password)
        {
            string message = string.Empty;


            try
            {

          

            UsersProfile CreateAccount = new UsersProfile();


            //if (HttpContext.Session.TryGetValue("UserType", out byte[] userTypeBytes))
            //{
            //    Type = Encoding.UTF8.GetString(userTypeBytes);
            //}


            CreateAccount.UserName = UserName.Trim().ToLower();
            CreateAccount.InsertDate = DateTime.Now;
            CreateAccount.Type = Type;
            CreateAccount.Loginchannel = loginchannel ?? "Local";
            CreateAccount.Firstname = FirstName.Trim();
            CreateAccount.Lastname = LastName.Trim();
            CreateAccount.Password = EncryptionHelper.encryption(Password.Trim());

           
            CreateAccount.IsUserNameVerified = true;
            CreateAccount.OtherMetaData = _userHelper.userothermetadata();

        

            _dbContext.UsersProfiles.Add(CreateAccount);
            _dbContext.SaveChanges();

            message = "success # USERGUID=" + CreateAccount.ProfileGuid + "#Type=" + CreateAccount.Type;


            #region SignupCredits
            int signupcredit = _membershiphelper.CreditOnSignup(CreateAccount.Type);

            if (signupcredit > 0)
            {

                TempData["success"] = signupcredit + " Sign Up Credits assigned";
               
               string type = "credit";

               string invoicenumber = GlobalHelper.GetInvoiceNumber(CreateAccount.ProfileId.ToString(), type);

                //GlobalHelper.SetCookie("cartInvoiceNumber", invoicenumber);
                string custom = $"Free credit on signup,{signupcredit},0";

                string itemhelper = _orderHelper.ItemMetaData(type, "", "", 0, 1, "", invoicenumber,"","", custom);

                // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                string ItemMetadata = itemMetadataResult.updatedjson;
                string summarymetadata = itemMetadataResult.updatedsummaryjson;
                int sellerid = 0;

                string paymentmetadata = _orderHelper.paymentmetadata("NA", "paid", "Free Signup", string.Empty, DateTime.Now, $"Free signup credit for {CreateAccount.ProfileId}", ItemMetadata, "0", _globalhelper.BaseCurrency(), CreateAccount.Email, invoicenumber);// payment metadata


                


                string orderstatus = _orderHelper.OrderCreation(CreateAccount.ProfileId, sellerid, type, "confirm", "free", "completed", invoicenumber, ItemMetadata, paymentmetadata, "paid", summarymetadata);


                //string orderstatus = _orderHelper.OrderCreation(CreateAccount.ProfileId, 0, "credit", "confirm", "free", "completed", invoicenumber, itemmetadata, paymentmetadata, "paid");



            }
            #endregion

            string returnurl = await LoginAuthenticate(CreateAccount.Type, CreateAccount.ProfileGuid.ToString(), CreateAccount.ProfileId.ToString(), CreateAccount.Firstname, CreateAccount.Lastname, CreateAccount.ProfileImage ?? CreateAccount.SellerImage);







            return returnurl;

            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }




        #endregion


        #region UserNameValidation

        [HttpPost]
        public IActionResult UserNameAvailableCheck(string UserName, string loginchannel)
        {
            string message = string.Empty;
            bool success = true;

            UsersProfile usernameExists = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName == UserName);

            if (usernameExists != null)
            {
                ///account created but not verified
                success = false;
                message = "User Name already exist";




            }
            else
            {
                //if (loginchannel == "Email")
                //{
                //    string verificationCode = GlobalHelper.RandomNumber();
                //    GenerateVerificationCode(verificationCode);


                //    _notificationHelper.notification("RegisterVerificationCode", 0, verificationCode, "", "", "", UserName, "");

                //}

                success = true;
                message = "User Name available";
               
            }

           return Json(new {success, message, loginchannel, UserName });
        }
        #endregion


        #region GenerateVerificationCode()
        [HttpPost]
        public IActionResult GenerateVerificationCode(string UserName, string codetype)
        {
            string verificationCode = GlobalHelper.RandomNumber();
           
            string message = "success";
            int notificationid = 0;

            DateTime expiryTime = DateTime.Now.AddMinutes(5);


            // Store the verification code securely in a user's cookie with an expiry time
            Response.Cookies.Append("VerificationCode", EncryptionHelper.encryption(verificationCode), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiryTime
            });

            //RegisterVerificationCode
            //PasswordVerificationCode
            notificationid= _notificationHelper.notification(codetype, 0, verificationCode, "", "", "", UserName, "");
           
            return Json(new {success=true, message, notificationid });
        }
        [HttpPost]
        public IActionResult RetreiveVerificationCode(string verificationcode)
        {
            string message = "success";
            bool success = true;
            string verificationCode = Request.Cookies["VerificationCode"];


            string decryptcode = EncryptionHelper.dycryption(verificationCode);


            if (verificationcode == decryptcode)
            {
                RemoveVerificationCode();
                message = "Processing...";
                success = true;

            }
            else
            {
                success = false;
                message = "Validation code not verified. Please check your email or spam folder.";
               
            }
            return Json(new { success, message });
        }

        [HttpPost]
        public void RemoveVerificationCode()
        {
            Response.Cookies.Delete("VerificationCode");

        }

        #endregion

        #region Login

        
        public async Task<string> LoginAuthenticate(string usertype, string userguid, string userid, string firstname, string lastname, string image)
        {
            try
            {

               

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userguid),
                        new Claim("UserGUID", userguid),
                        new Claim("UserType", usertype),
                        new Claim("UserID", userid),
                        new Claim("FirstName", firstname),
                        new Claim("LastName", lastname),
                        new Claim("Image", image??""),
                        new Claim("firstchar", firstname.ToLower().Substring(0,1))



                    };


                // Get user agent (browser) details
               
              
              


                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in the user

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(30))
                });





                string returnurl = GlobalHelper.GetReturnURLCookie("returnurl");
                string mainDomain = GlobalHelper.GetCurrentDomainName();
                string returnUrlValue = "";
                // Check if returnurl contains the "ReturnUrl" query parameter
                // Parse the returnurl as a Uri
                if (Uri.TryCreate(returnurl, UriKind.Absolute, out Uri uri))
                {

                   
                        // Get the "ReturnUrl" query parameter value
                        //if cookie return url is null so try to check if query string parameter exist
                       returnUrlValue = HttpUtility.ParseQueryString(uri.Query).Get("ReturnUrl");
                    

                    if (returnurl!=null)
                    {
                        // Extract the URL without the query string
                        // Get the scheme (http/https) and host (domain) without the path

                        //if (!string.IsNullOrEmpty(returnUrlValue))
                        //{
                        //    // Append "?USERGUID=" + userguid to the "ReturnUrl" value
                        //    returnurl = $"{returnurl}+ {returnUrlValue} + &USERGUID= {userguid}";
                        //}

                        //returnurl = mainDomain + returnUrlValue + "?USERGUID=" + userguid;

                    

                        if (returnurl.Contains("?"))
                        {
                            // If it already has a query string, use "&" to append userguid
                            returnurl = returnurl + "&USERGUID=" + userguid;
                        }
                        else
                        {
                            // If it doesn't have a query string, use "?" to append userguid
                            returnurl = returnurl + "?USERGUID=" + userguid;
                        }
                        GlobalHelper.RemoveCookie("returnurl");
                    }

                    if(returnUrlValue!=null)
                    {
                        returnurl = mainDomain + returnUrlValue + "&USERGUID=" + userguid;

                        
                        GlobalHelper.RemoveCookie("returnurl");
                    }
                }

                //if (usertype == "Client" && returnurl == string.Empty)
                //{
                //    returnurl = mainDomain + "?USERGUID=" + userguid;
                //    GlobalHelper.RemoveCookie("returnurl");
                //}
                if (usertype == "Vendor")
                {
                    returnurl = "/seller/Index?USERGUID=" + userguid;
                    GlobalHelper.RemoveCookie("returnurl");
                }

                // Get the query string from the URL




                return returnurl;
            }
            catch (Exception ex)
            {
                return "Error occurred while getting LoginAuthenticate: " + ex.Message;
            }
        }




        [HttpPost]
        public async Task<string> login(string username, string password)
        {
            try
            {

                string returnurl = string.Empty;

                string ps = EncryptionHelper.encryption(password);
                UsersProfile UserExist = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName == username);
                if (UserExist == null)
                {
                    returnurl = "User Name does not exist";
                    return returnurl;
                }

                UsersProfile UserLoginChannel = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName == username);
                if (UserLoginChannel != null)
                {

                    if (UserLoginChannel.Loginchannel == "Facebook")
                    {
                        returnurl = "Your email is associated with facebook. Please login with facebook authentication.";
                        return returnurl;
                    }
                    else if (UserLoginChannel.Loginchannel == "Google")
                    {
                        returnurl = "Your email is associated with google. Please login with google authentication.";
                        return returnurl;
                    }

                }

                UsersProfile UserVerified = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName == username && u.IsUserNameVerified == false);
                if (UserVerified != null)
                {
                    returnurl = "User Name is not verified. Click On Join here to verified your username";
                    return returnurl;
                }

                UsersProfile LockedByAdmin = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName == username && u.IsLockedByAdmin == true && u.UnLockedDate >= DateTime.Now);
                if (LockedByAdmin != null)
                {

                    returnurl = username + ", " + LockedByAdmin.AdminRemarksOnLocked + ". Unlocked date is " + GlobalHelper.DateFormat(LockedByAdmin.UnLockedDate);

                    return returnurl;
                }



                UsersProfile userValidate = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName == username && u.Password == ps);


                if (userValidate != null)
                {

                   ///get browser info
                   
                    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                    string useragentModel = _userHelper.GetUserAgentAsJson(userAgent);
                    _userHelper.UserOtherMetaDataUpdate(userValidate.ProfileId, useragentModel, "UserAgent");

                    userValidate.Lastlogin = DateTime.Now;
                    _dbContext.UsersProfiles.Update(userValidate);
                    _dbContext.SaveChanges();

                    returnurl = "success" + await LoginAuthenticate(userValidate.Type, userValidate.ProfileGuid.ToString(), userValidate.ProfileId.ToString(), userValidate.Firstname, userValidate.Lastname, userValidate.ProfileImage ?? userValidate.SellerImage);







                    return returnurl;
                }
                else
                {
                    returnurl = "Password is wrong.";
                }

                return returnurl;

            }
            catch (Exception ex)
            {

                return "Error occurred while getting Login" + ex.Message;
            }



        }

        [HttpPost]
        public async Task<IActionResult> logout()
        {
            try
            {

                //Retrieve user id from claims
                var userId = User.FindFirstValue("UserID");

                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                string useragentModel = _userHelper.GetUserAgentAsJson(userAgent);
                _userHelper.UserOtherMetaDataUpdate(int.Parse(userId), useragentModel, "Logout");

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


                TempData["success"] = "log out successfully";
                return new JsonResult("Success");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting logout: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> LogoutDevice(string browser, string browserversion, string operatingsystem, string devicetype, string ip)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    // Retrieve user id from claims
                    var userId = User.FindFirstValue("UserID");

                    var userAgent = new UserAgentMetaData
                    {
                        Browser = browser,
                        BrowserVersion = browserversion,
                        OperatingSystem = operatingsystem,
                        DeviceType = devicetype,
                        IP = ip
                    };

                    // Convert UserAgentMetaData to JSON string
                    string jsonUserAgent = JsonConvert.SerializeObject(userAgent);

                    _userHelper.UserOtherMetaDataUpdate(int.Parse(userId), jsonUserAgent, "Logout");

                   
                    return new JsonResult("Success");
                }
                else
                {
                    // Handle the case where the user is not authenticated
                    // You can return an appropriate response, such as a 401 Unauthorized status
                    return StatusCode(StatusCodes.Status401Unauthorized, "User is not authenticated");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting logout: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> ValidateLogin()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    //Retrieve user id from claims
                    var userId = User.FindFirstValue("UserID");

                    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                    string useragentModel = _userHelper.GetUserAgentAsJson(userAgent);
                    string message =_userHelper.ValidateDevice(int.Parse(userId), useragentModel);

                    if (message == "invalid")
                    {
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        TempData["success"] = "Device Log out successfully";

                    }
                   
                    return new JsonResult(message);
                }
                else
                {
                    // Handle the case where the user is not authenticated
                    // You can return an appropriate response, such as a 401 Unauthorized status
                    return StatusCode(StatusCodes.Status401Unauthorized, "User is not authenticated");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting logout: " + ex.Message);
            }
        }

        #endregion


        #region ChangePassword
        [HttpPost]
        public IActionResult ChangePassword(string password)
        {
            string message = string.Empty;
            Guid ProfileGUID = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {

                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value);
                // continue with loginid variable







                UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

                if (up != null)
                {
                    up.Password = EncryptionHelper.encryption(password);


                    _dbContext.UsersProfiles.Update(up);
                    _dbContext.SaveChanges();

                    message = "success";



                }
            }

            else
            {
                message = "Your not log in";

            }

            return Json(new { message });
        }
        #endregion


        #region ExternalRegistration


        #region Google
        public IActionResult GoogleLogin()
        {

            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleLoginCallback), null, null, "https"),


            };
            var options = _googleOptions.Get(GoogleDefaults.AuthenticationScheme);


            var model = _userHelper.registrationsetting();



            if (model != null)
            {


                options.ClientId = model.ClientId;
                options.ClientSecret = model.ClientSecret;

            }

            return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleLoginCallback()
        {
            var authResult = await HttpContext.AuthenticateAsync();

            // Access the user's details using authResult.Principal

            if (authResult.Succeeded)
            {
                var givenname = authResult.Principal.FindFirstValue(ClaimTypes.GivenName);
                var surname = authResult.Principal.FindFirstValue(ClaimTypes.Surname);
                var UserName = authResult.Principal.FindFirstValue(ClaimTypes.Email);
                var name = authResult.Principal.FindFirstValue(ClaimTypes.Name);
                var userId = authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);


                //if user exist so make it login
                UsersProfile Validate = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName.Trim().ToLower() == UserName.Trim().ToLower());

                if (Validate != null)
                {
                    string returnurl = await LoginAuthenticate(Validate.Type, Validate.ProfileGuid.ToString(), Validate.ProfileId.ToString(), Validate.Firstname, Validate.Lastname, Validate.ProfileImage ?? Validate.SellerImage);


                    return Redirect(returnurl);
                }

                else
                {
                    string Type = "Client";
                    string FirstName = givenname.Trim();
                    string LastName = surname.Trim();
                    string Password = userId;

                    if (HttpContext.Session.TryGetValue("UserType", out byte[] userTypeBytes))
                    {
                        Type = Encoding.UTF8.GetString(userTypeBytes);
                    }


                    string returnurl = await RegisterCreateAccount(UserName, Type, "Google", FirstName, LastName, Password);


                    //string returnurl = await LoginAuthenticate(Validate.Type, Validate.ProfileGuid.ToString(), Validate.ProfileId.ToString(), Validate.Firstname, Validate.Lastname, Validate.ProfileImage ?? Validate.SellerImage);


                    return Redirect(returnurl);
                }










            }



            // Example: Redirect to a different action


            Response.Redirect("/Error?Title=Fail&Message=Google authentication faild&Body=Please try again later.");

            return Content("Authentication failed.");

        }
        #endregion


        #region Facebook

        //Step 1
        public IActionResult FacebookLogin()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(FacebookLoginCallback))
            };


            var options = _facebookOptions.Get(FacebookDefaults.AuthenticationScheme);

            var model = _userHelper.registrationsetting();


            if (model != null)
            {


                options.AppId = model.AppId;
                options.AppSecret = model.AppSecret;

            }


            return Challenge(authenticationProperties, FacebookDefaults.AuthenticationScheme);
        }


        //Step 2
        public async Task<IActionResult> FacebookLoginCallback()
        {
            var authResult = await HttpContext.AuthenticateAsync();

            if (authResult.Succeeded)
            {
                //var UserName = authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                //var Name = authenticateResult.Principal.FindFirstValue(ClaimTypes.Name);

                var givenname = authResult.Principal.FindFirstValue(ClaimTypes.GivenName);
                var surname = authResult.Principal.FindFirstValue(ClaimTypes.Surname);
                var UserName = authResult.Principal.FindFirstValue(ClaimTypes.Email);
                var name = authResult.Principal.FindFirstValue(ClaimTypes.Name);
                var userId = authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);



                // Retrieve the Facebook access token
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var code = await HttpContext.GetTokenAsync("code");

                if (string.IsNullOrEmpty(accessToken))
                {
                    // Handle the case where the access token is missing.
                    // Log or redirect to an error page as needed.

                    Response.Redirect("/Error?Title=Access token missing.&Message=Access token missing.&Body=Access token missing.");
                    return Content("Access token missing.");



                }

                if (string.IsNullOrEmpty(code))
                {
                    // Handle the case where the access token is missing.
                    // Log or redirect to an error page as needed.

                    Response.Redirect("/Error?Title=code missing.&Message=code missing.&Body=code missing.");
                    return Content("Access token missing.");



                }

                // Fetch the user's profile picture using the obtained access token
                var profilePictureUrl = await GetFacebookProfilePictureUrl(accessToken);

                if (string.IsNullOrEmpty(profilePictureUrl))
                {
                    // Handle the case where fetching the profile picture failed.
                    // Log or redirect to an error page as needed.
                    Response.Redirect("/Error?Title=profile picture.&Message=profile picture.&Body=profile picture.");
                    return Content("Failed to fetch profile picture.");
                }

                // Perform further actions with the user data

                UsersProfile Validate = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName.Trim().ToLower() == UserName.Trim().ToLower());

                if (Validate != null)
                {
                    string returnurl = await LoginAuthenticate(Validate.Type, Validate.ProfileGuid.ToString(), Validate.ProfileId.ToString(), Validate.Firstname, Validate.Lastname, Validate.ProfileImage ?? Validate.SellerImage);


                    return Redirect(returnurl);
                }

                else
                {
                    string Type = "Client";
                    string FirstName = givenname.Trim();
                    string LastName = surname.Trim();
                    string Password = UserName;


                    if (HttpContext.Session.TryGetValue("UserType", out byte[] userTypeBytes))
                    {
                        Type = Encoding.UTF8.GetString(userTypeBytes);
                    }


                    string returnurl = await RegisterCreateAccount(UserName, Type, "Facebook", FirstName, LastName, Password);



                    return Redirect(returnurl);
                }
            }
            Response.Redirect("/Error?Title=Fail&Message=Facebook authentication faild&Body=Please try again later.");
            return Content("Authentication failed.");
        }







        private async Task<string> GetFacebookProfilePictureUrl(string accessToken)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://graph.facebook.com/me/picture?access_token={accessToken}&redirect=false");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var pictureData = JObject.Parse(content)["data"];
                return pictureData.Value<string>("url");
            }

            return null;
        }


        [HttpPost]
        public async Task<IActionResult> FacebookLoginJs(string givenname, string surname, string UserName, string userId)
        {
            try
            {




                UsersProfile Validate = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName.Trim().ToLower() == UserName.Trim().ToLower());

                if (Validate != null)
                {
                    string returnurl = await LoginAuthenticate(Validate.Type, Validate.ProfileGuid.ToString(), Validate.ProfileId.ToString(), Validate.Firstname, Validate.Lastname, Validate.ProfileImage ?? Validate.SellerImage);


                    return Content(returnurl);
                }

                else
                {
                    string Type = "Client";
                    string FirstName = givenname.Trim();
                    string LastName = surname.Trim();
                    string Password = userId;


                    if (HttpContext.Session.TryGetValue("UserType", out byte[] userTypeBytes))
                    {
                        Type = Encoding.UTF8.GetString(userTypeBytes);
                    }


                    string returnurl = await RegisterCreateAccount(UserName, Type, "Facebook", FirstName, LastName, Password);



                    return Content(returnurl);
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error?Title=Fail&Message=Facebook authentication faild&Body=" + ex.Message);
                return Content("Authentication failed.");

            }
        }

        #endregion

        #endregion


        #region ForgetPassword
        [HttpPost]
        public IActionResult ForgetPasswordUserNameAvailableCheck(string UserName)
        {
            string message = string.Empty;
            string loginchannel = "";
            string username = "";
            bool success = true;

            UsersProfile usernameExists = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName == UserName);

            if (usernameExists == null)
            {
                ///account created but not verified
                success = false;
                message = "User Name not exist.";




            }
            else
            {

                success = true;
                if (usernameExists.Loginchannel == "Email")
                {

                    //Use this method same used for Registration Verification method
                    //RetreiveVerificationCode  to verify and removed the cookie
                    //string verificationCode = GlobalHelper.RandomNumber();
                    //GenerateVerificationCode(verificationCode);

                    //send data in notification relay table
                    //_notificationHelper.notification("PasswordVerificationCode", 0, verificationCode, "", "", "", UserName, "");
                    loginchannel = "Email";
                    message = "User name exist";
                    username = usernameExists.UserName;
                }
                else if (usernameExists.Loginchannel == "Phone")
                {
                    loginchannel = "Phone";
                    message = "User name exist";

                    username = usernameExists.UserName;
                }
                else if (usernameExists.Loginchannel == "Facebook")
                {
                    loginchannel = "Facebook";
                    message = "You have logged in with Facebook";
                }
                else if (usernameExists.Loginchannel == "Google")
                {
                    loginchannel = "Google";
                    message = "You have logged in with Google";
                }

            }

            return Json(new {success, message, loginchannel, username });
        }

        [HttpPost]
        public async Task<string> UpdatePassword(string UserName, string Password)
        {
            string message = string.Empty;

            UsersProfile usernameExists = _dbContext.UsersProfiles.FirstOrDefault(u => u.UserName == UserName);

            if (usernameExists == null)
            {
                ///account created but not verified

                message = "User name does not exist.";




            }
            else
            {
                usernameExists.Password = EncryptionHelper.encryption(Password.Trim());
                _dbContext.UsersProfiles.Update(usernameExists);
                _dbContext.SaveChanges();


                TempData["success"] = "You have successfully updated your password and Logged In.";

            }

            string returnurl = await LoginAuthenticate(usernameExists.Type, usernameExists.ProfileGuid.ToString(), usernameExists.ProfileId.ToString(), usernameExists.Firstname, usernameExists.Lastname, usernameExists.ProfileImage ?? usernameExists.SellerImage);

            return "/index";
        }


        #endregion


        #region AcccountAccess

        [HttpPost]
        public async Task<string> AccountAccess(Guid userguid)
        {
            string returnUrl="";
            try
            {
                UsersProfile usernameExists = await _dbContext.UsersProfiles.FirstOrDefaultAsync(u => u.ProfileGuid == userguid);

                if (usernameExists != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usernameExists.UserName),
                new Claim("UserGUID", usernameExists.ProfileGuid.ToString()),
                new Claim("UserType", usernameExists.Type),
                new Claim("UserID", usernameExists.ProfileId.ToString()),
                new Claim("FirstName", usernameExists.Firstname ?? ""),
                new Claim("LastName", usernameExists.Lastname ?? ""),
                new Claim("Image", usernameExists.ProfileImage ?? ""),
                new Claim("firstchar", usernameExists.Firstname?.Substring(0, 1)?.ToLower() ?? "")
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
                    {
                        IsPersistent = true, // make the cookie persistent
                        ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(30)) // set the expiration time to 30 days
                    });

                    if (usernameExists.Type == "Client")
                    {
                        returnUrl += "/Index?USERGUID=" + usernameExists.ProfileGuid;
                    }
                    else if (usernameExists.Type == "Vendor")
                    {
                        returnUrl += "/seller/Index?USERGUID=" + usernameExists.ProfileGuid;
                    }
                }
                else
                {
                    TempData["success"] = "Error: User Name does not exist.";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here, log them, or take appropriate actions.
                TempData["success"] = "Error: An error occurred.";
                // You might also want to log the exception details for debugging.
            }

            // Return an appropriate response, such as an error page or a redirect to a login page.
            return "success"+ returnUrl;
        }
        #endregion
    }
}
