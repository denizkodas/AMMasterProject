// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

///Date functions

  //<span class="setdateformat">
  //                              @item.OrderDate
  //                         </span> 

  //                          <span class="settimeformat">
  //                              @item.OrderDate
  //                          </span>

$(document).ready(function () {
    // Call setDateformat with a 2-second delay
    setTimeout(function () {
        setDateformat();
        setTimeformat();
        set2Digits();
        set1Digits();
        setCommaSeparated();

        setKFormat();
    }, 2000);

   
});


function setDateformat() {
    $.ajax({
        type: 'GET',
        url: '/controller/AppSetting/DateFormat',
        data: {
            type: 'date',
        },
        success: function (data) {

          
            // Check if Format is a valid string
            
                // Assuming data.Format contains the desired date format
            var dateFormat = data.format;
            
                // Format each element with the specified date format
            $('.setdateformat').each(function () {
                var orderDate = $(this).text().trim();

                // Check if orderDate is not empty or not a valid date
                if (orderDate && !isNaN(new Date(orderDate).getTime())) {
                    var formattedDate = formatDate(orderDate, dateFormat);
                    $(this).text(formattedDate);
                } else {
                    // Handle the case where orderDate is empty or not a valid date
                    console.log('Invalid or empty date:', orderDate);
                }
            });
            
        },
        error: function (err) {
            // Handle error if any
            console.log("erro" +err);
        }
    });
}

function setTimeformat() {
    $.ajax({
        type: 'GET',
        url: '/controller/AppSetting/DateFormat',
        data: {
            type: 'time',
        },
        success: function (data) {
            // Assuming data.format contains the desired time format
            var timeFormat = data.format;

            // Format each element with the specified time format
            $('.settimeformat').each(function () {
                var orderTime = $(this).text().trim();
                var formattedTime = formatTime(orderTime, timeFormat);
                $(this).text(formattedTime);
            });
        },
        error: function (err) {
            // Handle error if any
            console.log(err);
        }
    });
}


function formatTime(dateTimeString, format) {
    var time = new Date(dateTimeString); // Parse the full date and time

    // Check if the time is valid
    if (isNaN(time.getTime())) {
        return '';
    }

    // Extract components
    var hours = time.getHours();
    var minutes = time.getMinutes();
    var seconds = time.getSeconds();

    // Determine whether to include AM/PM based on the format
    var includeAMPM = format.includes('tt');

    // Convert to 12-hour format if AM/PM is included
    if (includeAMPM) {
        var ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // Handle midnight (12 AM)
    }

    // Add leading zeros to single-digit hours, minutes, and seconds
    hours = ('0' + hours).slice(-2);
    minutes = ('0' + minutes).slice(-2);
    seconds = ('0' + seconds).slice(-2);

    // Construct the formatted time string based on the selected format
    var formattedTime;
    if (format === 'hh:mm:ss tt') {
        formattedTime = hours + ':' + minutes + ':' + seconds + ' ' + ampm;
    } else if (format === 'hh:mm tt') {
        formattedTime = hours + ':' + minutes + ' ' + ampm;
    } else if (format === 'hh:mm:ss') {
        formattedTime = hours + ':' + minutes + ':' + seconds;
    } else if (format === 'hh:mm') {
        formattedTime = hours + ':' + minutes;
    } else {
        // Default to 'hh:mm:ss tt' if an invalid format is provided
        formattedTime = hours + ':' + minutes + ':' + seconds + ' ' + ampm;
    }

    return formattedTime;
}

function formatDate(dateString, format) {
    var date = new Date(dateString);

   
    // Mapping of format tokens to date components
    var formatMapping = {
        'dd': ('0' + date.getDate()).slice(-2),
        'MM': ('0' + (date.getMonth() + 1)).slice(-2),
        'MMM': monthNames[date.getMonth()], // Use full month name
        'yyyy': date.getFullYear(),
        'hh': ('0' + date.getHours()).slice(-2),
        'mm': ('0' + date.getMinutes()).slice(-2),
        'ss': ('0' + date.getSeconds()).slice(-2),

       
    };

  
    format = format.replace(/(dd|MMM|MM|yyyy|hh|mm|ss)/g, function (match) {
        return formatMapping[match];

        
    });
  
    return format;
}

// Example full month names array
var monthNames = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
];

//function set1Digits() {
//    $('.1digit').each(function () {
//        // Get the content of each element
//        var originalValue = $(this).text();

//        // Parse the content as a float
//        var floatValue = parseFloat(originalValue);

//        // Check if the content is a valid number
//        if (!isNaN(floatValue)) {
//            // Format the content to have exactly two decimal places
//            var formattedValue = floatValue.toFixed(1);

//            // Set the formatted content back to the element
//            $(this).text(formattedValue);
//        }
//    });
//}

function set1Digits() { 
    $('.1digit').each(function () {
        // Get the content of each element
        var originalValue = $(this).text();

        // Parse the content as a float
        var floatValue = parseFloat(originalValue);

        // Check if the content is a valid number
        if (!isNaN(floatValue)) {
            // Format the content to have exactly two decimal places
            var formattedValue = floatValue.toFixed(1);

            // Set the formatted content back to the element
            $(this).text(formattedValue);
        }
    });
}

function set2Digits() {
    $('.2digit').each(function () {
        // Get the content of each element
        var originalValue = $(this).text();

        // Parse the content as a float
        var floatValue = parseFloat(originalValue);

        // Check if the content is a valid number
        if (!isNaN(floatValue)) {
            // Format the content to have exactly two decimal places
            var formattedValue = floatValue.toFixed(2);

            // If there is only one decimal place, add a trailing zero
            if (formattedValue.indexOf('.') === formattedValue.length - 2) {
                formattedValue += '0';
            }

            // Set the formatted content back to the element
            $(this).text(formattedValue);
        }
    });
}


function setroundoof() {
    $('.roundoff').each(function () {
        // Get the content of each element
        var originalValue = $(this).text();

        // Parse the content as an integer
        var integerValue = parseInt(originalValue);

        // Check if the content is a valid number
        if (!isNaN(integerValue)) {
            // Format the content with commas for thousands
            var formattedValue = integerValue.toLocaleString();

            // Set the formatted content back to the element
            $(this).text(formattedValue);
        }
    });
}

function setCommaSeparated() {
    $('.comma-separate').each(function () {
        // Get the content of each element
        var originalValue = $(this).text();

        // Parse the content as a float
        var floatValue = parseFloat(originalValue);

        // Check if the content is a valid number
        if (!isNaN(floatValue)) {
            // Format the content with commas for thousands and ensure max 2 decimal places
            var formattedValue = floatValue.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');

            // Set the formatted content back to the element
            $(this).text(formattedValue);
        }
    });
}




function formatNumberToK(number) {
    if (number >= 1000) {
        var formattedNumber = (number / 1000).toFixed(1);
        return formattedNumber + 'K';
    } else {
        return number.toString();
    }
}

function setKFormat() {
    $('.kformat').each(function () {
        // Get the content of each element
        var originalValue = $(this).text();

        // Parse the content as a float
        var floatValue = parseFloat(originalValue);

        // Check if the content is a valid number
        if (!isNaN(floatValue)) {
            // Format the content to "K" format
            var formattedValue = formatNumberToK(floatValue);

            // Set the formatted content back to the element
            $(this).text(formattedValue);
        }
    });
}