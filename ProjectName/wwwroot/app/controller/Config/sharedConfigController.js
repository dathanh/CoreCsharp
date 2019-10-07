(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        var controllerId = "sharedConfigController";

        angularAmd.controller(controllerId, sharedConfigController);

        sharedConfigController.$inject = ['$rootScope', '$scope', 'logger', 'common', '$controller', '$http', 'storeHistoryObject'];

        function sharedConfigController($rootScope, $scope, logger, common, $controller, $http, storeHistoryObject) {
	        var shared = this;
            shared.sharedControllerId = controllerId;
            shared.modelName = 'Config';
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
                        $scope.vm.basePopup.sharedControllerId = 'sharedConfigController';
                    }
                    $scope.$watch(function () {
                        return storeHistoryObject.storeHistory;
                    }, function (nval) {
                        if (nval.Id == undefined) {
                            $scope.setActionState(2);
                        }
                    });
                });

            }
            $scope.storeHistory = {};

            shared.shareInit = function (id) {
                shared.IsRequired = false;
                $scope.storeHistory.Id = id;
                if (id === 0) {
	                shared.Config = { Id: id, CreateMode: true, IsActive: true}
                } else {
                    $http.get(shared.modelName + '/Update/' + id).then(function (result) {

                        shared.Config = result.data;
                        shared.Config.CreateMode = false;
                        $scope.popupForm.$setPristine();
                    });
                }
            }
            shared.getShareViewData = function () {
                return { SharedParameter: JSON.stringify(shared.Config) };
            };
        }
    });
}());