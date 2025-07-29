using AMMasterProject.Controllers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AMMasterProject.Pages
{
    [BindProperties]
    public class PrivacyModel : PageModel
    {
        private readonly MyDbContext _dbContext;


        public List<CategoryMaster> Categories { get; set; }

        public ProductViewModel productdetail { get; set; }

        public PrivacyModel(MyDbContext dbContext)
        {
            _dbContext = dbContext;
           
        }

        public async Task OnGetAsync()
        {
            Categories = await _dbContext.CategoryMasters.ToListAsync();

            
        }


    }
}