// Language System Test Script
// This file is for testing purposes and can be removed in production

console.log('🌐 Language System Test Started');

// Test 1: Check if language functions are available
function testLanguageFunctions() {
    console.log('📋 Test 1: Checking language functions...');
    
    const requiredFunctions = [
        'setLanguage',
        'updateTranslations', 
        'translateElements',
        'getTranslation',
        'initializeLanguage',
        'updateTextDirection',
        'updateLanguageSelectorUI'
    ];
    
    requiredFunctions.forEach(funcName => {
        if (typeof window[funcName] === 'function') {
            console.log(`✅ ${funcName} function is available`);
        } else {
            console.log(`❌ ${funcName} function is missing`);
        }
    });
}

// Test 2: Check if language selector elements exist
function testLanguageSelectorElements() {
    console.log('📋 Test 2: Checking language selector elements...');
    
    const requiredElements = [
        '#languageSelector',
        '#languageDropdown', 
        '#currentLanguage',
        '#currentFlag',
        '.language-option'
    ];
    
    requiredElements.forEach(selector => {
        const element = document.querySelector(selector);
        if (element) {
            console.log(`✅ Element ${selector} exists`);
        } else {
            console.log(`❌ Element ${selector} is missing`);
        }
    });
}

// Test 3: Check supported languages
function testSupportedLanguages() {
    console.log('📋 Test 3: Checking supported languages...');
    
    if (typeof supportedLanguages !== 'undefined') {
        console.log(`✅ Supported languages: ${supportedLanguages.join(', ')}`);
        
        const expectedLanguages = ['en', 'tr', 'ru', 'ar'];
        expectedLanguages.forEach(lang => {
            if (supportedLanguages.includes(lang)) {
                console.log(`✅ Language ${lang} is supported`);
            } else {
                console.log(`❌ Language ${lang} is missing`);
            }
        });
    } else {
        console.log('❌ supportedLanguages array is not defined');
    }
}

// Test 4: Check translations loading
function testTranslationsLoading() {
    console.log('📋 Test 4: Testing translations loading...');
    
    // Test AJAX call to translations endpoint
    $.ajax({
        url: '/controller/Language/LabelLoads',
        method: 'GET',
        dataType: 'json',
        success: function(data) {
            console.log('✅ Translations loaded successfully');
            console.log('📊 Sample translations:', {
                login: data.login,
                register: data.register,
                home: data.home
            });
            
            // Test fallback mechanism
            if (data.login && data.login.en && data.login.tr && data.login.ru && data.login.ar) {
                console.log('✅ All language translations are available for "login"');
            } else {
                console.log('⚠️ Some language translations are missing for "login"');
            }
        },
        error: function(xhr, status, error) {
            console.log('❌ Failed to load translations:', error);
        }
    });
}

// Test 5: Test language switching
function testLanguageSwitching() {
    console.log('📋 Test 5: Testing language switching...');
    
    const testLanguages = ['en', 'tr', 'ru', 'ar'];
    let currentIndex = 0;
    
    function switchToNextLanguage() {
        if (currentIndex < testLanguages.length) {
            const lang = testLanguages[currentIndex];
            console.log(`🔄 Switching to ${lang}...`);
            
            if (typeof setLanguage === 'function') {
                setLanguage(lang);
                
                // Check if RTL is applied for Arabic
                if (lang === 'ar') {
                    setTimeout(() => {
                        const body = document.body;
                        if (body.getAttribute('dir') === 'rtl') {
                            console.log('✅ RTL direction applied for Arabic');
                        } else {
                            console.log('❌ RTL direction not applied for Arabic');
                        }
                    }, 100);
                }
                
                currentIndex++;
                setTimeout(switchToNextLanguage, 1000);
            } else {
                console.log('❌ setLanguage function not available');
            }
        } else {
            console.log('✅ Language switching test completed');
            // Switch back to English
            if (typeof setLanguage === 'function') {
                setLanguage('en');
            }
        }
    }
    
    switchToNextLanguage();
}

// Test 6: Test data-translate elements
function testDataTranslateElements() {
    console.log('📋 Test 6: Testing data-translate elements...');
    
    const elements = document.querySelectorAll('[data-translate]');
    console.log(`✅ Found ${elements.length} elements with data-translate attribute`);
    
    // Show first 5 elements for verification
    Array.from(elements).slice(0, 5).forEach((el, index) => {
        const key = el.getAttribute('data-translate');
        const text = el.textContent.trim();
        console.log(`📝 Element ${index + 1}: key="${key}", text="${text}"`);
    });
}

// Run all tests when DOM is ready
$(document).ready(function() {
    setTimeout(() => {
        console.log('🚀 Starting Language System Tests...');
        
        testLanguageFunctions();
        testLanguageSelectorElements();
        testSupportedLanguages();
        testTranslationsLoading();
        testDataTranslateElements();
        
        // Run language switching test after a delay
        setTimeout(() => {
            testLanguageSwitching();
        }, 2000);
        
        console.log('✨ Language System Tests Initiated');
    }, 500);
});

// Export test functions for manual testing
window.languageTests = {
    testLanguageFunctions,
    testLanguageSelectorElements,
    testSupportedLanguages,
    testTranslationsLoading,
    testLanguageSwitching,
    testDataTranslateElements
};