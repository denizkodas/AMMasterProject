using System.Security.Policy;

namespace AMMasterProject.ViewModel
{
    public class NotificationViewModel
    {
    }

    #region AnnouncementAll
    public class AnnouncementViewModel
    {
        public int AnnouncementId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? ExpiryDate { get; set;}

        public bool IsUserRead { get; set; }

        public string RedirectUrl { get; set; }
    }

    public class AnnouncementCounterViewModel
    {
        public int AnnouncementCounter { get; set; }
        public bool IsUserRead { get; set; }
    }
    #endregion
}
