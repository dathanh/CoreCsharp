(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisLookup', cisLookup);

        cisLookup.$inject = ['$timeout'];

        function cisLookup($timeout) {
            return {
                restrict: "E",
                scope: {
                    lookupId: '@',
                    lookupModelBinding: '=ngModel',
                    modelBindingText: "=",
                    isNonResetValueWhenSelect: "="
                },
                require: 'ngModel',
                transclude: true,
                link: function (scope, element, attrs, ngModel) {
                    scope.$watch("lookupModelBinding", function (n, o) {
                        if (n != o && n != undefined) {
                            if (scope.modelBindingText != null && scope.modelBindingText != '' && scope.lookupModelBinding > 0 && scope.isNonResetValueWhenSelect != true) {
                                scope.$broadcast(scope.lookupId + "_SetValueAndText", { KeyId: scope.lookupModelBinding, DisplayName: scope.modelBindingText });
                            }
                        }
                    }, true);
                },
                bindToController: {
                    lookupId: '@',
                    currentId: '@',
                    urlReadData: '@',
                    modelName: '@',
                    lookupModelBinding: '=ngModel',
                    modelBindingText: "=",
                    showEdit: '@',
                    showAddEdit: '@',
                    showAdvancedSearch: '@',
                    enableLookup: '@',
                    placeHolderText: '@',
                    showAddPopupLookup: "&addPopupLookup",
                    showEditPopupLookup: "&editPopupLookup",
                    showAdvancedSearchPopupLookup: "&advancedSearchPopupLookup",
                    hierarchyKeySend: '@',
                    hierarchyKeyListen: '@',
                    parameterDependencies: '@',
                    autoBind: "@",
                    isNonResetValueWhenSelect: "="
                },
                controller: cisLookupController,
                controllerAs: 'lookupVm',
                template:
                    '<label class="tool-lookup">' +
                    '<button class="btn btn-default mr0 mb0" type="button"  ng-show ="lookupVm.showAddEdit&&!lookupVm.showEdit"  ng-click ="lookupVm.AddLookup()"  data-toggle="tooltip" data-placement="bottom" title="Tạo mới"><i class="fa fa-plus"></i></button>' +
                    '<button class="btn btn-default mr0 mb0" type="button"  ng-show ="lookupVm.showAddEdit&&lookupVm.showEdit"  ng-click ="lookupVm.EditLookup()" data-toggle="tooltip" data-placement="bottom" title="Chỉnh sửa"><i class="fa fa-pencil"></i></button>' +
                    '<button class="btn btn-default mr0 mb0" type="button"  ng-show ="lookupVm.showAdvancedSearch"  ng-click ="lookupVm.AdvancedSearchLookup()" data-toggle="tooltip" data-placement="bottom" title="Tìm kiếm nâng cao"><i class="fa fa-search"></i></button>' +
                    '</label>' +
                    '<select k-ng-model="lookupVm.lookupModelBinding" k-value-primitive="true" k-auto-bind="{{lookupVm.autoBind}}"  id="{{lookupVm.lookupId}}" kendo-combo-box="{{lookupVm.lookupId}}" k-options="lookupVm.lookupOptions"> </select><input type="hidden" ng-model="lookupVm.modelBindingText"></input>'
            };
        }

        cisLookupController.$inject = ['$rootScope', '$scope', '$timeout', '$q'];

        function cisLookupController($rootScope, $scope, $timeout, $q) {
            var ctrl = this;
            //declare default value
            var modelUrl = "";
            //declare datasource
            if (ctrl.urlReadData != undefined && ctrl.urlReadData !== "") {
                modelUrl = ctrl.urlReadData;
            } else {
                modelUrl = "/" + ctrl.modelName + "/GetLookup";
            }

            var parentItems = new Array();
            ctrl.isFirstOpenUpdate = false;
            $timeout(function () {
                if (ctrl.hierarchyKeySend != undefined && ctrl.hierarchyKeySend != null && ctrl.hierarchyKeySend !== '' && ctrl.lookupModelBinding != null) {
                    $rootScope.$broadcast(ctrl.hierarchyKeySend, ctrl.lookupModelBinding, true);
                }
                $scope.$watch(function () {
                    if (ctrl.lookupModelBinding != undefined && ctrl.lookupModelBinding != "") {
                        ctrl.showEdit = ctrl.lookupModelBinding != null;
                        if (ctrl.hierarchyKeySend != undefined && ctrl.hierarchyKeySend != null && ctrl.hierarchyKeySend !== '' && parseInt(ctrl.lookupModelBinding) > 0 && ctrl.isFirstOpenUpdate == false) {
                            ctrl.isFirstOpenUpdate = true;
                            $rootScope.$broadcast(ctrl.hierarchyKeySend, ctrl.lookupModelBinding, false);
                        }
                    }
                });
                $("[data-role=combobox]").each(function () {
                    var widget = $(this).getKendoComboBox();
                    widget.input.on("focus", function () {
                        widget.input.select();
                    });
                });
            });


            $scope.$on(ctrl.lookupId + "_SetValue", function (event, id) {
                ctrl.currentId = id;
                ctrl.IncludeCurrentRecord = false;
                ctrl.dataSource.read();
            });
            $scope.$on(ctrl.lookupId + "_SetValueAndText", function (event, obj) {
                ctrl.lookupModelBinding = obj.KeyId;
                ctrl.modelBindingText = obj.DisplayName;
                ctrl.IsFirstSetValue = true;
                ctrl.dataSource.read();
            });
            $scope.$on(ctrl.lookupId + "_SetNull", function (event) {
                $rootScope.safeApply(function () {
                    ctrl.lookupModelBinding = null;
                    ctrl.modelBindingText = "";
                    ctrl.IsFirstSetValue = true;
                    //ctrl.dataSource.read();
                    ctrl.QueryString = "";
                });
            });
            ctrl.IsFirstSetValue = true;
            ctrl.IsFirstOpen = true;
            ctrl.IncludeCurrentRecord = true;
            ctrl.QueryString = "";
            ctrl.getDataSourceDefault = function () {
                var dataBindingText = ctrl.modelBindingText;
                if (ctrl.IsFirstSetValue == true && dataBindingText != null && dataBindingText != '' && ctrl.lookupModelBinding > 0) {
                    ctrl.IsFirstSetValue = false;
                    ctrl.IsFirstOpen = false;
                    return [{ "KeyId": ctrl.lookupModelBinding, "DisplayName": dataBindingText }];
                }
                return null;
            };
            var request;
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
                requestStart: function (e) {
                    var combobox = $("#" + ctrl.lookupId).data("kendoComboBox");
                    if (combobox != undefined) {
                        var ul = combobox.ul;
                        if (ul != undefined && ul.length > 0) {
                            for (var i = 0; i < ul.length; i++) {
                                $(ul[i]).find("li").remove();
                            }
                        }
                    }

                },
                requestEnd: function (e) {
                },
                transport: {
                    read: function (options) {
                        var dataSource = ctrl.getDataSourceDefault();
                        if (dataSource != null) {
                            options.success(dataSource);
                        } else {
                            //return {
                            //    url: modelUrl,
                            //    dataType: "json",
                            //    type: "GET"
                            //};
                            if (request != null) {
                                request.abort();
                            }
                            request = $.ajax({
                                url: modelUrl,
                                dataType: "json",
                                type: "GET",
                                data: {
                                    Id: ctrl.currentId,
                                    IncludeCurrentRecord: ctrl.IncludeCurrentRecord,
                                    Query: ctrl.QueryString,
                                    ParameterDependencies: ctrl.parameterDependencies,
                                    Take: 50,
                                    ParentItems: JSON.stringify(parentItems)
                                },
                                success: function (result) {
                                    options.success(result);
                                }
                            });
                        }
                    },
                    parameterMap: function (options, type) {
                        if (type == 'read') {
                            return kendo.data.transports["odata"].parameterMap(data, type);
                        } else {
                            var value = '';
                            if (options.filter !== undefined && options.filter.filters != undefined && options.filter.filters[0] != undefined && options.filter.filters[0].value !== '') {
                                value = options.filter.filters[0].value;
                            }

                            var result = {
                                Id: ctrl.currentId,
                                IncludeCurrentRecord: ctrl.IncludeCurrentRecord,
                                Query: value,
                                ParameterDependencies: ctrl.parameterDependencies,
                                Take: 50,
                                ParentItems: JSON.stringify(parentItems)
                            };
                            return result;
                        }
                    }
                }
            });

            ctrl.lookupOptions = {
                placeholder: ctrl.placeHolderText,
                dataSource: ctrl.dataSource,
                dataTextField: 'DisplayName',
                dataValueField: 'KeyId',
                filter: 'contains',
                enable: ctrl.enableLookup === 'true',
                delay: 200,
                change: onChanged,
                select: onSelect,
                open: function (e) {
                    if (ctrl.IsFirstOpen == false && ctrl.isFiltering == false) {
                        ctrl.dataSource.read();
                    }

                    ctrl.IsFirstOpen = false;
                },
                dataBound: onDataBound,
                filtering: onFiltering
            };

            ctrl.AddLookup = function () {
                if (ctrl.showAddPopupLookup != undefined && ctrl.showAddPopupLookup !== '') {
                    ctrl.showAddPopupLookup();
                }
            };

            ctrl.EditLookup = function () {
                if (ctrl.showEditPopupLookup != undefined && ctrl.showEditPopupLookup !== '') {
                    ctrl.showEditPopupLookup();
                }
            };

            ctrl.AdvancedSearchLookup = function () {
                if (ctrl.showAdvancedSearchPopupLookup != undefined && ctrl.showAdvancedSearchPopupLookup !== '') {
                    ctrl.showAdvancedSearchPopupLookup();
                }
            };
            //events
            ctrl.isFiltering = false;
            function onFiltering(e) {
                if (e.filter != undefined) {
                    ctrl.isFiltering = true;
                    $scope.$apply(function () {
                        ctrl.QueryString = e.filter.value;
                    });
                }
            }
            function onChanged(e) {
                var widget = e.sender;
                if (widget.value() && widget.select() === -1) {
                    $scope.$apply(function () {
                        ctrl.lookupModelBinding = null;
                    });
                }

                if (ctrl.lookupModelBinding != null) {
                    ctrl.showEdit = true;
                } else {
                    ctrl.showEdit = false;
                }

                //hierarchy send
                if (ctrl.hierarchyKeySend != undefined && ctrl.hierarchyKeySend != null && ctrl.hierarchyKeySend !== '') {
                    $rootScope.$broadcast(ctrl.hierarchyKeySend, ctrl.lookupModelBinding, true);
                }
                //if (ctrl.dataSource != undefined && ctrl.dataSource._filter != undefined && ctrl.dataSource._filter.filters != undefined &&
                //    ctrl.dataSource._filter.filters.length > 0 && ctrl.dataSource._filter.filters[0] != undefined) {
                //    ctrl.dataSource._filter.filters[0].value = "";
                //}

                //ctrl.QueryString = "";
                $scope.$digest();
            }

            function onSelect(e) {

                var dataItem = this.dataItem(e.item.index());
                ctrl.lookupModelBinding = dataItem.KeyId;
                ctrl.QueryString = "";
                //hierarchy send
                if (ctrl.hierarchyKeySend != undefined && ctrl.hierarchyKeySend != null && ctrl.hierarchyKeySend !== '') {

                    $rootScope.$broadcast(ctrl.hierarchyKeySend, ctrl.lookupModelBinding, true);
                }
                //if (ctrl.dataSource != undefined && ctrl.dataSource._filter != undefined && ctrl.dataSource._filter.filters != undefined &&
                //   ctrl.dataSource._filter.filters.length > 0 && ctrl.dataSource._filter.filters[0] != undefined) {
                //    ctrl.dataSource._filter.filters[0].value = "";
                //}
                $scope.$apply();
            }

            function onDataBound(e) {
                if (ctrl.IncludeCurrentRecord == false && ctrl.currentId > 0) {
                    ctrl.lookupModelBinding = ctrl.currentId;
                    ctrl.currentId = 0;
                }
                //if (ctrl.lookupModelBinding == null || ctrl.lookupModelBinding == 0) {
                //    $timeout(function() {
                //        $("#lookup-container-" + ctrl.lookupId).find(".k-input").val("");
                //    });
                //}
                ctrl.IncludeCurrentRecord = true;
                ctrl.IsFirstSetValue = false;
                ctrl.isFiltering = false;
                $('[data-toggle="tooltip"]').tooltip({
                    container: 'body'
                });
            }

            //hierarchy listen
            if (ctrl.hierarchyKeyListen != null) {
                $scope.$on(ctrl.hierarchyKeyListen, function (event, value, isClearModelBiding) {
                    var hierarchyId = 0;
                    if (value != null) {
                        hierarchyId = value;
                    } else {
                        hierarchyId = 0;
                    }
                    parentItems = new Array();
                    parentItems.push({ name: ctrl.hierarchyKeyListen, value: hierarchyId });
                    if (isClearModelBiding) {
                        //ctrl.dataSource.read();
                        ctrl.lookupModelBinding = null;
                    }

                });
            }

        }
    });
}());