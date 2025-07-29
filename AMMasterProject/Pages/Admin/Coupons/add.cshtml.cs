using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AMMasterProject.Pages.Admin.Coupons
{

    [Authorize]
    [BindProperties]
    public class addModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;

        public ProductCoupon productcoupon { get; set; }

        public int ProductCouponId { get; set; }
        public IEnumerable<SelectListItem> CouponType { get; set; }  ///vendor, seller, category, all, coming from generalsetup



        #endregion


        #region DI

        public addModel(MyDbContext context)
        {
            _dbContext = context;


            productcoupon = new ProductCoupon();
            productcoupon.IsPublish = true;
          

        }

        #endregion

        #region DataPopulate    

        public void setup()
        {





            //CouponType = _dbContext.GeneralSetups
            //    .OrderBy(u=>u.DisplayOrder)
            //.Where(u => u.GeneralSetupType == "Coupon Type")
            // .Select(u => new SelectListItem
            // {
            //     Value = u.GeneralSetupId.ToString(),
            //     Text = u.GeneralSetupName
            // }).ToList();




        }
        #endregion
        public void OnGet()
        {
            setup();
            if (Request.Query.ContainsKey("ID"))
            {
                ProductCouponId = int.Parse(Request.Query["ID"].ToString());


                productcoupon = _dbContext.ProductCoupons.FirstOrDefault(u =>  u.ProductCouponId == ProductCouponId);


            }
        }

        public IActionResult OnPost()
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

            #region ModelValidation


            if (productcoupon.DiscountType == "Percentage")
            {

                if (productcoupon.Discount > 100)
                {
                    ModelState.AddModelError("productcoupon.Discount", "Percentage should not be increased more than 100.");
                    setup();
                    return Page();

                }


            }

            if(productcoupon.StartDate ==null || productcoupon.EndDate ==null)
            {
                ModelState.AddModelError("productcoupon.EndDate", "Start and End is required");
                setup();
                return Page();

            }

            if (productcoupon.StartDate >= productcoupon.EndDate)
            {
                ModelState.AddModelError("productcoupon.EndDate", "End Date should be greater than Start Date");
                setup();
                return Page();

            }

            decimal percentage = productcoupon.SellerPercentage + productcoupon.AdminPercentage;

            if(percentage>100)
            {
                ModelState.AddModelError("productcoupon.SellerPercentage", "Admin and Seller Percentage sum should not be greater than 100");
                setup();
                return Page();
            }


            #endregion
            #region Up-sert

            //if (ModelState.IsValid)
            //{
                #region Duplicat

                ProductCoupon duplicate = _dbContext.ProductCoupons.FirstOrDefault(u=>u.CouponName.ToLower()==productcoupon.CouponName.ToLower() && u.ProductCouponId != productcoupon.ProductCouponId && u.EndDate > DateTime.Now);
                if(duplicate!=null)
                {


                    ModelState.AddModelError("productcoupon.CouponName", "Coupon already exist. End date is " + duplicate.EndDate);
                    setup();
                    return Page();
                }
                
                
                #endregion



                #region Insert

                if (productcoupon.ProductCouponId == 0)
                {
                    ProductCoupon insert = new ProductCoupon();


                    insert.CouponTypeId =productcoupon.CouponTypeId;
                    insert.CouponName= productcoupon.CouponName.Trim();
                    insert.CouponDescription= productcoupon.CouponDescription;
                    insert.StartDate= productcoupon.StartDate; 
                    insert.EndDate= productcoupon.EndDate;
                    insert.DiscountType= productcoupon.DiscountType;
                    insert.Discount = productcoupon.Discount;
                    insert.SellerPercentage= productcoupon.SellerPercentage;
                    insert.AdminPercentage= productcoupon.AdminPercentage;
                    insert.NoofCoupon= productcoupon.NoofCoupon;
                    insert.PerPersonUsed= productcoupon.PerPersonUsed;  
                   

                    insert.IsPublish =productcoupon.IsPublish;
                    insert.InsertDate = DateTime.Now;
                    insert.ProfileId = loginid;
                    _dbContext.ProductCoupons.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Coupon added successfully";
                }

                #endregion
                else
                {
                    ProductCoupon update = _dbContext.ProductCoupons.FirstOrDefault(u => u.ProductCouponId == productcoupon.ProductCouponId);

                    if (update != null)
                    {



                        update.CouponTypeId = productcoupon.CouponTypeId;
                        update.CouponName = productcoupon.CouponName.Trim();
                        update.CouponDescription = productcoupon.CouponDescription;
                        update.StartDate = productcoupon.StartDate;
                        update.EndDate = productcoupon.EndDate;
                        update.DiscountType = productcoupon.DiscountType;
                        update.Discount = productcoupon.Discount;
                        update.SellerPercentage = productcoupon.SellerPercentage;
                        update.AdminPercentage = productcoupon.AdminPercentage;
                        update.NoofCoupon = productcoupon.NoofCoupon;
                        update.PerPersonUsed = productcoupon.PerPersonUsed;

                        update.IsPublish = productcoupon.IsPublish;



                        _dbContext.ProductCoupons.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Coupon updated successfully";
                    }
                }

            #region Update

            #endregion
            //}

            //else
            //{
            //    setup();
            //    return Page();
            //}

            return RedirectToPage("/admin/coupons/index");

            /// it means is not all so either its product or category or vendor
            //if (productcoupon.CouponTypeId!=24)  
            //{

            //    TempData["success"] = "Please update the coupon records";
            //    return Redirect("/admin/coupons/couponchild?ID="+ productcoupon.ProductCouponId+ "&couponchild="+ productcoupon.CouponTypeId);
            //}
            //else
            //{
            //    return RedirectToPage("/admin/coupons/index");
            //}

           
            #endregion
        }
    }
}
