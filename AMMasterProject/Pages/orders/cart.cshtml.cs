using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using PayPal.Api;

namespace AMMasterProject.Pages.orders
{
    public class cartModel : PageModel
    {

        #region DI


        public List<OrderViewModel> orderViewModelsList { get; set; }

        
        private readonly OrderHelper _orderhelper;
        public ProductViewModel productmodel { get; set; }
        public cartModel(OrderHelper orderhelper)
        {
          

          
            _orderhelper = orderhelper;
        }


        #endregion

        //public void OnGet()
        //{
        //    int loginid = 0;
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");


               

        //        string  Invoicenumber = GlobalHelper.ReadCookie("cartInvoiceNumber");

        //        if(Invoicenumber == null)
        //        {
        //            Invoicenumber = _orderhelper.invoicenumberactive(loginid, "", "");
        //        }
        //    }

        //}
    }
}
