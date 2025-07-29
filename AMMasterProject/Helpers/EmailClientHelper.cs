using System.Net.Mail;
using System.Net;
using AMMasterProject.ViewModel;
using Microsoft.Extensions.Configuration;

namespace AMMasterProject.Helpers
{
    public class EmailClientHelper
    {

        #region DI


        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        private readonly MyDbContext _dbContext;
        private readonly NotificationHelper _notificationHelper ;


        public EmailClientHelper(MyDbContext context, NotificationHelper notificationHelper)
        {
            _dbContext = context;
            _notificationHelper = notificationHelper;

        }
        #endregion


        #region EmailSendingCode

       
       

        #endregion


      



    }
}
