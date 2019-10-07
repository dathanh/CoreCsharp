(function($) {
    var MutationObserver = window.MutationObserver || window.WebKitMutationObserver || window.MozMutationObserver;

    function isDOMAttrModifiedSupported() {
        var p = document.createElement('p');
        var flag = false;

        if (p.addEventListener)
            p.addEventListener('DOMAttrModified', function() {
                flag = true;
            }, false);
        else if (p.attachEvent)
            p.attachEvent('onDOMAttrModified', function() {
                flag = true;
            });
        else return false;

        p.setAttribute('id', 'target');

        return flag;
    }

    $.fn.attrchange = function(callback) {
        if (MutationObserver) {
            var options = {
                subtree: false,
                attributes: true
            };

            var observer = new MutationObserver(function(mutations) {
                mutations.forEach(function(e) {
                    callback.call(e.target, e.attributeName);
                });
            });

            return this.each(function() {
                observer.observe(this, options);
            });

        } else if (isDOMAttrModifiedSupported()) {
            return this.on('DOMAttrModified', function(e) {
                callback.call(this, e.attrName);
            });
        } else if ('onpropertychange' in document.body) {
            return this.on('propertychange', function(e) {
                callback.call(this, window.event.propertyName);
            });
        }
    }
})(jQuery);