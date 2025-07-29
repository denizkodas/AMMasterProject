////Language

var translations = null;
var currentLanguage = 'en';

function reloadfile() {

  
    translations = null;

    //console.log("relad file");
}

function setLanguage(language) {

   
    //console.log(language);
    //localStorage.setItem('languageselection', language);
    currentLanguage = language;
    updateTranslations();
};

function loadupdatedfile() {
    translateElements();
}

function updateTranslations() {
    // Check if translations are already loaded
    if (translations) {
        translateElements();
    } else {
        // Load translations from cache or server
        loadTranslations(function () {
            translateElements();
        });
    }
};



function loadTranslations(callback) {
    // Load translations from server using web method
    $.ajax({
        type: "GET",
        url: "/controller/language/MasterLanguage", // Update the URL to point to your web method
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

           
            var jsonData = response; 
            translations = JSON.parse(jsonData);
            // Cache translations
            localStorage.setItem('ammasterprojecttranslation', JSON.stringify(translations));
            callback();

           
        },
        error: function (xhr, status, error) {
            console.error("Error fetching translations:", error);
            // Handle error if needed
        }
    });
}


function translateElements() {
   /* setInterval(function () {*/

      
        $('[data-translate]').each(function () {
            var key = $(this).attr('data-translate');
            var translation = translations[key] && translations[key][currentLanguage]; // Check if translation exists

            // Handle dynamic content with placeholders
            if ($(this).attr('data-params')) {
                var params = JSON.parse($(this).attr('data-params'));
                for (var param in params) {
                    if (translation) { // Check if translation exists before replacing placeholders
                        translation = translation.replace(`{${param}}`, params[param]);
                    }
                }
            }
            // Set translated text or handle missing translation
            $(this).text(translation || `Translation Missing for ${key}`);
        });

    /*}, 3000);*/
};


///Language END
