using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AMMasterProject.Components
{
    
    public class NotificationViewComponent : ViewComponent
    {

        private readonly NotificationHelper _notificationHelper;
        private readonly InboxHelper _inboxHelper;

        public NotificationViewComponent(NotificationHelper notificationHelper, InboxHelper inboxHelper)
        {
                _notificationHelper = notificationHelper;
                 _inboxHelper = inboxHelper;


        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName, string methodname, int profielid) //announcementactive, announcementall
        {
            object model = null;

            if (methodname == "announcementactive")
            {
                model = _notificationHelper.announcementactive(profielid);
            }

            else if (methodname == "announcementall")
            {
                model = _notificationHelper.announcementall(profielid);
            }

            else if (methodname == "announcementactivecounter")
            {
                model = _notificationHelper.UnReadCounterannouncement(profielid);
            }

            else if (methodname == "inboxactivecounter")
            {

                model = await _inboxHelper.InboxUnreadCount(profielid);
                       
            }

         

            else if (methodname == "cookie")
            {
               
            }
            // Add other conditions for different types if needed

            return View(viewName, model); // Return a view to display the data
        }


    }


}
