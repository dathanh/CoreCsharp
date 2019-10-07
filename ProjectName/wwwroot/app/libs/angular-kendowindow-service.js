(function () {

    'use strict';

    var module = angular.module('angularKendoWindowService', []);

    kservice.$inject = ['$injector', '$rootScope', '$q', '$http', '$templateCache', '$controller', '$windowStack'];
    module.factory('angularKendoWindowService', kservice);

    function kservice($injector, $rootScope, $q, $http, $templateCache, $controller, $windowStack) {
        return {
            ShowKendoWindow: showKendoWindow
        };

        function showKendoWindow(windowOptions) {
            
            var windowResultDeferred = $q.defer();
            var windowOpenedDeferred = $q.defer();
            var windowInstance = {
                id: "kWindow" + $windowStack.length(),
                result: windowResultDeferred.promise,
                opened: windowOpenedDeferred.promise,
                close: function (result) {
                    $windowStack.close(windowInstance, result);
                },
                dismiss: function (reason) {
                    $windowStack.dismiss(windowInstance, reason);
                },
                activate: function () {
                }
            };

            //merge and clean up options
            windowOptions = angular.extend({}, windowOptions);
            //windowOptions.resolve = windowOptions.resolve || {};

            //verify options
            if (!windowOptions.template && !windowOptions.url && !windowOptions.content.url) {
                throw new Error('One of template or url options is required.');
            }
            require(windowOptions.controller, function () {
                $rootScope.IsLoadingPopup = true;
                var templateAndResolvePromise = $q.all([getTemplatePromise(windowOptions)].concat(getResolvePromises(windowOptions.resolve)));
                templateAndResolvePromise.then(function resolveSuccess(tplAndVars) {
                    var windowScope = (windowOptions.scope || $rootScope).$new();
                    windowScope.$close = windowInstance.close;
                    windowScope.$dismiss = windowInstance.dismiss;

                    var ctrlLocals = {};
                    var resolveIter = 1;

                    //controllers
                    if (windowOptions.controller) {
                        //ctrlLocals.$scope = windowScope;
                        ctrlLocals.$windowInstance = windowInstance;
                        angular.forEach(windowOptions.resolve, function (value, key) {
                            ctrlLocals[key] = tplAndVars[resolveIter++];
                        });
                        
                        //$controller(windowOptions.controller, ctrlLocals);
                    }

                    $windowStack.open(windowInstance, {
                        scope: windowScope,
                        deferred: windowResultDeferred,
                        content: tplAndVars[0],
                        keyboard: windowOptions.keyboard,
                        windowClass: windowOptions.windowClass
                    });

                    $rootScope.IsLoadingPopup = false;
                    $rootScope.$broadcast("BroadcastDataFromKendoWindow_" + windowOptions.controller[0], ctrlLocals);
                    
                }, function resolveError(reason) {
                    windowResultDeferred.reject(reason);
                });

                templateAndResolvePromise.then(function () {
                    var opts = {
                        title: windowOptions.title,
                        modal: true,
                        width: 500,
                        actions: windowOptions.actions!=undefined?windowOptions.actions:["Close"],
                        visible: false,
                        animation: false,
                        draggable: false,
                        open: function () {
                            this.center();
                        },
                        close: function (e) {
                            if (windowOptions.disablePreventClosePopup == undefined || windowOptions.disablePreventClosePopup == false) {
                                if ($rootScope.popupFormModified && !confirm("To prevent information loss, please save the changes before closing. Are you sure you want to close this window?")) {
                                    e.preventDefault();
                                    return;
                                }
                            }
                            if (windowOptions.closePopupEvent) {
                                windowOptions.closePopupEvent();
                            }
                            windowInstance.close(null);
                        },
                        activate: function () {
                            $(".k-window").find("form").css({ height: windowOptions.noFooter == undefined || windowOptions.noFooter==false ? windowOptions.height - 65 : windowOptions.height, "overflow-y": "auto", "overflow-x": "hidden" });
                            var autofocusElements = $(":input[autofocus]");
                            if (autofocusElements.length > 0) {
                                autofocusElements[0].focus();
                            }
                            windowOpenedDeferred.resolve(true);
                        },
                        resize: function () {
                            $(".k-window").find("form").css({ height: windowOptions.noFooter == undefined || windowOptions.noFooter == false ? parseInt(($(".k-window").css("height")).replace("px", "")) - 65 : parseInt(($(".k-window").css("height")).replace("px", "")), "overflow-y": "auto", "overflow-x": "hidden" });
                            if (typeof windowOptions.resize !== 'undefined' && $.isFunction(windowOptions.resize)) {

                                windowOptions.resize();
                            }
                        }
                    };

                    if (windowOptions.width) {
                        if (windowOptions.width > $(window).width() - 30) {
                            opts.width = $(window).width() - 30;
                        } else {
                            opts.width = windowOptions.width;
                        }
                    }
                    if (windowOptions.height) {
                        opts.height = windowOptions.height;
                    }
                    if (windowOptions.modal !== null) {
                        opts.modal = windowOptions.modal;
                    }
                    if (windowOptions.actions) {
                        opts.actions = windowOptions.actions;
                    }
                    if (windowOptions.noMaxHeight === null || windowOptions.noMaxHeight === false) {

                        if (windowOptions.maxHeight) {
                            opts.maxHeight = windowOptions.maxHeight;
                        }
                        else {
                            opts.maxHeight = 600;
                            opts.resizable = false;
                        }
                    }


                    if (windowOptions.center === null || windowOptions.center === false) {
                        var x = $(window).width() / 2;
                        var y = $(window).height() / 2;

                        var h = 600;
                        if (opts.height) {
                            h = opts.height;
                        }

                        opts.position = {
                            top: y - (h / 2),
                            left: x - (opts.width / 2)
                        };

                    }

                    var wnd = $("#" + windowInstance.id).kendoWindow(opts);
                    var dialog = $("#" + windowInstance.id).data("kendoWindow");
                    dialog.open();

                    if (windowOptions.center !== null || windowOptions.center === true) {
                        dialog.center();
                    }
                }, function () {
                    windowOpenedDeferred.reject(false);
                }); 
            });

            
            return windowInstance;
        }

        function getTemplatePromise(options) {
            if (options.content != undefined && options.content.type == "POST") {
                return options.template ? $q.when(options.template) :
                $http.post(options.content.url, options.content.data).then(function (result) {
                    return result.data;
                });
            } else {
                return options.template ? $q.when(options.template) :
                $http.get(options.url).then(function (result) {
                    return result.data;
                });
            }
        }

        function getResolvePromises(resolves) {
            var promisesArr = [];
            angular.forEach(resolves, function (value, key) {
                if (angular.isFunction(value) || angular.isArray(value)) {
                    promisesArr.push($q.when($injector.invoke(value)));
                }
            });
            return promisesArr;
        }
    };


    module.factory('$$stackedMap', function () {
        return {
            createNew: function () {
                var stack = [];

                return {
                    add: function (key, value) {
                        stack.push({
                            key: key,
                            value: value
                        });
                    },
                    get: function (key) {
                        for (var i = 0; i < stack.length; i++) {
                            if (key === stack[i].key) {
                                return stack[i];
                            }
                        }
                    },
                    keys: function () {
                        var keys = [];
                        for (var i = 0; i < stack.length; i++) {
                            keys.push(stack[i].key);
                        }
                        return keys;
                    },
                    top: function () {
                        return stack[stack.length - 1];
                    },
                    remove: function (key) {
                        var idx = -1;
                        for (var i = 0; i < stack.length; i++) {
                            if (key === stack[i].key) {
                                idx = i;
                                break;
                            }
                        }
                        return stack.splice(idx, 1)[0];
                    },
                    removeTop: function () {
                        return stack.splice(stack.length - 1, 1)[0];
                    },
                    length: function () {
                        return stack.length;
                    }
                };
            }
        };
    });

    module.factory('$windowStack', ['$document', '$compile', '$rootScope', '$$stackedMap',
        function ($document, $compile, $rootScope, $$stackedMap) {

            var body = $document.find('body').eq(0);
            var openedWindows = $$stackedMap.createNew();
            var $windowStack = {};

            function removeWindow(windowInstance) {
                var kendoWindow = openedWindows.get(windowInstance).value;

                $(kendoWindow.windowDomEl).data("kendoWindow").destroy();

                //clean up the stack
                openedWindows.remove(windowInstance);

                //remove window DOM element
                kendoWindow.windowDomEl.remove();

                //destroy scope
                kendoWindow.windowScope.$destroy();
            }

            $windowStack.open = function (windowInstance, kWindow) {
                openedWindows.add(windowInstance, {
                    deferred: kWindow.deferred,
                    windowScope: kWindow.scope,
                    keyboard: kWindow.keyboard
                });

                var angularDomEl = angular.element('<div id="' + windowInstance.id + '"></div>');
                angularDomEl.attr('window-class', kWindow.windowClass);
                angularDomEl.attr('index', openedWindows.length() - 1);
                angularDomEl.html(kWindow.content);

                var windowDomEl = $compile(angularDomEl)(kWindow.scope);
                openedWindows.top().value.windowDomEl = windowDomEl;
                body.append(windowDomEl);

            };

            $windowStack.close = function (windowInstance, result) {
                var kendoWindow = openedWindows.get(windowInstance).value;
                if (kendoWindow) {
                    kendoWindow.deferred.resolve(result);
                    removeWindow(windowInstance);
                }
            };

            $windowStack.dismiss = function (windowInstance, reason) {
                var kendoWindow = openedWindows.get(windowInstance).value;
                if (kendoWindow) {
                    kendoWindow.deferred.reject(reason);
                    removeWindow(windowInstance);
                }
            };

            $windowStack.getTop = function () {
                return openedWindows.top();
            };

            $windowStack.length = function() {
                return openedWindows.length();
            };

            return $windowStack;
        }
    ]);

}());
