// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $(".convert-emoji").each(function () {
        var original = $(this).html();
        // use .shortnameToImage if only converting shortnames (for slightly better performance)
        var converted = emojione.toImage(original);
        $(this).html(converted);
    });
});


var prevScrollPos = $(window).scrollTop();
// scroll functions
$(window).scroll(function(e) {

    var currentScrollPos = $(window).scrollTop();

    if (currentScrollPos >= 150 && currentScrollPos > prevScrollPos) {
        $('.navbar').addClass("navbar-hide");
    } else {
        $('.navbar').removeClass("navbar-hide");
    }

    prevScrollPos = currentScrollPos;
});
