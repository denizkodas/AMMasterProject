using AMMasterProject.Helpers;
using Microsoft.Identity.Client;
using System.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace AMMasterProject.ViewModel
{

    #region BothModel

   


    public class UserGeneralView
    {

        public int ProfileId { get; set; }

        public Guid ProfileGuid { get; set; }

        public string LoginChannel { get; set; }

        public string LoginName { get; set; }
        public string Type { get; set; }
        public string Displayname { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Dateofbirth { get; set; }

        public string? Gender { get; set; }
        public string Image { get; set; }

        public string InsertDate { get; set; }

        public string About { get; set; }

        public string? Contact { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }

        public string? CurrentTime { get; set; }

        //public string BusinessName { get; set; }

        //public string BusinessUrlPath { get; set; }

        //public string BusinessDescription { get; set; }
        //public string LevelName { get; set; }

        public AddressViewModel primaryaddressViewModel { get; set; }

        public UserOtherMetaData userothermetadata { get; set; }
        public SellerViewModel sellerviewmodel { get; set; }

        public ClientViewModel clientmodel { get; set; }

        
        public CreditLifeTimeMetaDataViewModel CreditPurchaseModel { get; set; }

        public List<SubscriptionLifeTimeMetaDataViewModel> SubscriptionPurchaseModel { get; set; }
    }

    #endregion


    #region Contact
    public class ContactModel
    {
        [StringLength(50)]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit contact number.")]
        public string Contactnumber { get; set; }
    }

    #endregion


    #region Client

    public class ClientViewModel:UserGeneralView
    {
        public decimal PurchaseAmountTotal { get; set; }
        public int PurchaseQtyTotal { get; set; }

        public int WishListTotal { get; set; }

        public int FollowingSeller { get; set; }


       
    }




















    public class ClientProfileModel
    {

        #region Mandatory


        public int ProfileID { get; set; }


        public Guid ProfileGuid { get; set; }


        [StringLength(50)]
        [DisplayName("First Name")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Only characters are allowed.")]
        [Required(ErrorMessage = "First Name Is Required")]

        public string Firstname { get; set; }



        [StringLength(50)]
        [DisplayName("Last Name")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Only characters are allowed.")]
        [Required(ErrorMessage = "Last Name Is Required")]
        public string Lastname { get; set; }
        #endregion


        #region Optional


        [StringLength(1000)]
        [DisplayName("Profile Image")]
        public string? ProfileImage { get; set; }



        [StringLength(100)]
        [DisplayName("Display Name")]
        [RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "Only characters and integers are allowed.")]
        public string? ClientDisplayName { get; set; }

        [DisplayName("About")]
        [StringLength(500)]
        public string? About { get; set; }




        [StringLength(300)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [DisplayName("Email Address")]
        public string? Email { get; set; }


        [StringLength(30)]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit contact number.")]
        public string? Contactnumber { get; set; }



        [DisplayName("Date of birth")]
        public DateTime? Dateofbirth { get; set; }

        [DisplayName("Gender")]
        public string? Gender { get; set; }


        //public UserOtherMetaData UserotherMetaData { get; set; }

        #endregion


    }

    #endregion


    #region Seller

    #region MetaData
    public class AddressViewModel
    {
        #region Address




        [StringLength(1000)]
        [DisplayName("Address")]
        [Required(ErrorMessage = "Address Is Required")]
        public string Address { get; set; }

        public string? Latitude { get; set; }


        public string? Longitude { get; set; }


        public string? Country { get; set; }

        public string? CountryImageURL { get; set; }

        public string? Country2DigitCode { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }


        public string? ZipCode { get; set; }

        public string? TimeZone { get; set; }

        #endregion
    }
    public class ProfileCompletionMetaData
    {
        public SellerProfileSettingsModel? sellersetting { get; set; }
        public SellerProfileSettingsModel? adminsetting { get; set; }
       
    }

    public class BusinessMetaData
    {

        [DisplayName("Date of birth")]
        public DateTime? Dateofbirth { get; set; }


        [StringLength(20)]
        [DisplayName("Gender")]
        [Required(ErrorMessage = "Gender Is Required")]
        public string? Gender { get; set; }




        [DisplayName("Founding Year")]
        [Range(0, 9999, ErrorMessage = "Please enter a whole number with a maximum of 4 digits.")]
        [Required(ErrorMessage = "Founding Year Is Required")]
        public int? FoundingYear { get; set; }



        [DisplayName("NoOfEmployee")]
        [Range(0, 9999, ErrorMessage = "Please enter a whole number with a maximum of 4 digits.")]
        [Required(ErrorMessage = "No Of Employee Year Is Required")]
        public int? NoOfEmployee { get; set; }


    }

    public class ContactMetaData: SellerContactModel
    {
     
    }

    public class AddressMetaData : SellerAddressModel
    {
       
    }


    public class TeamMetaData : SellerTeamMemberModel
    {

    }


    public class IdentityMetaData: SellerIdentityModel
    {

    }

    public class CertificateMetaData : SellerCertificateModel
    {

    }

    
    #endregion


    #region SellerViewModel
    public class SellerViewModel : UserGeneralView
    {
        

        public string? SellerCoverImage { get; set; }

        public string? SellerVideoURl { get; set; }



       

        //public int? Followers { get; set; }
        //public int? Averagerating { get; set; }

        public int ProductTotal { get; set; }
        public decimal? SalesAmountTotal { get; set; }
        public int? SalesQtyTotal { get; set; }

        public string SalesActualCurrency { get; set; }

        public int? CustomerTotal { get; set; }

        public int? PurchaseAmountTotal { get; set; }
        public int PurchaseQtyTotal { get; set; }

        public int? PurchaseConversionCurrency { get; set; }

        public int? BusinessType { get; set; }

        public string? BusinessTypeName { get; set; }

        public string? BusinessName { get; set; }

        public string? BusinessUrlpath { get; set; }

        public string? BusinessDescription { get; set; }


        public BusinessMetaData BusinessMetaData { get; set; }


       public  List<SellerAvailabilityModel> SellerAvailability { get; set; }

        public List<AddressMetaData> SecondaryAddressList { get; set; }

        public List<CertificateMetaData> CertificateList { get; set; }

        public List<TeamMetaData> TeamList { get; set; }

        public List<SellerSocialMediaModel> SocialMediaList { get; set; }
        public string? FollowStatus { get; set; }
    }
    #endregion




    #region ProfileModels


    public class SellerProfileModel : AddressViewModel
    {
        #region Mandatory

        public int Profileid { get; set; }

        public Guid ProfileGuid { get; set; }



        [StringLength(50)]
        [DisplayName("First Name")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Only characters are allowed.")]
        [Required(ErrorMessage = "First Name Is Required")]
        public string Firstname { get; set; }


        [StringLength(50)]
        [DisplayName("Last Name")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Only characters are allowed.")]
        [Required(ErrorMessage = "Last Name Is Required")]
        public string Lastname { get; set; }

        #endregion

        #region Contact


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


      

        #endregion

        #region SellerImage





        [StringLength(1000)]
        [DisplayName("Image")]
        [Required(ErrorMessage = "Image Is Required")]
        public string? SellerImage { get; set; }


        [StringLength(1000)]
        [DisplayName("Cover Image")]
        public string? SellerCoverImage { get; set; }

        [Required(ErrorMessage = "Select Provider")]
        public string Provider { get; set; }

        [StringLength(1000)]
        [DisplayName("Video Url about yourself of business")]
        //[RegularExpression(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$")]
        public string? SellerVideoURl { get; set; }

        #endregion



      

        public string? PrimaryAddressMetaData { get; set; }

        public string? ProfileVerificationMetaData { get; set; }


        #region Business

        [StringLength(200)]
        [DisplayName("Business Name")]
        [Required(ErrorMessage = "Business Name Is Required")]
        public string BusinessName { get; set; }


        //[DisplayName("Business Type")]
        //public int BusinessType { get; set; }




        //[StringLength(200)]
        //[DisplayName("Business Url path")]
        //public string? BusinessUrlpath { get; set; }



        //[StringLength(2000)]
        //[DisplayName("Business Description")]
        //public string? BusinessDescription { get; set; }


        //[DisplayName("Date of birth")]
        //public DateTime? Dateofbirth { get; set; }

        //[DisplayName("Gender")]
        //public string? Gender { get; set; }


        #endregion

    }

    public class SellerBusinessModel: BusinessMetaData
    {
        #region Mandatory

        public int Profileid { get; set; }

        public Guid ProfileGuid { get; set; }




        [DisplayName("Business Type")]
        [Required(ErrorMessage = "Business Type Is Required")]
        public int BusinessType { get; set; }




        [StringLength(500)]
        [DisplayName("Url path")]
        [Required(ErrorMessage = "Url path Is Required")]
        public string BusinessUrlpath { get; set; }



        [StringLength(2000)]
        [DisplayName("Business Description")]
        [Required(ErrorMessage = "Description Is Required")]
        public string BusinessDescription { get; set; }


        public string? BusinessMetaData { get; set; }

        //[DisplayName("Date of birth")]
        //public DateTime? Dateofbirth { get; set; }


        //[StringLength(20)]
        //[DisplayName("Gender")]
        //public string? Gender { get; set; }




        //[DisplayName("Founding Year")]
        //[Range(0, 9999, ErrorMessage = "Please enter a whole number with a maximum of 4 digits.")]
        //public int? FoundingYear { get; set; }



        //[DisplayName("NoOfEmployee")]
        //[Range(0, 9999, ErrorMessage = "Please enter a whole number with a maximum of 4 digits.")]
        //public int? NoOfEmployee { get; set; }




        #endregion

    }
    public class SellerContactModel
    {
        

       

        public int? ContactID { get; set; }

        public Guid? ContactGUID { get; set; }

        [StringLength(100)]
        [DisplayName("Name")]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Only characters are allowed.")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }


        [StringLength(50)]
        [DisplayName("Type")]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Only characters are allowed.")]
        [Required(ErrorMessage = "Type is required.")]
        public string Type { get; set; } ///phone or email

        [StringLength(300)]
        [DisplayName("Contact")]
        
        [Required(ErrorMessage = "Contact is required.")]
        public string Contact { get; set; } ///phone or email

      

        

    }

    public class SellerAddressModel : AddressViewModel
    {

        public int? AddressID { get; set; }

        public Guid? AddressGUID { get; set; }


        [StringLength(50)]
        [DisplayName("Type")]
        [Required(ErrorMessage = "Type Is Required")]
        public string Type { get; set; }
        public bool IsPublish { get; set; }

    }

    public class SellerIdentityModel
    {
        public int? IdentityID { get; set; }

        public Guid? IdentityGUID { get; set; }


        [StringLength(50)]
        [DisplayName("Identity Type")]
        [Required(ErrorMessage = "Identity Type Is Required")]
        public string IdentityType { get; set; }


        [StringLength(1000)]
        [DisplayName("Identity Proof")]
        [Required(ErrorMessage = "Identity Proof Is Required")]
        public string IdentityProof { get; set; }



       public DateTime? InsertDate { get; set; }

        public DateTime? UpdateDate { get; set; }


        [StringLength(50)]
        [DisplayName("Status")]
        [Required(ErrorMessage = "Status")]
        public string Status { get; set; } //Pending, Approved, Rejected

        [StringLength(500)]
        [DisplayName("Remarks")]
       
        public string? Remarks { get; set; }

    }

    public class SellerCertificateModel
    {
        public int? CertificateID { get; set; }

        public Guid? CertificateGUID { get; set; }


        [StringLength(1000)]
        [DisplayName("Certificate Attachment")]
        [Required(ErrorMessage = "Certificate Attachment Is Required")]
        public string CertificateAttachment { get; set; }

        [StringLength(200)]
        [DisplayName("Certificate Name")]
        [Required(ErrorMessage = "Certificate Name Is Required")]
        public string CertificateName { get; set; }


        [StringLength(200)]
        [DisplayName("Institute Name")]
        [Required(ErrorMessage = "Institute Name Is Required")]
        public string InstituteName { get; set; }

        [StringLength(1000)]
        [DisplayName("Course Content")]
        [Required(ErrorMessage = "Course Content Is Required")]
        public string CourseContent { get; set; }

        public DateTime? InsertDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DateTime? DateOfCompletion { get; set; }

    }

    public class SellerSocialMediaModel
    {
        public int SellerSocialMediaID { get; set; }
        public int SocialMediaID { get; set; }  //get from websetting SellerSocialMediaSettings

      
        public string URL { get; set; }

       
        public bool IsCreditUsed { get; set; }

        public int NumberofCredit { get; set; }

        public DateTime CreditUsedDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string? SocialMediaImage { get; set; }

    }

    public class SellerTeamMemberModel
    {
        public int? TeamID { get; set; }

        public Guid? TeamGUID { get; set; }

        [StringLength(100)]
        [DisplayName("Name")]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Only characters are allowed.")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }


        [StringLength(200)]
        [DisplayName("Role")]
        [Required(ErrorMessage = "Role Is Required")]
        public string Role { get; set; }



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



        [DisplayName("Years Of Experience")]


        [Range(0, 9999, ErrorMessage = "Please enter a whole number with a maximum of 4 digits.")]
        [Required(ErrorMessage = "Years Of Experience Is Required")]
        public int YearsOfExperience { get; set; }


        [StringLength(500)]
        [DisplayName("Speciality")]
        [Required(ErrorMessage = "Speciality Is Required")]
        public string Speciality { get; set; }


        [StringLength(1000)]
        [DisplayName("Experience")]
        [Required(ErrorMessage = "Experience Is Required")]
        public string Experience { get; set; }

        [StringLength(1000)]
        [DisplayName("Image")]
        [Required(ErrorMessage = "Image Is Required")]
        public string Image { get; set; }

       


        [DefaultValue(false)]
        public bool IsCreateLogin { get; set; }


        [Column("InsertDate")]
        public DateTime? InsertDate { get; set; }


        [Column("InsertDate")]
        public DateTime? UpdateDate { get; set; }


    }





    public class SellerAvailabilityModel
    {
     

        public string Day { get; set; }

        public bool IsDayEnable { get; set; }
        public bool IsCustomTiming { get; set; }

        public string FromTime { get; set; } // Store the time as a string in AM/PM format

        public string ToTime { get; set; }   // Store the time as a string in AM/PM format


        public DateTime UpdatedDate { get; set; }

    }














    #endregion


    #endregion



    #region Register

    public class RegisterUserNameModel
    {
        [StringLength(300)]
        [DisplayName("User Name")]
        [Required(ErrorMessage = "User Name Is Required")]
        public string UserName { get; set; }



        public string UserType { get; set; }  ///Client or Vendor


        [StringLength(20)]
        [DisplayName("Verification Code")]
        [Required(ErrorMessage = "Verification Code Is Required")]
        public string VerificationCode { get; set; }




        public DateTime? VerificationCodeDate { get; set; }




        public bool? IsVerificationCode { get; set; }
    }


    public class RegisterClientModel
    {



        [StringLength(200)]
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password Is Required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must have at least 8 characters, including one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }




        [StringLength(200)]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Confirm Password Is Required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Confirm Password must have at least 8 characters, including one uppercase letter, one lowercase letter, one digit, and one special character.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }




        [StringLength(100)]
        [DisplayName("First Name")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Only characters are allowed.")]
        [Required(ErrorMessage = "First Name Is Required")]

        public string Firstname { get; set; }


        [StringLength(100)]
        [DisplayName("First Name")]
        [RegularExpression("^[A-Za-z]*$", ErrorMessage = "Only characters are allowed.")]
        [Required(ErrorMessage = "Last Name Is Required")]

        public string Lastname { get; set; }



        ////Seller related Models
        ///

        [DisplayName("Business Type")]
        [Required(ErrorMessage = "Business Type Is Required")]

        public int BusinessType { get; set; }   ///0= Individual 1= Business



        [StringLength(200)]
        [DisplayName("Business Name")]

        public string? BusinessName { get; set; }



        [StringLength(1000)]
        [DisplayName("Address")]
        [Required(ErrorMessage = "Address Is Required")]
        public string Address { get; set; }
    }


    #endregion


    #region Login

    public class LoginModel
    {

        [StringLength(300)]
        [DisplayName("User Name")]
        [Required(ErrorMessage = "User Name Is Required")]
        public string UserName { get; set; }

        [StringLength(200)]
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password Is Required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must have at least 8 characters, including one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }


    }
    #endregion


    #region Password


    public class PasswordModel
    {
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password Is Required")]
        [StringLength(200)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must have at least 8 characters, including one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Confirm Password Is Required")]
        [StringLength(200)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


    }
    #endregion



    #region ForgetPassword

    public class ForgetPasswordUserNameValidation
    {

        [StringLength(300)]
        [DisplayName("User Name")]
        [Required(ErrorMessage = "User Name Is Required")]
        public string UserName { get; set; }




    }


    public class ForgetPasswordValidationCode
    {
        [StringLength(20)]
        [DisplayName("Verification Code")]
        [Required(ErrorMessage = "Verification Code Is Required")]
        public string VerificationCode { get; set; }


      
    }

    #endregion

    #region UserLock
    public class UserLockMetaData
    {
        [StringLength(1000)]
        [DisplayName("Remarks")]
        [Required(ErrorMessage = "Remarks Is Required")]
        public string Remarks { get; set; }
        public bool IsLock { get; set; }


       
        [DisplayName("UnlockDate")]
        [Required(ErrorMessage = "UnlockDate Is Required")]
        public DateTime UnlockDate { get; set; }
    }
    #endregion


    #region OtherMetaData
    public class UserOtherMetaData
    {
        
        public string IP { get; set; }
        public string Country { get; set; }
        public string CountryFlag { get; set; }

        public string City { get; set; }
        public string TimeZone { get; set; }

        public int Following { get; set; }
        public int Followers { get; set; }

        public int TotalReviewsAsBuyer { get; set; }  //how much received

        public int TotalReviewsAsSeller { get; set; }//how much received

        public decimal BuyerRating { get; set; }

        public decimal SellerRating { get; set; }

        public DateTime LastSeen { get; set; }


        public string SellerLevelName { get; set; } = "Aspiring Seller";

        public string BuyerLevelName { get; set; } = "Explorer";

        public List<UserAgentMetaData> UserAgentMetaData { get; set; }

        //depreciated logic
        //public WalletAvailable WalletAvailable { get; set; }


        // Constructor to set the default value for LastSeen
        public UserOtherMetaData()
        {
            // Set the default value to the current date and time in UTC
            LastSeen = DateTime.Now;
            IP = GlobalHelper.IPAddress();
            
        }

        public UserInActive UserInactiveMetaData { get; set; }

        public bool IsVerifiedSeller { get; set; } = false;
        public DateTime? IsVerifiedSellerDate { get; set; }

        public bool IsVerifiedBuyer { get; set; } = false;
        public DateTime? IsVerifiedBuyerDate { get; set; }

    }
    #endregion

    #region WalleMetaData

    //for wallet creation
    public class WalletItemMetaDataViewModel : OrderPaymentModel
    {
        public int OrderID { get; set; }
        public string InvoiceNumber { get; set; }

        public DateTime WalletDateTime { get; set; }

       /* public decimal Amount { get; set; }*/  //could be positive or negative number

        //public string Currency { get; set; }

        public string Description { get; set; }

    }
    //this is use in userothermetadata to store available balance to avoid calculation all the time.
    public class WalletAvailable
    {
        public decimal Amount { get; set; }  //could be positive or negative number

        public string Currency { get; set; }
    }

    public class CouponMetaData
    {
        public string couponamount { get; set; }
    }


    //for order table to create wallet use
    public class WalletUsedItemMetaData
    {
        public decimal WalletAmountUsed { get; set; }  //could be positive or negative number

        public string WalletCurrency { get; set; }
    }


    public class UserAgentMetaData
    {
       public string Browser { get; set; }
        public string BrowserVersion { get; set; }

        public string OperatingSystem { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string IP { get; set; }

        public UserAgentMetaData()
        {

          
            IP = GlobalHelper.IPAddress();
            DateTime = DateTime.Now;

        }

        public string DeviceType { get; set; }

        public DateTime DateTime { get; set; }

    }

    public class UserDeviceViewModel
    {
       public UserAgentMetaData UserAgent { get; set; }

        public List<UserAgentMetaData> ListUserDevice { get; set; }

    }
    public class Browser
    {
        public string userAgent { get; set; }
        public Regex OS { get; set; }
        public Regex device { get; set; }
    }

    #endregion

    #region UserInactive

    public class UserInActive
    {
        public string OffLineType { get; set; }

      
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool Availableforchat { get; set; }

        public bool Availableforsearch { get; set; }

        public DateTime LastUpdated { get; set; }

        public UserInActive()
        {

            OffLineType = "online";
        }



    }
    #endregion


    #region AvatarModel
    public class AvatarList
    {
        public string ImagePath { get; set; }
      
    }

    public class GenderModel
    {
        public List<AvatarList> malelist { get; set; }
        public List<AvatarList> femalelist { get; set; }
    }
    #endregion
}
