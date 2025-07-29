

//on click show load partial view of credit form any where application
$(document).on('click', '#btnShowCredit', function (e) {
    e.preventDefault();
    //$('#loginFormContainer').empty();


    creditpackageview();
});

$(document).on('click', '#aclosecredit', function (e) {
    e.preventDefault();
    //$('#loginFormContainer').empty();


    $("#membershipViewContainer").empty();
});


//on click show load partial view of subscription form any where application
$(document).on('click', '#btnShowSubscription', function (e) {
    e.preventDefault();
    //$('#loginFormContainer').empty();


    subscriptionpackageview();
});

//on click show load partial view of credit/_creditcounter form any where application




function creditpackageview() {
    $.ajax({
        url: "/Controller/Membership/CreditPackageView",
        type: "GET",
        success: function (partialView) {
            $("#membershipViewContainer").html(partialView);
        },
        error: function () {
            console.log("Error occurred.");
        }
    });
}



function subscriptionpackageview() {
    $.ajax({
        url: "/Controller/Membership/SubscriptionPackageView",
        type: "GET",
        success: function (partialView) {
            $("#membershipViewContainer").html(partialView);
        },
        error: function () {
            console.log("Error occurred.");
        }
    });
}




///depreciate this and shifted to orderjs
//function membershipbasketview(ID, MembershipType) {
//    $.ajax({
//        url: "/Controller/Membership/MembershipBasketSummaryView/",
//        type: "GET",
//        data: {
//            ID: ID,
//            MembershipType: MembershipType
//        },
//        success: function (response) {
//            if (response.startsWith("FAIL")) {
                
               
//                var q = "Title=Selection Fail&Message=Credit or Package Not Exist&Body=Selected ID=" + ID + " and Membership type " + MembershipType + " does not exist."
//                window.location.href = "/Error?" + q;
//                return;
//            } else {
//                $("#membershipbasketsummaryViewContainer").html(response);
//            }
//        },
//        error: function () {
//            console.log("Error occurred.");
//        }
//    });
//}




function creditchecked(cipherText, creditType) {
    var key = null;
    $.ajax({
        type: "GET",
        url: "/controller/membership/CreditUsage?cipherText="+cipherText+"&creditType=" + creditType,
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (response) {
            key = response;

            console.log(key);

            creditcountPartialView();

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
            console.log("Response status: " + xhr.status);
            console.log("Response text: " + xhr.responseText);
            console.log("creditchecked does not exist.");
        }
    });
    if (key == null) {
        console.log("creditchecked does not exist.");
        //window.location.href = "/ErrorPage.html";
    }
    return key;
};

function creditDeductionNumber(creditType)
{
    var key = null;
    $.ajax({
        type: "GET",
        url: "/controller/membership/CreditDeduction?creditType=" + creditType ,
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (response) {
            key = response;

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
            console.log("Response status: " + xhr.status);
            console.log("Response text: " + xhr.responseText);
            console.log("credit Deduction does not exist.");
        }
    });
    if (key == null) {
        console.log("credit Deduction does not exist.");
        //window.location.href = "/ErrorPage.html";
    }
    return key;
}


function userAvailableCredit() {
    var key = null;
    $.ajax({
        type: "GET",
        url: "/controller/membership/AvailbleUserCredit",
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (response) {
           
            key = response;

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
            console.log("Response status: " + xhr.status);
            console.log("Response text: " + xhr.responseText);
            console.log("credit Deduction does not exist.");
        }
    });
    if (key == null) {
        console.log("credit Deduction does not exist.");
        //window.location.href = "/ErrorPage.html";
    }
    return key;
}


function creditcountPartialView() {
    $.ajax({
        url: "/Controller/Membership/CreditAvailableCountPartialView",
        type: "GET",
        success: function (partialView) {
            $("#creditavailableViewContainer").html(partialView);
        },
        error: function () {
            console.log("Error occurred.");
        }
    });
}




