// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

///Date functions


$(document).ready(function () {

    // Check if the functions have already been executed in this session
    var hasExecuted = sessionStorage.getItem('hasExecuted');

    if (!hasExecuted) {
        // Execute the functions immediately
        lastseenupdate();
        validatelogin();

        // Mark that the functions have been executed in this session
        sessionStorage.setItem('hasExecuted', 'true');
    }
    // Schedule the function to run every 10 minutes (600,000 milliseconds)
    setInterval(lastseenupdate, 600000);
    setInterval(validatelogin, 600000);

    
}); 
$(document).ready(function () {
    // Delay the assignment of the event handler
    setTimeout(function () {
        $(".clickable-button").on("click", function () {
            var $button = $(this); // Store a reference to the button

         
            // Store the existing button text
            var originalText = $button.text();

            // Add the class to the clicked button
            $button.addClass('btn-processing');
            $button.text('Processing');

            // Remove the class and restore the original text after 3 seconds (3000 milliseconds)
            setTimeout(function () {
                $button.removeClass("btn-processing");
                $button.text(originalText); // Restore the original text
               
            }, 5000);
        });
    }, 2000); // Delay for 2 seconds after page load
});


///Back button for mobile view

$(document).ready(function () {
    // Add a click event handler to the back button
    $("#lnkback").on("click", function () {
        // Use the history object to go back to the previous page
        window.history.back();
    });
});



function convertDate(dateString) {
    var timestamp = dateString.match(/\d+/)[0];
    var date = new Date(parseInt(timestamp));
    return formatDate(date);
};


function getTimeAgo(dateTimeStr) {
    var dateTime = new Date(dateTimeStr);
    var now = new Date();
    var diff = now - dateTime;
    var diffInDays = Math.floor(diff / (1000 * 60 * 60 * 24));
    var diffInHours = Math.floor(diff / (1000 * 60 * 60));

    if (diffInDays > 1) {
        return diffInDays + " days ago, on " + dateTime.toLocaleDateString();
    } else if (diffInDays == 1) {
        return "Yesterday, on " + dateTime.toLocaleDateString();
    } else if (diffInHours > 1) {
        return diffInHours + " hours ago";
    } else {
        return "Less than an hour ago";
    }
};

function formatDate(dateString) {
    var formattedDate = new Date(dateString).toLocaleString('en-us', {
        month: 'short',
        day: 'numeric',
        year: 'numeric'
    });
    return formattedDate.replace(',', '');
}


///Reverse time from cookie

function getCookieExpiry(cookieName) {
    var promise1 = cookieStore.getAll().then(function (cookies) {
        return cookies[0].expires;
    });

    promise1.then(function (value) {
        alert(new Date(value).toLocaleDateString('en-US'));
    });
}


function showExpiryTimer() {
    // Retrieve the expiry time from the verification code cookie
    var verificationCodeCookie = document.cookie
        .split(';')
        .find(cookie => cookie.trim().startsWith('VerificationCode='));
    alert(verificationCodeCookie);
    var expiryTime = null;

    if (verificationCodeCookie) {
        var cookieParts = verificationCodeCookie.split('=');
        if (cookieParts.length === 2) {
            var verificationCode = decodeURIComponent(cookieParts[1]);
            var currentTime = new Date();
            var remainingTime = Math.floor((new Date(verificationCode) - currentTime) / 1000); // Remaining time in seconds

            // Display the remaining time in a reverse timer format
            var minutes = Math.floor(remainingTime / 60);
            var seconds = remainingTime % 60;

            // Format the minutes and seconds with leading zeros if necessary
            var formattedMinutes = ("0" + minutes).slice(-2);
            var formattedSeconds = ("0" + seconds).slice(-2);

            var timerText = formattedMinutes + ":" + formattedSeconds;

            // Display the timer text in your desired element
            $("#verificationcodeTimer").text(timerText);

            // Return the remaining time in seconds
            return remainingTime;
        }
    }

    return 0; // Default value if expiry time is not available
}

function updateCharacterCount(textarea, spanCharactercountlabel) {
    var maxLength = parseInt(textarea.getAttribute("maxlength"));
    var currentLength = textarea.value.length;

    var label = spanCharactercountlabel || document.getElementById("characterCountLabel");
    label.innerText = "Characters used: " + currentLength + "/" + maxLength;
}


function isValidEmail(email) {
    // Email validation regex
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
   
    // Test if email matches the regex pattern
    return emailRegex.test(email);
}

function isValidMobile(mobile) {
    // Mobile number validation regex
    var regex = /^\d{5,15}$/; // Regex for phone number without country code

    // Test if mobile number matches the regex pattern
    var isValid = regex.test(mobile);

    return isValid;
}
//function isValidMobile(mobile) {
//    // Mobile number validation regex
//    var regex = /^\+\d{1,3}\s?\d{1,14}$/; // Regex for country code + phone number

//    // Test if mobile number matches the regex pattern
//    var isValid = regex.test(mobile);

  
//    return isValid;
//}
$(document).ready(function () {
    // Specify the target action attribute
    var targetAction = '/controller/master/formdata';

    // Find all forms with the specified action attribute
    $('form[action="' + targetAction + '"]').each(function () {
        // Add a hidden field with the name "firstname" and default value
        $(this).append('<input type="hidden" name="First Name" value="" />');
        $(this).append('<input type="hidden" name="Last Name" value="" />');
        $(this).append('<input type="hidden" name="Fax" value="" />');
    });
});

function isValidChar(input) {
    // Character validation regex (uppercase and lowercase letters only, no space)
    var regex = /^[A-Za-z]+$/;

    // Test if the input matches the regex pattern
    var isValid = regex.test(input);

    return isValid;
}

///Cookie
function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
}

//function setCookie(cookieName, cookieValue, daysToExpire) {
//    var date = new Date();
//    date.setTime(date.getTime() + (daysToExpire * 24 * 60 * 60 * 1000));
//    var expires = "; expires=" + date.toUTCString();
//    document.cookie = cookieName + "=" + cookieValue + expires + "; path=/";
//};

function setCookie(cookieName, cookieValue, daysToExpire) {
    var date = new Date();
    date.setTime(date.getTime() + (daysToExpire * 24 * 60 * 60 * 1000));
    var expires = "; expires=" + date.toUTCString();

    // Automatically determine the base domain
    var hostname = window.location.hostname;
    var domainParts = hostname.split('.');
    var baseDomain = domainParts.length > 2 ? domainParts.slice(-2).join('.') : hostname;

    var domainAttribute = "; domain=." + baseDomain;  // Ensure it starts with a dot for wider availability
   /* console.log(domainAttribute);*/
    document.cookie = cookieName + "=" + cookieValue + expires + domainAttribute + "; path=/";
}

function setSessionCookie(cookieName, cookieValue, domain) {
    var cookieDomain = domain ? "; domain=" + domain : ""; // Add domain if provided
    document.cookie = cookieName + "=" + cookieValue + "; path=/" + cookieDomain;

   
}
function removeCookie(cookieName) {
    var pastDate = new Date(0).toUTCString(); // Set the date to the past
    document.cookie = cookieName + "=" + "; expires=" + pastDate + "; path=/";
}
function getBaseDomain() {
    var hostname = window.location.hostname;
    var domainParts = hostname.split('.');

    // This assumes you're always working with at least a second-level domain (e.g., example.com)
    if (domainParts.length > 2) {
        // Return the last two parts of the hostname joined as a base domain
        // Adjust this if you deal with TLDs that include dots (like .co.uk or .com.au)
        return '.' + domainParts.slice(-2).join('.');
    } else {
        // For local environments like 'localhost' or domains without subdomains
        return hostname;
    }
}

function removeAllCookiesByName(cookieName) {
    var cookies = document.cookie.split(';');
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i].trim(); // Trim whitespace
        if (cookie.indexOf(cookieName + '=') === 0) {
            var expirationDate = new Date();
            expirationDate.setFullYear(expirationDate.getFullYear() - 1);
            document.cookie = cookieName + '=; expires=' + expirationDate.toUTCString() + '; path=/';
        }


    }
}



///set current url in cookie

function setCurrentURL() {
    var expires = "";
    var days = 10;
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    var currentUrl = window.location.href;
    var encodedUrl = encodeURIComponent(currentUrl);

    // Check if the cookie already exists
    var existingCookie = document.cookie.replace(/(?:(?:^|.*;\s*)returnurl\s*\=\s*([^;]*).*$)|^.*$/, "$1");
    if (existingCookie) {
        // Replace the existing cookie value with the new URL
        document.cookie = "returnurl" + "=" + encodedUrl + expires + "; path=/";
    } else {
        // Create a new cookie with the URL
        document.cookie = "returnurl" + "=" + encodedUrl + expires + "; path=/";
    }
}
//SEO

function generateSeoFriendlyText(text) {
    // Replace special characters with empty string
    var seoFriendlyText = text.toLowerCase().replace(/[^\w\s-]/g, '');

    // Replace spaces with hyphens
    seoFriendlyText = seoFriendlyText.replace(/\s+/g, '-');

    return seoFriendlyText;
}




function numberformat(number, decimalPlaces = 2) {

   
    var formattedNumber = number.toLocaleString('en-US', {
        style: 'decimal',
        maximumFractionDigits: decimalPlaces,
        minimumFractionDigits: decimalPlaces,
        useGrouping: true
    });

    return formattedNumber;
};

///number to words
function formatNumberInMillions(number) {
    var million = 1000000;
    var result = (number / million).toFixed(2);
    if (result >= 0.1 && result < 1000) {
        return result.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " million";
    } else if (result >= 1000) {
        var billion = 1000000000;
        result = (number / billion).toFixed(2);
        return result.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " billion";
    } else {
        return "";
    }
};


//file extension and upload functions

function getAttachmentClass(fileName) {
    const ext = fileName.split(".").pop().toLowerCase();
    if (ext === "jpg" || ext === "jpeg" || ext === "png" || ext === "gif") {
        return "file-png";
    } else if (ext === "pdf") {
        return "file-pdf";
    } else if (ext === "doc" || ext === "docx") {
        return "file-doc";
    } else if (ext === "ppt" || ext === "pptx") {
        return "file-pptx";
    } else if (ext === "txt") {
        return "file-txt";
    } else {
        return "file-png";
    }
}


//QUERY STRING

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return "";
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
};




///Encryption

function encryption(value) {

    
    var key = null;
    $.ajax({
        type: "GET",
        url: "/controller/master/encryption?value="+value,
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        async: false,
        success: function (response) {
          
            key = response;
           
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
            console.log("key does not exist.");
            //window.location.href = "/Error/ErrorPage.html";
        }
    });
    if (key == null) {
        console.log("encrypt does not exist.");
        //window.location.href = "/ErrorPage.html";
    }
    return key;
};




function decryption(cipherText) {
    var key = null;
    $.ajax({
        type: "GET",
        url: "/controller/master/dycryption?cipherText=" + cipherText,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
      
        async:false,
        success: function (response) {
            key = response;
            
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
            console.log("decrypt does not exist.");
            //window.location.href = "/Error/ErrorPage.html";
        }
    });
    if (key == null) {
        console.log("key does not exist.");
        //window.location.href = "/ErrorPage.html";
    }
    return key;
};






//Product

function productdetailcontroller(productseourl, loginuserid) {
    $.ajax({
        type: 'GET',
        url: '/Controller/Product/Product_Detail?productseourl=' + productseourl + '&loginuserid=' + loginuserid,
       
        success: function (result) {
            $('#productDetailContainer').html(result);
        },
        error: function () {
            alert('Error loading product view.');
        }
    });
}




///contact form
function contactformload(contactname, sellerid, subject, message, currenturl) {

    if (sellerid == $("#hdnlogin").val()) {
        toaster("Contact to your own account is not allowed!", "toast-success");
        return;
    }


    $.ajax({
        type: 'POST',
        url: '/Controller/Master/contactform?contactname=' + contactname + '&sellerid=' + sellerid + '&subject=' + subject + '&message=' + message + '&currenturl=' + currenturl,

        success: function (result) {
            $('#ContactFormContainer').html(result);
        },
        error: function () {
            alert('Error loading contact form.');
        }
    });
}

function postcontactform(referenceid, emailtype, emailbody, mobilenotification, redirecturl,email) {
    $.ajax({
        type: 'POST',
        url: '/Controller/Master/postcontactform?referenceid=' + referenceid + '&emailtype=' + emailtype + '&emailbody=' + emailbody + '&mobilenotification=' + mobilenotification + '&redirecturl="' + redirecturl + '&email="' + email,

        success: function (result) {
            $('#ContactFormContainer').html(result);
        },
        error: function () {
            alert('Error loading postform.');
        }
    });
}

//USers


function sellerbyguid(profileguid, loginuserid) {
    $.ajax({
        type: 'GET',
        url: '/Controller/User/UserByGUID?profileguid=' + profileguid + '&loginuserid=' + loginuserid,

        success: function (result) {
            $('#SellerViewContainer').html(result);
        },
        error: function () {
            alert('Error loading product view.');
        }
    });
}


function userimageview(ProfileGuid) {
    $.ajax({
        type: 'GET',
        url: '/Controller/Inbox/UserImageView?ProfileGuid=' + ProfileGuid,

        success: function (result) {
            $('#dvUserImageViewContainer').html(result);
        },
        error: function () {
            alert('Error loading userimageview.');
        }
    });
}

///Chat

function createchat(senderid, recieverid) {


    if (senderid == recieverid) {
        toaster("Chat to your own account is not allowed!", "toast-success");
        return;
    }


    $.ajax({
        type: "POST",
        url: "/Controller/Inbox/createchat?senderid="+ senderid + '&recieverid=' + recieverid,
       
        success: function (chatid) {
           
            if (chatid) {
                window.location.href = "/inbox/index?chatid=" + chatid;
            }
        },
        error: function () {
            alert("Error creating chat");
        }
    });
}






function numbertowords(price) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: 'GET',
            url: '/Controller/appsetting/WordsFromNumber?number=' + price,
          
            success: function (result) {
                resolve(result); // Resolve the promise with the result
            },
            error: function () {
                reject('Error loading numbertowords.'); // Reject the promise on error
            }
        });
    });
}

function lastseenupdate() {
   
    $.ajax({
        type: "POST",
        url: "/controller/user/LastSeenUpdate",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            // Handle the success response here
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
            console.log("lastseenupdate.");
            // Handle the error here, e.g., redirect to an error page
        }
    });
}

function validatelogin() {

    $.ajax({
        type: "POST",
        url: "/controller/login/ValidateLogin",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            if (response == "invalid") {
                // Handle the success response here
                window.location.href = "/logout?action=logout";
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
            console.log("lastseenupdate.");
            // Handle the error here, e.g., redirect to an error page
        }
    });
}



function emailgenerator(notificationrelayid, username) {
    $.ajax({
        type: 'POST',
        url: '/controller/master/EmailGenerator',
        data: {
            notificationrelayid: notificationrelayid,
            
        },
        success: function (data) {


            if (data.success == true) {

                $('#lblnotificationsend').removeClass("loading-text success-text fail-text");
                $('#lblnotificationsend').text("");

               
               
                $('#lblnotificationsend').addClass("success-text").text("Verification code sent on your email: " + username);

                /*console.log(data.notificationrelayid);*/
                /* $('#validatorVerificationCode').text(data.message);*/

                // Set validation result to false

            } else {

                $('#lblnotificationsend').removeClass("loading-text success-text fail-text");
                $('#lblnotificationsend').text("");



                $('#lblnotificationsend').addClass("fail-text").text("Email sending fail on email: " + username);

                // Set validation result to true

                /* $('#validatorVerificationCode').text(data.message);*/

            }
        },
        error: function () {
            // Handle error if any

            toaster("Something went wrong. Please try again later.", "toast-success");

        }
    });
}

//function updateProgressBarColor(divId, percentage) {
//    var progressBar = $("#" + divId);
//    progressBar.removeClass("red-bar orange-bar blue-bar green-bar");

//    if (percentage >= 0 && percentage <= 25) {
//        progressBar.addClass("red-bar");
//    } else if (percentage > 25 && percentage <= 50) {
//        progressBar.addClass("orange-bar");
//    } else if (percentage > 50 && percentage <= 75) {
//        progressBar.addClass("blue-bar");
//    } else {
//        progressBar.addClass("green-bar");
//    }
//}

function updateProgressBarColors() {
    $(".progress-bar").each(function () {
        var progressBar = $(this);
        var percentage = parseFloat(progressBar.attr("data-percentage")); // Assumes you store the percentage value in a data attribute
        progressBar.removeClass("red-bar orange-bar blue-bar green-bar");

        if (percentage >= 0 && percentage <= 25) {
            progressBar.addClass("red-bar");
        } else if (percentage > 25 && percentage <= 50) {
            progressBar.addClass("orange-bar");
        } else if (percentage > 50 && percentage <= 75) {
            progressBar.addClass("blue-bar");
        } else {
            progressBar.addClass("green-bar");
        }

        
    });
}


function barcodegenerator(inputdata)
{
    $.ajax({
        url: '/controller/master/BarCodeGenerator', // Update the URL accordingly
        method: 'POST',
        data: { inputData: inputdata }, // Pass any required data
        success: function (result) {
            // Update the content of the container with the partial view result
            $('#barcodeContainer').html(result);
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });
}

function qrcodegenerator(inputdata) {

   
    $.ajax({
        url: '/controller/master/QRCodeGenerator', // Update the URL accordingly
        method: 'POST',
        data: { inputData: inputdata }, // Pass any required data
        success: function (result) {
            // Update the content of the container with the partial view result
            $('#qrcodeContainer').html(result);
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });
}

///Button click loading
//function btnclickWait(btn) {

//    ///change text
//    alert("fff");
//    btn.innerHTML = 'Processing...';
//    // Disable the button
//    btn.disabled = true;

//    // Set a timeout to enable the button after 3 seconds
//    setTimeout(function () {
//        btn.disabled = false;
//        btn.innerHTML = 'Return...';
//    }, 3000);
//}

//function btnclickWait(btn) {

//    btn.addClass('btnprocessing');
//    btn.prop('disabled', true);

//    setTimeout(function () {
//        btn.removeClass('btnprocessing');
//        btn.prop('disabled', false);
//    }, 3000);
//}

///store local storage with 24 hours timer

// Function to set an item in localStorage with expiration time
function setItemWithExpiry(key, value, expiryInHours) {
    const now = new Date();
    const expiryTime = now.getTime() + (expiryInHours * 60 * 60 * 1000); // Convert hours to milliseconds
    const item = {
        value: value,
        expiry: expiryTime,
    };
    localStorage.setItem(key, JSON.stringify(item));
}

// Function to check if an item in localStorage is expired
function isItemExpired(key) {
    const item = JSON.parse(localStorage.getItem(key));
    if (!item || !item.expiry) {
        return true; // Item not found or missing expiry, treat as expired
    }
    const now = new Date();
    return now.getTime() > item.expiry;
}



function previewvideo(url) {
    var videoID = extractVideoID(url);
    if (videoID) {
        var embedUrl = 'https://www.youtube.com/embed/' + videoID;
        $('#videoContainer').empty().append('<iframe width="560" height="315" src="' + embedUrl + '" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>');
    } else {
        $('#videoContainer').empty();
    }
}

function extractVideoID(url) {
    var videoID = null;
    var regex = /(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})/;
    var match = url.match(regex);
    if (match && match[1]) {
        videoID = match[1];
    }
    return videoID;
}
