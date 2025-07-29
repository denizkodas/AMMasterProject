using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AMMasterProject.ViewModel
{
    public class PaymentSettingViewModel
    {

    }

    #region Paypal
    public class PaypalSettingsModel
    {

        [StringLength(100)]
        [DisplayName("ClientId")]
        [Required(ErrorMessage = "ClientId Is Required")]
        public string ClientId { get; set; }

        [StringLength(100)]
        [DisplayName("Client Secret Key")]
        [Required(ErrorMessage = "Client Secret Key Is Required")]
        public string ClientSecretKey { get; set; }


        [StringLength(30)]
        [DisplayName("Environment")]
        [Required(ErrorMessage = "Environment Is Required")]
        public string Environment { get; set; }


        public bool IsEnable { get; set; }

    }
    #endregion

    #region Stripe
    public class StripeSettingsModel
    {

        [StringLength(500)]
        [DisplayName("API Key")]
        [Required(ErrorMessage = "API Key Is Required")]
        public string APIKey { get; set; }

        public bool IsEnable { get; set; }


    }
    #endregion


    #region RazorPay
    public class RazorPaySettingsModel
    {


        [StringLength(100)]
        [DisplayName("Keyid")]
        [Required(ErrorMessage = "Key ID Is Required")]
        public string KeyID { get; set; }



        [StringLength(100)]
        [DisplayName("Secret")]
        [Required(ErrorMessage = "Secret Is Required")]
        public string SecretID { get; set; }

        [StringLength(100)]
        [DisplayName("Secret")]
        [Required(ErrorMessage = "Store Name Is Required")]
        public string StoreName { get; set; }



        [StringLength(30)]
        [DisplayName("Environment")]
        [Required(ErrorMessage = "Environment Is Required")]
        public string Environment { get; set; }

        public bool IsEnable { get; set; }

    }
    #endregion

    #region PayFast
    public class PayFastSettingsModel
    {

        [StringLength(100)]
        [DisplayName("Merchant ID")]
        [Required(ErrorMessage = "Merchant ID Is Required")]
        public string MerchantID { get; set; }

        [StringLength(100)]
        [DisplayName("Merchant Key")]
        [Required(ErrorMessage = "Merchant Key Is Required")]
        public string MerchantKey { get; set; }

        public bool IsEnable { get; set; }

    }
    #endregion

    #region PayStack
    public class PayStackSettingsModel
    {

        [StringLength(100)]
        [DisplayName("PK Live")]
        [Required(ErrorMessage = "PK Live Is Required")]
        public string PKLive { get; set; }

        public bool IsEnable { get; set; }


    }
    #endregion




    #region DPO
    public class DPOSettingsModel
    {

        [StringLength(100)]
        [DisplayName("Company Token")]
        [Required(ErrorMessage = "Company Token Is Required")]
        public string CompanyToken { get; set; }

        [StringLength(100)]
        [DisplayName("Service Type")]
        [Required(ErrorMessage = "Service Type Is Required")]
        public string ServiceType { get; set; }

        public bool IsEnable { get; set; }

    }
    #endregion

    #region SSLCommerz
    public class SSLCommerzSettingsModel
    {

        [StringLength(100)]
        [DisplayName("Store ID")]
        [Required(ErrorMessage = "Store ID Is Required")]
        public string StoreID { get; set; }

        [StringLength(100)]
        [DisplayName("Store Password")]
        [Required(ErrorMessage = "Store Password Is Required")]
        public string StorePassword { get; set; }

        [StringLength(30)]
        [DisplayName("Environment")]
        [Required(ErrorMessage = "Environment Is Required")]
        public string Environment { get; set; }


        public bool IsEnable { get; set; }

    }
    #endregion

    #region MTN
    public class MTNSettingsModel
    {
        [StringLength(300)]
        [DisplayName("BaseKey")]
        [Required(ErrorMessage = "BaseKey Is Required")]
        public string BaseKey { get; set; }

        [StringLength(300)]
        [DisplayName("APIURL")]
        [Required(ErrorMessage = "API URL Is Required")]
        public string APIURL { get; set; }


        [StringLength(100)]
        [DisplayName("Ocp-Apim-Subscription-Key")]
        [Required(ErrorMessage = "Ocp-Apim-Subscription-Key Is Required")]
        public string OcpApimSubscriptionKey { get; set; }


        [StringLength(30)]
        [DisplayName("Environment")]
        [Required(ErrorMessage = "Environment Is Required")]
        public string Environment { get; set; }


        public bool IsEnable { get; set; }

    }
    #endregion

    #region CashOnDelivery
    public class CODSettingsModel
    {

        [StringLength(500)]
        [DisplayName("Message")]
        [Required(ErrorMessage = "Message Is Required")]
        public string Message { get; set; }


        public bool IsEnable { get; set; }

    }
    #endregion

    #region BankTransfer
    public class BankTransferSettingsModel
    {



        [StringLength(1000)]
        [DisplayName("Account Details")]
        [Required(ErrorMessage = "Account Details Is Required")]
        public string AccountDetails { get; set; }

        public bool IsEnable { get; set; }

    }
    #endregion


    #region OrderPaymentViewModel

    public class OrderPaymentModel
    {
        public decimal ConversionRate { get; set; }
        public decimal ConversionAmount { get; set; }

        public string ConversionCurrency { get; set; }


        public decimal ActualAmount { get; set; }

        public string ActualCurrency { get; set; }
    }

    #endregion




    #region PaymentGatewaModels


    #region RazorPay
    public class RazorPayOrderModel
    {
        public decimal OrderAmount { get; set; }
        public decimal OrderAmountInSubUnits
        {
            get
            {
                return OrderAmount * 100;
            }
        }
        public string Currency { get; set; }
        public int Payment_Capture { get; set; }
        public Dictionary<string, string> Notes { get; set; }
    }

    public class RazorPayOptionsModel
    {
        public string Key { get; set; }
        public decimal AmountInSubUnits { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageLogUrl { get; set; }
        public string OrderId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileContact { get; set; }
        public string ProfileEmail { get; set; }

        public string SuccessURL { get; set; }

        public string CancelURL { get; set; }
        public Dictionary<string, string> Notes { get; set; }
    }
    #endregion


    #region SSLCommerz
    public class Gw
    {
        public string visa { get; set; }
        public string master { get; set; }
        public string amex { get; set; }
        public string othercards { get; set; }
        public string internetbanking { get; set; }
        public string mobilebanking { get; set; }
    }

    public class Desc
    {
        public string name { get; set; }
        public string type { get; set; }
        public string logo { get; set; }
        public string gw { get; set; }
    }

    public class SSLCommerzInitResponse
    {
        public string status { get; set; }
        public string failedreason { get; set; }
        public string sessionkey { get; set; }
        public Gw gw { get; set; }
        public string redirectGatewayURL { get; set; }
        public string redirectGatewayURLFailed { get; set; }
        public string GatewayPageURL { get; set; }
        public string storeBanner { get; set; }
        public string storeLogo { get; set; }
        public List<Desc> desc { get; set; }
        public string is_direct_pay_enable { get; set; }
    }
    public class SSLCommerzValidatorResponse
    {
        public string status { get; set; }
        public string tran_date { get; set; }
        public string tran_id { get; set; }
        public string val_id { get; set; }
        public string amount { get; set; }
        public string store_amount { get; set; }
        public string currency { get; set; }
        public string bank_tran_id { get; set; }
        public string card_type { get; set; }
        public string card_no { get; set; }
        public string card_issuer { get; set; }
        public string card_brand { get; set; }
        public string card_issuer_country { get; set; }
        public string card_issuer_country_code { get; set; }
        public string currency_type { get; set; }
        public string currency_amount { get; set; }
        public string currency_rate { get; set; }
        public string base_fair { get; set; }
        public string value_a { get; set; }
        public string value_b { get; set; }
        public string value_c { get; set; }
        public string value_d { get; set; }
        public string emi_instalment { get; set; }
        public string emi_amount { get; set; }
        public string emi_description { get; set; }
        public string emi_issuer { get; set; }
        public string account_details { get; set; }
        public string risk_title { get; set; }
        public string risk_level { get; set; }
        public string APIConnect { get; set; }
        public string validated_on { get; set; }
        public string gw_version { get; set; }
        public string token_key { get; set; }
        public string shipping_method { get; set; }
        public string num_of_item { get; set; }
        public string product_name { get; set; }
        public string product_profile { get; set; }
        public string product_category { get; set; }
    }


    public class SSLCommerzTransactionValidationResponse
    {
        public string APIConnect { get; set; }
        public int no_of_trans_found { get; set; }
        public List<SSLCommerzTransactionElement> element { get; set; }
    }

    public class SSLCommerzTransactionElement
    {
        public string val_id { get; set; }
        public string status { get; set; }
        public string validated_on { get; set; }
        public string currency_type { get; set; }
        public decimal currency_amount { get; set; }
        public decimal currency_rate { get; set; }
        public decimal base_fair { get; set; }
        public string value_a { get; set; }
        public string value_b { get; set; }
        public string value_c { get; set; }
        public string value_d { get; set; }
        public decimal discount_percentage { get; set; }
        public string discount_remarks { get; set; }
        public decimal discount_amount { get; set; }
        public DateTime tran_date { get; set; }
        public string tran_id { get; set; }
        public decimal amount { get; set; }
        public decimal store_amount { get; set; }
        public string bank_tran_id { get; set; }
        public string card_type { get; set; }
        public string risk_title { get; set; }
        public string risk_level { get; set; }
        public string currency { get; set; }
        public string bank_gw { get; set; }
        public string card_no { get; set; }
        public string card_issuer { get; set; }
        public string card_brand { get; set; }
        public string card_issuer_country { get; set; }
        public string card_issuer_country_code { get; set; }
        public string gw_version { get; set; }
        public int emi_instalment { get; set; }
        public string emi_amount { get; set; }
        public string emi_description { get; set; }
        public string emi_issuer { get; set; }
        public string error { get; set; }
    }
    #endregion

    #endregion



    #region MTNBuyerModel

    public class MTNTransactionValidationResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public string FinancialTransactionId { get; set; }
        public string ExternalId { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string PayerPartyIdType { get; set; }
        public string PayerPartyId { get; set; }
        public string PayerMessage { get; set; }
        public string PayeeNote { get; set; }
        public string Status { get; set; }
    }
    public class MTNBuyerModel
    {
        [Required(ErrorMessage = "The career selection is required.")]
        public string CareerSelect { get; set; }

        [Required(ErrorMessage = "The phone number is required.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "The phone number must be numeric.")]
        public string PhoneNumber { get; set; }
    }

    #endregion
}
