using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AMMasterProject.Components
{
    public class InboxViewComponent : ViewComponent
    {
        private readonly InboxHelper _inboxhelper;


        public InboxViewComponent(InboxHelper inboxHelper) 
        {

            _inboxhelper = inboxHelper;
           

        }
        public async Task<IViewComponentResult> InvokeAsync(string viewName, int profileid, string type) //creditcounter
        {
            object model = null;

            if (type == "contactlist")
            {
                //model = CreditEnabled(profileid);
            }

            else if (type == "activesubscription")
            {
                //model = SubscriptionEnabled(profileid);
            }

            else if (type == "creditpurchasehistory")
            {
                //model = _membershipohelper.CreditPurchaseList(profileid);
            }

            
            // Add other conditions for different types if needed

            return View(viewName, model); // Return a view to display the data
        }


    }
}
