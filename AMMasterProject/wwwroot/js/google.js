
document.addEventListener("DOMContentLoaded", function () {
    getGoogleMapApiKey(function (apiKey) {
        loadGoogleMapsScript(apiKey, initializeGoogleAutocomplete);
        bindCurrentLocationToAddressField();
    });
});

function getGoogleMapApiKey(callback) {
    $.ajax({
        url: '/controller/AppSetting/getgooglemapi', // Adjust the URL based on your project structure
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Invoke the callback with the API key received from the server
            callback(data);
        },
        error: function (error) {
            console.error('Error fetching Google Maps API key:', error);
        }
    });
}
//function LoadGoogleAutoComplete() {
   
//    var places = new google.maps.places.Autocomplete(document.querySelector('.googleaddress'));
//    google.maps.event.addListener(places, 'place_changed', function () {
//        var place = places.getPlace();
//        var address = place.formatted_address;
//        var latitude = place.geometry.location.lat();
//        var longitude = place.geometry.location.lng();
//        var mesg = "Address: " + address;
//        mesg += "\nLatitude: " + latitude;
//        mesg += "\nLongitude: " + longitude;

//        getAddressDetails(address);
//        /*alert(mesg);*/
//    });


//    bindCurrentLocationToAddressField();
//}

function loadGoogleMapsScript(apiKey, callback) {
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = `https://maps.googleapis.com/maps/api/js?libraries=places&key=${apiKey}`;
    script.async = true;
    script.defer = true;
    script.onload = callback;
    document.head.appendChild(script);
}

function initializeGoogleAutocomplete() {
    var input = document.querySelector('.googleaddress');
    if (input) {
        var places = new google.maps.places.Autocomplete(input);
        google.maps.event.addListener(places, 'place_changed', function () {
            var place = places.getPlace();
            if (place.geometry) {
                var address = place.formatted_address;
                var latitude = place.geometry.location.lat();
                var longitude = place.geometry.location.lng();
                var mesg = "Address: " + address;
                mesg += "\nLatitude: " + latitude;
                mesg += "\nLongitude: " + longitude;

                getAddressDetails(address);
                /*alert(mesg);*/
            }
        });
    } else {
        console.error("Autocomplete input element not found.");
    }
}

function getAddressDetails(address) {

  
    //var address = ;
    //alert(address);
    // Use the Geocoding API to get the details for the given address
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ address: address }, function (results, status) {
        if (status === "OK" && results.length > 0) {
            var addressComponents = results[0].address_components;
            var country, state, city, zipcode,street;

            // Extract the components from the result
            //for (var i = 0; i < addressComponents.length; i++) {
            //    var component = addressComponents[i];
            //    if (component.types.indexOf('country') !== -1) {
            //        country = component.long_name;
            //    } else if (component.types.indexOf('administrative_area_level_1') !== -1) {
            //        state = component.long_name;
            //    } else if (component.types.indexOf('locality') !== -1) {
            //        city = component.long_name;
            //    } else if (component.types.indexOf('postal_code') !== -1) {
            //        zipcode = component.long_name;
            //    }
            //    else if (component.types.indexOf('route') !== -1) {
            //        street = component.long_name;


            //    }
            //}
            for (var i = 0; i < addressComponents.length; i++) {
                var component = addressComponents[i];
                if (component.types.includes('country')) {
                    country = component.long_name;
                } else if (component.types.includes('administrative_area_level_1')) {
                    state = component.long_name;
                } else if (component.types.includes('locality')) {
                    city = component.long_name;
                } else if (component.types.includes('postal_code')) {
                    zipcode = component.long_name;
                } else if (component.types.includes('route')) {
                    street = component.long_name;
                }
            }
           
            $('#mapcountry').val(country);
            $('#mapstate').val(state)
            $('#mapcity').val(city)
            $('#mapzipcode').val(zipcode)
            // Set the street information wherever you need it
            $('#mapstreet').val(street);

            // Print the extracted details
            //$('#result').html('Country: ' + country + '<br>State: ' + state + '<br>City: ' + city + '<br>ZIP Code: ' + zipcode);
        } else {
            /*$('#result').html('Geocode was not successful for the following reason: ' + status);*/
        }
    });
}


//3
function getLatLngFromAddress(address) {


    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var lat = results[0].geometry.location.lat();
            var lng = results[0].geometry.location.lng();
          
           
            $('#maplatitude').val(lat);
            $('#maplongitude').val(lng);

        } else {
            console.log("Geocode was not successful for the following reason: " + status);
            
        }
    });
};

function onblurloadotherdetails(address) {
   
    getAddressDetails(address);
    getLatLngFromAddress(address);
}






//get curenty location method
function bindCurrentLocationToAddressField() {
    // Get a reference to all the "Click here to get your current location" links
    const getCurrentLocationLinks = document.querySelectorAll('a.acurrentlocation');

    // Loop through all the links and add a click event listener to each of them
    getCurrentLocationLinks.forEach(link => {
        link.addEventListener('click', async () => {
            // Get a reference to the text input field with the class "googleaddress"
            const addressField = link.closest('.input-group').querySelector('input.googleaddress');

            // Use the Permissions API to request permission to access the user's location
            let permissionStatus;
            try {
                permissionStatus = await navigator.permissions.query({ name: 'geolocation' });
            } catch (error) {
                console.error(error);
                return;
            }

            // If the user has already granted permission, get the user's current location
            if (permissionStatus.state === 'granted') {
                getCurrentPosition(addressField);
            }
            // If the user has not yet granted permission, show a styled popup message asking for permission
            else {
                showPermissionPopup(addressField);
            }
        });
    });
};

function showPermissionPopup(addressField) {
    // Create a new container element for the popup message
    const popupContainer = document.createElement('div');
    popupContainer.style = `
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5);
        z-index: 1000009;
        display: flex;
        justify-content: center;
        align-items: center;
    `;

    // Create a new message element for the popup
    const popupMessage = document.createElement('div');
    popupMessage.style = `
        background-color: white;
        padding: 20px;
        border-radius: 10px;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
        text-align: center;
    `;

    // Create a new heading element for the popup message
    const popupHeading = document.createElement('h2');
    popupHeading.textContent = 'Allow Location Access?';

    // Create a new paragraph element for the popup message
    const popupParagraph = document.createElement('p');
    popupParagraph.textContent = 'Please allow this website to access your location.';

    // Create a new button element for the popup message
    const popupButton = document.createElement('button');
    popupButton.textContent = 'Allow';
    popupButton.style = `
        background-color: #4CAF50;
        color: white;
        padding: 10px;
        border: none;
        border-radius: 5px;
        cursor: pointer;
    `;

    // Add a click event listener to the button element
    popupButton.addEventListener('click', () => {
        // Try to get the user's location
        navigator.geolocation.getCurrentPosition(
            position => {
                const lat = position.coords.latitude;
                const lng = position.coords.longitude;
                const geocoder = new google.maps.Geocoder();
                const latlng = new google.maps.LatLng(lat, lng);
                // Use the geocoder to get the address from the latlng object
                geocoder.geocode({ 'latLng': latlng }, (results, status) => {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[0]) {
                            // Set the value of the address field to the formatted address
                            addressField.value = results[0].formatted_address;

                            getAddressDetails(results[0].formatted_address);
                        }
                    } else {
                        console.error('Geocoder failed due to: ' + status);
                    }
                });
            },
            error => {
                console.error(error);
            }
        );
        // Remove the popup message and container elements
        popupContainer.remove();
    });

    // Append the heading, paragraph, and button elements to the popup message
    popupMessage.appendChild(popupHeading);
    popupMessage.appendChild(popupParagraph);
    popupMessage.appendChild(popupButton);

    // Append the popup message element to the container element
    popupContainer.appendChild(popupMessage);

    // Append the container element to the document body
    document.body.appendChild(popupContainer);
};

// Function to get the user's current position and set the address field value
//function getCurrentPosition(addressField) {
//    navigator.geolocation.getCurrentPosition(
//        // Success callback function
//        position => {
//            // Get the latitude and longitude of the user's current position
//            const { latitude, longitude } = position.coords;
//            // Construct the URL for the reverse geocoding API request


//          /*  getKeys("GoogleMAPKey", function (response) {*/


//            const url = 'https://maps.googleapis.com/maps/api/geocode/json?latlng=' + latitude + ',' + longitude + '&key=AIzaSyBqbbVqPMBsVj_0VeHM7bj0bd1MPQP0e14';
//                // Send the reverse geocoding API request
//                fetch(url)
//                    .then(response => response.json())
//                    .then(data => {
//                        // Extract the formatted address from the reverse geocoding API response
//                        const formattedAddress = data.results[0].formatted_address;
//                        // Set the value of the text input field to the formatted address
//                        addressField.value = formattedAddress;

//                        onblurloadotherdetails(addressField.value);
//                    });
//          /*  });*/
//        },
//        // Error callback function
//        error => {
//            console.error(error);
//        },
//        // Geolocation options (optional)
//        {
//            enableHighAccuracy: true,
//            timeout: 5000,
//            maximumAge: 0
//        }
//    );
//};

function getCurrentPosition(addressField) {
    // Fetch the Google Maps API key first
    getGoogleMapApiKey(apiKey => {

        
        navigator.geolocation.getCurrentPosition(
            // Success callback function
            position => {
                // Get the latitude and longitude of the user's current position
                const { latitude, longitude } = position.coords;
                // Construct the URL for the reverse geocoding API request
                const url = 'https://maps.googleapis.com/maps/api/geocode/json?latlng=' + latitude + ',' + longitude + '&key=' + apiKey;

                // Send the reverse geocoding API request
                fetch(url)
                    .then(response => response.json())
                    .then(data => {
                        // Extract the formatted address from the reverse geocoding API response
                        const formattedAddress = data.results[0].formatted_address;
                        // Set the value of the text input field to the formatted address
                        addressField.value = formattedAddress;

                        onblurloadotherdetails(addressField.value);
                    });
            },
            // Error callback function
            error => {
                console.error(error);
            },
            // Geolocation options (optional)
            {
                enableHighAccuracy: true,
                timeout: 5000,
                maximumAge: 0
            }
        );
    });
}



///get city, street and zipcode based on address

//var googlekey = "AIzaSyD_Ia-RHd5bIWMLLL05tUJlDy_7DHb2--4";
