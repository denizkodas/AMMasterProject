function mtnpartialview() {
    $.ajax({
        url: "/Controller/PaymentGateway/MTNBuyerPhoneView",
        type: "GET",
        success: function (partialView) {
            $("#dvmtnviewContainer").html(partialView);
        },
        error: function () {
            console.log("Error occurred mtn container.");
        }
    });
}
function mtnrequesttopay(partyIdType, partyId, amount, currency, orderid) {
    $.ajax({
        url: "/Controller/PaymentGateway/RequestToPay",
        type: "POST",
        data: {
            partyIdType: partyIdType,
            partyId: partyId,
            amount: amount,
            currency: currency,
            orderid: orderid
        },
        success: function (response, textStatus, xhr) {
            console.log(response.status);
            if (response.status === 202) {
                $('#dvWaitingforApproval').text('Please approve the transaction on your phone.');
                var membershiptype = $("#spanMembershipType").text();
                startTimer(60, $('#dvWaitingforApproval'), membershiptype);
            } else {
                $('#dvWaitingforApproval').text(response.data || response.message);
            }
        },
        error: function (xhr) {
            console.log("Error occurred in mtnrequesttopay.");
            console.log('Error Status:', xhr.status);
            console.log('Error Response:', xhr.responseText);
        }
    });
}

function startTimer(duration, display, membershiptype) {
    $("#btnmtn").hide();
    var timer = duration, minutes, seconds;
    var validationIntervalId = null;

    validationIntervalId = setInterval(function () {
        mtntranscationvalidation(membershiptype, validationIntervalId);
    }, 6000);  // Start validation attempts

    var interval = setInterval(function () {
        minutes = parseInt(timer / 60, 10);
        seconds = parseInt(timer % 60, 10);

        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;

        display.html(`Please approve the transaction on your phone.<br><b>Time remaining: ${minutes}:${seconds}</b>`);

        if (--timer < 0) {
            clearInterval(interval);
            clearInterval(validationIntervalId);  // Clear validation attempts on timer expiry
            display.text("The time for approval has expired. Please try again later");
            $("#btnmtn").show();
        }
    }, 1000);
}

function mtntranscationvalidation(membershiptype, validationIntervalId) {
    $('#dvattemptingmtn').text('Attempting to verify payment...').show();

    $.ajax({
        url: "/Controller/PaymentGateway/mtntranscationvalidation",
        type: "GET",
        data: {
            membershiptype: membershiptype
        },
        success: function (response) {
            console.log(response);
            if (response.status === 'successful') {
                clearInterval(validationIntervalId);  // Stop further validation attempts on success
                $('#dvattemptingmtn').text('Payment verified!').fadeOut(5000);
                setTimeout(function () {
                    window.location.href = response.returnurl;
                }, 3000);
            } else {
                $('#dvattemptingmtn').text('Payment not verified.').fadeOut(5000);
            }
        },
        error: function (xhr) {
            console.log("Error occurred in mtntranscationvalidation.");
            console.log('Error Status:', xhr.status);
            console.log('Error Response:', xhr.responseText);
            $('#dvattemptingmtn').text('Failed to verify payment.').show();
        }
    });
}


//function mtnrequesttopay(partyIdType, partyId, amount, currency, orderid) {
//    $.ajax({
//        url: "/Controller/PaymentGateway/RequestToPay",
//        type: "POST",
//        data: {
//            partyIdType: partyIdType,
//            partyId: partyId,
//            amount: amount,
//            currency: currency,
//            orderid: orderid
//        },
//        success: function (response, textStatus, xhr) { // added xhr parameter to success function
//            console.log(response.status);
//            if (response.status === 202) { // check the actual HTTP status code
//                $('#dvWaitingforApproval').text('Please approve the transaction on your phone.');
//                startTimer(60, $('#dvWaitingforApproval'));

//                var membershiptype = $("#spanMembershipType").text();
//                mtntranscationvalidation(membershiptype);
//            } else {
//                $('#dvWaitingforApproval').text(response.data || response.message);
//            }
//        },
//        error: function (xhr) {
//            console.log("Error occurred in mtnrequesttopay.");
//            console.log('Error Status:', xhr.status);
//            console.log('Error Response:', xhr.responseText);
//        }
//    });
//}

//function startTimer(duration, display) {
//    $("#btnmtn").hide();
//    var timer = duration, minutes, seconds;
//    var interval = setInterval(function () {
//        minutes = parseInt(timer / 60, 10);
//        seconds = parseInt(timer % 60, 10);

//        minutes = minutes < 10 ? "0" + minutes : minutes;
//        seconds = seconds < 10 ? "0" + seconds : seconds;

       

//        display.html(`Please approve the transaction on your phone.<br><b>Time remaining: ${minutes}:${seconds}</b>`);


//        if (--timer < 0) {
//            clearInterval(interval);
//            display.text("The time for approval has expired. Please try again later");
//            $("#btnmtn").show();
           
//        }
//    }, 1000);
//}


//function mtntranscationvalidation(membershiptype) {
//    // Show payment attempting message
//    $('#dvattemptingmtn').text('Attempting to verify payment...').show();

//    $.ajax({
//        url: "/Controller/PaymentGateway/mtntranscationvalidation",
//        type: "GET",
//        data: {
//            membershiptype: membershiptype
         
//        },
//        success: function (response) {
//            console.log(response);
//            // Check if the response status is 200 (OK)
//            if (response.status === 'successful') {
                
//                console.log(response.status)
//                    console.log(response.returnurl);
//                // You might want to update or clear the message if the payment is verified
//                $('#dvattemptingmtn').text('Payment verified!').fadeOut(5000);
              

//                setTimeout(function () {
//                    window.location.href = response.returnurl;
//                }, 3000); //


//            } else {
//                // If response is not 200, update the message to show not verified
//                $('#dvattemptingmtn').text('Payment not verified.').fadeOut(5000);

//                setTimeout(function () {
//                    mtntranscationvalidation(membershiptype);
//                }, 6000); //
               
//            }
//        },
//        error: function (xhr) {
//            console.log("Error occurred in mtntranscationvalidation.");
//            console.log('Error Status:', xhr.status);
//            console.log('Error Response:', xhr.responseText);
//            // Update the message to reflect the error state
//            $('#dvattemptingmtn').text('Failed to verify payment.').show();
//        }
//    });
//}