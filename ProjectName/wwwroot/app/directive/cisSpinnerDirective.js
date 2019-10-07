(function () {
    'use strict';

    define(['angularAMD', 'spinnerlib'], function (angularAmd, spinner) {

        angularAmd.directive('cisSpinner', directive);

        directive.$inject = ['$window'];

        function directive() {
            return {
                restrict: 'A',
                link: link
            }

            function link(scope, element, attrs) {
                scope.spinner = null;
                scope.$watch(attrs.ccSpinner, function (options) {
                    if (scope.spinner) {
                        scope.spinner.stop();
                    }
                    scope.spinner = new spinner(options);
                    scope.spinner.spin(element[0]);
                }, true);
            }
        }
    });
}());