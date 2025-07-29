using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.shop
{
    public class IndexModel : PageModel
    {
        #region Models


        public string businessurlpath { get; set; }
        public SellerViewModel sellerview { get; set; }
        public ProductViewModel productmodel { get; set; }

        public string followstatus { get; set; }

        public int loginid { get; set; }

        #endregion

        #region DI


        private readonly ProductHelper _producthelper;
        private readonly UserHelper _userhelper;
        private readonly GlobalHelper _globalhelper;
        public IndexModel(ProductHelper producthelper, UserHelper userhelper, GlobalHelper globalhelper)
        {
            _producthelper = producthelper;
            _userhelper = userhelper;
            _globalhelper = globalhelper;


            
        }
        //public GlobalHelper GlobalHelper => _globalhelper;
        #endregion




        public async Task OnGetAsync()
        {
            businessurlpath = (string)RouteData.Values["businessurlpath"];

            Guid ProfileGUID = Guid.NewGuid();
          
            if (User.Identity.IsAuthenticated)
            {
                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value);

                loginid = int.Parse(User.FindFirst("UserID")?.Value);
                // continue with loginid variable
            }

            sellerview = _userhelper.SellerByBusinessUrlPath(businessurlpath);


            if (sellerview.SocialMediaList != null && sellerview.SocialMediaList.Count > 0)
            {
                foreach (var item in sellerview.SocialMediaList)
                {
                    string imagepath = _globalhelper.SocialMediaSettingIcon(item.SocialMediaID);
                    item.SocialMediaImage = imagepath;
                }
            }
            //var productlist = _producthelper.productmasterdata(loginid).Where(u=>u.Shopurlpath ==businessurlpath).ToList();


            var productlist = _producthelper.productmasterdataV2(loginid, "shopurlpath", 30, 1, 0, businessurlpath).ToList();



            productmodel = new ProductViewModel
            {
                ProductList = productlist
               

            };

            if (sellerview != null)
            {
                followstatus = _userhelper.followuserstatus(loginid, sellerview.ProfileId);
            }
        }
    }
}
