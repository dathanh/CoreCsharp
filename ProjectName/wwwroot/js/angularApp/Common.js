function EnableCreateFooterButton(isEnable) {
    if (isEnable) {
        $('a[data-function="FOOTER_ACTION_SAVE"]').removeAttr('disabled');
        $('a[data-function="FOOTER_ACTION_SAVE"]').removeClass('k-state-disabled');
    } else {
        $("form").removeClass("dirty");
        $('a[data-function="FOOTER_ACTION_SAVE"]').attr('disabled', 'disabled');
        $('a[data-function="FOOTER_ACTION_SAVE"]').addClass('k-state-disabled');
    }
}

function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
    return "";
}


function BindDirtyForm() {
    EnableCreateFooterButton(false);
    $('form').attr('autocomplete', 'off');
    // Setup dirty dialog handling across all forms
    $('form').areYouSure({
        message: '@Html.Raw(Model.DirtyDialogMessageText)',
        change: function () {
            var formElement = $(this);

            if (formElement[0] === undefined) {
                formElement = $(document).find('form');
            }

            if (formElement.hasClass('dirty')) {
                EnableCreateFooterButton(true);
            } else {
                EnableCreateFooterButton(false);
            }
        }
    });
}
function formatHtmlBodyMain() {
    if (!detectmob()) {
        //$("html,body").css({ "overflow": "hidden" });
        $("#main").css({ "padding-bottom": "0" });
    } else {
        $("html,body").css({ "overflow": "auto" });
    }

}
function detectmob() {
    if (navigator.userAgent.match(/Android/i)
    || navigator.userAgent.match(/webOS/i)
    || navigator.userAgent.match(/iPhone/i)
    || navigator.userAgent.match(/iPad/i)
    || navigator.userAgent.match(/iPod/i)
    || navigator.userAgent.match(/BlackBerry/i)
    || navigator.userAgent.match(/Windows Phone/i)
    ) {
        return true;
    }
    else {
        return false;
    }
}
