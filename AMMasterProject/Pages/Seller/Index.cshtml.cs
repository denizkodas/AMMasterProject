using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Seller
{

    [Authorize(Policy = "Seller")]
    public class IndexModel : PageModel
    {
        #region DI
      
        public ProfileCompletionMetaData profileCompletionMetaData { get; set; }

        public SellerViewModel sellerView;
        public SellerProfileModel UserProfile { get; set; }

        public decimal TotalProfileTabByAdmin { get; set; }
        public decimal TotalProfileTabByUser { get; set; }

        public decimal ProfileTabPercentage { get; set; }
        public decimal TotalOrderReceived { get; set; }

        public decimal ShippedCount { get; set; }
        public decimal ProcessingCount { get; set; }
        public decimal DeliveredCount { get; set; }

        public decimal CompletedCount { get; set; }

        public decimal CancelledCount { get; set; }
        public decimal ReturnedCount { get; set; }



        public decimal CompletedAmount { get; set; }
        public decimal ProcessingAmount { get; set; }
        public decimal ShippedAmount { get; set; }
        
        public decimal DeliveredAmount{ get; set; }

      

        public decimal CancelledAmount { get; set; }
        public decimal ReturnedAmount { get; set; }


        private readonly UserHelper _userhelper;
        private readonly GlobalHelper _globalhelper;

        private readonly MembershipHelper _membershiphelper;

        private readonly OrderHelper _orderHelper;

        private readonly MyDbContext _dbContext;

        public IndexModel(UserHelper userhelper, GlobalHelper globalHelper, MyDbContext dbContext, MembershipHelper membershiphelper, OrderHelper orderhelper)
        {


            _userhelper = userhelper;
            _globalhelper = globalHelper;
            _dbContext = dbContext;
            _membershiphelper = membershiphelper;
            _orderHelper = orderhelper;
        }
        #endregion
        public void OnGet()
        {
            int loginid = 0;
            Guid ProfileGUID = Guid.NewGuid();
            
            string dateformat = _globalhelper.Dateformat();
            if (User.Identity.IsAuthenticated)
            {

                loginid = int.Parse(User.FindFirst("UserID")?.Value);
                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value);
                // continue with loginid variable
            }
            sellerView = (from up in _userhelper.SellerList()
                          where up.Type == "Vendor" && up.ProfileId == loginid
                          select new SellerViewModel
                          {
                              ProfileId = up.ProfileId,
                              ProfileGuid = up.ProfileGuid,
                              Displayname = up.Displayname ?? (up.FirstName + " " + up.LastName),
                              About = up.About,
                              Contact = up.Contact?.ToString(),
                              Email = up.Email,
                              FirstName = up.FirstName,
                              LastName = up.LastName,
                              Image = up.Image != null ? up.Image.ToString() : up.SellerCoverImage != null ? up.SellerCoverImage.ToString() : "/images/no-image.png",
                              InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString(dateformat),
                              Type = up.Type,
                              BusinessName = up.BusinessName,
                              LoginName = up.LoginName,
                              //LevelName = up.LevelName,
                              ProductTotal = up.ProductTotal,
                              //Followers = up.Followers,
                              //CustomerTotal= up.CustomerTotal,
                              //SalesQtyTotal=up.SalesQtyTotal,
                              //SalesAmountTotal=up.SalesAmountTotal

                              BusinessUrlpath=up.BusinessUrlpath,
                              CustomerTotal = _orderHelper.GetOrdersItem().Where(u => u.SellerID == up.ProfileId && u.OrderStatus =="confirm").GroupBy(u => u.BuyerID).ToList().Count(),


                              SalesActualCurrency = _orderHelper.GetOrdersItem()
    .Where(u => u.SellerID == up.ProfileId && u.OrderProcessStatus == "completed")
    .Select(u => u.ItemDetailMetaData.paymentModel.ActualCurrency)
    .FirstOrDefault(),
                              SalesQtyTotal = _orderHelper.GetOrdersItem().Where(u => u.SellerID == up.ProfileId && u.OrderProcessStatus == "completed").ToList().Count(),


                              SalesAmountTotal = _orderHelper.GetOrdersItem()
    .Where(u => u.SellerID == up.ProfileId && u.OrderProcessStatus == "completed")
    .Select(u => u.ItemDetailMetaData.paymentModel.ActualAmount)
    .DefaultIfEmpty(0) // Handle the case where there are no completed orders
    .Sum(),



               userothermetadata= up.userothermetadata 
                          }).FirstOrDefault();



            ///order status count
            ///
            ProcessingCount = _orderHelper.orderstatuscount("processing", loginid, "seller");
            ShippedCount = _orderHelper.orderstatuscount("shipped", loginid, "seller");
            
            DeliveredCount = _orderHelper.orderstatuscount("delivered", loginid, "seller");
            CompletedCount = _orderHelper.orderstatuscount("completed", loginid, "seller");
            CancelledCount = _orderHelper.orderstatuscount("cancelled", loginid, "seller");
            ReturnedCount = _orderHelper.orderstatuscount("returned", loginid, "seller");


            TotalOrderReceived = ProcessingCount + ShippedCount + DeliveredCount + CompletedCount + CancelledCount + ReturnedCount;

            //order by order type amount

            ProcessingAmount = _orderHelper.orderAmountcount("processing", loginid, "seller");
            ShippedAmount = _orderHelper.orderAmountcount("shipped", loginid, "seller");

            DeliveredAmount = _orderHelper.orderAmountcount("delivered", loginid, "seller");
            CompletedAmount = _orderHelper.orderAmountcount("completed", loginid, "seller");
            CancelledAmount = _orderHelper.orderAmountcount("cancelled", loginid, "seller");
            ReturnedAmount = _orderHelper.orderAmountcount("returned", loginid, "seller");



            ///Profile Completion rate
            ///
            UsersProfile ups = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

            if (ups != null)
            {

                profileCompletionMetaData = _userhelper.profilecompletestatus(ups.ProfileVerificationMetaData);

                TotalProfileTabByAdmin = profileCompletionMetaData.adminsetting.TotalTabsRequired;
                TotalProfileTabByUser = profileCompletionMetaData.sellersetting.TotalTabsRequired;

                if (TotalProfileTabByUser != 0)
                {
                    ProfileTabPercentage = (TotalProfileTabByUser / TotalProfileTabByAdmin) * 100;

                    ProfileTabPercentage = Math.Round(ProfileTabPercentage, 0);
                }
                else
                {
                    ProfileTabPercentage = 0;
                }

            }
        }
    }
}
