$(document).ready(function(){
    $(window).bind('scroll', function() {
        var navHeight = $(".menu").height();
        ($(window).scrollTop() > navHeight) ? $('nav').addClass('nav-fixed-top') : $('nav').removeClass('nav-fixed-top');
    });
});
