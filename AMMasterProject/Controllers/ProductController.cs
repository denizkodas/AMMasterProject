using Amazon.Runtime;
using Amazon.S3.Model;
using Amazon.S3;
using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net;
using Serilog.Sinks.File;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AMMasterProject.Migrations;
using Stripe;
using PayPal.Api;

namespace AMMasterProject.Controllers
{

    [Route("controller/[controller]/{action}")]
    [Controller]
    public class ProductController : Controller
    {

        #region DI


        private readonly MyDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly FileUploadHelper _fileUploadHelper;

        private readonly ProductHelper _producthelper;
        private readonly OrderHelper _orderhelper;
        private readonly NotificationHelper _notificationHelper;

        private readonly UserHelper _userhelper;
        public ProductViewModel productmodel { get; set; }
        public ProductController(MyDbContext context, IMemoryCache cache, ProductHelper producthelper, FileUploadHelper fileUploadHelper, OrderHelper orderhelper, NotificationHelper notificationHelper, UserHelper userhelper)
        {
            _dbContext = context;
            _cache = cache;
            _producthelper = producthelper;
            _fileUploadHelper = fileUploadHelper;
            _orderhelper = orderhelper;
            _notificationHelper = notificationHelper;
            _userhelper = userhelper;
        }


        #endregion


        #region Detail




        public IActionResult productdetail(string productseourl)
        {


            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }



            //ProductViewModel productdetail = _producthelper.productmasterdata(loginid).Where(u => u.ProductSeourl == productseourl).FirstOrDefault();
            //ProductViewModel productdetail = _producthelper.productmasterdataV2(loginid, "seourl", 30, 1, 0, productseourl).FirstOrDefault();


            //List<ImageItemMetaData> productimage = _producthelper.productimages(productseourl);
            //List<ProductViewModel> productamenitiesheader = _producthelper.productamenitiesheader(productseourl, "Amenities");
            //List<ProductViewModel> productamenitieschild = _producthelper.productamenitieschild();

            ////productmodel = new ProductViewModel
            ////{
            ////    ProductDetail = productdetail,
                
            ////};



            // ProductViewModel  productdetail = _producthelper.productmasterdata(loginid).Where(u => u.ProductSeourl == productseourl).FirstOrDefault();

            ProductViewModel productdetail = _producthelper.productmasterdataV2(loginid, "seourl", 30, 1, 0, productseourl).FirstOrDefault();





            List<ProductViewModel> productamenitiesheader = _producthelper.productamenitiesheader(productdetail.ListAmenityMetaData);
            List<ProductViewModel> productamenitieschild = _producthelper.productamenitieschild(productdetail.ListAmenityMetaData);







            productmodel = new ProductViewModel
            {
                ProductDetail = productdetail,
                //ProductImages = productimage,
                ProductAmenityHeader = productamenitiesheader,
                ProductAmenityChild = productamenitieschild,

                ProductAttributeViewModel = _producthelper.GetProductAttributes(productdetail.ProductGUID),

            };


            return PartialView("/Pages/listing/_product-view.cshtml", productmodel);


        }
        #endregion



        #region Default

        //[ResponseCache(Duration = 86400)]
        public IActionResult Itemhomepagesetup()
        {
            List<HomePageFirstLevel> productmodel;

           
                // Data not found in cache, retrieve it and cache it for 7 days
                productmodel = _producthelper.itemsettingv2();
               
            return PartialView("/Pages/Listing/_ProductSettingList.cshtml", productmodel);
        }



        #endregion

        #region ItemDiscountlist
        public IActionResult ItemDiscounts()
        {

            List<ProductViewModel> productmodel = _producthelper.productmasterdataV2(0, "discount", 20, 1, 0);


            return PartialView("/Pages/Listing/_ProductList.cshtml", productmodel);


        }
        #endregion

        #region Wishlist


        #region MyWishlistList

        public IActionResult wishlist()
        {


            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }



            var productlist = _producthelper.productmasterdataV2(loginid, "favorite", 30, 1).ToList();

            return PartialView("/Pages/Listing/_ProductList.cshtml", productlist);


        }
        #endregion


        #region WishlistForm

        public IActionResult WishlistForm(int productid)
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }
            IEnumerable<SelectListItem> usergroup = (from cw in _dbContext.CustomerWishlists
                                                     orderby cw.Cwgroupname
                                                     where cw.UserId == loginid
                                                     select new SelectListItem
                                                     {
                                                         Value = cw.Cwgroupname.ToString(),
                                                         Text = cw.Cwgroupname
                                                     }).Distinct().ToList();




            var query = _producthelper.productmasterdataV2(loginid, "wishlistform", 30, 1).ToList();

            var productlist = (from p in query
                               join cw in _dbContext.CustomerWishlists
                               on p.ProductId equals cw.ProductId

                               select new ProductViewModel
                               {

                                   ProductId = p.ProductId,
                                   ProductName = p.ProductName,
                                   wishlistgroupname = cw.Cwgroupname

                               }).ToList();

            var Wishlistmodel = new WishlistViewModel
            {
                wishlistGroup = usergroup,
                productViewModel = productlist
            };



            return PartialView("/Pages/listing/_wishlist.cshtml", Wishlistmodel);
        }

        #endregion


        #region WishlistUp-Sert
        [HttpPost]
        public IActionResult wishlistupsert(int productid, string groupname)
        {
            string msg = string.Empty;
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }





            CustomerWishlist validate = _dbContext.CustomerWishlists.FirstOrDefault(w => w.ProductId == productid && w.UserId == loginid && w.Cwgroupname == groupname);
            if (validate == null)
            {
                CustomerWishlist cwinsert = new CustomerWishlist();


                cwinsert.ProductId = productid;
                cwinsert.UserId = loginid;
                cwinsert.InsertDate = DateTime.Now;
                //cwinsert.IP = ip;
                cwinsert.Cwgroupname = groupname;

                _dbContext.CustomerWishlists.Add(cwinsert);




                msg = "Added";
            }
            else
            {
                _dbContext.CustomerWishlists.Remove(validate);

                msg = "Deleted";
            }


            _dbContext.SaveChanges();



            return Json(msg);
        }

        #endregion


        #endregion

        #region VendorShop
        public IActionResult vendorshop(string shopurlpath)
        {


            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

            //var productsetting = _producthelper.productsetting("Vendor Shop");
            //var productlist = _producthelper.productmasterdata(loginid).Where(u => u.Shopurlpath == shopurlpath).ToList();

            var productlist = _producthelper.productmasterdataV2(loginid, "shopurlpath", 30, 1, 0, shopurlpath).ToList();




            ///if shopurl path is not null so filter the shop path
            ///



            productmodel = new ProductViewModel
            {
                ProductList = productlist,


            };


            return PartialView("/Pages/Listing/_ProductList.cshtml", productmodel);


        }

        #endregion


        #region Search
        public IActionResult search()
        {
            int pagesize = 12;
            int pagenumber = 1;
            if (Request.Query.ContainsKey("pagenumber"))
            {
                pagenumber = int.Parse(Request.Query["pagenumber"].ToString());
            }

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }





            #region Filters



            // Create a dictionary to store query string parameters and their values
            Dictionary<string, string> queryParams = new Dictionary<string, string>();

            // Check if the "productseourl" query string parameter is present
            if (Request.Query.ContainsKey("productseourl"))
            {
                string productseourl = Request.Query["productseourl"].ToString();
                queryParams["productseourl"] = productseourl;
            }

            // Check if the "productkeyword" query string parameter is present
            if (Request.Query.ContainsKey("q"))
            {
                string keyword = Request.Query["q"].ToString();
                queryParams["q"] = keyword;
            }

            // Check if other query string parameters are present and add them to the dictionary
            if (Request.Query.ContainsKey("categoryid"))
            {
                //int categoryId;
                //if (int.TryParse(Request.Query["categoryid"].ToString(), out categoryId))
                //{
                //    queryParams["categoryid"] = categoryId.ToString();
                //}

                string keyword = Request.Query["categoryid"].ToString();
                queryParams["categoryid"] = keyword;
            }

            if (Request.Query.ContainsKey("producttype"))
            {
                string vproducttype = Request.Query["producttype"].ToString();
                queryParams["producttype"] = vproducttype;
            }

            if (Request.Query.ContainsKey("sellingtype"))
            {
                string vsellingtype = Request.Query["sellingtype"].ToString();
                queryParams["sellingtype"] = vsellingtype;
            }

            if (Request.Query.ContainsKey("rating"))
            {
                string rating = Request.Query["rating"].ToString();
                queryParams["rating"] = rating;
            }

            if (Request.Query.ContainsKey("address"))
            {
                string address = Request.Query["address"].ToString();
                queryParams["address"] = address;
            }

            if (Request.Query.ContainsKey("distance"))
            {
                string distance = Request.Query["distance"].ToString();
                queryParams["distance"] = distance;
            }

            if (Request.Query.ContainsKey("distancetype"))
            {
                string distancetype = Request.Query["distancetype"].ToString();
                queryParams["distancetype"] = distancetype;
            }

            decimal fromprice, toprice;
            if (Request.Query.TryGetValue("minprice", out var minPriceValue) &&
                Request.Query.TryGetValue("maxprice", out var maxPriceValue) &&
                decimal.TryParse(minPriceValue, out fromprice) &&
                decimal.TryParse(maxPriceValue, out toprice))
            {
                queryParams["minprice"] = fromprice.ToString();
                queryParams["maxprice"] = toprice.ToString();
            }

            // Now you have a dictionary "queryParams" containing query string parameters and their values

            // Serialize the queryParams dictionary to a query string
            string queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));


            #endregion



            //var productlist = _producthelper.productmasterdata(loginid).ToList();

            var productlist = _producthelper.productmasterdataV2(loginid, "search", pagesize, pagenumber, 0, queryString.ToString());

            //productmodel = new ProductViewModel
            //{
            //    ProductList = productlist


            //};
            // Store the original unfiltered and unpaged product list count

            TempData["TotalItemCount"] = _producthelper.TotalRecord;
            //productlist = productlist.Skip(((int)pagenumber - 1) * pagesize).Take(pagesize).ToList();


            return PartialView("/Pages/Listing/_ProductList.cshtml", productlist);






        }
        #endregion


        #region MultiImages
        [HttpPost]
        public async Task<IActionResult> Image(List<IFormFile> files)
        {
            try
            {
                if (files == null || !files.Any())
                {
                    throw new Exception("No files received.");
                }

                var fileLinkTasks = files.Select(file => _fileUploadHelper.UploadFileAsync(file)).ToList();
                await Task.WhenAll(fileLinkTasks);

                var response = new List<string>(); // Changed the type of response to string list
                foreach (var task in fileLinkTasks)
                {

                    var fileLink = await task;

                    response.Add(fileLink.Trim());
                }

                var commaSeparatedValue = string.Join(",", response); // Create comma-separated value

                return Content(commaSeparatedValue); // Return the comma-separated value
            }
            catch (Exception ex)
            {
                return BadRequest("Error uploading file: " + ex.Message);
            }

        }

        #endregion


        #region AmenityUp-sert
        [HttpPost]
        public async Task<IActionResult> amenityupsert(int ProductAmenitiesOptionID, int amenityid, Guid productguid)
        {
            try
            {

                _producthelper.amenitiesmetadata(ProductAmenitiesOptionID, amenityid, productguid);



                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("Error uploading file: " + ex.Message);
            }

        }
        #endregion


        #region Taxclass

        [HttpPost]
        public async Task<IActionResult> taxclassdelete(Guid taxclassguid, int loginid)
        {
            try
            {
                SetupTaxClass taxclassremove = _dbContext.SetupTaxClasses.FirstOrDefault(u => u.TaxClassGuid == taxclassguid && u.ProfileId == loginid);

                if (taxclassremove != null)
                {
                    _dbContext.SetupTaxClasses.Remove(taxclassremove);
                    _dbContext.SaveChanges();
                }






                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("taxclassdelete: " + ex.Message);
            }
            finally
            {
                _dbContext.Dispose();
            }
        }

        [HttpPost]
        public async Task<IActionResult> taxclassupsert(Guid taxclassguid, int loginid, string taxclass)
        {
            try
            {
                SetupTaxClass upsert = _dbContext.SetupTaxClasses.FirstOrDefault(u => u.TaxClassGuid == taxclassguid && u.ProfileId == loginid);

                if (upsert != null)
                {
                    upsert.TaxClass = taxclass;


                    _dbContext.SaveChanges();
                }






                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("taxclassdelete: " + ex.Message);
            }
            finally
            {
                _dbContext.Dispose();
            }
        }


        #endregion


        #region ProductTax
        [HttpPost]
        public async Task<IActionResult> taxupsert(Guid productguid, int taxclassid, string type, decimal value)
        {
            try
            {
                ProductTaxV2 ups = _dbContext.ProductTaxV2s.FirstOrDefault(u => u.TaxClassId == taxclassid && u.ProductGuid == productguid);

                if (ups != null)
                {
                    ups.Value = value;
                    ups.Type = type;
                    ups.ProductGuid = productguid;
                    ups.TaxClassId = taxclassid;
                    ups.ModifiedDate = DateTime.Now;

                    _dbContext.SaveChanges();
                }

                else
                {
                    ProductTaxV2 ins = new ProductTaxV2();


                    ins.Value = value;
                    ins.Type = type;
                    ins.ProductGuid = productguid;
                    ins.TaxClassId = taxclassid;
                    ins.InsertDate = DateTime.Now;
                    ins.Status = true;
                    _dbContext.ProductTaxV2s.Add(ins);
                    _dbContext.SaveChanges();

                }




                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("taxupsert: " + ex.Message);
            }
            finally
            {
                _dbContext.Dispose();
            }
        }



        [HttpPost]
        public async Task<IActionResult> taxdelete(Guid productguid, int taxclassid)
        {
            try
            {
                ProductTaxV2 del = _dbContext.ProductTaxV2s.FirstOrDefault(u => u.TaxClassId == taxclassid && u.ProductGuid == productguid);

                if (del != null)
                {
                    _dbContext.ProductTaxV2s.Remove(del);

                    _dbContext.SaveChanges();
                }






                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("taxdelete: " + ex.Message);
            }
            finally
            {
                _dbContext.Dispose();
            }
        }
        #endregion


        #region Attribute

        #region Delete


        [HttpPost]
        public async Task<IActionResult> attributedelete(Guid productguid, int loginid, Guid productattributeguid)
        {
            try
            {
                ProductAttributeQuestionV2 attribute = _dbContext.ProductAttributeQuestionV2s.FirstOrDefault(u => u.ProductGuid == productguid && u.ProfileId == loginid && u.ProductAttributeGuid == productattributeguid);

                if (attribute != null)
                {
                    _dbContext.ProductAttributeQuestionV2s.Remove(attribute);
                    _dbContext.SaveChanges();
                }






                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("attributedelete: " + ex.Message);
            }
            finally
            {
                _dbContext.Dispose();
            }
        }
        #endregion


        #region AttributeUpsert


        [HttpPost]
        public IActionResult attributeupsert(int loginid, Guid productguid, string question, string type, int sort, bool ispublish, int ProductAttributeId)
        {
            #region CustomAssignment
            string AttributeGuid;

            #endregion

            #region ModelValidation
            if (ProductAttributeId == 0)
            {
                ProductAttributeQuestionV2 productAttributeQuestion = new ProductAttributeQuestionV2();

                productAttributeQuestion.ProfileId = loginid;
                productAttributeQuestion.InsertDate = DateTime.Now;
                productAttributeQuestion.ProductGuid = productguid;
                productAttributeQuestion.Question = question;
                productAttributeQuestion.Type = type;
                productAttributeQuestion.SortNumber = sort;
                productAttributeQuestion.IsPublish = true;

                _dbContext.ProductAttributeQuestionV2s.Add(productAttributeQuestion);
                _dbContext.SaveChanges();
                AttributeGuid = productAttributeQuestion.ProductAttributeGuid.ToString();

            }
            else
            {
                ProductAttributeQuestionV2 up = _dbContext.ProductAttributeQuestionV2s.FirstOrDefault(u => u.ProductAttributeId == ProductAttributeId);

                up.Question = question;
                up.Type = type;
                up.SortNumber = sort;
                up.IsPublish = ispublish;

                _dbContext.ProductAttributeQuestionV2s.Update(up);
                _dbContext.SaveChanges();
                AttributeGuid = up.ProductAttributeGuid.ToString();

            }



            TempData["success"] = "Attribute save successfully";
            #endregion



            return Json(AttributeGuid);
        }

        #endregion


        #region AttributeDelete
        [HttpPost]
        public IActionResult attdel(Guid attributeguid)
        {


            ProductAttributeQuestionV2 productAttributeQuestion = _dbContext.ProductAttributeQuestionV2s.FirstOrDefault(u => u.ProductAttributeGuid == attributeguid);



            #region ModelValidation

            if (productAttributeQuestion != null)
            {

                var productAttributeOption = _dbContext.ProductAttributeOptionV2s.Where(u => u.ProductAttributeGuid == attributeguid).ToList();

                foreach (var item in productAttributeOption)
                {
                    _dbContext.ProductAttributeOptionV2s.Remove(item); // Remove individual items one by one
                }


                _dbContext.ProductAttributeQuestionV2s.Remove(productAttributeQuestion);
                _dbContext.SaveChanges();

            }




            #endregion

            return Json("success");
        }
        #endregion

        #endregion

        #region AttributeOption

        #region OptionUpsert
        [HttpPost]

        public IActionResult optionusert(string optiontext, decimal price, int loginid, Guid attributeguid, int attributeoptionid, string image, int sort)
        {
            #region ModelValidation

            if (!string.IsNullOrEmpty(optiontext))
            {
                string[] optionValues = optiontext.Split(',').Select(value => value.Trim()).ToArray();


                foreach (string optionValue in optionValues)
                {

                    if (!string.IsNullOrEmpty(optionValue))
                    {
                        ProductAttributeOptionV2 productAttributeOption = new ProductAttributeOptionV2();

                        productAttributeOption.InsertDate = DateTime.Now;
                        productAttributeOption.IsCorrect = true;
                        productAttributeOption.OptionText = optionValue.Trim(); // Trim to remove leading/trailing spaces
                        productAttributeOption.Attributeprice = price;
                        productAttributeOption.ProductAttributeGuid = attributeguid;
                        productAttributeOption.ProductAttributeOptionId = attributeoptionid;
                        productAttributeOption.Attributeimage = image;
                        productAttributeOption.Sort = sort;

                        if (productAttributeOption.ProductAttributeOptionId == 0)
                        {
                            // Insert the new option
                            _dbContext.ProductAttributeOptionV2s.Add(productAttributeOption);
                        }
                        else
                        {
                            // Update the existing option
                            var existingOption = _dbContext.ProductAttributeOptionV2s.Find(productAttributeOption.ProductAttributeOptionId);

                            if (existingOption != null)
                            {
                                existingOption.OptionText = productAttributeOption.OptionText;
                                existingOption.Attributeprice = productAttributeOption.Attributeprice;
                                existingOption.Attributeimage = productAttributeOption.Attributeimage;
                                existingOption.ProductAttributeGuid = productAttributeOption.ProductAttributeGuid;
                                existingOption.Sort = productAttributeOption.Sort;
                                _dbContext.ProductAttributeOptionV2s.Update(existingOption);
                            }
                        }
                    }
                }

                _dbContext.SaveChanges();
            }

            #endregion

            return Json("success");
        }
        #endregion


        #region OptionView

        public IActionResult optionview(Guid attributeguid)
        {

            List<ProductAttributeOptionV2> attributeoptionlist = _dbContext.ProductAttributeOptionV2s.Where(u => u.ProductAttributeGuid == attributeguid).ToList();




            return PartialView("/Pages/listing/create/_attributeoption.cshtml", attributeoptionlist);


        }
        #endregion


        #region OptionDelete
        [HttpPost]
        public IActionResult optiondelete(int attributeoptionid)
        {


            ProductAttributeOptionV2 productAttributeOption = _dbContext.ProductAttributeOptionV2s.FirstOrDefault(u => u.ProductAttributeOptionId == attributeoptionid);



            #region ModelValidation

            if (productAttributeOption != null)
            {


                _dbContext.ProductAttributeOptionV2s.Remove(productAttributeOption);
                _dbContext.SaveChanges();

            }




            #endregion

            return Json("success");
        }

        #endregion

        #endregion

        #region CrossProduct
        [HttpPost]
        public IActionResult CrossProductupsert(Guid productguid, int relatedproductid, int relatedtype)
        {


            _producthelper.relatedmetadata(relatedproductid, relatedtype, productguid);


            return Json("success");
        }
        #endregion



        #region Coupon

        #region PartialViews
        public IActionResult CouponChildView(string couponchildtype)
        {
            List<CouponChildViewModel> couponChildList = new List<CouponChildViewModel>();

            if (couponchildtype == "8")
            {


                couponChildList = _producthelper.productmasterdataV2(0, "couponchildview", 500, 1)
                 .Select(p => new CouponChildViewModel
                 {
                     ReferenceId = p.ProductId,
                     ReferenceName = p.ProductName
                 })
                    .ToList();

            }
            else if (couponchildtype == "9")
            {
                couponChildList = _dbContext.CategoryMasters.OrderBy(u => u.CategoryName).Where(u => u.IsPublished == true).ToList()
                    .Select(p => new CouponChildViewModel
                    {
                        ReferenceId = p.CategoryId,
                        ReferenceName = p.CategoryName
                    })
                    .ToList();


            }

            else if (couponchildtype == "10")
            {
                couponChildList = _dbContext.UsersProfiles.Where(u => u.Type == "Vendor").ToList()
                .Select(p => new CouponChildViewModel
                {
                    ReferenceId = p.ProfileId,
                    ReferenceName = p.Firstname + " " + p.Lastname
                })
                    .ToList();

            }

            //ViewBag.MyString = myString;
            return PartialView("/Pages/Admin/Coupons/_CouponChild.cshtml", couponChildList);
        }
        #endregion


        #region CouponChildupdate


        [HttpPost]
        public async Task<IActionResult> CouponChild(int ReferenceId, int ProductCouponId, int ReferenceTypeID)
        {

            ProductCouponChild up = _dbContext.ProductCouponChildren.FirstOrDefault(u => u.ReferenceId == ReferenceId && u.ReferenceTypeID == ReferenceTypeID && u.ProductCouponId == ProductCouponId);


            if (up != null)
            {
                _dbContext.ProductCouponChildren.Remove(up);
            }
            else
            {
                ProductCouponChild pr = new ProductCouponChild();

                pr.ReferenceId = ReferenceId;
                pr.ReferenceTypeID = ReferenceTypeID;
                pr.InsertDate = DateTime.Now;
                pr.ProductCouponId = ProductCouponId;

                _dbContext.ProductCouponChildren.Add(pr);
            }



            _dbContext.SaveChanges();



            return Json("success");
        }


        #endregion
        #endregion



        #region SellingType
        [HttpGet]
        public IActionResult listingtypetypelist(int sellingtypeid)
        {
            var model = _producthelper.GetListingTypesBySellingType(sellingtypeid);

            return Json(model);
        }
        #endregion



        #region Product Data Import
        //[HttpGet]
        //public IActionResult classifiedDataImport()
        //{
        //    var model = _dbContext.ProductBasicV2s.ToList();

        //    foreach (var item in model)
        //    {
        //        string productdetail = _producthelper.ProductBasicmetadata(0, 0, item.ProductName, item.ShortDescription, item.CurrencyId, item.Price, item.ProductImage, item.Unit, item.BrandName, item.SeoMetaTitle, item.SeoMetadescription, item.SeoKeywords, item.CategoryArray.Replace("{", "").Replace("}", ""));
        //        //string classified = _producthelper.ProductClassifiedmetadata("0987654f3", "abc@gmail.com", item.Address);
        //        string detail = _producthelper.ProductDetailmetadata(item.DetailDescription, item.Sku, item.Eancode);



        //        var json = new ItemJsonList();
        //        if (productdetail != null)
        //        {
        //            json.ProductBasicMetaData = JsonConvert.DeserializeObject<ProductBasicMetaData>(productdetail);
        //        }

        //        //if (classified != null)
        //        //{
        //        //    json.ProductClassifiedMetaData = JsonConvert.DeserializeObject<ProductClassifiedMetaData>(classified);
        //        //}

        //        ItemListing insert = new ItemListing();

        //        insert.IsPublish = true;
        //        insert.InsertDate = DateTime.Now;
        //        insert.ItemMetaData = JsonConvert.SerializeObject(json);
        //        insert.ItemDetailMetaData = detail;
        //        insert.ProfileId = 1329;
        //        _dbContext.Add(insert);
        //        _dbContext.SaveChanges();

        //        TempData["success"] = "Item created successfully";
        //    }

        //    return Json(model);
        //}
        #endregion


        #region HeaderSearch
        [HttpGet]
        public IActionResult HeaderSearch(string searchText)
        {
            try
            {
                var productResults = (from p in _producthelper.productmasterdataV2(0, "headersearch", 20, 1)
                                      where p.ProductName.ToLower().Contains(searchText.ToLower())
                                      select new
                                      {
                                          urlpath = "/item/" + p.ProductSeourl,
                                          name = p.ProductName,
                                          Type = "Product"
                                      }).Take(20).ToList();

                var categoryResults = (from c in _dbContext.CategoryMasters
                                       where c.CategoryName.ToLower().Contains(searchText.ToLower())
                                       select new
                                       {
                                           urlpath = "/item/search?categoryid=" + c.CategoryId,
                                           name = c.CategoryName,
                                           Type = "Category"
                                       }).ToList();

                var keyword = new
                {
                    urlpath = "/item/search?q=" + searchText.ToLower(),
                    name = searchText.ToLower(),
                    Type = "Keyword"
                };

                var results = new List<object>();
                results.Add(keyword);
                results.AddRange(productResults);
                results.AddRange(categoryResults);

                return new JsonResult(results);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting HeaderSearch");
            }
        }
        #endregion


        #region ProductQuestion

        [HttpPost]

        public IActionResult PostQuestion(int userid, int productid, string question, int sellerid)

        {
            try
            {
                ProductQuestion pq = new ProductQuestion();




                pq.ProductId = productid;
                pq.ProfileId = userid;
                pq.Question = question;
                pq.InsertDate = DateTime.Now;
                pq.IsActive = true;

                _dbContext.ProductQuestions.Add(pq);
                _dbContext.SaveChanges();


                ///add notification
                ///

              
                _notificationHelper.NotificationSet(sellerid, "Question Posted On Your Item", question, "", "/listing/inquirylist?iteminquiryid="+ pq.ProductQaid);

                return new JsonResult("success");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting postquestion");
            }
        }

        [HttpPost]

        public IActionResult PostQuestionAnswer(int userid, int questionid, string answer, int buyerid, string redirecturl)

        {
            try
            {
                //validate if answer already exist update it


                ProductQuestionAnswer pavalidate = _dbContext.ProductQuestionAnswers.FirstOrDefault(u => u.ProductQaid == questionid);

                if(pavalidate!=null)
                {
                    pavalidate.Qanswer = answer;
                    pavalidate.ProfileId = userid;
                    pavalidate.InsertDate = DateTime.Now;
                    _dbContext.SaveChanges();
                }
                else
                {
                    ProductQuestionAnswer pa = new ProductQuestionAnswer();




                    pa.ProductQaid = questionid;
                    pa.ProfileId = userid;
                    pa.Qanswer = answer;
                    pa.InsertDate = DateTime.Now;
                    pa.IsActive = true;

                    _dbContext.ProductQuestionAnswers.Add(pa);
                    _dbContext.SaveChanges();
                }

              


                _notificationHelper.NotificationSet(buyerid, "You got an update on your question!", answer, "", redirecturl);

                return new JsonResult("success");
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting postanswer");
            }
        }

        public IActionResult QuestionListByItemId(int ItemID, int startIndex)
        {
           var ViewModelsList = _producthelper.productqa(ItemID);

            if (ViewModelsList == null)
            {
                string errorUrl = "FAIL:";
                return Content(errorUrl);
            }
            else
            {
                // Determine the number of records to fetch
                int takeCount = 10;

                // Filter and take the next set of records based on startIndex and takeCount
                var paginatedList = ViewModelsList.Skip(startIndex).Take(takeCount).ToList();

                return PartialView("/Pages/listing/_product-qa.cshtml", paginatedList);
            }
        }

        #endregion




        #region Categories


        //public static IEnumerable<CategorViewModel> BuildCategoryHierarchy(List<CategorViewModel> categories, int parentId, int level)
        //{
        //    var subcategories = categories.Where(c => c.ParentCategoryId == parentId).ToList();

        //    foreach (var subcategory in subcategories)
        //    {
        //        subcategory.LevelNumber = level; // Assign the current level to the subcategory
        //        yield return subcategory;

        //        foreach (var childCategory in BuildCategoryHierarchy(categories, subcategory.CategoryId, level + 1))
        //        {
        //            childCategory.CategoryName = subcategory.CategoryName + ">" + childCategory.CategoryName;
        //            yield return childCategory;
        //        }
        //    }




        //}

      
        public static IEnumerable<CategorViewModel> BuildCategoryHierarchy(List<CategorViewModel> categories, int parentId, string indentation)
        {
            var subcategories = categories.Where(c => c.ParentCategoryId == parentId).ToList();

            foreach (var subcategory in subcategories)
            {
                subcategory.LevelNumber = indentation.Count(c => c == '\t') + 1; // Calculate the level based on indentation
                yield return subcategory;

                foreach (var childCategory in BuildCategoryHierarchy(categories, subcategory.CategoryId, indentation + "\t"))
                {
                    yield return childCategory;
                }
            }
        }

       
        public IActionResult AllCategoryLevels(int categoryTypeId)
        {
            try
            {
                List<CategorViewModel> categoryMaster = (from cm in _dbContext.CategoryMasters
                                                         orderby cm.Sortnumber
                                                         where cm.IsPublished == true && cm.ListingTypeID == categoryTypeId
                                                         && cm.IsDeleted==false
                                                         select new CategorViewModel
                                                         {
                                                             CategoryId = cm.CategoryId,
                                                             ParentCategoryId = cm.ParentCategoryId,
                                                             Urlpath = cm.Urlpath + "-" + cm.CategoryId,
                                                             Icon = cm.Icon,
                                                             CategoryName = cm.CategoryName,
                                                             Sortnumber = cm.Sortnumber,
                                                             LevelNumber = 0 // Initialize to 0
                                                         }).ToList();

                var hierarchicalCategories = BuildCategoryHierarchy(categoryMaster, 0, "").ToList();

                return new JsonResult(hierarchicalCategories);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting AllCategoryLevels");
            }
        }


        //[HttpGet("categoryfirstlevel/{categorytypeid}")]
        public IActionResult categoryfirstlevel(int categorytypeid)
        {
            try
            {
                List<CategoryMaster> categorymaster = new List<CategoryMaster>();


                categorymaster = (from cm in _dbContext.CategoryMasters
                                  orderby cm.Sortnumber
                                  where cm.IsPublished == true && cm.ParentCategoryId == 0 && cm.ListingTypeID == categorytypeid &&
                                  cm.IsDeleted==false
                                  select new CategoryMaster
                                  {
                                      CategoryId = cm.CategoryId,
                                      Urlpath = cm.Urlpath + "-" + cm.CategoryId,
                                      Icon = cm.Icon,
                                      CategoryName = cm.CategoryName,
                                      Sortnumber = cm.Sortnumber
                                  }).ToList();





                return new JsonResult(categorymaster);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting CountryList");
            }
        }

        //[HttpGet("categorysecondlevel/{categoryid}")]
        public IActionResult categorysecondlevel(int categoryid)
        {
            try
            {
                List<CategoryMaster> categorymaster = new List<CategoryMaster>();

                categorymaster = (from cm in _dbContext.CategoryMasters
                                  orderby cm.Sortnumber
                                  where cm.IsPublished == true && cm.ParentCategoryId!=0&& cm.ParentCategoryId == categoryid &&
                                   cm.IsDeleted == false
                                  select new CategoryMaster
                                  {
                                      CategoryId = cm.CategoryId,
                                      Urlpath = cm.Urlpath + "-" + cm.CategoryId,
                                      Icon = cm.Icon,
                                      CategoryName = cm.CategoryName,
                                      Sortnumber = cm.Sortnumber
                                  }).ToList();




                return new JsonResult(categorymaster);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting CountryList");
            }
        }


        //[HttpGet("categorythirdlevel/{categoryid}")]
        public IActionResult categorythirdlevel(int categoryid)
        {
            try
            {
                List<CategoryMaster> categorymaster = new List<CategoryMaster>();

                categorymaster = (from cm in _dbContext.CategoryMasters
                                  orderby cm.Sortnumber
                                  where cm.IsPublished == true && cm.ParentCategoryId != 0 && cm.ParentCategoryId == categoryid &&
                                   cm.IsDeleted == false
                                  select new CategoryMaster
                                  {
                                      CategoryId = cm.CategoryId,
                                      Urlpath = cm.Urlpath + "-" + cm.CategoryId,
                                      Icon = cm.Icon,
                                      CategoryName = cm.CategoryName,
                                      Sortnumber = cm.Sortnumber
                                  }).ToList();




                return new JsonResult(categorymaster);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting CountryList");
            }
        }



        [HttpGet("categorybyid/{categoryid}")]
        public IActionResult CategoryByID(int categoryid)
        {
            try
            {
                List<CategoryMaster> categorymaster;

                var query = (from cm in _dbContext.CategoryMasters
                             orderby cm.Sortnumber
                             where cm.IsPublished == true
                             select new CategoryMaster
                             {
                                 CategoryId = cm.CategoryId,
                                 Urlpath = cm.Urlpath + "-" + cm.CategoryId,
                                 Icon = cm.Icon,
                                 CategoryName = cm.CategoryName,
                                 Sortnumber = cm.Sortnumber,
                                 ParentCategoryId = cm.ParentCategoryId
                             });

                query = query.Where(u => u.ParentCategoryId == categoryid);

                if (query.Count() <= 0)
                {
                    query = _dbContext.CategoryMasters.Where(u => u.ParentCategoryId == 0);

                }

                categorymaster = query.ToList();


                return new JsonResult(categorymaster);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting CategoryByID");
            }
        }


        #endregion



        #region HomePageDesign-ConditionData

        [ResponseCache(Duration = 86400)]
        public IActionResult ItemHomePageConditionalData(string datatype)
        {
            try
            {
                List<ItemHomePageDesignSelectionMetaData> model = new List<ItemHomePageDesignSelectionMetaData>();

                if (datatype == "category")
                {



                    List<CategorViewModel> categoryMaster = (from cm in _dbContext.CategoryMasters
                                                             orderby cm.Sortnumber
                                                             where cm.IsPublished == true
                                                             select new CategorViewModel
                                                             {
                                                                 CategoryId = cm.CategoryId,
                                                                 ParentCategoryId = cm.ParentCategoryId,
                                                                 Urlpath = cm.Urlpath + "-" + cm.CategoryId,
                                                                 Icon = cm.Icon,
                                                                 CategoryName = cm.CategoryName,
                                                                 Sortnumber = cm.Sortnumber,
                                                                 LevelNumber = 0 // Initialize to 0
                                                             }).ToList();

                    var hierarchicalCategories = BuildCategoryHierarchy(categoryMaster, 0, "").ToList();


                    model = hierarchicalCategories
                        .Select(cm => new ItemHomePageDesignSelectionMetaData
                        {
                            SelectionID = cm.CategoryId,
                            Name = cm.CategoryName + " Level:" + cm.LevelNumber,
                        })
                        .ToList();
                }

                if (datatype == "product")
                {


                   
                    var product = _producthelper.productmasterdataV2(0, "homepagedesign", 200, 1).ToList();
                    //model = product
                    //    .Select(cm => new ItemHomePageDesignSelectionMetaData
                    //    {
                    //        SelectionID = cm.ProductId,
                    //        Name = cm.CategoryName + " Level:" + cm.LevelNumber,
                    //    })
                    //    .ToList();
                }

                return PartialView("/Pages/admin/homepagesetup/_customizedsetup.cshtml", model);

                //return Json(model); // You can directly return JSON here
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting itemhomepageConditionalData");
            }
        }


        #region ChildDataSelection
        [HttpPost]
        public IActionResult ItemHomePageChildupsert(int itemdesignid, int selectionid, string selectiontype)
        {


            _producthelper.homagepageDesignChild(itemdesignid, selectionid, selectiontype);


            return Json("success");
        }
        #endregion
        #endregion



        #region AmenitiesUpsert


        [HttpPost]
        public IActionResult amenitiesupsert(int loginid, string question, string type, int sort, bool ispublish, int ProductAmenitiesId)
        {
            #region CustomAssignment
            string amenityid;

            #endregion

            #region ModelValidation
            if (ProductAmenitiesId == 0)
            {
                ProductAmenitiesQuestionV2 productamenityQuestion = new ProductAmenitiesQuestionV2();

                productamenityQuestion.ProfileId = loginid;
                productamenityQuestion.InsertDate = DateTime.Now;

                productamenityQuestion.ProductAmenitiesHeading = question;
                productamenityQuestion.ControlType = type;
                productamenityQuestion.SortNumber = sort;
                productamenityQuestion.Type = "Amenities";
                productamenityQuestion.IsPublish = true;

                _dbContext.ProductAmenitiesQuestionV2s.Add(productamenityQuestion);
                _dbContext.SaveChanges();
                amenityid = productamenityQuestion.ProductAmenitiesId.ToString();

            }
            else
            {
                ProductAmenitiesQuestionV2 up = _dbContext.ProductAmenitiesQuestionV2s.FirstOrDefault(u => u.ProductAmenitiesId == ProductAmenitiesId);

                up.ProductAmenitiesHeading = question;
                up.Type = type;
                up.SortNumber = sort;
                up.IsPublish = ispublish;

                _dbContext.ProductAmenitiesQuestionV2s.Update(up);
                _dbContext.SaveChanges();
                amenityid = up.ProductAmenitiesId.ToString();

            }

            #endregion



            return Json(amenityid);
        }

        #endregion

        #region OptionView

        public IActionResult Amenitiesoptionview(int ProductAmenitiesId)
        {

            List<ProductAmenitiesOptionsV2> attributeoptionlist = _dbContext.ProductAmenitiesOptionsV2s.Where(u => u.ProductAmenitiesId == ProductAmenitiesId).ToList();




            return PartialView("/Pages/admin/Amenities/_amenityoption.cshtml", attributeoptionlist);


        }
        #endregion

        #region OptionUpsert
        [HttpPost]

        public IActionResult Amenityoptionusert(string optiontext, int attributeguid, int attributeoptionid, string image)
        {
            #region ModelValidation

            if (!string.IsNullOrEmpty(optiontext))
            {
                string[] optionValues = optiontext.Split(',').Select(value => value.Trim()).ToArray();


                foreach (string optionValue in optionValues)
                {

                    if (!string.IsNullOrEmpty(optionValue))
                    {
                        ProductAmenitiesOptionsV2 productAttributeOption = new ProductAmenitiesOptionsV2();

                        //productAttributeOption.InsertDate = DateTime.Now;
                        //productAttributeOption.IsCorrect = true;
                        productAttributeOption.ProductAmenitiesName = optionValue.Trim(); // Trim to remove leading/trailing spaces
                        //productAttributeOption.Attributeprice = price;
                        //productAttributeOption.ProductAttributeGuid = attributeguid;
                        //productAttributeOption.ProductAttributeOptionId = attributeoptionid;
                        productAttributeOption.ProductAmenitiesIcon = image;
                        productAttributeOption.ProductAmenitiesId = attributeguid;
                        productAttributeOption.ProductAmenitiesOptionId = attributeoptionid;
                        productAttributeOption.IsPublish = true;



                        if (productAttributeOption.ProductAmenitiesOptionId == 0)
                        {
                            // Insert the new option
                            _dbContext.ProductAmenitiesOptionsV2s.Add(productAttributeOption);
                        }
                        else
                        {
                            // Update the existing option
                            var existingOption = _dbContext.ProductAmenitiesOptionsV2s.Find(productAttributeOption.ProductAmenitiesOptionId);

                            if (existingOption != null)
                            {
                                existingOption.ProductAmenitiesName = productAttributeOption.ProductAmenitiesName;
                                //existingOption.Attributeprice = productAttributeOption.Attributeprice;
                                existingOption.ProductAmenitiesIcon = productAttributeOption.ProductAmenitiesIcon;
                                existingOption.ProductAmenitiesId = productAttributeOption.ProductAmenitiesId;

                                _dbContext.ProductAmenitiesOptionsV2s.Update(existingOption);
                            }
                        }
                    }
                }

                _dbContext.SaveChanges();
            }

            #endregion

            return Json("success");
        }
        #endregion

        #region AmenityDelete
        [HttpPost]
        public IActionResult amenitydel(int attributeguid)
        {


            ProductAmenitiesQuestionV2 productAttributeQuestion = _dbContext.ProductAmenitiesQuestionV2s.FirstOrDefault(u => u.ProductAmenitiesId == attributeguid);



            #region ModelValidation

            if (productAttributeQuestion != null)
            {

                var productAttributeOption = _dbContext.ProductAmenitiesOptionsV2s.Where(u => u.ProductAmenitiesId == attributeguid).ToList();

                foreach (var item in productAttributeOption)
                {
                    _dbContext.ProductAmenitiesOptionsV2s.Remove(item); // Remove individual items one by one
                }


                _dbContext.ProductAmenitiesQuestionV2s.Remove(productAttributeQuestion);
                _dbContext.SaveChanges();

            }




            #endregion

            return Json("success");
        }
        #endregion


        #region OptionDelete
        [HttpPost]
        public IActionResult Amenityoptiondelete(int attributeoptionid)
        {


            ProductAmenitiesOptionsV2 productAttributeOption = _dbContext.ProductAmenitiesOptionsV2s.FirstOrDefault(u => u.ProductAmenitiesOptionId == attributeoptionid);



            #region ModelValidation

            if (productAttributeOption != null)
            {


                _dbContext.ProductAmenitiesOptionsV2s.Remove(productAttributeOption);
                _dbContext.SaveChanges();

            }




            #endregion

            return Json("success");
        }

        #endregion

        #region ItemDuplicate
        public IActionResult DuplicateItem(Guid productguid)
        {
            // Find the item to duplicate based on the productguid
            ItemListing itemToDuplicate = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

            if (itemToDuplicate != null)
            {
                // Create a new instance of ItemListing to hold the duplicated item
                ItemListing duplicatedItem = new ItemListing
                {

                    IsPublish = false,
                    InsertDate = DateTime.Now,
                    ItemMetaData = itemToDuplicate.ItemMetaData,
                    AdminMetaData =itemToDuplicate.AdminMetaData,
                    ProfileId = itemToDuplicate.ProfileId, // Assuming loginid is accessible in this context
                    IsAdminLocked=itemToDuplicate.IsAdminLocked,
                    ItemDetailMetaData = itemToDuplicate.ItemDetailMetaData,
                    ItemPolicyMetaData  = itemToDuplicate.ItemPolicyMetaData,
                    ItemShippingMetaData   = itemToDuplicate.ItemShippingMetaData,
                    ItemImagesMetaData = itemToDuplicate.ItemImagesMetaData,
                    AmenitiesMetaData = itemToDuplicate.AmenitiesMetaData,
                    RelatedItemMetaData = itemToDuplicate.RelatedItemMetaData,
                    VideoItemMetaData = itemToDuplicate.VideoItemMetaData,
                    InventoryItemMetaData = itemToDuplicate.InventoryItemMetaData,  
                    ItemOtherMetaData = itemToDuplicate.ItemOtherMetaData,
                    ItemDigitalMetaData = itemToDuplicate.ItemDigitalMetaData,
                    SellerDiscountMetaData  = itemToDuplicate.SellerDiscountMetaData,
                
                };

                // Add the duplicated item to the database context
                _dbContext.ItemListings.Add(duplicatedItem);

                // Save changes to persist the duplicated item in the database
                _dbContext.SaveChanges();

                // Return a success response with the GUID of the duplicated item
                return Json(new { success = true, ItemListingGUIDOptionalModel = duplicatedItem.ItemGuid });

                
            }
            else
            {
                // Item not found with the provided productguid
                return NotFound();
            }
        }
        #endregion


        #region SellerListing
        public IActionResult SellerListing(string searchstring)
        {
            // Find the item to duplicate based on the productguid
            var model = _userhelper.SellerList().Where(u=>u.Type== "Vendor").ToList();

            return PartialView("/Pages/seller/_sellerlisting.cshtml", model);

            //return Json(new { success = true, totalrecords=0 });



        }
        #endregion
    }
}
