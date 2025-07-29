var translations = null;
var currentLanguage = 'en'; // Set default language
var supportedLanguages = ['en', 'tr', 'ru', 'ar']; // Supported languages
var rtlLanguages = ['ar']; // RTL languages

// Initialize language from localStorage or default to 'en'
function initializeLanguage() {
    var savedLanguage = localStorage.getItem('selectedLanguage');
    if (savedLanguage && supportedLanguages.includes(savedLanguage)) {
        currentLanguage = savedLanguage;
    } else {
        currentLanguage = 'en';
        localStorage.setItem('selectedLanguage', currentLanguage);
    }
    
    // Set RTL direction
    updateTextDirection();
}

function setLanguage(language) {
    if (supportedLanguages.includes(language)) {
        currentLanguage = language;
        localStorage.setItem('selectedLanguage', language);
        updateTextDirection();
        updateTranslations();
        
        // Update language selector UI
        updateLanguageSelectorUI();
    }
}

// Update text direction based on language
function updateTextDirection() {
    var isRTL = rtlLanguages.includes(currentLanguage);
    var body = document.getElementById('mybody') || document.body;
    
    if (isRTL) {
        body.setAttribute('dir', 'rtl');
        body.classList.add('rtl');
        localStorage.setItem('isRTL', 'true');
    } else {
        body.setAttribute('dir', 'ltr');
        body.classList.remove('rtl');
        localStorage.setItem('isRTL', 'false');
    }
}

// Update language selector UI
function updateLanguageSelectorUI() {
    var currentLangElement = document.getElementById('currentLanguage');
    var currentFlagElement = document.getElementById('currentFlag');
    
    if (currentLangElement) {
        var languageNames = {
            'en': 'English',
            'tr': 'Türkçe', 
            'ru': 'Русский',
            'ar': 'العربية'
        };
        currentLangElement.textContent = languageNames[currentLanguage] || 'English';
    }
    
    if (currentFlagElement) {
        currentFlagElement.className = `flag-icon flag-icon-${getFlagCode(currentLanguage)}`;
    }
    
    // Update active state in dropdown
    document.querySelectorAll('.language-option').forEach(function(option) {
        var langCode = option.getAttribute('data-lang');
        if (langCode === currentLanguage) {
            option.classList.add('active');
        } else {
            option.classList.remove('active');
        }
    });
}

// Get flag code for language
function getFlagCode(langCode) {
    var flagCodes = {
        'en': 'us',
        'tr': 'tr',
        'ru': 'ru', 
        'ar': 'sa'
    };
    return flagCodes[langCode] || 'us';
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

// Get translation with fallback mechanism
function getTranslation(key, language) {
    if (!translations || !translations[key]) {
        return key; // Return key if translation not found
    }
    
    var translationObj = translations[key];
    
    // Try to get translation for requested language
    if (translationObj[language]) {
        return translationObj[language];
    }
    
    // Fallback to English if translation not available
    if (translationObj['en']) {
        return translationObj['en'];
    }
    
    // Return key if no translation found
    return key;
}

function translateElementsv1() {
    $('[data-translate]').each(function () {
        var key = $(this).attr('data-translate');
       
        var translation = getTranslation(key, currentLanguage);
        
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

        var translation = getTranslation(key, currentLanguage);
        
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

// Initialize language on page load
document.addEventListener("DOMContentLoaded", function () {
    initializeLanguage();
    updateTranslations();
});

$(document).on('click', '.open-popup-button', function () {
    setTimeout(function () {
        updateTranslations();
    }, 1000);
});

// Language selector event handlers
$(document).on('click', '.language-option', function(e) {
    e.preventDefault();
    var selectedLang = $(this).attr('data-lang');
    if (selectedLang && supportedLanguages.includes(selectedLang)) {
        setLanguage(selectedLang);
    }
});