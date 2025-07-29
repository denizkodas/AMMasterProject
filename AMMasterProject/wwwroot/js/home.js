$(document).ready(function ()
{

    
   

      
            bloghome();
           

      
            partnerhome();
           

        
            cmsviewhomefooter("homefooter");
            

    
            featuredseller();
           

       
            footerlinkview();
           

       
            socialmediafooter();
          
    headerlinkview();

    homecategories();

    homecategorieswithBanners();

    categoriesfirstlevel();

    categoriesincludeinmenu();

    // Check if userdefinedscripts is cached in localStorage
    if (!isItemExpired('userdefinedscripts')) {
        userdefinedscripts("");
    } else {
        userdefinedscripts("Root Style- Theme Color");
        setTimeout(function () {
            userdefinedscripts("");
        }, 10000);
    }

    //if (!isItemExpired('headerlinkview')) {
    //    headerlinkview();
    //} else {
    //    setTimeout(headerlinkview, 10000);
    //}

    //if (!isItemExpired('homecategories')) {
    //    homecategories();
    //    homecategorieswithBanners();
    //} else {
    //    setTimeout(homecategories, 3000);
    //    setTimeout(homecategorieswithBanners, 3000);
    //}

    //if (!isItemExpired('categoriesfirstlevel')) {
    //    categoriesfirstlevel();
    //} else {
    //    setTimeout(categoriesfirstlevel, 3000);
    //}

    //if (!isItemExpired('categoriesincludeinmenu')) {
    //    categoriesincludeinmenu();
    //} else {
    //    setTimeout(categoriesincludeinmenu, 3000);
    //}

    regionalsettingfooter();

    setTimeout(function () {
        inboxconter();
    }, 1000);
});



  ///Categories


//element in focus fuction

// Function to check if an element is visible in the viewport


function isElementInViewport(el) {

    var rect = $(el)[0].getBoundingClientRect();
    return (
        rect.top >= 0 &&
        rect.left >= 0 &&
        rect.bottom <= $(window).height() &&
        rect.right <= $(window).width()
    );
} 

//load other scripts after 10 seconds

function userdefinedscripts(scriptkey) {
    $.ajax({
        url: "/Controller/home/userdefinedscripts?scriptkey=" + scriptkey,
        type: "GET",
        success: function (partialView) {
          
            $("#dvUserDefinedScriptsContainer").html(partialView);

            //setItemWithExpiry('userdefinedscripts', 'true', 24); // 24 hours
        },
        error: function () {
            console.log("Error occurre user defined script.");
        }
    });
}


function headerlinkview() {

    $("#dvHeaderLinkViewContainer").empty();
    $.ajax({
        url: "/Controller/home/headerlinkview",
        type: "GET",
        success: function (partialView) {
          
            $("#dvHeaderLinkViewContainer").html(partialView);

            //setItemWithExpiry('headerlinkview', 'true', 24); // 24 hours
        },
        error: function () {
            console.log("Error occurred header link view.");
        }
    });
}

function footerlinkview() {
    $.ajax({
        url: "/Controller/home/footerlinkview",
        type: "GET",
        success: function (partialView) {

            $("#dvFooterLinkViewContainer").html(partialView);

           /* setItemWithExpiry('headerlinkview', 'true', 24);*/ // 24 hours
        },
        error: function () {
            console.log("Error occurred footer link view.");
        }
    });
}

function homecategories() {
    $.ajax({
        url: "/Controller/home/homecategories",
        type: "GET",
        success: function (partialView) {
           
            $("#dvCategoriesContainer").html(partialView);

            //setItemWithExpiry('homecategories', 'true', 1); // 24 hours
        
        },
        error: function () {
            console.log("Error occurred home categories.");
        }
    });
}

function homecategorieswithBanners() {
    $.ajax({
        url: "/Controller/home/homecategorieswithbanner",
        type: "GET",
        success: function (partialView) {

            $("#dvCategoriesWithBannerContainer").html(partialView);

            //setItemWithExpiry('homecategories', 'true', 1); // 24 hours

        },
        error: function () {
            console.log("Error occurred home categories with banner.");
        }
    });
}

function categoriesfirstlevel() {
    $.ajax({
        url: "/Controller/home/firstlevelcategories",
        type: "GET",
        success: function (partialView) {

            $("#dvCategoriesFirstLevelContainer").html(partialView);

            //setItemWithExpiry('categoriesfirstlevel', 'true', 1); // 24 hours

        },
        error: function () {
            console.log("Error occurred home Main categories.");
        }
    });
}

function categoriesincludeinmenu() {
    $.ajax({
        url: "/Controller/home/IncludeInMenucategories",
        type: "GET",
        success: function (partialView) {

            $("#dvCategoriesIncludeInMenuContainer").html(partialView);

           // setItemWithExpiry('categoriesincludeinmenu', 'true', 24); // 24 hours

        },
        error: function () {
            console.log("Error occurred home Main categoriesincludeinmenu.");
        }
    });
}

function categoryDirectory() {
    $.ajax({
        url: "/Controller/home/homecategories",
        type: "GET",
        success: function (partialView) {

            $("#dvCategoriesDirectoryContainer").html(partialView);

           

        },
        error: function () {
            console.log("Error occurred category directory");
        }
    });
}

function herobanner() {
    $.ajax({
        url: "/Controller/home/herobanner",
        type: "GET",
        success: function (partialView) {

            $("#dvHeroBannerContainer").html(partialView);

            // Set herobanner flag in localStorage
         
            //setItemWithExpiry('herobanner', 'true', 24); // 24 hours
        },
        error: function () {
            console.log("Error occurred hero banner");
        }
    });
}

function productboosthome() {
    $.ajax({
        url: "/Controller/home/productboosthome",
        type: "GET",
        success: function (partialView) {

            $("#dvProductBoostHomeContainer").html(partialView);


            //setItemWithExpiry('productboosthome', 'true', 24); // 24 hours
        },
        error: function () {
            console.log("Error occurred product boost home");
        }
    });
}

function productdiscounthome() {
    $.ajax({
        url: "/Controller/home/productdiscounthome",
        type: "GET",
        success: function (partialView) {

            $("#dvProductDiscountContainer").html(partialView);


            //setItemWithExpiry('productdiscounthome', 'true', 24); // 24 hours
        },
        error: function () {
            console.log("Error occurred product discount home");
        }
    });
}

function bloghome() {
    $.ajax({
        url: "/Controller/home/bloghome",
        type: "GET",
        success: function (partialView) {

            $("#dvBlogHomeContainer").html(partialView);


            //setItemWithExpiry('bloghome', 'true', 24); // 24 hours
        },
        error: function () {
            console.log("Error occurred blog home");
        }
    });
}

function partnerhome() {
    $.ajax({
        url: "/Controller/home/partnerhome",
        type: "GET",
        success: function (partialView) {

            $("#dvPartnerHomeContainer").html(partialView);


           // setItemWithExpiry('partnerhome', 'true', 24); // 24 hours
        },
        error: function () {
            console.log("Error occurred partner home");
        }
    });
}
function cmsview(cmskey) {
    $.ajax({
        url: "/Controller/home/cmsview?cmskey=" + cmskey,
        type: "GET",
        success: function (partialView) {

            $("#dvCMSViewhomeContainer").html(partialView);


           // setItemWithExpiry('cmsviewhome', 'true', 24); // 24 hours
        },
        error: function () {
            console.log("Error occurred cms" + cmskey);
        }
    });
}


function cmsviewhomefooter(cmskey) {
    $.ajax({
        url: "/Controller/home/cmsview?cmskey=" + cmskey,
        type: "GET",
        success: function (partialView) {

            $("#dvCMSViewhomefooterContainer").html(partialView);


           
        },
        error: function () {
            console.log("Error occurred cms" + cmskey);
        }
    });
}

function socialmediafooter() {
    $.ajax({
        url: "/Controller/home/socialmediafooter" ,
        type: "GET",
        success: function (partialView) {

            $("#dvSocialMediaFooterContainer").html(partialView);

            

        },
        error: function () {
            console.log("Error occurred social media footer" );
        }
    });
}


function featuredseller() {
    $.ajax({
        url: "/Controller/home/featuredsellerhome",
        type: "GET",
        success: function (partialView) {

            $("#dvFeaturedSellerHomeContainer").html(partialView);



        },
        error: function () {
            console.log("Error occurred featured seller home");
        }
    });
}
function regionalsettingfooter() {
    $.ajax({
        url: "/Controller/home/regionalsettingfooter",
        type: "GET",
        success: function (partialView) {

            $("#dvRegionalSettingFooterContainer").html(partialView);



        },
        error: function () {
            console.log("Error occurred regional setting" );
        }
    });
}

function inboxconter() {
    $.ajax({
        url: "/Controller/home/inboxcounterheader",
        type: "GET",
        success: function (partialView) {

            $("#dvInboxCounterViewContainer").html(partialView);



        },
        error: function () {
            console.log("Error occurred inbox counter header");
        }
    });
}

function homepageitempagesetup() {
    $.ajax({
        type: 'GET',
        url: '/Controller/product/Itemhomepagesetup',
        success: function (result) {
            // Update the content of the specified element with the received data
            $('#dvProductHomePageContainer').html(result);
        },
        error: function () {
            // Handle the error case
            // alert('Error loading product home page controller.');
        }
    });
}