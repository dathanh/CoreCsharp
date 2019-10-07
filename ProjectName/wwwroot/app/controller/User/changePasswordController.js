(function () {
    'use strict';

    define(['angularAMD', 'masterfileService', 'accountService'], function (angularAmd) {

        var controllerId = "changePasswordController";

        angularAmd.controller(controllerId, changePasswordController);

        changePasswordController.$inject = ['$rootScope', '$scope', 'logger', 'common', '$controller', 'accountService', '$cookies', '$localStorage', '$state'];

        function changePasswordController($rootScope, $scope, logger, common, $controller, accountService, $cookies, $localStorage, $state) {

            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);
            var vm = this;
            vm.basePopup = $controller('basePopupController', { $scope: $scope });
            vm.deRegisterEvent = $rootScope.$on('BroadcastDataFromKendoWindow_' + controllerId, function (event, data) {
                vm.basePopup.$popupData.resolve(data);
                vm.deRegisterEvent();
            });

            activate();

            function activate() {
                common.activateController(null, controllerId).then(function () { });
            }

            //implement

            vm.basePopup.$popupData.promise.then(function (popupData) {
                vm.passwordPopup = popupData;
                
            });

            vm.update = function () {
                var isExistError = false;
                var logError = getLogFn(controllerId, "error");
                var mess = common.getMessageFromSystemMessage("BussinessGenericErrorMessageKey", []) + '<br/>';
                mess += "<ol>";
                //  Current Password
                if (vm.CurrentPassword === undefined || vm.CurrentPassword === null || vm.CurrentPassword.trim() === "") {
                    mess += '<li>' + common.getMessageFromSystemMessage('RequiredField', ['Mật khẩu hiện tại']) + '</li>';
                    isExistError = true;
                }

                //  New Password
                if (vm.NewPassword === undefined || vm.NewPassword === null || vm.NewPassword.trim() === "") {
                    mess += '<li>' + common.getMessageFromSystemMessage('RequiredField', ['Mật khẩu mới']) + '</li>';
                    isExistError = true;
                }

                // Confirm New Password
                if (vm.ConfirmNewPassword === undefined || vm.ConfirmNewPassword === null || vm.ConfirmNewPassword.trim() === "") {
                    mess += '<li>' + common.getMessageFromSystemMessage('RequiredField', ['Mật khẩu nhập lại']) + '</li>';
                    isExistError = true;
                }

                mess += "</ol>";
                if (isExistError) {
                    logError(mess);
                    return;
                }

                accountService.ChangePassword(vm.CurrentPassword, vm.NewPassword, vm.ConfirmNewPassword).then(function (result) {
                    if (result.Error==null) { 
                        var logSuccess = getLogFn(controllerId, "success");
                        logSuccess(common.getMessageFromSystemMessage("ChangePasswordUserSuccess"));
                        vm.passwordPopup.$windowInstance.close(true);
                    }
                });
            };

            vm.cancel = function () {
                //accountService.LogOut().then(function (result) {
                //    $cookies.remove('loginInfoWeb');
                //    if (result.IsSignOut) {
                //        $("#loading-white-backgroud").css({ "background": "#CFD8DC" });
                //        $("#loading-white-backgroud").show();
                //        $localStorage.isLoggedIn = false;
                //        $state.transitionTo('Login');
                //    }
                //});
                vm.passwordPopup.$windowInstance.close(false);
            }
            
        }
    });
}());