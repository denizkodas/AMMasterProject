using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin.usermanagement
{

    [Authorize(Policy = "Admin")]
    [BindProperties]
    public class assigncredittouserModel : PageModel
    {


        #region Model

      
        public string ID { get; set; }
        public string itemtype { get; set; }

        public string BaseCurrency { get; set; }

        public CreditAssignUserViewModel creditassignuserviewmodel { get; set; }


        public CreditAssignByAdminViewModel creditAssignByAdminViewModel { get; set; }

        private readonly UserHelper _userhelper;
        private readonly GlobalHelper _globalhelper;


        private readonly MembershipHelper _membershiphelper;
        private readonly OrderHelper _orderHelper;
        #endregion

        #region DI
        public assigncredittouserModel(GlobalHelper globalhelper, UserHelper userhelper, MembershipHelper membershipHelper, OrderHelper orderHelper)
        {
           
        
            _globalhelper = globalhelper;
            _userhelper = userhelper;
            _membershiphelper = membershipHelper;
            _orderHelper = orderHelper;
        }
        #endregion
        public void OnGet()
        {

            if (RouteData.Values["ID"] == null || RouteData.Values["itemtype"] == null)
            {
                // Redirect to error page or return an error response


                Response.Redirect("/Error?Title=Selection Fail For assign credit&Message=Something went wrong&Body=Please try again later.");
            }
            else
            {
                ID = (string)RouteData.Values["ID"];
                itemtype = (string)RouteData.Values["itemtype"];

                BaseCurrency = _globalhelper.BaseCurrency();

                profile(Guid.Parse(ID));
            }
        }

        public void profile(Guid ID)
        {
          
                UserGeneralView queryResult = _userhelper.UserGeneralByGUID(ID);



                if (queryResult != null)
                {
                    creditassignuserviewmodel = new CreditAssignUserViewModel
                    {
                        ProfileID = queryResult.ProfileId,
                        Name = queryResult.FirstName + " " + queryResult.LastName,  // Replace 'queryResult.Name' with the actual property in your 'ProductData' type
                        Image = queryResult.Image?? "/images/user-image-not-found.png", // Replace 'queryResult.Image' with the actual property in your 'ProductData' type
                        Description = queryResult.About, // Replace 'queryResult.Description' with the actual property in your 'ProductData' type
                        AvailableCredits = _membershiphelper.CreditAvailable(queryResult.ProfileId),
                        Email = queryResult.Email != null && queryResult.LoginChannel == "Email" ? queryResult.Email : null

                    };

                    // Now, you have a single AdvertiseViewModel instance.
                }
               
        }

        public IActionResult OnPostAssignCredit()
        {
            #region SignupCredits

             
            string assigncredit = creditAssignByAdminViewModel.AssignCredit.ToString();
            ID = (string)RouteData.Values["ID"];
            //itemtype = (string)RouteData.Values["itemtype"];

            //BaseCurrency = _globalhelper.BaseCurrency();

            profile(Guid.Parse(ID));

            TempData["success"] = assigncredit + " Credits assigned by admin to " + creditassignuserviewmodel.Name;
               
          


            string type = "credit";

            string invoicenumber = GlobalHelper.GetInvoiceNumber(creditassignuserviewmodel.ProfileID.ToString(), "credit");

            GlobalHelper.SetCookie("cartInvoiceNumber", invoicenumber);
            string custom = $"Credits assigned by admin to {creditassignuserviewmodel.Name}, {assigncredit},{creditAssignByAdminViewModel.AmountPaid}";

            string itemhelper = _orderHelper.ItemMetaData(type, "", "", 0, 1, "", invoicenumber, "", "", custom);

            // Deserialize the JSON string to an anonymous type with properties updatedjson and success
            var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


            string ItemMetadata = itemMetadataResult.updatedjson;
            string summarymetadata = itemMetadataResult.updatedsummaryjson;
            int sellerid = 0;

            string paymentmetadata = _orderHelper.paymentmetadata(creditAssignByAdminViewModel.PaymentMethod, "paid", creditAssignByAdminViewModel.PaymentReference, string.Empty, DateTime.Now, $"Manual credit for assigned", ItemMetadata, creditAssignByAdminViewModel.AmountPaid.ToString(), _globalhelper.BaseCurrency(), creditassignuserviewmodel.Email, invoicenumber);// payment metadata




            string orderstatus = _orderHelper.OrderCreation(creditassignuserviewmodel.ProfileID, sellerid, type, "confirm", "purchased", "completed", invoicenumber, ItemMetadata, paymentmetadata, "paid", summarymetadata);



            string url =GlobalHelper.GetReturnURL();

            return Redirect(url);


            #endregion
        }
    }
}
