(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('charactersAndNumbersOnly', enterDirectiveFunction);

        function enterDirectiveFunction() {
            return {
                require: 'ngModel',
                link: function (scope, element, attr, ngModelCtrl) {
                    function fromUser(text) {
                        if (text) {
                            var transformedInput = text.replace(/[^0-9a-zA-Z]+/g, '');

                            if (transformedInput !== text) {
                                ngModelCtrl.$setViewValue(transformedInput);
                                ngModelCtrl.$render();
                            }
                            return transformedInput;
                        }
                        return undefined;
                    }
                    ngModelCtrl.$parsers.push(fromUser);
                }
            };
        }
    });
}());