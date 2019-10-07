(function () {
    'use strict';
    define(['angularAMD', 'masterfileService'], function (angularAmd) {

        var controllerId = "viewMyProfileController";

        angularAmd.controller(controllerId, viewMyProfileController);

        viewMyProfileController.$inject = [
            '$rootScope', '$scope', '$state', 'masterfileService', 'logger', 'common', 'config', '$http', '$window', '$localStorage', '$controller', 'accountService'
        ];

        function viewMyProfileController($rootScope, $scope, $state, masterfileService, logger, common, config, $http, $window, $localStorage, $controller, accountService) {
            var vm = this;
            vm.modelName = 'MyProfile';
            vm.controllerId = "viewMyProfileController";
            var getLogFn = logger.getLogFn;
            var log = getLogFn(vm.controllerId);
            function activate() {
                common.activateController(null, controllerId).then(function () {
                    $scope.$watch('popupForm.modified', function (newValue, oldValue) {
                        if (newValue !== oldValue) {
                            $rootScope.popupFormModified = newValue;
                        }
                    });
                    if ($scope.vm) {
                        $scope.vm.basePopup = $controller('basePopupController', { $scope: $scope });
                        $scope.vm.basePopup.modelName = vm.modelName;
                        $scope.vm.basePopup.sharedControllerId = 'viewMyProfileController';
                    }
                    $rootScope.IsShowAgency = false;
                });
                $("#li-navbar-nav-usermenu").removeClass("open");
            }
            activate();

            function reloadPage() {
                $state.reload();
            }

            vm.init = function () {

                accountService.GetProfileInfo().then(function (result) {
                    vm.MyProfile = result;
                    vm.MyProfile.CreateMode = false;
                    var avartarLink = "";
                    if ($localStorage.avartarPath == null || $localStorage.avartarPath == undefined || $localStorage.avartarPath == '' || $localStorage.avartarPath == '/Content/images/avatar.jpg') {
                        avartarLink = "/Content/images/avatar.jpg";
                    } else {
                        avartarLink = $localStorage.avartarPath;
                    }
                    vm.MyProfile.Avatar = avartarLink;
                    $scope.popupForm.$setPristine();
                });
            }

            $scope.$watch('vm.MyProfile.Avatar', function (nvl, ovl) {
                $rootScope.UserAvatar = nvl;
            });

            $scope.isOpen = false;
            $scope.cannotSave = true;
            $scope.cannotCancel = true;

            vm.EnableAllField = function () {
                $scope.isOpen = true;
                $scope.cannotCancel = false;
                $scope.cannotSave = false;
            };
            vm.CancelEditing = function () {
                $rootScope.waitingModified = false;
                $scope.cannotSave = true;
                $scope.isOpen = false;
                $scope.cannotCancel = true;
                reloadPage();
            };
            vm.SaveMyProfile = function () {
                var data = vm.MyProfile;
                $http.post("MyProfile/SaveMyProfile", { SharedParameter: JSON.stringify(data) }, {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).success(function (response) {
                    if (response.Error === undefined || response.Error === '') {
                        var logSuccess = getLogFn("viewMyProfileController", "success");
                        logSuccess(common.getMessageFromSystemMessage('UpdateSuccessfully', ["Profile"]));
                        $scope.isOpen = false;
                        $scope.cannotCancel = true;
                        $scope.cannotSave = true;
                        setTimeout(function () {
                            reloadPage();
                        }, 500);
                    }
                }).error(function (response) {
                    $scope.isOpen = true;
                });
            };
        };
    });
}());
