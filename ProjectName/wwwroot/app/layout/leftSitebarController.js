(function () {
    'use strict';

    define(['angularAMD', 'kendo'], function (angularAmd) {


        var controllerId = "leftSitebarController";

        angularAmd.controller(controllerId, controller);

        controller.$inject = ['$rootScope', '$scope', 'logger', 'common', 'config', 'routes', '$localStorage', '$injector', '$state', '$sce'];

        function controller($rootScope, $scope, logger, common, config, routes, $localStorage, $injector, $state, $sce) {
            var leftSitebar = this;
            leftSitebar.loggedIn = $localStorage.isLoggedIn;
            leftSitebar.menuActive = $localStorage.menuActive;
            leftSitebar.menuParentActive = $localStorage.menuActive;
            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);

            activate();

            function activate() {
                common.activateController(null, controllerId).then(function () {

                });
                getNavRoutes();
            }

            $scope.$watch(function () {
                if ($state.current.views != null) {
                    leftSitebar.menuActive = $state.current.views.main.settings.nav;
                    leftSitebar.menuParentActive = $state.current.views.main.settings.navParent;
                }
                return angular.toJson($localStorage.isLoggedIn);
            }, function () {
                leftSitebar.menuActive = $localStorage.menuActive;
                leftSitebar.loggedIn = $localStorage.isLoggedIn;
            });
            $scope.createFuction = function (event) {
              
            }

            $scope.createRoute = function (route) {
                if (route.isDirectLink == undefined || route.isDirectLink != true) {
                    if (route.urlShow == undefined || route.urlShow == "") {
                        return $sce.trustAsHtml("Javascript:void(0);");
                    } else {
                        return "#" + route.urlShow;
                    }
                } else {
                    if (route.toWebportal) {
                        return $rootScope.WebsitePortalURL+ route.urlShow;
                    }else 
                        return route.urlShow;
                }
            }

            $scope.createRouteChildRR = function (route) {
                return $scope.createRoute(route);
            };
            $scope.isNavSm = function() {
                return $("body").hasClass("nav-sm");
            };
            $scope.checkLiActive = function (childrens) {
                var result = false;
                if (childrens != null) {
                    $.each(childrens, function (index, value) {
                        if (value.views.main.settings.children != undefined && value.views.main.settings.children.length > 0) {
                            $.each(value.views.main.settings.children, function (index1, value1) {
                                if (value1.views.main.settings.nav == leftSitebar.menuActive || value1.views.main.settings.nav == leftSitebar.menuParentActive) {
                                    result = true;
                                }
                            });
                        } else {
                            if (value.views.main.settings.nav == leftSitebar.menuActive || value.views.main.settings.nav == leftSitebar.menuParentActive) {
                                result = true;
                            }
                        }
                    });
                }
                return result;
            }
            leftSitebar.isShowParent = function (r) {               
                return true;
            }
            leftSitebar.isVisibleParent = function (childrenRoutes) {
                var result = false;
                if (childrenRoutes != null && childrenRoutes.length>0) {
                    $.each(childrenRoutes, function (key, value) {
                        if (leftSitebar.isVisibleChildren(value.name)) {
                            result = true;
                        }
                    });
                }
                return result;
            }

            leftSitebar.isVisibleChildren = function (routeName) {              
                var result = false;
                if ($localStorage.menuList != null) {
                    $.each($localStorage.menuList, function (key, value) {
                        if (key == "CanView" + routeName && value == true) {                           
                            result = true;
                        }
                    });
                }
                return result;
            }

            function getNavRoutes() {
               
                leftSitebar.navRoutes = routes().filter(function (r) {
                    return r.views.main.settings && r.views.main.settings.nav && r.views.main.settings.isShow;
                }).sort(function (r1, r2) {
                    return r1.views.main.settings.nav - r2.views.main.settings.nav;
                });

            }
        }
    });
}());