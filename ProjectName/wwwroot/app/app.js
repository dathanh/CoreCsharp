'use strict';

define([
    'angularAMD',
    'ui-router',
    'route',
    'common',
    'logger',
    'commonViewModel',
    'masterfileService',
    'angularAnimate',
    'sanitize',
    'resource',
    'ngStorage',
    'underscore',
    'jquery',
    'mCustomScrollbar',
    'kendo',
    'encoder',
    'Base64',
    'jquery.fileDownload',
    'jquery.cookie',
    'angular-input-modified',
    'angularCookie',
    'angularKendoWindowService',
    'loading-bar',
    'ngmap',
    'angularLoad',
    'jquery.maskedinput.min',
    'basePopupController',
    'headerController',
    'leftSitebarController',
    'cisLookupDirective',
    'cisGridDirective',
    'cisDatetimePickerDirective',
    'cisTimePickerDirective',
    'cisDropdownlistDirective',
    'cisMultiSelectDirective',
    'cisUploadDirective',
    'cisEditor',
    'charactersAndNumbersOnlyDirective',
    'numbersOnlyDirective',
    'formDirective',
    'enterDirective',
    'cisPopupDirective',
    'cisAutoCompleteDirective',
    'cisComboBoxAutoCompleteDirective',
    'dateFormat',
    'fileTypeValue',
], function (angularAmd, uiroute, configRoute, commonAmd, loggerAmd, commonViewModelAmd, masterfileServiceAmd) {
    var app = angular.module('app', [
        'ui.router',
        'ngAnimate',
        'kendo.directives',
        'ngSanitize',
        'ngResource',
        'angularKendoWindowService',
        'angular-loading-bar',
        'ngInputModified',
        'ngStorage',
        'ngCookies',
        'ngMap',
        'angularLoad'
    ], function ($locationProvider, $httpProvider, $provide) {
        $provide.factory('myHttpInterceptor', httpRequestExceptionHandler);
        $httpProvider.interceptors.push('myHttpInterceptor');
    });

    httpRequestExceptionHandler.$inject = ['$rootScope', '$q', 'common', 'logger', '$injector', '$localStorage'];

    app.config(['$compileProvider', function ($compileProvider) {
        $compileProvider.debugInfoEnabled(true);
    }]);

    app.provider('commonConfig', function () {
        this.config = {
            // These are the properties we need to set
            //controllerActivateSuccessEvent: '',
        };

        this.$get = function () {
            return {
                config: this.config
            };
        };
    });

    app.constant('routes', configRoute.getRouters);

    app.factory('common', ['$q', '$rootScope', '$timeout', 'commonConfig', 'logger', function ($q, $rootScope, $timeout, commonConfig, logger) {
        return commonAmd.common($q, $rootScope, $timeout, commonConfig, logger);
    }]);

    app.factory('logger', ['$log', function (logger) {
        return loggerAmd.logger(logger);
    }]);

    app.factory('commonViewModel', function () {
        return commonViewModelAmd.CommonViewModel();
    });

    app.factory('masterfileService', ['$resource', function ($resource) {
        return masterfileServiceAmd.masterfileService($resource);
    }]);

    app.filter('trusted', ['$sce', function ($sce) {
        var div = document.createElement('div');
        return function (text) {
            div.innerHTML = text;
            return $sce.trustAsHtml(div.textContent);
        };
    }]);
    //{storeHistory: null}
    app.service('storeHistoryObject', ['$location', function ($location) {
        this.storeHistory = {};
        this.setStoreHistory = function (val) {
            if (val) {
                this.storeHistory = val;
            } else {
                this.storeHistory = {};
            }
        }
        this.isSelfUrl = function (val, url) {
            var host = $location.absUrl().replace('#' + $location.url(), val);
            return host == $location.absUrl() && host == url;
        }

    }]);
    app.run(['$rootScope', '$templateCache', 'common', 'routes', '$localStorage', '$window', '$location', 'storeHistoryObject', '$timeout', '$state', '$injector',
        function ($rootScope, $templateCache, common, routes, $localStorage, $window, $location, storeHistoryObject, $timeout, $state, $injector) {
            $rootScope.IsLoadingPopup = false;
            $rootScope.BodyClass = "login-page";
            $rootScope.$on('$viewContentLoaded', function (e) {
                //common.setTimezoneCookie();
                $templateCache.removeAll();

            });
            $rootScope.$on('$includeContentLoaded', function (event) {
                if ($window.history.state) {
                    $timeout(function () {
                        storeHistoryObject.setStoreHistory(window.history.state);
                        $rootScope.$apply();
                    }, 100);
                } else {
                    var obj = _.clone(event.targetScope.$$childHead);
                    if (obj && obj.storeHistory) {
                        obj.storeHistory.url = $location.absUrl();
                        $window.history.pushState(obj.storeHistory, 'Liberty Healthcare Corporation', $location.absUrl());
                        storeHistoryObject.setStoreHistory(obj.storeHistory);
                    }
                }
            });
            $window.addEventListener('popstate', function (event) {
                $timeout(function () {
                    storeHistoryObject.setStoreHistory(event.state);
                    $rootScope.$apply();
                });
            });
            angular.element($window).bind('orientationchange', function () {
                $state.go($state.current, {}, { reload: true });
            });
            //var resizeTimer;
            //var h = 0;
            //angular.element($window).bind('load', function () {
            //    h = $(window).height();
            //});
            //angular.element($window).bind('resize', function () {
            //    if (h != $(window).height() || $(window).width()<500) {
            //        // Stop the pending timeout
            //        $timeout.cancel(resizeTimer);
            //        // Start a timeout
            //        resizeTimer = $timeout(function () {
            //            $state.go($state.current, {}, {
            //                reload: true
            //            });
            //        }, 250);
            //        h = $(window).height();
            //    }
            //});
            $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
                $("[data-toggle='tooltip']").tooltip('destroy');
                $(".customTooltip").tooltip('destroy');
                if ($rootScope.isDisabledBackButton == true) {
                    // Prevent the browser default action (Going back):
                    event.preventDefault();
                }
            });
            $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
                if (toState.name != "Login" && toState.name != "ResetPassword") {
                    if ($("body").hasClass("nav-sm")) {
                        $rootScope.BodyClass = "nav-sm pace-done";
                    } else {
                        $rootScope.BodyClass = "nav-md pace-done";
                    }

                } else {
                    $rootScope.BodyClass = "login-page";
                }
                $("[data-toggle='tooltip']").tooltip('destroy');
                $(".customTooltip").tooltip('destroy');
            });

            $rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) {
                event.preventDefault();
                console.log(error);
                //$state.get('error').error = { code: 123, description: 'Exception stack trace' }
                //return $state.go('error');
            });
            $rootScope.safeApply = function (fn) {
                var phase = this.$root.$$phase;
                if (phase == '$apply' || phase == '$digest') {
                    if (fn && (typeof (fn) === 'function')) {
                        fn();
                    }
                } else {
                    this.$apply(fn);
                }
            };
            $rootScope.signOutBy403 = function () {
                $localStorage.isLoggedIn = false;
                $localStorage.returnUrl = location.href;
                $state.go("Login", {}, { reload: true });
                //$injector.get('$state').transitionTo('Login');

            };

            $rootScope.redirectPreUrl = function (stateDefault, nextMilestone) {
                if ($localStorage.executeRedirect) {
                    if ($localStorage.executeRedirect.id) {
                        //if (nextMilestone && $localStorage.requestRedirect != undefined && $localStorage.requestRedirect != null && $localStorage.requestRedirect!="") {
                        //    $state.go($localStorage.requestRedirect, {}, { reload: true });
                        //} else {
                        $state.go($localStorage.executeRedirect.routeName, { id: $localStorage.executeRedirect.id }, { reload: true });
                        $timeout(function () {
                            $window.history.replaceState(3, 'Liberty Healthcare Corporation', $location.absUrl());
                        });
                        //}
                    } else {
                        $state.go($localStorage.executeRedirect.routeName, {}, { reload: true });
                    }

                } else {
                    $state.go(stateDefault, {}, { reload: true });
                }
            }

            $rootScope.IsAllTask = 0;
            $(document).bind("ajaxSend", function () {
                $("[data-toggle='tooltip']").tooltip('destroy');
                $("[data-toggle='popover']").popover('destroy');
                $(".customTooltip").tooltip('destroy');
            }).bind("ajaxComplete", function (event, xhr, settings) {
                if (xhr.responseText != undefined && xhr.responseText.indexOf("\"Status\":403") >= 0) {
                    var json = JSON.parse(xhr.responseText);
                    if (json != undefined && json.Status == 403) {
                        var kendoWindow = $(".k-window .k-window-content"), count = kendoWindow.length;
                        if (count > 0) {
                            kendoWindow.each(function (index, element) {
                                if ($(element).data("kendoWindow") != undefined) {
                                    $(element).data("kendoWindow").close();
                                }
                                if (!--count) {
                                    $rootScope.signOutBy403();
                                }
                            });
                        } else {
                            $rootScope.signOutBy403();
                        }
                    }
                }
                $('[data-toggle="tooltip"]').tooltip({
                    container: 'body',
                    html: true
                });
                $('[data-toggle="popover"]').popover({
                    container: 'body',
                    html: true
                });
                $(".customTooltip").tooltip({
                    container: 'body',
                    html: true
                });
                $timeout(function () {
                    $(".customTooltip").mouseover(function () {
                        $(".customTooltip").tooltip("show");
                    });
                    $(".customTooltip").mouseout(function () {
                        $(".customTooltip").tooltip("hide");
                    });
                });
            });

            //config Time to save pcst after time period
            $rootScope.TimeToAutoSavePcst = 300000;
            $rootScope.IntervalAutoSavePcst = null;
            $(window).on('hashchange', function (e) {
                clearInterval($rootScope.IntervalAutoSavePcst);
            });

           
        }]);

    app.config(['$httpProvider', '$compileProvider', function ($httpProvider, $compileProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|file|javascript):/);
    }]);

    app.config(['$stateProvider', '$urlRouterProvider', function (stateProvider, urlRouterProvider) {
        configRoute.router(stateProvider, urlRouterProvider);
    }]);
    app.config(['$localStorageProvider', function ($localStorageProvider) {
    }]);

    app.config([
        'cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeSpinner = true;
            cfpLoadingBarProvider.includeBar = true;
            //cfpLoadingBarProvider.spinnerTemplate = '<div><span class="fa fa-spinner">Loading...</div>';
        }
    ]);

    // Config for exception handle
    // For use with the HotTowel-Angular-Breeze add-on that uses Breeze
    var remoteServiceName = 'breeze/Breeze';
    var events = {
        controllerActivateSuccess: 'controller.activateSuccess',
        controllerFormSaveDataEvent: 'controller.formSaveDataEvent',
        controllerFormCancelSaveDataEvent: 'controller.formCancelDataEvent'
    };
    var config = {
        appErrorPrefix: '[HT Error] ', //Configure the exceptionHandler decorator
        docTitle: 'Host: ',
        events: events,
        remoteServiceName: remoteServiceName,
        version: '1.0.0'
    };

    app.value('config', config);

    app.config(['$logProvider', function ($logProvider) {
        // turn debugging off/on (no info or warn)
        if ($logProvider.debugEnabled) {
            $logProvider.debugEnabled(true);
        }
    }]);

    app.config(['commonConfigProvider', function (cfg) {
        cfg.config.controllerActivateSuccessEvent = config.events.controllerActivateSuccess;
        cfg.config.controllerFormSaveDataEvent = config.events.controllerFormSaveDataEvent;
        cfg.config.controllerFormCancelSaveDataEvent = config.events.controllerFormCancelSaveDataEvent;
    }]);

    app.config(['$provide', function ($provide) {
        appExceptionHandler.$inject = ['$delegate', 'config', 'logger'];
        $provide.decorator('$exceptionHandler', appExceptionHandler);
    }]);

    //function define
    function httpRequestExceptionHandler($rootScope, $q, common, logger, $injector, $localStorage) {

        return {
            'request': function (req) {
                return req;
            },
            'requestError': function (rejection) {
                return $q.reject(rejection);
            },
            'response': function (response) {
                var resultData = response.data;
                if (resultData != undefined && resultData.Error != undefined && resultData.Error !== '') {
                    var feedback = new commonViewModelAmd.CommonViewModel();
                    if (resultData.Status == 403) {
                        $rootScope.signOutBy403();
                    } else {
                        common.showErrorModelState(new feedback.FeedbackViewModel(resultData.Status, resultData.Error, resultData.StackTrace, resultData.ModelStateErrors), resultData.LogType);
                    }

                }
                return response;
            },
            'responseError': function (rejection) {
                var getLogFn = logger.getLogFn;
                var logError = getLogFn("", "error");
                var status = rejection.status;
                if (status === 401) { // Forbidden: Access is denied
                    var feedback = new commonViewModelAmd.CommonViewModel();
                    common.showErrorModelState(new feedback.FeedbackViewModel(rejection.data.Status, rejection.statusText, rejection.data.Error), 'error');
                } else if (status === 403) { //Unauthorized
                    $rootScope.signOutBy403();
                } else if (status === 404) {
                    logError("404: Sorry but we couldn't find this page");
                } else if (status === 0 || status === -1) {
                } else {
                    logError(rejection);
                }
                return $q.reject(rejection);
            }
        };
    }

    function appExceptionHandler($delegate, cfg, logger) {
        var appErrorPrefix = cfg.appErrorPrefix;
        var logError = logger.getLogFn('app', 'error');
        return function (exception, cause) {
            $delegate(exception, cause);
            if (appErrorPrefix && exception.message.indexOf(appErrorPrefix) === 0) { return; }
            var errorData = { exception: exception, cause: cause };
            var msg = appErrorPrefix + exception.message;
            //logError("An error has occurred while downloading the data. Please try again or contact administrator. Thanks.");
            logError(msg, errorData, true);
        };
    }

    return angularAmd.bootstrap(app);
});