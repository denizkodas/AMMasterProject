using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Users_Profile")]
public partial class UsersProfile
{


    #region Required
    [Key]
    [Column("ProfileId")]
    public int ProfileId { get; set; }

    [Column("ProfileGUID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProfileGuid { get; set; }




    #endregion

    #region BothProperties
    [Column("Firstname")]
    [StringLength(50)]
    public string? Firstname { get; set; }



    [Column("Lastname")]
    [StringLength(50)]
    public string? Lastname { get; set; }


    //[Column("Dateofbirth")]
    //public DateTime? Dateofbirth { get; set; }



    //[StringLength(10)]
    //[Column("Gender")]
    //public string? Gender { get; set; }


    #endregion

    #region Login   

    [Column("UserName")]
    [StringLength(300)]
    public string? UserName { get; set; }


    [Column("Password")]
    [StringLength(200)]

    public string? Password { get; set; }


    [StringLength(20)]
    [Column("Loginchannel")]
    public string? Loginchannel { get; set; }


    [Column("Lastlogin")]
    public DateTime? Lastlogin { get; set; }


    //[StringLength(20)]
    //[Column("VerificationCode")]
    //public string? VerificationCode { get; set; }


    //[Column("VerificationCodeDate")]
    //public DateTime? VerificationCodeDate { get; set; }



    //[Column("IsVerificationCode")]
    //public bool? IsVerificationCode { get; set; }


    [Column("IsUserNameVerified")]
    public bool? IsUserNameVerified { get; set; }


    [Column("IsLockedByAdmin")]
    public bool? IsLockedByAdmin { get; set; } //if false so not locked


    [StringLength(1000)]
    [Column("AdminRemarksOnLocked")]
    public string? AdminRemarksOnLocked { get; set; }



    [Column("UnLockedDate")]
    public DateTime? UnLockedDate { get; set; }

    #endregion

    #region GeneralStatus


    [Column("UserType")]   ///Client or Vendor
    [StringLength(20)]
    public string? Type { get; set; }

    [Column("Datetime")]
    public DateTime? InsertDate { get; set; }


    //[Column("AdminStatus")]
    //[StringLength(50)]
    //public string? AdminStatus { get; set; }


    //[Column("Status")]
    //[StringLength(50)]
    //public string? Status { get; set; }



    //[Column("Isprofilecompleted")]
    //public bool? Isprofilecompleted { get; set; }

    //[Column("Affiliateid")]
    //public int? Affiliateid { get; set; }

    #endregion










    #region Client


    [Column("ProfileImage")]
    [StringLength(1000)]
    public string? ProfileImage { get; set; }


    [Column("ClientDisplayName")]
    [StringLength(100)]

    public string? ClientDisplayName { get; set; }

    [Column("About")]
    [StringLength(500)]
    public string? About { get; set; }



    #endregion

    #region Seller


    [Column("SellerDisplayName")]
    [StringLength(100)]
    public string? SellerDisplayName { get; set; }

    [Column("SellerCoverImage")]
    [StringLength(1000)]
    public string? SellerCoverImage { get; set; }


    [Column("SellerImage")]
    [StringLength(1000)]
    public string? SellerImage { get; set; }


    //[Column("Followers")]
    //public int? Followers { get; set; }


    //[Column("Averagerating")]
    //public int? Averagerating { get; set; }

    [StringLength(50)]
  
    public string? Provider { get; set; }

    [StringLength(1000)]
    [Column("SellerVideoURL")]
   
    public string? SellerVideoURl { get; set; }

    public DateTime? DeletedDate { get; set; }
    public bool? IsDeleted { get; set; }
    #endregion

    #region Business


  
    [Column("BusinessType")]
    public int? BusinessType { get; set; }   ///0= Individual  1= Business

    [StringLength(200)]
    [Column("BusinessName")]
    public string? BusinessName { get; set; }

    [Column("BusinessURLPath")]
    [StringLength(200)]
    public string? BusinessUrlpath { get; set; }


    [Column("BusinessDescription")]
    [StringLength(2000)]
    public string? BusinessDescription { get; set; }


    #endregion

    #region Contact
    [Column("ContactNumber")]
    [StringLength(50)]
   
    public string? ContactNumber { get; set; }

    [Column("Email")]
    [StringLength(300)]
  
    public string? Email { get; set; }


    [Column("Address")]
    [StringLength(1000)]
    public string? Address { get; set; }


    //[Column("Country")]
    //[StringLength(50)]
    //public string? Country { get; set; }


    //[Column("City")]
    //[StringLength(50)]
    //public string? City { get; set; }


    //[Column("Zip")]
    //[StringLength(50)]
    //public string? Zip { get; set; }


    //[Column("State")]
    //[StringLength(50)]
    //public string? State { get; set; }





    //[Column("Latitude")]
    //[StringLength(100)]
    //public string? Latitude { get; set; }

    //[Column("Longitude")]
    //[StringLength(100)]
    //public string? Longitude { get; set; }

    #endregion





    #region Metata


    [Column("PrimaryAddressMetaData")]
    [StringLength(500)]
    public string? PrimaryAddressMetaData { get; set; }//single json 


    [Column("BusinessMetaData")]
    [StringLength(1000)]
    public string? BusinessMetaData { get; set; }//single json 


    [Column("SecondaryAddressMetaData")]
    public string? SecondaryAddressMetaData { get; set; } //list of address


    [Column("SecondaryContactMetaData")]
    [StringLength(4000)]
    public string? SecondaryContactMetaData { get; set; }//list 

    [Column("AnnouncementMetaData")]
    public string? AnnouncementMetaData { get; set; }//list of address


    [Column("ProfileVerificationMetaData")]
    [StringLength(2000)]
    public string? ProfileVerificationMetaData { get; set; }//list of address



    [Column("SellerPayAccountMetaData")]
    [StringLength(3000)]
    public string? SellerPayAccountMetaData { get; set; }//list 

   


    [Column("SocialMediaMetaData")]
    [StringLength(4000)]
    public string? SocialMediaMetaData { get; set; }//list 


    [Column("TeamMembersMetaData")]
  
    public string? TeamMembersMetaData { get; set; }//list 


 


    [Column("IdentityProofMetaData")]
    [StringLength(4000)]
    public string? IdentityProofMetaData { get; set; }//list 


    [Column("CertificateProofMetaData")]
    [StringLength(4000)]
    public string? CertificateProofMetaData { get; set; }//list 



    [Column("AdminStatusMetaData")]
    public string? AdminStatusMetaData { get; set; }//list 



    [Column("AvailabilitySetupMetaData")]
    public string? AvailabilitySetupMetaData { get; set; }//list 



    [Column("OtherMetaData")]
    public string? OtherMetaData { get; set; }//list 

    #endregion













































}
