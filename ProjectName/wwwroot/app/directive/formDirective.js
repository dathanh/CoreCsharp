(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('form', formDirectiveFunction);
        formDirectiveFunction.$inject = ['$timeout', '$interval'];

        function formDirectiveFunction($timeout, $interval) {
            return {
                restrict: 'E',
                require: ["?form"],
                link: function (scope, element, attrs, formCtrl) {
                    formCtrl[0].$disableForm = function () {
                        var inputs = element.find('input');
                        inputs.attr("disabled", "disabled");
                    };
                    formCtrl[0].$enableForm = function () {
                        $timeout(function () {
                            var inputs = element.find('input');
                            inputs.removeAttr("disabled");
                        }, 80);
                    };
                    if (!element.hasClass('not-focus')) {
                        $timeout(function () {
                            //var firstInput = element.find('.tx-search');
                            //if (firstInput && firstInput.length==0) {
                            var firstInput = element.find('input:not([readonly])[type=password],input:not([readonly])[type=text]').first();
                            //}
                            if (firstInput && firstInput.length > 0) {
                                if (firstInput.attr('k-format') !== undefined) {
                                    return;
                                }
                                var count = 0;
                                var stopTime = $interval(function () {
                                    if (firstInput.is(":focus") || count++>15) {
                                        $interval.cancel(stopTime);
                                        return;
                                    } else {
                                        //console.log(firstInput);
                                        firstInput.get(0).focus();
                                        if (firstInput.is(":focus")) {
                                            $interval.cancel(stopTime);
                                        }
                                    }
                                }, 50);
                            }
                        },150);
                    }

                }
            };
        }
    });
}());