(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        var controllerId = "sharedUserController";

        angularAmd.controller(controllerId, sharedUserController);

        sharedUserController.$inject = ['$rootScope', '$scope', 'logger', 'common', '$controller', '$http', 'storeHistoryObject'];

        function sharedUserController($rootScope, $scope, logger, common, $controller, $http,storeHistoryObject) {
            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);
            var shared = this;
            shared.sharedControllerId = controllerId;
            shared.modelName = 'User';
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
                        $scope.vm.basePopup.sharedControllerId = 'sharedUserController';
                    }
                    $scope.$watch(function () {
                        return storeHistoryObject.storeHistory;
                    }, function (nval, oval) {
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
                var date = new Date();
                    var currentDateFormat = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
                    shared.User = { Id: id, CreateMode: true, IsActive: true, JoinDate: currentDateFormat }
                } else {
                    $http.get(shared.modelName + '/Update/' + id).then(function (result) {

                        shared.User = result.data;
                        shared.User.CreateMode = false;
                        $scope.popupForm.$setPristine();
                    });
                }
            }
            shared.getShareViewData = function () {
                return { SharedParameter: JSON.stringify(shared.User) };
            };
        }
    });
}());