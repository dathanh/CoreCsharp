(function () {
    'use strict';

    define(['angularAMD', 'masterfileService', 'accountService'], function (angularAmd) {

        var controllerId = "setPasswordController";

        angularAmd.controller(controllerId, setPasswordController);

        setPasswordController.$inject = ['$rootScope', '$scope', 'logger', 'common', '$controller', 'accountService','$state'];

        function setPasswordController($rootScope, $scope, logger, common, $controller, accountService, $state) {
            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);
            var vm = this;
            vm.WaitingButton = true;
            vm.basePopup = $controller('basePopupController', { $scope: $scope });
            vm.deRegisterEvent = $rootScope.$on('BroadcastDataFromKendoWindow_' + controllerId, function (event, data) {
                vm.basePopup.$popupData.resolve(data);
                vm.userId = data.userId;
                vm.deRegisterEvent();
            });
            
            activate();
            function activate() {
                common.activateController(null, controllerId).then(function () { });
                $scope.$watch('popupForm.modified', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        $rootScope.popupFormModified = newValue;
                    }
                });
            }
            //implement
            vm.basePopup.$popupData.promise.then(function (popupData) {
                vm.passwordPopup = popupData;
            });          

            vm.update = function () {
                vm.WaitingButton = false;
                var logError = getLogFn(controllerId, "error");
                var mess = common.getMessageFromSystemMessage("BussinessGenericErrorMessageKey", []) + '<br/>';
                mess += "<ol>";
                var isError = false;
                if (vm.NewPassword == undefined || vm.ConfirmPassword == undefined || vm.NewPassword.trim() === '' || vm.ConfirmPassword.trim() === '') {
                    //logError(common.getMessageFromSystemMessage("BussinessGenericErrorMessageKey", []).toUpperCase() + '<br/>'+ common.getMessageFromSystemMessage('PasswordAndConfirmRequired', []));
                    //vm.WaitingButton = true;
                    //return;
                    isError = true;
                    mess += '<li>' + common.getMessageFromSystemMessage('PasswordAndConfirmRequired', []) + '</li>';
                } else {
                    if (vm.NewPassword !== vm.ConfirmPassword) {
                        //logError(common.getMessageFromSystemMessage("BussinessGenericErrorMessageKey", []).toUpperCase() + '<br/>' + common.getMessageFromSystemMessage("ComparePassword", []));
                        //vm.WaitingButton = true;
                        //return;
                        isError = true;
                        mess += '<li>' + common.getMessageFromSystemMessage("ComparePassword", []) + '</li>';
                    }
                }
                //else if(false) {
                //    if (vm.NewPassword.length < 8) {
                //        //logError(common.getMessageFromSystemMessage("BussinessGenericErrorMessageKey", []).toUpperCase() + '<br/>' + common.getMessageFromSystemMessage("PasswordLength", ['Password']));
                //        //vm.WaitingButton = true;
                //        //return;
                //        isError = true;
                //        mess += '<li>' + common.getMessageFromSystemMessage("PasswordLength", ['Password']) + '</li>';

                //    }
                //    if (!/\d/.test(vm.NewPassword)) {
                //        //logError(common.getMessageFromSystemMessage("BussinessGenericErrorMessageKey", []).toUpperCase() + '<br/>Password must have least 1 number');
                //        //vm.WaitingButton = true;
                //        //return;
                //        isError = true;
                //        mess += '<li>' + 'Password must have least 1 number' + '</li>';

                //    }
                //    if (!/[A-Z]/.test(vm.NewPassword)) {
                //        //logError(common.getMessageFromSystemMessage("BussinessGenericErrorMessageKey", []).toUpperCase() + '<br/>Password must have least one upper case');
                //        //vm.WaitingButton = true;
                //        //return;
                //        isError = true;
                //        mess += '<li>' + 'Password must have least one upper case' + '</li>';

                //    }
                //    if (!/[$@$!%*?&]/.test(vm.NewPassword)) {
                //        //logError(common.getMessageFromSystemMessage("BussinessGenericErrorMessageKey", []).toUpperCase() + '<br/>Password must have least one special character');
                //        //vm.WaitingButton = true;
                //        //return;
                //        isError = true;
                //        mess += '<li>' + 'Password must have least one special character' + '</li>';
                //    }
                //}
                
                mess += "</ol>";
                if (isError) {
                    vm.WaitingButton = true;
                    logError(mess);
                    return;
                }
               
                accountService.SetPasswordUser(vm.userIdReset, vm.NewPassword)
                    .then(function (result,data) {
                    if (result.success != null) {
                        if (result.Data === false) {
                            logError(common.getMessageFromSystemMessage('BussinessGenericErrorMessageKey', []).toUpperCase() + '<br/>' + common.getMessageFromSystemMessage('ItemIsDeleted', ['User']));
                        } else  {
                            var logSuccess = getLogFn(controllerId, "success");
                            logSuccess(common.getMessageFromSystemMessage('ResetPasswordUserSuccess', []));
                            vm.passwordPopup.$windowInstance.close(null);
                        }
                    }
                    vm.WaitingButton = true;
                });
            };

            vm.cancel = function () {
                vm.passwordPopup.$windowInstance.close(null);
            }
            
            vm.shareInit = function (viewModel) {
                vm.userIdReset = viewModel;
            };                   
        }
    });
}());