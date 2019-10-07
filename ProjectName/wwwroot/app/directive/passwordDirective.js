(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisPassword', passwordDirectiveFunction);

        function passwordDirectiveFunction() {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    ngModel: '=',
                    name: '='
                },
                template: '' +
                    '<div>' +
                    '    <input type="text" ng-model="ngModel" name="name" ng-minlength="8" ng-maxlength="20" required />' +
                    '    <input type="password" ng-model="ngModel" name="name" ng-minlength="ngMinlength" required />' +
                    '    <input type="checkbox" ng-model="viewPasswordCheckbox" />' +
                    '</div>',
                link: function (scope, element, attrs) {
                    scope.$watch('viewPasswordCheckbox', function (newValue) {
                        var show = newValue ? 1 : 0,
                            hide = newValue ? 0 : 1,
                            inputs = element.find('input');
                        inputs[show].value = inputs[hide].value;
                        inputs[hide].style.display = 'none';
                        inputs[show].style.display = '';
                    });
                }
            };
        }
    });
}());