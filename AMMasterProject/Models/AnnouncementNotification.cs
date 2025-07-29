using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AMMasterProject
{
    public class AnnouncementNotification
    {

        [Key]
        [Column("AnnouncementId")]
        public int AnnouncementId { get; set; }

        [Column("AnnouncementGUID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AnnouncementGUID { get; set; }


        [Column("ProfileID")]
        public int? ProfileID { get; set; }   ///for notification to whom its sending

        [Column("Title")]
        [StringLength(100)]
        [DisplayName("Title")]
        [Required(ErrorMessage = "Title Is Required")]
        public string Title { get; set; }


        [Column("ImageURL")]
        [StringLength(1000)]
        [DisplayName("ImageURL")]
      
        public string? ImageURL { get; set; }

        [Column("RedirectURL")]
        [StringLength(300)]
        [DisplayName("RedirectURL")]

        public string? RedirectURL { get; set; }

        [Column("AnnouncementFor")]
        [StringLength(50)]
        [DisplayName("Announcement For")]
        [Required(ErrorMessage = "Announcement For Is Required")]
        public string AnnouncementFor { get; set; }   //buyer seller

        [Column("Description")]
        [StringLength(500)]
        [DisplayName("Description")]
        [Required(ErrorMessage = "Description Is Required")]
        public string Description { get; set; }

        [Column("InsertDate")]
        [DisplayName("Insert Date")]
        public DateTime? InsertDate { get; set; }



        [Column("StartDate")]
        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }


        [Column("ExpiryDate")]
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate { get; set; }


        [Column("IsPublish")]
        [DefaultValue(true)]
        public bool IsPublish { get; set; }

    }
}