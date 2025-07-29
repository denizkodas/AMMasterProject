function sellerlist(searchstring) {
    $.ajax({
        url: "/Controller/product/SellerListing",
        type: "GET",
        data: { searchstring: searchstring },
        success: function (partialView) {
            $("#dvSellerListingContainer").html(partialView);
        },
        error: function () {
            console.log("Error occurred.");
        }
    });
}

function sendfollow(loginid, sellerid) {
    try {
        // send AJAX request to follow user
        $.ajax({
            type: "POST",
            cache: false,
            url: "/controller/user/followuser",
            dataType: "json",
            data: { loginid: loginid, sellerid: sellerid },
            success: function (response) {
                // Handle success response here
                console.log(response);
            },
            error: function (response) {
                console.log(response.responseText);
            }
        });
    } catch (error) {
        console.log(error);
    }
}

function checkFollowStatus($element, loginid, sellerid) {
    $.ajax({
        url: '/controller/user/FollowUserStatus',
        method: 'GET',
        data: {
            loginid: loginid,
            sellerid: sellerid
        },
        success: function (response) {
            if (response.isFollowing) {
                $element.removeClass('s-un-fav').addClass('s-fav');
            } else {
                $element.removeClass('s-fav').addClass('s-un-fav');
            }
        },
        error: function (error) {
            console.error(error);
        }
    });
}