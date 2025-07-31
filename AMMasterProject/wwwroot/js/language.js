var translations = null;
var currentLanguage = 'en'; // Set default language
var supportedLanguages = ['en', 'tr', 'de', 'fr']; // Supported languages

// Database content mapping for translation
var databaseContentMap = {
    // Category mappings - with variations
    'Mens Fashion': 'mensfashion',
    'Phones and Telecommunications': 'phonesandtelecommunications', 
    'Computer Office and Security': 'computeroffice',
    'Computer Office and  Security': 'computeroffice', // Extra space variation
    'Consumer Electronics': 'consumerelectronics',
    'Jewelry and Watches': 'jewelryandwatches',
    'Home Pet and Appliances': 'homepetappliances',
    'Bags and Shoes': 'bagsandshoes',
    'Toys Kids and Babies': 'toyskidsbabies',
    'Outdoor Fun and Sports': 'outdoorfunsports',
    'Beauty Health and Hair': 'beautyhealthhair',
    'Automobiles and Motorcycles': 'automobilesmotorcycles',
    'Tools and Home Improvement': 'toolshomeimprovement',
    'Music': 'music',
    'Kindle E Readers and Books': 'kindleebooks',
    'Appstore for Android': 'appstore',
    
    // Product name patterns
    'Summer Slippers': 'summerslipper',
    '023 Summer Slippers Men Flip Flops Beach Sandals': 'summerslipper',
    '2022 New Men\'s Ladies Baseball Cap': 'baseballcap',
    '10 Color Winter Mens Turtleneck Sweaters Warm': 'turtlenecksweater',
    '2023 New Summer French Retro High-heeled Lady': 'frenchretroheels',
    
    // Common terms
    'About us': 'about',
    'Contact us': 'contact',
    'Buy Now': 'buynow',
    'Download App': 'downloadapp',
    'Become a Seller': 'becomeseller',
    'RTL Demo': 'rtldemo'
};

// Normalize text for better matching
function normalizeText(text) {
    return text.replace(/\s+/g, ' ').trim(); // Replace multiple spaces with single space
}

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
        
        console.log('🔄 Language changed to:', language);
        
        // Load translations first, then translate
        if (!translations) {
            loadTranslations(function() {
                translateAllContent();
                updateLanguageSelectorUI();
            });
        } else {
            translateAllContent();
            updateLanguageSelectorUI();
        }
    }
}

// Master translation function - translates everything
function translateAllContent() {
    console.log('🌍 Starting translation to:', currentLanguage);
    
    if (!translations) {
        console.warn('⚠️ Translations not loaded yet');
        return;
    }
    
    // 1. Translate static elements with data-translate
    translateStaticElements();
    
    // 2. Translate database-driven content
    translateDatabaseContent();
    
    // 3. Translate dynamic categories
    translateCategories();
    
    // 4. Translate product titles
    translateProductTitles();
    
    // 5. Translate header links
    translateHeaderLinks();
    
    console.log('✅ Translation completed for:', currentLanguage);
}

function translateStaticElements() {
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

function translateDatabaseContent() {
    console.log('🔄 Translating database content...');
    
    // Translate all text content that matches database mappings
    $('*').contents().filter(function() {
        return this.nodeType === 3; // Text nodes only
    }).each(function() {
        var text = normalizeText($(this).text());
        if (text && databaseContentMap[text]) {
            var translationKey = databaseContentMap[text];
            var translation = getTranslation(translationKey, currentLanguage);
            if (translation !== translationKey) {
                console.log(`📝 Translating text node: "${text}" → "${translation}"`);
                $(this).replaceWith(translation);
            }
        }
    });
}

function translateCategories() {
    console.log('🔄 Translating categories...');
    
    // Target category links specifically
    $('.first-level-a, .second-level-a, .third-level-a').each(function() {
        var $this = $(this);
        var categoryText = normalizeText($this.text());
        
        console.log(`🔍 Checking category: "${categoryText}"`);
        
        if (databaseContentMap[categoryText]) {
            var translationKey = databaseContentMap[categoryText];
            var translation = getTranslation(translationKey, currentLanguage);
            if (translation !== translationKey) {
                console.log(`✅ Translating category: "${categoryText}" → "${translation}"`);
                
                // Get the icon element if it exists
                var $icon = $this.find('.first-level-icon, .second-level-icon, .third-level-icon');
                
                if ($icon.length > 0) {
                    // Keep icon, replace text
                    $this.html('').append($icon).append(' ' + translation);
                } else {
                    // No icon, just replace text
                    $this.text(translation);
                }
            }
        } else {
            console.log(`❌ No mapping found for: "${categoryText}"`);
        }
    });
}

function translateProductTitles() {
    console.log('🔄 Translating product titles...');
    
    // Translate product titles in various containers
    $('.p-title').each(function() {
        var $this = $(this);
        var productText = normalizeText($this.text());
        
        console.log(`🔍 Checking product: "${productText}"`);
        
        // Check for exact matches first
        if (databaseContentMap[productText]) {
            var translationKey = databaseContentMap[productText];
            var translation = getTranslation(translationKey, currentLanguage);
            if (translation !== translationKey) {
                console.log(`✅ Translating product: "${productText}" → "${translation}"`);
                $this.text(translation);
            }
        } else {
            // Check for partial matches
            var found = false;
            for (var pattern in databaseContentMap) {
                if (productText.includes(pattern)) {
                    var translationKey = databaseContentMap[pattern];
                    var translation = getTranslation(translationKey, currentLanguage);
                    if (translation !== translationKey) {
                        console.log(`✅ Translating product (partial): "${productText}" → "${translation}"`);
                        $this.text(translation);
                        found = true;
                        break;
                    }
                }
            }
            if (!found) {
                console.log(`❌ No mapping found for product: "${productText}"`);
            }
        }
    });
}

function translateHeaderLinks() {
    // Translate header navigation links
    $('#dvheaderlinks a, .header-pages a').each(function() {
        var $this = $(this);
        var linkText = $this.text().trim();
        
        if (databaseContentMap[linkText]) {
            var translationKey = databaseContentMap[linkText];
            var translation = getTranslation(translationKey, currentLanguage);
            if (translation !== translationKey) {
                $this.text(translation);
            }
        }
    });
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
        translateAllContent();
    } else {
       /*  Load translations from cache or server*/
        loadTranslations(function () {
            translateAllContent();
        });
    }
};

function loadTranslations(callback) {
    // ALWAYS fetch fresh translations (temporary fix)
    console.log('🔄 Force fetching fresh translations from server...');
    fetchTranslations(callback);
    
    // TODO: Re-enable caching after confirming translations work
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
            
            // Check if German and French translations exist
            if (data.login && data.login.de && data.login.fr) {
                console.log('✅ German & French translations found!');
                console.log('🇩🇪 German login:', data.login.de);
                console.log('🇫🇷 French login:', data.login.fr);
            } else {
                console.warn('❌ German/French translations missing! Available languages for login:', 
                    data.login ? Object.keys(data.login) : 'login key not found');
            }
            
            // Cache translations with timestamp
            localStorage.setItem('translations', JSON.stringify(translations));
            localStorage.setItem('translationsTimestamp', new Date().getTime().toString());
            callback();
        },
        error: function (xhr, status, error) {
            console.error('❌ Server endpoint failed:', error);
            console.log('🔄 Trying fallback: loading from static JSON...');
            
            // Fallback: Load from static JSON file
            $.ajax({
                url: '/languages.json?v=' + cacheBuster,
                dataType: 'json',
                cache: false,
                success: function(data) {
                    translations = data;
                    console.log('✅ Fallback successful! Loaded from static JSON:', Object.keys(data).length, 'keys');
                    
                    // Cache translations
                    localStorage.setItem('translations', JSON.stringify(translations));
                    localStorage.setItem('translationsTimestamp', new Date().getTime().toString());
                    callback();
                },
                error: function(xhr2, status2, error2) {
                    console.error('❌ Both server and static JSON failed!', error2);
                    
                    // Last resort: Create minimal translations
                    translations = createMinimalTranslations();
                    console.log('⚠️ Using minimal fallback translations');
                    callback();
                }
            });
        }
    });
}

// Create minimal translations as last resort
function createMinimalTranslations() {
    return {
        "mensfashion": {
            "en": "Mens Fashion",
            "tr": "Erkek Modası",
            "de": "Herrenmode",
            "fr": "Mode homme"
        },
        "phonesandtelecommunications": {
            "en": "Phones and Telecommunications",
            "tr": "Telefon ve Telekomünikasyon",
            "de": "Telefone und Telekommunikation",
            "fr": "Téléphones et télécommunications"
        },
        "categories": {
            "en": "Categories",
            "tr": "Kategoriler", 
            "de": "Kategorien",
            "fr": "Catégories"
        },
        "login": {
            "en": "Login",
            "tr": "Giriş Yap",
            "de": "Anmelden",
            "fr": "Connexion"
        },
        "buynow": {
            "en": "Buy Now",
            "tr": "Şimdi Satın Al",
            "de": "Jetzt kaufen",
            "fr": "Acheter maintenant"
        }
    };
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

// BACKWARD COMPATIBILITY - Keep old function names
function translateElements() {
    translateAllContent();
}

function translateDynamicCategories() {
    translateCategories();
}

function translateDynamicProducts() {
    translateProductTitles();
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