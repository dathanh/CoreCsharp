$(document).ready(function () {
    //pageSetUp();
    //// summernote
    //$('.summernote').summernote({
    //    height: 150,
    //    focus: false,
    //    tabsize: 2
    //});
    //FormatContent();
    //FormatNav(false);
    //$(".minifyme").click(function () {
    //    FormatNav(true);
    //});
    //FormatTabReferral();
    //$(".scroll").mCustomScrollbar({
    //    theme: "light-3"
    //});
    setTimezoneCookie();
    detectBrowser();

});

function setTimezoneCookie() {

    var timezoneCookie = "timezoneoffset";

    // if the timezone cookie not exists create one.
    if (!$.cookie(timezoneCookie)) {

        // check if the browser supports cookie
        var testCookie = 'test cookie';
        $.cookie(testCookie, true);

        // browser supports cookie
        if ($.cookie(testCookie)) {

            // delete the test cookie
            $.cookie(testCookie, null);

            // create a new cookie 
            $.cookie(timezoneCookie, new Date().getTimezoneOffset());

            // re-load the page
            location.reload();
        }
    }
        // if the current timezone and the one stored in cookie are different
        // then store the new timezone in the cookie and refresh the page.
    else {

        var storedOffset = parseInt($.cookie(timezoneCookie));
        var currentOffset = new Date().getTimezoneOffset();

        // user may have changed the timezone
        if (storedOffset !== currentOffset) {
            $.cookie(timezoneCookie, new Date().getTimezoneOffset());
            location.reload();
        }
    }
}

function FormatContent() {
    $("#widget-grid").css({ height: $(window).height() - 143 });
}

function FormatNav(type) {
    $("#nav").css({ "max-height": $(window).height() - 141 });
    if ($("body").hasClass("minified") == type) {
        $("#nav").addClass("format-nav");
        $("#nav").mCustomScrollbar({
            theme: "light-3"
        });
    } else {
        $("#nav").removeClass("format-nav");
        $("#nav").mCustomScrollbar("destroy");
    }
}

function FormatTabReferral() {
    $(".nav.nav-tabs.tabs-left").css({ "height": $(window).height() - 209 });
    $(".tab-pane").css({ "height": $(window).height() - 215, "overflow-x": "hidden", "overflow-y": "auto" });
    $("#widget-grid").css({ "overflow": "hidden" });
}
$('#myModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget); // Button that triggered the modal
    var title = button.data('whatever'); // Extract info from data-* attributes
    var src = button.data('src'); // Extract info from data-* attributes
    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
    var modal = $(this);

    if (src != null && src != '') {
        modal.find('.modal-title').text(title);
        $.ajax({
            url: src,
            cache: false
        })
            .done(function (html) {
                $('#myModalMain').html(html).promise().done(function () {
                    $("[data-mask]").each(function () {
                        var a = $(this),
                            b = a.attr("data-mask") || "error...",
                            c = a.attr("data-mask-placeholder") || "X";
                        a.mask(b, { "placeholder": c }),
                        a = null;
                    });
                    $('.summernote').summernote({
                        height: 200,
                        focus: false,
                        tabsize: 2
                    });
                    modal.find('fieldset').css({ "max-height": modal.height() - 200, "overflow-y": "auto", "overflow-x": "hidden" });
                    modal.find('fieldset').addClass("scroll");
                    $(".scroll").mCustomScrollbar({
                        theme: "light-3"
                    });

                    $('#check-all').change(function () {
                        var checkboxes = $(this).closest('form').find(':checkbox');
                        if ($(this).is(':checked')) {
                            checkboxes.prop('checked', true);
                        } else {
                            checkboxes.prop('checked', false);
                        }
                    });
                });

            });
    }
});
$('#myModalChild').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget); // Button that triggered the modal
    var title = button.data('whatever'); // Extract info from data-* attributes
    var src = button.data('src'); // Extract info from data-* attributes
    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
    var modal = $(this);
    if (src != null && src != '') {
        modal.find('.modal-title').text(title);
        $.ajax({
            url: src,
            cache: false
        })
            .done(function (html) {
                $('#myModalChildMain').html(html).promise().done(function () {
                    $("[data-mask]").each(function () {
                        var a = $(this),
                            b = a.attr("data-mask") || "error...",
                            c = a.attr("data-mask-placeholder") || "X";
                        a.mask(b, { "placeholder": c }),
                        a = null;
                    });
                    $('.summernote').summernote({
                        height: 200,
                        focus: false,
                        tabsize: 2
                    });
                    modal.find('fieldset').css({ "max-height": modal.height() - 200, "overflow-y": "auto", "overflow-x": "hidden" });
                    modal.find('fieldset').addClass("scroll");
                    $(".scroll").mCustomScrollbar({
                        theme: "light-3"
                    });

                    $('#check-all').change(function () {
                        var checkboxes = $(this).closest('form').find(':checkbox');
                        if ($(this).is(':checked')) {
                            checkboxes.prop('checked', true);
                        } else {
                            checkboxes.prop('checked', false);
                        }
                    });
                });

            });
    }
});

$('#check-all').change(function () {
    var checkboxes = $(this).closest('form').find(':checkbox');
    if ($(this).is(':checked')) {
        checkboxes.prop('checked', true);
    } else {
        checkboxes.prop('checked', false);
    }
});
function searchGeneral(id) {
    //window.alert(id);
    if (id <= 5) {
        document.getElementById('generalSearch').style.display = 'none';
    } else {
        document.getElementById('generalSearch').style.display = 'block';
    }

}

function patientSearch() {
    document.getElementById('patient').style.display = 'block';
}

function recentReferral(id) {
    if (id == 0) {
        document.getElementById('referralResult24').style.display = 'none';
        document.getElementById('referralResult30').style.display = 'none';
        document.getElementById('referralResult60').style.display = 'none';
    };
    if (id == 1) {
        document.getElementById('referralResult24').style.display = 'block';
        document.getElementById('referralResult30').style.display = 'none';
        document.getElementById('referralResult60').style.display = 'none';
    };
    if (id == 2) {
        document.getElementById('referralResult24').style.display = 'none';
        document.getElementById('referralResult30').style.display = 'block';
        document.getElementById('referralResult60').style.display = 'none';
    };
    if (id == 3) {
        document.getElementById('referralResult24').style.display = 'none';
        document.getElementById('referralResult30').style.display = 'none';
        document.getElementById('referralResult60').style.display = 'block';
    };
}

function detectBrowser() {
    var userAgent = navigator.userAgent.toLowerCase(),
    browser = '',
    version = 0;
    //$.browser.chrome = /chrome/.test(navigator.userAgent.toLowerCase());

    // Is this a version of IE?
    //if (jQuery.browser.msie) {
    //    userAgent = $.browser.version;
    //    userAgent = userAgent.substring(0, userAgent.indexOf('.'));
    //    version = userAgent;
    //    browser = "Internet Explorer";

    //}

    //$('head').append('<link rel="stylesheet" href="~/content/libris/css/chrome.css" type="text/css" />');
    // Is this a version of Chrome?
    if (userAgent.indexOf('chrome/') >= 0) {
        //userAgent = userAgent.substring(userAgent.indexOf('chrome/') + 7);
        //userAgent = userAgent.substring(0, userAgent.indexOf('.'));
        //version = userAgent;
        //// If it is chrome then jQuery thinks it's safari so we have to tell it it isn't
        //$.browser.safari = false;
        //browser = "Chrome";
        $('head').append('<link rel="stylesheet" href="/content/libris/css/chrome.css" type="text/css" />');
    }

    // Is this a version of Safari?
    //if ($.browser.safari) {
    //    //userAgent = userAgent.substring(userAgent.indexOf('safari/') + 7);
    //    //userAgent = userAgent.substring(0, userAgent.indexOf('.'));
    //    //version = userAgent;
    //    //browser = "Safari";
    //    $('head').append('<link rel="stylesheet" href="~/content/libris/css/safari.css" type="text/css" />');
    //}

    // Is this a version of Mozilla?
    //if ($.browser.mozilla) {
    //    //Is it Firefox?
    //    if (navigator.userAgent.toLowerCase().indexOf('firefox') != -1) {
    //        userAgent = userAgent.substring(userAgent.indexOf('firefox/') + 8);
    //        userAgent = userAgent.substring(0, userAgent.indexOf('.'));
    //        version = userAgent;
    //        browser = "Firefox"
    //    }
    //        // If not then it must be another Mozilla
    //    else {
    //        browser = "Mozilla (not Firefox)"
    //    }
    //}

    // Is this a version of Opera?
    //if ($.browser.opera) {
    //    userAgent = userAgent.substring(userAgent.indexOf('version/') + 8);
    //    userAgent = userAgent.substring(0, userAgent.indexOf('.'));
    //    version = userAgent;
    //    browser = "Opera";
    //}

    // Now you have two variables, browser and version
    // which have the right info
}

