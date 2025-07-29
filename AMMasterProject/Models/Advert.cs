using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMasterProject
{
    public class Advert
    {

        [Key]
        public int AdvertId { get; set; }

      /*  public int AdsPlacementId { get; set; } */ // 600 by 600, etc.

        public int AdsPageId { get; set; }


        [StringLength(1000)]
        [DisplayName("Image")]
        [Required(ErrorMessage = "Image Is Required")]
        public string Image { get; set; }

        [StringLength(500)]
        [DisplayName("Description")]
        public string? Description { get; set; }

        [StringLength(50)]
        [DisplayName("Status")]
        [Required(ErrorMessage = "Status Is Required")]
        public string Status { get; set; } // approved, reject, pending, review

        [StringLength(500)]
        [DisplayName("Remarks")]
        public string? Remarks { get; set; } // any comment by admin

        public int ProfileId { get; set; }

        [DefaultValue(false)]
        public bool IsUrl { get; set; }

        [DisplayName("URL")]
        [StringLength(500)]
        //[Required(ErrorMessage = "Url Is Required")]
        public string? Url { get; set; }

        [Column("InsertDate")]
        public DateTime? InsertDate { get; set; }

        [Column("StartDate")]
        [Required(ErrorMessage = "Start Date Is Required")]
        public DateTime StartDate { get; set; }


        [Column("EndDate")]
        [Required(ErrorMessage = "End Date Is Required")]
        public DateTime EndDate { get; set; }

    }
}