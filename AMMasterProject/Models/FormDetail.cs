using System.ComponentModel.DataAnnotations;

namespace AMMasterProject.Models
{
    public class FormDetail
    {

        [Key]
        public int FormDetailsID  {get;set;}

        public string FormContent { get; set; }

        public DateTime SubmitDate { get; set; }


        public bool IsRead { get; set; }

        public string ContentReply { get; set; }

    }
}
