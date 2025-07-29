
using Amazon.Runtime.Internal.Transform;
using AMMasterProject.Migrations;
using AMMasterProject.Models;
using AMMasterProject.ViewModel;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using Stripe;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using static AMMasterProject.Helpers.GlobalHelper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace AMMasterProject.Helpers
{
    public class ProductHelper
    {

        #region DI



        private readonly MyDbContext _dbContext;
        private readonly GlobalHelper _globalhelper;

        public int TotalRecord { get; set; }
        public ProductHelper(MyDbContext context, GlobalHelper globalhelper)
        {
            _dbContext = context;
            _globalhelper = globalhelper;
        }
        #endregion


        #region Setting




        public List<HomePageFirstLevel> itemsettingv2()
        {


            var q = _dbContext.ItemPageDesign

                .Where(firstlevel => firstlevel.Ispublish == true)
                .OrderBy(u => u.SortOrder)
                .Select(firstlevel => new HomePageFirstLevel
                {
                    Title = firstlevel.Title,
                    ItemDesignMetaData = ParseMetaDataItemHomePageDesign(firstlevel.PageDesignMetaData),
                    ItemPageSecondLevel = _dbContext.ItemPageDesignChild
                        .Where(secondlevel => secondlevel.ItemPageDesignID == firstlevel.ItemPageDesignID)
                        .Select(secondlevel => new HomePageSecondLevel
                        {
                            SelecttionID = secondlevel.SelecttionID,
                        })
                        .ToList()
                })
                .ToList();

            // Now, based on ItemDesignMetaData.PreselectedCategory, filter and populate productViewModel
            foreach (var firstlevel in q)
            {

                int page = 1;
                if (firstlevel.ItemDesignMetaData.PreselectedCategory == "random")
                {
                    var productData = productmasterdataV2(0, "home", 30, page, 0, "");
                    page = page + 1;
                    // Show random records
                    firstlevel.productviewModel = productData
                        .OrderBy(x => Guid.NewGuid()) // Randomize the order
                        .Take(firstlevel.ItemDesignMetaData.NoofItemsDisplay)
                        .ToList();
                }
                if (firstlevel.ItemDesignMetaData.PreselectedCategory == "onlybanner")
                {

                }
                else if (firstlevel.ItemDesignMetaData.PreselectedCategory == "product")
                {
                    foreach (var secondlevel in firstlevel.ItemPageSecondLevel)
                    {
                        var products = productmasterdataV2(0, "idwise", 0, 1, secondlevel.SelecttionID).ToList();

                        if (firstlevel.productviewModel == null)
                        {
                            firstlevel.productviewModel = products;
                        }
                        else
                        {
                            firstlevel.productviewModel.AddRange(products);
                        }
                    }
                }
            }

            return q;
        }



        #endregion
        #region MasterData

        //Filter Type= Random
        public List<ItemJsonList> ItemJsonList(string dataFor, int pagesize, int pagenumber, int ItemId, string seourl, int loginid)//sellerid-itemid is to locate not to show the same seller id
        {
            var modelList = new List<ItemJsonList>();



            var query = from item in _dbContext.ItemListings

                        select new ItemJsonList
                        {
                            ID = item.ItemId,
                            ItemGUID = (Guid)item.ItemGuid,
                            IsPublish = item.IsPublish,
                            InsertDate = (DateTime)item.InsertDate,
                            ProductBasicMetaData = item.ItemMetaData != null ? ParseMetaDataProductBasic(item.ItemMetaData) : null,
                            ProfileID = item.ProfileId,
                            IsAdminLocked = item.IsAdminLocked,
                            ImagesMetaData = item.ItemImagesMetaData,
                            ProductDetailMetaData = item.ItemDetailMetaData != null ? ParseMetaDataProductDetail(item.ItemDetailMetaData) : null,
                            AmenityMetaData = item.AmenitiesMetaData,
                            productPolicyMetaData = item.ItemPolicyMetaData != null ? ParseMetaDataProductPolicy(item.ItemPolicyMetaData) : null,
                            productShippingMetaData = item.ItemShippingMetaData != null ? ParseMetaDataProductShipping(item.ItemShippingMetaData) : null,
                            VideoMetaData = item.VideoItemMetaData,
                            productInventoryMetaData = item.InventoryItemMetaData != null ? ParseMetaDataProductInventory(item.InventoryItemMetaData) : null,
                            ItemOtherMetaData = item.ItemOtherMetaData != null ? ParseMetaDataItemMetaData(item.ItemOtherMetaData) : null,
                            ProductDigitalMetaData = item.ItemDigitalMetaData != null ? ParseMetaDataProductDigital(item.ItemDigitalMetaData) : null,
                            ProductClassifiedMetaData = item.ItemMetaData != null ? ParseMetaDataProductClassified(item.ItemMetaData) : null,

                            ProductRelatedMetaData = item.RelatedItemMetaData != null ? ParseMetaDataProductRelated(item.RelatedItemMetaData) : null,
                            IsAttributeExist = _dbContext.ProductAttributeQuestionV2s.Where(u=>u.ProductGuid ==item.ItemGuid).Count(),


                            SellerDiscountMetaData=item.SellerDiscountMetaData

                        };



            if (dataFor == "home")
            {

                query = query.Where(u => u.IsPublish == true).OrderBy(_ => Guid.NewGuid()); // Randomize the order

                modelList = query
                    .Skip((pagenumber - 1) * pagesize)
                    .Take(pagesize).ToList();

            }

            else if (dataFor == "discount")
            {


                query = query.Where(u => u.IsPublish == true && u.SellerDiscountMetaData !=null).OrderBy(_ => Guid.NewGuid()); // Randomize the order

                modelList = query
                    .Skip((pagenumber - 1) * pagesize)
                    .Take(pagesize).ToList();



            }

            else if (dataFor == "boost")
            {
                ///call boost item list 
                var productboostlist = (from pb in _dbContext.ProductBoosts
                                        join order in _dbContext.OrderMasters on pb.InvoiceNumber equals order.InvoiceNumber
                                        orderby Guid.NewGuid()
                                        where pb.StartDate <= DateTime.Now && pb.EndDate >= DateTime.Now &&
                                         pb.BoostType == "item" && order.PaymentStatus =="paid"
                                        select pb
                                        ).ToList();
                //generate its view
                foreach (var item in productboostlist)
                {

                   
                   BoostrMetaDataUpdate(item.ProductBoostId, "0", "view");
                    
                }

                //then creates it view
                modelList = (from pb in productboostlist
                             join p in query on pb.ItemBoostGUID equals p.ItemGUID
                           
                           
                             select p
                                 ).Where(u => u.IsPublish == true).ToList();







            }
            else if (dataFor == "search")
            {
                modelList = query.Where(u => u.IsPublish == true).ToList();

                // Split the query string into key-value pairs
                var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(seourl);


                if (queryParams.ContainsKey("address"))
                {
                    string distancetype = queryParams["distancetype"].ToString();
                    string address = queryParams["address"].ToString();
                    int distance = int.Parse(queryParams["distance"].ToString());




                    double queryLatitude = 0;
                    double queryLongitude = 0;
                    if (address != null && address != string.Empty)
                    {
                        GeocodeResult geocodeResult = _globalhelper.GetGeocodeDetails(address);
                        queryLatitude = geocodeResult.Latitude;
                        queryLongitude = geocodeResult.Longitude;
                    }

                    modelList = modelList
      .Where(p =>
          p.ProductClassifiedMetaData != null && // Check if ProductClassifiedMetaData is not null
          p.ProductClassifiedMetaData.Address != null && // Check if Address is not null
          (address.Contains(p.ProductClassifiedMetaData.Address) || // Check if Address contains the specified value
          (p.ProductClassifiedMetaData.Latitude != null && p.ProductClassifiedMetaData.Longitude != null &&
              GlobalHelper.CalculateDistance(
                  queryLatitude, queryLongitude,
                  double.Parse(p.ProductClassifiedMetaData.Latitude),
                  double.Parse(p.ProductClassifiedMetaData.Longitude), distancetype) <= distance)))
      .ToList();



                }



                if (queryParams.ContainsKey("productseourl"))
                {
                    string productseourl = queryParams["productseourl"];
                    modelList = modelList.Where(p => p.ProductBasicMetaData.SEOURL == productseourl.ToLower()).ToList();
                }

                if (queryParams.ContainsKey("q"))
                {
                    string keyword = queryParams["q"];
                    modelList = modelList.Where(p =>
                        p.ProductBasicMetaData.Name.ToLower().Contains(keyword.ToLower()) ||
                        (p.ProductBasicMetaData.Unit != null && p.ProductBasicMetaData.Unit.ToLower().Contains(keyword.ToLower())) ||
                        (p.ProductBasicMetaData.Brand != null && p.ProductBasicMetaData.Brand.ToLower().Contains(keyword.ToLower()))
                    ).ToList();
                }



                if (queryParams.ContainsKey("categoryid"))
                {
                    //int categoryId = int.Parse(queryParams["categoryid"].ToString());

                    string categoryIds = queryParams["categoryid"].ToString();
                    string[] categoryIdArray = categoryIds.Split(',');

                    modelList = modelList
                        .Where(p =>
                            categoryIdArray.Any(categoryId => ProductCategorymetadataList(p.ProductBasicMetaData.ProductCategoryArray)
                                .Any(c => c.CategoryId == Int32.Parse(categoryId))))
                        .ToList();


                }


                if (queryParams.ContainsKey("sellingtype"))
                {
                    string producttype = queryParams["sellingtype"];

                    //productlist = productlist.Where(r => vproducttype.ToLower().Contains(r.ListingType.ToLower())).ToList();


                    modelList = modelList.Where(p => producttype.ToLower().Contains(GetSellingTypeNameById(p.ProductBasicMetaData.SellingTypeID).ToLower())).ToList();
                }

                if (queryParams.ContainsKey("producttype"))
                {
                    string sellingtype = queryParams["producttype"];

                    modelList = modelList.Where(p => sellingtype.ToLower().Contains(GetListingTypeNameBySellingType(p.ProductBasicMetaData.SellingTypeID, p.ProductBasicMetaData.ListingTypeID).ToLower())).ToList();

                }

                if (queryParams.ContainsKey("rating"))
                {
                    string rating = queryParams["rating"];

                    //productlist = productlist.Where(r => vproducttype.ToLower().Contains(r.ListingType.ToLower())).ToList();


                    //modelList = modelList.Where(p => rating.Contains(p.ItemOtherMetaData.ItemAverageRating.ToString())).ToList();


                    modelList = modelList.Where(p => p.ItemOtherMetaData == null || rating.Contains(p.ItemOtherMetaData.ItemAverageRating.ToString())).ToList();

                }

                if (queryParams.TryGetValue("minprice", out var minPriceValue) &&
                    queryParams.TryGetValue("maxprice", out var maxPriceValue) &&
                     decimal.TryParse(minPriceValue, out var fromprice) &&
                    decimal.TryParse(maxPriceValue, out var toprice))
                {

                    var userselectedcurrency = _globalhelper.GetUserCurrency();
                    var conversionrate = _globalhelper.ConversionRate(userselectedcurrency);

                    modelList = modelList
                        .Where(p => Math.Round((decimal)p.ProductBasicMetaData.Price * conversionrate, 2) >= fromprice && Math.Round((decimal)p.ProductBasicMetaData.Price * conversionrate, 2) <= toprice)
                        .ToList();
                }

                TotalRecord = modelList.Count();

                int getrecords = pagenumber * pagesize;

                modelList = modelList.Take(getrecords).ToList();

                //depreciated on 17 nov 2023
                //modelList = modelList.Skip((pagenumber - 1) * pagesize)
                //    .Take(pagesize).ToList();

            }
            else if (dataFor == "headersearch")
            {

                modelList = query.Where(u => u.IsPublish == true).AsEnumerable()
                 .OrderBy(u => u.ProductBasicMetaData.Name).Skip((pagenumber - 1) * pagesize)
                    .Take(pagesize)
                 .ToList();






            }

            else if (dataFor == "idwise")
            {
                modelList = query.Where(u => u.ID == ItemId && u.IsPublish == true).ToList();

            }

            else if (dataFor == "guididwise")
            {
                Guid itemguid = Guid.Parse(seourl);
                modelList = query.Where(u => u.ItemGUID == itemguid && u.IsPublish == true).ToList();

            }

            else if (dataFor == "seourl")
            {
                modelList = query.Where(u => u.IsPublish == true).AsEnumerable()
                  .Where(u => u.ProductBasicMetaData.SEOURL + "-" + u.ID == seourl)
                  .ToList();

            }

            else if (dataFor == "favorite")
            {


                modelList = (from pb in _dbContext.CustomerWishlists
                             join p in query on pb.ProductId equals p.ID

                             where pb.UserId == loginid
                             select p
                                 ).Where(u => u.IsPublish == true).ToList();

            }

            else if (dataFor == "wishlistform")
            {


                modelList = (from pb in _dbContext.CustomerWishlists
                             join p in query on pb.ProductId equals p.ID

                             where pb.UserId == loginid
                             select p
                                 ).Where(u => u.IsPublish == true).ToList();

            }


            else if (dataFor == "shopurlpath")
            {
                modelList = (from pb in _dbContext.UsersProfiles
                             join p in query on pb.ProfileId equals p.ProfileID



                             where pb.BusinessUrlpath == seourl ||
                                  (pb.ProfileGuid.ToString() == seourl)
                             select p
                                 ).Where(u => u.IsPublish == true).ToList();
            }

            else if (dataFor == "couponchildview")
            {
                modelList = query.ToList();
            }

            else if (dataFor == "cross")
            {
                modelList = query.AsEnumerable()
               .OrderBy(u => u.ProductBasicMetaData.Name).Skip((pagenumber - 1) * pagesize)
                  .Take(pagesize)
               .ToList();

            }

            else if (dataFor == "homepagedesign")
            {

                modelList = query.Where(u => u.IsPublish == true).ToList();
            }

            else if (dataFor == "vendorwise")
            {

                modelList = query.ToList();
            }


            else if (dataFor == "related_detail")
            {




                // string[] keywords = seourl.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


                //modelList = query.AsEnumerable()
                //    .Where(u => u.IsPublish == true && keywords.Any(keyword => u.ProductBasicMetaData.Name.ToLower().Contains(keyword)))
                //    .OrderBy(u => Guid.NewGuid()) // Replace with your sorting key
                //    .Take(12)
                //.ToList();


                // Materialize the results of the first query
                //var queryResults = query.ToList();
                // Extract distinct related product IDs
                var distinctRelatedProductIds = query.Where(u => u.ID == ItemId).AsEnumerable()
      .SelectMany(item => item.ProductRelatedMetaData ?? Enumerable.Empty<ProductRelatedMetaData>())
      .Where(relatedItem => relatedItem.RelatedType == 0)
      .Select(relatedItem => relatedItem.RelatedProductid)
      .Distinct()
      .ToList();

                if (distinctRelatedProductIds.Count() > 0)
                {
                    // Inner join with the distinct related product IDs
                    modelList = query.AsEnumerable()
                        .Where(item => distinctRelatedProductIds.Contains(item.ID))
                        .OrderBy(_ => Guid.NewGuid()) // Shuffle the results randomly
                        .Take(16)
                        .ToList();
                }
                else
                {

                    // Split the seourl into individual keywords
                    string[] keywords = seourl.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Filter and order the data
                    modelList = query.AsEnumerable()
                        .Where(u => u.IsPublish == true && keywords.Any(keyword => u.ProductBasicMetaData.Name.ToLower().Contains(keyword)))
                        .OrderBy(u => Guid.NewGuid()) // Replace with your sorting key
                        .Take(12)
                        .ToList();
                }


            }


            else if (dataFor == "cross_detail")
            {
                // Materialize the results of the first query
                //var queryResults = query.ToList();
                // Extract distinct related product IDs
                var distinctRelatedProductIds = query.Where(u => u.ID == ItemId).AsEnumerable()
      .SelectMany(item => item.ProductRelatedMetaData ?? Enumerable.Empty<ProductRelatedMetaData>())
      .Where(relatedItem => relatedItem.RelatedType == 1)
      .Select(relatedItem => relatedItem.RelatedProductid)
      .Distinct()
      .ToList();

                if (distinctRelatedProductIds.Count() > 0)
                {
                    // Inner join with the distinct related product IDs
                    modelList = query.AsEnumerable()
                        .Where(item => distinctRelatedProductIds.Contains(item.ID))
                        .OrderBy(_ => Guid.NewGuid()) // Shuffle the results randomly
                        .Take(16)
                        .ToList();
                }
                else
                {

                    // Split the seourl into individual keywords
                    string[] keywords = seourl.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    // Filter and order the data
                    modelList = query.AsEnumerable()
                        .Where(u => u.IsPublish == true && keywords.Any(keyword => u.ProductBasicMetaData.Name.ToLower().Contains(keyword)))
                        .OrderBy(u => Guid.NewGuid()) // Replace with your sorting key
                        .Take(12)
                        .ToList();
                }





            }

            else if (dataFor == "related_detail_same_seller")
            {




                // Split the seourl into individual keywords
                string[] keywords = seourl.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Filter and order the data
                modelList = query.AsEnumerable()
                    .Where(u => u.IsPublish == true && keywords.Any(keyword => u.ProductBasicMetaData.Name.ToLower().Contains(keyword))
                    && u.ID != ItemId
                    )
                    .OrderBy(u => Guid.NewGuid()) // Replace with your sorting key
                    .Take(5)
                    .ToList();
            }

            else if (dataFor == "adminsummary")
            {

                modelList = query.ToList();
            }

            return modelList;
        }

        public List<ProductViewModel> productmasterdataV2(int loginuserid, string datafor, int pagesize, int pagenumber, int? itemid = 0, string? seourl = "")//sellerid is to locate not to show the same seller id
        {
            #region Query
            var userselectedcurrency = _globalhelper.GetUserCurrency();
            var conversionrate = _globalhelper.ConversionRate(userselectedcurrency);
            var dateformat = _globalhelper.Dateformat();
            var itemList = ItemJsonList(datafor, pagesize, pagenumber, (int)itemid, seourl, loginuserid);

            var products = (from p in itemList
                            join up in _dbContext.UsersProfiles on p.ProfileID equals up.ProfileId
                            let discountInfo = CalculateDiscount(p.ProductBasicMetaData.Price, conversionrate, p.SellerDiscountMetaData)
                            select new ProductViewModel
                            {

                                ProductId = p.ID,
                                ProductGUID = Guid.Parse(p.ItemGUID.ToString()),
                                ProductSeourl = p.ProductBasicMetaData.SEOURL + '-' + p.ID,
                                ProductImage = p.ProductBasicMetaData != null ? p.ProductBasicMetaData.Image.Trim() : null,
                                ProductName = p.ProductBasicMetaData.Name,
                                ProductUnit = p.ProductBasicMetaData.Unit,
                                BrandName = p.ProductBasicMetaData.Brand,
                                ShortDescription = p.ProductBasicMetaData != null ? p.ProductBasicMetaData.ShortDescription : null,
                                DetailDescription = p.ProductDetailMetaData != null ? p.ProductDetailMetaData.DetailDescription : null,
                                ReturnPolicy = p.productPolicyMetaData != null ? p.productPolicyMetaData.ReturnPolicy : null,
                                CancelPolicy = p.productPolicyMetaData != null ? p.productPolicyMetaData.CancellationPolicy : null,
                                IsOutofStock = p.productInventoryMetaData != null ? (bool)p.productInventoryMetaData.IsOutOfStock : false,
                                SKU = p.productInventoryMetaData != null ? _globalhelper.GenerateSKU(p.productInventoryMetaData.SKU) : _globalhelper.GenerateSKU(p.ProfileID + p.ID.ToString()),
                                EANCode = p.productInventoryMetaData != null ? p.productInventoryMetaData.EANCode : null,
                                ActualCurrency = p.ProductBasicMetaData.CurrencyId,
                               
                                ActualPrice =p.ProductBasicMetaData.Price,

                                //IsDiscounted = IsItemDiscount(p.SellerDiscountMetaData),
                                //Price = itemdiscount(p.ProductBasicMetaData.Price, conversionrate, p.SellerDiscountMetaData) ,
                                //DiscountAmount =

                                Price = discountInfo.Priceafterdiscount,
                                IsDiscounted = discountInfo.IsDiscounted,


                                DiscountAmount = discountInfo.DiscountedValue,
                                PriceBeforeDiscount= discountInfo.Priceafterdiscount + discountInfo.DiscountedValue,

                                Currency = userselectedcurrency, /*p.ProductBasicMetaData.CurrencyId.ToString(),*/ // Add Currency property to ProductBasicV2



                                IsVideo = true,


                                IsManagedInventory = p.productInventoryMetaData != null ? p.productInventoryMetaData.IsManagedInventory : false,
                                MinQty = p.productInventoryMetaData != null ? (int)p.productInventoryMetaData.MINQTY : 0,
                                MaxQty = p.productInventoryMetaData != null ? (int)p.productInventoryMetaData.MAXQTY : 0,


                                ProfileId = p.ProfileID,
                                Shopurlpath = up.BusinessUrlpath != null ? up.BusinessUrlpath : up.ProfileGuid.ToString(),
                                profileguid = (Guid)up.ProfileGuid,
                                insertdate = p.InsertDate.ToString(dateformat),
                                //Totalreviews = 0 ,
                                //Starrating_average = 0 ,
                                //Followers = 0,
                                ispublish = p.IsPublish,
                               


                                Categoryarrary = p.ProductBasicMetaData.ProductCategoryArray,

                                CategoryId = p.ProductBasicMetaData.ProductCategoryArray != "" ? int.Parse(GlobalHelper.CommaSeperationLastValue(p.ProductBasicMetaData.ProductCategoryArray)) : 0,


                                ListProductCategoryMetaData = ProductCategorymetadataList(p.ProductBasicMetaData.ProductCategoryArray),

                                ListAmenityMetaData = ParseMetaDataProductAmenities(p.AmenityMetaData != null ? p.AmenityMetaData : null),

                                ListofImages = CreateItemSlider(p.ImagesMetaData, p.VideoMetaData, p.ProductBasicMetaData.Image),
                                ListofVideo = p.productVideoMetaDatas != null ? p.productVideoMetaDatas : null,

                                itemaddress = p.ProductClassifiedMetaData != null ? p.ProductClassifiedMetaData.Address : "",
                                itemcontact = p.ProductClassifiedMetaData != null ? p.ProductClassifiedMetaData.ContactNumber : "",
                                itememail = p.ProductClassifiedMetaData != null ? p.ProductClassifiedMetaData.Email : "",

                                //SellingType = p.ProductBasicMetaData.SellingType,  //sell, auction, classified, coupon
                                //ListingType = p.ProductBasicMetaData.ListingType,  //physical, digital, service course

                                Favorite = (loginuserid != null && _dbContext.CustomerWishlists.Any(w => w.UserId == loginuserid && w.ProductId == p.ID)) ? 1 : 0,

                                SellingType = GetSellingTypeNameById(p.ProductBasicMetaData.SellingTypeID),  //sell, auction, classified, coupon
                                ListingType = GetListingTypeNameBySellingType(p.ProductBasicMetaData.SellingTypeID, p.ProductBasicMetaData.ListingTypeID),  //physical, digital, service course


                                productShippingMetaData = p.productShippingMetaData != null ? p.productShippingMetaData : null,

                                userothermetadata = up.OtherMetaData != null ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData) : null,
                                itemothermetadata = p.ItemOtherMetaData,
                                itemdigitalmetadata = p.ProductDigitalMetaData,


                                IsAttributeExist=p.IsAttributeExist

                            }).ToList();



     //       products = products
     //.Where(u => u.userothermetadata != null &&
     //            u.userothermetadata.UserInactiveMetaData != null &&
     //            (u.userothermetadata.UserInactiveMetaData.Availableforsearch == true ||
     //             u.userothermetadata.UserInactiveMetaData.Availableforsearch == null))
     //.ToList();


            return products;

            #endregion
        }


        public (decimal Priceafterdiscount, bool IsDiscounted, decimal DiscountedValue) CalculateDiscount(decimal price, decimal conversionRate, string discountMetadata)
        {
            bool isDiscounted = false;
            decimal discountedValue = 0;
            decimal actualPrice = Math.Round(price * conversionRate, 2);


            ///if discount meta data null so just return the method
            ///

            if(discountMetadata==null)
            {
                return (actualPrice, isDiscounted, discountedValue);
            }

            DiscountSellerAlwaysMetaData discountSellerAlways = ParseMetaDataDiscountSellerMetaData(discountMetadata);
            DiscountSellerCustomMetaData discountSellerCustom = ParseMetaDataDiscountCustomMetaDataUpcoming(discountMetadata);
           
            //give priority to custom
            if (discountSellerCustom != null)
            {
                if (discountSellerCustom.DiscountType == "Value")
                {
                    discountedValue = discountSellerCustom.DiscountAmount;
                    actualPrice = actualPrice - discountSellerCustom.DiscountAmount;

                }
                else if (discountSellerCustom.DiscountType == "Percentage")
                {
                    discountedValue = (actualPrice * discountSellerCustom.DiscountAmount) / 100;
                    actualPrice = actualPrice - discountedValue;


                }

                isDiscounted = true;
            }

            if (discountSellerAlways.IsAlways && discountSellerCustom == null)
            {
                if (discountSellerAlways.DiscountType == "Value")
                {
                    discountedValue = discountSellerAlways.DiscountAmount;
                    actualPrice = actualPrice - discountSellerAlways.DiscountAmount;
                    
                }
                else if (discountSellerAlways.DiscountType == "Percentage")
                {
                    discountedValue = (actualPrice * discountSellerAlways.DiscountAmount) / 100;
                    actualPrice = actualPrice - discountedValue;


                }

                isDiscounted = true;
                
            }

            ///custom if not null it will override the always discount logic
            ///
           

            return (actualPrice, isDiscounted, discountedValue);
        }

       
        public void productboost(Guid itemid, string boosttype, DateTime startdate, DateTime enddate, string invoicenumber, bool isactive)
        {
            ProductBoost pb = new ProductBoost();

            pb.ItemBoostGUID = itemid;
            pb.BoostType = boosttype;
            pb.InsertDate = DateTime.Now;
            pb.IsActive = isactive;
            pb.StartDate = startdate;
            pb.EndDate = enddate;
            pb.InvoiceNumber = invoicenumber;

            _dbContext.ProductBoosts.Add(pb);
            _dbContext.SaveChanges();
        }

        public void BoostrMetaDataUpdate(int productboostid, string value, string type) //
        {
            BoostOtherMetaData existingMetaData = new BoostOtherMetaData();
            ProductBoost up = _dbContext.ProductBoosts.FirstOrDefault(u => u.ProductBoostId == productboostid);

            if (up.BoostMetaData != null)
            {
                if (up != null)
                {
                    existingMetaData = up.BoostMetaData != null ? JsonConvert.DeserializeObject<BoostOtherMetaData>(up.BoostMetaData) : null;
                }
            }
            #region CreateJsonForComments





            //record both view and click
            if (type == "viewclick")
            {
                if (existingMetaData.TotalViews == null)
                {
                    existingMetaData.TotalViews = 0;
                }

                if (existingMetaData.TotalClicks == null)
                {
                    existingMetaData.TotalClicks = 0;
                }

                existingMetaData.TotalClicks = existingMetaData.TotalClicks + 1;
                existingMetaData.TotalViews = existingMetaData.TotalViews + 1;
            }

            //Views
            if (type == "view")
            {
                if (existingMetaData.TotalViews == null)
                {
                    existingMetaData.TotalViews = 0;
                }


                existingMetaData.TotalViews = existingMetaData.TotalViews + 1;
            }
            //click
            if (type == "click")
            {
                if (existingMetaData.TotalClicks == null)
                {
                    existingMetaData.TotalClicks = 0;
                }


                existingMetaData.TotalClicks = existingMetaData.TotalClicks + 1;
            }



            up.BoostMetaData = JsonConvert.SerializeObject(existingMetaData);
            _dbContext.SaveChanges();

            #endregion



        }


        public object[] BoostValidation(Guid ID, string itemtype)
        {
            DateTime currentDate = DateTime.Now;

            var existingItem = _dbContext.ProductBoosts.FirstOrDefault(pb => pb.ItemBoostGUID == ID && pb.BoostType == itemtype && pb.StartDate <= currentDate && pb.EndDate >= currentDate);

            if (existingItem != null)
            {
                return new object[] { true, existingItem.StartDate, existingItem.EndDate };
            }
            else
            {
                return new object[] { false, DateTime.MinValue, DateTime.MinValue };
            }
        }

        //products = products.GroupBy(p => p.ProductId).Select(g => g.First());

        //Favorite = (loginuserid != null && _dbContext.CustomerWishlists.Any(w => w.UserId == loginuserid && w.ProductId == p.ID)) ? 1 : 0
        //Website_Setup_Product_Detail = subwd,




        // -------------Create dead lock
        //SellingType = GetSellingTypeNameById(p.ProductBasicMetaData.SellingTypeID),  //sell, auction, classified, coupon
        //ListingType = GetListingTypeNameBySellingType(p.ProductBasicMetaData.SellingTypeID, p.ProductBasicMetaData.ListingTypeID),  //physical, digital, service course
        //Favorite = (loginuserid != null && _dbContext.CustomerWishlists.Any(w => w.UserId == loginuserid && w.ProductId == p.ID)) ? 1 : 0,
        //CategoryName = p.ProductBasicMetaData.ProductCategoryArray != "" ? CategoryNameByID(p.ProductBasicMetaData.ProductCategoryArray) : "Category",
        //join wd in _dbContext.WebsiteSetupProductDetails on p.ID equals wd.SetupProductId into gj
        ////from subwd in gj.DefaultIfEmpty()
        //join category in _dbContext.CategoryMasters on p equals category.CategoryId
        //join producttype in _dbContext.GeneralSetups on p.ProductBasicMetaData.ListingTypeID equals producttype.GeneralSetupId
        //join sellingtype in _dbContext.GeneralSetups on p.ProductBasicMetaData.SellingTypeID equals sellingtype.GeneralSetupId

        /*  where*/ /*p.IsCustomProduct == false &&*/ /*up.Type == "Vendor"*/ /*&& category.IsDeleted == false*/ /*&& c.IsPublished == true*/
        public List<ProductViewModel> productmasterdata(int loginuserid)
        {
            #region Query
            var userselectedcurrency = _globalhelper.GetUserCurrency();
            var conversionrate = _globalhelper.ConversionRate(userselectedcurrency);
            var dateformat = _globalhelper.Dateformat();
            var itemList = ItemJsonList("home", 50, 1, 0, "", 0);

            var products = (from p in itemList
                            join up in _dbContext.UsersProfiles on p.ProfileID equals up.ProfileId

                            select new ProductViewModel
                            {

                                ProductId = p.ID,
                                ProductGUID = Guid.Parse(p.ItemGUID.ToString()),
                                ProductSeourl = p.ProductBasicMetaData.SEOURL + '-' + p.ID,
                                ProductImage = p.ProductBasicMetaData != null ? p.ProductBasicMetaData.Image : null,
                                ProductName = p.ProductBasicMetaData.Name,

                                BrandName = p.ProductBasicMetaData.Brand,
                                ShortDescription = p.ProductBasicMetaData != null ? p.ProductBasicMetaData.ShortDescription : null,
                                DetailDescription = p.ProductDetailMetaData != null ? p.ProductDetailMetaData.DetailDescription : null,
                                ReturnPolicy = p.productPolicyMetaData != null ? p.productPolicyMetaData.ReturnPolicy : null,
                                CancelPolicy = p.productPolicyMetaData != null ? p.productPolicyMetaData.CancellationPolicy : null,
                                IsOutofStock = p.productInventoryMetaData != null ? (bool)p.productInventoryMetaData.IsOutOfStock : false,
                                SKU = p.productInventoryMetaData != null ? p.productInventoryMetaData.SKU : p.ProfileID + p.ID.ToString(),
                                ActualCurrency = p.ProductBasicMetaData.CurrencyId,
                                ActualPrice = p.ProductBasicMetaData.Price,
                                Price = Math.Round((decimal)p.ProductBasicMetaData.Price * conversionrate, 2),
                                Currency = userselectedcurrency, /*p.ProductBasicMetaData.CurrencyId.ToString(),*/ // Add Currency property to ProductBasicV2



                                IsVideo = true,

                                MinQty = p.productInventoryMetaData != null ? (int)p.productInventoryMetaData.MINQTY : 0,
                                MaxQty = p.productInventoryMetaData != null ? (int)p.productInventoryMetaData.MAXQTY : 0,


                                ProfileId = p.ProfileID,
                                Shopurlpath = up.BusinessUrlpath,
                                profileguid = (Guid)up.ProfileGuid,
                                insertdate = p.InsertDate.ToString(dateformat),
                                //Totalreviews = 0 ,
                                //Starrating_average = 0 ,
                                //Followers = (int)(up.Followers),


                                ispublish = p.IsPublish,



                                Categoryarrary = p.ProductBasicMetaData.ProductCategoryArray,

                                CategoryId = p.ProductBasicMetaData.ProductCategoryArray != "" ? int.Parse(GlobalHelper.CommaSeperationLastValue(p.ProductBasicMetaData.ProductCategoryArray)) : 0,
                                ListProductCategoryMetaData = ProductCategorymetadataList(p.ProductBasicMetaData.ProductCategoryArray),

                                ListAmenityMetaData = ParseMetaDataProductAmenities(p.AmenityMetaData != null ? p.AmenityMetaData : null),

                                ListofImages = CreateItemSlider(p.ImagesMetaData, p.VideoMetaData, p.ProductBasicMetaData.Image),
                                ListofVideo = p.productVideoMetaDatas != null ? p.productVideoMetaDatas : null,

                                itemaddress = p.ProductClassifiedMetaData != null ? p.ProductClassifiedMetaData.Address : "",
                                itemcontact = p.ProductClassifiedMetaData != null ? p.ProductClassifiedMetaData.ContactNumber : "",
                                itememail = p.ProductClassifiedMetaData != null ? p.ProductClassifiedMetaData.Email : "",

                                SellingType = "Sell",  //sell, auction, classified, coupon
                                ListingType = "physical",  //physical, digital, service course


                                userothermetadata = up.OtherMetaData != null ? JsonConvert.DeserializeObject<UserOtherMetaData>(up.OtherMetaData) : null
                                //SellingType = GetSellingTypeNameById(p.ProductBasicMetaData.SellingTypeID),  //sell, auction, classified, coupon
                                //ListingType = GetListingTypeNameBySellingType(p.ProductBasicMetaData.SellingTypeID, p.ProductBasicMetaData.ListingTypeID),  //physical, digital, service course


                            }).ToList();



            return products;

            #endregion
        }
        #endregion




        #region ProductAmenities

        public AmenitiesViewModel GetCombinedAmenities(List<ProductAmenitiesMetaData> listAmenityMetaData)
        {
            var amenitiesViewModel = new AmenitiesViewModel
            {
                amenityheader = (from ps in listAmenityMetaData
                                 join question in _dbContext.ProductAmenitiesQuestionV2s on ps.AmenityID equals question.ProductAmenitiesId
                                 where question.IsPublish == true
                                 select new AmenitiesHeader
                                 {
                                     AmenityName = question.ProductAmenitiesHeading,
                                     Amenitiesid = question.ProductAmenitiesId
                                 }
                                ).DistinctBy(u => u.Amenitiesid).ToList(),

                amenitychild = (from ps in listAmenityMetaData
                                join option in _dbContext.ProductAmenitiesOptionsV2s on ps.OptionID equals option.ProductAmenitiesOptionId
                                where option.IsPublish == true
                                select new AmenitiesChild
                                {
                                    AmenitiesOptionName = option.ProductAmenitiesName,
                                    AmenitiesIcon = option.ProductAmenitiesIcon,
                                    OptionAmenityId = (int)option.ProductAmenitiesId
                                }
                              ).Distinct().ToList()
            };

            return amenitiesViewModel;
        }

        public List<ProductViewModel> productamenitiesheader(List<ProductAmenitiesMetaData> listamenityheader)
        {
            List<ProductViewModel> productAmenitiesheader = new List<ProductViewModel>();

            productAmenitiesheader = (from ps in listamenityheader

                                      join question in _dbContext.ProductAmenitiesQuestionV2s on ps.AmenityID equals question.ProductAmenitiesId

                                      where question.IsPublish == true
                                      select new ProductViewModel
                                      {
                                          AmenitiesHeader = question.ProductAmenitiesHeading,
                                          Amenitiesid = question.ProductAmenitiesId
                                      }
                              ).Distinct().ToList();


            return productAmenitiesheader.DistinctBy(u=>u.Amenitiesid).ToList();
        }


        public List<ProductViewModel> productamenitieschild(List<ProductAmenitiesMetaData> listamenityoption)
        {
            List<ProductViewModel> productAmenitieschild = new List<ProductViewModel>();

            productAmenitieschild = (from ps in listamenityoption
                                     join option in _dbContext.ProductAmenitiesOptionsV2s on ps.OptionID equals option.ProductAmenitiesOptionId

                                     where option.IsPublish == true /*&& option.ProductAmenitiesId == amenitiesid*/
                                     select new ProductViewModel
                                     {
                                         AmenitiesOptionName = option.ProductAmenitiesName,
                                         AmenitiesIcon = option.ProductAmenitiesIcon,
                                         OptionAmenityId = (int)option.ProductAmenitiesId
                                     }
                                   ).Distinct().ToList();


            return productAmenitieschild;
        }

        #endregion


        #region Images
        //public List<ImageItemMetaData> productimages(string productseourl)
        //{
        //    List<ImageItemMetaData> imagesData = new List<ImageItemMetaData>();

        //    var pr = (from p in _dbContext.ProductBasicV2s
        //              where p.ProductSeourl + '-' + p.ProductId == productseourl
        //              select new ImageItemMetaData
        //              {
        //                  Image = p.ProductImage.Replace(" ", ""),
        //                  ImageName = p.ProductName
        //              });

        //    var pi = (from productimages in _dbContext.ProductImages
        //              join p in _dbContext.ProductBasicV2s on productimages.ProductGuid equals p.ProductGuid
        //              where p.ProductSeourl + '-' + p.ProductId == productseourl
        //              select new ImageItemMetaData
        //              {
        //                  Image = productimages.Image.Replace(" ", ""),
        //                  ImageName = productimages.AltText,
        //              }).ToList();

        //    imagesData = pr.AsEnumerable().Union(pi).ToList();

        //    return imagesData;
        //}

        #endregion

        #region Reviews

        //public List<ProductReviews> productreview(string productseourl)
        //{
        //    List<ProductReviews> productReview = new List<ProductReviews>();

        //    var q = (from pr in _dbContext.ProductReviewV2s
        //             join om in _dbContext.OrderMasters on pr.OrderMasterId equals om.OrderId
        //             join up in _dbContext.UsersProfiles on pr.ProfileId equals up.ProfileId
        //             join p in _dbContext.ProductBasicV2s on pr.ReviewProductId equals p.ProductId
        //             where p.ProductSeourl + "-" + p.ProductId == productseourl
        //             select new ProductReviews
        //             {
        //                 Remarks = pr.Remarks,
        //                 rating = (int)pr.StartRating,
        //                 ReviewName = up.Firstname + " " + up.Lastname,
        //                 insertdate = pr.InsertDate.ToString(),
        //                 attachment = pr.Attachment
        //             }
        //                );

        //    return productReview.ToList();
        //}

        #endregion


        #region QA

        public List<ProductQAViewModel> productqa(int productid)
        {

            List<ProductQAViewModel> productqa = new List<ProductQAViewModel>();

            productqa = (from question in _dbContext.ProductQuestions

                         join answer in _dbContext.ProductQuestionAnswers on question.ProductQaid equals answer.ProductQaid

                         into gj
                         from subanswer in gj.DefaultIfEmpty()

                         orderby question.ProductQaid descending
                         where question.IsActive == true && question.ProductId == productid
                         select new ProductQAViewModel
                         {
                             question = question.Question,
                             answer = subanswer.Qanswer ?? "Pending Answer"
                         }
                                  ).ToList();


            return productqa;
        }


        public List<ProductQAViewModel> productqabySellerId()

        {

            List<ProductQAViewModel> productqa = new List<ProductQAViewModel>();

            productqa = (from question in _dbContext.ProductQuestions
                         join product in _dbContext.ItemListings on question.ProductId equals product.ItemId
                         join up in _dbContext.UsersProfiles on question.ProfileId equals up.ProfileId
                         join answer in _dbContext.ProductQuestionAnswers on question.ProductQaid equals answer.ProductQaid

                         into gj
                         from subanswer in gj.DefaultIfEmpty()

                         orderby question.ProductQaid descending
                         where question.IsActive == true
                         select new ProductQAViewModel
                         {
                             QuestionId = question.ProductQaid,
                             productid = question.ProductId,
                             question = question.Question,
                             answer = subanswer.Qanswer ?? "Pending Answer",
                             replydate = subanswer.InsertDate,
                             questiondate = question.InsertDate,
                             name = up.Firstname + " " + up.Lastname,
                             postedbyid = question.ProfileId,
                             sellerid = product.ProfileId,
                             ItemMetaData = ParseMetaDataProductBasic(product.ItemMetaData),
                             usertype = up.Type
                         }
                                  ).ToList();


            return productqa;
        }
        #endregion



        #region ItemListing-Deserialized
        public ItemMetaDataUpsert ItemJsonList(string ItemMetaData)
        {

            var json = new ItemMetaDataUpsert();
            if (ItemMetaData != null)
            {
                json.ProductBasicMetaData = ParseMetaDataProductBasic(ItemMetaData);
                //json.ProductBasicMetaData.ProductCategoryArray = ParseMetaDataProductCategory(ItemMetaData);



                if (json.ProductBasicMetaData.SellingTypeID == 1)//classified
                {
                    json.ProductClassifiedMetaData = ParseMetaDataProductClassified(ItemMetaData);
                }

                else if (json.ProductBasicMetaData.SellingTypeID == 2 || json.ProductBasicMetaData.SellingTypeID == 3)//auction or penny auction
                {
                    json.ProductAuctionMetaData = ParseMetaDataProductAuction(ItemMetaData);
                }

                else if (json.ProductBasicMetaData.SellingTypeID == 3) /// penny auction
                {
                    json.ProductPennyAuctionMetaData = ParseMetaDataProductPennyAuction(ItemMetaData);

                }



                ///llisting type  physical, digital, service, course
                //if (json.ProductBasicMetaData.ListingTypeID == 1)//digital
                //{
                //   json.ProductDigitalMetaData = ParseMetaDataProductDigital(ItemMetaData);

                //}

            }

            return json;
        }

        #endregion




        #region Attributlist
        public List<ProductAttributeQuestionV2> attributelist(Guid productguid)
        {


            List<ProductAttributeQuestionV2> attributelist = _dbContext.ProductAttributeQuestionV2s.Where(u => u.ProductGuid == productguid).OrderBy(u => u.SortNumber).ToList();


            return attributelist;

        }

        public List<ProductAttributeOptionV2> attributeoptionlist(Guid attributeguid)
        {


            List<ProductAttributeOptionV2> attributeoptionlist = _dbContext.ProductAttributeOptionV2s.Where(u => u.ProductAttributeGuid == attributeguid).ToList();

            return attributeoptionlist;

        }


        public List<ProductAttributeViewModel> GetProductAttributes(Guid productguid)
        {
            var userselectedcurrency = _globalhelper.GetUserCurrency();
            var conversionrate = _globalhelper.ConversionRate(userselectedcurrency);

            var query = from question in _dbContext.ProductAttributeQuestionV2s
                        where question.ProductGuid == productguid
                        orderby question.SortNumber
                        let options = _dbContext.ProductAttributeOptionV2s
                            .Where(option => option.ProductAttributeGuid == question.ProductAttributeGuid)
                            .Select(option => new AttributeOptionViewModel
                            {
                                ActualAttributeprice = option.Attributeprice,
                                ConversionAttributeprice = Math.Round((decimal)option.Attributeprice * conversionrate, 2),
                                OptionText = option.OptionText,
                                Attributeimage = option.Attributeimage
                                // Copy other fields as needed
                                // Add other field mappings here...
                            })
                            .ToList()
                        where options.Any() // Only include questions with at least one attribute
                        select new ProductAttributeViewModel
                        {
                            Question = question,
                            Options = options
                        };

            return query.ToList();
        }

        #endregion



        #region CategoryModel
        public List<CategoryViewModel> GetCategoriesViewModel()
        {
            var query = _dbContext.CategoryMasters
                .Where(firstlevel => firstlevel.IsPublished == true && firstlevel.IsDeleted == false)
                .OrderBy(firstlevel => firstlevel.Sortnumber)
                .Select(firstlevel => new CategoryViewModel
                {
                    ParentCategoryId = firstlevel.ParentCategoryId,
                    CategoryId = firstlevel.CategoryId,
                    CategoryName = firstlevel.CategoryName,
                    Icon = firstlevel.Icon,
                    SecondLevel = _dbContext.CategoryMasters
                        .Where(secondlevel => secondlevel.ParentCategoryId == firstlevel.CategoryId && secondlevel.IsPublished == true && secondlevel.IsDeleted == false)
                        .OrderBy(secondlevel => secondlevel.Sortnumber)
                        .Select(secondlevel => new SecondCategoryViewModel
                        {
                            CategoryId = secondlevel.CategoryId,
                            CategoryName = secondlevel.CategoryName,
                            Icon = secondlevel.Icon,
                            ThirdLevel = _dbContext.CategoryMasters
                                .Where(thirdlevel => thirdlevel.ParentCategoryId == secondlevel.CategoryId && thirdlevel.IsPublished == true && thirdlevel.IsDeleted == false)
                                .OrderBy(thirdlevel => thirdlevel.Sortnumber)
                                .Select(thirdlevel => new ThirdCategoryViewModel
                                {
                                    CategoryId = thirdlevel.CategoryId,
                                    CategoryName = thirdlevel.CategoryName,
                                    Icon = thirdlevel.Icon,
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return query;
        }
        #endregion


        #region JsonCreator


        #region ProductBasic-JsonCreator
        public string ProductBasicmetadata(int sellingtype, int listingtype, string name, string shortdescription, int currencyId, decimal price, string imageUrl, string unit, string brand, string seotitle, string seokeyword, string seodescription, string categoryarray)
        {
            var metadata = new ProductBasicMetaData
            {

                SellingTypeID = sellingtype,
                ListingTypeID = listingtype,
                Name = name.Trim(),
                SEOURL = GlobalHelper.SEOURL(name.Trim()),
                ShortDescription = shortdescription.Trim(),
                CurrencyId = currencyId,
                Price = price,
                Image = imageUrl,
                Unit = unit,
                Brand = brand,
                SeoMetaTitle = seotitle ?? name.Trim(),
                SeoKeywords = seokeyword ?? name.Trim(),
                SeoMetadescription = seodescription ?? name.Trim(),
                ProductCategoryArray = categoryarray

            };

            return JsonConvert.SerializeObject(metadata);
        }


        public static ProductBasicMetaData ParseMetaDataProductBasic(string json)
        {
            if (json == null)
            {
                return new ProductBasicMetaData();
            }

            JObject jsonObject = JObject.Parse(json);
            JToken productDetailToken = jsonObject["ProductBasicMetaData"];

            if (productDetailToken == null)
            {
                return new ProductBasicMetaData();
            }

            ProductBasicMetaData parsedData = productDetailToken.ToObject<ProductBasicMetaData>();
            return parsedData;
        }
        #endregion

        #region Category-JsonCreator

        //public string ProductCategorymetadata(string categoryarray)
        //{
        //    List<ProductCategoryMetaData> categoryList = new List<ProductCategoryMetaData>();

        //    // Split the categoryarray string into individual category IDs
        //    string[] categoryIds = categoryarray.Split(',');

        //    foreach (string categoryId in categoryIds)
        //    {
        //        if (int.TryParse(categoryId, out int parsedCategoryId))
        //        {
        //            var categoryItem = new ProductCategoryMetaData
        //            {
        //                ID = categoryList.Count + 1,
        //                CategoryId = parsedCategoryId
        //            };

        //            categoryList.Add(categoryItem);
        //        }
        //    }

        //    string jsonList = JsonConvert.SerializeObject(categoryList);
        //    return jsonList;
        //}

        //public string ParseMetaDataProductCategory(string json)
        //{
        //    if (json == null)
        //    {
        //        return string.Empty;
        //    }

        //    JObject jsonObject = JObject.Parse(json);
        //    JToken productCategoryToken = jsonObject["ProductCategoryMetaData"];

        //    if (productCategoryToken == null)
        //    {
        //        return string.Empty;
        //    }

        //    var productCategories = productCategoryToken.ToObject<List<ProductCategoryMetaData>>();

        //    string csv = string.Join(",", productCategories.Select(category => category.CategoryId.ToString()));
        //    return csv;
        //}


        public List<ProductCategoryMetaData> ProductCategorymetadataList(string categoryarray)
        {
            List<ProductCategoryMetaData> categoryList = new List<ProductCategoryMetaData>();

            // Split the categoryarray string into individual category IDs
            string[] categoryIds = categoryarray.Split(',');

            foreach (string categoryId in categoryIds)
            {
                if (int.TryParse(categoryId, out int parsedCategoryId))
                {
                    var categoryMaster = _dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == parsedCategoryId);

                    if (categoryMaster != null)
                    {
                        var categoryItem = new ProductCategoryMetaData
                        {
                            ID = categoryList.Count + 1,
                            CategoryId = parsedCategoryId,
                            CategoryName = categoryMaster.CategoryName
                        };

                        categoryList.Add(categoryItem);
                    }
                }
            }

            return categoryList;
        }

        #endregion

        #region Classified-JsonCreator
        public string ProductClassifiedmetadata(string contactnumber, string email, string address)
        {

            string latitude = "";
            string longitude = "";
            if (address != null && address != string.Empty)
            {
                GeocodeResult geocodeResult = _globalhelper.GetGeocodeDetails(address);
                latitude = geocodeResult.Latitude.ToString();
                longitude = geocodeResult.Longitude.ToString();
            }


            var metadata = new ProductClassifiedMetaData
            {
                ContactNumber = contactnumber,
                Email = email,
                Address = address,
                Latitude = latitude,
                Longitude = longitude
            };

            return JsonConvert.SerializeObject(metadata);
        }

        public static ProductClassifiedMetaData ParseMetaDataProductClassified(string json)
        {
            if (json == null)
            {
                return new ProductClassifiedMetaData();
            }

            JObject jsonObject = JObject.Parse(json);
            JToken productclassifiedToken = jsonObject["ProductClassifiedMetaData"];

            if (productclassifiedToken == null)
            {
                return new ProductClassifiedMetaData();
            }

            ProductClassifiedMetaData parsedData = productclassifiedToken.ToObject<ProductClassifiedMetaData>();
            return parsedData;
        }
        #endregion

        #region Digital-JsonCreator


        public string ProductDigitalmetadata(string imageArray, string existingMetaData)
        {
            List<ProductDigitalMetaData> categoryList;

            // Check if the existingMetaData is null or empty
            if (string.IsNullOrEmpty(existingMetaData))
            {
                // If it's empty, create a new list
                categoryList = new List<ProductDigitalMetaData>();
            }
            else
            {
                // If it's not empty, deserialize the existingMetaData JSON string into a list
                categoryList = JsonConvert.DeserializeObject<List<ProductDigitalMetaData>>(existingMetaData);
            }

            // Split the imageArray string into individual image URLs
            string[] imageUrls = imageArray.Split(',');

            foreach (string imageUrl in imageUrls)
            {



                var categoryItem = new ProductDigitalMetaData
                {


                    ID = categoryList.Count + 1,
                    DigitalLink = imageUrl,
                    Name = GlobalHelper.FileNameFromGUID(imageUrl)

                };

                categoryList.Add(categoryItem);
            }

            string jsonList = JsonConvert.SerializeObject(categoryList);
            return jsonList;
        }

        public static List<ProductDigitalMetaData> ParseMetaDataProductDigital(string json)
        {
            if (json == null)
            {
                return new List<ProductDigitalMetaData>(); // Return an empty list
            }

            List<ProductDigitalMetaData> parsedData = JsonConvert.DeserializeObject<List<ProductDigitalMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }


        #endregion

        #region Auction-JsonCreator




        public string ProductAuctionmetadata(DateTime StartDate, DateTime EndDate, decimal StartPrice, decimal EndPrice, bool IsIncrementByUser, decimal IncrementAmount)
        {
            var metadata = new ProductAuctionMetaData
            {
                StartDate = StartDate,
                EndDate = EndDate,
                StartPrice = StartPrice,
                EndPrice = EndPrice,
                IsIncrementByUser = IsIncrementByUser,
                IncrementAmount = IncrementAmount
            };

            return JsonConvert.SerializeObject(metadata);
        }


        public ProductAuctionMetaData ParseMetaDataProductAuction(string json)
        {
            if (json == null)
            {
                return new ProductAuctionMetaData();
            }

            JObject jsonObject = JObject.Parse(json);
            JToken productauctionToken = jsonObject["ProductAuctionMetaData"];

            if (productauctionToken == null)
            {
                return new ProductAuctionMetaData();
            }

            ProductAuctionMetaData parsedData = productauctionToken.ToObject<ProductAuctionMetaData>();
            return parsedData;
        }
        #endregion

        #region PennyAuction-JsonCreator




        public string ProductPennyAuctionmetadata(int noofrealbids, string array)
        {
            //create list of bot here

            List<ProductBotItemMetaData> botList = new List<ProductBotItemMetaData>();

            string[] arrayItems = array.Split(',');

            foreach (string item in arrayItems)
            {
                var botItem = new ProductBotItemMetaData
                {
                    ID = botList.Count + 1,
                    BotId = int.Parse(item)
                };

                botList.Add(botItem);
            }


            var metadata = new ProductPennyAuctionMetaData
            {
                NoofRealBids = noofrealbids,
                BotList = botList
            };

            return JsonConvert.SerializeObject(metadata);
        }



        public ProductPennyAuctionMetaData ParseMetaDataProductPennyAuction(string json)
        {
            if (json == null)
            {
                return new ProductPennyAuctionMetaData();
            }

            JObject jsonObject = JObject.Parse(json);
            JToken productpennyauctionToken = jsonObject["ProductPennyAuctionMetaData"];

            if (productpennyauctionToken == null)
            {
                return new ProductPennyAuctionMetaData();
            }

            ProductPennyAuctionMetaData parsedData = productpennyauctionToken.ToObject<ProductPennyAuctionMetaData>();
            return parsedData;
        }
        #endregion



        #region SellingType
        public List<SellingTypeMetaData> GetSellingTypeList()
        {
            // Fetch the JSON data from the "websitesetup" table
            Websetting websiteSetup = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "SellingTypes"); // Implement your own logic to fetch the data

            if (websiteSetup == null)
            {
                return new List<SellingTypeMetaData>();
            }

            // Parse the JSON data
            JObject jsonObject = JObject.Parse(websiteSetup.WebsettingValue);
            JToken sellingTypeToken = jsonObject["SellingType"];

            if (sellingTypeToken == null)
            {
                return new List<SellingTypeMetaData>();
            }

            // Deserialize the SellingType list
            List<SellingTypeMetaData> sellingTypeList = JsonConvert.DeserializeObject<List<SellingTypeMetaData>>(sellingTypeToken.ToString());

            // Filter and return the desired SellingType list
            List<SellingTypeMetaData> filteredList = sellingTypeList.Where(st => st.Name != null && st.IsPublish == true).ToList();

            return filteredList;
        }

        public List<ListingTypeMetaData> GetUniqueListingTypes()
        {
            // Fetch the JSON data from the "websitesetup" table
            Websetting websiteSetup = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "SellingTypes"); // Implement your own logic to fetch the data

            if (websiteSetup == null)
            {
                return new List<ListingTypeMetaData>();
            }

            // Parse the JSON data
            JObject jsonObject = JObject.Parse(websiteSetup.WebsettingValue);
            JToken sellingTypeToken = jsonObject["SellingType"];

            if (sellingTypeToken == null)
            {
                return new List<ListingTypeMetaData>();
            }

            // Deserialize the SellingType list
            List<SellingTypeMetaData> sellingTypeList = JsonConvert.DeserializeObject<List<SellingTypeMetaData>>(sellingTypeToken.ToString());

            // Collect unique ListingType names from all SellingTypes
            var uniqueListingTypes = sellingTypeList
                .SelectMany(sellingType => sellingType.ListingType)
                .GroupBy(listingType => listingType.Name)
                .Select(group => group.First())
                .ToList();

            return uniqueListingTypes;
        }


        //deserialized to return only name
        public string GetSellingTypeNameById(int id)
        {
            // Fetch the JSON data from the "websitesetup" table
            Websetting websiteSetup = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "SellingTypes"); // Implement your own logic to fetch the data

            if (websiteSetup == null)
            {
                return null;
            }

            // Parse the JSON data
            JObject jsonObject = JObject.Parse(websiteSetup.WebsettingValue);
            JToken sellingTypeToken = jsonObject["SellingType"];

            if (sellingTypeToken == null)
            {
                return null;
            }

            // Deserialize the SellingType list
            List<SellingTypeMetaData> sellingTypeList = JsonConvert.DeserializeObject<List<SellingTypeMetaData>>(sellingTypeToken.ToString());

            // Find the SellingType with the given id
            SellingTypeMetaData sellingType = sellingTypeList.FirstOrDefault(st => st.ID == id);

            // Return the Name property of the SellingType as a string
            return sellingType?.Name;
        }
        #endregion


        #region CategoryNameByID
        public string CategoryNameByID(string array)
        {
            if (array == "")
            {
                return "";
            }
            int categoryid = int.Parse(GlobalHelper.CommaSeperationLastValue(array));


            CategoryMaster c = _dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == categoryid);
            return c?.CategoryName;

        }
        #endregion

        #region ListingType
        public List<ListingTypeMetaData> GetListingTypesBySellingType(int sellingTypeId)
        {
            // Fetch the JSON data from the "websitesetup" table
            Websetting websiteSetup = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "SellingTypes"); // Implement your own logic to fetch the data

            if (websiteSetup == null)
            {
                return new List<ListingTypeMetaData>();
            }

            // Parse the JSON data
            JObject jsonObject = JObject.Parse(websiteSetup.WebsettingValue);
            JToken sellingTypeToken = jsonObject["SellingType"];

            if (sellingTypeToken == null)
            {
                return new List<ListingTypeMetaData>();
            }

            // Deserialize the SellingType list
            List<SellingTypeMetaData> sellingTypeList = JsonConvert.DeserializeObject<List<SellingTypeMetaData>>(sellingTypeToken.ToString());

            // Find the SellingType by ID
            SellingTypeMetaData sellingType = sellingTypeList.FirstOrDefault(st => st.ID == sellingTypeId);

            if (sellingType == null || sellingType.ListingType == null)
            {
                return new List<ListingTypeMetaData>();
            }

            // Return the ListingType records for the SellingType
            return sellingType.ListingType;
        }



        //deserialized to return only name
        public string GetListingTypeNameBySellingType(int sellingTypeId, int listingTypeId)
        {
            // Fetch the JSON data from the "websitesetup" table
            Websetting websiteSetup = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "SellingTypes"); // Implement your own logic to fetch the data

            if (websiteSetup == null)
            {
                return null;
            }

            // Parse the JSON data
            JObject jsonObject = JObject.Parse(websiteSetup.WebsettingValue);
            JToken sellingTypeToken = jsonObject["SellingType"];

            if (sellingTypeToken == null)
            {
                return null;
            }

            // Deserialize the SellingType list
            List<SellingTypeMetaData> sellingTypeList = JsonConvert.DeserializeObject<List<SellingTypeMetaData>>(sellingTypeToken.ToString());

            // Find the SellingType by ID
            SellingTypeMetaData sellingType = sellingTypeList.FirstOrDefault(st => st.ID == sellingTypeId);

            if (sellingType == null || sellingType.ListingType == null)
            {
                return null;
            }

            // Find the ListingType by ID
            ListingTypeMetaData listingType = sellingType.ListingType.FirstOrDefault(lt => lt.ID == listingTypeId);

            // Return the Name property of the ListingType as a string
            return listingType?.Name;
        }
        #endregion

        #region SellerDiscount-JsonCreator

        public string ProductSellerDiscountAlwaysmetadata(bool isalways, string discounttype, decimal discountvalue)
        {
            var metadata = new DiscountSellerAlwaysMetaData
            {
                IsAlways = isalways,
                DiscountType = discounttype,
                DiscountAmount = discountvalue
            };

            return JsonConvert.SerializeObject(metadata);
        }

        public string ProductSellerDiscountCustommetadata(string type, string discountid, DateTime startdate, DateTime enddate, string discounttype, decimal discountvalue, bool isactive, string existingMetaData)
        {

            List<DiscountSellerCustomMetaData> metadata = new List<DiscountSellerCustomMetaData>();
            if (existingMetaData!=null)
            {
                // Deserialize the existing metadata JSON string into a List
                JObject jsonObject = JObject.Parse(existingMetaData);
                JToken productDetailToken = jsonObject["DiscountSellerCustomMetaData"];

                metadata = JsonConvert.DeserializeObject<List<DiscountSellerCustomMetaData>>(productDetailToken.ToString() ?? "[]");

            }



            //if (metadata == null)
            //{
            //    metadata = new List<DiscountSellerCustomMetaData>();
            //}

            if (type == "add" && discounttype != null)
            {
                // Adding a new record

                // Generate a random ID
                string nextId = GlobalHelper.RandomNumber().ToString();
                discountid = nextId.ToString();

                // Create a new instance of DiscountSellerCustomMetaData
                var newMetadata = new DiscountSellerCustomMetaData
                {
                    ID = int.Parse(discountid),
                    DiscountStartDate = startdate,
                    DiscountEndDate = enddate,
                    DiscountType = discounttype,
                    DiscountAmount = discountvalue,
                    IsActive = isactive,
                };

                // Add the new metadata to the existing List
                metadata.Add(newMetadata);
            }
            else if (type == "update")
            {
                // Updating an existing record
                DiscountSellerCustomMetaData existingRecord = metadata.FirstOrDefault(m => m.ID.ToString() == discountid);

                if (existingRecord != null)
                {
                    // Update the properties of the existing record
                    existingRecord.IsActive = isactive;
                }
            }

            string updatedJson = JsonConvert.SerializeObject(metadata);

            return updatedJson;
        }
        #endregion

        #region SellerDiscount-JsonDeseralized
        public static DiscountSellerAlwaysMetaData ParseMetaDataDiscountSellerMetaData(string json)
        {
            if (json == null)
            {
                return new DiscountSellerAlwaysMetaData();
            }

            JObject jsonObject = JObject.Parse(json);
            JToken productDetailToken = jsonObject["DiscountSellerAlwaysMetaData"];

            if (productDetailToken == null)
            {
                return new DiscountSellerAlwaysMetaData();
            }

            DiscountSellerAlwaysMetaData parsedData = productDetailToken.ToObject<DiscountSellerAlwaysMetaData>();
            return parsedData;
        }

        public static List<DiscountSellerCustomMetaData> ParseMetaDataDiscountCustomMetaData(string json)
        {
            if (json == null)
            {
                return new List<DiscountSellerCustomMetaData>();
            }

            JObject jsonObject = JObject.Parse(json);
            JToken productDetailToken = jsonObject["DiscountSellerCustomMetaData"];

            if (productDetailToken == null)
            {
                return new List<DiscountSellerCustomMetaData>();
            }

            // Assuming DiscountSellerCustomMetaData is an array in your JSON
            List<DiscountSellerCustomMetaData> parsedData = productDetailToken.ToObject<List<DiscountSellerCustomMetaData>>();
            return parsedData ?? new List<DiscountSellerCustomMetaData>();
        }

        public static DiscountSellerCustomMetaData ParseMetaDataDiscountCustomMetaDataUpcoming(string json)
        {
            if (json == null)
            {
                return null;
            }

            JObject jsonObject = JObject.Parse(json);
            JToken productDetailToken = jsonObject["DiscountSellerCustomMetaData"];

            if (productDetailToken == null || productDetailToken.Type == JTokenType.Null)
            {
                return null;
            }

            // Assuming DiscountSellerCustomMetaData is an array in your JSON
            List<DiscountSellerCustomMetaData> allData = productDetailToken.ToObject<List<DiscountSellerCustomMetaData>>();

            if (allData.Count == 0)
            {
                return null;
            }

            // Filter out the records based on the current date
            DateTime currentDate = DateTime.Now;
            DiscountSellerCustomMetaData upcomingDiscount = allData
                .Where(d => d.DiscountStartDate <= currentDate && d.DiscountEndDate >= currentDate && d.IsActive)
                .OrderBy(d => d.DiscountStartDate)
                .FirstOrDefault();

            return upcomingDiscount;
        }
        #endregion

        #region ProductDetail-JsonCreator
        public string ProductDetailmetadata(string description, string sku, string ean)
        {
            var metadata = new ProductDetailMetaData
            {
                DetailDescription = description,


            };

            return JsonConvert.SerializeObject(metadata);
        }


        public static ProductDetailMetaData ParseMetaDataProductDetail(string json)
        {
            if (json == null)
            {
                return new ProductDetailMetaData();
            }

            ProductDetailMetaData parsedData = JsonConvert.DeserializeObject<ProductDetailMetaData>(json);
            return parsedData;
        }


        #endregion

        #region Inventory

        public string ProductInventorymetadata(string eancode, string sku, int minqty, int maxqty, bool ismanagedinventory, bool isoutofstock)
        {
            var metadata = new ProductInventoryMetaData
            {
                EANCode = eancode,
                SKU = sku,
                MINQTY = minqty,
                MAXQTY = maxqty,
                IsManagedInventory = ismanagedinventory,
                IsOutOfStock = isoutofstock


            };

            return JsonConvert.SerializeObject(metadata);
        }


        public static ProductInventoryMetaData ParseMetaDataProductInventory(string json)
        {
            if (json == null)
            {
                return new ProductInventoryMetaData();
            }

            ProductInventoryMetaData parsedData = JsonConvert.DeserializeObject<ProductInventoryMetaData>(json);
            return parsedData;
        }

        #endregion

        #region ProductPolicy-JsonCreator
        public string ProductPolicymetadata(string returnpolicy, string cancelpolicy)
        {
            var metadata = new ProductPolicyMetaData
            {
                ReturnPolicy = returnpolicy,
                CancellationPolicy = cancelpolicy

            };

            return JsonConvert.SerializeObject(metadata);
        }



        public static ProductPolicyMetaData ParseMetaDataProductPolicy(string json)
        {
            if (json == null)
            {
                return new ProductPolicyMetaData();
            }

            ProductPolicyMetaData parsedData = JsonConvert.DeserializeObject<ProductPolicyMetaData>(json);
            return parsedData;
        }



        #endregion

        #region ProductShipping-JsonCreator
        public string ProductShippingmetadata(bool isfreeshipping, string shippingweight, string shippinglength, string shippingwidth, string shippingheight, decimal shippingaddoncharges)
        {
            var metadata = new ProductShippingMetaData
            {
                IsFreeShipping = isfreeshipping,
                ShippingWeight = shippingweight,
                ShippingHeight = shippingheight,
                ShippingWidth = shippingwidth,
                ShippingLength = shippinglength,
                ShippingAddOnCharges = shippingaddoncharges


            };

            return JsonConvert.SerializeObject(metadata);
        }



        public static ProductShippingMetaData ParseMetaDataProductShipping(string json)
        {
            if (json == null)
            {
                return new ProductShippingMetaData();
            }

            ProductShippingMetaData parsedData = JsonConvert.DeserializeObject<ProductShippingMetaData>(json);
            return parsedData;
        }



        #endregion



        #region Images-JsonCreator
        public string ProductImagesmetadata(string imageArray, string existingMetaData)
        {
            List<ImageItemMetaData> categoryList;

            // Check if the existingMetaData is null or empty
            if (string.IsNullOrEmpty(existingMetaData))
            {
                // If it's empty, create a new list
                categoryList = new List<ImageItemMetaData>();
            }
            else
            {
                // If it's not empty, deserialize the existingMetaData JSON string into a list
                categoryList = JsonConvert.DeserializeObject<List<ImageItemMetaData>>(existingMetaData);
            }

            // Split the imageArray string into individual image URLs
            string[] imageUrls = imageArray.Split(',');

            foreach (string imageUrl in imageUrls)
            {
                var categoryItem = new ImageItemMetaData
                {
                    ID = categoryList.Count + 1,
                    Image = imageUrl,
                    ImageName = GlobalHelper.FileNameFromGUID(imageUrl)
                };

                categoryList.Add(categoryItem);
            }

            string jsonList = JsonConvert.SerializeObject(categoryList);
            return jsonList;
        }
        #endregion


        #region Images-Deserialized
        public List<ImageItemMetaData> ParseMetaDataProductImages(string json)
        {
            if (json == null)
            {
                return new List<ImageItemMetaData>(); // Return an empty list
            }

            List<ImageItemMetaData> parsedData = JsonConvert.DeserializeObject<List<ImageItemMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }

        public List<ItemSliderViewModel> CreateItemSlider(string imagesmetadata, string videometadata, string mainimage)
        {
            List<ItemSliderViewModel> combinedList = new List<ItemSliderViewModel>();

            if (!string.IsNullOrEmpty(mainimage))
            {
                ItemSliderViewModel imageItem = new ItemSliderViewModel
                {
                    Image = mainimage,
                    Source = "image",
                };
                combinedList.Add(imageItem);
            }

            if (!string.IsNullOrEmpty(imagesmetadata))
            {
                List<ImageItemMetaData> imagesList = JsonConvert.DeserializeObject<List<ImageItemMetaData>>(imagesmetadata);
                foreach (var image in imagesList)
                {
                    ItemSliderViewModel imageItem = new ItemSliderViewModel
                    {
                        Image = image.Image,
                        Source = "image"
                    };
                    combinedList.Add(imageItem);
                }
            }

            if (!string.IsNullOrEmpty(videometadata))
            {
                List<ProductVideoMetaData> videoList = JsonConvert.DeserializeObject<List<ProductVideoMetaData>>(videometadata);

                // Convert ProductVideoMetaData to ImageItemMetaData by extracting URLs
                foreach (var video in videoList)
                {
                    ItemSliderViewModel imageItem = new ItemSliderViewModel
                    {
                        Image = video.URL,
                        Source = "video",
                        Poster = video.Poster ?? "../images/videoposter.png"
                    };
                    combinedList.Add(imageItem);
                }
            }



            return combinedList;
        }
        #endregion


        #region Amenities-JsonCreator
        public string amenitiesmetadata(int optionid, int amenityid, Guid productguid)
        {
            ItemListing itemListing = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);




            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<ProductAmenitiesMetaData> existingMetadata = JsonConvert.DeserializeObject<List<ProductAmenitiesMetaData>>(itemListing.AmenitiesMetaData ?? "[]");




            // Find the item to be deleted
            ProductAmenitiesMetaData ProductAmenitiesMetaData = existingMetadata.FirstOrDefault(x => x.AmenityID == amenityid && x.OptionID == optionid);




            if (ProductAmenitiesMetaData != null)
            {
                // Remove the item from the list
                existingMetadata.Remove(ProductAmenitiesMetaData);

                // Serialize the updated list back to JSON
                string json = JsonConvert.SerializeObject(existingMetadata);

                // Update the SecondaryContactMetaData property with the updated JSON
                itemListing.AmenitiesMetaData = json;

                _dbContext.ItemListings.Update(itemListing);
                _dbContext.SaveChanges();


            }
            else
            {

                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata
                int nextContactId = int.Parse(GlobalHelper.RandomNumber()) + existingMetadata.Count + 1;



                // Create a new instance of ContactMetaData
                var newMetadata = new ProductAmenitiesMetaData
                {

                    ID = nextContactId,
                    AmenityID = amenityid,
                    OptionID = optionid
                };


                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);

                string updatedJson = JsonConvert.SerializeObject(existingMetadata);

                if (newMetadata != null)
                {
                    itemListing.AmenitiesMetaData = updatedJson.ToString();
                    _dbContext.Update(itemListing);
                    _dbContext.SaveChanges();
                }
            }







            return "success";
        }



        public List<ProductAmenitiesMetaData> ParseMetaDataProductAmenities(string json)
        {
            if (json == null)
            {
                return new List<ProductAmenitiesMetaData>(); // Return an empty list
            }

            List<ProductAmenitiesMetaData> parsedData = JsonConvert.DeserializeObject<List<ProductAmenitiesMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }

        #endregion



        #region Related-JsonCreator
        public string relatedmetadata(int productid, int relatedtype, Guid productguid)
        {
            ItemListing itemListing = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);




            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<ProductRelatedMetaData> existingMetadata = JsonConvert.DeserializeObject<List<ProductRelatedMetaData>>(itemListing.RelatedItemMetaData ?? "[]");





            // Find the item to be deleted
            ProductRelatedMetaData ProductRelatedMetaData = existingMetadata.FirstOrDefault(x => x.RelatedProductid == productid && x.RelatedType == relatedtype);


            if (ProductRelatedMetaData != null)
            {
                // Remove the item from the list
                existingMetadata.Remove(ProductRelatedMetaData);

                // Serialize the updated list back to JSON
                string json = JsonConvert.SerializeObject(existingMetadata);

                // Update the SecondaryContactMetaData property with the updated JSON
                itemListing.RelatedItemMetaData = json;

                _dbContext.ItemListings.Update(itemListing);
                _dbContext.SaveChanges();


            }
            else
            {

                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata
                int nextContactId = int.Parse(GlobalHelper.RandomNumber()) + existingMetadata.Count + 1;



                // Create a new instance of ContactMetaData
                var newMetadata = new ProductRelatedMetaData
                {

                    ID = nextContactId,
                    RelatedProductid = productid,
                    RelatedType = relatedtype   // 0 related 1 is cross
                };


                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);

                string updatedJson = JsonConvert.SerializeObject(existingMetadata);

                if (newMetadata != null)
                {
                    itemListing.RelatedItemMetaData = updatedJson.ToString();
                    _dbContext.Update(itemListing);
                    _dbContext.SaveChanges();
                }
            }






            // Serialize the updated list back to JSON
            //string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return "success";
        }



        public static List<ProductRelatedMetaData> ParseMetaDataProductRelated(string json)
        {
            if (json == null)
            {
                return new List<ProductRelatedMetaData>(); // Return an empty list
            }

            List<ProductRelatedMetaData> parsedData = JsonConvert.DeserializeObject<List<ProductRelatedMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion


        #region Video-JsonCreator
        public string videometadata(int id, string provider, string source, string poster, string VideoMetaData)
        {




            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<ProductVideoMetaData> existingMetadata = JsonConvert.DeserializeObject<List<ProductVideoMetaData>>(VideoMetaData ?? "[]");


            int indexToDelete = existingMetadata.FindIndex(x => x.ID == id);


            if (indexToDelete > 0)
            {
                // Remove the item from the list
                existingMetadata.RemoveAt(indexToDelete);

                // Serialize the updated list back to JSON
                //string json = JsonConvert.SerializeObject(existingMetadata);



            }
            else
            {

                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata
                int nextContactId = int.Parse(GlobalHelper.RandomNumber()) + existingMetadata.Count + 1;



                // Create a new instance of ContactMetaData
                var newMetadata = new ProductVideoMetaData
                {

                    ID = nextContactId,
                    Provider = provider,
                    Source = source,
                    Poster = poster,
                    URL = GlobalHelper.VideoURLGenerator(provider, source)
                };


                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);



            }


            string updatedJson = JsonConvert.SerializeObject(existingMetadata);




            // Serialize the updated list back to JSON
            //string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }
        #endregion


        #region Video-Deserialized
        public List<ProductVideoMetaData> ParseMetaDataProductVideo(string json)
        {
            if (json == null)
            {
                return new List<ProductVideoMetaData>(); // Return an empty list
            }

            List<ProductVideoMetaData> parsedData = JsonConvert.DeserializeObject<List<ProductVideoMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion


        #region ITemOtherProperties-JsonCreator
        public string itemotherpropertiesmetadata(int id, string label, string value, string ExistMetaData)
        {




            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<ProductOtherPropertiesMetaData> existingMetadata = JsonConvert.DeserializeObject<List<ProductOtherPropertiesMetaData>>(ExistMetaData ?? "[]");


            int indexToDelete = existingMetadata.FindIndex(x => x.ID == id);


            if (indexToDelete > 0)
            {
                // Remove the item from the list
                existingMetadata.RemoveAt(indexToDelete);

                // Serialize the updated list back to JSON
                //string json = JsonConvert.SerializeObject(existingMetadata);



            }
            else
            {

                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata
                int nextContactId = int.Parse(GlobalHelper.RandomNumber()) + existingMetadata.Count + 1;



                // Create a new instance of ContactMetaData
                var newMetadata = new ProductOtherPropertiesMetaData
                {

                    ID = nextContactId,
                    LabelName=label,
                    ValueName=value
                };


                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);



            }


            string updatedJson = JsonConvert.SerializeObject(existingMetadata);




            // Serialize the updated list back to JSON
            //string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }
        #endregion


        #region ItemOtherProperties-Deserialized
        public List<ProductOtherPropertiesMetaData> ParseMetaDataitemotherproperties(string json)
        {
            if (json == null)
            {
                return new List<ProductOtherPropertiesMetaData>(); // Return an empty list
            }

            List<ProductOtherPropertiesMetaData> parsedData = JsonConvert.DeserializeObject<List<ProductOtherPropertiesMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion



        #region ItemHomePageDesign-JsonCreator
        public string ItemHomePageDesignmetadata(int sellingtypeid, int style, string preselectedcategory, int noofitemsdisplay, string background, string banner, bool showtitle, bool showbanner, bool showslider, bool isurl, string url)
        {
            var metadata = new ItemHomePageDesignMetaData
            {
                SellingType = sellingtypeid,

                Style = style,
                PreselectedCategory = preselectedcategory,
                NoofItemsDisplay = noofitemsdisplay,
                Background = background,
                Banner = banner,
                ShowTitle = showtitle,
                ShowBanner = showbanner,
                ShowItemSlider = showslider,

                IsURL = isurl,

                URL = url





            };

            return JsonConvert.SerializeObject(metadata);
        }


        public static ItemHomePageDesignMetaData ParseMetaDataItemHomePageDesign(string json)
        {
            if (json == null)
            {
                return new ItemHomePageDesignMetaData();
            }

            ItemHomePageDesignMetaData parsedData = JsonConvert.DeserializeObject<ItemHomePageDesignMetaData>(json);
            return parsedData;
        }


        public string homagepageDesignChild(int itemdesignid, int selectionid, string selectiontype)
        {
            ItemPageDesignChild childExisting = _dbContext.ItemPageDesignChild.FirstOrDefault(u => u.ItemPageDesignID == itemdesignid && u.SelecttionID == selectionid);









            if (childExisting != null)
            {


                _dbContext.ItemPageDesignChild.Remove(childExisting);
                _dbContext.SaveChanges();


            }
            else
            {
                ItemPageDesignChild insert = new ItemPageDesignChild();

                insert.ItemPageDesignID = itemdesignid;
                insert.SelecttionID = selectionid;
                insert.Selectiontype = selectiontype;
                insert.InsertDate = DateTime.Now;

                _dbContext.ItemPageDesignChild.Add(insert);
                _dbContext.SaveChanges();


            }






            // Serialize the updated list back to JSON
            //string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return "success";
        }



        //public List<ItemHomePageDesignSelectionMetaData> ItemPageDesignProductIDsmetadataList(string selectionarray)
        //{
        //    List<ItemHomePageDesignSelectionMetaData> productidList = new List<ItemHomePageDesignSelectionMetaData>();

        //    // Split the categoryarray string into individual category IDs
        //    string[] selectionIds = selectionarray.Split(',');

        //    foreach (string productid in selectionIds)
        //    {
        //        if (int.TryParse(productid, out int parsedselectionID))
        //        {
        //            var productItem = new ItemHomePageDesignSelectionMetaData
        //            {
        //                SelectionID = parsedselectionID,

        //            };

        //            productidList.Add(productItem);
        //        }
        //    }

        //    return productidList;
        //}
        #endregion




        #region ItemOtherMetaData -Deserialized

        public static ItemOtherMetaData ParseMetaDataItemMetaData(string json)
        {
            if (json == null)
            {
                return new ItemOtherMetaData();
            }

            ItemOtherMetaData parsedData = JsonConvert.DeserializeObject<ItemOtherMetaData>(json);
            return parsedData;
        }
        #endregion


        #region ItemOtherMetaData- Update
        #region UserOther- ExistingUpdate
        public void ItemOtherMetaDataUpdate(int itemId, string value, string type) //
        {
            ItemOtherMetaData existingMetaData = new ItemOtherMetaData();
            ItemListing up = _dbContext.ItemListings.FirstOrDefault(u => u.ItemId == itemId);

            if (up.ItemOtherMetaData != null)
            {
                if (up != null)
                {
                    existingMetaData = up.ItemOtherMetaData != null ? JsonConvert.DeserializeObject<ItemOtherMetaData>(up.ItemOtherMetaData) : null;
                }
            }
            #region CreateJsonForComments

            ///review and average rating 
            if (type == "review")
            {
                if (existingMetaData.TotalReviews == null)
                {
                    existingMetaData.TotalReviews = 0;
                }


                existingMetaData.TotalReviews = existingMetaData.TotalReviews + 1;



                if (existingMetaData.ItemAverageRating == null)
                {
                    existingMetaData.ItemAverageRating = 0;
                }

                if (existingMetaData.ItemAverageRating == 0)
                {
                    existingMetaData.ItemAverageRating = (existingMetaData.ItemAverageRating + decimal.Parse(value));
                }

                if (existingMetaData.ItemAverageRating > 0)
                {
                    existingMetaData.ItemAverageRating = (existingMetaData.ItemAverageRating + decimal.Parse(value)) / 2;
                }
            }


            //order completed
            if (type == "totalcompletedorders")
            {
                if (existingMetaData.TotalCompletedOrders == null)
                {
                    existingMetaData.TotalCompletedOrders = 0;
                }


                existingMetaData.TotalCompletedOrders = existingMetaData.TotalCompletedOrders + 1;



            }

            //record both view and click
            if (type == "viewclick")
            {
                if (existingMetaData.TotalViews == null)
                {
                    existingMetaData.TotalViews = 0;
                }

                if (existingMetaData.TotalClicks == null)
                {
                    existingMetaData.TotalClicks = 0;
                }

                existingMetaData.TotalClicks = existingMetaData.TotalClicks + 1;
                existingMetaData.TotalViews = existingMetaData.TotalViews + 1;
            }

            //Views
            if (type == "view")
            {
                if (existingMetaData.TotalViews == null)
                {
                    existingMetaData.TotalViews = 0;
                }


                existingMetaData.TotalViews = existingMetaData.TotalViews + 1;
            }
            //click
            if (type == "click")
            {
                if (existingMetaData.TotalClicks == null)
                {
                    existingMetaData.TotalClicks = 0;
                }


                existingMetaData.TotalClicks = existingMetaData.TotalClicks + 1;
            }



            up.ItemOtherMetaData = JsonConvert.SerializeObject(existingMetaData);
            _dbContext.SaveChanges();

            #endregion



        }
        #endregion
        #endregion


        #endregion




        #region CategoryLevels
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


        public List<CategorViewModel> AllCategoryLevels(/*int categoryTypeId*/)
        {
            try
            {
                List<CategorViewModel> categoryMaster = (from cm in _dbContext.CategoryMasters
                                                         orderby cm.CategoryName
                                                         where cm.IsPublished == true && cm.IsDeleted == false
                                                         //&& cm.ListingTypeID == categoryTypeId
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

                return hierarchicalCategories;
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return new List<CategorViewModel>();
            }
        }
        #endregion
    }



}
