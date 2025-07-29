using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


namespace AMMasterProject.Components
{
    public class SellerViewComponent : ViewComponent
    {



        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _cache;
        private readonly UserHelper _userHelper;
        public SellerViewComponent(MyDbContext context, IMemoryCache cache, UserHelper userHelper)
        {
            _dbcontext = context;
            _cache = cache;
            _userHelper = userHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            try
            {
                // Materialize the ProductBoosts results with ToList()
                var productBoosts = _dbcontext.ProductBoosts.AsEnumerable().ToList();

                // Materialize the SellerList results with ToList()
                var sellerList = _userHelper.SellerList().ToList();

                var q = from pb in productBoosts
                        join order in _dbcontext.OrderMasters on pb.InvoiceNumber equals order.InvoiceNumber
                        join up in sellerList on pb.ItemBoostGUID equals up.ProfileGuid
                        orderby Guid.NewGuid()
                        where pb.StartDate <= DateTime.Now && pb.EndDate >= DateTime.Now &&
                              pb.BoostType == "profile" && order.PaymentStatus == "paid"
                        select new SellerViewModel
                        {
                            BusinessUrlpath = up.BusinessUrlpath,
                            Image = up.Image,
                            BusinessName = up.BusinessName,
                            ProfileId = up.ProfileId,
                            ProductTotal = up.ProductTotal //_dbcontext.ItemListings.Count(u => u.ProfileId == up.ProfileId && u.IsAdminLocked == false && u.IsPublish)
                        };

                var sellerlist = q.Distinct().ToList();
                return View(viewName, sellerlist); // Return a view to display the data
            }
            catch (Exception ex)
            {
                // Handle exception or log the error
                return View("ErrorView", ex.Message); // Return an error view
            }
        }

    }
}
