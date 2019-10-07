(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisMultiSelect', cisMultiSelectDirective);

        function cisMultiSelectDirective() {
            return {
                restrict: "E",
                scope: {
                    multiselectId: '@',
                    modelBinding: '=ngModel',
                    modelBindingText: "="
                },
                require: 'ngModel',
                link: function (scope, iElement, iAttrs, ngModel) {
                    scope.$watch("modelBinding", function (n, o) {
                        if (n != o && n != undefined) {
                            if (scope.modelBindingText != null && scope.modelBindingText != '' && scope.modelBinding != undefined && scope.modelBinding != null &&
                                scope.modelBinding != '' && scope.modelBinding.length > 0 && scope.modelBindingText.length > 0 && scope.modelBinding.length == scope.modelBindingText.length) {
                                scope.$broadcast(scope.multiselectId + "_ResetValueAndText", { KeyId: scope.modelBinding, DisplayName: scope.modelBindingText });
                            }
                        }
                    }, true);
                },
                transclude: true,
                bindToController: {
                    multiselectId: '@',
                    urlReadData: '@',
                    parameterDependencies: '@',
                    enableMultiselect: '@',
                    placeHolderText: '@',
                    showSelectall: '@',
                    isDisplayOnTop: '@',
                    onchangeDatasourceFuc: '=',
                    modelBinding: '=ngModel',
                    modelBindingText: "="
                },
                controller: cisMultiSelectController,
                controllerAs: 'multiselectVm',
                template:
                    '<select k-value-primitive="true" k-ng-model="modelBinding" id="{{multiselectVm.multiselectId}}" class="hide-selected" kendo-multi-select ' +
                    'k-options="multiselectVm.multiselectOptions" /><input type="hidden" ng-model="multiselectVm.modelBindingText"></input>' +
                    '<div><button ng-show={{multiselectVm.showSelectall}} class="k-button" ng-click="multiselectVm.OnSelectAll(kendoEvent)">Select All</button>' +
                    '<button ng-show={{multiselectVm.showSelectall}} class="k-button" ng-click="multiselectVm.OnDeselect(kendoEvent)">Deselect All</button></div>'
            };
        }

        cisMultiSelectController.$inject = ['$rootScope', '$scope', '$timeout'];

        function cisMultiSelectController($rootScope, $scope, $timeout) {
            var ctrl = this;
            //declare default value
            ctrl.IsFirstSetValue = true;
            ctrl.getDataSourceDefault = function () {
                var dataBindingText = ctrl.modelBindingText;
                if (ctrl.IsFirstSetValue == true && dataBindingText != null && dataBindingText != '' && ctrl.modelBinding != undefined && ctrl.modelBinding != null && ctrl.modelBinding != '' &&
                    ctrl.modelBinding.length > 0 && dataBindingText.length > 0 && ctrl.modelBinding.length == dataBindingText.length) {
                    var arr = [];
                    for (var i = 0; i < ctrl.modelBinding.length; i++) {
                        arr.push({ "KeyId": ctrl.modelBinding[i], "DisplayName": dataBindingText[i] });
                    }
                    ctrl.IsFirstSetValue = false;
                    ctrl.IsFirstOpen = false;
                    return arr;
                }
                return null;
            };
            ctrl.isSelectEvent = false;
            $scope.$on(ctrl.multiselectId + "_ResetValueAndText", function (event, obj) {
                if (ctrl.isSelectEvent == false) {
                    ctrl.modelBinding = obj.KeyId;
                    ctrl.modelBindingText = obj.DisplayName;
                    ctrl.IsFirstSetValue = true;
                    ctrl.dataSource.read();
                }
                
            });
            var request;
            ctrl.QueryString = "";
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
                            request = $.ajax({
                                url: ctrl.urlReadData,
                                dataType: "json",
                                type: "GET",
                                data: {
                                    Id: 0,
                                    IncludeCurrentRecord: true,
                                    Query: ctrl.QueryString,
                                    ParameterDependencies: ctrl.parameterDependencies,
                                    Take: 500000,
                                    IncludeIds: JSON.stringify(ctrl.modelBinding)
                                },
                                success: function (result) {
                                    options.success(result);
                                }
                            });
                        }
                    },
                    parameterMap: function (options, type) {
                        var value = '';
                        if (options.filter !== undefined && options.filter.filters != undefined && options.filter.filters[0] != undefined && options.filter.filters[0].value !== '') {
                            value = options.filter.filters[0].value;
                        }
                        var result = {
                            Id: 0,
                            IncludeCurrentRecord: true,
                            Query: value,
                            ParameterDependencies: ctrl.parameterDependencies,
                            Take: 500000,
                            IncludeIds: JSON.stringify(ctrl.modelBinding)
                        };
                        return result;
                    },

                },
                requestStart: function(e) {
                }
            });
            ctrl.IsFirstOpen = true;
            ctrl.multiselectOptions = {
                filter: "contains",
                autoBind: false,
                placeholder: ctrl.placeHolderText,
                dataSource: ctrl.dataSource,
                dataTextField: 'DisplayName',
                dataValueField: 'KeyId',
                autoClose: false,
                delay: 200,
                enable: ctrl.enableMultiselect === 'true',
                itemTemplate: kendo.template("<span class='multiselect-item'>#: DisplayName #</span>"),
                //valuePrimitive: true,
                open: function(e) {
                    ctrl.positionDropDown(e);
                    if (ctrl.IsFirstOpen == false && ctrl.isFiltering == false) {
                        ctrl.dataSource.read();
                    }
                    ctrl.IsFirstOpen = false;
                },
                dataBound: function(e) {
                    if (typeof ctrl.onchangeDatasourceFuc == "function") {
                        ctrl.onchangeDatasourceFuc(e);
                    }
                    ctrl.positionDropDown(e);
                    $timeout(function () {
                        var multiSelect = $("#" + ctrl.multiselectId).data("kendoMultiSelect");
                        if (multiSelect != undefined && multiSelect.dataSource.data().length > 0) {
                            if (ctrl.isSelectEvent == false) {
                                multiSelect.value(ctrl.modelBinding);
                            }
                        }
                        ctrl.QueryString = "";
                    });
                    ctrl.isFiltering = false;
                    $(".multiselect-item").parent().parent().addClass("hide-selected");
                },
                select: onSelect,
                filtering: onFiltering,
                change: function(e) {
                    ctrl.positionDropDown(e);
                },
                close: function() {
                    ctrl.isSelectEvent = false;
                    ctrl.QueryString = "";
                }
        };
            ctrl.isFiltering = false;
            function onFiltering(e) {
                if (e.filter != undefined) {
                    ctrl.isFiltering = true;
                    ctrl.isSelectEvent = true;
                    $scope.$apply(function () {
                        ctrl.QueryString = e.filter.value;
                    });
                }
            }
            function onSelect(e) {
                ctrl.QueryString = "";
                ctrl.isSelectEvent = true;
                ctrl.modelBindingText = null;
                $scope.$apply();
            }
            ctrl.positionDropDown = function (e) {
                var ddl = e.sender;
                var f = ddl.popup.wrapper.find('.k-list-container');
                var h = f.height();
                ddl.popup.wrapper.height(h);
                if (ctrl.isDisplayOnTop && ctrl.isDisplayOnTop == "true")
                    ddl.popup.wrapper.css({ top: ddl.wrapper.offset().top - h - 5 });
            }

            ctrl.OnSelectAll = function () {
                if (!ctrl.showSelectall || ctrl.showSelectall == "false") return;
                var required = $("#" + ctrl.multiselectId).data("kendoMultiSelect");
                var values = $.map(required.dataSource.data(), function (dataItem) {
                    return dataItem.KeyId;
                });

                $scope.modelBinding = values;
            };

            ctrl.OnDeselect = function () {
                if (!ctrl.showSelectall || ctrl.showSelectall == "false") return;
                $scope.modelBinding = [];
            };
        }
    });
}());