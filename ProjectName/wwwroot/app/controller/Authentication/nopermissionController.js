(function () {
    'use strict';

    define(['angularAMD', 'accountService'], function (angularAmd) {

        var controllerId = "nopermissionController";

        angularAmd.controller(controllerId, nopermissionController);

        nopermissionController.$inject = ['$rootScope', '$scope', 'logger', 'common', 'commonViewModel', 'accountService', '$injector', '$timeout'];

        function nopermissionController($rootScope, $scope, logger, common, commonViewModelAmd, accountService, $injector, $timeout) {
            
            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);
            var vm = this;

            activate();

            function activate() {
                common.activateController(null, controllerId).then(function () { });
            }


            vm.login = function () {
                $injector.get('$state').transitionTo('Login');
            };
        }
    });
}());