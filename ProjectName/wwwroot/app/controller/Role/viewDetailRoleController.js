﻿(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        var controllerId = "viewDetailRoleController";

        angularAmd.controller(controllerId, viewDetailRoleController);

        viewDetailRoleController.$inject = ['$rootScope', '$scope', 'logger', 'common', '$timeout', '$controller', '$http', 'storeHistoryObject', '$state'];

        var schemaFields = {
            Id: { editable: false },
            Name: { editable: false },
             IsShowMenu: { type: "boolean", editable: true },
            IsView: { type: "boolean", editable: true },
            IsInsert: { type: "boolean", editable: true },
            IsUpdate: { type: "boolean", editable: true },
            IsDelete: { type: "boolean", editable: true },
        };
        var columns = [            
            { field: "Name", title: "Name", attributes: { style: 'text-align:Left;' }, hidden: false },
            { field: "IsShowMenu", title: "Show Menu", attributes: { style: 'text-align:center;' }, width: 100, hidden: false, template: '<div ng-click="showHideTooltip();" data-toggle="tooltip" data-placement="top"><i #if(IsShowMenu){# class="fa fa-check" #}else{#class="fa fa-times"#}#></i></div>' },
            { field: "IsView", title: "View", attributes: { style: 'text-align:center;' }, width: 70, hidden: false, template: '<div ng-click="showHideTooltip();"  data-toggle="tooltip" data-placement="top"><i #if(IsView){# class="fa fa-check" #}else{#class="fa fa-times"#}#></i></div>' },
            { field: "IsInsert", title: "Create", attributes: { style: 'text-align:center;' }, width: 70, hidden: false, template: '<div ng-click="showHideTooltip();" data-toggle="tooltip" data-placement="top"><i #if(IsInsert){# class="fa fa-check" #}else{#class="fa fa-times"#}#></i></div>' },
            { field: "IsUpdate", title: "Update", attributes: { style: 'text-align:center;' }, width: 70, hidden: false, template: '<div ng-click="showHideTooltip();" data-toggle="tooltip" data-placement="top"><i #if(IsUpdate){# class="fa fa-check" #}else{#class="fa fa-times"#}#></i></div>' },
            { field: "IsDelete", title: "Delete", attributes: { style: 'text-align:center;' }, width: 70, hidden: false, template: '<div ng-click="showHideTooltip();" data-toggle="tooltip" data-placement="top"><i #if(IsDelete){# class="fa fa-check" #}else{#class="fa fa-times"#}#></i></div>' },
        ];
        function viewDetailRoleController($rootScope, $scope, logger, common, $timeout, $controller, $http, storeHistoryObject, $state) {
            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);
            var shared = this;
            shared.sharedControllerId = controllerId;
            shared.modelName = 'UserRole';
            activate();

            function activate() {              
                common.activateController(null, controllerId).then(function () {
                    $scope.$watch('popupForm.modified', function (newValue, oldValue) {
                        if (newValue !== oldValue) {
                            $rootScope.popupFormModified = newValue;
                        }
                    });
                    if ($scope.vm) {
                        $scope.vm.basePopup = $controller('basePopupController', { $scope: $scope });
                        $scope.vm.basePopup.modelName = shared.modelName;
                        $scope.vm.basePopup.sharedControllerId = 'viewDetailRoleController';
                    }
                   
                });

            }

            shared.dataSource = null;
            $scope.storeHistory = {};
            shared.shareInit = function (id) {
                $scope.storeHistory.Id = id;
                shared.Role = {};
                if (id == undefined) {
                    shared.Role = { Id: id, CreateMode: true, CheckAll: false }
                }
                else {
                    if (id === 0) {
                        shared.Role = { Id: id, CreateMode: true }
                    } else {
                        $http.get(shared.modelName + '/ViewDetailId/' + id).then(function (result) {
                            shared.Role = result.data;                                                      
                        });
                    }
                }
                //shared.Role = angular.fromJson();
                shared.Role.CheckAll = false;
                var url = "/UserRole/GetRoleFunction?id=" + id;
                shared.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: url,
                            dataType: 'json',
                            contentType: 'application/json; charset=utf-8'
                        },
                        parameterMap: function (options, operation) {
                            if (operation === "read") {
                                var result = {
                                    pageSize: options.pageSize,
                                    skip: options.skip,
                                    take: options.take
                                };

                                return result;
                            }
                        }
                    },
                    batch: true,
                    emptyMsg: 'No Record',
                    table: "#roleGrid",
                    change: changeValueInGrid,
                    schema: {
                        model: {
                            id: "Id",
                            fields: schemaFields
                        },
                        data: "Data",
                        total: "TotalRowCount"
                    }
                });
                shared.functionGridOptions = {
                    dataSource: shared.dataSource,
                    columns: columns,
                    scrollable: { virtual: true },
                    editable: false,
                    dataBound: onDataBound,
                    height: $(window).height()-370
                };
            };


            function onDataBound(arg) {
                $(".k-grid td").each(function () {
                    if ($(this).text() === "true") {
                        $(this).css({ "color": "green" });
                    }
                });
                $('[data-toggle="tooltip"]').tooltip({
                    container: 'body',
                    html: true
                });
            }

            function changeValueInGrid(e) {
                if (e.action !== undefined) {
                    shared.Role.CheckAll = false;
                    $("#roleGrid").parents('form').addClass('dirty');
                    $rootScope.popupFormModified = true;
                }
            }

            $scope.CheckAll = function () {
                if (shared.Role.CheckAll) {
                    var checkedAllData = _.map(shared.dataSource.data(), function (obj) {
                        return { Id: obj.Id, Name: obj.Name, IsView: true, IsInsert: true, IsUpdate: true, IsDelete: true, IsProcess: true };
                    });
                    $("#roleGrid").data("kendoGrid").dataSource.data(checkedAllData);
                } else {
                    $("#roleGrid").data("kendoGrid").dataSource.read();
                }
            };
            shared.getShareViewData = function () {
                shared.Role.RoleFunctionData = JSON.stringify(shared.dataSource.data());
                return { SharedParameter: JSON.stringify(shared.Role) };
            };

            shared.cancel = function () {
                $state.go("Role", {}, { reload: true });
            };
        }
    });
}());