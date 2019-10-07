(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisAutocomplete', cisAutocomplete);
        
        function cisAutocomplete() {
            return {
                restrict: "E",
                scope: {
                    modelBinding: '=ngModel'
                },
                require: 'ngModel',
                transclude: true,
                bindToController: {
                    autocompleteId: '@',
                    urlReadData: '@',
                    parameterDependencies: '@',
                    modelBinding: '=ngModel',
                    onchangefuc: '=',
                    maxLength: '@',
                    isReadonly:'@'
                },
                controller: cisAutocompleteController,
                controllerAs: 'autocompleteVm',
                template: '<div class="autocomplete-component"><input ng-readonly="{{autocompleteVm.isReadonly}}" maxlength="{{autocompleteVm.maxLength}}" kendo-auto-complete style="width: 100%;height:34px;padding-right:11px;" id="{{autocompleteVm.autocompleteId}}" ng-model="modelBinding" k-value-primitive="true" k-options="autocompleteVm.autoCompleteOptions"  /><span class="icon-autocomplete"><i class="k-icon k-i-arrow-s"></i></span></div>'
                    
            };
        }

        cisAutocompleteController.$inject = ['$rootScope', '$scope', '$timeout'];

        function cisAutocompleteController($rootScope, $scope, $timeout) {
            var ctrl = this;

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
                    read: {
                        url: ctrl.urlReadData,
                        dataType: "json",
                        type: "GET"
                    },
                    parameterMap: function (options, type) {
                        var value = '';
                        if (options.filter !== undefined && options.filter.filters != undefined && options.filter.filters[0] != undefined && options.filter.filters[0].value !== '') {
                            value = options.filter.filters[0].value;
                        }

                        var result = {
                            IncludeCurrentRecord: true,
                            Query: value,
                            ParameterDependencies: ctrl.parameterDependencies,
                            Take: 50
                        };
                        return result;
                    }
                }
            });
            ctrl.autoCompleteOptions = {
                dataSource: ctrl.dataSource,
                dataTextField: 'DisplayName',
                dataValueField: 'KeyId',
                template: '<span data-recordid="#= KeyId #"> #= DisplayName #</span>',
                minLength: 0,
                valuePrimitive: true,
                delay: 500,
                filter: "contains",
                dataBound: function (e) {
                },
                select: function (e) {
                    $rootScope.$broadcast(ctrl.autocompleteId, e.item.find('span').data('recordid'));
                },
                change: onChangeAutocomplete,
                open: function (e) {
                },
                close: function (e) {
                }
            };
            function onChangeAutocomplete(e) {
                if (typeof ctrl.onchangefuc == "function") {
                    ctrl.onchangefuc(e, this.dataItem());
                }
            };

            $timeout(function() {
                $("#" + ctrl.autocompleteId).parent().parent().click(function () {
                    var autocomplete = $("#" + ctrl.autocompleteId).data("kendoAutoComplete");
                    if (autocomplete != undefined) {
                        autocomplete.search($("#" + ctrl.autocompleteId).val());
                    }
                });
            });
        }
    });
}());