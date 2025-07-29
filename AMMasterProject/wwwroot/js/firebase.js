// For Firebase JS SDK v7.20.0 and later, measurementId is optional


let firebaseConfig = {}; // Define an empty object for Firebase configuration


//firebaseConfig = {
//    apiKey: "AIzaSyAqwvHIaBZ5iG8YO8HTL_hyvc_NcQEuHPI",
//    authDomain: "ammasterproject-e4125.firebaseapp.com",
//    projectId: "ammasterproject-e4125",
//    storageBucket: "ammasterproject-e4125.appspot.com",
//    messagingSenderId: "901837285175",
//    appId: "1:901837285175:web:0ad4a485e6d251a2649cb2",
//    measurementId: "G-5S6L6T161N"
//};
//firebaseConfig = {
//    apiKey: "AIzaSyDVmiXO180oyhyT3Tr8-eCzFwvnlz2Xsg4",
//    authDomain: "ammasterproject-1f531.firebaseapp.com",
//    projectId: "ammasterproject-1f531",
//    storageBucket: "ammasterproject-1f531.appspot.com",
//    messagingSenderId: "511958923165",
//    appId: "1:511958923165:web:d3dbfa7919f8e409d7a174",
//    measurementId: "G-LQPMFERF12"
//};




// Call the function to fetch and set Firebase configuration


function firebasecredential() {
    $.ajax({
        url: '/controller/AppSetting/firebasesettings', // Replace with the actual URL of your controller
        type: 'GET',
        dataType: 'json',
        success: function (data) {
             firebaseConfig = {
                apiKey: data.apiKey,
                authDomain: data.authDomain,
                projectId: data.projectId,
                storageBucket: data.storageBucket,
                messagingSenderId: data.messagingSenderId,
                appId: data.appId,
                measurementId: data.measurementId
               


                
            };

          /*  console.log(firebaseConfig);*/
            firebase.initializeApp(firebaseConfig);
            render();

            // Initialize Firebase with the updated configuration
            // Delay the call to initializeFirebase by 2 seconds
           
            // Use the firebaseConfig object as needed
           /* console.log(firebaseConfig);*/
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error(error);
        }
    });
}



  



/*firebase.initializeApp(firebaseConfig);*/

function render() {

   
   
    ///to make captcha visible uncomment this
    //window.recaptchaVerifier = new firebase.auth.RecaptchaVerifier('recaptcha-container');
    //recaptchaVerifier.render();


    //to make captach in visible uncomment this
    window.recaptchaVerifier = new firebase.auth.RecaptchaVerifier('recaptcha-container', {
        'size': 'invisible'
    });
    recaptchaVerifier.render();

   
}
// function for send message
var confirmationResult; // Declare confirmationResult in the global scope




function phoneAuth(number) {

    
   /* console.log("phoneauth"+ number);*/
        //firebase.auth().settings.appVerificationDisabledForTesting = true;
       

        //var appVerifier = new firebase.auth.RecaptchaVerifier('recaptcha-container');

        firebase.auth().signInWithPhoneNumber(number, window.recaptchaVerifier)
            .then(function (result) {
                confirmationResult = result; // Store the confirmationResult in the global variable


               
                //$('#dvRegistersecondstep').hide();
                //$('#dvRegisterfirststep').hide();
                //$('#dvthirdstep').show();


                //$('#dvUserNameValidation').hide();
                //$('#dvVerificationCode').show();
                $('#dvRegistersecondstep').hide();
                $('#dvRegisterfirststep').hide();
                $('#dvthirdstep').show();

                $('#lblnotificationsend').removeClass("loading-text success-text fail-text");
                $('#lblnotificationsend').text("");


                $('#lblnotificationsend').addClass("success-text").text("Verification code sent on your phone: " + number);
            })
            .catch(function (error) {
                ///this is on first screent
                $('#validatorPhone').text(error.message.replace("Firebase:", ""));

               

                //$('#lblnotificationsend').removeClass("loading-text success-text fail-text");
                //$('#lblnotificationsend').text("");


                //$('#lblnotificationsend').addClass("fail-text").text("Fail to send verification code on phone: " + number);

               /* console.log("Error: " + error.message.replace("Firebase:", ""));*/ // Reject the promise with an error message
            });
   
}

function codeverify(code, username, type, loginchannel, firstname, lastname, password) {
    if (confirmationResult) {
        confirmationResult.confirm(code)
            .then(function (result) {
                if (result && result.user) {

                    RegisterAccount(username, type, loginchannel, firstname, lastname, password);


                   
                   /* console.log("Code verified:", result);*/
                } else {
                    
                    $('#validatorVerificationCode').text("Code verification failed");
                   /* console.error("Code verification failed");*/
                }
            })
            .catch(function (error) {
                $('#validatorVerificationCode').text(error.message.replace("Firebase:",""));
                /*console.error("Error during code verification:", error.message);*/
            });
    } else {

        $('#validatorVerificationCode').text(("Confirmation result is undefined"));
        /*console.error("Confirmation result is undefined");*/
    }
}


function codeverifypasswordreset(code, username, password) {
    if (confirmationResult) {
        confirmationResult.confirm(code)
            .then(function (result) {
                if (result && result.user) {

                    PasswordUpdate(username, password);

                    //$('#lblnotificationsend').removeClass("loading-text success-text fail-text");
                    //$('#lblnotificationsend').addClass("success-text").text("Verification in process");
                   
                    /* console.log("Code verified:", result);*/
                } else {
                  /*  $('#lblnotificationsend').removeClass("loading-text success-text fail-text");*/
                    $('#validatorVerificationCode').text("Code verification failed");
                    /* console.error("Code verification failed");*/
                }
            })
            .catch(function (error) {
                $('#validatorVerificationCode').text(error.message.replace("Firebase:", ""));
                /*console.error("Error during code verification:", error.message);*/
            });
    } else {

        $('#validatorVerificationCode').text(("Confirmation result is undefined"));
        /*console.error("Confirmation result is undefined");*/
    }
}