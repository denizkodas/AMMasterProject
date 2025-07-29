using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayPal.Api;

namespace AMMasterProject.Pages.shipping
{

    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region DI
        public CustomerAddress CustomerAddress { get; set; }
        private readonly OrderHelper _orderhelper;
        public string Invoicenumber { get; set; }

        public IndexModel( OrderHelper orderhelper)
        {


         
            _orderhelper = orderhelper;
          

        }
        #endregion


        public IActionResult OnGet()
        {
            GlobalHelper.SetReturnURL();

            Invoicenumber = GlobalHelper.ReadCookie("cartInvoiceNumber");

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable


            }
            ///if listing type has physical so shipping is required else do not need shipping
            ///
            var orderViewModelsList = _orderhelper.GetOrdersItem("cart", loginid).Where(u=>u.ItemDetailMetaData.basicModel.ListingType=="Physical").ToList();

            if(orderViewModelsList.Count==0)
            {
                string redirectUrl = $"/Payment/selection/{Invoicenumber}/item";
                return Redirect(redirectUrl);
            }
            else
            {
                return Page();
            }
        }

        //public IActionResult OnPostShipping()
        //{
        //    string invoicenumber = GlobalHelper.ReadCookie("cartInvoiceNumber");

        //    string redirectUrl = $"/Payment/selection/{invoicenumber}/item";
        //    return Redirect(redirectUrl);
            
        //}
    }
}
