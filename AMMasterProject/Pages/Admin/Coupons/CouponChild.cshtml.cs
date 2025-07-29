using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AMMasterProject.Pages.Admin.Coupons
{


    [Authorize]
    [BindProperties]
    public class CouponChildModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public List<CouponChildViewModel> couponChildList { get; set; }

        public List<ProductCouponChild> productCouponChildren { get; set; }


        public int ReferenceTypeId { get; set; }

        public int ProductCouponId { get; set; }
        private readonly ProductHelper _producthelper;


        #endregion

        #region DI

        public CouponChildModel(MyDbContext context, ProductHelper producthelper)
        {
            _dbContext = context;
            _producthelper = producthelper;




        }
        #endregion


        #region DataPopulate    

        public void setup()
        {
            if (Request.Query.ContainsKey("ID") && Request.Query.ContainsKey("couponchild"))
            {
                ReferenceTypeId = int.Parse(Request.Query["couponchild"].ToString());
                 ProductCouponId = int.Parse(Request.Query["ID"].ToString());

              

                if (ReferenceTypeId == 8)
                {


                    couponChildList = _producthelper.productmasterdataV2(0, "couponchildview", 500, 1)
                        .OrderBy(u=>u.ProductName)
                     .Select(p => new CouponChildViewModel
                     {
                         ReferenceId = p.ProductId,
                         ReferenceName = p.ProductName
                     })
                        .ToList();

                }
                else if (ReferenceTypeId == 9)
                {
                    couponChildList = _dbContext.CategoryMasters.OrderBy(u => u.CategoryName).Where(u => u.IsPublished == true).ToList()
                        .OrderBy(u => u.CategoryName)
                        .Select(p => new CouponChildViewModel
                        {
                            ReferenceId = p.CategoryId,
                            ReferenceName = p.CategoryName
                        })
                        .ToList();


                }

                else if (ReferenceTypeId == 10)
                {
                    couponChildList = _dbContext.UsersProfiles.Where(u => u.Type == "Vendor").ToList()
                         .OrderBy(u => u.Firstname)
                    .Select(p => new CouponChildViewModel
                    {
                        ReferenceId = p.ProfileId,
                        ReferenceName = p.Firstname + " " + p.Lastname
                    })
                        .ToList();

                }



                productCouponChildren = _dbContext.ProductCouponChildren.Where(u => u.ReferenceTypeID == ReferenceTypeId && u.ProductCouponId== ProductCouponId).ToList();

            }


        }
        #endregion
        public void OnGet()
         {
            setup();
        }
    }
}
