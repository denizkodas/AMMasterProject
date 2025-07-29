using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AMMasterProject
{
    public class UserMembership
    {

        [Key]
        [Column("UserCreditId")]
        public int UserCreditId { get; set; }

        [Required(ErrorMessage = "ProfileId")]
        public int ProfileId { get; set; }


        [Column("TransationType")]
        [StringLength(50)]

        public string? TransactionType { get; set; }   //free purchased, used

        [Column("InvoiceNumber")]
        [StringLength(200)]

        public string? InvoiceNumber { get; set; }

        [Column("NoofCredit")]
        [DisplayName("No. of Credit")]
        [Required(ErrorMessage = "No. of Credit Is Required")]
        public int NoofCredit { get; set; }


        #region MetaData
        public string? UsageMetaData { get; set; }
        #endregion

        [Column("IsExpiry")]
        [DisplayName("Is Expiry")]

        [DefaultValue(false)]
        public bool IsExpiry { get; set; }



        [Column("ExpiryDate")]
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [Column("IsDeleted")]
        [DisplayName("IsDeleted")]

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }



        [Column("InsertDate")]
        [DisplayName("Insert Date")]
        public DateTime? InsertDate { get; set; }



       

        [Column("IsPublish")]
        [DefaultValue(true)]
        public bool IsPublish { get; set; }


    }
}