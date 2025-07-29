using AMMasterProject.Models;
using AMMasterProject.ViewModel;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Amazon;
using PayPal.Api;

namespace AMMasterProject.Helpers
{
    public class NotificationHelper
    {
        #region DI

        private readonly WebsettingHelper _websettinghelper;
        private readonly MyDbContext _dbContext;



        public NotificationHelper(MyDbContext context, WebsettingHelper websettinghelper)
        {
            _dbContext = context;
            _websettinghelper = websettinghelper;

        }
        #endregion

        #region CallThisMethod


        public int notification(string notificationtype, int profileid, string var1, string var2, string var3, string var4, string receiver, string redirecturl)  ///type means register, forgot password or other
        {
            //0 email  1 =sms
            int notificationid = 0;
            int notificationchannel = 0;//based on receiver determine email or phone
            string logintype = GlobalHelper.GetEmailOrPhone(receiver); //email or phone

            if (logintype=="Phone")
            {
                notificationchannel = 1;
                notificationid = 0;
            }


           PageName nc = _dbContext.PageNames.FirstOrDefault(u => u.CMSKey == notificationtype && u.PageType == "email");
            if (nc != null)
            {

            
                string notificationbody = nc.PageDescription.Replace("{var1}", var1).Replace("{var3}", var2).Replace("{var3}", var3).Replace("{var4}", var4);

                notificationid= notificationrelay(nc.Name, notificationbody, profileid, receiver, redirecturl, notificationchannel);
                    //Pendingemails();
                
            }

            return notificationid;

        }
        #endregion


        #region NotificationSetting




        //this method only take insert and hold the notificaiton either for email or sms
        //then auto scheduler will take the data from this issent==false and keep sending the notifications
        public int notificationrelay(string subject, string notificationbody, int profileid, string receiver, string redirecturl, int notificationchannel)
        {
            int NotificationId = 0;

            NotificationRelay nr = new NotificationRelay();

            nr.InsertDate = DateTime.Now;
            nr.NotificationRelaySubject = subject;
            nr.NotificationRelayBody = notificationbody;
            nr.Issent = false;
            nr.IsRead = false;
            nr.ProfileId = profileid;
            nr.Receiver = receiver;
            nr.RedirectUrl = redirecturl;
            nr.NotificationChannel = notificationchannel;  //0 email  1 =sms

            _dbContext.NotificationRelays.Add(nr);
            _dbContext.SaveChanges();



            if(nr!=null)
            {
                NotificationId = nr.NotificationRelayId;
            }


            return NotificationId;
            ///send email quickly
            ///
            //if (nr != null)
            //{
            //    emailrelay(nr.NotificationRelaySubject, nr.NotificationRelayBody, nr.Receiver, nr.NotificationRelayId);
            //}
        }
        public void notificationrelayIsSent(int notificationrelayid)
        {
            NotificationRelay nr = _dbContext.NotificationRelays.FirstOrDefault(u => u.NotificationRelayId == notificationrelayid);


            if (nr != null)
            {
                nr.Issent = true;
                nr.DeliveryDate = DateTime.Now;
            }

            _dbContext.SaveChanges();
        }


        public void notificationrelayIsRead(int notificationrelayid)
        {
            NotificationRelay nr = _dbContext.NotificationRelays.FirstOrDefault(u => u.NotificationRelayId == notificationrelayid);


            if (nr != null)
            {
                nr.IsRead = true;

            }

            _dbContext.SaveChanges();
        }



        #endregion


        #region SendNotification
        public  void NotificationSet(int ProfileID, string title, string description, string imageurl, string redirecturl)
        {
            AnnouncementNotification announcement = new AnnouncementNotification();
            announcement.Title = title.Trim();
            announcement.Description = description.Trim();
            announcement.ProfileID = ProfileID;  //to whome announcemnet will receive
            announcement.ImageURL = imageurl.Trim();
            announcement.RedirectURL = redirecturl.Trim();
            announcement.AnnouncementFor = "Auto";
            announcement.StartDate = DateTime.Now;
            announcement.ExpiryDate = DateTime.Now.AddDays(7);
            announcement.InsertDate = DateTime.Now;
            announcement.IsPublish =true;


            _dbContext.AnnouncementNotification.Add(announcement);
            _dbContext.SaveChanges();
        }
        #endregion


        #region PendingNotification

        public void Pendingemails()
        {
            var q = (from nr in _dbContext.NotificationRelays
                     where nr.Issent == false && nr.NotificationChannel == 0
                     select nr).ToList();

            foreach (var item in q)
            {
                emailrelay(item.NotificationRelaySubject, item.NotificationRelayBody, item.Receiver, item.NotificationRelayId);
            }
        }

        #endregion


        #region EmailClientCode
        public string emailrelay(string subject, string body, string receiver, int notificationrelayid)
        {
            ////get setting of email
            string message = "";
            try
            {

                ///get email credentials
                ///

                var emailSettings = _websettinghelper.GetWebsettingJson("EmailSettings");

                if (emailSettings != null && !string.IsNullOrEmpty(emailSettings))
                {

                    var json = JsonConvert.DeserializeObject<EmailSettingsModel>(emailSettings);

                    if (json != null)
                    {

                        string emailfrom = json.FromEmail;
                        string password = json.Password;
                        string bcc = json.BCC;

                        int port = json.Port;
                        string smtphost = json.SMTP;
                        bool enablessl = json.EnableSSL;



                        ///email variable
                        ///





                        using (MailMessage mm = new MailMessage(emailfrom, receiver))
                        {

                            mm.Subject = subject;


                            mm.Bcc.Add(bcc);





                            mm.IsBodyHtml = true;
                            mm.Body = body;




                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = smtphost;
                            smtp.EnableSsl = enablessl;
                            NetworkCredential NetworkCred = new NetworkCredential(emailfrom, password);
                            //smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = port;
                            smtp.Send(mm);
                            smtp.Dispose();

                            message = "Success";

                            //once email sent update the db


                            notificationrelayIsSent(notificationrelayid);


                        }
                    }
                }

            }
            catch (Exception ex)
            {

                message = "Error " + ex.Message + " Inner Exception " + ex.InnerException;
            }

            return message;

        }
        #endregion


        #region Announcement


        public AnnouncementCounterViewModel UnReadCounterannouncement(int profileId)
        {
            var announcementList = (from announcement in _dbContext.AnnouncementNotification
                                    where announcement.StartDate <= DateTime.Now && announcement.ExpiryDate >= DateTime.Now
                                    && announcement.ProfileID == profileId || announcement.ProfileID ==null
                                    orderby announcement.AnnouncementId
                                    select new AnnouncementCounterViewModel
                                    {
                                        AnnouncementCounter = announcement.AnnouncementId,
                                        IsUserRead = false // Assume initially the user has not read the announcement
                                    }).ToList();

            // Get the user's profile
            var userProfile = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId == profileId); // Adjust this to fetch the user's profile based on your application logic

            // Check if the user's profile exists and has announcement metadata
            if (userProfile != null && !string.IsNullOrEmpty(userProfile.AnnouncementMetaData))
            {
                // Parse the announcement metadata as a JArray
                var announcementMetadataArray = JArray.Parse(userProfile.AnnouncementMetaData);

                // Iterate through each announcement in the list
                foreach (var announcement in announcementList)
                {
                    // Check if the announcement ID exists in the metadata
                    var exists = announcementMetadataArray.Any(a => (int)a["AnnouncementID"] == announcement.AnnouncementCounter);

                    // Set the flag indicating if the user has read the announcement or not
                    announcement.IsUserRead = exists;
                }

                // Filter the list to show only announcements where IsUserRead is false
                announcementList = announcementList.Where(a => !a.IsUserRead).ToList();
            }

            var viewModel = new AnnouncementCounterViewModel
            {
                AnnouncementCounter = announcementList.Count,
                IsUserRead = announcementList.Count > 0
            };

            return viewModel;
        }
        public List<AnnouncementViewModel> announcementactive(int profielid)
        {
            var announcementList = (from announcement in _dbContext.AnnouncementNotification
                                    where announcement.StartDate <= DateTime.Now && announcement.ExpiryDate >= DateTime.Now
                                   && announcement.ProfileID == profielid || announcement.ProfileID == null
                                    orderby announcement.AnnouncementId
                                    select new AnnouncementViewModel
                                    {
                                        AnnouncementId = announcement.AnnouncementId,
                                        Title = announcement.Title,
                                        Description = announcement.Description,
                                        StartDate = announcement.StartDate,
                                        ExpiryDate = announcement.ExpiryDate,
                                        IsUserRead = false, // Assume initially the user has not read the announcement
                                        RedirectUrl =announcement.RedirectURL
                                    }).ToList();

            // Get the user's profile
            var userProfile = _dbContext.UsersProfiles.Where(u => u.ProfileId == profielid).FirstOrDefault(); // Adjust this to fetch the user's profile based on your application logic

            // Check if the user's profile exists and has announcement metadata
            if (userProfile != null && !string.IsNullOrEmpty(userProfile.AnnouncementMetaData))
            {
                // Parse the announcement metadata as a JArray
                var announcementMetadataArray = JArray.Parse(userProfile.AnnouncementMetaData);

                // Iterate through each announcement in the list
                foreach (var announcement in announcementList)
                {
                    // Check if the announcement ID exists in the metadata
                    var exists = announcementMetadataArray.Any(a => (int)a["AnnouncementID"] == announcement.AnnouncementId);

                    // Set the flag indicating if the user has read the announcement or not
                    announcement.IsUserRead = exists;
                }

                // Filter the list to show only announcements where IsUserRead is false
                announcementList = announcementList.Where(a => !a.IsUserRead).ToList();
            }

            return announcementList;
        }

        public List<AnnouncementViewModel> announcementall(int profile)
        {
            var announcementList = (from announcement in _dbContext.AnnouncementNotification
                                    where announcement.ProfileID == profile || announcement.ProfileID == null
                                    orderby announcement.AnnouncementId
                                    select new AnnouncementViewModel
                                    {
                                        AnnouncementId = announcement.AnnouncementId,
                                        Title = announcement.Title,
                                        Description = announcement.Description,
                                        StartDate = announcement.StartDate,
                                        ExpiryDate = announcement.ExpiryDate,
                                        IsUserRead = false, // Assume initially the user has not read the announcement
                                        RedirectUrl = announcement.RedirectURL
                                    }).ToList();

            // Get the user's profile
            var userProfile = _dbContext.UsersProfiles.Where(u => u.ProfileId == profile).FirstOrDefault(); // Adjust this to fetch the user's profile based on your application logic

            // Check if the user's profile exists and has announcement metadata
            if (userProfile != null && !string.IsNullOrEmpty(userProfile.AnnouncementMetaData))
            {
                // Parse the announcement metadata as a JArray
                var announcementMetadataArray = JArray.Parse(userProfile.AnnouncementMetaData);

                // Iterate through each announcement in the list
                foreach (var announcement in announcementList)
                {
                    // Check if the announcement ID exists in the metadata
                    var exists = announcementMetadataArray.Any(a => (int)a["AnnouncementID"] == announcement.AnnouncementId);

                    // Set the flag indicating if the user has read the announcement or not
                    announcement.IsUserRead = exists;
                }
            }

            return announcementList;
        }


        public void AnnouncementMetaDataPost(int userId, int announcementId)
        {
            // Retrieve the user's profile from the database
            var userProfile = _dbContext.UsersProfiles.Find(userId);

            // Create a new JSON array or list to store the announcement metadata if it's null or empty
            var announcementMetadataArray = string.IsNullOrEmpty(userProfile.AnnouncementMetaData)
                ? new JArray()
                : JArray.Parse(userProfile.AnnouncementMetaData);

            // Create a new JSON object for the current announcement
            var announcementObject = new JObject
        {
            { "AnnouncementID", announcementId },
            { "Date", DateTime.Now }
        };

            // Add the announcement object to the array
            announcementMetadataArray.Add(announcementObject);

            // Update the announcement metadata property in the user's profile
            userProfile.AnnouncementMetaData = announcementMetadataArray.ToString(Formatting.None);

            // Save the changes to the database
            _dbContext.SaveChanges();
        }


        


        #endregion



    }
}
