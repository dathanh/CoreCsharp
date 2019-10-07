(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.factory('accountService', dataService);

        dataService.$inject = ['masterfileService', 'logger'];

        function dataService(masterfileService, logger) {
            return {
                Login: login,
                LogOut: logout,
                ChangePassword: changePassword,
                SetPasswordUser: setPasswordUser,
                GetProfileInfo:getProfileInfo,
            };

            //login function
            function login(userName, password, isRememberMe) {
                return masterfileService.CallWithUrl('/Authentication/SignIn')
                    .perform({ userName: userName, password: password, rememberMe: isRememberMe})
                    .$promise
                    .then(loginCompleted)
                    .catch(loginFail);
            }

            //logout function
            function logout() {
                return masterfileService.CallWithUrl('/Authentication/SignOut')
                    .perform({})
                    .$promise
                    .then(loginCompleted)
                    .catch(loginFail);
            }

            //changePassword function
            function changePassword(oldPassword, password, confirmPassword) {
                return masterfileService.CallWithUrl('/User/SaveChangePasswordProfile')
                    .perform({ oldPassword: oldPassword, password: password, confirmPassword: confirmPassword })
                    .$promise
                    .then(loginCompleted)
                    .catch(loginFail);
            }

            //setPasswordUser function
            function setPasswordUser(userId, password) {
                return masterfileService.CallWithUrl('/User/ResetPassword')
                    .perform({ id: userId, password: password })
                    .$promise
                    .then(loginCompleted)
                    .catch(loginFail);
            }

            
            function getProfileInfo() {
                return masterfileService.CallWithUrl('/MyProfile/GetProfileInfo')
                    .perform({})
                    .$promise
                    .then(loginCompleted)
                    .catch(loginFail);
            }
            function loginCompleted(response) {
                return response;
            }

            function loginFail(error) {
                logger.logError(error);
            }
        }
    });

}());
