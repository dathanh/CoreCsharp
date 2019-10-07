(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisDatetimePicker', cisDatetimePickerDirective);
        cisDatetimePickerController.$inject = ['$timeout', 'common', '$rootScope', '$parse'];
        function cisDatetimePickerDirective($timeout, common, $rootScope, $parse) {
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
                controller: cisDatetimePickerController,
                controllerAs: 'pickerVm',
                template: '<input id="{{pickerVm.pickerId}}" kendo-date-time-picker k-format="\'{{pickerVm.pickerFormat}}\'" k-ng-readonly="\'{{pickerVm.pickerReadonly}}\'" k-ng-model="pickerModelBinding" placeholder="{{pickerVm.pickerPlaceHolder}}" style="width:100%" />',
                link: function (scope, el, attrs, ngModel) {
                    ngModel.$formatters.push(function (value) {
                        var date;
                        if (value != undefined && value != null) {
                            if (Object.prototype.toString.call(value) === "[object Date]") {
                                date = value;
                            } else {
                                date = new Date(value);
                            }
                        } 
                        $timeout(function () {
                            var datepicker = $("#" + attrs.pickerId);
                            var modalview = datepicker.data("kendoDateTimePicker");
                            modalview.value(date);
                            var toggleOrigin = modalview.toggle;
                            modalview.toggle = function (e) {
                                toggleOrigin.call(modalview, e);
                                var calender = modalview['dateView'].calendar;
                                if (calender) {
                                    var todayClickOrigin = calender._todayClick;
                                    calender._today.off();
                                    calender._today.on("click", function (e) {
                                        //e.preventDefault();
                                        //console.log(ngModel)
                                        todayClickOrigin.call(calender, e);
                                        $timeout(function() {
                                            modalview.value(new Date());
                                            ngModel.$setViewValue(new Date());
                                            ngModel.$render();
                                        })
                                       
                                    });
                                }
                            }

                            //masked
                            datepicker.kendoMaskedTextBox({
                                mask: "00/00/0000 00:00 ~^",
                                rules: {
                                    "~": function (char) {
                                        return char === "A" || char === "P" || char === "a" || char === "p";
                                    },
                                    "^": function (char) {
                                        return char === "M" || char === "m"; //allow ony "^" symbol
                                    }
                                },
                                change: function () {
                                    var v = this.value();
                                    if (!common.isValidDatetime(v)) {
                                        datepicker.data("kendoDateTimePicker").value(null);
                                        ngModel.$setViewValue(null);
                                        ngModel.$render();
                                        $rootScope.popupFormModified = true;
                                    }
                                }
                            });

                            datepicker.closest(".k-datepicker")
                                .add(datepicker)
                                .removeClass("k-textbox")
                            .addClass("p0");
                        });
                        if (date!=undefined) {
                                return date;
                        } else {
                            return null;
                        }
                        //var date = new Date(value);
                        //$timeout(function () {
                        //    var datepicker = $("#" + attrs.pickerId);
                        //    if (value !== null) {
                        //        datepicker.data("kendoDateTimePicker").value(date);
                                
                        //    }
                        //});
                        //return date;

                    });
                    
                }
            };
        }

        cisDatetimePickerController.$inject = ['$rootScope', '$scope'];

        function cisDatetimePickerController($rootScope, $scope) {
            var ctrl = this;
            //console.log(ctrl);
        }
    });
}());