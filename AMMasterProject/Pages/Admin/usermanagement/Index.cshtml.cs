using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;

namespace AMMasterProject.Pages.Admin.usermanagement
{


    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        #region DI
        public List<UserGeneralView> userList;
      
        private readonly UserHelper _userhelper;
        private readonly GlobalHelper _globalhelper;
        private readonly MembershipHelper _membershipHelper;
        public IndexModel(UserHelper userhelper, GlobalHelper globalHelper, MembershipHelper membershipHelper)
        {
           
            _userhelper = userhelper;
            _globalhelper = globalHelper;
            _membershipHelper = membershipHelper;
        }
        #endregion


        // Add a property to store the selected filter
        public string TimeFilter { get; set; } = "All"; // Default is "All"

        public void OnGet()
        {
          
            string usertype = (string)RouteData.Values["type"];
            string dateformat = _globalhelper.Dateformat();


            string url ="/admin/usermanagement/"+ usertype;
            GlobalHelper.SetReturnURLInSession(url);
            if (usertype.ToLower()=="buyer")
            {
                userList = (from up in _userhelper.ClientList()
                            where up.Type =="Client"
                                select new UserGeneralView
                                {
                                    ProfileId = up.ProfileId,
                                    ProfileGuid = up.ProfileGuid,
                                    Displayname = up.Displayname ?? (up.FirstName + " " + up.LastName),
                                    About = up.About,
                                    Contact = up.Contact?.ToString(),
                                    Email = up.Email,
                                    FirstName = up.FirstName,
                                    LastName = up.LastName,
                                    Image = up.Image,
                                    InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString(dateformat),
                                    Type=up.Type,
                                    LoginName = up.LoginName,
                                    sellerviewmodel = null,
                                     clientmodel = new ClientViewModel
                                     {
                                        PurchaseQtyTotal=up.PurchaseQtyTotal,
                                         // Assign other properties of SellerViewModel if needed
                                     },


                                    CreditPurchaseModel = _membershipHelper.CreditPurchaseLifeTime(up.ProfileId)

                                }).ToList();


                

            }
            else if(usertype.ToLower() == "seller")
            {
                userList = (from up in _userhelper.SellerList()
                            where up.Type == "Vendor"
                            select new UserGeneralView
                            {
                                ProfileId = up.ProfileId,
                                ProfileGuid = up.ProfileGuid,
                                Displayname = up.Displayname ?? (up.FirstName + " " + up.LastName),
                                About = up.About,
                                Contact = up.Contact?.ToString(),
                                Email = up.Email,
                                FirstName = up.FirstName,
                                LastName = up.LastName,
                                Image = up.SellerCoverImage,
                                InsertDate = DateTime.Parse(up.InsertDate.ToString()).ToString(dateformat),
                                 Type = up.Type,
                                sellerviewmodel = new SellerViewModel
                                {
                                    BusinessName = up.BusinessName,
                                    ProductTotal =up.ProductTotal,
                                    userothermetadata=up.userothermetadata,
                                    SalesQtyTotal =up.SalesQtyTotal,
                                    // Assign other properties of SellerViewModel if needed
                                },

                                //shift this as purchase model
                                //just like credit model
                                clientmodel = new ClientViewModel
                                {
                                    PurchaseQtyTotal = up.PurchaseQtyTotal,
                                    // Assign other properties of SellerViewModel if needed
                                },
                                LoginName =up.LoginName,

                                CreditPurchaseModel= _membershipHelper.CreditPurchaseLifeTime(up.ProfileId)
                               

                            }).ToList();
            }



            // Retrieve the selected filter from the query string
            string selectedFilter = Request.Query["timeFilter"];

            // Set the selected filter to the SelectedFilter property
            TimeFilter = string.IsNullOrEmpty(selectedFilter) ? "All" : selectedFilter;


            // Filter userList based on the selected filter
            FilterTimeUserList();
        }

        // Method to filter the user list based on the selected filter
        private void FilterTimeUserList()
        {
           
            string dateFormat = _globalhelper.Dateformat();
            switch (TimeFilter)
            {
                case "Today":
                    userList = userList.Where(u => DateTime.ParseExact(u.InsertDate, dateFormat, CultureInfo.InvariantCulture).Date == DateTime.Today).ToList();
                    break;
                case "ThisWeek":
                    userList = userList.Where(u => DateTime.ParseExact(u.InsertDate, dateFormat, CultureInfo.InvariantCulture).Date >= DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek)).ToList();
                    break;
                case "ThisMonth":
                    userList = userList.Where(u => DateTime.ParseExact(u.InsertDate, dateFormat, CultureInfo.InvariantCulture).Year == DateTime.Today.Year && DateTime.ParseExact(u.InsertDate, dateFormat, CultureInfo.InvariantCulture).Month == DateTime.Today.Month).ToList();
                    break;
                case "ThisYear":
                    userList = userList.Where(u => DateTime.ParseExact(u.InsertDate, dateFormat, CultureInfo.InvariantCulture).Year == DateTime.Today.Year).ToList();
                    break;
                // Add other cases as needed
                default:
                    // "All" case or default case
                    break;
            }
        }



    }
}
