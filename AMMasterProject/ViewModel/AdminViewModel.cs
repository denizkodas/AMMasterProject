using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AMMasterProject.ViewModel
{
    public class AdminViewModel
    {
    }

    #region MetaData
    public class AdminAddressMetaData : AdminAddressModel
    {

    }

    #endregion

    #region CompanySetup


    public class CompanySetupModel
    {
        [Column("Logo")]
        [StringLength(1000)]
        [DisplayName("Logo")]
        [Required(ErrorMessage = "Logo Is Required")]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.jpg|.png)$", ErrorMessage = "Only JPG or PNG files are allowed")]
        public string Logo { get; set; }


        [Column("Favicon")]
        [StringLength(1000)]
        [DisplayName("Favicon")]
        [Required(ErrorMessage = "Favicon Is Required")]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.ico)$", ErrorMessage = "Only .ico files is allowed")]
        public string Favicon { get; set; }



        [Column("CompanyName")]
        [StringLength(200)]
        [DisplayName("Company Name")]
        [Required(ErrorMessage = "Company Name Is Required")]
        public string CompanyName { get; set; }


        [Column("CompanyDescription")]
        [StringLength(500)]
        [DisplayName("Company Description")]
        [Required(ErrorMessage = "Company Description Is Required")]
        public string CompanyDescription { get; set; }


        [Column("CompanyAddress")]
        [StringLength(200)]
        [DisplayName("Company Address")]
        [Required(ErrorMessage = "Company Address Is Required")]
        public string CompanyAddress { get; set; }

        [StringLength(30)]
        [Column("SupportContact")]
        [DisplayName("Support Contact")]
        [Required(ErrorMessage = "Support Contact Is Required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit contact number.")]
        public string SupportContact { get; set; }


        [StringLength(300)]
        [Column("SupportEmail")]
        [DisplayName("Support Email")]
        [Required(ErrorMessage = "Support Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]

        public string SupportEmail { get; set; }

        [StringLength(100)]
        [Column("MetaTitle")]
        [DisplayName("Meta Title")]
        [Required(ErrorMessage = "Meta Title Is Required")]

        public string MetaTitle { get; set; }

        [StringLength(200)]
        [Column("MetaKeyword")]
        [DisplayName("Meta Keyword")]
        [Required(ErrorMessage = "Meta Keyword Is Required")]

        public string MetaKeyword { get; set; }


        [StringLength(200)]
        [Column("MetaDescription")]
        [DisplayName("Meta Description")]
        [Required(ErrorMessage = "Meta Description Is Required")]

        public string MetaDescription { get; set; }




       
        [Column("IsMultiVendor")]
        [DisplayName("Is MultiVendor")]
        [Required(ErrorMessage = "Is MultiVendor Is Required")]
        public bool IsMultiVendor { get; set; }
    }



    #endregion


    #region LocationSetup
    public class AdminAddressModel : AddressViewModel
    {

        public int? AddressID { get; set; }

        public Guid? AddressGUID { get; set; }


        [StringLength(50)]
        [DisplayName("Type")]
        [Required(ErrorMessage = "Type Is Required")]
        public string Type { get; set; }

        [StringLength(200)]
        [DisplayName("Store Name")]
        [Required(ErrorMessage = "Store Name Is Required")]
        public string StoreName { get; set; }
        [DisplayName("Contact Number")]
        [StringLength(50)]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit contact number.")]
        [Required(ErrorMessage = "Contact Number Is Required")]
        public string ContactNumber { get; set; }

        [StringLength(300)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
        public bool IsPublish { get; set; }

    }
    #endregion


   
}
