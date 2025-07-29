$(document).ready(function () {
    cartcounter();
    
});



//int Id,
//    int quantity,
//        string instruction,
//            string type,  =credit subscription item
//                string transactiontype, free, purchased, used
//                    string orderStatus, Cart Confirm Cancel  this is for user CArt to confirm only payment is processed
//                        string orderprocesstatus rocessing Completed  Cancelled-- it should have only 3 status this is for admin
function orderCreate(Id, quantity, instruction, type, transactiontype, orderStatus, orderprocesstatus, btntype, variationmetadata) {
    // Set the product SEO URL here


    $.ajax({
        url: '/Controller/order/CreateOrder?Id=' + Id + '&quantity=' + quantity + '&instruction=' + instruction + '&type=' + type + '&transactiontype=' + transactiontype + '&orderStatus=' + orderStatus + '&orderprocesstatus=' + orderprocesstatus + "&VariationMetaData=" + variationmetadata,
        type: 'GET',

        success: function (result) {
            // Handle the success response
            //$('#wishlistContainer').html(result);
            //$('#hiddenproductid').val(productid);
            //$('#hdnclassid').val('#' + "lblfavorite");


            ///update cart counter
            cartcounter();

            // Redirect to the returned URL

            if (btntype == 'buy') {
                window.location.href = result;
            }
            else if (btntype == 'cart') {

            }

            else if (type=="credit") {
                window.location.href = result;
            }

            else if (type == "subscription") {
                window.location.href = result;
            }
        },
        error: function () {
            // Handle the error


        }
    });



}



function cartcounter() {
    $.ajax({
        url: '/Controller/order/CartCounter',
        type: 'GET',
        success: function (result) {
            // Handle the success response
            $('.cart-counter-span').html(result); // Update all elements with the class 'cart-counter-span'
        },
        error: function () {
            // Handle the error
            // You might want to provide some feedback to the user in case of an error
        }
    });
}


//invoice number is option only for basketview
function deleteCart(orderId, Invoicenumber = "") {
    $.ajax({
        url: '/Controller/order/DeleteCartItem?OrderId=' + orderId,
        type: 'POST',
        success: function (result) {
            // If the delete operation was successful, remove the corresponding row
            if (result === "success") {
                $('#aorderremove_' + orderId).closest('.row').parent().remove();


                showcart();
                cartcounter();

                if (Invoicenumber != "") {
                   
                    orderbasketview(Invoicenumber);
                }
                
            } else {
                // Handle other success responses if needed
            }
        },
        error: function () {
            // Handle the error
            // You might want to provide some feedback to the user in case of an error
        }
    });
}


function QuantityCart(orderId, quantity, instruction) {
    $.ajax({
        url: '/Controller/order/QtyUpdate?OrderId=' + orderId + "&Quantity=" + quantity + "&instruction=" + instruction,
        type: 'POST',
        success: function (result) {
            // If the update operation was successful, you can handle it here
            if (result === "success") {

                showcart();

                cartcounter();
                // Calculate the new subtotal
                //var itemAmount = parseFloat(itemamount);
                //var newSubtotal = (quantity * itemAmount);


                //console.log(itemamount);
                //console.log(quantity);
                //console.log(newSubtotal);
                // Update the subtotal display

                //var subtotalElement = $("#subtotalSpan_" + orderId);
                //if (quantity > 1) {

                //    $("#subtotalValue_" + orderId).text(numberformat(newSubtotal, 2));
                //    subtotalElement.show();
                //} else {
                //    subtotalElement.hide();
                //}


                // Handle the success response if needed
            } else {
                // Handle other success responses if needed
            }
        },
        error: function () {
            // Handle the error
            // You might want to provide some feedback to the user in case of an error
        }
    });
}



function orderbasketview(invoicenumber) {
    $.ajax({
        url: "/Controller/order/OrderBasketSummaryView/",
        type: "GET",
        data: {
            InvoiceNumber: invoicenumber,
           
        },
        success: function (response) {
            if (response.startsWith("FAIL")) {


                var q = "Title=Selection Fail&Message=Credit or Package Not Exist&Body=Selected ID=" + ID + " and Membership type " + MembershipType + " does not exist."
                window.location.href = "/Error?" + q;
                return;
            } else {
                $("#membershipbasketsummaryViewContainer").html(response);
            }
        },
        error: function () {
            console.log("Error occurred.");
        }
    });
}



function showcart() {

   
    $.ajax({
        type: 'GET',
        url: '/Controller/order/ShowCart',
        success: function (result) {
            // update the page content here


            $('#dvShowCartViewContainer').html(result);

          

        }
    });


}


function Ordershowiteminvoice(InvoiceNumber, usertype) {


    $.ajax({
        type: 'GET',
        url: '/Controller/order/ItemInvoice?InvoiceNumber=' + InvoiceNumber + "&usertype=" + usertype,
        success: function (result) {
            // update the page content here


            $('#dvShowItemIvnoiceViewContainer').html(result);



        }
    });


}


function ReviewShowItemID(itemid, itemtype) {
    // Calculate the current page number based on the number of reviews shown
    var currentPage = $('#dvShowReviewListViewContainer .review-item').length / 10;
   
    $.ajax({
        type: 'GET',
        url: '/Controller/order/ReviewListItemID?ItemID=' + itemid + '&itemtype=' + itemtype+  '&startIndex=' + (currentPage * 10),
        success: function (result) {
            // update the page content here
          

            // Append the result to the existing review list container
            $('#dvShowReviewListViewContainer').append(result);
        }
    });
}


function QAShowItemID(itemid) {
    // Calculate the current page number based on the number of reviews shown
    var currentPage = $('#dvShowQuestionAnswerViewContainer .qa-item').length / 10;

    $.ajax({
        type: 'GET',
        url: '/Controller/product/QuestionListByItemId?ItemID=' + itemid + '&startIndex=' + (currentPage * 10),
        success: function (result) {
            // update the page content here

            
            // Append the result to the existing review list container
            $('#dvShowQuestionAnswerViewContainer').append(result);
        }
    });
}

    ///is managed inventory reached

function validateQuantityInventory(qty, minQty, maxQty) {
   /* var messageDiv = document.getElementById("message");*/

    qty = parseInt(qty, 10);
    minQty = parseInt(minQty, 10);
    maxQty = parseInt(maxQty, 10);


    //console.log("quantity"+ qty);
    //console.log("min qty"+ minQty);
    //console.log("max qty" + maxQty);

    if (qty <= minQty) {
        toaster("Minimum " +minQty + " Quantity required." , "toast-success");
    } else if (qty >= maxQty) {
       

        toaster("Quantity should not be exceed " + maxQty, "toast-success");
    } else {
      
    }
}





function Advertisecreditchecked(advertisetype, itemtype, noofcreditrequired, itemid, startdate, enddate, iscustomized) {
    // Set the product SEO URL here


    $.ajax({
        url: "/controller/order/AdvertiseCreditUsage?advertisetype=" + advertisetype + "&itemtype=" + itemtype + "&noofcreditrequired=" + noofcreditrequired + "&itemid=" + itemid + "&startdate=" + startdate + "&enddate=" + enddate + "&iscustomized=" + iscustomized,
        type: 'GET',

        success: function (result) {

          
            // Handle the success response
            //$('#wishlistContainer').html(result);
            //$('#hiddenproductid').val(productid);
            //$('#hdnclassid').val('#' + "lblfavorite");


            ///update cart counter
            /* cartcounter();*/

            // Redirect to the returned URL
            if (result != "insufficient")
            { 

            window.location.href = result;
            }

        },
        error: function () {
            // Handle the error


        }
    });



}


function AdvertiseAdmin(advertisetype, itemtype, itemid, noofDays, iscustomized) {
    // Set the product SEO URL here


    $.ajax({
        url: "/controller/order/AdvertiseAdmin?advertisetype=" + advertisetype + "&itemtype=" + itemtype + "&itemid=" + itemid + "&noofdays=" + noofDays +  "&iscustomized=" + iscustomized,
        type: 'GET',

        success: function (result) {
            // Handle the success response
            //$('#wishlistContainer').html(result);
            //$('#hiddenproductid').val(productid);
            //$('#hdnclassid').val('#' + "lblfavorite");


            ///update cart counter
            /* cartcounter();*/

            // Redirect to the returned URL


            window.location.href = result;

        },
        error: function () {
            // Handle the error


        }
    });



}
function Advertisepayment(advertisetype, itemtype, itemid, startdate, enddate, iscustomized, amount) {
    // Set the product SEO URL here


    $.ajax({
        url: "/controller/order/AdvertisePayment?advertisetype=" + advertisetype + "&itemtype=" + itemtype + "&itemid=" + itemid + "&startdate=" + startdate + "&enddate=" + enddate + "&iscustomized=" + iscustomized + "&amount=" + amount,
        type: 'GET',

        success: function (result) {
            // Handle the success response
            //$('#wishlistContainer').html(result);
            //$('#hiddenproductid').val(productid);
            //$('#hdnclassid').val('#' + "lblfavorite");


            ///update cart counter
           /* cartcounter();*/

            // Redirect to the returned URL

          
                window.location.href = result;
          
        },
        error: function () {
            // Handle the error


        }
    });



}


function walletavailable() {
    $.ajax({
        url: "/controller/order/WalletAvailable",
        type: 'GET',
       
        success: function (data) {

            console.log(data.availablewallet);
            $("#SpanAvailableWallet").text(data.availablewallet);

        },
        error: function () {
            // Handle the error


        }
    });
}


function loadshippingaddresslist() {
    $.ajax({
        type: 'GET',
        url: '/Controller/Shipping/shippinglist',
        success: function (result) {

            $('#shippingContainerList').html(result);
        },
        error: function () {
            alert('Error loading quick view.');
        }
    });
}