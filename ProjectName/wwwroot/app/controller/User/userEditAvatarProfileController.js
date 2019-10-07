(function () {
    'use strict';

    define(['angularAMD', 'masterfileService'], function (angularAmd) {

        var controllerId = "userEditAvatarProfileController";

        angularAmd.controller(controllerId, userEditAvatarProfileController);

        userEditAvatarProfileController.$inject = ['$rootScope', '$scope', 'logger', 'common', '$controller', '$http', 'masterfileService'];

        function userEditAvatarProfileController($rootScope, $scope, logger, common, $controller, $http, masterfileService) {

            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);
            var vm = this;
            vm.modelName = 'MyProfile';
            vm.basePopup = $controller('basePopupController', { $scope: $scope });
            vm.deRegisterEvent = $rootScope.$on('BroadcastDataFromKendoWindow_' + controllerId, function (event, data) {
                vm.basePopup.$popupData.resolve(data);
                vm.basePopup.modelName = data.modelName;
                vm.deRegisterEvent();
            });
            vm.basePopup.sharedControllerId = 'sharedUserController';

            vm.init = function () {
                $http.get(vm.modelName + '/GetProfileInfo').then(function (result) {
                    vm.MyProfile = result.data;
                    vm.MyProfile.CreateMode = false;
                });
                common.formatFormInPage();
            }

            activate();

            function activate() {
                common.activateController(null, controllerId).then(function () { });
            }

            function reloadPage() {
                $state.reload();
            }
            vm.CancelEditing= function() {
                reloadPage();
            }
            vm.SaveAvatar = function () {
                var data = vm.MyProfile;

                $http.post("MyProfile/SaveMyProfile", { SharedParameter: JSON.stringify(data) }, {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).success(function(response) {
                    if (response.Error === undefined || response.Error === '') {
                        var logSuccess = getLogFn("viewMyProfileController", "success");
                        logSuccess("Update profile successfully");
                        $scope.isOpen = false;
                        $scope.cannotCancel = true;
                        $scope.cannotSave = true;
                        setTimeout(function() {
                            reloadPage();
                        }, 500);

                    }
                });
            };
        }
    });
}());
