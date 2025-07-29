var translations = null;
var currentLanguage = 'en'; // Set default language


function setLanguage(language) {
    currentLanguage = language;
    updateTranslations();

    
}

// Retrieve cookie value by name
function getCookie(name) {
    var cookies = document.cookie.split(';');
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i].trim();
        if (cookie.indexOf(name + '=') === 0) {
            return cookie.substring(name.length + 1);
        }
    }
    return null;
}

// Call updateTranslations function on page load

function updateTranslations() {
    // Check if translations are already loaded
    if (translations) {
        translateElements();
    } else {
       /*  Load translations from cache or server*/
        loadTranslations(function () {
            translateElements();
        });
    }
};

function loadTranslations(callback) {
    // Check if translations are cached
    var cachedTranslations = localStorage.getItem('translations');
    var cacheTimestamp = localStorage.getItem('translationsTimestamp');

    if (cachedTranslations && cacheTimestamp) {
        var currentTime = new Date().getTime();
        var cacheAge = currentTime - parseInt(cacheTimestamp);

        // If the cache is older than 1 day (86400000 milliseconds), fetch new translations
        if (cacheAge < 0 || cacheAge > 86400000) {
            fetchTranslations(callback);
        } else {
            translations = JSON.parse(cachedTranslations);
            callback();
        }
    } else {
        // Load translations from server if not cached
        fetchTranslations(callback);
    }
}

function fetchTranslations(callback) {
    // Load translations from server
    $.ajax({
        url: '/controller/Language/LabelLoads', // Update the URL to match your controller and action
        dataType: 'json',
        success: function (data) {
            translations = data;
            // Cache translations with timestamp
            localStorage.setItem('translations', JSON.stringify(translations));
            localStorage.setItem('translationsTimestamp', new Date().getTime().toString());
            callback();
        },
        error: function (xhr, status, error) {
            console.error('Error loading translations:', error);
            // Handle the error condition
            // For example, you can display an error message to the user or fallback to default translations
        }
    });
}

//function loadTranslations(callback) {
//    // Check if translations are cached
//    var cachedTranslations = localStorage.getItem('translations');
//    if (cachedTranslations) {
//        translations = JSON.parse(cachedTranslations);
//        callback();
//    } else {
//        // Load translations from server
//        $.ajax({
//            url: '/controller/Language/LabelLoads', // Update the URL to match your controller and action
//            dataType: 'json',
//            success: function (data) {
//                translations = data;
//                // Cache translations
//                localStorage.setItem('translations', JSON.stringify(translations));
//                callback();
//            },
//            error: function (xhr, status, error) {
//                console.error('Error loading translations:', error);
//                // Handle the error condition
//                // For example, you can display an error message to the user or fallback to default translations
//            }
//        });
//    }
//}

function translateElementsv1() {
    $('[data-translate]').each(function () {
        var key = $(this).attr('data-translate');
       
        var translation = translations[key][currentLanguage];
        // Handle dynamic content with placeholders
        if ($(this).attr('data-params')) {
            var params = JSON.parse($(this).attr('data-params'));
            for (var param in params) {
                translation = translation.replace(`{${param}}`, params[param]);
            }
        }
        $(this).text(translation);
    });
};

function translateElements() {
    $('[data-translate]').each(function () {
        var key = $(this).attr('data-translate');

        var translation = translations[key];
        // Handle dynamic content with placeholders
        if ($(this).attr('data-params')) {
            var params = JSON.parse($(this).attr('data-params'));
            for (var param in params) {
                translation = translation.replace(`{${param}}`, params[param]);
            }
        }
        $(this).text(translation);
    });
}


document.addEventListener("DOMContentLoaded", function () {

   
    updateTranslations();
});


$(document).on('click', '.open-popup-button', function () {
    setTimeout(function () {
        
        updateTranslations();
    }, 1000);
});