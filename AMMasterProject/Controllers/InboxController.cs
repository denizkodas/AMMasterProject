using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AMMasterProject.Controllers
{
    [Route("controller/[controller]/{action}")]
    [Controller]
    public class InboxController : Controller
    {

        #region DI


        private readonly InboxHelper _inboxHelper;
        private readonly FileUploadHelper _fileUploadHelper;
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userhelper;

        public InboxController(InboxHelper inboxHelper, MyDbContext context, FileUploadHelper fileUploadHelper, UserHelper userhelper)
        {
            _inboxHelper = inboxHelper;
            _fileUploadHelper = fileUploadHelper;
            _dbContext = context;
            _userhelper = userhelper;
        }
        #endregion


        #region StartChat


        public IActionResult createchat(int senderid, int recieverid)
        {
            string chatid = _inboxHelper.createchat(senderid, recieverid);

            if (chatid != string.Empty)
            {



                return Content(chatid);
            }
            else
            {
                // return an error or do some other action
                return View("Error");
            }
        }
        #endregion


        #region MessageList


        public IActionResult messagelist(Guid chatid, int loginuserid)
        {
            int pagesize = 20;
            int pagenumber = 1;
            if (Request.Query.ContainsKey("pagenumber"))
            {
                pagenumber = int.Parse(Request.Query["pagenumber"].ToString());
            }

            List<InboxViewModel> messgelist = _inboxHelper.messagelist(chatid, loginuserid);


        
            
            messgelist = messgelist.OrderByDescending(u=>u.messageid).Skip(((int)pagenumber - 1) * pagesize).Take(pagesize).ToList();


            messgelist = messgelist.OrderBy(u=>u.messageid).ToList();
            return PartialView("/Pages/Inbox/_messagelist.cshtml", messgelist);

        }
        #endregion
        #region ContactList


        public async Task<IActionResult> contactlist(int loginuserid)
        {
            List<InboxViewModel> contactlist = await _inboxHelper.inboxmycontacts(loginuserid);


            if (Request.Query.ContainsKey("contact"))
            {
                string contactname = Request.Query["contact"].ToString();

                contactlist = contactlist.Where(u => u.fullname.ToLower().Contains(contactname.ToLower())).ToList();

            }


            return PartialView("/Pages/Inbox/_contactlist.cshtml", contactlist);

        }
        #endregion


        #region Lastmessageupdate


        public IActionResult inboxmessageupdate(int chatId)
        {
            // Find the message by its ID
            Message message = _dbContext.Messages.FirstOrDefault(m => m.MesageId == chatId);

            if (message != null)
            {
                // Update the status
                message.Status = "Read";

                // Save changes to the database
                _dbContext.SaveChangesAsync();

                // Return a success message
                return Content("success");
            }
            else
            {
                // Handle the case where the message with the given ID is not found
                return NotFound();
            }
        }
          
        #endregion

        #region Attachment


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new Exception("No files received.");
                }
                var fileLink = await _fileUploadHelper.UploadFileAsync(file);

                var response = new
                {
                    FileName = file.FileName,
                    FileLink = fileLink
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error uploading file: " + ex.Message);
            }
        }
        #endregion


        #region SendMessage
        public IActionResult sendmessage(Guid chatid, string message, string attachment, string filename, int loginuserid)
        {

            
            string prompt = _inboxHelper.insertmessage(chatid, message, attachment, filename, loginuserid);
            if (prompt != string.Empty)
            {



                return Content(prompt);
            }
            else
            {
                // return an error or do some other action
                return View("Error");
            }
        }
        #endregion


        #region UserView
        public IActionResult UserView(Guid ProfileGuid)
        {
            UserGeneralView user = _userhelper.UserGeneralByGUID(ProfileGuid);


            return PartialView("/Pages/Inbox/_UserView.cshtml", user);

        }

        public IActionResult UserImageView(Guid ProfileGuid)
        {
            UserGeneralView user = _userhelper.UserGeneralByGUID(ProfileGuid);


            return PartialView("/Pages/Inbox/_UserImageView.cshtml", user);

        }
        #endregion
    }
}
