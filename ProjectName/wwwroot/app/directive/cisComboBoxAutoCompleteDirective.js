(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisComboboxautocomplete', cisComboboxautocomplete);

        function cisComboboxautocomplete() {
            return {
                restrict: "E",
                scope: {
                    modelBinding: '=ngModel',
                },
                require: 'ngModel',
                transclude: true,
                link: function (scope, elem, attrs) {
                    elem.find('select').attr('id', attrs.dropdownId);
                },
                bindToController: {
                    dropdownId: '@',
                    urlReadData: '@',
                    enableDropdown: '@',
                    modelBinding: '=ngModel',
                    modelBindingText: "=",
                    parameterDependencies: '@',
                    autoBind: "@",
                    placeHolder: "@",
                    onchangefuc: '=',
                    isReadonly: '@',
                    customParams: '@',
                    isJustAutoComplete: '@'
                },
                controller: cisComboboxautocompleteController,
                controllerAs: 'dropdownVm',
                template: '<select kendo-combo-box k-ng-model="modelBinding" k-auto-bind="{{dropdownVm.autoBind}}" k-value-primitive="true" ' +
                'id="{{dropdownVm.dropdownId}}" ng-readonly="{{dropdownVm.isReadonly}}"' +
                'k-cascade-from="\'{{dropdownVm.parameterDependencies}}\'" ' +
                'k-options="dropdownVm.dropdownlistOptions">' +
                '</select><input type="hidden" ng-model="dropdownVm.modelBindingText"></input>',
            };
        }

        cisComboboxautocompleteController.$inject = ['$rootScope', '$scope', '$timeout'];

        function cisComboboxautocompleteController($rootScope, $scope, $timeout) {
            var ctrl = this;
            $timeout(function () {
                $("[data-role=combobox]").each(function () {
                    var widget = $(this).getKendoComboBox();
                    widget.input.on("focus", function () {
                        widget.input.select();
                    });
                });
            });
            var customParams = [];
            if (ctrl.customParams != undefined && ctrl.customParams != null) {
                customParams = JSON.parse(ctrl.customParams);
            }
            ctrl.IsFirstSetValue = true;
            ctrl.IncludeCurrentRecord = true;
            ctrl.QueryString = "";
            ctrl.getDataSourceDefault = function () {
                var dataBindingText = ctrl.modelBindingText;// angular.element("#" + ctrl.lookupId + "_TextDefaultHidden").html();
                if (ctrl.IsFirstSetValue == true && dataBindingText != null && dataBindingText != '' && ctrl.modelBinding > 0) {
                    ctrl.IsFirstSetValue = false;
                    return [{ "KeyId": ctrl.modelBinding, "DisplayName": dataBindingText }];
                }
                return null;
            };
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
                                Query: ctrl.QueryString,
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
                                    if (ctrl.modelBinding == 0 && $('#' + ctrl.dropdownId).data("kendoComboBox") != null) {
                                        $('#' + ctrl.dropdownId).data("kendoComboBox").text("");
                                    }

                                }
                            });
                        }
                    },
                    parameterMap: function (options, type) {
                        var value = $('#' + ctrl.dropdownId).data("kendoComboBox").text();
                        // if (options.filter !== undefined && options.filter.filters != undefined && options.filter.filters[0] != undefined && options.filter.filters[0].value !== '') {
                        //     value = options.filter.filters[0].value;
                        // }

                        var result = {
                            Id: ctrl.currentId,
                            IncludeCurrentRecord: true,
                            Query: value,
                            ParameterDependencies: ctrl.parameterDependencies,
                            Take: 100
                        };
                        return result;
                    }

                }
            });
            ctrl.dropdownlistOptions = {
                placeholder: ctrl.placeHolder,
                filter: "contains",
                dataSource: ctrl.dataSource,
                dataTextField: 'DisplayName',
                dataValueField: 'KeyId',
                enable: ctrl.enableDropdown === 'true',
                delay: 200,
                //minLength:1,
                change: onChangeDropdown,
                select: onSelect,
                open: function (e) {
                    if (ctrl.isFiltering == false) {
                        ctrl.dataSource.read();
                    }

                },
                dataBound: onDataBound,
                filtering: onFiltering
            };
            ctrl.isFiltering = false;
            function onFiltering(e) {
                if (e.filter != undefined) {
                    ctrl.isFiltering = true;
                    $scope.$apply(function () {
                        ctrl.QueryString = e.filter.value;
                    });
                }
            }
            function onChangeDropdown(e) {

                ctrl.QueryString = "";
                if (typeof ctrl.onchangefuc == "function") {
                    ctrl.onchangefuc(e, this.value(), this.text());
                }

            }

            function onSelect(e) {
                ctrl.QueryString = "";
                $scope.$apply();
            }

            function onDataBound(e) {
                if (ctrl.IncludeCurrentRecord == false && ctrl.currentId > 0) {
                    ctrl.lookupModelBinding = ctrl.currentId;
                    ctrl.currentId = 0;
                }
                ctrl.IncludeCurrentRecord = true;
                ctrl.IsFirstSetValue = false;
                ctrl.isFiltering = false;
                if (ctrl.isJustAutoComplete == false || ctrl.isJustAutoComplete == "false") {
                    $('#' + ctrl.dropdownId).focusout(function () {
                        if ((ctrl.modelBinding == 0 || ctrl.modelBinding == null) && $('#' + ctrl.dropdownId).data("kendoComboBox") != null) {
                            $('#' + ctrl.dropdownId).data("kendoComboBox").text("");
                        }
                    });
                }

            }


        }


    });
}());