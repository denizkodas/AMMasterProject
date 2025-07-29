using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using BitMiracle.LibTiff.Classic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Stripe;
using System.Collections.Generic;
using System.Globalization;
using static AMMasterProject.Helpers.GlobalHelper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AMMasterProject.Pages.Listing.create
{
    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class basicModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;
        private readonly GlobalHelper _globalHelper;
        private readonly WebsettingHelper _websettinghelper;
        public Guid productguid { get; set; }

      




      
        public List<ProductOtherPropertiesForListingViewModel> listotherproperties { get; set; }

        //public ProductOtherPropertiesForListingViewModel otherproperties { get; set; }
        public List<GeneralSetup> GeneralSetups { get; set; }


        //public ProductBasicV2 ProductBasicV2 { get; set; }

        public ProductBasicViewModel product { get; set; }
        public List<SellingTypeMetaData>  sellingtype { get; set; }

      
        public IEnumerable<SelectListItem> Currency { get; set; }
        public IEnumerable<SelectListItem> CatgoryList { get; set; }



        public int loginid { get; set; }
        #endregion

        #region DI






        public basicModel(MyDbContext context, ProductHelper productHelper, GlobalHelper globalHelper, WebsettingHelper websettinghelper)
        {
            _dbContext = context;
            _productHelper = productHelper;
            _globalHelper = globalHelper;
            _websettinghelper = websettinghelper;
            product = new ProductBasicViewModel();
            product.IsIncrementByUser = false;
            product.IsPublish = true;
            product.SellingType = "0";

            product.StartDate = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            product.EndDate = DateTime.ParseExact(DateTime.Now.Date.AddYears(1).ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
           
        }

        #endregion



        #region DataPopulate

        public void setup()
        {

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

            ///selling type list , Sell, Classified, Auction, Penny Auction
            ///
            sellingtype = _productHelper.GetSellingTypeList();


           

            Currency = _dbContext.Countries
                .Where(u => u.IsCurrencyPublish == true)
                .Select(u => new SelectListItem
                {
                    Value = u.CountryID.ToString(),
                    Text = u.CurrencySymbol + " " + u.CurrencyCode,
                   
                }).ToList();


            CatgoryList = _dbContext.CategoryMasters

                .OrderBy(u => u.Sortnumber)
               .Where(u => u.IsPublished == true && u.ParentCategoryId == 0 && u.IsDeleted == false)
               .Select(u => new SelectListItem
               {
                   Value = u.CategoryId.ToString(),
                   Text = u.CategoryName

               }).ToList();




            //load other properties design by admin
            var itemsJson = _websettinghelper.GetWebsettingJson("ItemOtherProperties");
            if (itemsJson != null)
            {
                listotherproperties = JsonConvert.DeserializeObject<List<ProductOtherPropertiesForListingViewModel>>(itemsJson);
            }
            else
            {
                listotherproperties = new List<ProductOtherPropertiesForListingViewModel>();
            }


        }



        #endregion
        public void OnGet()
        {



            setup();

            //load eidt mode

            if (Request.Query.ContainsKey("ID"))
            {
                productguid = Guid.Parse(Request.Query["ID"].ToString());

                ItemListing items = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);
                if (items == null)
                {
                    TempData["success"] = "Listing does not exist. You can create new listing.";
                    RedirectToPage("/listing/create/basic");
                }

                else
                {

                    var json = _productHelper.ItemJsonList(items.ItemMetaData);

                    ///assign the value
                    ///
                    product.ItemId = items.ItemId;
                    product.ItemGuid = Guid.Parse(items.ItemGuid.ToString());
                    product.SellingType = json.ProductBasicMetaData.SellingTypeID.ToString();
                    product.ListingType = json.ProductBasicMetaData.ListingTypeID.ToString();
                    product.Name = json.ProductBasicMetaData.Name;
                    product.Unit = json.ProductBasicMetaData.Unit;
                    product.BrandName = json.ProductBasicMetaData.Brand;
                    product.ShortDescription = json.ProductBasicMetaData.ShortDescription;
                    product.CurrencyId = json.ProductBasicMetaData.CurrencyId;
                    product.Price = json.ProductBasicMetaData.Price;
                    product.ProductImage = json.ProductBasicMetaData.Image;
                    product.CategoryArray = json.ProductBasicMetaData.ProductCategoryArray;
                    product.SeoMetaTitle = json.ProductBasicMetaData.SeoMetaTitle;
                    product.SeoKeywords = json.ProductBasicMetaData.SeoKeywords;
                    product.SeoMetadescription = json.ProductBasicMetaData.SeoMetadescription;



                    ///Classified
                    product.Address =json.ProductClassifiedMetaData?.Address;
                    product.ContactNumber=json.ProductClassifiedMetaData?.ContactNumber;
                    product.Email=json.ProductClassifiedMetaData?.Email;

                    //digital tab
                    ///depreciated and shifted to digital page 
                    //product.ProductDigitalMetaDataList =  json.ProductDigitalMetaData;

                    product.IsPublish = items.IsPublish;


                    // Load and merge other properties
                    if (!string.IsNullOrEmpty(items.ItemOtherProperites))
                    {
                        var existingOtherProperties = JsonConvert.DeserializeObject<List<ProductOtherPropertiesForListingViewModel>>(items.ItemOtherProperites);
                        foreach (var existingProp in existingOtherProperties)
                        {
                            var adminProp = listotherproperties.FirstOrDefault(p => p.ID == existingProp.ID);
                            if (adminProp != null)
                            {
                                adminProp.ValueName = existingProp.ValueName;
                            }
                        }
                    }



                }
            }
        }


        public IActionResult OnPost()
        {
            try
            {



                #region ID
                int loginid = 0;
                if (User.Identity.IsAuthenticated)
                {
                    loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                    // continue with loginid variable
                }



                #endregion
                #region ModelValidation
                if (product.ProductImage == null)
                {
                    ModelState.AddModelError("product.ProductImage", "Image is required");
                    setup();
                    return Page();
                }

                if (product.Price <= 0)
                {
                    ModelState.AddModelError("product.Price", "Price should be greater than 0");
                    setup();
                    return Page();
                }

                if (int.Parse(product.SellingType) < 0)
                {
                    ModelState.AddModelError("product.SellingType", "Please select selling type");
                    setup();
                    return Page();
                }

                if (int.Parse(product.ListingType) < 0)
                {
                    ModelState.AddModelError("product.ListingType", "Please select listing type");
                    setup();
                    return Page();
                }

                if (product.CategoryArray ==null )
                {
                    ModelState.AddModelError("product.CategoryArray", "Please select all categories");
                    setup();
                    return Page();
                }
                #endregion

                #region JsonCreator
                //json creator
                string productdetail = _productHelper.ProductBasicmetadata(int.Parse(product.SellingType.ToString()), int.Parse(product.ListingType.ToString()), product.Name, product.ShortDescription, product.CurrencyId, product.Price, product.ProductImage, product.Unit, product.BrandName, product.SeoMetaTitle, product.SeoMetadescription, product.SeoKeywords, product.CategoryArray);

                //string category = _productHelper.ProductCategorymetadata(product.CategoryArray);


                string classified = null;
                //string digitalfile = null;
                string auction = null;
                string pennyauction = null;
                string otherproperties = null;
                if (product.SellingType == "1")//classified
                {
                   

                    classified = _productHelper.ProductClassifiedmetadata(product.ContactNumber, product.Email, product.Address);
                }

                ///depreciated and shifted to digital page 
                //else if (product.ListingType == "1"  && product.ProductDigitalMetaData.DigitalLink!= null)
                //{
                //    digitalfile = _productHelper.ProductDigitalmetadata(product.ProductDigitalMetaData.DigitalLink, product.ProductDigitalMetaData.Name);

                //}
                else if (product.SellingType == "2" || product.SellingType == "3")//auction or penny auction
                {
                    auction = _productHelper.ProductAuctionmetadata(product.StartDate, product.EndDate, product.StartPrice, product.EndPrice, product.IsIncrementByUser, product.IncrementAmount);

                }

                else if (product.SellingType == "3") /// penny auction
                {
                    pennyauction = _productHelper.ProductPennyAuctionmetadata(product.NoofRealBids, "1,2,3");

                }


                var json = new ItemMetaDataUpsert();

                if (productdetail != null)
                {
                    json.ProductBasicMetaData = JsonConvert.DeserializeObject<ProductBasicMetaData>(productdetail);
                }

                //if (category != null)
                //{
                //    json.ProductCategoryMetaData = JsonConvert.DeserializeObject<List<ProductCategoryMetaData>>(category);
                //}

                if (classified != null)
                {
                    json.ProductClassifiedMetaData = JsonConvert.DeserializeObject<ProductClassifiedMetaData>(classified);
                }

                if (auction != null)
                {
                    json.ProductAuctionMetaData = JsonConvert.DeserializeObject<ProductAuctionMetaData>(auction);
                }

                if (pennyauction != null)
                {
                    json.ProductPennyAuctionMetaData = JsonConvert.DeserializeObject<ProductPennyAuctionMetaData>(pennyauction);
                }

                if (listotherproperties.Count > 0)
                {
                    
                        foreach (var property in listotherproperties)
                        {
                            if (string.IsNullOrWhiteSpace(property.ValueName))
                            {
                                ModelState.AddModelError("product.ProductOtherProperties", "All Value Name fields must be filled.");
                                setup();
                                return Page();
                            }
                        }
                    



                    otherproperties = JsonConvert.SerializeObject(listotherproperties);
                }

                ///depreciated and shifted to digital page 
                //if (digitalfile != null )
                //{


                //    json.ProductDigitalMetaData = JsonConvert.DeserializeObject<List<ProductDigitalMetaData>>(digitalfile);
                //}

                ///check if item is digital and there is no data in productdigitalcolumn so make it unpublish
                ///


                #endregion




                if (product.ItemId == 0)
                {
                    //Validate unpublish of digital item on insert mode
                    if (product.ListingType == "1")
                    {
                        product.IsPublish = false;
                    }

                    ItemListing insert = new ItemListing();

                    insert.IsPublish = product.IsPublish;
                    insert.InsertDate = DateTime.Now;
                    insert.ItemMetaData = JsonConvert.SerializeObject(json);
                    insert.ProfileId = loginid;
                    insert.ItemOtherProperites = otherproperties;



                    _dbContext.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Item created successfully";
                    

                    return RedirectToPage("/listing/create/description", new { ID = insert.ItemGuid });
                }
                else
                {
                    ItemListing update = _dbContext.ItemListings.FirstOrDefault(u => u.ItemId == product.ItemId);
                    if (update != null)
                    {

                        //Validate unpublish of digital item if there is no digital item 
                        if (product.ListingType == "1" && update.ItemDigitalMetaData ==null )
                        {
                            product.IsPublish = false;
                        }

                        update.IsPublish = product.IsPublish;
                        update.ItemMetaData = JsonConvert.SerializeObject(json);
                        update.ItemOtherProperites = otherproperties;

                        _dbContext.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Item updated successfully";


                        return RedirectToPage("/listing/create/description", new { ID = update.ItemGuid });
                        
                    }
                }

                setup();
                return Page();

            }
            catch (Exception ex)
            {
                TempData["success"] = ex.Message;
                setup();
                return Page();
            }

        }
    }
}
