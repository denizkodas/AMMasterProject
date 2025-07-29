using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject.Pages.Admin.usermanagement
{

    [Authorize(Policy = "Admin")]
    [BindProperties]
    public class userboardModel : PageModel
    {
        #region DI
        public ClientViewModel BuyerView;
        public SellerViewModel SellerView;
        public UserLockMetaData userlock { get; set; }

        public decimal S_ShippedCount { get; set; }
        public decimal S_ProcessingCount { get; set; }
        public decimal S_DeliveredCount { get; set; }

        public decimal S_CompletedCount { get; set; }

        public decimal S_CancelledCount { get; set; }
        public decimal S_ReturnedCount { get; set; }


        public decimal B_ShippedCount { get; set; }
        public decimal B_ProcessingCount { get; set; }
        public decimal B_DeliveredCount { get; set; }

        public decimal B_CompletedCount { get; set; }

        public decimal B_CancelledCount { get; set; }
        public decimal B_ReturnedCount { get; set; }

        private readonly UserHelper _userhelper;
        private readonly MembershipHelper _membershiphelper;
        private readonly GlobalHelper _globalhelper;
        private readonly MyDbContext _dbContext;
        private readonly OrderHelper _orderHelper;

        public userboardModel(UserHelper userhelper, GlobalHelper globalHelper, MyDbContext dbContext, MembershipHelper membershiphelper, OrderHelper orderhelper)
        {

            _userhelper = userhelper;
            _globalhelper = globalHelper;
            _dbContext = dbContext;
            _membershiphelper = membershiphelper;
            _orderHelper = orderhelper;

            userlock = new UserLockMetaData();
            userlock.IsLock = true;
            userlock.UnlockDate = DateTime.Now.AddDays(5);
        }
        #endregion

      
        public void OnGet()
        {
            string routeid = (string)RouteData.Values["id"];

            int userid = int.Parse(routeid.ToString());

            string dateformat = _globalhelper.Dateformat();

            BuyerView = (from up in _userhelper.ClientList()
                         where up.ProfileId ==userid
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
                                CreditPurchaseModel= _membershiphelper.CreditPurchaseLifeTime(up.ProfileId),


                                //
                                SubscriptionPurchaseModel = _membershiphelper.SubscriptionPurchaseLifeTimeList(up.ProfileId),


                                //LevelName = up.LevelName,
                                userothermetadata = up.userothermetadata,

                            }).FirstOrDefault();




            SellerView = (from up in _userhelper.SellerList()
                          where up.ProfileId == userid
                          select new SellerViewModel
                          {
                                ProfileId = up.ProfileId,
                                ProfileGuid = up.ProfileGuid,
                                Displayname = up.Displayname ?? (up.FirstName + " " + up.LastName),
                                About = up.About,
                                Contact = up.Contact != null ? up.Contact.ToString() : "not updated",
                                Email = up.Email != null ? up.Email.ToString() : "not updated",
                                FirstName = up.FirstName,
                                LastName = up.LastName,
                                Image = up.Image  != null ? up.Image.ToString() : up.SellerCoverImage != null ? up.SellerCoverImage.ToString() : "/images/no-image.png",
                               SellerCoverImage = up.SellerCoverImage != null ? up.SellerCoverImage.ToString() : "/images/no-image.png",
                              InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString(dateformat),
                                Type = up.Type,
                                BusinessName = up.BusinessName,
                                BusinessDescription=up.BusinessDescription,
                                LoginName = up.LoginName,
                              LoginChannel = up.LoginChannel,
                              Address   = up.Address != null ? up.Address.ToString() : "not updated",

                              //LevelName = up.LevelName,

                              userothermetadata=up.userothermetadata,
                              //Following sellers

                              //Followers = _dbContext.VendorFollows.Count(w => w.VendorId == up.ProfileId),


                              ProductTotal=up.ProductTotal,

                              ///
                              CreditPurchaseModel = _membershiphelper.CreditPurchaseLifeTime(up.ProfileId),




                              CustomerTotal = _orderHelper.GetOrdersItem().Where(u => u.SellerID == up.ProfileId).GroupBy(u => u.BuyerID).ToList().Count(),


                              SalesActualCurrency = _orderHelper.GetOrdersItem()
    .Where(u => u.SellerID == up.ProfileId && u.OrderProcessStatus == "completed")
    .Select(u => u.ItemDetailMetaData.paymentModel.ActualCurrency)
    .FirstOrDefault(),
                              SalesQtyTotal = _orderHelper.GetOrdersItem().Where(u => u.SellerID == up.ProfileId && u.OrderProcessStatus == "completed").ToList().Count(),


                              SalesAmountTotal = _orderHelper.GetOrdersItem()
    .Where(u => u.SellerID == up.ProfileId && u.OrderProcessStatus == "completed")
    .Select(u => u.ItemDetailMetaData.paymentModel.ActualAmount)
    .DefaultIfEmpty(0) // Handle the case where there are no completed orders
    .Sum()

                          }).FirstOrDefault();




            ///order status count
            ///
            S_ShippedCount = _orderHelper.orderstatuscount("shipped", userid, "seller");
            S_ProcessingCount = _orderHelper.orderstatuscount("processing", userid, "seller");
            S_DeliveredCount = _orderHelper.orderstatuscount("delivered", userid, "seller");
            S_CompletedCount = _orderHelper.orderstatuscount("completed", userid, "seller");
            S_CancelledCount = _orderHelper.orderstatuscount("cancelled", userid, "seller");
            S_ReturnedCount = _orderHelper.orderstatuscount("returned", userid, "seller");



            B_ShippedCount = _orderHelper.orderstatuscount("shipped", userid, "buyer");
            B_ProcessingCount = _orderHelper.orderstatuscount("processing", userid, "buyer");
            B_DeliveredCount = _orderHelper.orderstatuscount("delivered", userid, "buyer");
            B_CompletedCount = _orderHelper.orderstatuscount("completed", userid, "buyer");
            B_CancelledCount = _orderHelper.orderstatuscount("cancelled", userid, "buyer");
            B_ReturnedCount = _orderHelper.orderstatuscount("returned", userid, "buyer");

        }



        #region AccountDelete
        public IActionResult OnPostAccountLock()
        {
            string routeid = (string)RouteData.Values["id"];

            int userid = int.Parse(routeid.ToString());
            _userhelper.AccountLocked(userid,userlock.Remarks, userlock.IsLock, userlock.UnlockDate);

            TempData["success"] = "User status updated.";

            OnGet();
            return Page();
        }
        #endregion


        #region VerifyAsBuyer
        public IActionResult OnPostVerifyAsBuyer()
        {
            string routeid = (string)RouteData.Values["id"];

            int userid = int.Parse(routeid.ToString());
            UsersProfile usernameExists = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId ==userid);

            if (usernameExists != null)
            {
                ///create user agent
              
                _userhelper.UserOtherMetaDataUpdate(usernameExists.ProfileId, "", "VerifyAsBuyer");

                TempData["success"] = "User Status Updated.";
            }
            OnGet();
            return Page();
        }
        #endregion

        #region VerifyAsSeller
        public IActionResult OnPostVerifyAsSeller()
        {
            string routeid = (string)RouteData.Values["id"];

            int userid = int.Parse(routeid.ToString());
            UsersProfile usernameExists = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileId == userid);


            if (usernameExists != null)
            {
                ///create user agent

                _userhelper.UserOtherMetaDataUpdate(usernameExists.ProfileId, "", "VerifyAsSeller");

                TempData["success"] = "User Status Updated.";
            }
            OnGet();
            return Page();
        }
        #endregion
    }
}
