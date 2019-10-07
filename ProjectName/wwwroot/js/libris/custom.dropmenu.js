function toggleDropMenu(id) {
    var requestCard = $(id);
    if (!requestCard) { return; }

    requestCard.slideToggle();
};

function toggleCornerMenu(id, btnId) {
    var requestCard = $(id);
    var elementOffset = $(btnId).offset();
    var parentOffset = requestCard.parent().offset();
    
    var newX = elementOffset.left - parentOffset.left;
    var newY = elementOffset.top - parentOffset.top + $(btnId).outerHeight();
    
    if (newX + requestCard.outerWidth() > $(document).innerWidth()) {
        newX = $(document).innerWidth() - requestCard.outerWidth();
    }
    
    requestCard.css({
        "left": newX,
        "top": newY
    });
    
    requestCard.slideToggle();
};

function hideDropMenu(id) {
    var requestCard = $(id);
    if (!requestCard) return;
    if (requestCard.is(":visible")) {
        requestCard.slideUp();
    }
};

function hideCornerMenu(id) {
    var requestCard = $(id);
    if (!requestCard) return;
    if (requestCard.is(":visible")) {
        requestCard.slideUp(400);
    }
};

function dropMenuRegister(id, btn) {
    $(".container").click(function (e) {
        var bt = $(btn);
        if (bt.is(e.target)
            || bt.has(e.target).length != 0) { return; }
        var dropMenu = $(id);

        if (!dropMenu || !dropMenu.is(":visible")) { return; }

        if (!dropMenu.is(e.target) // if the target of the click isn't the container...
            && dropMenu.has(e.target).length == 0) // ... nor a descendant of the container
        {
            dropMenu.slideUp();
        }
    });
};

function cornerMenuRegister(id, btn) {
    $(".container").click(function (e) {
        var bt = $(btn);
        if (bt.is(e.target)
            || bt.has(e.target).length != 0) { return; }
        var menu = $(id);

        if (!menu || !menu.is(":visible")) { return; }

        if (!menu.is(e.target) // if the target of the click isn't the container...
            && menu.has(e.target).length == 0) // ... nor a descendant of the container
        {
            menu.slideUp(400);
        }
    });
};