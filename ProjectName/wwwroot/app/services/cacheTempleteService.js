(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {
        
        angularAmd.factory('cacheTempleteService', cacheTempleteService);

        cacheTempleteService.$inject = ['masterfileService', 'logger', '$templateCache', '$localStorage', '$http', 'cfpLoadingBar'];

        function cacheTempleteService(masterfileService, logger, $templateCache, $localStorage, $http, cfpLoadingBar) {
            var cacheList = [
            ];
            var index;
            return {
                run: run,
                set: set,
                get: get,
                clear: clear,
                //setCache: setCache
            };
            function clear() {
                $localStorage["appVersion"] = null;
                run();
            }

            function run() {
                $('#loading-request').css('background-color', '');
                if ($localStorage["appVersion"] != appVersion) {
                    $localStorage["cacheTemplete"] = null;
                    $localStorage["appVersion"] = appVersion;
                    index = 0;
                    privateSetCache();
                    
                }
            }

            function privateGetCache(url) {
                var cacheTemplete = $localStorage["cacheTemplete"];
                if (cacheTemplete) {
                    for (var i = 0; i < cacheTemplete.length; i++) {
                        if (cacheTemplete[i].key === url) {
                            return cacheTemplete[i].content;
                        }
                    }
                }
                return null;
            }

            function privateSetCache() {
                cfpLoadingBar.set(0);
                if (index >= cacheList.length - 1) {
                    $('#loading-request').css('background-color', '#c0c0c0');
                    return;
                }
                $http.get(cacheList[index]).then(function (result) {
                    
                    var cacheTemplete = $localStorage["cacheTemplete"];
                    if (cacheTemplete == undefined) {
                        cacheTemplete = [];
                        cacheTemplete.push({ key: cacheList[index], content: result.data });
                    } else {
                        cacheTemplete.push({ key: cacheList[index], content: result.data });
                    }
                    $localStorage["cacheTemplete"] = cacheTemplete;
                    index++;
                    privateSetCache();
                });
               
            }

            function set(name, content, url) {
            }

            function get(name) {
                return privateGetCache(name);
            }
        }
    });

}());
