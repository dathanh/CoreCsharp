'use strict';

define(['toastr', 'bootbox'], function (toastr, bootbox) {
    return {
        common: function ($q, $rootScope, $timeout, commonConfig, $localStorage) {
            var throttles = {};
            $localStorage.xmlDoc = null;
            var service = {
                // common angular dependencies
                $broadcast: $broadcast,
                $q: $q,
                $timeout: $timeout,
                // generic
                activateController: activateController,
                createSearchThrottle: createSearchThrottle,
                debouncedThrottle: debouncedThrottle,
                isNumber: isNumber,
                textContains: textContains,
                showErrorModelState: showErrorModelState,
                formSaveDataEvent: formSaveDataEvent,
                formCancelDataEvent: formCancelDataEvent,
                bootboxConfirm: bootboxConfirm,
                bootboxAlert: bootboxAlert,
                hideAllBootboxConfirm: hideAllBootboxConfirm,
                randomNumber: randomNumber,
                setTimezoneCookie: setTimezoneCookie,
                getWithPopupResponsive: getWithPopupResponsive,
                formatFormInPage: formatFormInPage,
                getLastDayOfWeek: getLastDayOfWeek,
                getMessageFromSystemMessage: getMessageFromSystemMessage,
                getFirstDayOfWeek: getFirstDayOfWeek,
                getFirstDateOfMonth: getFirstDateOfMonth,
                getLastDateOfMonth: getLastDateOfMonth,
                formatDateTime: formatDateTime,
                formatDateTimeWithFormatddMMyyyyHHmm: formatDateTimeWithFormatddMMyyyyHHmm,
                formatDate: formatDate,
                formatTime: formatTime,
                convertUtcToLocalTime: convertUtcToLocalTime,
                convertUtcToLocalDateString: convertUtcToLocalDateString,
                convertUtcToLocalDateTimeString: convertUtcToLocalDateTimeString,
                encode: encode,
                decode: decode,
                decodeHTMLEntities: decodeHTMLEntities,
                encodeHtmlEntities: encodeHtmlEntities,
                decodeObject: decodeObject,
                addDays: addDays,
                getDayOfWeek: getDayOfWeek,
                convertLocalDateToUtc: convertLocalDateToUtc,
                getActionMessageResult: getActionMessageResult,
                convertTotalSecondToHourFormat: convertTotalSecondToHourFormat,
                convertyyyyMMddTHHmmssfffZtoDateTime: convertyyyyMMddTHHmmssfffZtoDateTime,
                getStartTimeOfDay: getStartTimeOfDay,
                getEndTimeOfDay: getEndTimeOfDay,
                isValidDate: isValidDate,
                isValidTime: isValidTime,
                formatDateTimeMMddyyyyHHmmss: formatDateTimeMMddyyyyHHmmss,
                detectMobileDevice: detectMobileDevice,
                getLinkGooglePdfView: getLinkGooglePdfView,
                checkAndSetWidthPopup: checkAndSetWidthPopup,
                convertNumberToNameMonth: convertNumberToNameMonth,
                listFilterStatusTask: listFilterStatusTask,
                listFilterStatusTaskSelected: listFilterStatusTaskSelected,
                compareWithCurrentTime: compareWithCurrentTime,
                isValidDatetime: isValidDatetime,
                timeSince: timeSince,
                convertUtcToLocalDateTimeMMddyyyyHHmmssString: convertUtcToLocalDateTimeMMddyyyyHHmmssString,
                formatDateTimeMMddyyyyHHmmssHasSplit: formatDateTimeMMddyyyyHHmmssHasSplit,
                urlExists: urlExists,
                getVariableFromUrl: getVariableFromUrl,
                removeURLParameter: removeURLParameter,
                removeAllVariableFromUrl: removeAllVariableFromUrl,
                htmlEscape: htmlEscape,
                htmlUnescape: htmlUnescape,
                getDaysBetweenDay: getDaysBetweenDay,
                returnColorRequest: returnColorRequest,
                disabledForm: disabledForm,
                convertStringToDate: convertStringToDate
            };

            return service;

            function convertStringToDate(str) {
                var parts = str.split('/');
                return new Date(parts[2], parts[1] - 1, parts[0]);
            }

            function disabledForm(obj) {
                $timeout(function () {
                    $(obj).find('input[ng-model], input[k-ng-model], input[data-ng-model], textarea[ng-model], input[data-ng-model], input[ng-model]').each(function () {
                        $(this).attr('disabled', 'disabled');
                    });
                    $(obj).find('label.k-checkbox-label').each(function () {
                        $(this).addClass('k-checkbox-label-disabled');
                    });
                    $(obj).find('label.k-radio-label').each(function () {
                        $(this).addClass('k-radio-label-disabled');
                    });
                    $(obj).find('cis-dropdownlist').each(function () {
                        var id = $(this).attr('dropdown-id');
                        if (id && $('#' + id).data('kendoDropDownList')) {
                            $('#' + id).data('kendoDropDownList').enable(false);
                        }
                    });
                }, 500);
            }

            function urlExists(url, callback) {
                var xhr = new XMLHttpRequest();
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === 4) {
                        callback(xhr.status < 400);
                    }
                };
                xhr.open('HEAD', url);
                xhr.send();
            }
            function formatDateTimeMMddyyyyHHmmssHasSplit(datetime) {
                var date = parseInt(datetime.getDate());
                var month = parseInt((datetime.getMonth() + 1));
                var hours = parseInt(datetime.getHours());
                var minutes = parseInt(datetime.getMinutes());
                var second = parseInt(datetime.getSeconds());
                date = date < 10 ? '0' + date : date;
                month = month < 10 ? '0' + month : month;
                hours = hours < 10 ? '0' + hours : hours;
                minutes = minutes < 10 ? '0' + minutes : minutes;
                second = second < 10 ? '0' + second : second;

                return date + "/" + month + "/" + datetime.getFullYear() + " " + hours + ":" + minutes + ":" + second;
            }
            function convertUtcToLocalDateTimeMMddyyyyHHmmssString(serverdate) {
                var date = new Date();
                var offsetms = date.getTimezoneOffset() * 60 * 1000;

                var serverDate = new Date(serverdate);

                serverDate = new Date(serverDate.valueOf() - offsetms);

                var d = serverDate.getDate();
                var month = (serverDate.getMonth() + 1);
                var hours = serverDate.getHours();
                var minutes = serverDate.getMinutes();
                var second = serverDate.getSeconds();

                d = d < 10 ? '0' + d : d;
                month = month < 10 ? '0' + month : month;
                hours = hours < 10 ? '0' + hours : hours;
                hours = hours < 10 ? '0' + hours : hours;
                minutes = minutes < 10 ? '0' + minutes : minutes;
                second = second < 10 ? '0' + second : second;

                return d + "/" + month + "/" + serverDate.getFullYear() + " " + hours + ":" + minutes + ":" + second;
            }
            function timeSince(date) {

                var seconds = Math.floor((new Date() - date) / 1000);

                var interval = Math.floor(seconds / 31536000);

                if (interval >= 1) {
                    return interval + " years ago";
                }
                interval = Math.floor(seconds / 2592000);
                if (interval >= 1) {
                    return interval + " months ago";
                }
                interval = Math.floor(seconds / 86400);
                if (interval >= 1) {
                    return interval + " days ago";
                }
                interval = Math.floor(seconds / 3600);
                if (interval >= 1) {
                    return interval + " hours ago";
                }
                interval = Math.floor(seconds / 60);
                if (interval >= 1) {
                    return interval + " minutes ago";
                }
                return Math.floor(seconds) + " seconds ago";
            }

            function formSaveDataEvent(controllerId) {
                var data = { controllerId: controllerId };
                $broadcast(commonConfig.config.controllerFormSaveDataEvent, data);
            }

            function formCancelDataEvent(controllerId) {
                var data = { controllerId: controllerId };
                $broadcast(commonConfig.config.controllerFormCancelSaveDataEvent, data);
            }

            function activateController(promises, controllerId) {
                formatFormInPage();
                return $q.all(promises).then(function (eventArgs) {
                    var data = { controllerId: controllerId };
                    $broadcast(commonConfig.config.controllerActivateSuccessEvent, data);
                });
            }


            //// Opera 8.0+
            //var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
            //// Firefox 1.0+
            //var isFirefox = typeof InstallTrigger !== 'undefined';
            //// At least Safari 3+: "[object HTMLElementConstructor]"
            //var isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;
            //// Internet Explorer 6-11
            //var isIE = /*@cc_on!@*/false || !!document.documentMode;
            //// Edge 20+
            //var isEdge = !isIE && !!window.StyleMedia;
            //// Chrome 1+
            //var isChrome = !!window.chrome && !!window.chrome.webstore;
            //// Blink engine detection
            //var isBlink = (isChrome || isOpera) && !!window.CSS;
            function formatFormInPage() {
                $("#content-container").find(".x_content form").css({ "height": $(window).height() - 248, "overflow-y": "auto", "overflow-x": "hidden" });
                $("#content-container").find(".x_content form.beneficiary-detail").css({ "height": $(window).height() - 300, "overflow-y": "auto", "overflow-x": "hidden" });
                $("#content-container").find(".x_content .k-tabstrip-wrapper form").css({ "height": $(window).height() - 260, "overflow-y": "auto", "overflow-x": "hidden" });
                $("#content-container").find(".content-tab-index").css({ "height": $(window).height() - 180, "overflow-y": "auto", "overflow-x": "hidden" });

                $("#content-container").find(".content-tab-index .x_content form").css({ "height": $(window).height() - 303, "overflow-y": "auto", "overflow-x": "hidden" });
                var isChrome = !!window.chrome && !!window.chrome.webstore;
                if (isChrome) {
                    $("#content-container").find(".content-tab-index .x_content form").css({ "height": $(window).height() - 307, "overflow-y": "auto", "overflow-x": "hidden" });
                }
                $(".right_col").css({ height: $(window).height() });
                $("#content-container").find("form .content-tab").css({ height: $(window).height() - 205 });
                $("#content-container").find("form .content-tab.request").css({ height: $(window).height() - 155 });
                $("#content-container").find("form .content-tab.beneficiary").css({ height: $(window).height() - 155 });
                $("#content-container").find("form.detailcase .content-tab").css({ height: $(window).height() - 205 });
            }

            function detectMobileDevice() {
                return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
                //var ua = navigator.userAgent.toLowerCase();
                //var isAndroid = ua.indexOf("android") > -1; //&& ua.indexOf("mobile");
                //return isAndroid;
            }

            function convertNumberToNameMonth(month) {
                var monthNames = ["January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
                ];
                return monthNames[month - 1];
            }

            function checkAndSetWidthPopup(popup) {
                popup.setOptions({
                    width: popup.options.width > $(window).width() - 30 ? $(window).width() - 30 : popup.options.width
                });
            }

            function getLinkGooglePdfView(urlPdf) {
                return "http://docs.google.com/gview?embedded=true&url=" + urlPdf;
            }

            function getFirstDayOfWeek(curr) {
                var d = curr;
                var first = d.getDate() - d.getDay(); // First day is the day of the month - the day of the week
                return new Date(d.setDate(first));
            }

            function getLastDayOfWeek(curr) {
                var d = curr;
                var first = d.getDate() - d.getDay(); // First day is the day of the month - the day of the week
                var last = first + 6; // last day is the first day + 6

                return new Date(d.setDate(last));
            }

            function getFirstDateOfMonth(d) {
                var y = d.getFullYear(), m = d.getMonth();
                return new Date(y, m, 1);
            }
            function getLastDateOfMonth(d) {
                var y = d.getFullYear(), m = d.getMonth();
                return new Date(y, m + 1, 0);
            }
            function showErrorModelState(feedback, logType) {
                toastr.clear();
                toastr.options.onShown = function () {
                    $(".toast-message ul").css({ "max-height": $(window).height() - 150 });
                };
                // Build the error message
                $.get('/app/template/errorHandlerTemplate.html', function (html) {
                    var errorMessage = _.template(html)({ data: feedback });
                    toastr.options.closeButton = true;
                    toastr.options.timeOut = 5000;
                    toastr.options.extendedTimeOut = 5000;
                    if (logType === "Info") {
                        toastr.info(errorMessage);
                    } else {
                        toastr.error(errorMessage);
                    }
                    //toastr.options.closeButton = false;
                    //toastr.options.timeOut = 4000;
                    //toastr.options.extendedTimeOut = 1000;
                }, 'html');

            }
            function $broadcast() {
                return $rootScope.$broadcast.apply($rootScope, arguments);
            }

            function createSearchThrottle(viewmodel, list, filteredList, filter, delay) {
                // After a delay, search a viewmodel's list using 
                // a filter function, and return a filteredList.

                // custom delay or use default
                delay = +delay || 300;
                // if only vm and list parameters were passed, set others by naming convention 
                if (!filteredList) {
                    // assuming list is named sessions, filteredList is filteredSessions
                    filteredList = 'filtered' + list[0].toUpperCase() + list.substr(1).toLowerCase(); // string
                    // filter function is named sessionFilter
                    filter = list + 'Filter'; // function in string form
                }

                // create the filtering function we will call from here
                var filterFn = function () {
                    // translates to ...
                    // vm.filteredSessions 
                    //      = vm.sessions.filter(function(item( { returns vm.sessionFilter (item) } );
                    viewmodel[filteredList] = viewmodel[list].filter(function (item) {
                        return viewmodel[filter](item);
                    });
                };

                return (function () {
                    // Wrapped in outer IFFE so we can use closure 
                    // over filterInputTimeout which references the timeout
                    var filterInputTimeout;

                    // return what becomes the 'applyFilter' function in the controller
                    return function (searchNow) {
                        if (filterInputTimeout) {
                            $timeout.cancel(filterInputTimeout);
                            filterInputTimeout = null;
                        }
                        if (searchNow || !delay) {
                            filterFn();
                        } else {
                            filterInputTimeout = $timeout(filterFn, delay);
                        }
                    };
                })();
            }

            function debouncedThrottle(key, callback, delay, immediate) {
                // Perform some action (callback) after a delay. 
                // Track the callback by key, so if the same callback 
                // is issued again, restart the delay.

                var defaultDelay = 1000;
                delay = delay || defaultDelay;
                if (throttles[key]) {
                    $timeout.cancel(throttles[key]);
                    throttles[key] = undefined;
                }
                if (immediate) {
                    callback();
                } else {
                    throttles[key] = $timeout(callback, delay);
                }
            }

            function getWithPopupResponsive(pw) {

                return $(window).width() - 20 > pw ? pw + "px" : $(window).width() - 20 + "px";
            }
            function isNumber(val) {
                // negative or positive
                return /^[-]?\d+$/.test(val);
            }

            function textContains(text, searchText) {
                return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
            }

            function randomNumber() {
                return Math.floor((Math.random() * 1000000) + 1);
            }

            function hideAllBootboxConfirm() {
                bootbox.hideAll();
            }

            function bootboxConfirm(msg, callbackSuccess, callbackCancel) {
                var d = bootbox.confirm({
                    message: msg,
                    show: false,
                    className: "bootbox-small",
                    callback: function (result) {
                        if (result)
                            callbackSuccess();
                        else if (typeof (callbackCancel) == 'function')
                            callbackCancel();
                    }
                });

                $(window).on('popstate', function (event) {

                    bootbox.hideAll();
                });
                d.on("show.bs.modal", function () {
                    $(".bootbox-small").css({ "padding-top": ($(window).height() - 200) / 2, "width": "100%", "height": "100%" });
                });
                return d;
            }

            function bootboxAlert(msg, callbackSuccess, callbackCancel) {
                var d = bootbox.alert({
                    message: msg,
                    show: false,
                    className: "bootbox-small",
                    callback: function (result) {
                        if (result)
                            callbackSuccess();
                        else if (typeof (callbackCancel) == 'function')
                            callbackCancel();
                    }
                });

                $(window).on('popstate', function (event) {

                    bootbox.hideAll();
                });
                d.on("show.bs.modal", function () {
                    $(".bootbox-small").css({ "padding-top": ($(window).height() - 200) / 2, "width": "100%", "height": "100%" });
                });
                return d;
            }
            function setTimezoneCookie() {

                var timezoneCookie = "timezoneoffset";

                // if the timezone cookie not exists create one.
                if (!$.cookie(timezoneCookie)) {

                    // check if the browser supports cookie
                    $.cookie(timezoneCookie, true);

                    // browser supports cookie
                    if ($.cookie(timezoneCookie)) {
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

            function convertLocalDateToUtc(localdate) {
                var date = new Date();
                var offsetms = date.getTimezoneOffset() * 60 * 1000;
                var localDate = new Date(localdate);
                return new Date(localDate.valueOf() + offsetms);
            }
            function convertUtcToLocalTime(serverdate) {
                var date = new Date();
                var offsetms = date.getTimezoneOffset() * 60 * 1000;
                var serverDate = new Date(serverdate);
                return new Date(serverDate.valueOf() - offsetms);
            }

            function convertUtcToLocalDateString(serverdate) {
                var date = new Date();
                var offsetms = date.getTimezoneOffset() * 60 * 1000;
                var serverDate = new Date(serverdate);
                serverDate = new Date(serverDate.valueOf() - offsetms);
                return formatDate(serverDate);
            }

            function convertUtcToLocalDateTimeString(serverdate) {
                var date = new Date();
                var offsetms = date.getTimezoneOffset() * 60 * 1000;

                var serverDate = new Date(serverdate);

                serverDate = new Date(serverDate.valueOf() - offsetms);
                return formatDateTime(serverDate);
            }
            function formatTime(minute) {
                var hours = minute / 60;
                var minutes = minute % 60;
                var ampm = hours >= 12 ? 'PM' : 'AM';
                hours = hours % 12;
                hours = hours ? hours : 12; // the hour '0' should be '12'
                hours = hours < 10 ? '0' + hours : hours;
                minutes = minutes < 10 ? '0' + minutes : minutes;
                var strTime = hours + ':' + minutes + ' ' + ampm;
                return strTime;
            }
            function formatDateTime(datetime) {
                var date = datetime.getDate();
                var month = (datetime.getMonth() + 1);
                var hours = datetime.getHours();
                var minutes = datetime.getMinutes();
                var seconds = datetime.getSeconds();
                var ampm = hours >= 12 ? 'PM' : 'AM';
                hours = hours % 12;
                hours = hours ? hours : 12; // the hour '0' should be '12'
                date = date < 10 ? '0' + date : date;
                month = month < 10 ? '0' + month : month;
                hours = hours < 10 ? '0' + hours : hours;
                minutes = minutes < 10 ? '0' + minutes : minutes;
                seconds = seconds < 10 ? '0' + seconds : seconds;
                var strTime = hours + ':' + minutes + ':' + seconds + ' ' + ampm;
                return month + "/" + date + "/" +  datetime.getFullYear() + " " + strTime;
            }

            function formatDateTimeWithFormatddMMyyyyHHmm(datetime) {
                var date = datetime.getDate();
                var month = (datetime.getMonth() + 1);
                var hours = datetime.getHours();
                var minutes = datetime.getMinutes();
                date = date < 10 ? '0' + date : date;
                month = month < 10 ? '0' + month : month;
                hours = hours < 10 ? '0' + hours : hours;
                minutes = minutes < 10 ? '0' + minutes : minutes;
                var strTime = hours + ':' + minutes;
                return month + "/" + date + "/" + datetime.getFullYear() + " " + strTime;
            }

            function formatDateTimeMMddyyyyHHmmss(datetime) {
                var date = datetime.getDate();
                var month = (datetime.getMonth() + 1);
                var hours = datetime.getHours();
                var minutes = datetime.getMinutes();
                var second = datetime.getSeconds();

                date = date < 10 ? '0' + date : date;
                month = month < 10 ? '0' + month : month;
                hours = hours < 10 ? '0' + hours : hours;
                hours = hours < 10 ? '0' + hours : hours;
                minutes = minutes < 10 ? '0' + minutes : minutes;
                second = second < 10 ? '0' + second : second;

                return date + "" + month + "" + date + "" + datetime.getFullYear() + "" + hours + "" + minutes + "" + second;
            }
            function formatDate(datetime) {
                var date = datetime.getDate();
                var month = (datetime.getMonth() + 1);
                date = date < 10 ? '0' + date : date;
                month = month < 10 ? '0' + month : month;
                return month + "/" + date + "/" + datetime.getFullYear();
            }

            function getStartTimeOfDay(datetime) {
                var date = datetime.getDate();
                var month = (datetime.getMonth() + 1);
                date = date < 10 ? '0' + date : date;
                month = month < 10 ? '0' + month : month;
                return month + "/" + date + "/" + datetime.getFullYear() + " 12:00 AM";
            }
            function getEndTimeOfDay(datetime) {
                var date = datetime.getDate();
                var month = (datetime.getMonth() + 1);
                date = date < 10 ? '0' + date : date;
                month = month < 10 ? '0' + month : month;
                return month + "/" + date + "/" + datetime.getFullYear() + " 11:59 PM";
            }
            function convertyyyyMMddTHHmmssfffZtoDateTime(d) {
                return new Date((d || "").replace(/-/g, "/").replace(/[TZ]/g, " "));
            }

            function addDays(date, days) {
                var result = new Date(date);
                result.setDate(result.getDate() + days);
                return result;
            }

            function getDayOfWeek(date) {
                var weekday = new Array(7);
                weekday[0] = "Sunday";
                weekday[1] = "Monday";
                weekday[2] = "Tuesday";
                weekday[3] = "Wednesday";
                weekday[4] = "Thursday";
                weekday[5] = "Friday";
                weekday[6] = "Saturday";

                return weekday[date.getDay()];
            }

            function getMessageFromSystemMessage(key, params) {

                var result = "";
                var xmlDoc;
                if ($localStorage.xmlDoc == undefined || $localStorage.xmlDoc == null) {
                    if (typeof window.DOMParser != "undefined") {
                        var xmlhttp = new XMLHttpRequest();
                        xmlhttp.open("GET", "../../../ConfigData/ConfigData/SystemMessage.xml?v=" + formatDateTimeMMddyyyyHHmmss(new Date()), false);
                        if (xmlhttp.overrideMimeType) {
                            xmlhttp.overrideMimeType('text/xml');
                        }
                        xmlhttp.send();
                        xmlDoc = xmlhttp.responseXML;
                    } else {
                        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
                        xmlDoc.async = "false";
                        xmlDoc.load("../../../ConfigData/ConfigData/SystemMessage.xml?v=" + formatDateTimeMMddyyyyHHmmss(new Date()));
                    }
                    $localStorage.xmlDoc = xmlDoc;
                } else {
                    xmlDoc = $localStorage.xmlDoc;
                }

                var tagObj = xmlDoc.getElementsByTagName("root");
                if (tagObj != undefined && tagObj.length > 0 && tagObj[0].getElementsByTagName("Message").length > 0) {
                    var arr = tagObj[0].getElementsByTagName("Message");
                    for (var i = 0; i < arr.length ; i++) {
                        if (arr[i].getAttribute("key") == key && arr[i].getElementsByTagName("Content") != undefined && arr[i].getElementsByTagName("Content").length > 0 &&
                            arr[i].getElementsByTagName("Content")[0].childNodes != undefined && arr[i].getElementsByTagName("Content")[0].childNodes.length) {
                            result = format(arr[i].getElementsByTagName("Content")[0].childNodes[0].nodeValue, params);
                        }
                    }
                }
                return result;

            }
            function format(str, obj) {
                return str.replace(/\{\s*([^}\s]+)\s*\}/g, function (m, p1, offset, string) {
                    return obj[p1];
                });
            }

            function decodeObject(obj) {
                for (var key in obj) {
                    if (obj[key] != null && typeof obj[key] == "string") {
                        obj[key] = decodeHTMLEntities(obj[key].toString());
                    }
                }
                return obj;
            }
            function htmlEscape(str) {
                return str
                    .replace(/&/g, '&amp;')
                    .replace(/"/g, '&quot;')
                    .replace(/'/g, '&#39;')
                    .replace(/</g, '&lt;')
                    .replace(/>/g, '&gt;');
            }

            // I needed the opposite function today, so adding here too:
            function htmlUnescape(str) {
                return str
                    .replace(/&quot;/g, '"')
                    .replace(/&#39;/g, "'")
                    .replace(/&lt;/g, '<')
                    .replace(/&gt;/g, '>')
                    .replace(/&amp;/g, '&');
            }
            function encodeHtmlEntities(text) {

                // all HTML4 entities as defined here: http://www.w3.org/TR/html4/sgml/entities.html
                // added: amp, lt, gt, quot and apos
                var entityTable = {
                    34: 'quot',
                    38: 'amp',
                    39: 'apos',
                    60: 'lt',
                    62: 'gt',
                    160: 'nbsp',
                    161: 'iexcl',
                    162: 'cent',
                    163: 'pound',
                    164: 'curren',
                    165: 'yen',
                    166: 'brvbar',
                    167: 'sect',
                    168: 'uml',
                    169: 'copy',
                    170: 'ordf',
                    171: 'laquo',
                    172: 'not',
                    173: 'shy',
                    174: 'reg',
                    175: 'macr',
                    176: 'deg',
                    177: 'plusmn',
                    178: 'sup2',
                    179: 'sup3',
                    180: 'acute',
                    181: 'micro',
                    182: 'para',
                    183: 'middot',
                    184: 'cedil',
                    185: 'sup1',
                    186: 'ordm',
                    187: 'raquo',
                    188: 'frac14',
                    189: 'frac12',
                    190: 'frac34',
                    191: 'iquest',
                    192: 'Agrave',
                    193: 'Aacute',
                    194: 'Acirc',
                    195: 'Atilde',
                    196: 'Auml',
                    197: 'Aring',
                    198: 'AElig',
                    199: 'Ccedil',
                    200: 'Egrave',
                    201: 'Eacute',
                    202: 'Ecirc',
                    203: 'Euml',
                    204: 'Igrave',
                    205: 'Iacute',
                    206: 'Icirc',
                    207: 'Iuml',
                    208: 'ETH',
                    209: 'Ntilde',
                    210: 'Ograve',
                    211: 'Oacute',
                    212: 'Ocirc',
                    213: 'Otilde',
                    214: 'Ouml',
                    215: 'times',
                    216: 'Oslash',
                    217: 'Ugrave',
                    218: 'Uacute',
                    219: 'Ucirc',
                    220: 'Uuml',
                    221: 'Yacute',
                    222: 'THORN',
                    223: 'szlig',
                    224: 'agrave',
                    225: 'aacute',
                    226: 'acirc',
                    227: 'atilde',
                    228: 'auml',
                    229: 'aring',
                    230: 'aelig',
                    231: 'ccedil',
                    232: 'egrave',
                    233: 'eacute',
                    234: 'ecirc',
                    235: 'euml',
                    236: 'igrave',
                    237: 'iacute',
                    238: 'icirc',
                    239: 'iuml',
                    240: 'eth',
                    241: 'ntilde',
                    242: 'ograve',
                    243: 'oacute',
                    244: 'ocirc',
                    245: 'otilde',
                    246: 'ouml',
                    247: 'divide',
                    248: 'oslash',
                    249: 'ugrave',
                    250: 'uacute',
                    251: 'ucirc',
                    252: 'uuml',
                    253: 'yacute',
                    254: 'thorn',
                    255: 'yuml',
                    402: 'fnof',
                    913: 'Alpha',
                    914: 'Beta',
                    915: 'Gamma',
                    916: 'Delta',
                    917: 'Epsilon',
                    918: 'Zeta',
                    919: 'Eta',
                    920: 'Theta',
                    921: 'Iota',
                    922: 'Kappa',
                    923: 'Lambda',
                    924: 'Mu',
                    925: 'Nu',
                    926: 'Xi',
                    927: 'Omicron',
                    928: 'Pi',
                    929: 'Rho',
                    931: 'Sigma',
                    932: 'Tau',
                    933: 'Upsilon',
                    934: 'Phi',
                    935: 'Chi',
                    936: 'Psi',
                    937: 'Omega',
                    945: 'alpha',
                    946: 'beta',
                    947: 'gamma',
                    948: 'delta',
                    949: 'epsilon',
                    950: 'zeta',
                    951: 'eta',
                    952: 'theta',
                    953: 'iota',
                    954: 'kappa',
                    955: 'lambda',
                    956: 'mu',
                    957: 'nu',
                    958: 'xi',
                    959: 'omicron',
                    960: 'pi',
                    961: 'rho',
                    962: 'sigmaf',
                    963: 'sigma',
                    964: 'tau',
                    965: 'upsilon',
                    966: 'phi',
                    967: 'chi',
                    968: 'psi',
                    969: 'omega',
                    977: 'thetasym',
                    978: 'upsih',
                    982: 'piv',
                    8226: 'bull',
                    8230: 'hellip',
                    8242: 'prime',
                    8243: 'Prime',
                    8254: 'oline',
                    8260: 'frasl',
                    8472: 'weierp',
                    8465: 'image',
                    8476: 'real',
                    8482: 'trade',
                    8501: 'alefsym',
                    8592: 'larr',
                    8593: 'uarr',
                    8594: 'rarr',
                    8595: 'darr',
                    8596: 'harr',
                    8629: 'crarr',
                    8656: 'lArr',
                    8657: 'uArr',
                    8658: 'rArr',
                    8659: 'dArr',
                    8660: 'hArr',
                    8704: 'forall',
                    8706: 'part',
                    8707: 'exist',
                    8709: 'empty',
                    8711: 'nabla',
                    8712: 'isin',
                    8713: 'notin',
                    8715: 'ni',
                    8719: 'prod',
                    8721: 'sum',
                    8722: 'minus',
                    8727: 'lowast',
                    8730: 'radic',
                    8733: 'prop',
                    8734: 'infin',
                    8736: 'ang',
                    8743: 'and',
                    8744: 'or',
                    8745: 'cap',
                    8746: 'cup',
                    8747: 'int',
                    8756: 'there4',
                    8764: 'sim',
                    8773: 'cong',
                    8776: 'asymp',
                    8800: 'ne',
                    8801: 'equiv',
                    8804: 'le',
                    8805: 'ge',
                    8834: 'sub',
                    8835: 'sup',
                    8836: 'nsub',
                    8838: 'sube',
                    8839: 'supe',
                    8853: 'oplus',
                    8855: 'otimes',
                    8869: 'perp',
                    8901: 'sdot',
                    8968: 'lceil',
                    8969: 'rceil',
                    8970: 'lfloor',
                    8971: 'rfloor',
                    9001: 'lang',
                    9002: 'rang',
                    9674: 'loz',
                    9824: 'spades',
                    9827: 'clubs',
                    9829: 'hearts',
                    9830: 'diams',
                    338: 'OElig',
                    339: 'oelig',
                    352: 'Scaron',
                    353: 'scaron',
                    376: 'Yuml',
                    710: 'circ',
                    732: 'tilde',
                    8194: 'ensp',
                    8195: 'emsp',
                    8201: 'thinsp',
                    8204: 'zwnj',
                    8205: 'zwj',
                    8206: 'lrm',
                    8207: 'rlm',
                    8211: 'ndash',
                    8212: 'mdash',
                    8216: 'lsquo',
                    8217: 'rsquo',
                    8218: 'sbquo',
                    8220: 'ldquo',
                    8221: 'rdquo',
                    8222: 'bdquo',
                    8224: 'dagger',
                    8225: 'Dagger',
                    8240: 'permil',
                    8249: 'lsaquo',
                    8250: 'rsaquo',
                    8364: 'euro'
                };
                return text.replace(/[\u00A0-\u2666<>\&]/g, function (c) {
                    return '&' +
                    (entityTable[c.charCodeAt(0)] || '#' + c.charCodeAt(0)) + ';';
                });
            }

            function decodeHTMLEntities(text) {
                var entities = [
                    ['amp', '&'],
                    ['apos', '\''],
                    ['#x27', '\''],
                    ['#x2F', '/'],
                    ['#39', '\''],
                    ['#47', '/'],
                    ['lt', '<'],
                    ['gt', '>'],
                    ['nbsp', ' '],
                    ['quot', '"']
                ];

                for (var i = 0, max = entities.length; i < max; ++i)
                    text = text.replace(new RegExp('&' + entities[i][0] + ';', 'g'), entities[i][1]);

                return text;
            }
            function encode(str) {
                // Create Base64 Object
                var base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9+/=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/rn/g, "n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }

                // Encode the String
                return base64.encode(str);
            }

            function decode(str) {
                // Create Base64 Object
                var base64 = { _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", encode: function (e) { var t = ""; var n, r, i, s, o, u, a; var f = 0; e = Base64._utf8_encode(e); while (f < e.length) { n = e.charCodeAt(f++); r = e.charCodeAt(f++); i = e.charCodeAt(f++); s = n >> 2; o = (n & 3) << 4 | r >> 4; u = (r & 15) << 2 | i >> 6; a = i & 63; if (isNaN(r)) { u = a = 64 } else if (isNaN(i)) { a = 64 } t = t + this._keyStr.charAt(s) + this._keyStr.charAt(o) + this._keyStr.charAt(u) + this._keyStr.charAt(a) } return t }, decode: function (e) { var t = ""; var n, r, i; var s, o, u, a; var f = 0; e = e.replace(/[^A-Za-z0-9+/=]/g, ""); while (f < e.length) { s = this._keyStr.indexOf(e.charAt(f++)); o = this._keyStr.indexOf(e.charAt(f++)); u = this._keyStr.indexOf(e.charAt(f++)); a = this._keyStr.indexOf(e.charAt(f++)); n = s << 2 | o >> 4; r = (o & 15) << 4 | u >> 2; i = (u & 3) << 6 | a; t = t + String.fromCharCode(n); if (u != 64) { t = t + String.fromCharCode(r) } if (a != 64) { t = t + String.fromCharCode(i) } } t = Base64._utf8_decode(t); return t }, _utf8_encode: function (e) { e = e.replace(/rn/g, "n"); var t = ""; for (var n = 0; n < e.length; n++) { var r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r) } else if (r > 127 && r < 2048) { t += String.fromCharCode(r >> 6 | 192); t += String.fromCharCode(r & 63 | 128) } else { t += String.fromCharCode(r >> 12 | 224); t += String.fromCharCode(r >> 6 & 63 | 128); t += String.fromCharCode(r & 63 | 128) } } return t }, _utf8_decode: function (e) { var t = ""; var n = 0; var r = c1 = c2 = 0; while (n < e.length) { r = e.charCodeAt(n); if (r < 128) { t += String.fromCharCode(r); n++ } else if (r > 191 && r < 224) { c2 = e.charCodeAt(n + 1); t += String.fromCharCode((r & 31) << 6 | c2 & 63); n += 2 } else { c2 = e.charCodeAt(n + 1); c3 = e.charCodeAt(n + 2); t += String.fromCharCode((r & 15) << 12 | (c2 & 63) << 6 | c3 & 63); n += 3 } } return t } }

                // Decode the String
                return base64.decode(str);
            }

            function getActionMessageResult(actionNumber_ResultValue, params) {
                //var result = getMessageFromSystemMessage(actionNumber_ResultValue, params, true);
                var result = [];
                var xmlDoc;
                if ($localStorage.xmlDoc == undefined || $localStorage.xmlDoc == null) {
                    if (typeof window.DOMParser != "undefined") {
                        var xmlhttp = new XMLHttpRequest();
                        xmlhttp.open("GET", "../../../ConfigData/ConfigData/SystemMessage.xml?v=" + formatDateTimeMMddyyyyHHmmss(new Date()), false);
                        if (xmlhttp.overrideMimeType) {
                            xmlhttp.overrideMimeType('text/xml');
                        }
                        xmlhttp.send();
                        xmlDoc = xmlhttp.responseXML;
                    } else {
                        xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
                        xmlDoc.async = "false";
                        xmlDoc.load("../../../ConfigData/ConfigData/SystemMessage.xml?v=" + formatDateTimeMMddyyyyHHmmss(new Date()));
                    }
                    $localStorage.xmlDoc = xmlDoc;
                } else {
                    xmlDoc = $localStorage.xmlDoc;
                }

                var tagObj = xmlDoc.getElementsByTagName("root");
                if (tagObj != undefined && tagObj.length > 0 && tagObj[0].getElementsByTagName("Message").length > 0) {
                    var arr = tagObj[0].getElementsByTagName("Message");
                    for (var i = 0; i < arr.length ; i++) {
                        if (arr[i].getAttribute("key") == actionNumber_ResultValue && arr[i].getElementsByTagName("Content") != undefined && arr[i].getElementsByTagName("Content").length > 0 &&
                            arr[i].getElementsByTagName("Content")[0].childNodes != undefined && arr[i].getElementsByTagName("Content")[0].childNodes.length) {
                            result.push({
                                "Content": format(arr[i].getElementsByTagName("Content")[0].childNodes[0].nodeValue, params),
                                "Type": format(arr[i].getElementsByTagName("Type")[0].childNodes[0].nodeValue, params),
                                "Order": format(arr[i].getElementsByTagName("Order")[0].childNodes[0].nodeValue, params)
                            });
                        }
                    }
                }
                if (result.length == 0) {
                    result.push({
                        "Content": getMessageFromSystemMessage("ExcuteSuccessfully", params),
                        "Type": "Success",
                        "Order": "1"
                    });
                    return result;
                } else {

                    return result.sort(sort_by('Order', false, parseInt));
                }
            }

            function convertTotalSecondToHourFormat(totalSec) {
                var hours = parseInt(totalSec / 3600) % 24;
                var minutes = parseInt(totalSec / 60) % 60;
                var seconds = totalSec % 60;

                return (hours < 10 ? "0" + hours : hours) + ":" + (minutes < 10 ? "0" + minutes : minutes) + ":" + (seconds < 10 ? "0" + seconds : seconds);
            }

            function isValidTime(str) {
                var dateReg = /^(1[012]|0[1-9]):[0-5][0-9](\\s)? (AM|PM)+$/;
                if (!dateReg.test(str)) {
                    return false;
                }
                return true;
            }

            function isValidDate(str) {
                var comp = str.split('/');
                var m = parseInt(comp[1], 10);
                var d = parseInt(comp[0], 10);
                var y = parseInt(comp[2], 10);
                var date = new Date(y, m - 1, d);
                if (date.getFullYear() == y && date.getMonth() + 1 == m && date.getDate() == d) {
                    return true;
                }
                return false;
            }

            function isValidDatetime(str) {
                if (Object.prototype.toString.call(str) === "[object Date]") {
                    // it is a date
                    if (isNaN(str.getTime())) {  // d.valueOf() could also work
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    var comp1 = str.split(' ');
                    var m, d, y, h, mi, t;
                    if (comp1.length == 3) {
                        var comp2 = comp1[0].split('/');
                        if (comp2.length == 3) {
                            m = parseInt(comp2[0], 10);
                            d = parseInt(comp2[1], 10);
                            y = parseInt(comp2[2], 10);
                        }
                        var comp3 = comp1[1].split(':');
                        if (comp3.length == 2) {
                            h = parseInt(comp3[0], 10);
                            mi = parseInt(comp3[1], 10);
                        }
                        t = comp1[2];

                    }
                    var date = new Date(y, m - 1, d, h, mi, 0, 0);
                    if (date.getFullYear() == y && date.getMonth() + 1 == m && date.getDate() == d && date.getHours() == h && date.getMinutes() == mi && (t == "AM" || t == "PM")) {
                        if (y < 1800 || y > 2099) {//kendoui datetimepick dont accept year less than 1800 and..
                            return false;
                        }
                        return true;
                    }
                    return false;
                }

            }
            //filter status task
            function listFilterStatusTask() {
                return [{ 'Id': '1', 'Text': 'Open', 'Lable': 'label-default', 'IsSelected': true },
                        { 'Id': '2', 'Text': 'Inprogress', 'Lable': 'label-success', 'IsSelected': false },
                        { 'Id': '3', 'Text': 'Done', 'Lable': 'label-primary', 'IsSelected': false },
                        { 'Id': '4', 'Text': 'Cancel', 'Lable': 'label-warning', 'IsSelected': false }];
            }

            function listFilterStatusTaskSelected() {
                return ["1"];
            }
            //

            function compareWithCurrentTime(val) {
                var date = new Date(val);
                date = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0);
                var timestampVal = Date.parse(date);

                var now = new Date();
                now = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0, 0);
                var timestampNow = Date.parse(now);

                if (timestampVal < timestampNow) {
                    return false;
                }
                return true;
            }
            function getVariableFromUrl(name, url) {
                if (!url) {
                    url = window.location.href;
                }
                name = name.replace(/[\[\]]/g, "\\$&");
                var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                    results = regex.exec(url);
                if (!results) return null;
                if (!results[2]) return '';
                return decodeURIComponent(results[2].replace(/\+/g, " "));
            }
            function removeURLParameter(url, parameter) {
                //prefer to use l.search if you have a location/link object
                var urlparts = url.split('?');
                if (urlparts.length >= 2) {

                    var prefix = encodeURIComponent(parameter) + '=';
                    var pars = urlparts[1].split(/[&;]/g);

                    //reverse iteration as may be destructive
                    for (var i = pars.length; i-- > 0;) {
                        //idiom for string.startsWith
                        if (pars[i].lastIndexOf(prefix, 0) !== -1) {
                            pars.splice(i, 1);
                        }
                    }

                    url = urlparts[0] + (pars.length > 0 ? '?' + pars.join('&') : "");
                    return url;
                } else {
                    return url;
                }
            }
            function removeAllVariableFromUrl(url) {
                //prefer to use l.search if you have a location/link object
                var urlparts = url.split('?');
                if (urlparts.length >= 2) {
                    return urlparts[0];
                } else {
                    return url;
                }
            }

            function getDaysBetweenDay(startDate, endDate) {
                var diff = endDate.getTime() - startDate.getTime();
                var minutes = diff / 60000;
                return parseFloat(minutes / 1440).toFixed(4);
            }

            function returnColorRequest(dataItem) {
                //console.log(dataItem );
                var now = new Date();
                var startDate = new Date();
                var endDate = new Date();
                var days = 0;
                var color = 1;

                if (dataItem.RequestType === 1) {//Initial
                    if (dataItem.MilestoneId === 1 || dataItem.MilestoneId === 8) {//Request
                        if (dataItem.ColorRequestStartDateFormat != null) {
                            startDate = dataItem.ColorRequestStartDateFormat;
                            if (dataItem.ColorRequestEndDateFormat != null) {
                                endDate = dataItem.ColorRequestEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                            //console.log(dataItem.RequestNo, days);
                            if (days <= 2) {
                                color = color > 1 ? color : 1;
                            }
                            else if (days > 2 && days <= 4) {
                                color = color > 2 ? color : 2;
                            }
                            else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 2) { //Scheduling
                        if (dataItem.ColorSchedulingStartDateFormat != null) {
                            startDate = dataItem.ColorSchedulingStartDateFormat;
                            if (dataItem.ColorSchedulingEndDateFormat != null) {
                                endDate = dataItem.ColorSchedulingEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 7) {
                                color = color > 1 ? color : 1;
                            } else if (days > 7 && days <= 8) {
                                color = color > 2 ? color : 2;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 3) { //Accessment
                        //return color;
                        if (dataItem.ColorAssessmentStartDateFormat != null) {
                            startDate = dataItem.ColorAssessmentStartDateFormat;
                            if (dataItem.ColorAssessmentEndDateFormat != null) {
                                endDate = dataItem.ColorAssessmentEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 4) { //Quality Control
                        if (dataItem.ColorQCStartDateFormat != null) {
                            startDate = dataItem.ColorQCStartDateFormat;
                            if (dataItem.ColorQCEndDateFormat != null) {
                                endDate = dataItem.ColorQCEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 5) { //Provider Acceptance
                        if (dataItem.ColorProviderAcceptanceStartDateFormat != null) {
                            startDate = dataItem.ColorProviderAcceptanceStartDateFormat;
                            if (dataItem.ColorProviderAcceptanceEndDateFormat != null) {
                                endDate = dataItem.ColorProviderAcceptanceEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 5) {
                                color = color > 1 ? color : 1;
                            }
                            else if (days > 5 && days <= 10) {
                                color = color > 2 ? color : 2;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 6) {//Physician Order
                        if (dataItem.ColorPhysicianOrderStartDateFormat != null) {
                            startDate = dataItem.ColorPhysicianOrderStartDateFormat;
                            if (dataItem.ColorPhysicianOrderEndDateFormat != null) {
                                endDate = dataItem.ColorPhysicianOrderEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days < 3) {
                                color = color > 1 ? color : 1;
                            }
                            else if (days >= 3 && days <= 7) {
                                color = color > 2 ? color : 2;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 7) {//Prior Authorization
                        if (dataItem.ColorPAStartDateFormat != null) {
                            startDate = dataItem.ColorPAStartDateFormat;
                            if (dataItem.ColorPAEndDateFormat != null) {
                                endDate = dataItem.ColorPAEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    //if (dataItem.MilestoneId === 8) {//Close
                    //    //return color;
                    //    return {
                    //        color: color,
                    //        numberDuration: days
                    //    };
                    //}
                    //return color;
                    return {
                        color: color,
                        numberDuration: days
                    };
                }

                if (dataItem.RequestType === 5) {//Continuation
                    //console.log('vo', dataItem.RequestNo, dataItem);
                    if (dataItem.MilestoneId === 1 || dataItem.MilestoneId === 8) {//Request      
                        if (dataItem.ColorRequestStartDateFormat != null) {
                            startDate = dataItem.ColorRequestStartDateFormat;
                            if (dataItem.ColorRequestEndDateFormat != null) {
                                endDate = dataItem.ColorRequestEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 2) { //Scheduling
                        if (dataItem.ColorSchedulingStartDateFormat != null) {
                            startDate = dataItem.ColorSchedulingStartDateFormat;
                            if (dataItem.ColorSchedulingEndDateFormat != null) {
                                endDate = dataItem.ColorSchedulingEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 7) {
                                color = color > 1 ? color : 1;
                            }
                            else {
                                color = 3;
                            }
                        }
                        //console.log(dataItem.RequestNo, dataItem.RequestType, days);
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 3) { //Accessment
                        if (dataItem.ColorAssessmentStartDateFormat != null) {
                            startDate = dataItem.ColorAssessmentStartDateFormat;
                            if (dataItem.ColorAssessmentEndDateFormat != null) {
                                endDate = dataItem.ColorAssessmentEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 4) { //Quality Control
                        if (dataItem.ColorQCStartDateFormat != null) {
                            startDate = dataItem.ColorQCStartDateFormat;
                            if (dataItem.ColorQCEndDateFormat != null) {
                                endDate = dataItem.ColorQCEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 5) { //Provider Acceptance
                        if (dataItem.ColorProviderAcceptanceStartDateFormat != null) {
                            startDate = dataItem.ColorProviderAcceptanceStartDateFormat;
                            if (dataItem.ColorProviderAcceptanceEndDateFormat != null) {
                                endDate = dataItem.ColorProviderAcceptanceEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 5) {
                                color = color > 1 ? color : 1;
                            }
                            else if (days > 5 && days <= 10) {
                                color = color > 2 ? color : 2;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 6) {//Physician Order
                        if (dataItem.ColorPhysicianOrderStartDateFormat != null) {
                            startDate = dataItem.ColorPhysicianOrderStartDateFormat;
                            if (dataItem.ColorPhysicianOrderEndDateFormat != null) {
                                endDate = dataItem.ColorPhysicianOrderEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days < 3) {
                                color = color > 1 ? color : 1;
                            }
                            else if (days >= 3 && days <= 7) {
                                color = color > 2 ? color : 2;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 7) {//Prior Authorization
                        if (dataItem.ColorPAStartDateFormat != null) {
                            startDate = dataItem.ColorPAStartDateFormat;
                            if (dataItem.ColorPAEndDateFormat != null) {
                                endDate = dataItem.ColorPAEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    //if (dataItem.MilestoneId === 8) {//Close
                    //    //return color;
                    //    return {
                    //        color: color,
                    //        numberDuration: days
                    //    };
                    //}
                    //return color;
                    return {
                        color: color,
                        numberDuration: days
                    };
                }

                if (dataItem.RequestType === 3) {//Change Of Status
                    if (dataItem.MilestoneId === 1 || dataItem.MilestoneId === 8) {//Request  
                        if (dataItem.ColorRequestStartDateFormat != null) {
                            startDate = dataItem.ColorRequestStartDateFormat;
                            if (dataItem.ColorRequestEndDateFormat != null) {
                                endDate = dataItem.ColorRequestEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 2) {
                                color = color > 1 ? color : 1;
                            }
                            else if (days > 2 && days <= 4) {
                                color = color > 2 ? color : 2;
                            }
                            else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 2) { //Scheduling
                        if (dataItem.ColorSchedulingStartDateFormat != null) {
                            startDate = dataItem.ColorSchedulingStartDateFormat;
                            if (dataItem.ColorSchedulingEndDateFormat != null) {
                                endDate = dataItem.ColorSchedulingEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 7) {
                                color = color > 1 ? color : 1;
                            }
                            else if (days > 7 && days <= 8) {
                                color = color > 2 ? color : 2;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 3) { //Accessment
                        //return color;
                        if (dataItem.ColorAssessmentStartDateFormat != null) {
                            startDate = dataItem.ColorAssessmentStartDateFormat;
                            if (dataItem.ColorAssessmentEndDateFormat != null) {
                                endDate = dataItem.ColorAssessmentEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 4) { //Quality Control
                        if (dataItem.ColorQCStartDateFormat != null) {
                            startDate = dataItem.ColorQCStartDateFormat;
                            if (dataItem.ColorQCEndDateFormat != null) {
                                endDate = dataItem.ColorQCEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 5) { //Provider Acceptance 
                        if (dataItem.ColorProviderAcceptanceStartDateFormat != null) {
                            startDate = dataItem.ColorProviderAcceptanceStartDateFormat;
                            if (dataItem.ColorProviderAcceptanceEndDateFormat != null) {
                                endDate = dataItem.ColorProviderAcceptanceEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 6) {//Physician Order
                        if (dataItem.ColorPhysicianOrderStartDateFormat != null) {
                            startDate = dataItem.ColorPhysicianOrderStartDateFormat;
                            if (dataItem.ColorPhysicianOrderEndDateFormat != null) {
                                endDate = dataItem.ColorPhysicianOrderEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days < 3) {
                                color = color > 1 ? color : 1;
                            }
                            else if (days >= 3 && days <= 7) {
                                color = color > 2 ? color : 2;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 7) {//Prior Authorization
                        if (dataItem.ColorPAStartDateFormat != null) {
                            startDate = dataItem.ColorPAStartDateFormat;
                            if (dataItem.ColorPAEndDateFormat != null) {
                                endDate = dataItem.ColorPAEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    //if (dataItem.MilestoneId === 8) {//Close
                    //    //return color;
                    //    return {
                    //        color: color,
                    //        numberDuration: days
                    //    };
                    //}
                    //return color;
                    return {
                        color: color,
                        numberDuration: days
                    };
                }

                if (dataItem.RequestType === 4) {//Change Of Provider
                    if (dataItem.MilestoneId === 1 || dataItem.MilestoneId === 8) {//Request  
                        if (dataItem.ColorRequestStartDateFormat != null) {
                            startDate = dataItem.ColorRequestStartDateFormat;
                            if (dataItem.ColorRequestEndDateFormat != null) {
                                endDate = dataItem.ColorRequestEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 2) {
                                color = color > 1 ? color : 1;
                            }
                            else if (days > 2 && days <= 4) {
                                color = color > 2 ? color : 2;
                            }
                            else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 2) { //Scheduling  
                        if (dataItem.ColorSchedulingStartDateFormat != null) {
                            startDate = dataItem.ColorSchedulingStartDateFormat;
                            if (dataItem.ColorSchedulingEndDateFormat != null) {
                                endDate = dataItem.ColorSchedulingEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 3) { //Accessment
                        //return color;
                        if (dataItem.ColorAssessmentStartDateFormat != null) {
                            startDate = dataItem.ColorAssessmentStartDateFormat;
                            if (dataItem.ColorAssessmentEndDateFormat != null) {
                                endDate = dataItem.ColorAssessmentEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 4) { //Quality Control
                        if (dataItem.ColorQCStartDateFormat != null) {
                            startDate = dataItem.ColorQCStartDateFormat;
                            if (dataItem.ColorQCEndDateFormat != null) {
                                endDate = dataItem.ColorQCEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 5) { //Provider Acceptance   
                        if (dataItem.ColorProviderAcceptanceStartDateFormat != null) {
                            startDate = dataItem.ColorProviderAcceptanceStartDateFormat;
                            if (dataItem.ColorProviderAcceptanceEndDateFormat != null) {
                                endDate = dataItem.ColorProviderAcceptanceEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 2) {
                                color = color > 1 ? color : 1;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 6) {//Physician Order 
                        if (dataItem.ColorPhysicianOrderStartDateFormat != null) {
                            startDate = dataItem.ColorPhysicianOrderStartDateFormat;
                            if (dataItem.ColorPhysicianOrderEndDateFormat != null) {
                                endDate = dataItem.ColorPhysicianOrderEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 7) {//Prior Authorization
                        if (dataItem.ColorPAStartDateFormat != null) {
                            startDate = dataItem.ColorPAStartDateFormat;
                            if (dataItem.ColorPAEndDateFormat != null) {
                                endDate = dataItem.ColorPAEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    //if (dataItem.MilestoneId === 8) {//Close
                    //    //return color;
                    //    return {
                    //        color: color,
                    //        numberDuration: days
                    //    };
                    //}
                    //return color;
                    return {
                        color: color,
                        numberDuration: days
                    };
                }

                if (dataItem.RequestType === 2) {//Expedited
                    if (dataItem.MilestoneId === 1 || dataItem.MilestoneId === 8) {//Request  
                        if (dataItem.ColorRequestStartDateFormat != null) {
                            startDate = dataItem.ColorRequestStartDateFormat;
                            if (dataItem.ColorRequestEndDateFormat != null) {
                                endDate = dataItem.ColorRequestEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 1) {
                                color = color > 1 ? color : 1;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 2) { //Scheduling   
                        if (dataItem.ColorSchedulingStartDateFormat != null) {
                            startDate = dataItem.ColorSchedulingStartDateFormat;
                            if (dataItem.ColorSchedulingEndDateFormat != null) {
                                endDate = dataItem.ColorSchedulingEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 3) { //Accessment
                        //return color;
                        if (dataItem.ColorAssessmentStartDateFormat != null) {
                            startDate = dataItem.ColorAssessmentStartDateFormat;
                            if (dataItem.ColorAssessmentEndDateFormat != null) {
                                endDate = dataItem.ColorAssessmentEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 4) { //Quality Control
                        if (dataItem.ColorQCStartDateFormat != null) {
                            startDate = dataItem.ColorQCStartDateFormat;
                            if (dataItem.ColorQCEndDateFormat != null) {
                                endDate = dataItem.ColorQCEndDateFormat;
                            } else {
                                endDate = now;
                            }

                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 5) { //Provider Acceptance   
                        if (dataItem.ColorProviderAcceptanceStartDateFormat != null) {
                            startDate = dataItem.ColorProviderAcceptanceStartDateFormat;
                            if (dataItem.ColorProviderAcceptanceEndDateFormat != null) {
                                endDate = dataItem.ColorProviderAcceptanceEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 1) {
                                color = color > 1 ? color : 1;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 6) {//Physician Order
                        if (dataItem.ColorPhysicianOrderStartDateFormat != null) {
                            startDate = dataItem.ColorPhysicianOrderStartDateFormat;
                            if (dataItem.ColorPhysicianOrderEndDateFormat != null) {
                                endDate = dataItem.ColorPhysicianOrderEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                            if (days <= 1) {
                                color = color > 1 ? color : 1;
                            } else {
                                color = 3;
                            }
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    if (dataItem.MilestoneId === 7) {//Prior Authorization
                        if (dataItem.ColorPAStartDateFormat != null) {
                            startDate = dataItem.ColorPAStartDateFormat;
                            if (dataItem.ColorPAEndDateFormat != null) {
                                endDate = dataItem.ColorPAEndDateFormat;
                            } else {
                                endDate = now;
                            }
                            days = getDaysBetweenDay(startDate, endDate);
                        }
                        //return color;
                        return {
                            color: color,
                            numberDuration: days
                        };
                    }
                    //if (dataItem.MilestoneId === 8) {//Close
                    //    //return color;
                    //    return {
                    //        color: color,
                    //        numberDuration: days
                    //    };
                    //}
                    //return color;
                    return {
                        color: color,
                        numberDuration: days
                    };
                }

                //return color;
                return {
                    color: color,
                    numberDuration: days
                };
            }

        }
    };
});