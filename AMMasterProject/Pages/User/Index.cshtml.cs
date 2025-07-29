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
    public class IndexModel : PageModel
    {

        #region DI
        public ClientViewModel BuyerView;

        public bool IsMultiVendor { get; set; }
        public string AvailableWallet { get; set; }
        private readonly UserHelper _userhelper;
        private readonly GlobalHelper _globalhelper;

        private readonly MembershipHelper _membershiphelper;

        private readonly MyDbContext _dbContext;

        public IndexModel(UserHelper userhelper, GlobalHelper globalHelper, MyDbContext dbContext, MembershipHelper membershiphelper)
        {


            _userhelper = userhelper;
            _globalhelper = globalHelper;
            _dbContext = dbContext;
            _membershiphelper = membershiphelper;
        }
        #endregion

        public void OnGet()
        {
           


            int loginid = 0;
            string dateformat = _globalhelper.Dateformat();
            if (User.Identity.IsAuthenticated)
            {

                loginid = int.Parse(User.FindFirst("UserID")?.Value);
                // continue with loginid variable
            }

            IsMultiVendor = bool.Parse(HttpContext.Items["IsMultiVendor"].ToString());

            BuyerView = (from up in _userhelper.ClientList()
                         where up.ProfileId == loginid
                         select new ClientViewModel
                         {
                             ProfileId = up.ProfileId,
                             ProfileGuid = up.ProfileGuid,
                             Displayname = up.Displayname ?? (up.FirstName + " " + up.LastName),
                             About = up.About != null ? up.About.ToString() : "not updated",
                             Contact = up.Contact != null ? up.Contact.ToString() : "not updated",
                             Email = up.Email != null ? up.Email.ToString() : "not updated",
                             FirstName = up.FirstName,
                             LastName = up.LastName,
                             Image = up.Image != null ? up.Image.ToString() : "/images/no-image.png",
                             InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString(dateformat),
                             Type = up.Type,
                             LoginName = up.LoginName,
                             LoginChannel = up.LoginChannel,
                             Address = up.Address != null ? up.Address.ToString() : "not updated",

                             ///order details
                             ///
                             PurchaseQtyTotal = up.PurchaseQtyTotal,
                             PurchaseAmountTotal = up.PurchaseAmountTotal,

                             // Wish list count
                             WishListTotal = _dbContext.CustomerWishlists.Count(w => w.UserId == up.ProfileId),

                             //Following sellers

                             FollowingSeller = _dbContext.VendorFollows.Count(w => w.ProfileId == up.ProfileId),

                             ///
                             CreditPurchaseModel = _membershiphelper.CreditPurchaseLifeTime(up.ProfileId),


                             //
                             SubscriptionPurchaseModel = _membershiphelper.SubscriptionPurchaseLifeTimeList(up.ProfileId),


                             //LevelName = up.LevelName,


                         }).FirstOrDefault();

            // Get available wallet information
            string availableWalletJson = _userhelper.MyAvailableWallet(loginid);

            // Deserialize the JSON string to a dynamic object
            dynamic walletInfo = JsonConvert.DeserializeObject(availableWalletJson);

            // Access the properties from the JObject
            string currency = walletInfo.Currency;
            decimal availableWalletAmount = walletInfo.AvailableWallet;

            AvailableWallet = $"{currency} {availableWalletAmount}";
            

        }
    }
}
