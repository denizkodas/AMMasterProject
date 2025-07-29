using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMMasterProject.Models
{
    public class NotificationRelay
    {

        [Key]

        [Column("NotificationContentId")]
        public int NotificationRelayId { get; set; }

        [Column("NotificationChannel")]
        public int NotificationChannel { get; set; }  //0 Email  - 1 = sms


        [Column("ProfileId")]
        public int ProfileId { get; set; }



        [StringLength(300)]
        [Column("Receiver")]
        public string Receiver { get; set; }  //email or mobile number


        [Column("InsertDate")]
        public DateTime InsertDate { get; set; }


        //[StringLength(300)]
        [Column("NotificationRelayBody")]
        public string NotificationRelayBody { get; set; }

        [StringLength(100)]
        [Column("NotificationRelaySubject")]
        public string NotificationRelaySubject { get; set; }


        [Column("Issent")]
        public bool Issent { get; set; }

        [Column("IsRead")]
        public bool IsRead { get; set; }



        [Column("DeliveryDate")]
        public DateTime DeliveryDate { get; set; }



        [StringLength(300)]
        [Column("RedirectUrl")]
        public string RedirectUrl { get; set; }

    }
}
