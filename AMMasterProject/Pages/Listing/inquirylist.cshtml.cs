using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Listing
{
    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class inquirylistModel : PageModel
    {



        #region Models
        private readonly ProductHelper _producthelper;
        public List<ProductQAViewModel> qalist { get; set; }

        public int loginid { get; set; }
        #endregion
        #region DI


        public inquirylistModel(ProductHelper productHelper)
        {

            _producthelper = productHelper;
        }


        #endregion

        public void OnGet()
        {

            loginid = 0;
            string usertype = "";
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                usertype = User.FindFirst("UserType")?.Value ?? "Client";
                // continue with loginid variable
            }
            qalist = _producthelper.productqabySellerId().OrderByDescending(u => u.QuestionId).ToList();

            if (usertype=="Vendor")
            {
                qalist= qalist.Where(u=>u.sellerid ==loginid).ToList();

            }

            if (Request.Query.ContainsKey("itemid"))
            {
                int itemid = int.Parse(Request.Query["itemid"].ToString());
                qalist = qalist.Where(u => u.productid == itemid).ToList();
            }


            if (Request.Query.ContainsKey("sellerid"))
            {
                int sellerid = int.Parse(Request.Query["sellerid"].ToString());
                qalist = qalist.Where(u => u.sellerid == sellerid).ToList();
            }

            if (Request.Query.ContainsKey("iteminquiryid"))
            {
                int iteminquiryid = int.Parse(Request.Query["iteminquiryid"].ToString());
                qalist = qalist.Where(u => u.QuestionId == iteminquiryid).ToList();
            }

        }
    }
}
