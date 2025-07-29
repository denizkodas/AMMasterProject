
/*---------------------main menu open and close---------------------------*/

const accordionItem = document.querySelectorAll(".menu-item");

accordionItem.forEach((el) =>
    el.addEventListener("click", () => {
        if (el.classList.contains("open")) {
            el.classList.remove("open");

            

        }
    
        else {
            accordionItem.forEach((el2) => el2.classList.remove("open"));
            el.classList.add("open");
        }


    })
);


/*---------------------main menu Active class on url---------------------------*/
$(function () {
    var currentPath = location.pathname;
    $('.menu-inner a').each(function () {
        var href = $(this).attr('href');
        if (href === currentPath) {
            $(this).addClass('active');
        } else {
            $(this).removeClass('active');
        }
    });
});


/*---------------------listing tab Active class on url---------------------------*/
$(function () {
    var currentPath = location.pathname;
    $('.l-tabs a').each(function () {
        var href = $(this).attr('href');
        if (href === currentPath) {
            $(this).addClass('active');
        } else {
            $(this).removeClass('active');
        }
    });
});

/*---------------------Profile tab Active class on url---------------------------*/
$(function () {
    var currentPath = location.pathname;
    $('.nav-pills a').each(function () {
        var href = $(this).attr('href');
        if (href === currentPath) {
            $(this).addClass('active');
        } else {
            $(this).removeClass('active');
        }
    });
});


/*---------------------mobile menu---------------------------*/

    function openbar() {
                var element = document.getElementById("mybody");
        element.classList.add("layout-menu-expanded");
            }
    function closebar() {
                var element = document.getElementById("mybody");
        element.classList.remove("layout-menu-expanded");
            }



/*----------------------Charts--------------------------*/
