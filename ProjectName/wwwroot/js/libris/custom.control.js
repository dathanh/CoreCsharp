$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('.search-box').hover(
        function () {
            $(this).addClass('k-state-hover');
        },
        function () {
            $(this).removeClass('k-state-hover');
        }
        );

    $('.search-box').click(
        function () {
            $(this).addClass('k-state-focused');
        });
    $('.search-box').focusout(function () {
        $(this).removeClass('k-state-focused');
    });
});

createScrollBarForGrid = function (id) {
    $('#' + id + ' .k-grid-header').css('padding-right', 0);
    var elContent = $('#' + id + ' .k-grid-content');
    elContent.css('overflow-y', 'hidden');
    elContent.slimScroll({
        size: '6px',
        color: '#555',
        railVisible: true,
        railColor: '#555',
        railOpacity: 0.1,
        wheelStep: 1,
        //height: 'auto'// elContent.css('height')
    });
    $("#" + id).find(".slimScrollDiv").height('auto');
}

createScrollBarForList = function (id) {
    $('#' + id).slimScroll({
        size: '6px',
        color: '#555',
        railVisible: true,
        railColor: '#555',
        railOpacity: 0.1,
        wheelStep: 1,
        height: '100%'
    });
}