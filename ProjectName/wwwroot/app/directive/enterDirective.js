(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('ngEnter', enterDirectiveFunction);

        function enterDirectiveFunction() {
            return function (scope, element, attrs) {
                element.bind("keyup", function (event) {
                    if (event.which === 13) {
                        scope.$apply(function () {
                            scope.$eval(attrs.ngEnter);
                        });

                        event.preventDefault();
                    }
                });
            };
        }
    });
}());