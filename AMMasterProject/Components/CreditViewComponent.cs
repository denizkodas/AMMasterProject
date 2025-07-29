
using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;




namespace AMMasterProject.Components
{
    public class CreditViewComponent : ViewComponent
    {
        //private readonly MyDbContext _dbcontext;
        private readonly WebsettingHelper _websettinghelper;
        private readonly MembershipHelper _membershipohelper;
        private readonly OrderHelper _orderhelper;

        public CreditViewComponent(MyDbContext context, WebsettingHelper websettinghelper, MembershipHelper membershipHelper, OrderHelper orderHelper)
        {
         
            _websettinghelper = websettinghelper;
            _membershipohelper = membershipHelper;
            _orderhelper = orderHelper;

        }

        #region CreditCounterViewModelCheckIfActivateCreditFunctionality
        public CreditCounterViewModel CreditEnabled(int profileid)
        {
            CreditCounterViewModel model = new CreditCounterViewModel();

            var _creditsystemSettings = _websettinghelper.GetWebsettingJson("CreditSystemSettings");

            if (_creditsystemSettings != null && !string.IsNullOrEmpty(_creditsystemSettings))
            {
                var json = JsonConvert.DeserializeObject<CreditCounterViewModel>(_creditsystemSettings);

                if (json != null)
                {
                    model = new CreditCounterViewModel
                    {
                        IsEnable = json.IsEnable,
                        CreditAvailable = _membershipohelper.CreditAvailable(profileid)
                    };
                }
            }

            return model;
        }
        #endregion

        #region SubscriptionViewModelCheckIfActivateSubscriptionFunctionality
        public SubscriptionActiveViewModel SubscriptionEnabled(int profileid)
        {
            SubscriptionActiveViewModel model = new SubscriptionActiveViewModel();

            var _subscriptionsystemSettings = _websettinghelper.GetWebsettingJson("SubscriptionSystemSettings");

            if (_subscriptionsystemSettings != null && !string.IsNullOrEmpty(_subscriptionsystemSettings))
            {
                var json = JsonConvert.DeserializeObject<SubscriptionActiveViewModel>(_subscriptionsystemSettings);

                if (json != null)
                {
                    model = new SubscriptionActiveViewModel
                    {
                        IsEnable = json.IsEnable,
                        subscriptionviewmodel = _membershipohelper.SubscriptionActive(profileid)
                    };
                }
            }

            return model;
        }
        #endregion










        public async Task<IViewComponentResult> InvokeAsync(string viewName, int profileid, string type) //creditcounter
        {
            object model = null;

            if (type == "creditcounter")
            {
                model = CreditEnabled(profileid);
            }

            else if(type == "activesubscription")
            {
                model = SubscriptionEnabled(profileid);
            }

            else if(type=="creditpurchasehistory")
            {
                model = _membershipohelper.CreditPurchaseList(profileid).OrderByDescending(u => u.PurchaseDate).ToList();
            }

            else if (type == "subscriptionpurchasehistory")
            {
                model = _membershipohelper.SubscriptionPurchaseList(profileid).OrderByDescending(u => u.PurchaseDate).ToList();
            }

            else if(type== "incomingoutgoingtransactionhistorybuyer")
            {
                //model = _membershipohelper.PurchaseHistoryList(profileid).OrderByDescending(u=>u.PurchaseDate).ToList
                //
                model = _orderhelper.GetOrdersItem("transactionhistorybuyer", profileid).OrderByDescending(u=>u.OrderId).ToList();
            }

            else if (type == "incomingoutgoingtransactionhistoryseller")
            {
                //model = _membershipohelper.PurchaseHistoryList(profileid).OrderByDescending(u=>u.PurchaseDate).ToList
                //
                model = _orderhelper.GetOrdersItem("transactionhistoryseller", profileid).OrderByDescending(u => u.OrderId).ToList();
            }
            // Add other conditions for different types if needed

            return View(viewName, model); // Return a view to display the data
        }



    }
}
