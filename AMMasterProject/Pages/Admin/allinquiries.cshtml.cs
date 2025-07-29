using AMMasterProject.Helpers;
using AMMasterProject.Models;
//using AMMasterProject.Pages.Shared;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using AMMasterProject.Models;

namespace AMMasterProject.Pages.Admin
{

    [BindProperties]
    public class allinquiriesModel : PageModel
    {
        private readonly MyDbContext _dbContext;

        public List<FormDetail> listformsubmitted { get; set; }


        public allinquiriesModel(MyDbContext dbContext)
        {

            _dbContext = dbContext;


        }

        public void setup()
        {


            if (Request.Query.TryGetValue("readstatus", out var readStatusParam))
            {
                // Value of the "readstatus" parameter from the query string
                string readStatusValue = readStatusParam.ToString();

                if (readStatusValue == "true")
                {
                    // Filter records where IsRead is true
                    listformsubmitted = _dbContext.FormDetails.Where(fd => fd.IsRead).ToList();
                }
                else if (readStatusValue == "false")
                {
                    // Filter records where IsRead is false
                    listformsubmitted = _dbContext.FormDetails.Where(fd => !fd.IsRead).ToList();
                }
                else
                {
                    // Invalid or unrecognized parameter value; return a default view
                    listformsubmitted = _dbContext.FormDetails.Where(fd => !fd.IsRead).ToList();
                }
            }
            else
            {
                // If "readstatus" query string parameter doesn't exist, show IsRead==false records
                listformsubmitted = _dbContext.FormDetails.Where(fd => !fd.IsRead).ToList();
            }



        }
        public void OnGet()
        {
            setup();

        }
    }
}
