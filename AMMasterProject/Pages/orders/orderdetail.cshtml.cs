using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.orders
{

    [Authorize(Policy = "SellerAdmin")]
    public class orderdetailModel : PageModel
    {
        public string usertype { get; set; }    
        public void OnGet()
        {

            usertype= User.FindFirst("UserType")?.Value ?? "0";
        }
    }
}
