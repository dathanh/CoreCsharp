
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
                $('#myModalMain').html(html);
            });
    }
});
$(".check-all").click(function () {
    if ($(this).is(":checked")) {
        $(this).parent().parent().parent().parent().find("input:checkbox").prop('checked', true);
    } else {
        $(this).parent().parent().parent().parent().find("input:checkbox").prop('checked', false);
    }
});
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    
});
$('.dropdown-menu').find('form').click(function (e) {
    e.stopPropagation();
});

//$('#main-nav').find("a").click(function (index) {
//    $('#main-nav').find("li").removeClass("active");
//    $(this).parent().addClass("active");
//});