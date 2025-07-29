using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.inbox
{
    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {

        #region DI

        
      
        private readonly UserHelper _userhelper;
        private readonly InboxHelper _inboxHelper;
       
        public IndexModel( UserHelper userhelper, InboxHelper inboxHelper)
        {

            _userhelper = userhelper;
            _inboxHelper = inboxHelper;
        }

        #endregion


        #region Models

     
        public List<InboxViewModel> inboxcontactlist { get; set; }
       
        #endregion

        public string chatid { get; set; }
        public async Task OnGetAsync()
        {

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

          
           


           
        }



        //public Message? Message { get; set; }
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    if (Message != null)
        //    {

        //        var files = HttpContext.Request.Form.Files;

        //        int loginid = 0;
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
        //            // continue with loginid variable
        //        }

        //        Message.ChatId = Guid.Parse(Request.Query["chatid"].ToString());


        //        await _inboxHelper.insertmessage(Message, loginid);

        //    }

        //    return RedirectToPage("/Pages/Inbox/Index");
        //}
    }
}
