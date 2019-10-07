
(function () {
    'use strict';

    define(['angularAMD', 'accountService', 'cacheTempleteService'], function (angularAmd) {

        var controllerId = "loginController";

        angularAmd.controller(controllerId, loginController);

        loginController.$inject = ['$scope', 'common', 'commonViewModel', 'accountService', '$injector', '$cookies', '$timeout', '$localStorage', 'masterfileService', 'routes'];

        function loginController($scope, common,commonViewModelAmd, accountService, $injector, $cookies, $timeout, $localStorage, masterfileService, routes) {

            $scope.showLogin = true;
            activate();

            function activate() {
                common.activateController(null, controllerId).then(function () {
                    $("#loading-white-backgroud").hide();
                    $("#loading-white-backgroud").css({ "background": "#ffffff" });
                });

            }

            $timeout(function () {
                var favoriteCookie = $cookies.getObject("loginProjectNameWeb");
                if (favoriteCookie !== undefined && favoriteCookie !== null && favoriteCookie.staySignIn) {
                    $scope.userName = favoriteCookie.userName;
                    $scope.password = favoriteCookie.password;
                    $scope.staySignIn = favoriteCookie.staySignIn;
                    if ($scope.staySignIn == true) {
                        $scope.login();
                    }
                    //$scope.login();
                }
            });
            $scope.login = function () {


                if ($scope.userName === "" || $scope.password === "" || $scope.userName === undefined || $scope.password === undefined) {
                    common.showErrorModelState(new commonViewModelAmd.FeedbackViewModel("error", "Fill in user name and password", "", []));
                    return;
                }

                accountService.Login($scope.userName, $scope.password, false).then(function (result) {

                    if ($scope.staySignIn) {
                        $timeout(function () {
                            $cookies.putObject('loginProjectNameWeb', { userName: $scope.userName, password: $scope.password, staySignIn: $scope.staySignIn });
                        }, 1000);
                    }
                    if (result.isLogin) {
                        $scope.showLogin = false;
                        masterfileService.CallWithUrl("/MainContainer/GetMenu").perform({ parameters: "" }).$promise
                            .then(function (result1) {
                                var transitionToName = "NoPermission";
                                if (result1.Error === undefined || result1.Error === '') {
                                    $timeout(function () {
                                        $localStorage.menuList = result1.Data.Menu;
                                        $localStorage.isLoggedIn = true;
                                        $localStorage.userName = $scope.userName;

                                        if ($localStorage.returnUrl !== undefined && $localStorage.returnUrl !== null && $localStorage.returnUrl !== "" && $localStorage.returnUrl.indexOf("#/login") < 0) {
                                            var returnUrl = $localStorage.returnUrl;
                                            $localStorage.returnUrl = null;
                                            location.href = returnUrl;

                                        } else {
                                            //redirect to url default
                                            $scope.navRoutes = routes().filter(function (r) {
                                                return r.views.main.settings && r.views.main.settings.nav && r.views.main.settings.isShow;
                                            }).sort(function (r1, r2) {
                                                return r1.views.main.settings.nav - r2.views.main.settings.nav;
                                            });
                                            if ($scope.navRoutes != null && $scope.navRoutes.length > 0) {
                                                $.each($scope.navRoutes, function (key, value) {
                                                    if (value.views.main.settings.children.length > 0) {
                                                        $.each(value.views.main.settings.children, function (key1, value1) {
                                                            if ($localStorage.menuList != null) {
                                                                $.each($localStorage.menuList, function (key2, value2) {
                                                                    if (key2 == "CanView" + value1.name && value2 == true) {
                                                                        transitionToName = value1.name;
                                                                        $localStorage.menuActive = value1.views.main.settings.nav;
                                                                        return false;
                                                                    }
                                                                });
                                                            }
                                                            if (transitionToName != "NoPermission") {
                                                                return false;
                                                            }
                                                        });
                                                    }
                                                    if (transitionToName != "NoPermission") {
                                                        return false;
                                                    }
                                                });
                                            }

                                            $injector.get('$state').transitionTo(transitionToName);
                                        }
                                        //run Cache
                                        //cacheTempleteService.clear();
                                    }, 1000);

                                } else {
                                    $localStorage.menuList = [];
                                }
                            });
                    }
                });
            };
        }
    });
}());