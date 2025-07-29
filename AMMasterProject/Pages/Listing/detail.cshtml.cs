
using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace AMMasterProject.Pages.products
{

    [BindProperties]
   
    public class detailModel : PageModel
    {
        #region Models
       

        public bool IsMultiVendor { get; set; }
        public string productseourl { get; set; }
        public int loginid { get; set; }
        public int productid { get; set; }
        public Guid productguid { get; set; }
        public ProductViewModel productmodel { get; set; }
        public List<ProductViewModel> productrelatedModel { get; set; }

        public List<ProductViewModel> productotherSellerModel { get; set; }

        public List<ProductViewModel> productcrossModel{ get; set; }
        public SellerViewModel sellerview { get; set; }

        public AmenitiesViewModel combinedAmenities { get; set; }

        //public List<ReviewMetaDataViewModel> productreviewmodel { get; set; }
        //public List<ProductQAViewModel> productqamodel { get; set; }





        #endregion
        #region DI



        private readonly ProductHelper _producthelper;
        private readonly OrderHelper _orderHelper;
        private readonly UserHelper _userhelper;

       

        public detailModel(ProductHelper producthelper, UserHelper userhelper, OrderHelper orderhelper)
        {
            _producthelper = producthelper;
            _userhelper = userhelper;
            _orderHelper = orderhelper;
           

        }

        #endregion






        public void OnGet()
        { // use the userId variable as needed


            try
            {

           
            ///set current url in session
           GlobalHelper.SetReturnURL();

            IsMultiVendor = bool.Parse(HttpContext.Items["IsMultiVendor"].ToString());


            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }



            productseourl = (string)RouteData.Values["productseourl"];



            // ProductViewModel  productdetail = _producthelper.productmasterdata(loginid).Where(u => u.ProductSeourl == productseourl).FirstOrDefault();

            ProductViewModel productdetail = _producthelper.productmasterdataV2(loginid, "seourl", 30, 1, 0, productseourl).FirstOrDefault();

                if (productdetail == null)
                {
                     RedirectToPage("/Error", new { Title = "Item not available", Message = "Item you are looking for is not available" });
                    return;
                }



                productid = productdetail.ProductId;
            productguid = productdetail.ProductGUID;
            //List<ProductViewModel> productimage = _producthelper.productimages(productseourl);

            //List<ProductViewModel> productamenitiesheader = _producthelper.productamenitiesheader(productdetail.ListAmenityMetaData);
            //List<ProductViewModel> productamenitieschild = _producthelper.productamenitieschild(productdetail.ListAmenityMetaData);


                 combinedAmenities = _producthelper.GetCombinedAmenities(productdetail.ListAmenityMetaData);




                productmodel = new ProductViewModel
            {
                ProductDetail = productdetail,
                //ProductImages = productimage,
                //ProductAmenityHeader = productamenitiesheader,
                //ProductAmenityChild = productamenitieschild,
                //AmenityViewModel = combinedAmenities,

                ProductAttributeViewModel = _producthelper.GetProductAttributes(productguid),
                
            };

            //if (IsMultiVendor == true)
            //{
                sellerview = _userhelper.SellerByGUID(productdetail.profileguid);

            //}

            //productreviewmodel = _orderHelper.ReviewListItemID(productdetail.ProductId);


             //productqamodel = _producthelper.productqa(productdetail.ProductId);


            productrelatedModel = _producthelper.productmasterdataV2(loginid, "related_detail", 5, 1, productdetail.ProductId, productdetail.ProductName.ToLower());
            productotherSellerModel = _producthelper.productmasterdataV2(loginid, "related_detail_same_seller", 5, 1, productdetail.ProductId, productdetail.ProductName.ToLower());

            productcrossModel = _producthelper.productmasterdataV2(loginid, "cross_detail", 5, 1, productdetail.ProductId, productdetail.ProductName.ToLower());



            ///create views and click on this page
            ///
            // Check if the user has already interacted with the page


            // Set session data

            // Check if the user has already interacted with the page
            // Check if the session variable exists
            // Check if the session exists

            if (HttpContext.Session.TryGetValue("viewclickproductid", out byte[] productIdBytes))
            {
                // Deserialize the byte array into a string
                string productIdInSession = Encoding.UTF8.GetString(productIdBytes);

                // Check if the specific product ID exists in the session
                if (productIdInSession == productdetail.ProductId.ToString())
                {
                    // User has already interacted with this product, do not update counts
                    //Console.WriteLine("User has already interacted with this product. Ignoring interaction.");
                }
                else
                {
                    // Update counts only if the session exists and product ID doesn't match
                    _producthelper.ItemOtherMetaDataUpdate(productdetail.ProductId, "", "viewclick");

                    // Set the session variable to indicate interaction for this product
                    HttpContext.Session.SetString("viewclickproductid", productdetail.ProductId.ToString());
                }
            }
            else
            {
                // Session doesn't exist, or the specific product ID is not in the session
                // Perform any necessary initialization or set the session variable here
                HttpContext.Session.SetString("viewclickproductid", productdetail.ProductId.ToString());

                // Update counts since this is the first interaction
                _producthelper.ItemOtherMetaDataUpdate(productdetail.ProductId, "", "viewclick");
            }
            }
            catch (Exception ex)
            {

                 RedirectToPage("/Error", new { Title = "Some thing went wrong", Message = ex.Message });
                return;
            }

        }
    }
}
