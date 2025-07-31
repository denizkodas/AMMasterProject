var translations = null;
var currentLanguage = 'en'; // Set default language
var supportedLanguages = ['en', 'tr', 'de', 'fr']; // Supported languages

// Database content mapping for translation
var databaseContentMap = {
    // Category mappings - with variations
    'Mens Fashion': 'mensfashion',
    'Men\'s Fashion': 'mensfashion',
    'Phones and Telecommunications': 'phonesandtelecommunications', 
    'Computer Office and Security': 'computeroffice',
    'Computer Office and  Security': 'computeroffice', // Extra space variation
    'Computer, Office & Security': 'computeroffice',
    'Consumer Electronics': 'consumerelectronics',
    'Jewelry and Watches': 'jewelryandwatches',
    'Jewelry & Watches': 'jewelryandwatches',
    'Home Pet and Appliances': 'homepetappliances',
    'Home, Pet & Appliances': 'homepetappliances',
    'Bags and Shoes': 'bagsandshoes',
    'Bags & Shoes': 'bagsandshoes',
    'Toys Kids and Babies': 'toyskidsbabies',
    'Toys, Kids & Babies': 'toyskidsbabies',
    'Outdoor Fun and Sports': 'outdoorfunsports',
    'Outdoor, Fun & Sports': 'outdoorfunsports',
    'Beauty Health and Hair': 'beautyhealthhair',
    'Beauty, Health & Hair': 'beautyhealthhair',
    'Automobiles and Motorcycles': 'automobilesmotorcycles',
    'Automobiles & Motorcycles': 'automobilesmotorcycles',
    'Tools and Home Improvement': 'toolshomeimprovement',
    'Tools & Home Improvement': 'toolshomeimprovement',
    'Music': 'music',
    'Kindle E Readers and Books': 'kindleebooks',
    'Kindle E-Readers and Books': 'kindleebooks',
    'Appstore for Android': 'appstore',
    'Sports & Entertainment': 'sportsandentertainment',
    'Sports and Entertainment': 'sportsandentertainment',
    'Health & Beauty': 'healthandbeauty',
    'Health and Beauty': 'healthandbeauty',
    'Women\'s Clothing': 'womensclothing',
    'Womens Clothing': 'womensclothing',
    'Mother & Kids': 'motherandkids',
    'Mother and Kids': 'motherandkids',
    'Food & Beverages': 'foodandbeverages',
    'Food and Beverages': 'foodandbeverages',
    'Furniture': 'furniture',
    'Lights & Lighting': 'lightsandlighting',
    'Lights and Lighting': 'lightsandlighting',
    'Security & Protection': 'securityandprotection',
    'Security and Protection': 'securityandprotection',
    'Electronic Components & Supplies': 'electroniccomponents',
    'Electronic Components and Supplies': 'electroniccomponents',
    
    // Product name patterns - exact matches first
    'Summer Slippers': 'summerslipper',
    '023 Summer Slippers Men Flip Flops Beach Sandals': 'summerslipper',
    '2022 New Men\'s Ladies Baseball Cap': 'baseballcap',
    '10 Color Winter Mens Turtleneck Sweaters Warm': 'turtlenecksweater',
    '2023 New Summer French Retro High-heeled Lady': 'frenchretroheels',
    
    // Hot Sale products - all variations
    'Hot Sale': 'hotsale',
    'Hot sale': 'hotsale',
    'HOT SALE': 'hotsale',
    'HOT SALES': 'hotsales',
    'Hot Sale Fashion Red Sneakers Men': 'hotsaleredsneakers',
    'Hot Sale Moccasins Driving Shoe Big Size 38-48': 'hotsalemoccasins',
    'Hot Sale Masha and Bear Children\'s Plush Doll': 'hotsalemashbear',
    'New Design Hot Sale gold-color Austria Crystal': 'hotsalecrystal',
    'Hot sale SD Memory Card 32GB 16GB 8GB SDHC': 'hotsalesdcard',
    'Video Action Sport Cameras hot sale': 'hotsaleactioncamera',
    'HOT SALES!!! Women Sexy V Neck Wrap Blouse': 'hotsaleblouse',
    
    // Jacket and clothing products
    'Zipper Girls Boys Jackets Kids Outfits 3-12 Years': 'kidszipper',
    'Hoodies Sweatshirts': 'hoodiessweatshirts',
    'Winter Warm Fleece Padded Thick Child Coat': 'winterchildcoat',
    'Men\'s Hoodies': 'menshoodies',
    'Women\'s Hoodies': 'womenshoodies',
    'T-Shirts': 'tshirts',
    'Polo Shirts': 'poloshirts',
    'Dress Shirts': 'dressshirts',
    'Casual Shirts': 'casualshirts',
    'Tank Tops': 'tanktops',
    'Sweaters': 'sweaters',
    'Cardigans': 'cardigans',
    'Jackets & Coats': 'jacketsandcoats',
    'Jackets and Coats': 'jacketsandcoats',
    'Blazers': 'blazers',
    'Suits': 'suits',
    'Jeans': 'jeans',
    'Pants': 'pants',
    'Shorts': 'shorts',
    'Underwear': 'underwear',
    'Sleepwear': 'sleepwear',
    
    // Electronics - expanded
    'HONOR 70 5G Mobile Phone Snapdragon': 'honor70phone',
    'NEW HONOR 70 5G Mobile Phone Snapdragon 778G+': 'honor70phone',
    'HONOR 70 5G Mobile Phone Snapdragon 778G+ Octa': 'honor70phone',
    'Smartphone Android Mobile Phone': 'androidphone',
    'Wireless Bluetooth Earphones Headset': 'bluetoothearphones',
    'Power Bank Portable Charger': 'powerbank',
    'USB Cable Fast Charging': 'usbcable',
    'Phone Case Cover Protection': 'phonecase',
    'Tablet PC Android WiFi': 'androidtablet',
    'Smart Watch Fitness Tracker': 'smartwatch',
    'Laptop Computer Notebook': 'laptop',
    'Gaming Mouse Wireless': 'gamingmouse',
    'Keyboard Mechanical RGB': 'mechanicalkeyboard',
    'Mobile Phones': 'mobilephones',
    'Tablets': 'tablets',
    'Computers & Laptops': 'computersandlaptops',
    'Computers and Laptops': 'computersandlaptops',
    'TV & Audio': 'tvandaudio',
    'TV and Audio': 'tvandaudio',
    'Cameras & Photo': 'camerasandphoto',
    'Cameras and Photo': 'camerasandphoto',
    'Video Games': 'videogames',
    'Headphones': 'headphones',
    'Speakers': 'speakers',
    'Chargers & Cables': 'chargersandcables',
    'Chargers and Cables': 'chargersandcables',
    
    // Memory and storage
    'Mini USB Flash Drive 16GB 32GB PenDrive': 'miniusbdrive',
    'SD Memory Card 32GB 16GB 8GB SDHC': 'sdmemorycard',
    'Memory Cards': 'memorycards',
    'USB Flash Drives': 'usbflashdrives',
    'External Hard Drives': 'externalharddrives',
    'SSD Drives': 'ssddrives',
    
    // Watches and accessories
    'New Arrival Cool Rubber Band RPM Speedo meter': 'coolwristwatch',
    'Digital LED Wrist Watch Gift': 'digitalwristwatch',
    'Wristwatches': 'wristwatches',
    'Smart Watches': 'smartwatches',
    'Fashion Watches': 'fashionwatches',
    'Sports Watches': 'sportswatches',
    'Luxury Watches': 'luxurywatches',
    
    // Shoes and footwear - expanded
    'Fashion Red Sneakers Men Comfortable High top': 'redsneakers',
    'Moccasins Driving Shoe Big Size': 'moccasinsshoe',
    'Comfortable Leather Casual Shoes Men Loafers': 'leathercasualshoes',
    'Men Casual Shoes Breathable Walking Sneakers': 'casualsneakers',
    'Women High Heels Platform Sandals Summer': 'womenhighheels',
    'Athletic Shoes': 'athleticshoes',
    'Running Shoes': 'runningshoes',
    'Basketball Shoes': 'basketballshoes',
    'Football Shoes': 'footballshoes',
    'Formal Shoes': 'formalshoes',
    'Boots': 'boots',
    'Sandals': 'sandals',
    'Slippers': 'slippers',
    'High Heels': 'highheels',
    'Flats': 'flats',
    'Wedges': 'wedges',
    
    // Toys and dolls
    'Masha and Bear Children\'s Plush Doll': 'mashbeardoll',
    'Cute Russian Princess Doll': 'russianprincessdoll',
    'Educational Wooden Puzzle Toys for Kids': 'woodenpuzzle',
    'Remote Control Racing Car Toy': 'rccar',
    'Action Figures': 'actionfigures',
    'Dolls & Accessories': 'dollsandaccessories',
    'Dolls and Accessories': 'dollsandaccessories',
    'Building Blocks': 'buildingblocks',
    'Puzzles': 'puzzles',
    'Board Games': 'boardgames',
    'RC Toys': 'rctoys',
    'Educational Toys': 'educationaltoys',
    'Stuffed Animals': 'stuffedanimals',
    'Baby Toys': 'babytoys',
    
    // Women's clothing - expanded  
    'Women Sexy V Neck Wrap Blouse': 'womenvneckblouse',
    'Solid Color Long Sleeve Slim Ribbed': 'solidcolorblouse',
    'Women Summer Dress Casual Short Sleeve': 'summerdress',
    'Ladies Fashion Handbag Shoulder Bag': 'fashionhandbag',
    'Dresses': 'dresses',
    'Tops & Blouses': 'topsandblouses',
    'Tops and Blouses': 'topsandblouses',
    'Skirts': 'skirts',
    'Lingerie & Sleepwear': 'lingerieandsleepwear',
    'Lingerie and Sleepwear': 'lingerieandsleepwear',
    'Swimwear': 'swimwear',
    'Activewear': 'activewear',
    'Maternity': 'maternity',
    
    // Men's clothing - expanded
    'Men Cotton T-Shirt Short Sleeve Casual': 'mentshirt',
    'Men Business Formal Shirt Long Sleeve': 'menformalshirt',
    'Men Winter Jacket Warm Thick Coat': 'menwinterjacket',
    'Men\'s Clothing': 'mensclothing',
    'Mens Clothing': 'mensclothing',
    'Men\'s Shoes': 'mensshoes',
    'Mens Shoes': 'mensshoes',
    'Men\'s Accessories': 'mensaccessories',
    'Mens Accessories': 'mensaccessories',
    'Men\'s Watches': 'menswatches',
    'Mens Watches': 'menswatches',
    
    // Jewelry and accessories
    'Fashion Gold Chain Necklace Women': 'goldnecklace',
    'Silver Ring Adjustable Size Crystal': 'silverring',
    'Luxury Watch Men Automatic Mechanical': 'luxurywatch',
    'Necklaces': 'necklaces',
    'Earrings': 'earrings',
    'Bracelets': 'bracelets',
    'Rings': 'rings',
    'Brooches': 'brooches',
    'Body Jewelry': 'bodyjewelry',
    'Fine Jewelry': 'finejewelry',
    'Fashion Jewelry': 'fashionjewelry',
    
    // Home and garden
    'Kitchen Utensils Cooking Tools Set': 'kitchentools',
    'LED String Lights Decoration Indoor': 'ledstringlights',
    'Bathroom Accessories Set Soap Dispenser': 'bathroomaccessories',
    'Home Decor': 'homedecor',
    'Kitchen & Dining': 'kitchenanddining',
    'Kitchen and Dining': 'kitchenanddining',
    'Bedding': 'bedding',
    'Bath & Shower': 'bathandshower',
    'Bath and Shower': 'bathandshower',
    'Storage & Organization': 'storageandorganization',
    'Storage and Organization': 'storageandorganization',
    'Garden & Patio': 'gardenandpatio',
    'Garden and Patio': 'gardenandpatio',
    'Home Improvement': 'homeimprovement',
    'Appliances': 'appliances',
    'Lighting': 'lighting',
    
    // Sports and outdoor
    'Fitness Resistance Bands Exercise Set': 'resistancebands',
    'Camping Tent Waterproof 2-4 Person': 'campingtent',
    'Bicycle Accessories Safety Helmet': 'bicyclehelmet',
    'Exercise & Fitness': 'exerciseandfitness',
    'Exercise and Fitness': 'exerciseandfitness',
    'Outdoor Recreation': 'outdoorrecreation',
    'Team Sports': 'teamsports',
    'Water Sports': 'watersports',
    'Winter Sports': 'wintersports',
    'Cycling': 'cycling',
    'Fishing': 'fishing',
    'Hunting': 'hunting',
    'Golf': 'golf',
    'Tennis': 'tennis',
    'Basketball': 'basketball',
    'Football': 'football',
    'Soccer': 'soccer',
    
    // Pet supplies
    'Dog Collar Adjustable Comfortable': 'dogcollar',
    'Cat Toy Interactive Playing Ball': 'cattoy',
    'Pet Food Bowl Stainless Steel': 'petfoodbowl',
    'Pet Supplies': 'petsupplies',
    'Dog Supplies': 'dogsupplies',
    'Cat Supplies': 'catsupplies',
    'Bird Supplies': 'birdsupplies',
    'Fish & Aquatic Pets': 'fishandaquaticpets',
    'Fish and Aquatic Pets': 'fishandaquaticpets',
    'Small Animals': 'smallanimals',
    'Pet Food': 'petfood',
    'Pet Toys': 'pettoys',
    'Pet Grooming': 'petgrooming',
    'Pet Health': 'pethealth',
    
    // Beauty and health
    'Face Mask Moisturizing Anti-Aging': 'facemask',
    'Hair Care Shampoo Natural Organic': 'hairshampoo',
    'Makeup Brush Set Professional': 'makeupbrushset',
    'Makeup': 'makeup',
    'Skincare': 'skincare',
    'Hair Care': 'haircare',
    'Fragrances': 'fragrances',
    'Personal Care': 'personalcare',
    'Health Care': 'healthcare',
    'Vitamins & Supplements': 'vitaminsandsupplements',
    'Vitamins and Supplements': 'vitaminsandsupplements',
    'Medical Supplies': 'medicalsupplies',
    'Oral Care': 'oralcare',
    'Vision Care': 'visioncare',
    
    // Automotive
    'Car Electronics': 'carelectronics',
    'Car Accessories': 'caraccessories',
    'Motorcycle Accessories': 'motorcycleaccessories',
    'Tires & Wheels': 'tiresandwheels',
    'Tires and Wheels': 'tiresandwheels',
    'Car Parts': 'carparts',
    'Motorcycle Parts': 'motorcycleparts',
    'Tools & Equipment': 'toolsandequipment',
    'Tools and Equipment': 'toolsandequipment',
    'Car Care': 'carcare',
    'Oils & Fluids': 'oilsandfluids',
    'Oils and Fluids': 'oilsandfluids',
    
    // Books and media
    'Books': 'books',
    'E-Books': 'ebooks',
    'Audiobooks': 'audiobooks',
    'Magazines': 'magazines',
    'Comics': 'comics',
    'Movies & TV': 'moviesandtv',
    'Movies and TV': 'moviesandtv',
    'Music CDs': 'musiccds',
    'Vinyl Records': 'vinylrecords',
    
    // Office supplies
    'Office Supplies': 'officesupplies',
    'Stationery': 'stationery',
    'School Supplies': 'schoolsupplies',
    'Art Supplies': 'artsupplies',
    'Printers & Scanners': 'printersandscanners',
    'Printers and Scanners': 'printersandscanners',
    'Office Furniture': 'officefurniture',
    'Filing & Organization': 'filingandorganization',
    'Filing and Organization': 'filingandorganization',
    
    // Common terms and navigation
    'About us': 'about',
    'About Us': 'about',
    'Contact us': 'contact',
    'Contact Us': 'contact',
    'Buy Now': 'buynow',
    'Download App': 'downloadapp',
    'Become a Seller': 'becomeseller',
    'RTL Demo': 'rtldemo',
    'All Categories': 'allcategories',
    'Show More': 'showmore',
    'Load More': 'loadmore',
    'View All': 'viewall',
    'See All': 'seeall',
    'More': 'more',
    'Best Seller': 'bestseller',
    'New Arrival': 'newarrival',
    'Featured': 'featured',
    'Popular': 'popular',
    'Trending': 'trending',
    'Sale': 'sale',
    'Discount': 'discount',
    'Free Shipping': 'freeshipping',
    'Limited Time': 'limitedtime',
    'Special Offer': 'specialoffer',
    'Add to Cart': 'addtocart',
    'Add to Wishlist': 'addtowishlist',
    'Compare': 'compare',
    'Quick View': 'quickview',
    'Out of Stock': 'outofstock',
    'In Stock': 'instock',
    'Low Stock': 'lowstock',
    'Sold Out': 'soldout',
    'On Sale': 'onsale',
    'New': 'new',
    'Used': 'used',
    'Refurbished': 'refurbished',
    'Digital': 'digital',
    'Physical': 'physical',
    'Downloadable': 'downloadable',
    'Subscription': 'subscription',
    'Gift Card': 'giftcard',
    'Coupon': 'coupon',
    'Promo Code': 'promocode',
    'Free Trial': 'freetrial',
    'Premium': 'premium',
    'Basic': 'basic',
    'Standard': 'standard',
    'Pro': 'pro',
    'Enterprise': 'enterprise',
    'Starter': 'starter',
    'Advanced': 'advanced',
    'Professional': 'professional',
    'Business': 'business',
    'Personal': 'personal',
    'Family': 'family',
    'Student': 'student',
    'Senior': 'senior',
    'Military': 'military',
    'Corporate': 'corporate',
    'Non-Profit': 'nonprofit',
    'Government': 'government',
    'Education': 'education',
    'Healthcare': 'healthcare',
    'Retail': 'retail',
    'Wholesale': 'wholesale',
    'Manufacturer': 'manufacturer',
    'Distributor': 'distributor',
    'Supplier': 'supplier',
    'Vendor': 'vendor',
    'Partner': 'partner',
    'Affiliate': 'affiliate',
    'Reseller': 'reseller',
    'Dealer': 'dealer',
    'Agent': 'agent',
    'Representative': 'representative',
    'Consultant': 'consultant',
    'Advisor': 'advisor',
    'Expert': 'expert',
    'Specialist': 'specialist',
    'Professional': 'professional',
    'Certified': 'certified',
    'Licensed': 'licensed',
    'Authorized': 'authorized',
    'Official': 'official',
    'Genuine': 'genuine',
    'Original': 'original',
    'Authentic': 'authentic',
    'Brand New': 'brandnew',
    'Like New': 'likenew',
    'Very Good': 'verygood',
    'Good': 'good',
    'Acceptable': 'acceptable',
    'Poor': 'poor',
    'Damaged': 'damaged',
    'Broken': 'broken',
    'Defective': 'defective',
    'Faulty': 'faulty',
    'Malfunctioning': 'malfunctioning',
    'Not Working': 'notworking',
    'For Parts': 'forparts',
    'Spare Parts': 'spareparts',
    'Replacement Parts': 'replacementparts',
    'Accessories': 'accessories',
    'Add-Ons': 'addons',
    'Extensions': 'extensions',
    'Upgrades': 'upgrades',
    'Updates': 'updates',
    'Patches': 'patches',
    'Fixes': 'fixes',
    'Improvements': 'improvements',
    'Enhancements': 'enhancements',
    'Features': 'features',
    'Benefits': 'benefits',
    'Advantages': 'advantages',
    'Pros': 'pros',
    'Cons': 'cons',
    'Reviews': 'reviews',
    'Ratings': 'ratings',
    'Feedback': 'feedback',
    'Comments': 'comments',
    'Testimonials': 'testimonials',
    'Recommendations': 'recommendations',
    'Suggestions': 'suggestions',
    'Tips': 'tips',
    'Tricks': 'tricks',
    'Hacks': 'hacks',
    'Shortcuts': 'shortcuts',
    'Guides': 'guides',
    'Tutorials': 'tutorials',
    'Instructions': 'instructions',
    'Manuals': 'manuals',
    'Documentation': 'documentation',
    'Help': 'help',
    'Support': 'support',
    'FAQ': 'faq',
    'Q&A': 'qanda',
    'Questions': 'questions',
    'Answers': 'answers',
    'Solutions': 'solutions',
    'Troubleshooting': 'troubleshooting',
    'Problems': 'problems',
    'Issues': 'issues',
    'Bugs': 'bugs',
    'Errors': 'errors',
    'Warnings': 'warnings',
    'Alerts': 'alerts',
    'Notifications': 'notifications',
    'Messages': 'messages',
    'News': 'news',
    'Updates': 'updates',
    'Announcements': 'announcements',
    'Events': 'events',
    'Promotions': 'promotions',
    'Deals': 'deals',
    'Offers': 'offers',
    'Specials': 'specials',
    'Clearance': 'clearance',
    'Closeout': 'closeout',
    'Liquidation': 'liquidation',
    'Bankruptcy': 'bankruptcy',
    'Going Out of Business': 'goingoutofbusiness',
    'Everything Must Go': 'everythingmustgo',
    'Final Sale': 'finalsale',
    'No Returns': 'noreturns',
    'All Sales Final': 'allsalesfinal',
    'As Is': 'asis',
    'No Warranty': 'nowarranty',
    'Limited Warranty': 'limitedwarranty',
    'Extended Warranty': 'extendedwarranty',
    'Lifetime Warranty': 'lifetimewarranty',
    'Money Back Guarantee': 'moneybackguarantee',
    'Satisfaction Guaranteed': 'satisfactionguaranteed',
    'Risk Free': 'riskfree',
    'No Risk': 'norisk',
    'Try Before You Buy': 'trybeforeyoubuy',
    'Free Returns': 'freereturns',
    'Easy Returns': 'easyreturns',
    'Hassle Free Returns': 'hasslereturns'
};

// Normalize text for better matching
function normalizeText(text) {
    if (!text) return '';
    
    return text
        .replace(/\s+/g, ' ')           // Replace multiple spaces with single space
        .replace(/['']/g, "'")          // Normalize apostrophes
        .replace(/[""]/g, '"')          // Normalize quotes
        .replace(/[–—]/g, '-')          // Normalize dashes
        .trim();                        // Remove leading/trailing spaces
}

// Enhanced pattern matching for product variations
function findBestProductMatch(productText) {
    if (!productText || productText.length < 3) return null;
    
    var normalizedProduct = productText.toLowerCase();
    var bestMatch = null;
    var bestScore = 0;
    
    // First try exact matches
    if (databaseContentMap[productText]) {
        return { key: databaseContentMap[productText], type: 'exact' };
    }
    
    // Try case-insensitive exact matches
    for (var pattern in databaseContentMap) {
        if (pattern.toLowerCase() === normalizedProduct) {
            return { key: databaseContentMap[pattern], type: 'case-insensitive' };
        }
    }
    
    // Try partial matches - prioritize longer patterns
    for (var pattern in databaseContentMap) {
        var normalizedPattern = pattern.toLowerCase();
        
        // Skip very short patterns for partial matching
        if (normalizedPattern.length < 5) continue;
        
        if (normalizedProduct.includes(normalizedPattern)) {
            // Score based on pattern length and position
            var score = normalizedPattern.length;
            if (normalizedProduct.startsWith(normalizedPattern)) {
                score += 10; // Bonus for starting match
            }
            if (score > bestScore) {
                bestScore = score;
                bestMatch = { key: databaseContentMap[pattern], type: 'partial', pattern: pattern };
            }
        }
    }
    
    return bestMatch;
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
    
    // 6. Start dynamic content observer (only once)
    if (!window.dynamicContentObserverStarted) {
        translateDynamicContent();
        window.dynamicContentObserverStarted = true;
    }
    
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
    
    // Track translations for summary
    var translatedCount = 0;
    var totalCount = 0;
    var unmappedCategories = [];
    
    // Target category links specifically
    $('.first-level-a, .second-level-a, .third-level-a').each(function() {
        var $this = $(this);
        var categoryText = normalizeText($this.text());
        totalCount++;
        
        if (!categoryText || categoryText.length < 2) {
            return; // Skip very short or empty text
        }
        
        if (databaseContentMap[categoryText]) {
            var translationKey = databaseContentMap[categoryText];
            var translation = getTranslation(translationKey, currentLanguage);
            if (translation !== translationKey) {
                // Get the icon element if it exists
                var $icon = $this.find('.first-level-icon, .second-level-icon, .third-level-icon');
                
                if ($icon.length > 0) {
                    // Keep icon, replace text
                    $this.html('').append($icon).append(' ' + translation);
                } else {
                    // No icon, just replace text
                    $this.text(translation);
                }
                translatedCount++;
            }
        } else {
            // Only log unmapped categories if they're meaningful
            if (categoryText.length > 3 && !categoryText.match(/^[0-9\s\-_]+$/)) {
                unmappedCategories.push(categoryText);
            }
        }
    });
    
    // Summary logging
    console.log(`✅ Category translation complete: ${translatedCount}/${totalCount} translated`);
    
    // Show unmapped categories (they're usually fewer than products)
    if (unmappedCategories.length > 0) {
        console.log(`❌ ${unmappedCategories.length} unmapped categories:`, unmappedCategories);
    }
}

function translateProductTitles() {
    console.log('🔄 Translating product titles...');
    
    // Track translations for summary
    var translatedCount = 0;
    var totalCount = 0;
    var unmappedProducts = [];
    
    // Translate product titles in various containers
    $('.p-title').each(function() {
        var $this = $(this);
        var productText = normalizeText($this.text());
        totalCount++;
        
        if (!productText || productText.length < 3) {
            return; // Skip very short or empty text
        }
        
        // Use enhanced pattern matching
        var match = findBestProductMatch(productText);
        
        if (match) {
            var translation = getTranslation(match.key, currentLanguage);
            if (translation !== match.key) {
                $this.text(translation);
                translatedCount++;
            }
        } else {
            // Only log unmapped products if they're not too generic
            if (productText.length > 10 && !productText.match(/^[0-9\s\-_]+$/)) {
                unmappedProducts.push(productText);
            }
        }
    });
    
    // Summary logging
    console.log(`✅ Product translation complete: ${translatedCount}/${totalCount} translated`);
    
    // Show sample of unmapped products (max 10)
    if (unmappedProducts.length > 0) {
        console.log(`❌ ${unmappedProducts.length} unmapped products. Sample:`, unmappedProducts.slice(0, 10));
    }
}

// Function to handle dynamically loaded content
function translateDynamicContent() {
    console.log('🔄 Translating dynamically loaded content...');
    
    // Use MutationObserver to watch for new content
    var observer = new MutationObserver(function(mutations) {
        var shouldTranslate = false;
        
        mutations.forEach(function(mutation) {
            if (mutation.type === 'childList' && mutation.addedNodes.length > 0) {
                // Check if any new nodes contain translatable content
                for (var i = 0; i < mutation.addedNodes.length; i++) {
                    var node = mutation.addedNodes[i];
                    if (node.nodeType === 1) { // Element node
                        var $node = $(node);
                        if ($node.find('.p-title, .first-level-a, .second-level-a, .third-level-a').length > 0 ||
                            $node.hasClass('p-title') || 
                            $node.hasClass('first-level-a') || 
                            $node.hasClass('second-level-a') || 
                            $node.hasClass('third-level-a')) {
                            shouldTranslate = true;
                            break;
                        }
                    }
                }
            }
        });
        
        if (shouldTranslate) {
            console.log('🔄 New translatable content detected, translating...');
            // Delay translation slightly to ensure content is fully rendered
            setTimeout(function() {
                translateCategories();
                translateProductTitles();
            }, 100);
        }
    });
    
    // Start observing
    observer.observe(document.body, {
        childList: true,
        subtree: true
    });
    
    console.log('👁️ Dynamic content observer started');
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

// Force refresh all translations (useful for debugging or after content updates)
function forceTranslationRefresh() {
    console.log('🔄 Force refreshing all translations...');
    
    if (!translations) {
        console.log('📥 Loading translations first...');
        loadTranslations(function() {
            translateAllContent();
        });
    } else {
        translateAllContent();
    }
}

// Comprehensive translation testing function
function testAllTranslations() {
    console.log('🧪 Starting comprehensive translation test...');
    
    var languages = ['en', 'tr', 'de', 'fr'];
    var testResults = {
        categories: {},
        products: {},
        coverage: {}
    };
    
    // Test each language
    languages.forEach(function(lang) {
        console.log(`\n🌍 Testing ${lang.toUpperCase()} translations...`);
        
        var originalLang = currentLanguage;
        currentLanguage = lang;
        
        // Test categories
        var categoryCount = 0;
        var categoryTranslated = 0;
        
        $('.first-level-a, .second-level-a, .third-level-a').each(function() {
            var categoryText = normalizeText($(this).text());
            if (categoryText && categoryText.length > 2) {
                categoryCount++;
                if (databaseContentMap[categoryText]) {
                    var translationKey = databaseContentMap[categoryText];
                    var translation = getTranslation(translationKey, lang);
                    if (translation !== translationKey) {
                        categoryTranslated++;
                    }
                }
            }
        });
        
        // Test products
        var productCount = 0;
        var productTranslated = 0;
        
        $('.p-title').each(function() {
            var productText = normalizeText($(this).text());
            if (productText && productText.length > 3) {
                productCount++;
                var match = findBestProductMatch(productText);
                if (match) {
                    var translation = getTranslation(match.key, lang);
                    if (translation !== match.key) {
                        productTranslated++;
                    }
                }
            }
        });
        
        testResults.categories[lang] = {
            total: categoryCount,
            translated: categoryTranslated,
            coverage: categoryCount > 0 ? Math.round((categoryTranslated / categoryCount) * 100) : 0
        };
        
        testResults.products[lang] = {
            total: productCount,
            translated: productTranslated,
            coverage: productCount > 0 ? Math.round((productTranslated / productCount) * 100) : 0
        };
        
        console.log(`📊 ${lang.toUpperCase()} Results:`);
        console.log(`   Categories: ${categoryTranslated}/${categoryCount} (${testResults.categories[lang].coverage}%)`);
        console.log(`   Products: ${productTranslated}/${productCount} (${testResults.products[lang].coverage}%)`);
        
        // Restore original language
        currentLanguage = originalLang;
    });
    
    // Overall summary
    console.log('\n📈 TRANSLATION COVERAGE SUMMARY:');
    console.log('=====================================');
    
    languages.forEach(function(lang) {
        console.log(`${lang.toUpperCase()}: Categories ${testResults.categories[lang].coverage}%, Products ${testResults.products[lang].coverage}%`);
    });
    
    // Calculate averages
    var avgCategoryContent = languages.reduce((sum, lang) => sum + testResults.categories[lang].coverage, 0) / languages.length;
    var avgProductCoverage = languages.reduce((sum, lang) => sum + testResults.products[lang].coverage, 0) / languages.length;
    
    console.log(`\n🎯 Average Coverage: Categories ${Math.round(avgCategoryContent)}%, Products ${Math.round(avgProductCoverage)}%`);
    
    return testResults;
}

// Quick language switch test
function testLanguageSwitching() {
    console.log('🔄 Testing language switching...');
    
    var languages = ['en', 'tr', 'de', 'fr'];
    var originalLang = currentLanguage;
    
    languages.forEach(function(lang, index) {
        setTimeout(function() {
            console.log(`🌍 Switching to ${lang.toUpperCase()}...`);
            setLanguage(lang);
            
            if (index === languages.length - 1) {
                setTimeout(function() {
                    console.log('✅ Language switching test completed');
                    setLanguage(originalLang); // Restore original
                }, 1000);
            }
        }, index * 2000);
    });
}

// Enhanced event handlers for various content loading scenarios
$(document).ready(function() {
    // Handle AJAX content loading
    $(document).ajaxComplete(function(event, xhr, settings) {
        // Only translate if the request was successful and might contain translatable content
        if (xhr.status === 200 && settings.url) {
            console.log('🌐 AJAX request completed:', settings.url);
            setTimeout(function() {
                translateCategories();
                translateProductTitles();
            }, 200);
        }
    });
    
    // Handle common dynamic content triggers
    $(document).on('click', '.load-more, .show-more, .pagination a, .category-link', function() {
        console.log('🔄 Content loading trigger detected');
        setTimeout(function() {
            translateCategories();
            translateProductTitles();
        }, 500);
    });
    
    // Handle tab switches and modal opens
    $(document).on('shown.bs.tab shown.bs.modal', function() {
        console.log('🔄 Tab/Modal content shown');
        setTimeout(function() {
            translateCategories();
            translateProductTitles();
        }, 100);
    });
});

// Initialize language on page load
document.addEventListener("DOMContentLoaded", function () {
    initializeLanguage();
    updateTranslations();
    
    // Make debugging functions available globally
    window.forceTranslationRefresh = forceTranslationRefresh;
    window.testAllTranslations = testAllTranslations;
    window.testLanguageSwitching = testLanguageSwitching;
    window.findBestProductMatch = findBestProductMatch;
    
    console.log('🌍 Language system initialized.');
    console.log('🧪 Debug functions available:');
    console.log('   - forceTranslationRefresh() - Manually refresh all translations');
    console.log('   - testAllTranslations() - Test translation coverage for all languages');
    console.log('   - testLanguageSwitching() - Test automatic language switching');
    console.log('   - findBestProductMatch("text") - Test product matching for specific text');
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