
using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AMMasterProject.Components
{
    public class ProductViewComponent : ViewComponent
    {

        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _cache;
        private readonly ProductHelper _producthelper;
        public ProductViewComponent(MyDbContext context, IMemoryCache cache, ProductHelper producthelper)
        {
            _dbcontext = context;
            _cache = cache;
            _producthelper = producthelper;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName, string type)
        {

            int loginuserid = 0;

            List<ProductViewModel> viewModelList = null;
            if (type == "BoostHome")
            {

                //_producthelper.productmasterdata(loginid).OrderBy(x => Guid.NewGuid()).ToList()
                viewModelList = _producthelper.productmasterdataV2(0, "boost", 30, 1, 0);




            }
            if (type == "DiscountSale")
            {

                //_producthelper.productmasterdata(loginid).OrderBy(x => Guid.NewGuid()).ToList()
                viewModelList = _producthelper.productmasterdataV2(0, "discount", 20, 1, 0);




            }


            // Return a view to display the data
            return View(viewName, viewModelList);
        }

    }
}
