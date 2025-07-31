var translations = null;
var currentLanguage = 'en'; // Set default language
var supportedLanguages = ['en', 'tr', 'de', 'fr']; // Supported languages

// Initialize language from localStorage or default to 'en'
function initializeLanguage() {
    var savedLanguage = localStorage.getItem('selectedLanguage');
    if (savedLanguage && supportedLanguages.includes(savedLanguage)) {
        currentLanguage = savedLanguage;
    } else {
        currentLanguage = 'en';
        localStorage.setItem('selectedLanguage', currentLanguage);
    }
}

function setLanguage(language) {
    if (supportedLanguages.includes(language)) {
        currentLanguage = language;
        localStorage.setItem('selectedLanguage', language);
        updateTranslations();
        
        // Update language selector UI
        updateLanguageSelectorUI();
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
            'de': 'Deutsch',
            'fr': 'Français'
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
        'de': 'de',
        'fr': 'fr'
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
    // ALWAYS fetch fresh translations (temporary fix)
    console.log('🔄 Force fetching fresh translations from server...');
    fetchTranslations(callback);
    
    // TODO: Re-enable caching after confirming Russian translations work
    /*
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
    */
}

function fetchTranslations(callback) {
    // Load translations from server with cache busting
    var cacheBuster = new Date().getTime();
    $.ajax({
        url: '/controller/Language/LabelLoads?v=' + cacheBuster,
        dataType: 'json',
        cache: false, // Disable jQuery cache
        headers: {
            'Cache-Control': 'no-cache, no-store, must-revalidate',
            'Pragma': 'no-cache',
            'Expires': '0'
        },
        success: function (data) {
            translations = data;
            console.log('🔄 Fresh translations loaded from server:', Object.keys(data).length, 'keys');
            
            // Check if Russian translations exist
            if (data.login && data.login.ru) {
                console.log('✅ Russian translations found! login.ru =', data.login.ru);
            } else {
                console.warn('❌ Russian translations missing! Available languages for login:', 
                    data.login ? Object.keys(data.login) : 'login key not found');
            }
            
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
        console.warn('Translation missing for key:', key, 'in language:', language);
        return key; // Return key if translation not found
    }
    
    var translationObj = translations[key];
    
    // Try to get translation for requested language
    if (translationObj && translationObj[language]) {
        return translationObj[language];
    }
    
    // Fallback to English if translation not available
    if (translationObj && translationObj['en']) {
        console.warn('Using English fallback for key:', key, 'requested language:', language);
        return translationObj['en'];
    }
    
    // Return key if no translation found
    console.warn('No translation found for key:', key);
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