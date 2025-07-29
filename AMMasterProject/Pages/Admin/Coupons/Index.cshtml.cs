using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Coupons
{

    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public List<ProductCoupon> productcouponlist { get; set; }


        #endregion

        #region DI

        public IndexModel(MyDbContext context)
        {
            _dbContext = context;


        }

        #endregion

        #region DataPopulate    

        public void setup()
        {


            productcouponlist = _dbContext.ProductCoupons.OrderBy(u=>u.EndDate).ToList();



        }
        #endregion
        public void OnGet()
        {
            setup();
        }

        public IActionResult OnPostDelete(int productcouponid)
        {
            ProductCoupon productcoupon = _dbContext.ProductCoupons.FirstOrDefault(u => u.ProductCouponId == productcouponid );

            if (productcoupon != null)
            {

                _dbContext.ProductCoupons.Remove(productcoupon);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";


                return RedirectToPage("/admin/coupons/Index");


            }

            setup();
            return Page();
        }

    }
}
