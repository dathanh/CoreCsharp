function screenFit_1() {
    var responsiveContent1NewHeight = Math.floor(window.innerHeight - 51 - 15 - 40 - 15);
    var fullScreen = document.fullScreen || document.mozFullScreen || document.webkitIsFullScreen;
    if (fullScreen)
        responsiveContent1NewHeight = Math.floor(window.innerHeight - 40);
    var responsiveContent1 = $(".screen-fit-1");

    _.each(responsiveContent1, function (e) {
        $(e).parent().children(".panel-control").length;
        var gridContentHeight = responsiveContent1NewHeight;
        _.each($(e).closest(".panel").children(".panel-control"), function (ctr) {
            gridContentHeight -= $(ctr).outerHeight();
        });
        $(e).height(gridContentHeight);
    });
}

function screenFit_2() {
    var responsiveContent2NewHeight = Math.floor((window.innerHeight - 51 - 15) / 2 - 40 - 15);
    var responsiveContent2 = $(".screen-fit-2");


    _.each(responsiveContent2, function (e) {
        $(e).parent().children(".panel-control").length;
        var gridContentHeight = responsiveContent2NewHeight;
        _.each($(e).closest(".panel").children(".panel-control"), function (ctr) {
            gridContentHeight -= $(ctr).outerHeight();
        });
        $(e).height(gridContentHeight);
    });

    //responsiveContent2.height(responsiveContent2NewHeight);

}

function screenFitGrid_1() {
    var responsiveContent1NewHeight = Math.floor(window.innerHeight - 51 - 15 - 40 - 15-30);
    var fullScreen = document.fullScreen || document.mozFullScreen || document.webkitIsFullScreen;
    if (fullScreen)
        responsiveContent1NewHeight = Math.floor(window.innerHeight - 40);
    var responsiveContent1 = $(".screen-fit-1");

    responsiveContent1.find(".reponsive-list").height(responsiveContent1NewHeight);
    _.each(responsiveContent1, function (e) {

        var gridControlHeight = 68;
        if ($(e).find(".hidden-pager").length > 0) gridControlHeight = 30;

        $(e).parent().children(".panel-control").length;
        var gridContentHeight = responsiveContent1NewHeight;
        _.each($(e).closest(".panel").children(".panel-control"), function (ctr) {
            gridContentHeight -= $(ctr).outerHeight();
        });
        $(e).find(".k-grid .k-grid-content").height(gridContentHeight - gridControlHeight);
    });
}

function screenFitGrid_2() {
    var responsiveContent2NewHeight = Math.floor((window.innerHeight - 51 - 15) / 2 - 40 - 15-30);
    var responsiveContent2 = $(".screen-fit-2");

    responsiveContent2.find(".reponsive-list").height(responsiveContent2NewHeight);
    
    _.each(responsiveContent2, function (e) {

        var gridControlHeight = 68;
        if ($(e).find(".hidden-pager").length > 0) gridControlHeight = 30;

        $(e).parent().children(".panel-control").length;
        var gridContentHeight = responsiveContent2NewHeight;
        _.each($(e).closest(".panel").children(".panel-control"), function (ctr) {
            gridContentHeight -= $(ctr).outerHeight();
        });
        $(e).find(".k-grid .k-grid-content").height(gridContentHeight - gridControlHeight);
    });
    //responsiveContent2.find(".k-grid .k-grid-content").height(responsiveContent2NewHeight - gridControlHeight);
    return responsiveContent2NewHeight;
}

function screenFitChart_2() {
    var responsiveContent2NewHeight = Math.floor((window.innerHeight - 51 - 15) / 2 - 40 - 15);
    var responsiveContent2 = $(".screen-fit-2");

    var responsiveContent2Chart = responsiveContent2.find(".reponsive-chart");
    if (responsiveContent2Chart.length > 0) {
        responsiveContent2Chart.height(responsiveContent2NewHeight);
        var chart = responsiveContent2Chart.data("kendoChart");
        if (chart != undefined) {
            chart.redraw();
        }
        
    }
}

function screenFitChart_1() {
    var responsiveContent1NewHeight = Math.floor(window.innerHeight - 51 - 15 - 40 - 15);
    var fullScreen = document.fullScreen || document.mozFullScreen || document.webkitIsFullScreen;
    if (fullScreen)
        responsiveContent1NewHeight = Math.floor(window.innerHeight - 40);

    var responsiveContent1 = $(".screen-fit-1");

    var responsiveContent1Chart = responsiveContent1.find(".reponsive-chart");
    if (responsiveContent1Chart.length > 0) {
        responsiveContent1Chart.height(responsiveContent1NewHeight);
        var chart = responsiveContent1Chart.data("kendoChart");
        if (chart != undefined) {
            chart.redraw();
        }
    }
}

$(document).ready(function () {
    screenFit_1();
    screenFit_2();

    screenFitGrid_1();
    screenFitGrid_2();
    screenFitChart_1();
    screenFitChart_2();

});

var rtime = new Date(1, 1, 2000, 12, 00, 00);
var timeout = false;
var delta = 200;

function resizeend() {
    if (new Date() - rtime < delta) {
        setTimeout(resizeend, delta);
    } else {
        timeout = false;

        screenFitChart_1();
        screenFitChart_2();
    }
}

$(window).resize(function () {
    rtime = new Date();
    if (timeout === false) {
        timeout = true;
        setTimeout(resizeend, delta);
    }

    screenFit_1();
    screenFit_2();

    screenFitGrid_1();
    screenFitGrid_2();

});