using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AMMasterProject
{
    public class Event
    {


        [Key]
        [Column("EventId")]
        public int EventId { get; set; }


       


        [Column("categoryid")]
        [DisplayName("Category")]
        [Required(ErrorMessage = "Category Is Required")]
        public int Categoryid { get; set; }


        [StringLength(100)]
        [DisplayName("Title")]

        [Required(ErrorMessage = "Title Is Required")]
        public string Title { get; set; }


        [Column("EventImage")]
        [StringLength(1000)]
        public string? EventImage { get; set; }


        [Column("Summary")]
        [StringLength(1000)]
        [DisplayName("Summary")]
        [Required(ErrorMessage = "Summary Is Required")]
        public string Summary { get; set; }


        [Column("Description")]

        [DisplayName("Description")]
        //[Required(ErrorMessage = "Description Is Required")]

        public string? Description { get; set; }


        [Column(TypeName = "date")]
        public DateTime? EventStartDate { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? EventStartTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EventEndDate { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? EventEndTime { get; set; }


        [Column(TypeName = "date")]
        public DateTime? LastDateOfRegistration { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? LastTimeOfRegistration { get; set; }


        [Column(TypeName = "datetime")]
        public DateTime? InsertDate { get; set; }



        [Column("TotalSeats")]
        [DisplayName("Total Seats")]
        [Required(ErrorMessage = "Total Seats Is Required")]

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Total Seats should be a whole number.")]
        public int TotalSeats { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Amount")]
        [Required(ErrorMessage = "Per Seat Fees Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Per Seat Fees should be a valid decimal number.")]
        public decimal Amount { get; set; }


        public int ProfileId { get; set; }

        public bool IsPublish { get; set; }







        [Column("isaddonhomepage")]
        [DefaultValue(false)]
        public bool Isaddonhomepage { get; set; }

        [Column("externalurl")]
        [StringLength(200)]
        [DisplayName("External url")]
        [Url(ErrorMessage = "Invalid URL")]
        public string? Externalurl { get; set; }


        [Column("SEOPageName")]
        [StringLength(100)]
        [DisplayName("SEO Page Name")]
        [Required(ErrorMessage = "SEO Page Name Is Required")]
        public string SeoPageName { get; set; }

        [Column("SEOTitle")]
        [StringLength(100)]
        [DisplayName("SEO Title Name")]
        [Required(ErrorMessage = "SEO Title Is Required")]
        public string SeoTitle { get; set; }

        [Column("SEOKeyword")]
        [StringLength(200)]
        [DisplayName("SEO Keyword")]
        [Required(ErrorMessage = "SEO Keyword Is Required")]
        public string SeoKeyword { get; set; }

        [Column("SEODescription")]
        [StringLength(200)]
        [DisplayName("SEO Description")]
        [Required(ErrorMessage = "SEO Description Is Required")]
        public string SeoDescription { get; set; }


    }
}