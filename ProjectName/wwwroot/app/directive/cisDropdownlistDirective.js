(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisDropdownlist', cisDropdownlist);
        function cisDropdownlist() {
            return {
                restrict: "E",
                scope: {
                    dropdownId: '@',
                    modelBinding: '=ngModel',
                    modelBindingText: "=",
                    useBindingText: "@"
                },
                require: 'ngModel',
                transclude: true,
                link: function (scope, iElement, iAttrs, ngModel) {
                    scope.$watch("modelBinding", function (n, o) {
                        if (n != o && n != undefined) {
                            if (scope.useBindingText) {
                                var dropdownlist = $("#" + scope.dropdownId).data("kendoDropDownList");
                                scope.modelBindingText = dropdownlist.text();
                            }

                            if (scope.modelBindingText != null && scope.modelBindingText != '' && scope.modelBinding != undefined && scope.modelBinding != null && scope.modelBinding != '') {
                                scope.$broadcast(scope.dropdownId + "_SetValueAndText", { KeyId: scope.modelBinding, DisplayName: scope.modelBindingText });
                            }
                        }
                    }, true);
                },
                bindToController: {
                    dropdownId: '@',
                    urlReadData: '@',
                    enableDropdown: '@',
                    autoBind: "@",
                    placeHolder: "@",
                    placeHolderValue: "@",
                    modelBinding: '=ngModel',
                    modelBindingText: "=",
                    onchangefuc: '=',
                    onDataBoundFuc: '=',
                    parameterDependencies: '@',
                    setFirstValueIsDefault: "@",
                    customParams: '@',
                },
                controller: cisDropdownlistController,
                controllerAs: 'dropdownVm',
                template: function (scope, iElement, iAttrs, ngModel) {

                    if (iElement.placeHolder != undefined && iElement.placeHolder != null && iElement.placeHolder != '') {
                        return '<select  k-ng-model="modelBinding" ' +
                            'k-cascade-from="\'{{dropdownVm.parameterDependencies}}\'" ' + 'k-option-label="{DisplayName:\'{{dropdownVm.placeHolder}}\',KeyId:\'{{dropdownVm.placeHolderValue}}\'}" k-auto-bind="{{dropdownVm.autoBind}}"  k-value-primitive="true" id="{{dropdownVm.dropdownId}}" kendo-drop-down-list k-options="dropdownVm.dropdownlistOptions"></select><input type="hidden" ng-model="dropdownVm.modelBindingText"></input>';
                    } else {
                        return '<select  k-ng-model="modelBinding"' +
                            'k-cascade-from="\'{{dropdownVm.parameterDependencies}}\'" ' + ' k-auto-bind="{{dropdownVm.autoBind}}"  k-value-primitive="true" id="{{dropdownVm.dropdownId}}" kendo-drop-down-list k-options="dropdownVm.dropdownlistOptions"></select><input type="hidden" ng-model="dropdownVm.modelBindingText"></input>';
                    }

                }
            };
        }

        cisDropdownlistController.$inject = ['$rootScope', '$scope', '$timeout'];

        function cisDropdownlistController($rootScope, $scope, $timeout) {
            var ctrl = this;

            ctrl.IsFirstSetValue = true;
            ctrl.getDataSourceDefault = function () {
                var dataBindingText = ctrl.modelBindingText;
                if (ctrl.IsFirstSetValue == true && dataBindingText != null && dataBindingText != '' && ctrl.modelBinding != undefined && ctrl.modelBinding != null && ctrl.modelBinding != '') {

                    return [{ "KeyId": ctrl.modelBinding, "DisplayName": dataBindingText }];
                }
                return null;
            };
            var customParams = [];
            if (ctrl.customParams != undefined && ctrl.customParams != null) {
                customParams = JSON.parse(ctrl.customParams);
            }
            ctrl.modelBindingTextOld = "";
            ctrl.isLoadDataSourceFromServer = false;
            $scope.$on(ctrl.dropdownId + "_ResetValueAndText", function (event, obj) {
                if (ctrl.isLoadDataSourceFromServer == false) {
                    ctrl.modelBinding = obj.KeyId;
                    ctrl.modelBindingText = obj.DisplayName;
                    ctrl.IsFirstSetValue = true;
                    ctrl.dataSource.read();
                }
            });
            $scope.$on(ctrl.dropdownId + "_SetValueAndText", function (event, obj) {
                ctrl.modelBinding = obj.KeyId;
                ctrl.modelBindingText = obj.DisplayName;
                ctrl.IsFirstSetValue = true;
                ctrl.dataSource.read();
            });
            var request;
            //declare datasource
            ctrl.dataSource = new kendo.data.DataSource({
                serverFiltering: true,
                type: 'odata',
                schema: {
                    data: function (data) {
                        return data;
                    },
                    total: function (data) {
                        return data.length;
                    }
                },
                transport: {
                    read: function (options) {
                        var dataSource = ctrl.getDataSourceDefault();
                        if (dataSource != null) {
                            options.success(dataSource);
                        } else {
                            if (request != null) {
                                request.abort();
                            }
                            var data = {
                                Id: ctrl.currentId,
                                IncludeCurrentRecord: ctrl.IncludeCurrentRecord,
                                Query: '',
                                ParameterDependencies: ctrl.parameterDependencies,
                                Take: 100
                            };
                            if (customParams != null && customParams.length != 0) {
                                for (var i = 0; i < customParams.length; i++) {
                                    data[customParams[i]] = $scope.$parent[customParams[i]];
                                }
                            }
                            request = $.ajax({
                                url: ctrl.urlReadData,
                                dataType: "json",
                                type: "GET",
                                data: data,
                                success: function (result) {
                                    options.success(result);
                                    ctrl.isLoadDataSourceFromServer = true;
                                }
                            });
                        }
                    }
                }


            });

            ctrl.dropdownlistOptions = {
                dataSource: ctrl.dataSource,
                dataTextField: 'DisplayName',
                dataValueField: 'KeyId',
                enable: ctrl.enableDropdown === 'true',
                change: onChangeDropdown,
                open: function (e) {
                    var dataBindingText = ctrl.modelBindingText;
                    if (ctrl.IsFirstSetValue == true && ((dataBindingText != null && dataBindingText != '') || (ctrl.modelBinding != undefined && ctrl.modelBinding != null && ctrl.modelBinding != ''))) {
                        ctrl.IsFirstSetValue = false;
                        ctrl.dataSource.read();
                    }

                },
                dataBound: onDataBound

            };
            function onChangeDropdown(e) {
                if (typeof ctrl.onchangefuc == "function") {
                    ctrl.onchangefuc(e, this.value(), this.text(), this.dataItem());
                }

            };
            function onDataBound(e) {
                $timeout(function () {
                    var dropdownlist = $("#" + ctrl.dropdownId).data("kendoDropDownList");
                    if (dropdownlist != undefined && dropdownlist.dataSource.data().length > 0) {
                        if (ctrl.isLoadDataSourceFromServer == false) {
                            dropdownlist.value(ctrl.modelBinding);
                        }
                        if (ctrl.setFirstValueIsDefault == false || ctrl.setFirstValueIsDefault == "false") {
                            return;
                        }
                        if (ctrl.setFirstValueIsDefault && (ctrl.modelBinding == null || ctrl.modelBinding == 0)) {
                            dropdownlist.select(1);
                            ctrl.modelBinding = dropdownlist.value();
                        }
                    }
                });
            }
        }
    });
}());