using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Amazon.S3.Model;

namespace AMMasterProject.ViewModel
{
    public class AppSettingModel
    {
    }


    public class MySettingsModel
    {
        public string DbConnection { get; set; }
        public string Email { get; set; }
        public string SMTPPort { get; set; }
    }


    #region Register

    public class RegisterSettingsModel
    {
        public bool IsEmail { get; set; }
        public bool IsPhone { get; set; }

        public bool IsRegisterVerification { get; set; }

        public bool IsGoogleLogin { get; set; }

        [DisplayName("Client Id")]
       
        public string? ClientId { get; set; }


        [DisplayName("Client Secret")]
       

        public string? ClientSecret { get; set; }

        public bool IsFacebookLogin { get; set; }

        [DisplayName("App Id")]
        
        public string? AppId { get; set; }


        [DisplayName("App Secret")]
      
        public string? AppSecret { get; set; }

        public string RegisterImage { get; set; }

        public string LoginImage { get; set; }
        public string VerificationPlatform { get; set; }  ///Firebase Twilio

       

    }


    public class RegisterVerificationSettingsModel
    {
        public bool RegisterVerification { get; set; }


    }
    #endregion




    #region EmailSetting
    public class EmailSettingsModel
    {
        [StringLength(300)]
        [DisplayName("From Email")]
        [Required(ErrorMessage = "Email From Is Required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string FromEmail { get; set; }


        [StringLength(300)]
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }



        [DisplayName("Port")]
        [Required(ErrorMessage = "Port Is Required")]
        public int Port { get; set; }


        [StringLength(200)]
        [DisplayName("SMTP")]
        [Required(ErrorMessage = "SMTP Is Required")]
        public string SMTP { get; set; }



        [DisplayName("Enable SSL")]
        [Required(ErrorMessage = "Enable SSL Is Required")]
        public bool EnableSSL { get; set; }


        [StringLength(500)]
        [DisplayName("SMBCCTP")]
        [Required(ErrorMessage = "BCC Is Required")]
        public string BCC { get; set; }

    }
    #endregion



    #region ProfileBoost
    public class ProfileBoostSettingsModel
    {



        [DisplayName("No. of Days")]
        [Required(ErrorMessage = "No. of Days Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "No. of Days should be a whole number.")]
        public int NoofDays { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount should be a valid decimal number.")]
        public decimal Amount { get; set; }

        [DisplayName("Credit")]
        [Required(ErrorMessage = "Credit Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Credit should be a whole number.")]
        public int Credit { get; set; }


        [DisplayName("Use Credit Or Amount")]
        [Required(ErrorMessage = "Use Credit Or Amount Is Required")]
        public string DeductionType { get; set; }




        [DisplayName("Custom Amount")]
        [Required(ErrorMessage = "Custom Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Custom Amount should be a valid decimal number.")]
        public decimal CustomAmount { get; set; }

        [DisplayName("Custom Credit")]
        [Required(ErrorMessage = "Custom Credit Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Custom Credit should be a whole number.")]
        public int CustomCredit { get; set; }

    }
    #endregion


    #region ServiceBoost
    public class ServiceBoostSettingsModel
    {



        [DisplayName("No. of Days")]
        [Required(ErrorMessage = "No. of Days Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "No. of Days should be a whole number.")]
        public int NoofDays { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount should be a valid decimal number.")]
        public decimal Amount { get; set; }

        [DisplayName("Credit")]
        [Required(ErrorMessage = "Credit Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Credit should be a whole number.")]
        public int Credit { get; set; }


        [DisplayName("Use Credit Or Amount")]
        [Required(ErrorMessage = "Use Credit Or Amount Is Required")]
        public string DeductionType { get; set; }




        [DisplayName("Custom Amount")]
        [Required(ErrorMessage = "Custom Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Custom Amount should be a valid decimal number.")]
        public decimal CustomAmount { get; set; }

        [DisplayName("Custom Credit")]
        [Required(ErrorMessage = "Custom Credit Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Custom Credit should be a whole number.")]
        public int CustomCredit { get; set; }

    }
    #endregion


    #region ListingBoost

    public class ListingBoostSettingsModel
    {



        [DisplayName("No. of Days")]
        [Required(ErrorMessage = "No. of Days Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "No. of Days should be a whole number.")]
        public int NoofDays { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount should be a valid decimal number.")]
        public decimal Amount { get; set; }

        [DisplayName("Credit")]
        [Required(ErrorMessage = "Credit Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Credit should be a whole number.")]
        public int Credit { get; set; }


        [DisplayName("Use Credit Or Amount")]
        [Required(ErrorMessage = "Use Credit Or Amount Is Required")]
        public string DeductionType { get; set; }




        [DisplayName("Custom Amount")]
        [Required(ErrorMessage = "Custom Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Custom Amount should be a valid decimal number.")]
        public decimal CustomAmount { get; set; }

        [DisplayName("Custom Credit")]
        [Required(ErrorMessage = "Custom Credit Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Custom Credit should be a whole number.")]
        public int CustomCredit { get; set; }

    }



    public class ListingBoostViewModel:ListingBoostSettingsModel
    {

        public decimal ConversionAutoAmount { get; set; }
        public decimal ConversionCustomizedAmount { get; set; }

        public string ConversionCurrency { get; set; }

      
 

    }
    #endregion


    #region AdvertisementBoost

    public class AdvertisementBoostSettingsModel
    {



        [DisplayName("No. of Days")]
        [Required(ErrorMessage = "No. of Days Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "No. of Days should be a whole number.")]
        public int NoofDays { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount should be a valid decimal number.")]
        public decimal Amount { get; set; }

        [DisplayName("Credit")]
        [Required(ErrorMessage = "Credit Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Credit should be a whole number.")]
        public int Credit { get; set; }


        [DisplayName("Use Credit Or Amount")]
        [Required(ErrorMessage = "Use Credit Or Amount Is Required")]
        public string DeductionType { get; set; }




        [DisplayName("Custom Amount")]
        [Required(ErrorMessage = "Custom Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Custom Amount should be a valid decimal number.")]
        public decimal CustomAmount { get; set; }

        [DisplayName("Custom Credit")]
        [Required(ErrorMessage = "Custom Credit Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Custom Credit should be a whole number.")]
        public int CustomCredit { get; set; }

    }
    #endregion


    #region CommissionSystem

    public class CommissionSettingsModel
    {



        [DisplayName("Commission Type")]
        [Required(ErrorMessage = "Commission Type Is Required")]

        public string CommissionType { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount should be a valid decimal number.")]
        public decimal Amount { get; set; }

        [DisplayName("Label Name")]
        [Required(ErrorMessage = "Label Is Required")]
        public string Label { get; set; }


    }
    #endregion

    #region CommissionPlatformSystem

    public class CommissionPlatformSettingsModel
    {



        [DisplayName("Commission Type")]
        [Required(ErrorMessage = "Commission Type Is Required")]

        public string CommissionType { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount should be a valid decimal number.")]
        public decimal Amount { get; set; }

        [DisplayName("Label Name")]
        [Required(ErrorMessage = "Label Is Required")]
        public string Label { get; set; }

    }
    #endregion

    #region CommissionBuyerSystem

    public class CommissionBuyerSettingsModel
    {



        [DisplayName("Commission Type")]
        [Required(ErrorMessage = "Commission Type Is Required")]

        public string CommissionType { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount should be a valid decimal number.")]
        public decimal Amount { get; set; }

        [DisplayName("Label Name")]
        [Required(ErrorMessage = "Label Is Required")]
        public string Label { get; set; }

    }
    #endregion


    #region Commission&TaxForBuyerList
    public class CommissionTaxBuyerSettingsModel
    {

        public string ID { get; set; }


        [DisplayName("Commission Type")]
        [Required(ErrorMessage = "Commission Type Is Required")]

        public string CommissionType { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount should be a valid decimal number.")]
        public decimal Amount { get; set; }

        [DisplayName("Label Name")]
        [Required(ErrorMessage = "Label Is Required")]
        public string Label { get; set; }


        public DateTime UpdatedDate { get; set; }


    }
    #endregion

    #region Commission&TaxForSellerList
    public class CommissionTaxSellerSettingsModel
    {


        public string ID { get; set; }


        [DisplayName("Commission Type")]
        [Required(ErrorMessage = "Commission Type Is Required")]

        public string CommissionType { get; set; }

        [DisplayName("Amount")]
        [Required(ErrorMessage = "Amount Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Amount should be a valid decimal number.")]
        public decimal Amount { get; set; }

        [DisplayName("Label Name")]
        [Required(ErrorMessage = "Label Is Required")]
        public string Label { get; set; }


        public DateTime UpdatedDate { get; set; }



    }
    #endregion

    #region LoyaltyPoints
    public class LoyaltyPointSettingsModel
    {

        [DisplayName("Points Coversion Rate")]
        [Required(ErrorMessage = "Points Conversion Rate Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Points Conversion Rate should be a valid decimal number.")]
        public decimal PointsConversionRate { get; set; }

        [DisplayName("Minimum Points Redeem")]
        [Required(ErrorMessage = "Minimum Points Redeem Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Minimum Points Redeem should be a valid decimal number.")]
        public decimal MinPointsRedeem { get; set; }


        [DisplayName("Points Expiry In Days")]
        [Required(ErrorMessage = "Points Expiry In Days Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Points Expiry In Days should be a whole number.")]
        public int PointsExpiry { get; set; }


        public bool IsEnable { get; set; }

    }
    #endregion

    #region ReferalPoints
    public class AffiliateSettingsModel
    {

        [DisplayName("Affiliate Percentage")]
        [Required(ErrorMessage = "Affiliate Percentage Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Affiliate Percentage should be a valid decimal number.")]
        public decimal AffiliatePercentage { get; set; }

        [DisplayName("Referal Percentage")]
        [Required(ErrorMessage = "Referal Percentage Is Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Referal Percentage should be a valid decimal number.")]
        public decimal ReferalPercentage { get; set; }




        public bool IsEnable { get; set; }

    }
    #endregion


   


    #region CreditSystem

    public class CreditSystemSettingsModel
    {

        [DisplayName("Free Credit For Seller")]
        [Required(ErrorMessage = "Free Credit For Seller")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Free Credit For Seller should be a whole number.")]
        public int FreeCreditForSeller { get; set; }

        [DisplayName("Free Credit For Buyer")]
        [Required(ErrorMessage = "Free Credit For Buyer Is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Free Credit For Buyer should be a whole number.")]
        public int FreeCreditForBuyer { get; set; }


        [DisplayName("Credit Deduction On Address")]
        [Required(ErrorMessage = "Credit Deduction On Address")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Should be a whole number.")]
        public int DeductionOnAddressView { get; set; }

        [DisplayName("Credit Deduction On Email")]
        [Required(ErrorMessage = "Credit Deduction On Email")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Should be a whole number.")]
        public int DeductionOnEmailView { get; set; }

        [DisplayName("Credit Deduction On Contact")]
        [Required(ErrorMessage = "Credit Deduction On Contact")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Should be a whole number.")]
        public int DeductionOnContactView { get; set; }



        public bool IsEnable { get; set; }
    }

    #endregion

    #region SubscriptionSystem

    public class SubscriptionSystemSettingsModel
    {

        public bool IsEnable { get; set; }
    }

    #endregion


   

   


    #region LanguageSetup

    public class LanguageSetupSettingsModel
    {



        public bool IsMultilingual { get; set; }

        public string DefaultLanguage { get; set; }
       
    }

    #endregion


    #region DefaultCurrency

    public class DefaultCurrencySettingsModel
    {

        [DisplayName("Default Currency")]
        [Required(ErrorMessage = "Default Currency")]

        public string BaseCurrency { get; set; }


        public bool IsMultiCurrency { get; set; }


       
        

    }

    #endregion

    #region CurrencyAPI



    #region APILayercom
    public class CurrencyAPILayerSettingsModel
    {
        [Required(ErrorMessage = "APIKey")]
        public string APIKey { get; set; }

        public bool IsPublish { get; set; }
    }
    #endregion


    #endregion

    #region SMSAPI

    #region Twilio

    public class TwilioSettingsModel
    {

        [DisplayName("accountSid")]
        [Required(ErrorMessage = "accountSid")]

        public string AccountSid { get; set; }



        [DisplayName("Auth Token")]
        [Required(ErrorMessage = "Auth Token")]

        public string AuthToken { get; set; }


        [DisplayName("Phone")]
        [Required(ErrorMessage = "Phone")]

        public string Phone { get; set; }

    }

    #endregion

    #region FireBase

    public class FireBaseSettingsModel
    {

        [DisplayName("apiKey")]
        [Required(ErrorMessage = "apiKey")]

        public string apiKey { get; set; }

        [DisplayName("authDomain")]
        [Required(ErrorMessage = "authDomain")]

        public string authDomain { get; set; }


        [DisplayName("projectId")]
        [Required(ErrorMessage = "projectId")]

        public string projectId { get; set; }


        [DisplayName("storageBucket")]
        [Required(ErrorMessage = "storageBucket")]

        public string storageBucket { get; set; }


        [DisplayName("messagingSenderId")]
        [Required(ErrorMessage = "messagingSenderId")]

        public string messagingSenderId { get; set; }


        [DisplayName("appId")]
        [Required(ErrorMessage = "appId")]

        public string appId { get; set; }

        [DisplayName("measurementId")]
        [Required(ErrorMessage = "measurementId")]

        public string measurementId { get; set; }




    }

    #endregion

    #endregion

    #region DateFormats

    public class DateFormatSettingsModel
    {

        [DisplayName("Date Format")]
        [Required(ErrorMessage = "Date Format")]

        public string DateFormat { get; set; }


        [DisplayName("Time Format")]
        [Required(ErrorMessage = "Time Format")]

        public string TimeFormat { get; set; }



    }


    #endregion


    #region AdminSettings
    #region SellerProfile
    public class  SellerProfileSettingsModel
    {
        public bool Profile { get; set; }
        public bool BusinessInfo { get; set; }
      

        public bool SecondaryContactDetails { get; set; }

        public bool SecondaryAddress { get; set; }

        public bool IdentityProof { get; set; }

        public bool Certificate { get; set; }

        public bool IsVideo { get; set; }
        public bool SocialMediaLink { get; set; }

        public bool TeamMemmber { get; set; }


        public int TotalTabsRequired
        {
            get
            {
                // Calculate the total based on the true values in other properties
                return new bool[]
                {
              Profile,  BusinessInfo, SecondaryContactDetails, SecondaryAddress,
                IdentityProof, Certificate, IsVideo, SocialMediaLink, TeamMemmber
                }.Count(value => value);
            }
        }
    }


    #endregion

    #region LabelSettings

    public class LabelSettingsModel
    {

        [DisplayName("Seller")]
        [Required(ErrorMessage = "Seller")]
        public string Seller { get; set; }


        [DisplayName("Buyer")]
        [Required(ErrorMessage = "Buyer")]
        public string Buyer { get; set; }


        [DisplayName("Listing")]
        [Required(ErrorMessage = "Listing")]
        public string Listing { get; set; }


        [DisplayName("Individual")]
        [Required(ErrorMessage = "Individual")]
        public string Individual { get; set; }


        [DisplayName("Company")]
        [Required(ErrorMessage = "Company")]
        public string Company { get; set; }



        [DisplayName("Address")]
        [Required(ErrorMessage = "Address")]
        public string Address { get; set; }


        [DisplayName("Become a Seller")]
        [Required(ErrorMessage = "Become a Seller")]
        public string BecomeaSeller { get; set; }


        [DisplayName("Identity Proof")]
        [Required(ErrorMessage = "Identity Proof")]
        public string IdentityProof { get; set; }


        [DisplayName("Certificate")]
        [Required(ErrorMessage = "Certificate")]
        public string Certificate { get; set; }


        [DisplayName("PostRequest")]
        [Required(ErrorMessage = "Post Request")]
        public string PostRequest { get; set; }

        [DisplayName("SellerLogo")]
        [Required(ErrorMessage = "SellerLogo")]
        public string SellerLogo { get; set; }

        [DisplayName("SellerBusiness")]
        [Required(ErrorMessage = "SellerBusiness")]
        public string SellerBusiness { get; set; }



        [DisplayName("HeaderSearchPlaceHolder")]
        [Required(ErrorMessage = "HeaderSearchPlaceHolder")]
        public string HeaderSearchPlaceHolder { get; set; }

        [DisplayName("ViewProfile")]
        [Required(ErrorMessage = "ViewProfile")]
        public string ViewProfile { get; set; }



        [DisplayName("ShippingPhone")]
        [Required(ErrorMessage = "ShippingPhone")]
        public string ShippingPhone { get; set; }



        [DisplayName("ShippingState")]
        [Required(ErrorMessage = "Shipping State")]
        public string ShippingState { get; set; }

        [DisplayName("ShippingCity")]
        [Required(ErrorMessage = "Shipping City")]
        public string ShippingCity { get; set; }


        [DisplayName("ShippingStreet")]
        [Required(ErrorMessage = "Shipping Street")]
        public string ShippingStreet { get; set; }


        [DisplayName("Shippinglandmark")]
        [Required(ErrorMessage = "Shipping Landmark")]
        public string ShippingLandmark { get; set; }


        [DisplayName("Shippingzipcode")]
        [Required(ErrorMessage = "Shipping Zip Code")]
        public string ShippingZipcode { get; set; }

    }

    #endregion

    #region MasterLanguageSetting
    public class MasterLangaugeSettingModel
    {
     
            public string Key { get; set; }
            public Dictionary<string, string> Translations { get; set; }
        
    }
    #endregion


    #region Cookie
    public class CookieSettingsModel
    {

        [DisplayName("Seller")]
        [Required(ErrorMessage = "Seller")]
        public string CookieText { get; set; }


        [DisplayName("URL")]
        [Required(ErrorMessage = "URL")]
        public string URL { get; set; }

        public bool IsCookie { get; set; }
    }
    #endregion

    #endregion


    #region FileStorage

    #region FileStorageChannle
    public class FileStorageSettingsModel
    {

        [DisplayName("File Storage")]
        [Required(ErrorMessage = "File Storage")]

        public string FileStorageType { get; set; }  ///aws, azure, local




    }
    #endregion

    #region AWS

    public class AWSSettingsModel
    {

        [DisplayName("Access Key")]
        [Required(ErrorMessage = "Access Key")]

        public string AccessKey { get; set; }



        [DisplayName("Secret Key")]
        [Required(ErrorMessage = "Secret Key")]

        public string SecretKey { get; set; }


        [DisplayName("URLPath")]
        [Required(ErrorMessage = "URLPath")]

        public string URLPath { get; set; }

        [DisplayName("Bucket")]
        [Required(ErrorMessage = "Bucket")]

        public string Bucket { get; set; }


    }

    #endregion

    #region AzureBlog

    public class AzureSettingsModel
    {

        [DisplayName("Connection String")]
        [Required(ErrorMessage = "Connection String")]

        public string ConnectionString { get; set; }



        [DisplayName("Container Name")]
        [Required(ErrorMessage = "Container Name")]

        public string ContainerName { get; set; }


        [DisplayName("URLPath")]
        [Required(ErrorMessage = "URLPath")]

        public string URLPath { get; set; }



    }

    #endregion

    #region FireBaseStorageModel

    public class FireBaseStorageSettingsModel
    {

        [DisplayName("Service Acount ")]
        [Required(ErrorMessage = "Service Acount")]

        public string ServiceAcountMetaData { get; set; }




    }

    #endregion

    #endregion

    #region UserDefineScripts
    public class ScriptCustom
    {
        public string Script { get; set; }
        public string ScriptName { get; set; }
    }
    #endregion

    #region CountryViewModel
    public class CountryViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }

        public string CountryName { get; set; }
        public string CountryAlpha2 { get; set; }

        public string CountryMobileCode { get; set; }

        public string CurrencyCode { get; set; }

        public string CurrencySymbol { get; set; }

        public string CurrencyName { get; set; }

        public bool IsCountryPublish { get; set; }

        public bool IsCurrencyPublish { get; set; }

        public decimal ConversionRate { get; set; }

        public DateTime? ConversionDateupdate { get; set; }

    }
    #endregion

    #region LanguageViewModel
    public class LanguageViewModel
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsRTL { get; set; }
        public bool IsPublish { get; set; }
    }
    #endregion

   
    #region LocationModelAPI
    public class LocationModel
    {
        public string geoplugin_countryCode { get; set; }
        public string geoplugin_currencyCode { get; set; }
        public string geoplugin_countryName { get; set; }
        //public string Region_Code { get; set; }
        //public string Region_Name { get; set; }
        //public string City { get; set; }
        //public string Zip_Code { get; set; }
        //public string Time_Zone { get; set; }
        //public string Latitude { get; set; }
        //public string Longitude { get; set; }
        //public string Metro_Code { get; set; }
    }
    #endregion

    #region RegionalSettingViewModel
    public class RegionalSettingViewModel
    {
        public bool IsMultilingual { get; set;}
        public bool IsMultiCurrency { get; set; }

        public bool IsCountrySelectionEnabled { get; set; }


    }
    #endregion


    #region ScriptManager

    public class ScriptManagerSettingsViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string Script { get; set; }

        public bool IsPublish { get; set; }
    }
    #endregion


    #region HeroBanner

    public class HeroBannerSettingsViewModel
    {
        public string ID { get; set; }
      

        public string Banner { get; set; }

        public bool IsPublish { get; set; }
    }
    #endregion


    #region SocialMediaSetupModel
    public class SocialMediaSettingViewModel
    {

        public int ID { get; set; }  //get from websetting SellerSocialMediaSettings

     
        public string Name { get; set; }

        public string Icon { get; set; }


        public bool IsPublish { get; set; }

        public int Credit { get; set; }

        public bool IsCreditExpiry { get; set; }

        public int NumberOfExpiryDays { get; set; }

    }
    #endregion


    #region GlobalAppsetting

    public class GlobalAppSettingsModel
    {
     
        public bool IsCountrySelectionEnabled { get; set; }
    }
    #endregion


    #region GoogleCaptchV2Setting

    public class GoogleRecaptchaSettingModel
    {

        public string ReCaptchaKey { get; set; }
        public string ReCaptchaSecret { get; set; }


    }
    #endregion


    #region License
    public class LicenseAppSettingsModel
    {
        public string LicenseKey { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string LicenseKeyForBrandRemoval { get; set; }
        public DateTime? BrandRemovalActivationDate { get; set; }
        public DateTime? BrandRemovalExpiryDate { get; set; }
    }
    #endregion


    #region GoogleMapAPI
    public class GoogleMapsApiSettingsModel
    {
        [Required(ErrorMessage = "APIKey")]
        public string APIKey { get; set; }

        public bool IsPublish { get; set; }
    }
    #endregion


    #region BarCode
    public class BarcodeViewModel
    {
        public string Data { get; set; }
        // Additional properties if needed
    }
    #endregion


    #region ShippingFormSetting

    public class ShippingFormSettingsModel
    {
        public bool IsZipCodeHide { get; set; }
        public bool IsStreetHide { get; set; }

     
    }


  
    #endregion
}

