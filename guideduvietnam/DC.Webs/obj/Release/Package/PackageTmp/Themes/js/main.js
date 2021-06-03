$(document).ready(function () {
    $('.sliderWrap').owlCarousel({
        items: 1,
        lazyLoad: true,
        loop: true,
        autoplay: true,
        margin: 0,
        responsiveClass: true,
        paginationSpeed: 800,
        nav: true,
        navText: ['‹', '›']        
    });

    $('.sliderTourWrap').owlCarousel({
        items: 1,
        lazyLoad: true,
        loop: true,
        autoplay: true,
        margin: 0,
        responsiveClass: true,
        paginationSpeed: 800,
        nav: true,
        navText: ['‹', '›']
    });


    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.tempFixed .itemFixed').addClass('trans');
            $('.headerTemp').addClass('headerFixed');
        } else {
            $('.tempFixed .itemFixed').removeClass('trans');
            $('.headerTemp').removeClass('headerFixed');
        }

        if ($(this).scrollTop() > 500) {
            $('.nav-tabs').addClass('tab-fixed');
        } else {
            $('.nav-tabs').removeClass('tab-fixed');
        }
    });

    $('.backTop a').click(function () {
        $('html, body').animate({
            scrollTop: 0
        }, 600);
        return false;
    });
    $('.tempFixed .itemFixed.hotLine label i').click(function () {
        $(this).parents('.hotLine').fadeOut(500);
    });

    $('.tawkarrow').click(function() {
        $('.tawkcontent').toggle();
        var classObj = document.getElementById("tawkarrow").className;
        if (classObj === 'fa fa-angle-up') {
            $('#tawkarrow').removeClass('fa-angle-up');
            $('#tawkarrow').addClass('fa-angle-down');
            setCookie('ShowHideSupport', 'show', 1);
        } else {
            $('#tawkarrow').removeClass('fa-angle-down');
            $('#tawkarrow').addClass('fa-angle-up');
            setCookie('ShowHideSupport', 'hide', 1);
        }
    });

    ShowHideSupport();
});

$(document).on('click', '.btnMBToggleNav, .closeMenuMB, .overlayMenu', function () {
    $('body').toggleClass('openNav');
});
$(document).on('click', '.navSiteMain ul.nav-navbar li a i', function (e) {
    e.preventDefault();
    $(this).parent().toggleClass('open').next().slideToggle();
});


function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function checkCookie() {
    var user = getCookie("username");
    if (user != "") {
        alert("Welcome again " + user);
    } else {
        user = prompt("Please enter your name:", "");
        if (user != "" && user != null) {
            setCookie("username", user, 30);
        }
    }
}

function ShowHideSupport() {
    var isShow = getCookie("ShowHideSupport");
    if (isShow === 'show') {
        $('.tawkcontent').show();
        $('#tawkarrow').removeClass('fa-angle-up');
        $('#tawkarrow').addClass('fa-angle-down');
    }
    if (isShow === 'hide') {
        $('.tawkcontent').hide();
        $('#tawkarrow').removeClass('fa-angle-down');
        $('#tawkarrow').addClass('fa-angle-up');        
    }
}