using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AMMasterProject.Models
{
    public class UserWallet
    {

        [Key]
        [Column("UserWalletID")]
        public int UserWalletID { get; set; }


        
        [Column("ProfileID")]
        public int ProfileID { get; set; }


        [Column("UserWalletMetaData")]
        public string? UserWalletMetaData { get; set; }

        [Column("InvoiceNumber")]
        [StringLength(200)]
        public string InvoiceNumber { get; set; }


        [Column("Type")]
        [StringLength(50)]
        public string Type { get; set; }

        [Column("IsApplied")]
       
        public bool IsApplied { get; set; }

        [Column(TypeName = "datetime")]
        [DisplayName("Insert Date")]
        public DateTime InsertDate { get; set; }

    }
}
