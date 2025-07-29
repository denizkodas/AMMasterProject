$(document).ready(function () {
    loadRTL();
});

function loadRTL() {
    // Corrected the variable name from 'isr' to 'isRTL'
    var isRTL = localStorage.getItem('isRTL') === 'true'; // Ensure comparison as boolean

    // jQuery is used to modify the class and 'dir' attribute of the 'body' element
    if (isRTL) {
        $("#mybody").addClass("rtl").attr("dir", "rtl");
    } else {
        $("#mybody").removeClass("rtl").removeAttr("dir");
    }
}

function loadcountry() {
    var countryList = $('#country-list-ul');

    $.ajax({
        url: '/Controller/AppSetting/countrylist',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Handle the returned data
            countryList.empty();

            $.each(data, function (index, country) {
                var listItem = $('<li></li>');
                var cookieValue = getCookie('countrycode');
                var classValue = cookieValue === country.countryAlpha2.trim() ? 'active' : '';
                var countryLink = $('<a></a>').text(country.name.trim()).addClass(classValue);



                
                // Set flag image source
                var flagImage = $('<img>').attr('src', '/countryflags/' + country.countryAlpha2 + '.png');

                // Append flag image and country link to list item
                listItem.append(flagImage);
                listItem.append(countryLink);

                // Append list item to country list
                countryList.append(listItem);

                // Handle click event to store countryAlpha2 code in cookie
                countryLink.on('click', function () {
                    // Set cookie with the countryAlpha2 code
                    

                    setCookie("countrycode", country.countryAlpha2, 300);

                    ///set value to label and flag
                    $('#spancountrycode').text(country.countryAlpha2);
                    $('#countryflag').attr('src', '/countryflags/' + country.countryAlpha2 + '.png');

                    //hide popu;
                    $('#country-div').hide();
                });
            });

            
            // Check if there are no records and hide the element
            //if (data.length === 0) {
            //    $("#ashowcountry").hide();
            //} else {
            //    $("#ashowcountry").show();
            //}
        },
        error: function (xhr, status, error) {
            // Handle any errors
            console.log(error);
        }
    });
}



function loadcurrency() {
    var currencyList = $('#currency-list-ul');

    $.ajax({
        url: '/Controller/AppSetting/currencylist',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Handle the returned data
            currencyList.empty();

            $.each(data, function (index, currency) {
                var listItem = $('<li></li>');
                var cookieValue = getCookie('currencycode');
                var classValue = cookieValue === currency.currencyCode.trim() ? 'active' : '';
                var currencyLink = $('<a></a>').addClass(classValue);
                currencyLink.append($('<span class="currencyname"></span>').text(currency.currencyName.trim()));
                currencyLink.append($('<span class="currencycode"></span>').text(currency.currencyCode.trim() + " - " + currency.currencySymbol.trim()));

                listItem.append(currencyLink);

                // Append list item to currency list
                currencyList.append(listItem);

                // Handle click event to store currency code in cookie and update label
                currencyLink.on('click', function () {
                    var currencyCode = currency.currencyCode.trim();

                    // Set cookie with the currency code
                    setCookie('currencycode', currencyCode, 300);

                    // Set value to label
                    $('#spancurrencycode').text(currencyCode);

                  

                    // Clear the hash value
                    location.replace(location.origin + location.pathname);


                    // Hide popup
                    $('#currency-div').hide();

                });
            });
        },
        error: function (xhr, status, error) {
            // Handle any errors
            console.log(error);
        }
    });
}


function loadBaseCurrency() {
    $.ajax({
        url: '/Controller/AppSetting/currencybaseselection',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Handle the returned LanguageViewModel object
           

            var currencyCode = data.currencyCode.trim();

            $('#spancurrencycode').text(currencyCode);

            // Set cookie with the currency code
            setCookie('currencycode', currencyCode, 300);

            

            //location.replace(location.origin + location.pathname);

        },
        error: function (xhr, status, error) {
            // Handle any errors
            console.log(error);
        }
    });
}

function loadlanguage() {
    var languageList = $('#language-list-ul');

    $.ajax({
        url: '/Controller/AppSetting/languagelist',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Handle the returned data
            languageList.empty();

            $.each(data, function (index, language) {

            
                var listItem = $('<li></li>');
                var cookieValue = localStorage.getItem('languagename');
              
                var classValue = cookieValue === language.name.trim() ? 'active' : '';
                var languageLink = $('<a></a>').text(language.name.trim()).addClass(classValue);


                listItem.append(languageLink);

                // Append list item to currency list
                languageList.append(listItem);

                // Handle click event to store currency code in cookie and update label
                languageLink.on('click', function () {
                    var languageSelectedName = $(this).text().trim();

                    ///remove cookie
                    /* removeAllCookiesList('googtrans');*/
                   /* removeCookie('languagecode');*/
                   
                    $(this).text("processing....")

                    removeAllCookiesByName('googtrans');
                    setCookie('googtrans', '/auto/' + language.code, 1);
                    
                   
                 

                    localStorage.removeItem('languagename');
                    localStorage.setItem('languagename', languageSelectedName);

                    localStorage.removeItem('isRTL');
                    localStorage.setItem('isRTL', language.isRTL);

                    localStorage.removeItem('languageshortcode');
                    localStorage.setItem('languageshortcode', language.code);

                   
                    ///master language.js file
                    localStorage.removeItem('ammasterprojecttranslation');
                    reloadfile();


                    //var selectedLanguage = getCookie('googtrans');
                   /* setLanguage(language.code);*/


                   /* setCookie('languagecode', languageCode, 300);*/

                   
                    // Set value to label
                    $('#spanlanguagecode').text(languageSelectedName);  ///its like page is loading
                    // Clear the hash value

                    setTimeout(function () {

                     
                        location.replace(location.origin + location.pathname);
                    }, 2000); 


                    // Hide popup
                    //

                });
            });
        },
        error: function (xhr, status, error) {
            // Handle any errors
            console.log(error);
        }
    });
} 


function loadBaseLanguage() {
    $.ajax({
        url: '/Controller/AppSetting/languagebaseselection',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Handle the returned LanguageViewModel object
            $('#spanlanguagecode').text(data.name);

         
            // Set cookie with the currency code
            /*setCookie('langaugeshortcode',  data.code, 300);*/


            removeAllCookiesByName('googtrans');
           




            localStorage.removeItem('languagename');
            localStorage.setItem('languagename', data.name);

            localStorage.removeItem('isRTL');
            localStorage.setItem('isRTL', data.isRTL);

            localStorage.removeItem('languageshortcode');
            localStorage.setItem('languageshortcode', data.code);

          
            setCookie('googtrans', '/auto/' + data.code, 1);

            location.replace(location.origin + location.pathname);
            /*loadRTL();*/

            //localStorage.setItem('languagename', data.name);

           
            //localStorage.setItem('languageshortcode', data.code);
         
            //setCookie('googtrans', '/auto/' + data.code, 1);
         
          
        },
        error: function (xhr, status, error) {
            // Handle any errors
            console.log(error);
        }
    });
}


function usercountry2digit() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: '/Controller/appsetting/CountryCode2Digit',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                resolve(data); // Resolve the promise with the response data
            },
            error: function (xhr, status, error) {
                reject(error); // Reject the promise with the error
            }
        });
    });
}




//$(document).ready(function () {
//    getcurrentcountry();
//});


    //$(document).ready(function () {
    //    const ip = '39.39.125.31';
    //    const accessKey = 'b3fea954-8e07-49f0-a05a-b12c83336f8b';

    //    $.ajax({
    //        url: 'http://apiip.net/api/check?ip=' + ip + '&accessKey=' + accessKey,
    //        success: function (result) {
    //            console.log(result);
    //        }
    //    });
    //});


function getcurrentcountry() {
    return new Promise(function (resolve, reject) {
        var cookieValue = getCookie('countrycode');

        if (cookieValue !== null) {
            // If the country code is in the cookie, resolve immediately
            resolve({ country: cookieValue, countryCode: getCountryCodeFromName(cookieValue) });
        } else if (navigator.geolocation) {
            // Get the current latitude and longitude
            navigator.geolocation.getCurrentPosition(
                function (position) {
                    success(position)
                        .then(function (result) {
                            resolve(result);
                        })
                        .catch(function (error) {
                            reject(error);
                        });
                },
                function (error) {
                    reject('Geolocation error: ' + error.message);
                }
            );
        } else {
            reject('Geolocation is not supported by this browser.');
        }
    });
}

function success(position) {
    return new Promise(function (resolve, reject) {
        var latitude = position.coords.latitude;
        var longitude = position.coords.longitude;

        // Reverse geocoding using Google Geocoding API
        $.ajax({
            url: 'https://maps.googleapis.com/maps/api/geocode/json',
            type: 'GET',
            dataType: 'json',
            data: {
                latlng: latitude + ',' + longitude,
                key: 'AIzaSyD_Ia-RHd5bIWMLLL05tUJlDy_7DHb2--4'
            },
            success: function (data) {
                var countryComponent = data.results.find(function (result) {
                    return result.types.includes('country');
                });

                if (countryComponent) {
                    var country = countryComponent.formatted_address;
                    var countryCode = getCountryCode(countryComponent);

                    resolve({ country: country, countryCode: countryCode });
                } else {
                    reject('Country information not found.');
                }
            },
            error: function (error) {
                reject('Error: ' + error.statusText);
            }
        });
    });
}

function error() {
    console.log('Unable to retrieve your location.');
}

function getCountryCode(countryComponent) {
    var countryCode = null;
    var addressComponents = countryComponent.address_components;

    // Find the country code in the address components
    for (var i = 0; i < addressComponents.length; i++) {
        var component = addressComponents[i];
        var types = component.types;

        if (types.includes('country')) {
            countryCode = component.short_name;
            break;
        }
    }

    return countryCode;
}


function loadcountryList() {
    var countryList = $('#mobilecode-list-ul');
    var filterInput = $('#filterInput');

    $.ajax({
        url: '/Controller/AppSetting/countrylist',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Handle the returned data
            countryList.empty();

            //get user country code from ip
            var mobilecode = "0";
           
            var storedCountryAlpha2 = getCookie('countrycode');
            /// do not use this, instead use gooogle country code
            //usercountry2digit()
            //    .then(function (data) {
            //        mobilecode = data.phoneCode;
            //        $("#hdnmobilecode").val(mobilecode);
            //        $("#spanautophoneCode").text(mobilecode);
            //    })
            //    .catch(function (error) {

            //        console.log(error);
            //    });

            //getcurrentcountry()
            //    .then(function (result) {
            //        //console.log('Country:', result.country);
            //        //console.log('Country Code:', result.countryCode);

            //         countryCode = result.countryCode;

                   
                  


            //    })
            //    .catch(function (error) {
            //        console.error(error);
            //    });

            // Function to generate a list item
            function createListItem(country) {
                var listItem = $('<li></li>');

                
                if (country.countryAlpha2 == storedCountryAlpha2) {
                    mobilecode = country.countryMobileCode;
                    $("#hdnmobilecode").val(mobilecode);
                   $("#spanautophoneCode").text(mobilecode);
                }

                var classValue = country.countryMobileCode === mobilecode ? 'active' : '';
                var countryLink = $('<a></a>').text(country.countryMobileCode.trim() + ' ' + country.name.trim()).addClass(classValue);
                var flagImage = $('<img>').attr('src', '/countryflags/' + country.countryAlpha2 + '.png');

                listItem.append(flagImage);
                listItem.append(countryLink);

                countryLink.on('click', function () {
                    $("#spanautophoneCode").text(country.countryMobileCode);
                    $("#hdnmobilecode").val(country.countryMobileCode);
                });

                return listItem;
            }

            // Initial population of the country list
            $.each(data, function (index, country) {
                countryList.append(createListItem(country));
            });

            // Filter the country list based on user input
            filterInput.on('input', function () {
                var filterText = filterInput.val().trim().toLowerCase();
                countryList.empty();

                $.each(data, function (index, country) {
                    if (country.name.toLowerCase().includes(filterText)) {
                        countryList.append(createListItem(country));
                    }
                });
            });
        },
        error: function (xhr, status, error) {
            // Handle any errors
            console.log(error);
        }
    });
}











