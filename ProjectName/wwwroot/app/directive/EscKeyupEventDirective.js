(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('escKeyupEvent', escKeyupEventFunction);

        function escKeyupEventFunction() {
            return {
                restrict: "A",
                link: function (scope, element, attrs) {
                    element.bind('keydown', function (event) {
                        if (event.keyCode === 27) {
                            scope.$apply(attrs.escKeyupEvent);
                        }
                    });

                    scope.$on('$destroy', function () {
                        element.unbind('keydown');
                    });
                }
            }
        }
    });
}());