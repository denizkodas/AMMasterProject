using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace AMMasterProject.Pages.Blog
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(MyDbContext dbContext, ILogger<IndexModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


     


        public void OnGet()
        {
         


        }
    }
}
