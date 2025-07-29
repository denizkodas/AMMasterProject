using AMMasterProject.Helpers;
using AMMasterProject.Migrations;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AMMasterProject.Pages.Admin
{

    [Authorize(Policy = "Admin")]


    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model

        #region UserVariable


        public int TotalUsers { get; set; } //all user client or vendor
        public int TotalUsersToday { get; set; }
        public int TotalUsersWeek { get; set; }
        public int TotalUsersMonth { get; set; }
        public int TotalUsersYear { get; set; }



        public int TotalCustomers { get; set; }

        public int TotalCustomersToday { get; set; }
        public int TotalCustomersWeek { get; set; }
        public int TotalCustomersMonth { get; set; }
        public int TotalCustomersYear { get; set; }

        public int TotalCustomersPurchaseProducts { get; set; } //purchase atleast one item

        public int TotalSellers { get; set; } //added atleast one item

        public int TotalSellersToday { get; set; }
        public int TotalSellersWeek { get; set; }
        public int TotalSellersMonth { get; set; }
        public int TotalSellersYear { get; set; }

        public int TotalSellersHaveProducts { get; set; } //added atleast one item

        #endregion


        #region OrderVariable

        public string BaseCurrency { get; set; }
        public decimal TotalRevenue { get; set; } //total revenue



        #region CreditRevenue
        public decimal TotalRevenueCredit { get; set; }

        public decimal TotalRevenueCreditToday { get; set; }
        public decimal TotalRevenueCreditWeek { get; set; }
        public decimal TotalRevenueCreditMonth { get; set; }
        public decimal TotalRevenueCreditYear { get; set; }

        #endregion


        #region SubscriptionRevenue
        public decimal TotalRevenueSubscription { get; set; }

        public decimal TotalRevenueSubscriptionToday { get; set; }
        public decimal TotalRevenueSubscriptionWeek { get; set; }
        public decimal TotalRevenueSubscriptionMonth { get; set; }
        public decimal TotalRevenueSubscriptionYear { get; set; }
        #endregion


        #region RevenueCommission
        public decimal TotalRevenueCommission { get; set; }

        public decimal TotalRevenueCommissionToday { get; set; }
        public decimal TotalRevenueCommissionWeek { get; set; }
        public decimal TotalRevenueCommissionMonth { get; set; }
        public decimal TotalRevenueCommissionYear { get; set; }
        #endregion



        #region RevenueProfileBoost
        public decimal TotalRevenueProfileBoost { get; set; }

        public decimal TotalRevenueProfileBoostToday { get; set; }
        public decimal TotalRevenueProfileBoostWeek { get; set; }
        public decimal TotalRevenueProfileBoostMonth { get; set; }
        public decimal TotalRevenueProfileBoostYear { get; set; }
        #endregion

        #region RevenueitemBoost
        public decimal TotalRevenueItemBoost { get; set; }

        public decimal TotalRevenueItemBoostToday { get; set; }
        public decimal TotalRevenueItemBoostWeek { get; set; }
        public decimal TotalRevenueItemBoostMonth { get; set; }
        public decimal TotalRevenueItemBoostYear { get; set; }
        #endregion





        public decimal TotalRevenueItem { get; set; }
        #endregion



        #region ListingVariation


        public int TotalListing { get; set; }


        #endregion


        #region ListingTotal

        public int GrandTotal { get; set; }
        public int TotalPhysical { get; set; }
        public int TotalDigital { get; set; }
        public int TotalService { get; set; }
        public int TotalCourses { get; set; }



        public int TotalSell { get; set; }
        public int TotalClassified { get; set; }
        public int TotalAuction { get; set; }
        public int TotalPennyAuction { get; set; }




        public int PhysicalSell { get; set; }
        public int DigitalSell { get; set; }
        public int ServiceSell { get; set; }
        public int CoursesSell { get; set; }



        public int PhysicalClassified { get; set; }
        public int DigitalClassified { get; set; }
        public int ServiceClassified { get; set; }
        public int CoursesClassified { get; set; }



        public int AuctionPhysical { get; set; }
        public int AuctionDigital { get; set; }


        public int PennyAuctionPhysical { get; set; }
        public int PennyAuctionDigital { get; set; }

        #endregion

        #endregion


        #region DI


        private readonly WebsettingHelper _websettinghelper;

        public LicenseAppSettingsModel licensesettings { get; set; }

        private readonly MyDbContext _dbContext;

        private readonly OrderHelper _orderHelper;

        private readonly GlobalHelper _globalHelper;

        private readonly ProductHelper _productHelper;


        public IndexModel(WebsettingHelper websettinghelper, MyDbContext dbContext, OrderHelper orderHelper, GlobalHelper globalHelper, ProductHelper productHelper)
        {
            _websettinghelper = websettinghelper;
            _dbContext = dbContext;
            _orderHelper = orderHelper;
            _globalHelper = globalHelper;
            _productHelper = productHelper;


        }


        protected void licensevalidation()
        {
            var _licenseSettings = _websettinghelper.GetWebsettingJson("LicenseAppSettings");

            if (_licenseSettings != null && !string.IsNullOrEmpty(_licenseSettings))
            {

                var json = JsonConvert.DeserializeObject<LicenseAppSettingsModel>(_licenseSettings);



                if (json != null)
                {

                    licensesettings = new LicenseAppSettingsModel
                    {
                        LicenseKey = json.LicenseKey,
                        LicenseKeyForBrandRemoval = json.LicenseKeyForBrandRemoval,
                    };

                }
            }
        }



        #endregion
        public void OnGet()
        {
            licensevalidation();
            usersummary();

            revenuesummary();


            listingsummary();
        }

        public void usersummary()
        {



            TotalUsers = _dbContext.UsersProfiles.Count(u => u.Type != "Admin" && u.Type != "SubAdmin");

            DateTime today = DateTime.Today;
            TotalUsersToday = _dbContext.UsersProfiles.Count(u =>
                u.InsertDate.HasValue &&
                u.InsertDate.Value.Year == today.Year &&
                u.InsertDate.Value.Month == today.Month &&
                u.InsertDate.Value.Day == today.Day);

            // Total users registered this week
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            TotalUsersWeek = _dbContext.UsersProfiles.Count(u => u.InsertDate >= startOfWeek && u.InsertDate < today);

            // Total users registered this month
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            TotalUsersMonth = _dbContext.UsersProfiles.Count(u => u.InsertDate >= startOfMonth && u.InsertDate < today);

            // Total users registered this year
            DateTime startOfYear = new DateTime(today.Year, 1, 1);
            TotalUsersYear = _dbContext.UsersProfiles.Count(u => u.InsertDate >= startOfYear && u.InsertDate < today);




            TotalCustomers = _dbContext.UsersProfiles.Count(u => u.Type == "Client");



            // Total customer 
           


            TotalCustomersToday = _dbContext.UsersProfiles.Count(u =>
               u.InsertDate.HasValue &&
               u.InsertDate.Value.Year == today.Year &&
               u.InsertDate.Value.Month == today.Month &&
               u.InsertDate.Value.Day == today.Day &&
               u.Type =="Client"
               );


            TotalCustomersWeek = _dbContext.UsersProfiles.Count(u => u.Type == "Client" && u.InsertDate >= startOfWeek && u.InsertDate < today);


            TotalCustomersMonth = _dbContext.UsersProfiles.Count(u => u.Type == "Client" && u.InsertDate >= startOfMonth && u.InsertDate < today);

            TotalCustomersYear = _dbContext.UsersProfiles.Count(u => u.Type == "Client" && u.InsertDate >= startOfYear && u.InsertDate < today);

            TotalCustomersPurchaseProducts = _dbContext.UsersProfiles
     .Join(
         _dbContext.OrderMasters,
         user => user.ProfileId,
         order => order.BuyerId,
         (user, order) => new { User = user, Order = order }
     )
     .Where(joined => joined.User.Type == "Client" && joined.Order.OrderProcessStatus == "completed" && joined.Order.TransactionType == "purchased")
     .Distinct()
     .Count();
            //Total Seller

            TotalSellers = _dbContext.UsersProfiles.Count(u => u.Type == "Vendor");

           


            TotalSellersToday = _dbContext.UsersProfiles.Count(u =>
               u.InsertDate.HasValue &&
               u.InsertDate.Value.Year == today.Year &&
               u.InsertDate.Value.Month == today.Month &&
               u.InsertDate.Value.Day == today.Day &&
               u.Type == "Vendor"
               );


            TotalSellersWeek = _dbContext.UsersProfiles.Count(u => u.Type == "Vendor" && u.InsertDate >= startOfWeek && u.InsertDate < today);


            TotalSellersMonth = _dbContext.UsersProfiles.Count(u => u.Type == "Vendor" && u.InsertDate >= startOfMonth && u.InsertDate < today);

            TotalSellersYear = _dbContext.UsersProfiles.Count(u => u.Type == "Vendor" && u.InsertDate >= startOfYear && u.InsertDate < today);


            TotalSellersHaveProducts = _dbContext.UsersProfiles
                .Join(
                    _dbContext.ItemListings,
                    user => user.ProfileId,
                    item => item.ProfileId,
                    (user, item) => new { User = user, Item = item }
                )
                .Where(joined => joined.User.Type == "Vendor")
                .Select(joined => joined.User) // Select only the user part
                .Distinct() // Ensure unique users are counted
                .Count();
        }

        public void revenuesummary()
        {
            List<OrderViewModel>  ordermodel= _orderHelper.GetOrdersItem().Where(u => u.TransactionType =="purchased" && u.OrderProcessStatus =="completed").ToList();

          

            DateTime today = DateTime.Today;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            DateTime startOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime startOfYear = new DateTime(today.Year, 1, 1);



            BaseCurrency = _globalHelper.BaseCurrency();


            #region RevenueCredit
            TotalRevenueCredit = ordermodel.Where(u => u.ItemType == "credit").Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);
            // Assuming today is a DateTime representing the current date
           
           
            TotalRevenueCreditToday = ordermodel
    .Where(u =>
        u.ItemType == "credit" &&
       
        u.OrderDateDT.Year == today.Year &&
        u.OrderDateDT.Month == today.Month &&
        u.OrderDateDT.Day == today.Day)
    .Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);

            // Total users registered this week

            TotalRevenueCreditWeek = ordermodel
     .Where(u => u.ItemType == "credit" && u.OrderDateDT >= startOfWeek && u.OrderDateDT < today)
     .Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);

            // Total users registered this month
            TotalRevenueCreditMonth = ordermodel
                .Where(u => u.ItemType == "credit" && u.OrderDateDT >= startOfMonth && u.OrderDateDT < today)
                .Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);

            // Total users registered this year
            TotalRevenueCreditYear = ordermodel
                .Where(u => u.ItemType == "credit" && u.OrderDateDT >= startOfYear && u.OrderDateDT < today)
                .Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);
            #endregion

            #region RevenueSubscription
            TotalRevenueSubscription = ordermodel.Where(u => u.ItemType == "subscription").Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);


            // Assuming today is a DateTime representing the current date

          

            TotalRevenueSubscriptionToday = ordermodel
 .Where(u =>
     u.ItemType == "subscription" &&

     u.OrderDateDT.Year == today.Year &&
     u.OrderDateDT.Month == today.Month &&
     u.OrderDateDT.Day == today.Day)
 .Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);



            // Total users registered this week

            TotalRevenueSubscriptionWeek = ordermodel.Where(u => u.ItemType == "subscription" && u.OrderDateDT >= startOfWeek && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);


            // Total users registered this month

            TotalRevenueSubscriptionMonth = ordermodel.Where(u => u.ItemType == "subscription" && u.OrderDateDT >= startOfMonth && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);


            // Total users registered this year

            TotalRevenueSubscriptionYear = ordermodel.Where(u => u.ItemType == "subscription" && u.OrderDateDT >= startOfYear && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);

            #endregion


            //TotalRevenueItem = ordermodel.Where(u => u.ItemType == "item").Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);

            #region RevenueCommission
            TotalRevenueCommission = ordermodel.Sum(u => u.SummaryOrderMetaData.Commission);


            // Assuming today is a DateTime representing the current date

          

            TotalRevenueCommissionToday = ordermodel
.Where(u =>
  

    u.OrderDateDT.Year == today.Year &&
    u.OrderDateDT.Month == today.Month &&
    u.OrderDateDT.Day == today.Day)
.Sum(u => u.SummaryOrderMetaData.Commission);



            TotalRevenueCommissionWeek = ordermodel.Where(u => u.OrderDateDT >= startOfWeek && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.Commission);
            TotalRevenueCommissionMonth = ordermodel.Where(u => u.OrderDateDT >= startOfMonth && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.Commission);
            TotalRevenueCommissionYear = ordermodel.Where(u => u.OrderDateDT >= startOfYear && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.Commission);



            #endregion

            #region RevenueProfileBoost
            TotalRevenueProfileBoost = ordermodel.Where(u => u.ItemType == "profileboost").Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);
           


            TotalRevenueProfileBoostToday = ordermodel
.Where(u =>

u.ItemType == "profileboost" &&
 u.OrderDateDT.Year == today.Year &&
 u.OrderDateDT.Month == today.Month &&
 u.OrderDateDT.Day == today.Day)
.Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);


            TotalRevenueProfileBoostWeek = ordermodel.Where(u => u.ItemType == "profileboost" && u.OrderDateDT >= startOfWeek && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);
            TotalRevenueProfileBoostMonth = ordermodel.Where(u => u.ItemType == "profileboost" && u.OrderDateDT >= startOfMonth && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);
            TotalRevenueProfileBoostYear = ordermodel.Where(u => u.ItemType == "profileboost" && u.OrderDateDT >= startOfYear && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);


            #endregion


            #region RevenueItemBoost
            TotalRevenueItemBoost = ordermodel.Where(u => u.ItemType == "serviceboost").Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);
           


            TotalRevenueItemBoostToday = ordermodel
.Where(u =>

u.ItemType == "serviceboost" &&
u.OrderDateDT.Year == today.Year &&
u.OrderDateDT.Month == today.Month &&
u.OrderDateDT.Day == today.Day)
.Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);


            TotalRevenueItemBoostWeek = ordermodel.Where(u => u.ItemType == "serviceboost" && u.OrderDateDT >= startOfWeek && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);
            TotalRevenueItemBoostMonth = ordermodel.Where(u => u.ItemType == "serviceboost" && u.OrderDateDT >= startOfMonth && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);
            TotalRevenueItemBoostYear = ordermodel.Where(u => u.ItemType == "serviceboost" && u.OrderDateDT >= startOfYear && u.OrderDateDT < today).Sum(u => u.SummaryOrderMetaData.BaseGrandTotal);


            #endregion




            TotalRevenue = TotalRevenueCredit+ TotalRevenueSubscription+ TotalRevenueProfileBoost+ TotalRevenueItemBoost+ TotalRevenueCommission;
        }


        public void listingsummary()
        {
            List<ProductViewModel> productlist = _productHelper.productmasterdataV2(0, "adminsummary",0,0);


            GrandTotal = productlist.Count();

            TotalPhysical = productlist.Count(u=>u.ListingType == "Physical");
            TotalDigital = productlist.Count(u => u.ListingType == "Digital");
            TotalService = productlist.Count(u => u.ListingType == "Service");
            TotalCourses = productlist.Count(u => u.ListingType == "Course");



            TotalSell = productlist.Count(u => u.SellingType == "Sell");
            TotalClassified = productlist.Count(u => u.SellingType == "Classified");
            TotalAuction = productlist.Count(u => u.SellingType == "Auction");
            TotalPennyAuction = productlist.Count(u => u.SellingType == "Penny Auction");



            PhysicalSell = productlist.Count(u => u.ListingType == "Physical" && u.SellingType =="Sell");
            DigitalSell= productlist.Count(u => u.ListingType == "Digital" && u.SellingType == "Sell");
            ServiceSell = productlist.Count(u => u.ListingType == "Service" && u.SellingType == "Sell");
            CoursesSell= productlist.Count(u => u.ListingType == "Courses" && u.SellingType == "Sell");


            PhysicalClassified = productlist.Count(u => u.ListingType == "Physical" && u.SellingType == "Classified");
            DigitalClassified = productlist.Count(u => u.ListingType == "Digital" && u.SellingType == "Classified");
            ServiceClassified = productlist.Count(u => u.ListingType == "Service" && u.SellingType == "Classified");
            CoursesClassified = productlist.Count(u => u.ListingType == "Courses" && u.SellingType == "Classified");



            AuctionPhysical = productlist.Count(u => u.ListingType == "Physical" && u.SellingType == "Auction");
            AuctionDigital = productlist.Count(u => u.ListingType == "Digital" && u.SellingType == "Auction");


            PennyAuctionPhysical = productlist.Count(u => u.ListingType == "Physical" && u.SellingType == "PennyAuction");
            PennyAuctionDigital = productlist.Count(u => u.ListingType == "Digital" && u.SellingType == "PennyAuction");




        }
    }
}








