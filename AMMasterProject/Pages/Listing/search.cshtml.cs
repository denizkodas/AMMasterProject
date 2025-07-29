using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AMMasterProject.Pages.products
{

    [BindProperties]
    public class searchModel : PageModel
    {

        #region DI


        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _producthelper;
        public int QueryStringcategoryid { get; set; }
        public searchModel(MyDbContext dbContext, ProductHelper producthelper)
        {
            _dbContext = dbContext;
            _producthelper = producthelper;
        }

        #endregion

        #region Models


        public IEnumerable<SelectListItem> ProductType { get; set; }
        public IEnumerable<SelectListItem> SellingType { get; set; }

        public IEnumerable<CategorViewModel> CategoryList { get; set; }

        //public List<CategorViewModel> CategoryList { get; set; }
       
        #endregion



        public void OnGet()
        {

            #region Setup

           
            SellingType = _producthelper.GetSellingTypeList().Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Name,


            }).ToList();

            ProductType = _producthelper.GetUniqueListingTypes().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Name
            }).ToList();



            CategoryList = _producthelper.AllCategoryLevels().Select(i => new CategorViewModel
            {
                CategoryName = i.CategoryName.ToString(),
                CategoryId = i.CategoryId,
                LevelNumber = i.LevelNumber
            }).ToList();


            #endregion



            #region QueryString


            #region Binding


            // Check if the "producttype" query string parameter is present
            if (Request.Query.ContainsKey("producttype"))
            {
                // Get the selected values from the query string
                var selectedValues = Request.Query["producttype"].ToString().Split(',');

                // Loop through each SelectListItem in the ProductType list and set the Selected property
                foreach (var item in ProductType)
                {
                    if (selectedValues.Contains(item.Value) || selectedValues.Contains(item.Text))
                    {
                        item.Selected = true;
                    }
                }
            }



            if (Request.Query.ContainsKey("categoryid"))
            {
                // Get the selected values from the query string
                var selectedValues = Request.Query["categoryid"].ToString().Split(',');

                // Loop through each SelectListItem in the ProductType list and set the Selected property
                foreach (var item in CategoryList)
                {
                    if (selectedValues.Contains(item.CategoryId.ToString()) || selectedValues.Contains(item.CategoryName))
                    {
                        item.Selected = true;
                    }
                }
            }


        
         

            // Check if the "producttype" query string parameter is present
            if (Request.Query.ContainsKey("sellingtype"))
            {
                // Get the selected values from the query string
                var selectedValues = Request.Query["sellingtype"].ToString().Split(',');

                // Loop through each SelectListItem in the ProductType list and set the Selected property
                foreach (var item in SellingType)
                {
                    if (selectedValues.Contains(item.Value) || selectedValues.Contains(item.Text))
                    {
                        item.Selected = true;
                    }
                }
            }

            #endregion


          

            #endregion







       

           


        }




    }
}
