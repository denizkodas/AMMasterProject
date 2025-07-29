using Amazon;
using AMMasterProject.Helpers;
using AMMasterProject.Models;
using AMMasterProject.Pages.Payment;
using AMMasterProject.ViewModel;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Serilog;
using System;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Drawing;
using System.IO;

using Azure;
using Azure.Core;
using Microsoft.Extensions.Hosting;
using PayPal.Api;
using Microsoft.Extensions.Caching.Memory;



//using IronPdf;

///this is controller to define all master method which use globally

namespace AMMasterProject.Controllers
{
    [Route("controller/[controller]/{action}")]
    [Controller]
    public class MasterController : Controller
    {

        #region DI


        private readonly MyDbContext _dbContext;
        private readonly FileUploadHelper _fileUploadHelper;
        private readonly NotificationHelper _notificationHelper;
        private readonly GlobalHelper _globalhelper;
        private readonly OrderHelper _orderhelper;
        private readonly UserHelper _userhelper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly WebsettingHelper _websettinghelper;
        private readonly IMemoryCache _cache;
        public MasterController(MyDbContext context, IMemoryCache cache, FileUploadHelper fileUploadHelper, NotificationHelper notificationHelper, GlobalHelper globalhelper, IWebHostEnvironment hostingEnvironment, OrderHelper orderhelper, WebsettingHelper websettinghelper, UserHelper userhelper)
        {
            _dbContext = context;
            _fileUploadHelper = fileUploadHelper;
            _notificationHelper = notificationHelper;
            _globalhelper = globalhelper;
            _hostingEnvironment = hostingEnvironment;
            _orderhelper = orderhelper;
            _websettinghelper = websettinghelper;
            _userhelper = userhelper;
            _cache = cache;
        }
        #endregion


        #region ContactForm



        [HttpPost]
        public IActionResult contactform(string contactname, int sellerid, string subject, string message, string currenturl)
        {




            ViewBag.Contactname = contactname;
            ViewBag.ReferenceID = sellerid;
            ViewBag.Subject = "Inquiry For: " + subject;

            ViewBag.Message = currenturl + " " + new HtmlString(message);

            return PartialView("/Pages/shared/_ContactFormView.cshtml");


        }
        [HttpPost]
        public IActionResult postcontactform(int referenceid, string emailtype, string emailbody, string mobilenotification, string redirecturl, string email)
        {

            try
            {
                string emailid = _userhelper.UserEmailByID(referenceid);

                // Create a JSON representation of the form data
                var formDataJson = new
                {
                    Email = email,
                    ToEmail = emailid,
                    ToProfileID = referenceid,


                    EmailBody = emailbody,
                    Title = mobilenotification,

                };

                // Convert the form data to a JSON string
                var formDataJsonString = JsonConvert.SerializeObject(formDataJson);

                // Set announcement for the receiver first
                _notificationHelper.NotificationSet(referenceid, mobilenotification, emailbody, "", "");

                // Store it for admin
                FormDetail fd = new FormDetail
                {
                    SubmitDate = DateTime.Now,
                    FormContent = formDataJsonString, // Store the JSON representation
                    IsRead = false,
                    ContentReply = "Pending"
                };

                _dbContext.FormDetails.Add(fd);
                _dbContext.SaveChanges();

                TempData["success"] = "Inquiry sent successfully";


                if (emailid != null)
                {
                    _notificationHelper.notification("sellercontactform", referenceid, emailbody, email, "", "", emailid, "");
                }

                // Return a response indicating the success or failure of the operation
                return Json(new { success = true });

            }
            catch (Exception ex)
            {

                Log.Error(ex, "postcontactform- Master Controller");
                return Json(new { success = false, messsage = ex.Message });

            }
        }
        #endregion


        #region FileUpload

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new Exception("No files received.");
                }
                var fileLink = await _fileUploadHelper.UploadFileAsync(file);

                var response = new
                {
                    FileName = file.FileName,
                    FileLink = fileLink
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error uploading file: " + ex.Message);
            }
        }


        #endregion

        #region HTMLEditor
        [HttpPost]
        public async Task<IActionResult> SaveHtml(string merge, string html, string css, string json, int pagenameid)
        {
            try
            {


                int loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");

                PageName update = _dbContext.PageNames.FirstOrDefault(u => u.PageNameId == pagenameid);
                if (update != null)
                {
                    update.PageDescription = merge;
                    update.PageHTML = html;
                    update.PageCSS = css;
                    update.PageJson = json;
                    update.InsertDate = DateTime.Now;
                    update.ProfileId = loginid;

                    _dbContext.PageNames.Update(update);

                    _dbContext.SaveChanges();

                    TempData["success"] = "Content updated successfully";





                }
                //else
                //{
                //    PageDetail insert = new PageDetail();

                //    insert.PageNameId = pagenameid;
                //    insert.PageDescription = merge;
                //    insert.PageHTML = html;
                //    insert.PageCSS = css;
                //    insert.PageJson = json;
                //    insert.IsPublish = true;
                //    insert.ProfileId = loginid;
                //    _dbContext.PageDetails.Add(insert);
                //    _dbContext.SaveChanges();


                //    TempData["success"] = "Content updated successfully";





                //}

            }
            catch (Exception ex)
            {
                TempData["success"] = ex.Message;
                return Json(new { success = false, message = ex.Message });
            }
            TempData["success"] = "Content updated successfully";
            // Return a response indicating the success or failure of the operation
            return Json(new { success = true, message = "" });
        }
        #endregion

        #region EmailScheduler
        [HttpGet]
        public async Task<IActionResult> EmailScheduler()
        {
            try
            {
                _notificationHelper.Pendingemails();
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }

            return Ok("success");
        }
        #endregion



        #region Encryption
        [HttpGet]

        public IActionResult encryption(string value)
        {
            try
            {

                string encryptedkey = EncryptionHelper.encryption(value);

                return new JsonResult(encryptedkey);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting encryption" + ex.Message);
            }
        }

        [HttpGet]

        public IActionResult dycryption(string cipherText)
        {
            try
            {
                ;

                string dycryptedkey = EncryptionHelper.dycryption(cipherText);

                return new JsonResult(dycryptedkey);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting dycryption" + ex.Message);
            }
        }
        #endregion


        #region IronPDF-Export


        [HttpGet]
        public async Task<IActionResult> ExportPDFIron(string url, string invoicenumber)
        {
            try
            {
                var companyName = HttpContext.Items["CompanyName"] as string;
                var companyLogo = HttpContext.Items["CompanyLogo"] as string;


                List<OrderViewModel> orderMaster = _orderhelper.GetOrdersItem().Where(u => u.OrderStatus == "confirm" && u.InvoiceNumber == invoicenumber).ToList();

                int loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                string usertype = User.FindFirst("UserType")?.Value ?? "0";

                // continue with loginid variable
                if (usertype == "Vendor")
                {
                    orderMaster = orderMaster.Where(u => u.SellerID == loginid).ToList();
                }



                var buyerName = orderMaster.FirstOrDefault()?.ShippingDetailMetaData?.FullName;

                var orderDate = orderMaster.FirstOrDefault().OrderDate;

                var shippingfullname = orderMaster.FirstOrDefault()?.ShippingDetailMetaData?.FullName;
                var shippingaddress = orderMaster.FirstOrDefault()?.ShippingDetailMetaData?.ShippingAddress;
                var shippinglandmark = orderMaster.FirstOrDefault()?.ShippingDetailMetaData?.NearestLandMark ?? "";
                var shippingemail = orderMaster.FirstOrDefault()?.ShippingDetailMetaData?.ShippingEmail;
                var shippingphone = orderMaster.FirstOrDefault()?.ShippingDetailMetaData?.ShippingPhone;


                var sellername = orderMaster.FirstOrDefault().SellerViewModel.BusinessName;


                var paymentmethod = @orderMaster.FirstOrDefault().PaymentMetaData.PaymentMethod.Replace("cod", "Cash On Delivery").Replace("banktransfer", "Bank Transfer");
                var paymentdate = @orderMaster.FirstOrDefault().PaymentMetaData.PaymentDate;
                var paymentreference = orderMaster.FirstOrDefault().PaymentMetaData.PaymentReference;
                var paymentstatus = orderMaster.FirstOrDefault().PaymentMetaData.PaymentStatus;



                var currency = orderMaster.FirstOrDefault().SummaryOrderMetaData.Currency;

                var itemtotal = orderMaster.Sum(order => order.SummaryOrderMetaData.ItemQtyTotal + order.SummaryOrderMetaData.VariationQtyTotal);

                var chargesTotal = orderMaster.Sum(order => order.SummaryOrderMetaData.ChargesQtyTotal);

                var Shippingtotal = orderMaster.Sum(order => order.SummaryOrderMetaData.ShippingQtyCost);

                var total = orderMaster.Sum(order => order.SummaryOrderMetaData.GrandTotal);

                //Define the path to the HTML template
                string templatePath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Templates", "ItemInvoiceTemplate.html");

                //Read the HTML template from the file

                string htmlTemplate = System.IO.File.ReadAllText(templatePath);

                //Replace placeholders with dynamic data
                htmlTemplate = htmlTemplate.Replace("{companylogo}", companyLogo);
                htmlTemplate = htmlTemplate.Replace("{Name}", buyerName);


                htmlTemplate = htmlTemplate.Replace("{invoicenumber}", invoicenumber);
                htmlTemplate = htmlTemplate.Replace("{orderdate}", orderDate);


                if (string.IsNullOrEmpty(shippingfullname))
                {
                    shippingfullname = "Digital item so shipping information is not required";
                }

                htmlTemplate = htmlTemplate.Replace("{shippingfullname}", shippingfullname);
                htmlTemplate = htmlTemplate.Replace("{shippingaddress}", shippingaddress);
                 htmlTemplate = htmlTemplate.Replace("{shippinglandmark}", shippinglandmark);
                htmlTemplate = htmlTemplate.Replace("{shippingemail}", shippingemail);
                htmlTemplate = htmlTemplate.Replace("{shippingphone}", shippingphone);


                htmlTemplate = htmlTemplate.Replace("{sellername}", sellername);



                htmlTemplate = htmlTemplate.Replace("{paymentmethod}", paymentmethod);
                htmlTemplate = htmlTemplate.Replace("{paymentdate}", paymentdate.ToString());
                htmlTemplate = htmlTemplate.Replace("{paymentreference}", paymentreference);
                htmlTemplate = htmlTemplate.Replace("{paymentstatus}", paymentstatus);



                htmlTemplate = htmlTemplate.Replace("{currency}", currency);


                htmlTemplate = htmlTemplate.Replace("{itemtotal}", itemtotal.ToString("N2"));
                htmlTemplate = htmlTemplate.Replace("{shippingtotal}", Shippingtotal.ToString("N2"));
                htmlTemplate = htmlTemplate.Replace("{othercharges}", chargesTotal.ToString("N2"));
                htmlTemplate = htmlTemplate.Replace("{total}", total.ToString("N2"));

                //Create a StringBuilder to accumulate the item rows
                var itemRows = new StringBuilder();

                foreach (var item in orderMaster)
                {
                    string digital = "";
                    if (item.ItemDetailMetaData.basicModel.ListingType == "Digital")
                    {
                        // Display the "digital" value
                        digital = " - " + item.ItemDetailMetaData.basicModel.ListingType;
                    }

                    //Calculate itemVariation
                    //decimal itemVariation = item.SummaryOrderMetaData.Total;
                    //if (item.ItemDetailMetaData.variationModel != null)
                    //{
                    //    //FIX IT
                    //    foreach (var VariationTotal in item.ItemDetailMetaData.variationModel)
                    //    {
                    //        itemVariation += VariationTotal.ConversionAmount;
                    //    }
                    //}

                    //Calculate itemVariationQTYTotal
                    //decimal itemVariationQTYTotal = item.SummaryOrderMetaData.ItemQtyTotal;
                    //if (item.SummaryOrderMetaData != null)
                    //{
                    //    itemVariationQTYTotal += item.SummaryOrderMetaData.VariationQtyTotal;
                    //}

                    //Define the HTML row structure for each item

                    var itemRow = $@"
        <tr class='item'>
            <td class='desc'>{item.ItemDetailMetaData.basicModel.Name} 
                  {digital} 

               </td>
            <td class='id num'>SKU-{item.ItemDetailMetaData.basicModel.ID}</td>
            <td class='qty'>{item.ItemDetailMetaData.basicModel.Quantity}</td>
            <td class='amt'>{currency} {item.SummaryOrderMetaData.Total.ToString("N2")}</td>
            <td class='amt'>{currency} {item.SummaryOrderMetaData.TotalQty.ToString("N2")}</td>
        </tr>";

                    //Append the item row to the StringBuilder
                    itemRows.Append(itemRow);
                }

                //Replace the { itemRows}
                //placeholder in the HTML template with the generated item rows
                htmlTemplate = htmlTemplate.Replace("{itemRows}", itemRows.ToString());

                //Create a ChromePdfRenderer
                var renderer = new ChromePdfRenderer();

                //Render HTML as PDF
                var pdf = renderer.RenderHtmlAsPdf(htmlTemplate);

                //Convert PDF to a byte array
                byte[] pdfBytes = pdf.BinaryData;

                //Specify the file name for the stored file

                string fileName = $"{invoicenumber}.pdf";

                //Define the directory path where the file will be stored on the server
                string directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "invoices");


                //Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath);

                //Combine the directory path and file name to get the full file path
                string filePath = Path.Combine(directoryPath, fileName);

                //Write the PDF bytes to the file
                System.IO.File.WriteAllBytes(filePath, pdfBytes);

                //Return the file path as a response(for example, for further download)
                return Ok(filePath);

            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred: " + ex.Message);
                //Handle the exception as needed, e.g., return an error view
                return View("Error");
            }
        }





        [HttpPost]
        public IActionResult DeletePDFFile(string invoicenumber)
        {
            try
            {
                // Define the directory path where the file is stored on the server
                string directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "invoices");


                // Combine the directory path and file name to get the full file path
                string filePath = Path.Combine(directoryPath, $"{invoicenumber}" + ".pdf");

                // Check if the file exists before attempting to delete it
                if (System.IO.File.Exists(filePath))
                {
                    // Delete the file
                    System.IO.File.Delete(filePath);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                // Handle the exception as needed, e.g., return an error response
                return BadRequest();
            }
        }
        #endregion





        #region Edit-FormSubmission
        [HttpPost]
        public IActionResult formdata()
        {



            // Access all form fields using the IFormCollection
            IFormCollection formFields = Request.Form;

            // Check if the "firstname" field is not null (indicating potential bot submission)
            if (formFields.ContainsKey("First Name") && formFields["First Name"] != "")
            {
                // Log or handle the bot submission, e.g., return an error response
                return Redirect("~/Error");
            }

            if (formFields.ContainsKey("Last Name") && formFields["Last Name"] != "")
            {
                // Log or handle the bot submission, e.g., return an error response
                return Redirect("~/Error");
            }
            if (formFields.ContainsKey("Fax") && formFields["Fax"] != "")
            {
                // Log or handle the bot submission, e.g., return an error response
                return Redirect("~/Error");
            }


            // Create a dictionary to store form field names and values
            Dictionary<string, string> formDataDict = new Dictionary<string, string>();

            // Process the form fields and populate the dictionary
            foreach (var field in formFields)
            {
                string fieldName = field.Key;
                string fieldValue = field.Value;

                // Add the field name and value to the dictionary
                formDataDict[fieldName] = fieldValue;
            }

            // Serialize the dictionary to JSON
            string formDataJson = JsonConvert.SerializeObject(formDataDict);


            FormDetail fd = new FormDetail();
            fd.SubmitDate = DateTime.Now;
            fd.FormContent = formDataJson;
            fd.IsRead = false;
            fd.ContentReply = "Pending";
            _dbContext.FormDetails.Add(fd);
            _dbContext.SaveChanges();



            // Create an HTML table to display the form data
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.Append("<table>");

            foreach (var field in formFields)
            {
                string fieldName = field.Key;
                string fieldValue = field.Value;

                // Add a row to the HTML table for each form field
                htmlTable.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", fieldName, fieldValue);
            }

            htmlTable.Append("</table>");



            string jsonString = formDataJson;

            // Parse the JSON string
            JObject json = JObject.Parse(jsonString);

            // Check if the "Email" key exists and is not null
            if (json.TryGetValue("Email", out JToken emailToken) && emailToken.Type != JTokenType.Null)
            {
                string email = emailToken.Value<string>();

                // Call the email relay method with the email value
                //_globalHelper.emailrelay("Reply- Inquiry From Amtechnology.info", htmlTable.ToString(), email);
                _notificationHelper.notificationrelay("Contact Form Inquiry", htmlTable.ToString(), 0, email, "", 0);

            }



            //return Content(formDataJson, "application/json");
            // Return the JSON response
            return Redirect("/formsubmitted");
        }
        #endregion


        #region CaptachValidation

        [HttpPost]

        public IActionResult CaptchaValidation(string response)
        {
            var _capthasettingSettings = _websettinghelper.GetWebsettingJson("GoogleRecaptchaSettings");
            var json = JsonConvert.DeserializeObject<GoogleRecaptchaSettingModel>(_capthasettingSettings);
            string ReCaptcha_Secret = json.ReCaptchaSecret; // Replace with your actual reCAPTCHA secret key
            //string response = Request.Form["g-recaptcha-response"]; 

            string result = "";
            string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + ReCaptcha_Secret + "&response=" + response;

            using (WebClient client = new WebClient())
            {
                result = client.DownloadString(url);
            }

            // Parse the result as JSON and return it
            dynamic resultObj = JsonConvert.DeserializeObject(result);
            return Json(resultObj);
        }
        #endregion


        #region Inquiries

        public IActionResult InquiryRead(int formdetailsid)
        {
            try
            {
                FormDetail fd = _dbContext.FormDetails.FirstOrDefault(u => u.FormDetailsID == formdetailsid);

                if (fd != null)
                {
                    fd.IsRead = !fd.IsRead;
                    _dbContext.SaveChanges();
                }


                TempData["success"] = "Updated successfully";
                return RedirectToPage("/admin/allinquiries");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while InquiryRead");
            }
        }

        public IActionResult InquiryDelete(int formdetailsid)
        {
            try
            {
                FormDetail fd = _dbContext.FormDetails.FirstOrDefault(u => u.FormDetailsID == formdetailsid);

                if (fd != null)
                {
                    _dbContext.Remove(fd);
                    _dbContext.SaveChanges();
                }


                TempData["success"] = "Deleted successfully";
                return RedirectToPage("/admin/allinquiries");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while InquiryDelete");
            }
        }

        public IActionResult InquiryReply(int formdetailsid, string contentreply)
        {
            try
            {

                if (contentreply == "")
                {
                    return Content("Error");
                }


                FormDetail fd = _dbContext.FormDetails.FirstOrDefault(u => u.FormDetailsID == formdetailsid);

                if (fd != null)
                {



                    fd.ContentReply = contentreply;
                    _dbContext.SaveChanges();

                    // Your JSON string
                    string jsonString = fd.FormContent;

                    // Parse the JSON string
                    JObject json = JObject.Parse(jsonString);

                    // Check if the "Email" key exists and is not null
                    if (json.TryGetValue("Email", out JToken emailToken) && emailToken.Type != JTokenType.Null)
                    {
                        string email = emailToken.Value<string>();

                        // Call the email relay method with the email value

                        // Create an HTML table to display the form data
                        StringBuilder htmlTable = new StringBuilder();
                        htmlTable.Append("<table>");

                        // Add a row to the HTML table for each form field
                        htmlTable.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", "Message", $"Hello,<br />{fd.ContentReply}");

                        htmlTable.Append("</table>");
                        _notificationHelper.emailrelay("Inquiry Replied #" + fd.FormDetailsID, htmlTable.ToString(), email, 0);

                        //_notificationHelper.notification("sellercontactform", referenceid, emailbody, email, "", "", emailid, "");



                    }
                }



                TempData["success"] = "Reply Added Successfully";


                return RedirectToPage("/admin/allinquiries");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while InquiryReply");
            }
        }
        #endregion



        #region PaymentOptions

        public IActionResult GetBankDetails()
        {
            try
            {


                var bankTransfer = new BankTransferSettingsModel();

                var transferSettingsJson = _websettinghelper.GetWebsettingJson("BankTransferSettings");
                if (!string.IsNullOrEmpty(transferSettingsJson))
                {
                    var json = JsonConvert.DeserializeObject<BankTransferSettingsModel>(transferSettingsJson);

                    if (json != null)
                    {
                        bankTransfer = new BankTransferSettingsModel
                        {
                            AccountDetails = json.AccountDetails,
                            IsEnable = json.IsEnable
                        };
                    }
                }

                return PartialView("/Pages/payment/_bankdetails.cshtml", bankTransfer);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region EmailGenerator
        public IActionResult EmailGenerator(int notificationrelayid)
        {
            try
            {

                NotificationRelay nr = _dbContext.NotificationRelays.FirstOrDefault(u => u.NotificationRelayId == notificationrelayid);

                if (nr != null)
                {

                    _notificationHelper.emailrelay(nr.NotificationRelaySubject, nr.NotificationRelayBody, nr.Receiver, nr.NotificationRelayId);

                }


                return Json(new { success = true, notificationrelayid });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region BarCodeGenerator


        [HttpPost]
        

        public IActionResult BarCodeGenerator(string inputData)
        {
            var imageData = _globalhelper.GenerateBarcodes(inputData);

            var viewModel = new BarcodeViewModel
            {
                Data = "data:image/png;base64," + Convert.ToBase64String(imageData)
            };

            return PartialView("/Pages/shared/_barcodegenerator.cshtml", viewModel);
        }

        public IActionResult QRCodeGenerator(string inputData)
        {
            var imageData = _globalhelper.GenerateQRcodes(inputData);

            var viewModel = new BarcodeViewModel
            {
                Data = "data:image/png;base64," + Convert.ToBase64String(imageData)
            };

            return PartialView("/Pages/shared/_barcodegenerator.cshtml", viewModel);
        }
        #endregion

        #region AccessDenied
        public IActionResult AccessDenied()
        {

            //ViewBag.MyString = myString;
            return RedirectToPage("/Error", new { Title = "Access Denied", Message = "You do not have permission to access this feature" });
        }
        #endregion


        #region CacheReleased

        public IActionResult CacheReleased(string cachekey)
        {
           
                // Remove the cached item by its key
                _cache.Remove("CacheHomeItemhomepagesetup");

                _cache.Remove("CacheHomeCategories");
                _cache.Remove("CacheHomeBlog");

            ///all blog cache in blog component
                _cache.Remove("BlogCache");

            ///globalviecompoent cache


            // Optionally, return a response indicating cache release
            return Ok(new { message = "Cache released successfully." });
        }
        #endregion
    }


}

