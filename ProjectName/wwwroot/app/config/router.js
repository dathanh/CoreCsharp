(function () {
    'use strict';
    define(['angularAMD'], function (angularAmd) {
        var loadController = function (controllerName) {
            return [
                "$q", function ($q) {
                    var deferred = $q.defer();
                    require([controllerName], function () { deferred.resolve(); });
                    return deferred.promise;
                }
            ];
        }
        return {
            router: function ($stateProvider, $urlRouterProvider, $stateParams) {

                getRoutes().forEach(function (r) {
                    if (r.url == undefined || r.url == "") {
                        r.views.main.settings.children.forEach(function (rc) {
                            if (rc.views.main.settings.children != undefined && rc.views.main.settings.children.length > 0) {
                                rc.views.main.settings.children.forEach(function (rcc) {
                                    $stateProvider.state(rcc.name, angularAmd.route(rcc));
                                });
                            } else {
                                $stateProvider.state(rc.name, angularAmd.route(rc));
                            }
                        });
                    } else {
                        $stateProvider.state(r.name, angularAmd.route(r));
                    }
                });

                $urlRouterProvider.otherwise("/login");
            },
            getRouters: getRoutes,
            getUrlByRouteName: getUrlByRouteName,
        }

        function getRoutes() {
            var routes = [];
            routes.push(getRoutesForAdministration());
            routes.push(getRoutesForOrther());
            return routes;
        }

        function getUrlByRouteName(name) {
            getRoutes().forEach(function (r) {
                if (r.url == undefined || r.url == "") {
                    r.views.main.settings.children.forEach(function (rc) {
                        if (rc.name == name) {
                            return rc.url;
                        }
                    });
                }
            });
        }
        function getRoutesForAdministration() {
            return {
                name: "administration",
                url: "",
                urlShow: "",
                views: {
                    "main": {
                        title: '<i class="fa fa-cogs"></i> Administrator <span class="fa fa-chevron-down"></span>',
                        settings: {
                            nav: 17,
                            isShow: true,
                            children: [
                                {
                                    name: 'User',
                                    url: '/user',
                                    urlShow: '/user',
                                    views: {
                                        "main": {
                                            templateUrl: '/User/Index',
                                            title: 'Manage users',
                                            settings: {
                                                nav: 1701,
                                                isShow: true
                                            }
                                        }

                                    },
                                    resolve: loadController('userController')
                                },
                                {
                                    name: 'Role',
                                    url: '/role',
                                    urlShow: '/role',
                                    views: {
                                        "main": {
                                            templateUrl: '/UserRole/Index',
                                            title: 'Manage roles',
                                            settings: {
                                                nav: 1702,
                                                isShow: true
                                            }
                                        }
                                    },
                                    resolve: loadController('roleController')
                                },
                                {
                                    name: 'ViewDetailRole',
                                    url: "/role/view-detail/:id",
                                    urlShow: "/role/view-detail",
                                    params: {
                                        id: null
                                    },
                                    views: {
                                        "main": {
                                            templateUrl: function ($stateParams) {
                                                if ($stateParams != undefined && $stateParams.id != null && $stateParams.id != '' && !isNaN($stateParams.id)) {
                                                    return '/UserRole/ViewDetailRole?id=' + $stateParams.id;
                                                }
                                                return '/UserRole/Index';
                                            },
                                            title: 'Role',
                                            settings: {
                                                nav: 1720,
                                                isShow: false,
                                                isNeedLogin: false
                                            }
                                        }
                                    },
                                    resolve: loadController('viewDetailRoleController')
                                },
                                {
                                    name: 'Config',
                                    url: '/config',
                                    urlShow: '/config',
                                    views: {
                                        "main": {
                                            templateUrl: '/Config/Index',
                                            title: 'Manage config',
                                            settings: {
                                                nav: 1703,
                                                isShow: true
                                            }
                                        }
                                    },
                                    resolve: loadController('configController')
                                },
                                //========Import Menu Start========//

                                //========Import Menu End========//
                            ]
                        }
                    }
                }
            };
        }
        function getRoutesForOrther() {
            return {
                name: "Other",
                url: "",
                urlShow: "",
                views: {
                    "main": {
                        title: 'Other',
                        settings: {
                            nav: 18,
                            isShow: false,
                            children: [
                                {
                                    name: 'Login',
                                    url: "/login",
                                    urlShow: "/login",
                                    views: {
                                        "main": {
                                            templateUrl: '/Authentication/SignIn',
                                            title: 'Login',
                                            settings: {
                                                nav: 1801,
                                                isShow: false,
                                            }
                                        }
                                    },
                                    resolve: loadController('loginController')
                                },
                                {
                                    name: 'ResetPassword',
                                    url: "/reset-password?code",
                                    urlShow: "/reset-password",
                                    params: {
                                        code: null
                                    },
                                    views: {
                                        "main": {
                                            templateUrl: function ($stateParams) {
                                                return '/Authentication/ChangePassword?code=' + $stateParams.code;
                                            },
                                            title: 'Forgot password',
                                            settings: {
                                                nav: 1803,
                                                isShow: false,
                                            }
                                        }
                                    },
                                    resolve: loadController('forgotChangePasswordController')
                                },
                                {
                                    name: 'Error404',
                                    url: "/error-404",
                                    urlShow: "/error-404",
                                    views: {
                                        "main": {
                                            templateUrl: '/Error/HTTP404',
                                            title: '404',
                                            settings: {
                                                nav: 1804,
                                                isShow: false,
                                            }
                                        }
                                    },
                                    resolve: loadController('errorController')
                                },
                                {
                                    name: 'Error406',
                                    url: "/error-406",
                                    urlShow: "/error-406",
                                    views: {
                                        "main": {
                                            templateUrl: '/Error/HTTP406',
                                            title: '406',
                                            settings: {
                                                nav: 1805,
                                                isShow: false,
                                            }
                                        }
                                    },
                                    resolve: loadController('errorController')
                                },
                                {
                                    name: 'Error403',
                                    url: "/error-403",
                                    urlShow: "/error-403",
                                    views: {
                                        "main": {
                                            templateUrl: '/Error/UnAuthorizedAccess',
                                            title: '406',
                                            settings: {
                                                nav: 1806,
                                                isShow: false,
                                            }
                                        }
                                    },
                                    resolve: loadController('errorController')
                                },
                                {
                                    name: 'myprofile',
                                    url: "/my-profile",
                                    urlShow: "/my-profile",
                                    views: {
                                        "main": {
                                            templateUrl: '/MyProfile/Index',
                                            title: 'Your account',
                                            settings: {
                                                nav: 1807,
                                                isShow: false,
                                            }
                                        }
                                    },
                                    resolve: loadController('viewMyProfileController')
                                },
                                {
                                    name: 'NoPermission',
                                    url: "/no-permission",
                                    urlShow: "/no-permission",
                                    views: {
                                        "main": {
                                            templateUrl: '/Authentication/NoPermission',
                                            title: 'No permission',
                                            settings: {
                                                nav: 1808,
                                                isShow: false,
                                            }
                                        }
                                    },
                                    resolve: loadController('nopermissionController')
                                }
                            ]
                        }
                    }
                }
            };
        }
    });
}());