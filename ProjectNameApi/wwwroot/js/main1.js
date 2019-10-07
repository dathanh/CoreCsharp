

var curPos,
    popPos,
    popActive = false,
    myClock = null;

var ua = navigator.userAgent;
var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(ua);


var IEMobile = ua.match(/IEMobile/i);
var isIE9 = /MSIE 9/i.test(ua);
var isIE10 = /MSIE 10/i.test(ua);
var isIE11 = /rv:11.0/i.test(ua) && !IEMobile ? true : false;

if (isIE9 || isIE10 || isIE11) {
    $('body').addClass('isIE');
}

function inputHolder() {
    $('.fs_error_txt').click(function () {
        $(this).parent().removeClass('fs-show-error');
        $(this).parent().find('input').focus();
    });
    $('input[type="text"]').focus(function (e) {
        $(this).parent().removeClass('fs-show-error');
    });

}


var threshold = 300;
function ImgLazyLoad() {

    var winH = $(window).height();

    lazyImages = window.innerWidth > 1100 ? document.querySelectorAll('.cmPic.fs-lazy, .pcPic.fs-lazy, .active .cmPic.fs-lazyExtra, .active .pcPic.fs-lazyExtra') : document.querySelectorAll('.cmPic.fs-lazy, .spPic.fs-lazy, .active .cmPic.fs-lazyExtra, .active .spPic.fs-lazyExtra');
    [].slice.call(lazyImages).forEach(function (elm) {
        if (elm.getBoundingClientRect().top <= winH + threshold) {
            var src = elm.getAttribute('data-src');
            elm.setAttribute('src', src);
            elm.classList.remove('fs-lazy');
        }
    });

    lazyBgs = window.innerWidth > 1100 ? document.querySelectorAll('.cmBg.fs-lazy, .pcBg.fs-lazy, .active .pcBg.fs-lazyExtra, .active .cmBg.fs-lazyExtra') : document.querySelectorAll('.cmBg.fs-lazy, .spBg.fs-lazy, .active .cmBg.fs-lazyExtra, .active .spBg.fs-lazyExtra');
    [].slice.call(lazyBgs).forEach(function (elm) {
        if (elm.getBoundingClientRect().top <= winH + threshold) {
            var src = elm.getAttribute('data-src');
            elm.style.backgroundImage = 'url(' + src + ')';
            elm.classList.remove('fs-lazy');
            elm.classList.remove('fs-lazyExtra');
        }
    });

}

//SET LIMITED TIME FOR BOOT TICKET
function setClock(value) {

    var timeClock = value;

    myClock = $('.clock').FlipClock(timeClock, {
        clockFace: 'DailyCounter',
        countdown: true,
        autoStart: false,
        callbacks: {
            start: function () {
                $('.message').html('The clock has started!');
            }
        }
    });
}

function fsEvent() {

    //Select list
    $('.fs-select-header').click(function (e) {

        var box = $(this).parent();
        if (box.hasClass('open')) {
            box.removeClass('open');
        } else {
            $('.fs-select-list').removeClass('open');
            box.addClass('open');
        }

    });

    // $('.fs-select-box li').click(function () {
    // var that = $(this);
    // var box = $(this).parent().parent().parent();

    // if (!that.hasClass('selected')) {
    // box.find('li').removeClass('selected');
    // that.addClass('selected');
    // box.removeClass('open');

    // target = that.data('value');
    // if (box.hasClass('fsSelectPromotion')) {
    // $('.is-single img').removeClass('active');
    // $('.is-single img[data-target=' + target + ']').addClass('active');
    // } else {
    // box.find('.fs-select-header h3').html(that.text());
    // }
    // }

    // });

    $(document).on('click touchstart', function (event) {
        if ($(".fs-select-list").has(event.target).length == 0 && !$(".fs-select-list").is(event.target)) {
            $(".fs-select-list").removeClass("open");
        }
    });
    //End

    //Close poppup
    $('.overlay-form, .close-overlay').click(function (e) {
        e.preventDefault();
        popActive = false;
        $('.overlay-form').removeClass('active');
        $('body').removeClass('no-scroll');
        $('html,body').scrollTop(popPos);
    });

    // if ($('.clock').length) {
    // setClock();
    // myClock.start();
    // }


    //Temp code show thank pop
    //$('#fs_hunter_form .fs-btn').click(function () {
    //    $('.fs-confirm').fadeIn(0);
    //    $('.is-form, .fs-payment').addClass('active');
    //    ImgLazyLoad();
    //    var delTop = 80;
    //    if (window.innerWidth <= 1100) {
    //        delTop = 118;
    //    }
    //    var top = $('.is-form').offset().top - delTop;
    //    $('html,body').animate({ scrollTop: top }, 500);
    //});
    //$('.close-payment, .btn-confirm-next').click(function () {
    //    $('.is-form, .fs-payment').removeClass('active');
    //    setTimeout(function () {
    //        $('.fs-payment-box').css({ 'display': 'none' });
    //    }, 300);
    //});

    //$('.btn-confirm').click(function () {
    //    $('.fs-confirm').fadeOut(300, function () {
    //        $('.fs-confirm').removeClass('fs-show');
    //        $('.fs-success').addClass('fs-show');
    //        $('.fs-success').fadeIn(0);
    //    });
    //});

    //Close overlay
    $('.fs-close-overlay').click(function () {
        $('.fs-overlay').removeClass('active');
        $('body').removeClass('fs-no-scroll');
    });

    //Nav click
    $('.fs-navigation li').click(function () {
        var target = $(this).attr('data-target');
        if ($('.' + target).length) {

            if (window.innerWidth > 1100) {
                if ($('.fs-overlay.active').length) {

                    setTimeout(function () {
                        $('body').removeClass('fs-no-scroll');
                        $('.fs-overlay').removeClass('active');
                        var top = $('.' + target).offset().top - 80;
                        $('html,body').animate({ scrollTop: top }, 500);
                    }, 150);

                } else {
                    var top = $('.' + target).offset().top - 80;
                    $('html,body').animate({ scrollTop: top }, 500);
                }
            } else {

                setTimeout(function () {
                    $('.fs-overlay, .fs-nav-but, .fs-navigation').removeClass('active');
                    $('body').removeClass('fs-no-scroll');
                    var top = $('.' + target).offset().top - 118;
                    $('html,body').animate({ scrollTop: top }, 500);
                }, 150);
            }

        }
    });


    //Radio hide
    $('.fs-form-radio .fs-radio-input').click(function () {
        var target = $(this).attr('data-target');
        var $fsCurBox = $('.fs-flow-radio.active');

        if (!$('.fs-flow-radio[data-target=' + target + ']').hasClass('active')) {
            $fsCurBox.fadeOut(300, function () {
                $fsCurBox.removeClass('active');
                $('.fs-flow-radio[data-target=' + target + ']').addClass('active');
                $('.fs-flow-radio[data-target=' + target + ']').fadeIn(0);
            });
        }

    });

    //Nav but
    $('.fs-nav-but').click(function () {
        if ($(this).hasClass('active')) {
            $('.fs-nav-but, .fs-navigation').removeClass('active');
            $('body').removeClass('fs-no-scroll');
        } else {
            $('.fs-nav-but, .fs-navigation').addClass('active');
            $('body').addClass('fs-no-scroll');
        }
    });

    //Goto form
    $('.fs-overlay-inr .fs-btn').click(function () {
        setTimeout(function () {
            $('.fs-overlay').removeClass('active');
            $('body').removeClass('fs-no-scroll');
            var delTop = 80;
            if (window.innerWidth <= 1100) {
                delTop = 118;
            }
            var top = $('.is-form').offset().top - delTop;
            $('html,body').animate({ scrollTop: top }, 500);
        }, 150);
    });

    $('.fs-header-inr .fs-btn, .is-supper .fs-btn, .is-timeline .fs-btn').click(function () {
        var delTop = 80;
        if (window.innerWidth <= 1100) {
            delTop = 118;
        }
        var top = $('.is-form').offset().top - delTop;
        $('html,body').animate({ scrollTop: top }, 500);
    });


    inputHolder();

}

function sfSlider() {

    if ($('.fs-supper-slider').length) {
        fsSlider = new Swiper('.fs-supper-slider', {
            autoPlay: false,
            loop: true,
            speed: 800,
            effect: 'fade',
            pagination: {
                el: '.is-supper .pagination',
                clickable: true
            },
            on: {
                init: function () {

                },
                transitionStart: function () {
                },
                transitionEnd: function () {

                }
            }
        });
    }

    if ($('.fs-timeline-slider').length) {
        fsSlider = new Swiper('.fs-timeline-slider', {
            simulateTouch: true,
            loop: true,
            speed: 500,
            navigation: {
                nextEl: '.is-timeline .fs-nav-next',
                prevEl: '.is-timeline .fs-nav-prev',
            },
            pagination: {
                el: '.is-timeline .pagination',
                clickable: true
            },
            on: {
                init: function () {

                },
                transitionStart: function () {
                },
                transitionEnd: function () {

                }
            }
        });
    }


}

function onScroll() {
    curPos = $(window).scrollTop();
    ImgLazyLoad();
}

function Rotate() {
    ImgLazyLoad();

    setTimeout(function () {
        $('.is-supper .fs-section-inr').css({ 'height': window.innerHeight - 118 });
    }, 100);
}

function Resize() {
    if (!isMobile) {
        ImgLazyLoad();
    }
}

$(window).on('scroll', onScroll);

$(window).on('resize', Resize);

$(window).on('load', function () {
    ImgLazyLoad();
    $('.fs-cnt-wrap').animate({ 'opacity': 1 }, 1000, function () {
        sfSlider();
        ImgLazyLoad();
    });
    fsEvent();
});

$(window).on("orientationchange", Rotate);

(function () {
    ImgLazyLoad();

    //$('body').addClass('fs-no-scroll');
    //$('.fs-overlay').addClass('active');

    if (window.innerWidth <= 1100) {
        $('.is-supper .fs-section-inr').css({ 'height': window.innerHeight - 118 });
    }

})();