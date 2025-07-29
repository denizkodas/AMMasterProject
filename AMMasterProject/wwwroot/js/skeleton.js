$(document).ready(function () {

    //headerlinkviewskeleton();

    //categoryviewskeleton();

    //herobannerskeleton();

    //cmsviewhomeskeleton();

    //productboosthomeskeleton();

    //productdiscounthomeskeleton();
});

function headerlinkviewskeleton()
{
    var skeletonCount = 5; // Number of skeleton blocks to create

    for (var i = 0; i < skeletonCount; i++) {
        var skeletonDiv = $('<div class="skeleton headerlinkview"></div>');
        $('#dvHeaderLinkViewContainer').append(skeletonDiv);
    }
}

function categoryviewskeleton() {
    var skeletonCount = 5; // Number of skeleton blocks to create

    for (var i = 0; i < skeletonCount; i++) {
        var skeletonDiv = $('<div class="skeleton categoriescontainer"></div>');
        $('#dvCategoriesContainer').append(skeletonDiv);
    }
}

function herobannerskeleton() {
   

   
        var skeletonDiv = $('<div class="skeletonfade herobannercontainer"></div>');
        $('#dvHeroBannerContainer').append(skeletonDiv);
    
}

function cmsviewhomeskeleton() {



    var skeletonDiv = $('<div class="skeletonfade cmsviewhomecontainer"></div>');
    $('#dvCMSViewhomeContainer').append(skeletonDiv);

}

function productboosthomeskeleton() {
    var skeletonCount = 5; // Number of skeleton blocks to create

    for (var i = 0; i < skeletonCount; i++) {
        var skeletonDiv = $('<div class="productboosthomecontainer skeleton"></div>');
        $('#dvProductBoostHomeContainer').append(skeletonDiv);
    }
}

function productdiscounthomeskeleton() {
    var skeletonCount = 8; // Number of skeleton blocks to create

    for (var i = 0; i < skeletonCount; i++) {
        var skeletonDiv = $('<div class="productdiscounthomecontainer skeleton container"></div>');
        $('#dvProductDiscountContainer').append(skeletonDiv);
    }
}