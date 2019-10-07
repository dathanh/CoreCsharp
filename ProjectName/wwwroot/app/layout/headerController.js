(function () {
    'use strict';

    define(['angularAMD', 'kendo', 'accountService'], function (angularAmd) {

        var controllerId = "headerController";

        angularAmd.controller(controllerId, controller);

        controller.$inject = ['$rootScope', '$scope', 'logger', 'common', '$localStorage', 'accountService', '$injector', '$state', '$cookies', '$timeout', 'angularLoad', 'angularKendoWindowService'];

        function controller($rootScope,$scope, logger, common, $localStorage, accountService, $injector, $state, $cookies, $timeout, angularLoad, kendoWindowService) {
            var header = this;
            header.loggedIn = $localStorage.isLoggedIn;
            header.userName = $localStorage.userName;
            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);

            activate();
            header.isUpdateProviderAgencyDefault = true;
            function activate() {
                common.activateController(null, controllerId).then(function() {
                    if ($localStorage.isLoggedIn) {
                        accountService.GetProfileInfo().then(function(result) {
                            $rootScope.CurrentUserId = result.Id;
                            $rootScope.UserAvatar = result.Avatar;
                            if ($rootScope.UserAvatar == null || $rootScope.UserAvatar == undefined || $rootScope.UserAvatar == '') {
                                $rootScope.UserAvatar = "/css/images/avatar.jpg";
                            }
                            $localStorage.avartarPath = $rootScope.UserAvatar;
                        });
                    }
                });
            }

            header.parseJson = function (json) {
                return common.decodeObject(angular.fromJson(json));
            }
            header.signOut = function () {
                $("#li-navbar-nav-usermenu").removeClass("open");
                common.bootboxConfirm("Do you want to log out?", function () {
                    accountService.LogOut().then(function (result) {
                        $cookies.remove('loginInfoHostWeb');
                        if (result.IsSignOut) {
                            $("#loading-white-backgroud").css({ "background": "#CFD8DC" });
                            $("#loading-white-backgroud").show();
                            $localStorage.isLoggedIn = false;
                            $injector.get('$state').transitionTo('Login');
                        }
                    });
                }, function () { }).modal('show');
            };

            header.changePassword = function () {
                $("#li-navbar-nav-usermenu").removeClass("open");
                var windowInstance = kendoWindowService.ShowKendoWindow({
                    modal: true,
                    title: 'Change Password',
                    width: 600,
                    height: 175,
                    url: '/User/ChangePassword/',
                    controller: ['changePasswordController']
                });
            };



            $scope.$watch(function () {
                return angular.toJson($localStorage.isLoggedIn);
            }, function () {
                header.loggedIn = $localStorage.isLoggedIn;
                if (header.loggedIn) {
                    header.userName = $localStorage.userName;
                }

                $timeout(function () {
                    var date = new Date();
                    var ver = date.getDate() + "" + date.getMonth() + "" + date.getFullYear() + "" + date.getHours() + "" + date.getMinutes() + "" + date.getMilliseconds();
                    angularLoad.loadScript(cdnUrl+'/js/custom.js?ver=' + ver).then(function () {
                    }).catch(function () {
                        // There was some error loading the script. Meh
                    });
                });
            });
          
        }
    });
}());
