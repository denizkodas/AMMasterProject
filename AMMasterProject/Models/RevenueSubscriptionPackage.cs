using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMasterProject.Models
{

    [Table("RevenueSubscriptionPackage")]
    public class RevenueSubscriptionPackage
    {
        #region Mandatory



        [Key]
        [Column("RevenueSubscriptionPackageID")]

        public int RevenueSubscriptionPackageID { get; set; }


        [Column("RevenuePackageName")]
        [StringLength(200)]
        [DisplayName("Name")]
        [Required(ErrorMessage = "Name Is Required")]

        public string RevenuePackageName { get; set; }

       


        [Column("CurrencyId")]
        [DisplayName("Currency")]
        [Required(ErrorMessage = "Currency Is Required")]

        public int CurrencyID { get; set; }

        [Column("Sortnumber")]
        [DisplayName("Sort Order")]
        [Required(ErrorMessage = "Sort Order Is Required")]
        [DefaultValue(0)]
        public int Sortnumber { get; set; }

        [Column("CreditAmount")]
        [DisplayName("Credit Amount")]
        [Required(ErrorMessage = "Credit Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Credit Amount should be a valid decimal number.")]
        public decimal CreditAmount { get; set; }


        [Column("RecurringPeriodInDays")]
        [DisplayName("Recurring Period In Days")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid number.")]
        [Required(ErrorMessage = "Recurring Period In Days Is Required")]
        public int RecurringPeriodInDays { get; set; }

        [DisplayName("Insert Date")]
        public DateTime? InsertDate { get; set; }

        [Column("IsPublish")]
        [DisplayName("Is Active")]

        [DefaultValue(true)]
        public bool IsPublish { get; set; }


        [Column("IsDeleted")]
        [DisplayName("IsDeleted")]

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }



        [Column("IsRecommended")]
        [DisplayName("Is Recommended")]

        [DefaultValue(false)]
        public bool IsRecommended { get; set; }

        [Column("ProfileId")]

        public int ProfileId { get; set; }

        #endregion

        #region Optional

        [Column("Description")]
        [StringLength(150)]
        [DisplayName("Description")]
        [Required(ErrorMessage = "Description Is Required")]
        public string Description { get; set; }


        [StringLength(1000)]
        [DisplayName("Subscription Image")]
        public string? SubscriptionImage { get; set; }





       


        #endregion

    }
}
