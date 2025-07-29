using AMMasterProject.Migrations;
using AMMasterProject.Models;
using AMMasterProject.ViewModel;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using UAParser;

using static System.Runtime.InteropServices.JavaScript.JSType;
using Azure.Core;

using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Results.Device;
using DeviceDetectorNET.Cache;
using ThirdParty.Json.LitJson;
using System.Net;

namespace AMMasterProject.Helpers
{
    public class UserHelper
    {

        #region DI


        private readonly MyDbContext _dbContext;
        private readonly WebsettingHelper _websettinghelper;
        private readonly GlobalHelper _globalHelper;

        //private readonly MembershipHelper _membershipHelper;


        public RegisterSettingsModel _accountcreationsetup { get; set; }
        public UserHelper(MyDbContext context, WebsettingHelper websettinghelper, GlobalHelper globalHelper)
        {
            _dbContext = context;
            _websettinghelper = websettinghelper;
            _globalHelper = globalHelper;
            //_membershipHelper = membershipHelper;
        }
        #endregion

        #region Contact
        public ContactModel Contact(Guid ProfileGuid)
        {

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            ContactModel contact = (from up in _dbContext.UsersProfiles
                                    where up.ProfileGuid == ProfileGuid
                                    select new ContactModel
                                    {

                                        Contactnumber = up.ContactNumber,

                                    }).FirstOrDefault();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return contact;
        }
        #endregion


        #region UserGeneralView

        public UserGeneralView UserGeneralByGUID(Guid ProfileGuid)
        {
            UserGeneralView user = new UserGeneralView();
            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGuid);

            if (up != null)
            {
                if (up.Type == "Client")
                {
                    user.Email = up.Email;
                    user.LoginChannel = up.Loginchannel;
                    user.LoginName = up.UserName;
                    user.ProfileId = up.ProfileId;
                    user.ProfileGuid = up.ProfileGuid;
                    user.Displayname = up.ClientDisplayName!=null ? up.ClientDisplayName: up.Firstname + " " + up.Lastname;
                    user.FirstName = up.Firstname;
                    user.LastName = up.Lastname;
                    user.Image = up.ProfileImage;
                    user.InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString("MMM dd,yyyy");
                    user.About = up.About;
                    user.Type = up.Type;
                    user.Contact = up.ContactNumber!=null ? up.ContactNumber: null;
                    user.userothermetadata = up.OtherMetaData != null ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData) : null;
                    user.sellerviewmodel = null;

                }
                else if (up.Type == "Vendor")
                {
                    user.Email = up.Email;
                    user.LoginChannel = up.Loginchannel;
                    user.LoginName = up.UserName;
                    user.ProfileId = up.ProfileId;
                    user.ProfileGuid = up.ProfileGuid;
                    user.Displayname = up.SellerDisplayName !=null? up.SellerDisplayName: up.Firstname + " " + up.Lastname;
                    user.FirstName = up.Firstname;
                    user.LastName = up.Lastname;
                    user.Image = up.SellerImage != null ? up.SellerImage: up.SellerCoverImage;
                    user.InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString("MMM dd,yyyy");
                    user.About = up.BusinessDescription!=null ? up.BusinessDescription: up.About;
                    user.Type = up.Type;
                     user.Contact = up.ContactNumber != null ? up.ContactNumber : null;
                    user.userothermetadata = up.OtherMetaData != null ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData) : null;
                 
                    user.sellerviewmodel = new SellerViewModel
                    {
                      
                        BusinessName = up.BusinessName != null ? up.BusinessName : up.Firstname + " " + up.Lastname,
                        BusinessUrlpath = up.BusinessUrlpath != null ? "/user/" + up.BusinessUrlpath : "/user/" + up.ProfileGuid.ToString(),
                        // ... (initialize other properties if needed)
                    };

                }


            }




            return user;
        }
        #endregion



        #region Client



        public List<ClientViewModel> ClientList()
        {



            var clientlist = (from up in _dbContext.UsersProfiles



                              select new ClientViewModel
                              {

                                  ProfileId = up.ProfileId,
                                  ProfileGuid = up.ProfileGuid,
                                  Displayname = up.ClientDisplayName ?? up.Firstname + " " + up.Lastname,
                                  //Dateofbirth = up.Dateofbirth != null ? DateTime.Parse(up.Dateofbirth.ToString()).ToString("MMM dd, yyyy") : null,
                                  //Gender = up.Gender,
                                  About = up.About,
                                  Contact = up.ContactNumber,
                                  Email = up.Email,
                                  FirstName = up.Firstname,
                                  LastName = up.Lastname,
                                  Address = up.Address,
                                  //Latitude = up.Latitude,
                                  //Longitude = up.Longitude,
                                  Image = up.ProfileImage,
                                  InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString("MMM dd,yyyy"),
                                  Type = up.Type,
                                  LoginChannel = up.Loginchannel,
                                  LoginName = up.UserName,
                                  //LevelName = _globalHelper.LevelofUser(up.ProfileId, "buyer"),
                                  PurchaseQtyTotal = _dbContext.OrderMasters
    .Where(w => w.BuyerId == up.ProfileId && w.OrderProcessStatus == "completed" && w.ItemType == "item").Count(),

                                  PurchaseAmountTotal = 0,
                                  userothermetadata = up.OtherMetaData != null ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData) : null,

                                  //CreditPurchaseModel = _membershipHelper.CreditPurchaseLifeTime(up.ProfileId)
                                  //loginuserfollow = (loginuserid != 0 && _dbContext.VendorFollows.Any(w => w.ProfileId == loginuserid && w.VendorId == up.ProfileId)) ? true : false

                              });

            return clientlist.ToList();
        }


        public ClientViewModel ClientByGUID(Guid ProfileGuid)
        {



#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            ClientViewModel client = (from up in _dbContext.UsersProfiles
                                      where up.ProfileGuid == ProfileGuid
                                      select new ClientViewModel
                                      {
                                          ProfileId = up.ProfileId,
                                          ProfileGuid = up.ProfileGuid,
                                          Displayname = up.ClientDisplayName ?? up.Firstname + " " + up.Lastname,
                                          //Dateofbirth = up.Dateofbirth != null ? DateTime.Parse(up.Dateofbirth.ToString()).ToString("MMM dd, yyyy") : null,
                                          //Gender = up.Gender,
                                          About = up.About,
                                          Contact = up.ContactNumber,
                                          Email = up.Email,
                                          FirstName = up.Firstname,
                                          LastName = up.Lastname,
                                          Address = up.Address,
                                          //Latitude = up.Latitude,
                                          //Longitude = up.Longitude,
                                          Image = up.ProfileImage,
                                          InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString("MMM dd,yyyy"),
                                          userothermetadata = up.OtherMetaData != null ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData) : null
                                      }).FirstOrDefault();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return client;
        }


        #endregion


        #region Seller
        public List<SellerViewModel> SellerList()
        {



            var sellerlist = (from up in _dbContext.UsersProfiles



                              select new SellerViewModel
                              {

                                  ProfileId = up.ProfileId,
                                  ProfileGuid = up.ProfileGuid,
                                  Displayname = up.SellerDisplayName ?? up.Firstname + " " + up.Lastname,
                                  //Dateofbirth = up.Dateofbirth != null ? DateTime.Parse(up.Dateofbirth.ToString()).ToString("MMM dd, yyyy") : null,
                                  //Gender = up.Gender,
                                  About = up.About,
                                  Contact = up.ContactNumber,
                                  Email = up.Email,
                                  FirstName = up.Firstname,
                                  LastName = up.Lastname,
                                  Address = up.Address,
                                  //Latitude = up.Latitude,
                                  //Longitude = up.Longitude,
                                  Image = up.SellerImage != null ? up.SellerImage.ToString() : up.SellerCoverImage != null ? up.SellerCoverImage.ToString() : "/images/no-image.png",
                                  SellerCoverImage = up.SellerCoverImage != null ? up.SellerCoverImage.ToString() : "/images/no-image.png",
                                  SellerVideoURl = up.SellerVideoURl,
                                  InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString("MMM dd,yyyy"),

                                  //Followers = up.Followers,
                                  //Averagerating = up.Averagerating,
                                  BusinessTypeName = up.BusinessType.ToString().Replace("0", "Individual").Replace("1", "Business"),
                                  BusinessType = up.BusinessType,
                                  BusinessName = up.BusinessName ?? up.Firstname + " " + up.Lastname,
                                  BusinessUrlpath = up.BusinessUrlpath != null ? up.BusinessUrlpath : up.ProfileGuid.ToString(),
                                  BusinessDescription = up.BusinessDescription,
                                  Type = up.Type,
                                  LoginChannel = up.Loginchannel,
                                  LoginName = up.UserName,
                                  //LevelName = _globalHelper.LevelofUser(up.ProfileId, "seller"),
                                  ProductTotal = _dbContext.ItemListings.Count(w => w.ProfileId == up.ProfileId && w.IsPublish == true),
                                  SalesQtyTotal = _dbContext.OrderMasters
    .Where(w => w.SellerId == up.ProfileId && w.OrderProcessStatus == "completed" && w.ItemType == "item").Count(),
                                  PurchaseQtyTotal = _dbContext.OrderMasters
    .Where(w => w.BuyerId == up.ProfileId && w.OrderProcessStatus == "completed" && w.ItemType == "item").Count(),

                                  userothermetadata = up.OtherMetaData != null ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData) : null,

                                  BusinessMetaData = up.BusinessMetaData != null ? JsonConvert.DeserializeObject<BusinessMetaData>(up.BusinessMetaData) : null
                                  ///credit model
                                  //CreditPurchaseModel = _membershipHelper.CreditPurchaseLifeTime(up.ProfileId)



                                  //  _dbContext.OrderMasters
                                  //.Where(w => w.SellerId == up.ProfileId && w.OrderProcessStatus =="completed").Count(),


                                  //loginuserfollow = (loginuserid != 0 && _dbContext.VendorFollows.Any(w => w.ProfileId == loginuserid && w.VendorId == up.ProfileId)) ? true : false

                                 
                              }) ;

            return sellerlist.ToList();
        }


        public SellerViewModel SellerByGUID(Guid ProfileGuid)
        {



#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            SellerViewModel seller = (from up in _dbContext.UsersProfiles
                                      where up.ProfileGuid == ProfileGuid


                                      select new SellerViewModel
                                      {

                                          ProfileId = up.ProfileId,
                                          ProfileGuid = up.ProfileGuid,
                                          Displayname = up.SellerDisplayName ?? up.Firstname + " " + up.Lastname,
                                          //Dateofbirth = up.Dateofbirth != null ? DateTime.Parse(up.Dateofbirth.ToString()).ToString("MMM dd, yyyy") : null,
                                          //Gender = up.Gender,
                                          About = up.About,
                                          Contact = up.ContactNumber,
                                          Email = up.Email,
                                          FirstName = up.Firstname,
                                          LastName = up.Lastname,
                                          Address = up.Address,
                                          //Latitude = up.Latitude,
                                          //Longitude = up.Longitude,
                                          //Image = up.SellerImage,
                                          //SellerCoverImage = up.SellerCoverImage,

                                          Image = up.SellerImage != null ? up.SellerImage.ToString() : up.SellerCoverImage != null ? up.SellerCoverImage.ToString() : "/images/no-image.png",
                                          SellerCoverImage = up.SellerCoverImage != null ? up.SellerCoverImage.ToString() : "/images/no-image.png",
                                          SellerVideoURl = up.SellerVideoURl,
                                          InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString("MMM dd,yyyy"),

                                          //Followers = up.Followers,
                                          //Averagerating = up.Averagerating,
                                          BusinessType = up.BusinessType,
                                          BusinessName = up.BusinessName ?? up.Firstname + " " + up.Lastname,
                                          BusinessUrlpath = up.BusinessUrlpath != null ? up.BusinessUrlpath : up.ProfileGuid.ToString(),
                                          BusinessDescription = up.BusinessDescription,

                                          ProductTotal = _dbContext.ItemListings.Count(w => w.ProfileId == up.ProfileId && w.IsPublish == true),

                                          userothermetadata = up.OtherMetaData != null ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData) : null


                                          //loginuserfollow = (loginuserid != 0 && _dbContext.VendorFollows.Any(w => w.ProfileId == loginuserid && w.VendorId == up.ProfileId)) ? true : false

                                      }).FirstOrDefault();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return seller;
        }


        public SellerViewModel SellerByBusinessUrlPath(string businessurlpath)
        {



#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.


            
            SellerViewModel seller = (from up in _dbContext.UsersProfiles
                                      where up.BusinessUrlpath == businessurlpath ||
                                   ( up.ProfileGuid.ToString() == businessurlpath)

                                      select new SellerViewModel
                                      {

                                          ProfileId = up.ProfileId,
                                          ProfileGuid = up.ProfileGuid,
                                          Displayname = up.SellerDisplayName ?? up.Firstname + " " + up.Lastname,

                                         
                                          //Dateofbirth = up.Dateofbirth != null ? DateTime.Parse(up.Dateofbirth.ToString()).ToString("MMM dd, yyyy") : null,
                                          //Gender = up.Gender,
                                          About = up.About,
                                          Contact = up.ContactNumber,
                                          Email = up.Email,
                                          FirstName = up.Firstname,
                                          LastName = up.Lastname,
                                          Address = up.Address,
                                          //Latitude = up.Latitude,
                                          //Longitude = up.Longitude,
                                          Image = up.SellerImage != null ? up.SellerImage.ToString() : up.SellerCoverImage != null ? up.SellerCoverImage.ToString() : "/images/no-image.png",
                                          SellerCoverImage = up.SellerCoverImage != null ? up.SellerCoverImage.ToString() : "/images/no-image.png",
                                          SellerVideoURl = up.SellerVideoURl,
                                          InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString("MMM dd,yyyy"),

                                          //Followers = up.Followers,
                                          //Averagerating = up.Averagerating,
                                          BusinessTypeName = up.BusinessType.ToString().Replace("0", "Individual").Replace("1", "Business"),
                                          BusinessType = up.BusinessType,
                                          BusinessName = up.BusinessName ?? up.Firstname + " " + up.Lastname,
                                          BusinessUrlpath = up.BusinessUrlpath != null ? up.BusinessUrlpath : up.ProfileGuid.ToString(),
                                          BusinessDescription = up.BusinessDescription,
                                          ProductTotal = _dbContext.ItemListings.Count(w => w.ProfileId == up.ProfileId && w.IsPublish == true),
                                          


                                          userothermetadata = up.OtherMetaData != null ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData) : null,
                                          primaryaddressViewModel =addressmetadataDeserialized(up.PrimaryAddressMetaData),
                                          BusinessMetaData = ParseMetaDataBusinessInfo(up.BusinessMetaData),

                                          SellerAvailability= AvailabilityParseData(up.AvailabilitySetupMetaData),
                                          SecondaryAddressList= ParseMetaDataAddressList(up.SecondaryAddressMetaData),

                                          CertificateList=ParseMetaDataCertificateList(up.CertificateProofMetaData),
                                          TeamList = ParseMetaDataTeamList(up.TeamMembersMetaData),

                                          SocialMediaList= ParseMetaDataSellerSocialMediaList(up.SocialMediaMetaData),
                                          //loginuserfollow = (loginuserid != 0 && _dbContext.VendorFollows.Any(w => w.ProfileId == loginuserid && w.VendorId == up.ProfileId)) ? true : false

                                      }).FirstOrDefault();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return seller;
        }



        public string BusinessURLValidation(string businessurlpath, Guid ProfileGUID)
        {
            string message = "exist";
            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.BusinessUrlpath.Trim().ToLower() == businessurlpath.Trim().ToLower() && u.ProfileGuid != ProfileGUID);

            if (up == null)
            {
                message = "notexist";
            }

            return message;

        }


        public ProfileCompletionMetaData profilecompletestatus(string profilemetadata)
        {
            var _sellerprofileSettings = _websettinghelper.GetWebsettingJson("SellerProfileSettings");


            var sellerdata = ParseMetaDataProfileCompletion(profilemetadata);
            var admindata = ParseMetaDataProfileCompletion(_sellerprofileSettings);


            var profileCompletionMetaData = new ProfileCompletionMetaData
            {
                sellersetting = sellerdata,
                adminsetting = admindata,
            };

            return profileCompletionMetaData;
        }


        public string UserEmailByID(int profileid)
        {
            // Assuming your UsersProfiles table has an Email property
            var userEmail = _dbContext.UsersProfiles
                .Where(u => u.ProfileId == profileid)
                .Select(u => u.Email)
                .FirstOrDefault();

            return userEmail;
        }

        #endregion


        #region SellerSocialMediaList-Deseralized
        public static List<SellerSocialMediaModel> ParseMetaDataSellerSocialMediaList(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                // Return a new list if the json string is null or empty
                return new List<SellerSocialMediaModel>();
            }

            // Deserialize the JSON string into a list of SellerSocialMediaModel objects
            var parsedData = JsonConvert.DeserializeObject<List<SellerSocialMediaModel>>(json);
            return parsedData ?? new List<SellerSocialMediaModel>();
        }
        #endregion


        #region Follow
        public string followuser(int buyerid, int sellerid)
        {
            string message = string.Empty;
            VendorFollow vf = _dbContext.VendorFollows.FirstOrDefault(u => u.ProfileId == buyerid && u.VendorId == sellerid);

            ///remove follower
            if (vf != null)
            {
                _dbContext.VendorFollows.Remove(vf);

                UserOtherMetaDataUpdate(sellerid, "-1", "followers");
                UserOtherMetaDataUpdate(buyerid, "-1", "following");

                //UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId == sellerid);

                //if (up != null)
                //{
                //    up.Followers = up.Followers - 1;

                //}

                //_dbContext.SaveChanges();
                message = "Removed";
            }

            else
            {

                VendorFollow vfi = new VendorFollow();

                vfi.ProfileId = buyerid;
                vfi.VendorId = sellerid;
                vfi.InsertDate = DateTime.Now;
                _dbContext.VendorFollows.Add(vfi);


                UserOtherMetaDataUpdate(sellerid, "+1", "followers");
                UserOtherMetaDataUpdate(buyerid, "+1", "following");
                //UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId == sellerid);

                //if (up != null)
                //{
                //    //if (up.Followers == null)
                //    //{
                //    //    up.Followers = 0;
                //    //}
                //    //up.Followers += 1;

                //}
                //_dbContext.SaveChanges();
                message = "Add";

            }


            return message;
        }



        public string followuserstatus(int buyerid, int sellerid)
        {
            string message = string.Empty;
            VendorFollow vf = _dbContext.VendorFollows.FirstOrDefault(u => u.ProfileId == buyerid && u.VendorId == sellerid);

            ///remove follower
            if (vf != null)
            {
                message = "Un Follow";
            }

            else
            {

                message = "Follow";


            }


            return message;
        }
        #endregion

        #region AccountDelete
        public string AccountDelete(int profileid)
        {
            string message = string.Empty;
            try
            {
                ///remove all orders, listing, inbox


                UsersProfile up = _dbContext.UsersProfiles.First(u => u.ProfileId == profileid);

                if (up != null)
                {
                    up.UserName = up.UserName + "-Deleted";
                    up.DeletedDate = DateTime.Now;
                    up.IsDeleted = true;
                    //_dbContext.UsersProfiles.Remove(up);
                    _dbContext.SaveChanges();

                    message = "success";
                }

            }
            catch (Exception ex)
            {

                message = ex.Message;
            }

            return message;
        }

        #endregion

        #region AccountLocked
        public string AccountLocked(int profileid, string remarks, bool islocked, DateTime unlockdate)
        {
            string message = string.Empty;
            try
            {
                UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId == profileid);

                if (up != null)
                {
                    up.AdminRemarksOnLocked = remarks;
                    up.IsLockedByAdmin = islocked;
                    up.UnLockedDate = unlockdate;
                    up.AdminStatusMetaData = userLockmetadata(remarks, islocked, unlockdate, up.AdminStatusMetaData);
                    _dbContext.SaveChanges();

                    message = "success";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }

        public string userLockmetadata(string remarks, bool islocked, DateTime unlockdate, string existingMetaData)
        {
            // Deserialize the existing metadata JSON string into a list of UserLockMetaData
            List<UserLockMetaData> existingMetadata = JsonConvert.DeserializeObject<List<UserLockMetaData>>(existingMetaData ?? "[]");

            // Create a new instance of UserLockMetaData
            var newMetadata = new UserLockMetaData
            {
                Remarks = remarks,
                IsLock = islocked,
                UnlockDate = unlockdate
            };

            // Add the new metadata to the existing list
            existingMetadata.Add(newMetadata);

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }
        #endregion



        #region Address-JsonCreator
        public string addressmetadata(string address, string latitude, string longitude, string country, string city, string state, string zipcode, string countryimageurlpath, string country2digitcode, string timezone) //
        {

            #region CreateJsonForComments

            string addressmetadata = string.Empty;
            // Create a new instance of AddressViewModel and assign the values
            var metadata = new AddressViewModel
            {
                Address = address,
                Latitude = latitude,
                Longitude = longitude,
                Country = country,
                City = city,
                State = state,
                ZipCode = zipcode,
                CountryImageURL=countryimageurlpath,
                Country2DigitCode =country2digitcode,
                TimeZone= timezone




            };

            // Serialize the object to JSON
            addressmetadata = JsonConvert.SerializeObject(metadata);
            #endregion


            return addressmetadata;
        }

        //Address-JsonDeserialized
        public static AddressViewModel addressmetadataDeserialized(string json) //
        {

            #region CreateJsonForComments
            if (json == null)
            {
                // Return a new instance of SellerProfileSettingsModel if json is null
                return new AddressViewModel();
            }


            var parsedData = JsonConvert.DeserializeObject<AddressViewModel>(json);
            return parsedData;

            #endregion


            
        }
        #endregion

        #region BusinessInfo-JsonCreator
        public string businessinfometadata(string dateofbirth, string gender, int foundingyear, int noofemployee) //
        {

            #region CreateJsonForComments

            string businessinfometadata = string.Empty;
            DateTime? parsedDateOfBirth = !string.IsNullOrEmpty(dateofbirth) ? DateTime.Parse(dateofbirth) : (DateTime?)null;

            // Create a new instance of CreditCommentViewModel and assign the values
            var metadata = new BusinessMetaData
            {
                Dateofbirth = parsedDateOfBirth,
                Gender = gender,
                FoundingYear = foundingyear,
                NoOfEmployee = noofemployee



            };

            // Serialize the object to JSON
            businessinfometadata = JsonConvert.SerializeObject(metadata);
            #endregion


            return businessinfometadata;
        }



        #endregion

        #region BusinessInfo- Deserialized
        public static BusinessMetaData ParseMetaDataBusinessInfo(string json)
        {
            // Perform the necessary JSON parsing and mapping here
            // You can use JsonConvert.DeserializeObject or any other JSON library of your choice

            // Example: Assuming the JSON structure matches the desired CreditCommentViewModel properties
            if (json == null)
            {
                // Return a new instance of SellerProfileSettingsModel if json is null
                return new BusinessMetaData();
            }


            var parsedData = JsonConvert.DeserializeObject<BusinessMetaData>(json);
            return parsedData;




        }
        #endregion


        #region ContactInfo-JsonCreator
        public string contactmetadata(string contactguid, string contactid, string name, string type, string contact, string existingMetaData)
        {
            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<ContactMetaData> existingMetadata = JsonConvert.DeserializeObject<List<ContactMetaData>>(existingMetaData ?? "[]");

            if (string.IsNullOrEmpty(contactguid))
            {
                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata
                int nextContactId = existingMetadata.Count + 1;
                contactguid = Guid.NewGuid().ToString();
                contactid = nextContactId.ToString();

                // Create a new instance of ContactMetaData
                var newMetadata = new ContactMetaData
                {
                    ContactGUID = Guid.Parse(contactguid),
                    ContactID = int.Parse(contactid),
                    Name = name,
                    Type = type,
                    Contact = contact,


                };

                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);
            }
            else
            {
                // Updating an existing record

                // Find the existing metadata record with the matching ContactGUID
                ContactMetaData existingRecord = existingMetadata.FirstOrDefault(m => m.ContactGUID.ToString() == contactguid);

                if (existingRecord != null)
                {
                    // Update the properties of the existing record

                    existingRecord.Name = name;
                    existingRecord.Type = type;
                    existingRecord.Contact = contact;



                }
            }

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }

        #endregion

        #region ContactInfor-Deserialized

        public List<ContactMetaData> ParseMetaDataContactList(string json)
        {
            if (json == null)
            {
                return new List<ContactMetaData>(); // Return an empty list
            }

            List<ContactMetaData> parsedData = JsonConvert.DeserializeObject<List<ContactMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }




        public ContactMetaData ParseMetaDataContact(string json)
        {
            // Perform the necessary JSON parsing and mapping here
            // You can use JsonConvert.DeserializeObject or any other JSON library of your choice

            // Example: Assuming the JSON structure matches the desired CreditCommentViewModel properties
            if (json == null)
            {
                // Return a new instance of SellerProfileSettingsModel if json is null
                return new ContactMetaData();
            }


            var parsedData = JsonConvert.DeserializeObject<ContactMetaData>(json);
            return parsedData;




        }
        #endregion


        #region TeamMember-JsonCreator
        public string teammetadata(string teamguid, string teamid, string name, string role, string speciality, string experience, string contactnumber, string email, int yearsofexperience, bool iscreatelogin, string image, string existingMetaData)
        {
            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<TeamMetaData> existingMetadata = JsonConvert.DeserializeObject<List<TeamMetaData>>(existingMetaData ?? "[]");

            if (string.IsNullOrEmpty(teamguid))
            {
                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata
                int nextContactId = existingMetadata.Count + 1;
                teamguid = Guid.NewGuid().ToString();
                teamid = int.Parse(GlobalHelper.RandomNumber()) + nextContactId.ToString();

                // Create a new instance of ContactMetaData
                var newMetadata = new TeamMetaData
                {
                    TeamGUID = Guid.Parse(teamguid),
                    TeamID = int.Parse(teamid),
                    Name = name,
                    Role = role,
                    Experience = experience,
                    Speciality = speciality,
                    ContactNumber = contactnumber,
                    Email = email,

                    YearsOfExperience = yearsofexperience,

                    IsCreateLogin = iscreatelogin,
                    Image = image,
                    InsertDate = DateTime.Now
                };

                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);
            }
            else
            {
                // Updating an existing record

                // Find the existing metadata record with the matching ContactGUID
                TeamMetaData existingRecord = existingMetadata.FirstOrDefault(m => m.TeamGUID.ToString() == teamguid);

                if (existingRecord != null)
                {
                    // Update the properties of the existing record

                    existingRecord.Name = name;
                    existingRecord.Role = role;
                    existingRecord.Speciality = speciality;
                    existingRecord.Experience = experience;
                    existingRecord.ContactNumber = contactnumber;
                    existingRecord.Email = email;

                    existingRecord.YearsOfExperience = yearsofexperience;

                    existingRecord.Image = image;
                    existingRecord.UpdateDate = DateTime.Now;
                }
            }

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }
        #endregion

        #region TeamMember-Deserialized
        public static List<TeamMetaData> ParseMetaDataTeamList(string json)
        {
            if (json == null)
            {
                return new List<TeamMetaData>(); // Return an empty list
            }

            List<TeamMetaData> parsedData = JsonConvert.DeserializeObject<List<TeamMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion


        #region Identity-JsonCreator
        public string identitymetadata(string identityguid, string identityid, string type, string proof, string existingMetaData)
        {
            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<IdentityMetaData> existingMetadata = JsonConvert.DeserializeObject<List<IdentityMetaData>>(existingMetaData ?? "[]");

            if (string.IsNullOrEmpty(identityguid))
            {
                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata
                int nextContactId = existingMetadata.Count + 1;
                identityguid = Guid.NewGuid().ToString();
                identityid = nextContactId.ToString();

                // Create a new instance of ContactMetaData
                var newMetadata = new IdentityMetaData
                {
                    IdentityGUID = Guid.Parse(identityguid),
                    IdentityID = int.Parse(identityid),
                    IdentityType = type,
                    IdentityProof = proof,
                    InsertDate = DateTime.Now,
                    Status = "Pending",
                    Remarks = ""

                };

                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);
            }
            else
            {
                // Updating an existing record

                // Find the existing metadata record with the matching ContactGUID
                IdentityMetaData existingRecord = existingMetadata.FirstOrDefault(m => m.IdentityGUID.ToString() == identityguid);

                if (existingRecord != null)
                {
                    // Update the properties of the existing record


                    existingRecord.IdentityProof = proof;
                    existingRecord.IdentityType = type;
                    existingRecord.UpdateDate = DateTime.Now;


                }
            }

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }
        #endregion


        #region Identity-Deserialized
        public List<IdentityMetaData> ParseMetaDataIdentityList(string json)
        {
            if (json == null)
            {
                return new List<IdentityMetaData>(); // Return an empty list
            }

            List<IdentityMetaData> parsedData = JsonConvert.DeserializeObject<List<IdentityMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion

        #region Certificate-JsonCreator

        public string certificatemetadata(string certificateguid, string certificateid, string attachment, string name, string institute, string coursecontent, string existingMetaData)
        {
            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<CertificateMetaData> existingMetadata = JsonConvert.DeserializeObject<List<CertificateMetaData>>(existingMetaData ?? "[]");

            if (string.IsNullOrEmpty(certificateguid))
            {
                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata
                int nextContactId = existingMetadata.Count + 1;
                certificateguid = Guid.NewGuid().ToString();
                certificateid = nextContactId.ToString();

                // Create a new instance of ContactMetaData
                var newMetadata = new CertificateMetaData
                {
                    CertificateGUID = Guid.Parse(certificateguid),
                    CertificateID = int.Parse(certificateid),
                    InstituteName = institute,
                    CertificateAttachment = attachment,
                    CertificateName = name,
                    CourseContent = coursecontent,
                    InsertDate = DateTime.Now,



                };

                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);
            }
            else
            {
                // Updating an existing record

                // Find the existing metadata record with the matching ContactGUID
                CertificateMetaData existingRecord = existingMetadata.FirstOrDefault(m => m.CertificateGUID.ToString() == certificateguid);

                if (existingRecord != null)
                {
                    // Update the properties of the existing record


                    existingRecord.InstituteName = institute;
                    existingRecord.CertificateAttachment = attachment;
                    existingRecord.CertificateName = name;
                    existingRecord.CourseContent = coursecontent;
                    existingRecord.UpdateDate = DateTime.Now;


                }
            }

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }

        #endregion

        #region Certificate-Deserialized
        public static List<CertificateMetaData> ParseMetaDataCertificateList(string json)
        {
            if (json == null)
            {
                return new List<CertificateMetaData>(); // Return an empty list
            }

            List<CertificateMetaData> parsedData = JsonConvert.DeserializeObject<List<CertificateMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion


        #region AddressSecondary-JsonCreator
        public string addressmetadata(string addressguid, string addressid, string address, string type, string latitude, string longitude, string country, string state, string city, string zipcode, string existingMetaData)
        {
            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<AddressMetaData> existingMetadata = JsonConvert.DeserializeObject<List<AddressMetaData>>(existingMetaData ?? "[]");

            if (string.IsNullOrEmpty(addressguid))
            {
                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata
                int nextContactId = existingMetadata.Count + 1;
                addressguid = Guid.NewGuid().ToString();
                addressid = int.Parse(GlobalHelper.RandomNumber()) + nextContactId.ToString();

                // Create a new instance of ContactMetaData
                var newMetadata = new AddressMetaData
                {
                    AddressGUID = Guid.Parse(addressguid),
                    AddressID = int.Parse(addressid),
                    Address = address,
                    Type = type,
                    Latitude = latitude,
                    Longitude = longitude,
                    Country = country,
                    State = state,
                    City = city,
                    ZipCode = zipcode,

                };

                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);
            }
            else
            {
                // Updating an existing record

                // Find the existing metadata record with the matching ContactGUID
                AddressMetaData existingRecord = existingMetadata.FirstOrDefault(m => m.AddressGUID.ToString() == addressguid);

                if (existingRecord != null)
                {
                    // Update the properties of the existing record

                    existingRecord.Address = address;
                    existingRecord.Type = type;
                    existingRecord.Latitude = latitude;
                    existingRecord.Longitude = longitude;
                    existingRecord.Country = country;
                    existingRecord.State = state;
                    existingRecord.City = city;
                    existingRecord.ZipCode = zipcode;


                }
            }

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }

        #endregion

        #region AddressSecondary-Deserialized
        public static List<AddressMetaData> ParseMetaDataAddressList(string json)
        {
            if (json == null)
            {
                return new List<AddressMetaData>(); // Return an empty list
            }

            List<AddressMetaData> parsedData = JsonConvert.DeserializeObject<List<AddressMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion


        #region ProfileCompletion-JsonCreator
        public string profilecompletionmetadata(bool profile, bool BusinessInfo, bool Contact, bool Address, bool identityproof, bool Certificate, bool SocialMediaLink, bool TeamMemmber) //
        {

            #region CreateJsonForComments

            string profilemetadata = string.Empty;
            // Create a new instance of CreditCommentViewModel and assign the values
            var metadata = new SellerProfileSettingsModel
            {
                Profile = profile,
                BusinessInfo = BusinessInfo,
                SecondaryContactDetails = Contact,
                SecondaryAddress = Address,
                IdentityProof = identityproof,
                Certificate = Certificate,
                SocialMediaLink = SocialMediaLink,
                TeamMemmber = TeamMemmber,



            };

            // Serialize the object to JSON
            profilemetadata = JsonConvert.SerializeObject(metadata);
            #endregion


            return profilemetadata;
        }
        #endregion

        #region ProfileCompletion- Deserialized
        public SellerProfileSettingsModel ParseMetaDataProfileCompletion(string json)
        {
            if (json == null)
            {
                // Return a new instance of SellerProfileSettingsModel if json is null
                return new SellerProfileSettingsModel();
            }

            var parsedData = JsonConvert.DeserializeObject<SellerProfileSettingsModel>(json);
            return parsedData;

        }



        #endregion



        #region SocialMedia-JsonCreator
        public string socialMediametadata(int socialmediaid, int userprofileId, string url)
        {
            bool iscredit = false;
            int numberofcreditused = 0;
            int numberofexpirydays = 0;
            int SellerSocialMediaID = 0;

            Websetting websiteSetup = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "SocialMediaSettings");

            List<SocialMediaSettingViewModel> socialMediaList = new List<SocialMediaSettingViewModel>();
            SocialMediaSettingViewModel targetSocialMedia = null;

            if (websiteSetup != null)
            {
                // Assuming that the JSON data is stored as a string in the "ItemValue" field of the websiteSetup object
                string jsonData = websiteSetup.WebsettingValue;

                // Deserialize the JSON string into a list of SocialMediaSettingViewModel objects
                socialMediaList = JsonConvert.DeserializeObject<List<SocialMediaSettingViewModel>>(jsonData);

                // Find the record in the list by ID
                targetSocialMedia = socialMediaList.FirstOrDefault(item => item.ID == socialmediaid);

                if (targetSocialMedia != null)
                {
                    iscredit = targetSocialMedia.Credit > 0; // if greater than 0, set to true, else set to false
                    numberofcreditused = targetSocialMedia.Credit;
                    numberofexpirydays = targetSocialMedia.NumberOfExpiryDays;
                }
            }

            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId == userprofileId);

            if (up != null)
            {
                // Deserialize the existing metadata JSON string into a list of SellerSocialMediaModel
                List<SellerSocialMediaModel> existingMetadata = JsonConvert.DeserializeObject<List<SellerSocialMediaModel>>(up.SocialMediaMetaData ?? "[]");

                SellerSocialMediaModel existingRecord = existingMetadata.FirstOrDefault(m => m.SocialMediaID == socialmediaid);

                if (existingRecord != null)
                {
                    // Update existing record
                    existingRecord.URL = url;
                    existingRecord.IsCreditUsed = iscredit;
                    existingRecord.NumberofCredit = numberofcreditused;
                    existingRecord.CreditUsedDate = DateTime.Now;
                    existingRecord.ExpiryDate = numberofexpirydays != 0 ? (DateTime?)DateTime.Now.AddDays(numberofexpirydays) : null;
                }
                else
                {
                    // Adding a new record
                    SellerSocialMediaID = int.Parse(GlobalHelper.RandomNumber().ToString());

                    // Create a new instance of SellerSocialMediaModel
                    var newMetadata = new SellerSocialMediaModel
                    {
                        SellerSocialMediaID = SellerSocialMediaID,
                        SocialMediaID = socialmediaid,
                        URL = url,
                        IsCreditUsed = iscredit,
                        NumberofCredit = numberofcreditused,
                        CreditUsedDate = DateTime.Now,
                        ExpiryDate = numberofexpirydays != 0 ? (DateTime?)DateTime.Now.AddDays(numberofexpirydays) : null
                    };

                    // Add the new metadata to the existing list
                    existingMetadata.Add(newMetadata);
                }

                // Serialize the updated list back to JSON
                string updatedJson = JsonConvert.SerializeObject(existingMetadata);



                up.SocialMediaMetaData = updatedJson;
                _dbContext.SaveChanges();

            }

            return "success";
        }
        public string SellersocialMediaDelete(int Sellersocialmediaid, int userprofileId)
        {


            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId == userprofileId);

            if (up != null)
            {
                // Deserialize the existing metadata JSON string into a list of SellerSocialMediaModel
                List<SellerSocialMediaModel> existingMetadata = JsonConvert.DeserializeObject<List<SellerSocialMediaModel>>(up.SocialMediaMetaData ?? "[]");

                SellerSocialMediaModel existingRecord = existingMetadata.FirstOrDefault(m => m.SellerSocialMediaID == Sellersocialmediaid);

                if (existingRecord != null)
                {
                    // Update existing record
                    // Remove the existing record from the list
                    existingMetadata.Remove(existingRecord);

                }

                // Serialize the updated list back to JSON
                string updatedJson = JsonConvert.SerializeObject(existingMetadata);


                up.SocialMediaMetaData = updatedJson;
                _dbContext.SaveChanges();

            }

            return "success";
        }

        #endregion



        #region SellerAvailabilitySetup

        public string availabilitySetupmetadata(int userprofileId, string day, bool IsDayEnable, bool iscustomtime, string fromtime, string totime)
        {


            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId == userprofileId);

            if (up != null)
            {

                List<SellerAvailabilityModel> existingMetadata = JsonConvert.DeserializeObject<List<SellerAvailabilityModel>>(up.AvailabilitySetupMetaData ?? "[]");

                SellerAvailabilityModel existingRecord = existingMetadata.FirstOrDefault(m => m.Day.ToLower() == day.ToLower());

                if (existingRecord != null)
                {
                    // Update existing record
                    existingRecord.Day = day;
                    existingRecord.IsDayEnable = IsDayEnable;
                    existingRecord.IsCustomTiming = iscustomtime;
                    existingRecord.FromTime = fromtime;
                    existingRecord.ToTime = totime;
                    existingRecord.UpdatedDate = DateTime.Now;
                }
                else
                {

                    var newMetadata = new SellerAvailabilityModel
                    {
                        Day = day,
                        IsDayEnable = IsDayEnable,
                        IsCustomTiming = iscustomtime,
                        FromTime = iscustomtime == true ? fromtime : null,
                        ToTime = iscustomtime == true ? totime : null,
                        UpdatedDate = DateTime.Now,

                    };

                    // Add the new metadata to the existing list
                    existingMetadata.Add(newMetadata);
                }

                // Serialize the updated list back to JSON
                //string updatedJson = JsonConvert.SerializeObject(existingMetadata);

                // Sort the list by day name (Monday, Tuesday, Wednesday, etc.)
                existingMetadata = existingMetadata.OrderBy(m => GetDayNumber(m.Day)).ToList();

                // Serialize the updated list back to JSON
                string updatedJson = JsonConvert.SerializeObject(existingMetadata);


                up.AvailabilitySetupMetaData = updatedJson;
                _dbContext.SaveChanges();

            }

            return "success";
        }


        // Helper method to get the day number based on day name
        private int GetDayNumber(string dayName)
        {
            string[] daysOfWeek = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            return Array.IndexOf(daysOfWeek, dayName);
        }

        public static List<SellerAvailabilityModel> AvailabilityParseData(string json)
        {
            if (json == null)
            {
                // Return an empty list if json is null
                return new List<SellerAvailabilityModel>();
            }

            var parsedData = JsonConvert.DeserializeObject<List<SellerAvailabilityModel>>(json);
            return parsedData;
        }


        #endregion


        #region UserOther-JsonCreator
        public string userothermetadata() //
        {

            #region CreateJsonForComments
            string ip = GlobalHelper.IPAddress();
            //string country = _globalHelper.GetCountryOnIP(ip);

            
            Dictionary<string, string> locationData = _globalHelper.GetLocationOnIP(ip);

          

            string Userothermetadata = string.Empty;

            // Create a new instance of CreditCommentViewModel and assign the values
            var metadata = new UserOtherMetaData
            {
                IP = ip,
                Country = locationData["Country"],
                CountryFlag = "/countryflags/" + locationData["Country"] + ".png",
                City = locationData["City"],
                TimeZone = locationData["Timezone"],
                Followers = 0,
                TotalReviewsAsBuyer = 0,
                TotalReviewsAsSeller = 0,
                BuyerRating = 0,
                SellerRating = 0,
                LastSeen = DateTime.Now,
                IsVerifiedBuyer = false,
                IsVerifiedSeller = false,

            };

            // Serialize the object to JSON
            Userothermetadata = JsonConvert.SerializeObject(metadata);
            #endregion


            return Userothermetadata;
        }



        #endregion


        #region UserOther- ExistingUpdate
        public void UserOtherMetaDataUpdate(int userid, string value, string type) //
        {
            UserOtherMetaData existingMetaData = new UserOtherMetaData();
            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId == userid);
           

            if (up != null)
            {
                existingMetaData = up.OtherMetaData != null
                    ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData)
                    : new UserOtherMetaData(); // Initialize with an empty instance if null
            }

            #region CreateJsonForComments


            if (type == "country")
            {
                existingMetaData.Country = value;
            }

            if (type == "followers")
            {

                if (existingMetaData.Followers == null)
                {
                    existingMetaData.Followers = 0;
                }


                existingMetaData.Followers = existingMetaData.Followers + int.Parse(value);
            }

            if (type == "following")
            {

                if (existingMetaData.Following == null)
                {
                    existingMetaData.Following = 0;
                }


                existingMetaData.Following = existingMetaData.Following + int.Parse(value);
            }

            if (type == "BuyerRating")
            {

                if (existingMetaData.TotalReviewsAsBuyer == null)
                {
                    existingMetaData.TotalReviewsAsBuyer = 0;
                }


                existingMetaData.TotalReviewsAsBuyer = existingMetaData.TotalReviewsAsBuyer + 1;
            }
            if (type == "SellerRating")
            {

                if (existingMetaData.TotalReviewsAsSeller == null)
                {
                    existingMetaData.TotalReviewsAsSeller = 0;
                }

                existingMetaData.TotalReviewsAsSeller = existingMetaData.TotalReviewsAsSeller + 1;
            }

            if (type == "BuyerRating")
            {

                if (existingMetaData.BuyerRating == null)
                {
                    existingMetaData.BuyerRating = 0;
                }

                if (existingMetaData.BuyerRating == 0)
                {
                    existingMetaData.BuyerRating = (existingMetaData.BuyerRating + decimal.Parse(value));
                }

                if (existingMetaData.BuyerRating > 0)
                {
                    existingMetaData.BuyerRating = (existingMetaData.BuyerRating + decimal.Parse(value)) / 2;
                }

            }

            if (type == "SellerRating")
            {

                if (existingMetaData.SellerRating == null)
                {
                    existingMetaData.SellerRating = 0;
                }

                if (existingMetaData.SellerRating == 0)
                {
                    existingMetaData.SellerRating = (existingMetaData.SellerRating + decimal.Parse(value));
                }

                if (existingMetaData.SellerRating > 0)
                {
                    existingMetaData.SellerRating = (existingMetaData.SellerRating + decimal.Parse(value)) / 2;
                }

             

            }

            if (type == "LastSeen")
            {
                existingMetaData.LastSeen = DateTime.Now;
            }

            if (type == "UserAgent")
            {
                // Check if UserAgentMetaData list is null, if so, initialize it
                if (existingMetaData.UserAgentMetaData == null)
                {
                    existingMetaData.UserAgentMetaData = new List<UserAgentMetaData>();
                }

                // Parse the string value into UserAgentMetaData
                UserAgentMetaData userAgentValue = JsonConvert.DeserializeObject<UserAgentMetaData>(value);

                // Check if the new UserAgentMetaData already exists
                var existingUserAgent = existingMetaData.UserAgentMetaData
                    .FirstOrDefault(ua => ua.Browser == userAgentValue.Browser && ua.BrowserVersion == userAgentValue.BrowserVersion && ua.OperatingSystem == userAgentValue.OperatingSystem && ua.DeviceType==userAgentValue.DeviceType && ua.IP == GlobalHelper.IPAddress() );


                if (existingUserAgent != null)
                {

                }
                else
                {

                    existingMetaData.UserAgentMetaData.Add(userAgentValue);
                }
            }

            if (type == "Logout")
            {
                // Check if UserAgentMetaData list is null, if so, initialize it
                if (existingMetaData.UserAgentMetaData == null)
                {
                    existingMetaData.UserAgentMetaData = new List<UserAgentMetaData>();
                }

                // Parse the string value into UserAgentMetaData
                UserAgentMetaData userAgentValue = JsonConvert.DeserializeObject<UserAgentMetaData>(value);

                // Check if the new UserAgentMetaData already exists
                var existingUserAgent = existingMetaData.UserAgentMetaData
                    .FirstOrDefault(ua => ua.Browser == userAgentValue.Browser && ua.BrowserVersion == userAgentValue.BrowserVersion && ua.OperatingSystem == userAgentValue.OperatingSystem && ua.IP == GlobalHelper.IPAddress());

                if (existingUserAgent != null)
                {
                    // Remove the existing UserAgentMetaData
                    existingMetaData.UserAgentMetaData.Remove(existingUserAgent);
                }
            }
            if (type == "UserInActive")
            {
                // Check if UserAgentMetaData list is null, if so, initialize it
                if (existingMetaData.UserInactiveMetaData == null)
                {
                    existingMetaData.UserInactiveMetaData = new UserInActive();
                }

                // Parse the string value into UserAgentMetaData
                UserInActive userInactiveMetadata = JsonConvert.DeserializeObject<UserInActive>(value);

                // Check if the new UserAgentMetaData already exists

                existingMetaData.UserInactiveMetaData = userInactiveMetadata;
            }

            if (type == "VerifyAsBuyer")
            {
              
                // Check if the new UserAgentMetaData already exists

                existingMetaData.IsVerifiedBuyer = !existingMetaData.IsVerifiedBuyer;
                existingMetaData.IsVerifiedBuyerDate = DateTime.Now;
            }

            if (type == "VerifyAsSeller")
            {

                // Check if the new UserAgentMetaData already exists

                existingMetaData.IsVerifiedSeller = !existingMetaData.IsVerifiedSeller;
                existingMetaData.IsVerifiedSellerDate  = DateTime.Now;
            }

            try
            {
                up.OtherMetaData = JsonConvert.SerializeObject(existingMetaData);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                Console.WriteLine($"Error updating user metadata: {ex.Message}");
                // Optionally, you can rethrow the exception or handle it in a way that fits your application's error handling strategy
                throw;
            }

            #endregion



        }

        public string ValidateDevice(int userid, string value) //
        {
            string message = "";
            UserOtherMetaData existingMetaData = new UserOtherMetaData();
            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId == userid);
            if (up != null)
            {
                existingMetaData = up.OtherMetaData != null ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData) : null;
            }






            // Check if UserAgentMetaData list is null, if so, initialize it
            if (existingMetaData.UserAgentMetaData == null)
            {
                existingMetaData.UserAgentMetaData = new List<UserAgentMetaData>();
            }

            // Parse the string value into UserAgentMetaData
            UserAgentMetaData userAgentValue = JsonConvert.DeserializeObject<UserAgentMetaData>(value);

            // Check if the new UserAgentMetaData already exists
            var existingUserAgent = existingMetaData.UserAgentMetaData
                .FirstOrDefault(ua => ua.Browser == userAgentValue.Browser && ua.BrowserVersion == userAgentValue.BrowserVersion && ua.OperatingSystem == userAgentValue.OperatingSystem && ua.IP == GlobalHelper.IPAddress());

            if (existingUserAgent != null)
            {
                // Remove the existing UserAgentMetaData
                existingMetaData.UserAgentMetaData.Remove(existingUserAgent);
                message = "valid";
            }
            else
            {
                message = "invalid";
            }


            return message;






        }

        #endregion



        #region Registrationsetting
        public RegisterSettingsModel registrationsetting()
        {
            var _registerSettings = _websettinghelper.GetWebsettingJson("RegisterSettings");


            var registerJson = JsonConvert.DeserializeObject<RegisterSettingsModel>(_registerSettings);




            _accountcreationsetup = new RegisterSettingsModel
            {
                IsEmail = registerJson.IsEmail,
                IsPhone = registerJson.IsPhone,
                IsRegisterVerification = registerJson.IsRegisterVerification,

                RegisterImage = registerJson.RegisterImage,
                LoginImage = registerJson.LoginImage,

                IsGoogleLogin = registerJson.IsGoogleLogin,
                ClientId = registerJson.ClientId,
                ClientSecret = registerJson.ClientSecret,


                IsFacebookLogin = registerJson.IsFacebookLogin,
                AppId = registerJson.AppId,
                AppSecret = registerJson.AppSecret,


            };


            return registerJson;
        }

        #endregion



        #region AvailableWallet
        public decimal WalletAvailable(int profileid)
        {
            decimal availableWallet = 0;

            // Filter UserWallets based on the profileid
            var userWalletsForProfile = (from wallet in _dbContext.OrderMasters
                                         where wallet.BuyerId == profileid && wallet.ItemType == "wallet" &&
                                         wallet.OrderProcessStatus == "confirm"
                                         select new
                                         {
                                             UserWalletMetaData = ParseMetaDataWalletMetaData(wallet.SummaryMetaData)
                                            

                                         }
                                         ).ToList();

            // Calculate the sum of ConversionAmount for the filtered UserWallets
            availableWallet = userWalletsForProfile.Sum(wallet => wallet.UserWalletMetaData.GrandTotal);

            return availableWallet;
        }

        public string MyAvailableWallet(int profileId)
        {
            decimal availableWallet = 0;
            string currency = string.Empty;

            // Filter UserWallets based on the profileId
            var userWalletsForProfile = _dbContext.OrderMasters
                .Where(wallet => wallet.BuyerId == profileId && wallet.ItemType == "wallet" &&
                                wallet.OrderProcessStatus == "confirm")
                .Select(wallet => new
                {
                    UserWalletMetaData = ParseMetaDataWalletMetaData(wallet.SummaryMetaData),
                    Currency = ParseMetaDataWalletMetaData(wallet.SummaryMetaData).Currency
                })
                .ToList();

            // Calculate the sum of GrandTotal for the filtered UserWallets
            availableWallet = userWalletsForProfile.Sum(wallet => wallet.UserWalletMetaData.GrandTotal);

            // If there are UserWallets, set the currency to the currency of the first wallet
            if (userWalletsForProfile.Any())
            {
                currency = userWalletsForProfile.First().Currency;
            }

            // Create a JSON object with the currency and available wallet amount
            var result = new
            {
                Currency = currency,
                AvailableWallet = availableWallet
            };

            // Serialize the result to JSON
            string jsonResult = JsonConvert.SerializeObject(result);

            return jsonResult;
        }
        #endregion

        #region JsonCreator-WalletMetadata
        public string Walletmetadata(int orderid, string invoicenumber, DateTime walletdatetime, string actualcurrecny, decimal actualamount, decimal conversionamount, string conversioncurrency, string description)
        {

            #region CreateJsonForComments


            string walletmetdata = "";

            var metadata = new WalletItemMetaDataViewModel
            {
                OrderID = orderid,
                InvoiceNumber = invoicenumber,
                WalletDateTime = walletdatetime,
                ConversionAmount = conversionamount,
                ConversionCurrency = conversioncurrency,


                ActualAmount = actualamount,
                ActualCurrency = actualcurrecny,
                Description = description


            };

            // Serialize the object to JSON
            walletmetdata = JsonConvert.SerializeObject(metadata);
            #endregion


            return walletmetdata;
        }



        public static SummaryModel ParseMetaDataWalletMetaData(string json)
        {
            if (json == null)
            {
                // Return a new instance of SellerProfileSettingsModel if json is null
                return new SummaryModel();
            }

            var parsedData = JsonConvert.DeserializeObject<SummaryModel>(json);
            return parsedData;

        }

        #endregion
        public string WalletCreate(int profileid, string userewalletmetadata, string invoicenumber, string type, bool isapplied)
        {
            UserWallet insert = new UserWallet();


            insert.ProfileID = profileid;
            insert.UserWalletMetaData = userewalletmetadata;
            insert.InvoiceNumber = invoicenumber;
            insert.Type = type;   //wallet, used, topup
            insert.IsApplied = isapplied;
            insert.InsertDate = DateTime.Now;

            _dbContext.UserWallets.Add(insert);
            _dbContext.SaveChanges();

            return "";
        }




        #region UserAgent-BrowserInfo

        public  string GetUserAgentAsJson(string agentData)
        {
            // Set version truncation to none
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            // Create DeviceDetector instance
            var dd = new DeviceDetector(agentData);

            // Set caching method (optional)
            dd.SetCache(new DictionaryCache());

            // Parse user agent
            dd.Parse();

            if (dd.IsBot())
            {
                // Handle bots, spiders, crawlers
                var botInfo = dd.GetBot();
                return $"Bot detected: {botInfo}";
            }
            else
            {
                // Retrieve information for non-bot requests
                var clientInfo = dd.GetClient(); // Browser, feed reader, media player, etc.
                var osInfo = dd.GetOs();
                var device = dd.GetDeviceName();
                var brand = dd.GetBrandName();
                var model = dd.GetModel();

                // Create UserAgentMetaData object
                var userAgentInfo = new UserAgentMetaData
                {
                    Browser = $"{clientInfo.Match.Name} {clientInfo.Match.Version}",
                    BrowserVersion = clientInfo.Match.Version,
                    OperatingSystem = $"{osInfo.Match.Name} {osInfo.Match.Platform} {osInfo.Match.Version}"  ,
                    DeviceType = string.IsNullOrEmpty(device) ? "Unknown" : device,
                    Brand= string.IsNullOrEmpty(device) ? "" : brand,
                    Model= string.IsNullOrEmpty(device) ? "" : model,
                };

                // Convert UserAgentMetaData to JSON string
                string jsonUserAgent = JsonConvert.SerializeObject(userAgentInfo);

                return jsonUserAgent;
            }
        }
        //public string GetUserAgentAsJsonv1(string agentdata)
        //{
        //    var uaParser = Parser.GetDefault();
        //    var clientInfo = uaParser.Parse(agentdata);

        //    var useragent = new UserAgentMetaData
        //    {
        //        Browser = $"{clientInfo.UA.Family} {clientInfo.UA.Major}.{clientInfo.UA.Minor}",
        //        BrowserVersion = $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}",
        //        OperatingSystem = clientInfo.OS.Family,
        //        DeviceType = GetDeviceTypev1(agentdata)
        //    };

        //    // Convert UserAgentMetaData to JSON string
        //    string jsonUserAgent = JsonConvert.SerializeObject(useragent);

        //    return jsonUserAgent;
        //}

        //private string GetDeviceTypev1(string agentdata)
        //{
        //    string device_info = "Un Known";
        //    var browserinfo = new Browser()
        //    {
        //        userAgent = agentdata,
        //        OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline),
        //        device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline)
        //    };
        //    if (browserinfo.OS.IsMatch(browserinfo.userAgent))
        //    {
        //        device_info = browserinfo.OS.Match(browserinfo.userAgent).Groups[0].Value;
        //    }
        //    if (browserinfo.device.IsMatch(browserinfo.userAgent.Substring(0, 4)))
        //    {
        //        device_info += browserinfo.device.Match(browserinfo.userAgent).Groups[0].Value;
        //    }

        //    return device_info;
        //}
        #endregion



    }
}
