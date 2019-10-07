(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisTimePicker', cisTimePickerDirective);
        cisTimePickerDirective.$inject = ['$timeout', 'common', '$parse'];
        function cisTimePickerDirective($timeout, common, $parse) {
            return {
                restrict: "E",
                scope: {
                    pickerModelBinding: '=ngModel'
                },
                require: 'ngModel',
                //replace: true,
                transclude: true,
                bindToController: {
                    pickerId: '@',
                    pickerFormat: '@',
                    pickerReadonly: '@',
                    pickerPlaceHolder: '@',
                    pickerModelBindingValue: '=ngModel'
                },
                controller: cisTimePickerController,
                controllerAs: 'pickerVm',
                template: '<input style=\'width:100%\' id="{{pickerVm.pickerId}}" kendo-time-picker k-format="\'{{pickerVm.pickerFormat}}\'" k-ng-readonly="\'{{pickerVm.pickerReadonly}}\'" k-ng-model="pickerModelBinding"  placeholder="{{pickerVm.pickerPlaceHolder}}" />',
                link: function (scope, el, attrs, ngModel) {
                    ngModel.$formatters.push(function (value) {
                        var d = new Date();
                        var date;

                        $timeout(function () {
                            var timepicker = $("#" + attrs.pickerId);
                            timepicker.data("kendoTimePicker").value(date);
                            //console.log(timepicker.data("kendoTimePicker").value());
                            //masked
                            timepicker.kendoMaskedTextBox({
                                mask: "00:00 ~^",
                                rules: {
                                    "~": /[PA]/,
                                    "^": /[M]/
                                },
                                change: function () {
                                    var v = this.value();
                                    if (!common.isValidTime(v)) {
                                        ngModel.$setViewValue(null);
                                        timepicker.data("kendoTimePicker").value("");
                                    }
                                }
                            });

                            timepicker.closest(".k-timepicker")
                                .add(timepicker)
                                .removeClass("k-textbox")
                            .addClass("p0");
                        });
                        if (value !== null) {
                            if (common.isValidTime(value)) {

                                date = new Date((d.getMonth()+1)+"/"+ d.getDate()+"/"+d.getFullYear()+" "+ value);

                                return date;
                            } else {
                                if (common.isValidDate(common.formatDate(new Date(value)))) {
                                    date = new Date(value);

                                    date.setYear(d.getFullYear());
                                    date.setMonth(date.getMonth());
                                    date.setDate(date.getDate());

                                    return date;
                                } else {
                                    return null;
                                }
                            }
                        } else {
                            return null;
                        }
                    });
                }
            };
        }

        cisTimePickerController.$inject = ['$rootScope', '$scope', '$timeout'];

        function cisTimePickerController($rootScope, $scope, $timeout) {
            var ctrl = this;
        }
    });
}());