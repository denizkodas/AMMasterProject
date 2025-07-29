

//on click show load partial view of login form any where application
$(document).on('click', '#btnShowLoginForm', function (e) {
    e.preventDefault();


    loginview("LoginView");


});

//on click show load partial view of register form any where application
$(document).on('click', '#btnShowRegisterForm', function (e) {
    e.preventDefault();
    //$('#loginFormContainer').empty();

    loginview("RegisterView", "Client");


    setTimeout(function () {
        $("#hdnUserType").val("Client");



    }, 1000);


});


$(document).on('click', '#btnShowRegisterFormVendor', function (e) {
    e.preventDefault();

    loginview("RegisterView", "Vendor");


    setTimeout(function () {
        $("#hdnUserType").val("Vendor");

    }, 1000);




});




$(document).on('click', '#btnLogout', function (e) {
    e.preventDefault();
    //$('#loginFormContainer').empty();


    $.ajax({
        type: "POST",
        url: "/controller/login/logout/",
        async: false, // Make the request synchronous
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            // Handle the response
            if (response === "Success") {
                window.location.href = "/logout?action=logout";
            } else {
                console.log(response);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
        }
    });
});



function devicelogout(browser, browserversion, operatingsystem, devicetype, ip, event) {
    $.ajax({
        type: "POST",
        url: "/controller/login/logoutDevice/",
        data: {
            browser: browser,
            browserversion: browserversion,
            operatingsystem: operatingsystem,
            devicetype: devicetype,
            ip:ip
        },
        success: function (response) {
            // Handle the response
            if (response === "Success") {
                $(event.target).closest('.border').remove();
                toaster("Device logout successfully", "toast-success");
            } else {
                console.log(response);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
        }
    });
}


function passwordPolicy(password) {
    // Validate password length
    if (password.length < 8) {
        return 'Password must be at least 8 characters long';
    }

    // Validate password complexity
    if (!/\d/.test(password) || !/[a-zA-Z]/.test(password)) {
        return 'Password must contain at least one letter and one digit';
    }

    // Validate special character
    if (!/[!#$%^&*@]/.test(password)) {
        return 'Password must contain at least one special character (!#$%^&*@)';
    }

    // Password is valid
    return '';
}
function loginview(controllername) {
    $.ajax({
        url: "/Controller/Login/" + controllername,
        type: "GET",
        success: function (partialView) {
            $("#loginFormContainer").html(partialView);
        },
        error: function () {
            console.log("Error occurred.");
        }
    });
}

///obselete 2 jan 2024 shifted logic to return controller
//function loginredirection(usertype, profileguid) {

//    var trimmedProfileGuid = profileguid.trim();
//    var redirecturl = getParameterByName("redirecturl");

   

//    if (redirecturl != null && redirecturl !== '') {
//        console.log("redirect url");
//        var trimmedProfileGuid = profileguid.trim(); // Trim leading and trailing spaces

//        window.location.href = "Index?USERGUID=" + trimmedProfileGuid + "&redirecturl=" + redirecturl;
//        return; // Stop further execution
//    }

//    if (usertype == "Vendor") {
//        console.log("vendor");
//        window.location.href = "/seller/Index?USERGUID=" + trimmedProfileGuid;
//        return; // Stop further execution
//    }
//    else if (usertype == "Admin") {
//        console.log("admin");
//        window.location.href = "/admin/";
//        return; // Stop further execution
//    }
//    else if (usertype == "Client") {
//        console.log("client");
//        window.location.href = "Index?USERGUID=" + trimmedProfileGuid;
//        return; // Stop further execution
//    }
//    else if (document.title == "Login" || document.title == "Register" || document.title == "Sign In") {
//        console.log("login");
//        window.location.href = "Index?USERGUID=" + trimmedProfileGuid;
//        return;
//    }
        

//    else {
//        //location.reload() +  "?USERGUID=" + getProfileGUID(username);
//        const userGUID = trimmedProfileGuid; // Get the GUID of the user
//        location.reload(true); // Reloads the current page from the server


//        const urlParams = new URLSearchParams(window.location.search); // Get the query parameters of the current URL
//        urlParams.set("USERGUID", userGUID); // Set the "USERGUID" parameter to the user's GUID
//        const newUrl = window.location.pathname + "?" + urlParams.toString(); // Create a new URL with the updated query parameters
//        window.location.href = newUrl; // Redirect to the new URL
//        return; // Stop further execution
//    }


   
//}



//depreciated on 25 nov 2023 break availability check check as soon as the user type the username
//function UserNameAvailablecheck(username, loginchannel) {

//    return new Promise(function (resolve, reject) {
//        $.ajax({
//            type: 'POST',
//            url: '/controller/Login/UserNameAvailableCheck',
//            data: {
//                UserName: username,
//                loginchannel: loginchannel
//            },
//            success: function (data) {
//                if (data.message !== "Available") {
//                    // Set validation result to false
//                    resolve(false);
//                } else {
//                    // Set validation result to true
//                    resolve(true);
//                }
//            },
//            error: function () {
//                // Handle error if any
//                reject(new Error("Something went wrong. Please try again later."));
//            }
//        });
//    });
//}

function UserNameAvailablecheck(username, loginchannel) {
    // Clear previous classes and messages
    

    $.ajax({
        type: 'POST',
        url: '/controller/Login/UserNameAvailableCheck',
        data: {
            UserName: username,
            loginchannel: loginchannel
        },
        success: function (data) {
            if (data.success == true) {
                // Assign class and message for username available
                $('#validateUserNameAvailableCheck').addClass("usernameavailable").text(data.message);
                $("#btnverification").addClass("dimmed-button").prop("disabled", false);
            } else {
                // Assign class and message for username not available
                $('#validateUserNameAvailableCheck').addClass("usernamenotavailable").text(data.message);
                $('#spanError').text(data.message);
                $("#btnverification").addClass("dimmed-button").prop("disabled", true);
            }
        },
        error: function (err) {
            // Handle error if any
            console.log(err);
        }
    });
}


function VerificationCodeGenerator(username, codetype) {

  /*  $("#lblUserName").text("Processing verification code on your email: " + username);*/
   
    $('#lblnotificationsend').removeClass("loading-text success-text fail-text");
    $('#lblnotificationsend').text("");
  

    $('#lblnotificationsend').addClass("loading-text").text("Sending verification code on your email: " + username);
    $.ajax({
        type: 'POST',
        url: '/controller/Login/GenerateVerificationCode',
        data: {
            UserName: username,
            codetype: codetype
        },
        success: function (data) {


            if (data.success == true) {
              
                emailgenerator(data.notificationid, username);
              
            } else {

                $('#lblnotificationsend').removeClass("loading-text success-text fail-text");
                $('#lblnotificationsend').addClass("fail-text").text(data.message);

            }
        },
        error: function () {
            // Handle error if any

            toaster("Something went wrong. Please try again later.", "toast-success");

        }
    });

};
function VerificationCodeValidation(verificationcode, username, type, loginchannel, firstname, lastname, password) {
    $('#validatorVerificationCode').text("");

        $.ajax({
            type: 'POST',
            url: '/controller/Login/RetreiveVerificationCode',
            data: {
                verificationcode: verificationcode,
            },
            success: function (data) {


                if (data.success == true) {


                    

                    $('#validatorVerificationCode').text(data.message);
                    RegisterAccount(username, type, loginchannel, firstname, lastname, password);
                    // Set validation result to false
                  
                } else {
                    // Set validation result to true

                    $('#validatorVerificationCode').text(data.message);
                    
                }
            },
            error: function () {
                // Handle error if any

                toaster("Something went wrong. Please try again later.", "toast-success");

            }
        });
    
};

function PasswordVerificationCodeValidation(verificationcode, password ,username) {
    $('#validatorVerificationCode').text("");

    $.ajax({
        type: 'POST',
        url: '/controller/Login/RetreiveVerificationCode',
        data: {
            verificationcode: verificationcode,
        },
        success: function (data) {


            if (data.success == true) {


                PasswordUpdate(username, password);

                $('#validatorVerificationCode').text(data.message);
              
                // Set validation result to false

            } else {
                // Set validation result to true

                $('#validatorVerificationCode').text(data.message);

            }
        },
        error: function () {
            // Handle error if any

            toaster("Something went wrong. Please try again later.", "toast-success");

        }
    });

};

function RegisterAccount(UserName, Type, loginchannel, FirstName, LastName, Password) {
    $.ajax({
        type: 'POST',
        url: '/controller/Login/RegisterCreateAccount',
        data: {
            UserName: UserName,
            Type: Type,
            loginchannel: loginchannel,
            FirstName: FirstName,
            LastName: LastName,
            Password: Password
        },
        success: function (response) {
            console.log(response);
            window.location.href = response;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Log detailed error information
            console.error("Error details:");
            console.error("jqXHR:", jqXHR);
            console.error("TextStatus:", textStatus);
            console.error("ErrorThrown:", errorThrown);

            // Display an error message to the user
            toaster("Something went wrong. Please try again later.", "toast-error");
        }
    });
}


function login(username, password) {

    var message = null;
    $.ajax({
        type: "POST",
        url: "/controller/Login/login/",

        data: {
            username: username,
            password: password,

        },



        success: function (response) {


            if (response && response.startsWith("success")) {
                window.location.href = response.replace("success", "");;
                return;

            } else {


                // Message is returned, display it to the user
                $('#lblloginerror').text(response);
            }



        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
            //alert(thrownError);

            //window.location.href = "/Error/ErrorPage.html";
        }
    });
};


function accountaccess(userguid) {


    $.ajax({
        type: "POST",
        url: "/controller/Login/AccountAccess/",  // Replace with the actual URL

        data: {
            userguid: userguid
        },

        success: function (response) {


            if (response && response.startsWith("success")) {
                /*window.location.href = response.replace("success", "");*/

                var newWindow = window.open(response.replace("success", ""), "_blank");
                if (newWindow) {
                    newWindow.focus(); // Bring the new window to the foreground if it was blocked by a pop-up blocker.
                }
                return;
            } else {
                // The following line had a syntax error
                // toaster(response), "toast-success");
                // Corrected version:
                toaster(response, "toast-success");
                // Message is returned, display it to the user
                /*$('#lblloginerror').text(response);*/
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error: " + thrownError);
            //alert(thrownError);

            //window.location.href = "/Error/ErrorPage.html";
        }
    });
}



//document.addEventListener("DOMContentLoaded", function () {
//    loadRegisterPopup();
//});
//function loadRegisterPopup() {
//    // Get the hash value from the URL
//    var hash = window.location.hash;

//    // Check if the hash value matches the desired value, e.g., "#register"
//    if (hash === "#register") {
//        // Show the popup
//        loginview("RegisterView");
//    }
//}

function ForgetPasswordUserNameAvailableCheck(username) {
   


        $.ajax({
            type: 'POST',
            url: '/controller/Login/ForgetPasswordUserNameAvailableCheck',
            data: {
                UserName: username,
            },
            success: function (data) {

                if (data.success == true) {
                    $('#validateUserNameAvailableCheck').addClass("usernameavailable");
                    $('#validateUserNameAvailableCheck').text(data.message);

                    $('#loginchannel').text(data.loginchannel);

                    
                    if (data.loginchannel == 'Email') {

                        $('#lblloginchannel').text('Email');

                        
                        VerificationCodeGenerator(data.username, 'PasswordVerificationCode');

                        $('#dvUserNameValidation').hide();
                        $('#dvVerificationCode').show();
                    }
                    else if (data.loginchannel == 'Phone') {

                       
                        $('#lblloginchannel').text('Phone');

                        $('#lblnotificationsend').removeClass("loading-text success-text fail-text");
                      
                        $('#lblnotificationsend').text("");

                        $('#lblnotificationsend').addClass("loading-text ").text("Sending verification code on your phone: " + data.username);
                        /*username = "+" + $("#hdnmobilecode").val() + username;*/

                        // console.log(username);
                      
                      
                        phoneAuth(data.username);
                    }
                }
                else {
                    $('#validateUserNameAvailableCheck').addClass("usernamenotavailable");
                    $('#validateUserNameAvailableCheck').text(data.message);

                    $('#loginchannel').text(data.loginchannel);
                }
               
            },
            error: function (err) {
                $('#validateUserNameAvailableCheck').addClass("usernamenotavailable");
                $('#validateUserNameAvailableCheck').text(err);
            }
        });
   
}

function PasswordUpdate(UserName, Password) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: 'POST',
            url: '/controller/Login/UpdatePassword',
            data: {
                UserName: UserName,
                Password: Password

            },
            success: function (response) {




                window.location.href = response;
                return;

                //if (message && message.startsWith("success")) {
                //    const parts = message.split("#");
                //    const userGuid = parts[1].replace("USERGUID=", "");
                //    const userType = parts[2].replace("Type=", "").trim();
                //    loginredirection(userType, userGuid);

                //} else {
                //    // Message is returned, display it to the user
                //    $('#lblregistererror').text(data.message);
                //}


            },
            error: function () {
                // Handle error if any

                toaster("Something went wrong. Please try again later.", "toast-success");

            }
        });
    });
}




/////////////////FAcebook KIT
// Load the Facebook SDK asynchronously


// facebook-init.js

// Handle the Facebook Login status
function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}
// Callback to handle the Facebook Login status
function statusChangeCallback(response) {
    if (response.status === 'connected') {
        // User is logged in and has authorized your app
        var accessToken = response.authResponse.accessToken;
        var userID = response.authResponse.userID;

        // Request user data from Facebook
        FB.api('/me?fields=name,email,picture.type(large)', function (userData) {
            if (userData && !userData.error) {
                var fullname = userData.name;
                var userEmail = userData.email;
                var id = userData.id
                var profilePictureUrl = userData.picture.data.url;



                var fullNameArray = fullname.split(' '); // Split the full name into an array of words

                var firstName = fullNameArray[0]; // The first word is typically the first name
                var lastName = fullNameArray.slice(1).join(' '); // The rest of the words (if any) make up the last name

                console.log('First Name: ' + firstName);
                console.log('Last Name: ' + lastName);

                console.log('Logged in as ' + fullname + ' with access token: ' + accessToken);
                console.log('Email: ' + userEmail);
                console.log('Profile Picture URL: ' + profilePictureUrl);
                console.log('ID ' + id);



                $.ajax({
                    type: "POST",
                    url: "/controller/Login/FacebookLoginJs/",

                    data: {
                        givenname: firstName,
                        surname: lastName,
                        UserName: userEmail,
                        userId: id,

                    },



                    success: function (response) {

                        console.log("Response: " + response);
                        if (response != null) {
                            window.location.href = response;


                        } else {
                            toaster(response, "toast-success");

                            // Message is returned, display it to the user
                            // $('#lblloginerror').text(response);
                        }



                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log("Error: " + thrownError);
                        toaster(thrownError, "toast-success");
                        //alert(thrownError);

                        //window.location.href = "/Error/ErrorPage.html";
                    }
                });



            }
        });
    } else if (response.status === 'not_authorized') {
        // User is logged into Facebook but hasn't authorized your app
        console.log('Not authorized');
    } else {
        // User is not logged into Facebook
        console.log('Not logged into Facebook');
    }
}


function facebookkitinit(appid) {
    window.fbAsyncInit = function () {
        FB.init({
            appId: appid,  // Replace with your Facebook App ID '1462619270551703'
            cookie: true,
            xfbml: true,
            version: 'v17.0'      // Update to the desired Facebook API version
        });

        FB.AppEvents.logPageView();
    };

    // Load the Facebook SDK asynchronously
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s);
        js.id = id;
        js.src = "https://connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
}

////////////////FAcebook login kit


function avatarlist() {
    $.ajax({
        url: "/Controller/User/GetAvatars",
        type: "GET",
        success: function (partialView) {
            $("#dvAvatarlistcontainer").html(partialView);
        },
        error: function () {
            console.log("Error occurred.");
        }
    });
}
    