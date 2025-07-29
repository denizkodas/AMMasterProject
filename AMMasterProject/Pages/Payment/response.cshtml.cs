using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Google.Api.Gax;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PayPal.Api;
using Serilog;
using Stripe;
using Stripe.Checkout;
using System.Net;



namespace AMMasterProject.Pages.Payment
{
    public class responseModel : PageModel
    {
        #region Model

        private readonly PaymentGatewayHelper _paymentGatewayHelper;
        private readonly MembershipHelper _membershipHelper;
        private readonly OrderHelper _orderHelper;
        private readonly ProductHelper _producthelper;
        public string PaymentMethod { get; set; }

        #endregion

        #region DI

        public responseModel(PaymentGatewayHelper paymentGatewayHelper, MembershipHelper membershipHelper, OrderHelper orderHelper)
        {
            _paymentGatewayHelper = paymentGatewayHelper;
            _membershipHelper = membershipHelper;
            _orderHelper = orderHelper;
        }

        #endregion
        public void OnGet()
        {

            //if(TempData["PaymentResponse"]==null)

            //{

            //    Response.Redirect(failredirection());
            //    return;
            //}


            if (Request.Query.ContainsKey("paymentmethod"))
            {
                PaymentMethod = Request.Query["paymentmethod"].ToString();


                int ProfileId = 0;
                if (User.Identity.IsAuthenticated)
                {

                    ProfileId = int.Parse(User.FindFirst("UserID")?.Value);
                    // continue with loginid variable

                    if(PaymentMethod =="paypal")
                    {
                        PaypalExecution();
                    }
                    else if(PaymentMethod == "stripe")
                    {
                        StripeExecution();
                    }

                    else if (PaymentMethod == "razorpay")
                    {
                        RazorPayExecution();
                    }

                    else if (PaymentMethod == "sslcommerz")
                    {
                        SSLCommerzExecution();
                    }

                    else if (PaymentMethod == "cod")
                    {
                        CODExecution();
                    }

                    else if (PaymentMethod == "wallet")
                    {
                        zeroamountExecution();
                    }

                    else if (PaymentMethod == "mtnmobile")
                    {
                        MTNMobilePayExecution();
                    }


                }

                else
                {
                    Response.Redirect(failredirection());

                }




            }

            else
            {
                Response.Redirect(failredirection());
            }


        }

        public string failredirection()
        {
            return "/Error?Title=Payment Response&Message=Payment Response Failed.&Body=Something went wrong please try again later.";

        }

        #region PaypalExecution
        public void PaypalExecution()
        {



            if (Request.Query.ContainsKey("paymentId") && Request.Query.ContainsKey("PayerID"))
            {
                string paymentid = Request.Query["paymentId"].ToString();
                string payerid = Request.Query["PayerID"].ToString();





                Dictionary<string, object> executestatus = _paymentGatewayHelper.ExecutePayment(paymentid, payerid);


                ///if success so execute order credit subscription or itempurchased
                if (executestatus != null && executestatus["executedstatus"] == "success")
                {
                    string membershiptype = Request.Query["membershiptype"].ToString();
                    string status = string.Empty;



                    var payerEmail = executestatus["payerEmail"].ToString();
                    var amountPaid = executestatus["amountPaid"].ToString();
                    var paidCurrency = executestatus["Currency"].ToString().ToUpper();

                    //var payerEmail = executestatus["payerEmail"] as string;
                    //var amountPaid = executestatus["amountPaid"];

                    if (membershiptype == "credit")
                    {
                       
                         GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "paypal", "paid", paymentid, payerid, amountPaid, paidCurrency, payerEmail, "completed");
                    }
                    else if (membershiptype == "subscription")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "paypal", "paid", paymentid, payerid, amountPaid, paidCurrency, payerEmail, "completed");
                    }
                    else if (membershiptype == "item")
                    {
                        GlobalHelper.SetReturnURLInSession("/orders");
                        status = orderexecute(membershiptype, "paypal", "paid", paymentid, payerid, amountPaid, paidCurrency, payerEmail, "processing");
                    }

                    else if (membershiptype == "boost")
                    {
                        GlobalHelper.SetReturnURLInSession("/orders");
                        status = orderexecute(membershiptype, "paypal", "paid", paymentid, payerid, amountPaid, paidCurrency, payerEmail, "processing");
                    }

                    if (status == "success")
                    {

                        TempData["success"] = "Purchased successfully";
                        Response.Redirect("/payment/success");
                    }
                    else
                    {

                        Response.Redirect("/Error?Title=Payment Success- Purchase Fail&Message=Your payment is success but your purchase is not assigned to your account.&Body=" + status);
                    }
                }

                else
                {
                    Response.Redirect(failredirection());
                }

            }

            else
            {
                Response.Redirect(failredirection());
            }



        }
        #endregion


        #region StripeExecution
        public void StripeExecution()
        {


            if (Request.Query.ContainsKey("session_id") )
            {
                string paymentid = Request.Query["session_id"].ToString();
                string membershiptype = Request.Query["membershiptype"].ToString();

                Dictionary<string, object> executestatus = _paymentGatewayHelper.ExecutePaymentStripe(paymentid);


                ///if success so execute order credit subscription or itempurchased
                if (executestatus != null && executestatus["executedstatus"] == "success")
                {

                    var payerid = executestatus["payerid"].ToString();
                    var amountPaid = executestatus["amountPaid"].ToString();
                    var paidCurrency = executestatus["Currency"].ToString();
                    var payerEmail = executestatus["payerEmail"].ToString();
                    //replace paymentid with payment intent id which is same as stripe dashboard id
                    paymentid = executestatus["paymentid"].ToString();


                    string status = string.Empty;
                    if (membershiptype == "credit")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "stripe", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                    }
                    else if (membershiptype == "subscription")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "stripe", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                    }
                    else if (membershiptype == "item")
                    {
                        GlobalHelper.SetReturnURLInSession("/orders");
                        status = orderexecute(membershiptype, "stripe", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                    }

                    else if (membershiptype == "boost")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "stripe", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                    }

                    if (status == "success")
                    {
                        Response.Redirect("/payment/success");
                    }
                    else
                    {
                       
                        Response.Redirect("/Error?Title=Payment Success- Purchase Fail&Message=Your payment is success but your purchase is not assigned to your account.&Body=" + status);
                    }


                }

                else
                {
                    Response.Redirect(failredirection());
                }
            }

            else
            {
                Response.Redirect(failredirection());
            }
        }
        #endregion

        #region SSLCommerzExecution

        public void SSLCommerzExecution()
        {

            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (Request.Query.ContainsKey("ssl_commerzID")) //its an invoice to clal the ssl commerz  verification method
            {
                string paymentid = Request.Query["ssl_commerzID"].ToString();
                string membershiptype = Request.Query["membershiptype"].ToString();

                Dictionary<string, object> executestatus = _paymentGatewayHelper.SSLCommerzPaymentVerification(paymentid);


                ///if success so execute order credit subscription or itempurchased
                if (executestatus != null && executestatus["executedstatus"] == "success")
                {

                    var payerid = executestatus["payerid"].ToString();
                    var amountPaid = executestatus["amountPaid"].ToString();
                    var paidCurrency = executestatus["Currency"].ToString();
                    var payerEmail = executestatus["payerEmail"].ToString();
                    //replace paymentid with payment intent id which is same as stripe dashboard id
                    paymentid = executestatus["paymentid"].ToString();


                    string status = string.Empty;
                    if (membershiptype == "credit")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "sslcommerz", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                    }
                    else if (membershiptype == "subscription")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "sslcommerz", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                    }
                    else if (membershiptype == "item")
                    {
                        GlobalHelper.SetReturnURLInSession("/orders");
                        status = orderexecute(membershiptype, "sslcommerz", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                    }

                    else if (membershiptype == "boost")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "sslcommerz", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                    }

                    if (status == "success")
                    {
                        Response.Redirect("/payment/success");
                    }
                    else
                    {

                        Response.Redirect("/Error?Title=Payment Success- Purchase Fail&Message=Your payment is success but your purchase is not assigned to your account.&Body=" + status);
                    }


                }

                else
                {
                    Response.Redirect(failredirection());
                }
            }

            else
            {
                Response.Redirect(failredirection());
            }
        }
        #endregion


        #region RazorPayExecution
        public void RazorPayExecution()
        {


            if (Request.Query.ContainsKey("razorpayorderid"))
            {
                string paymentid = Request.Query["razorpayorderid"].ToString();
                string membershiptype = Request.Query["membershiptype"].ToString();

                Dictionary<string, object> executestatus = _paymentGatewayHelper.ExecuteRazorPay(paymentid);


                ///if success so execute order credit subscription or itempurchased
                if (executestatus != null && executestatus["executedstatus"] == "success")
                {

                    var payerid = executestatus["payerid"].ToString();
                    var amountPaid = executestatus["amountPaid"].ToString();
                    var paidCurrency = executestatus["Currency"].ToString();
                    var payerEmail = executestatus["payerEmail"].ToString();
                    //replace paymentid with payment intent id which is same as stripe dashboard id
                    paymentid = executestatus["paymentid"].ToString();


                    string status = string.Empty;
                    if (membershiptype == "credit")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "razorpay", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                    }
                    else if (membershiptype == "subscription")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "razorpay", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                    }
                    else if (membershiptype == "item")
                    {
                        GlobalHelper.SetReturnURLInSession("/orders");
                        status = orderexecute(membershiptype, "razorpay", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                    }

                    else if (membershiptype == "boost")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "razorpay", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                    }

                    if (status == "success")
                    {
                        Response.Redirect("/payment/success");
                    }
                    else
                    {

                        Response.Redirect("/Error?Title=Payment Success- Purchase Fail&Message=Your payment is success but your purchase is not assigned to your account.&Body=" + status);
                    }


                }

                else
                {
                    Response.Redirect(failredirection());
                }
            }

            else
            {
                Response.Redirect(failredirection());
            }
        }
        #endregion

        #region MTMPayExecution

        public  void MTNMobilePayExecution()
        {


            if (Request.Query.ContainsKey("mtnmobileID"))
            {
                string paymentid = Request.Query["mtnmobileID"].ToString();
                string membershiptype = Request.Query["membershiptype"].ToString();

                Dictionary<string, object> executestatus = _paymentGatewayHelper.ExecuteMTNMobile(paymentid);/*_paymentGatewayHelper.ExecuteMTNMobile(paymentid)""*/;


                ///if success so execute order credit subscription or itempurchased
                if (executestatus != null && executestatus["executedstatus"] == "success")
                {

                    var payerid = executestatus["payerid"].ToString();
                    var amountPaid = executestatus["amountPaid"].ToString();
                    var paidCurrency = executestatus["Currency"].ToString();
                    var payerEmail = executestatus["payerEmail"].ToString();
                    //replace paymentid with payment intent id which is same as stripe dashboard id
                    paymentid = executestatus["paymentid"].ToString();


                    string status = string.Empty;
                    if (membershiptype == "credit")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "mtnmobile", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                    }
                    else if (membershiptype == "subscription")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "mtnmobile", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                    }
                    else if (membershiptype == "item")
                    {
                        GlobalHelper.SetReturnURLInSession("/orders");
                        status = orderexecute(membershiptype, "mtnmobile", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                    }

                    else if (membershiptype == "boost")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "mtnmobile", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                    }

                    if (status == "success")
                    {
                        Response.Redirect("/payment/success");
                    }
                    else
                    {

                        Response.Redirect("/Error?Title=Payment Success- Purchase Fail&Message=Your payment is success but your purchase is not assigned to your account.&Body=" + status);
                    }


                }

                else
                {
                    Response.Redirect(failredirection());
                }
            }

            else
            {
                Response.Redirect(failredirection());
            }
        }
        #endregion

        #region CODExecution
        public void CODExecution()
        {


            if (Request.Query.ContainsKey("cod_id"))
            {
                string paymentid = Request.Query["cod_id"].ToString();
                string membershiptype = Request.Query["membershiptype"].ToString();

               

                    var payerid = paymentid;
                    var amountPaid = Request.Query["amount"].ToString();
                    var paidCurrency = Request.Query["currency"].ToString();
                    var payerEmail = "";
                    //replace paymentid with payment intent id which is same as stripe dashboard id
                    


                    string status = string.Empty;
                    if (membershiptype == "credit")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "cod", "pending", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                    }
                    else if (membershiptype == "subscription")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "cod", "pending", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                    }
                    else if (membershiptype == "item")
                    {
                        GlobalHelper.SetReturnURLInSession("/orders");
                        status = orderexecute(membershiptype, "cod", "pending", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                    }

                    else if (membershiptype == "boost")
                    {
                        GlobalHelper.SetReturnURLInSession("/user/billing");
                        status = orderexecute(membershiptype, "cod", "pending", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                    }

                    if (status == "success")
                    {
                        Response.Redirect("/payment/success");
                    }
                    else
                    {

                        Response.Redirect("/Error?Title=Payment COD- Purchase Fail&Message=Your payment is success but your purchase is not assigned to your account.&Body=" + status);
                    }


                

               
            }

            else
            {
                Response.Redirect(failredirection());
            }
        }
        #endregion


        #region ZeroAmountExecutionExecution
        public void zeroamountExecution()
        {


            if (Request.Query.ContainsKey("wallet_id"))
            {
                string paymentid = Request.Query["wallet_id"].ToString();
                string membershiptype = Request.Query["membershiptype"].ToString();



                var payerid = paymentid;
                var amountPaid = Request.Query["amount"].ToString();
                var paidCurrency = Request.Query["currency"].ToString();
                var payerEmail = "";
                //replace paymentid with payment intent id which is same as stripe dashboard id



                string status = string.Empty;
                if (membershiptype == "credit")
                {
                    GlobalHelper.SetReturnURLInSession("/user/billing");
                    status = orderexecute(membershiptype, "wallet", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                }
                else if (membershiptype == "subscription")
                {
                    GlobalHelper.SetReturnURLInSession("/user/billing");
                    status = orderexecute(membershiptype, "wallet", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "completed");
                }
                else if (membershiptype == "item")
                {
                    GlobalHelper.SetReturnURLInSession("/orders");
                    status = orderexecute(membershiptype, "wallet", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                }

                else if (membershiptype == "boost")
                {
                    GlobalHelper.SetReturnURLInSession("/user/billing");
                    status = orderexecute(membershiptype, "wallet", "paid", paymentid, payerid, amountPaid.ToString(), paidCurrency.ToString(), payerEmail, "processing");
                }

                if (status == "success")
                {
                    Response.Redirect("/payment/success");
                }
                else
                {

                    Response.Redirect("/Error?Title=Payment wallet- Purchase Fail&Message=Your payment is success but your purchase is not assigned to your account.&Body=" + status);
                }





            }

            else
            {
                Response.Redirect(failredirection());
            }
        }
        #endregion

        #region OrderExecute
        public string orderexecute(string _membershiptype, string _paymentmethod, string paymentstatus, string _paymentreferencekey, string _payerid, string _amountpaid, string _currencypaid, string _payeremail, string orderprocessstatus)
        {
           
            string id = Request.Query["id"].ToString(); /// this is invoice number
            //string invoicenumber = Request.Query["invoicenumber"].ToString();

            
            int profileid = int.Parse(User.FindFirst("UserID")?.Value);
            if (_membershiptype == "credit")
            {
                //remove this meta data creation here and on click generate it
                //based on invoice number create payment update method

                //string paymentmetadata = _orderHelper.paymentmetadata(_paymentmethod, "paid", _paymentreferencekey, string.Empty, DateTime.Now, _payerid, itemmetadata, _amountpaid, _currencypaid,_payeremail);// payment metadata

                //string orderstatus = _orderHelper.OrderCreation(profileid, _membershiptype,"confirm", "purchased", "completed", invoicenumber, itemmetadata, paymentmetadata, "paid");

                string orderstatus = _orderHelper.OrderPaymentUpdate(_paymentmethod, paymentstatus, _paymentreferencekey, string.Empty, _amountpaid, _currencypaid, _payeremail, id, profileid, orderprocessstatus);
                if (orderstatus == "success")
                {

                   
                    return "success";
                }
                else
                {
                    return orderstatus;
                }



            }
            else if (_membershiptype == "subscription")
            {

                //string itemmetadata = _membershipHelper.SubscriptionMetaData(int.Parse(id), invoicenumber);  /// subscription itemmetadata
                //string paymentmetadata = _orderHelper.paymentmetadata(_paymentmethod, "paid", _paymentreferencekey, string.Empty, DateTime.Now, _payerid, itemmetadata, _amountpaid, _currencypaid, _payeremail);// payment metadata

                string orderstatus = _orderHelper.OrderPaymentUpdate(_paymentmethod, paymentstatus, _paymentreferencekey, string.Empty, _amountpaid, _currencypaid, _payeremail, id, profileid, orderprocessstatus);

                if (orderstatus == "success")
                {

                   
                    return "success";
                }
                else
                {
                    return orderstatus;
                }


            }
            else if (_membershiptype == "item")
            {

                //string itemmetadata = _membershipHelper.SubscriptionMetaData(int.Parse(id), invoicenumber);  /// subscription itemmetadata
                //string paymentmetadata = _orderHelper.paymentmetadata(_paymentmethod, "paid", _paymentreferencekey, string.Empty, DateTime.Now, _payerid, itemmetadata, _amountpaid, _currencypaid, _payeremail);// payment metadata

                string orderstatus = _orderHelper.OrderPaymentUpdate(_paymentmethod, paymentstatus, _paymentreferencekey, string.Empty, _amountpaid, _currencypaid, _payeremail, id, profileid, orderprocessstatus);

                if (orderstatus == "success")
                {

                    
                    return "success";
                }
                else
                {
                    return orderstatus;
                }


            }

            else if (_membershiptype == "boost")
            {

                //string itemmetadata = _membershipHelper.SubscriptionMetaData(int.Parse(id), invoicenumber);  /// subscription itemmetadata
                //string paymentmetadata = _orderHelper.paymentmetadata(_paymentmethod, "paid", _paymentreferencekey, string.Empty, DateTime.Now, _payerid, itemmetadata, _amountpaid, _currencypaid, _payeremail);// payment metadata

                string orderstatus = _orderHelper.OrderPaymentUpdate(_paymentmethod, paymentstatus, _paymentreferencekey, string.Empty, _amountpaid, _currencypaid, _payeremail, id, profileid, orderprocessstatus);

                if (orderstatus == "success")
                {

                   

                    return "success";
                }
                else
                {
                    return orderstatus;
                }


            }

            return "fail";
        }
        #endregion
    }
}
