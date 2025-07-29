using System.ComponentModel.DataAnnotations;

namespace AMMasterProject.ViewModel
{
    public class InboxViewModel
    {
        public int messageid { get; set; }

        public string chatid { get; set; }

        public int contactid { get; set; }
        public Guid contactguid { get; set; }

        public string fullname { get; set; }

        public string image { get; set; }

        public string firstchar { get; set; }

        public string message { get; set; }

        public string readstatus { get; set; }
        public string type { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd,yyyy}")]
        public string insertdate { get; set; }


        public string attachment { get; set; }
        public string filename { get; set; }

    }
}
