(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        var controllerId = "userControllerForPassword";

        angularAmd.controller(controllerId, userController);

        userController.$inject = ['$rootScope', '$scope', 'logger', 'common', 'angularKendoWindowService', 'accountService', '$timeout'];

        function userController($rootScope, $scope, logger, common, kendoWindowService, accountService, $timeout) {

            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);
            var vm = this;

            activate();

            function activate() {
                common.activateController(null, controllerId).then(function () {  });

            }

            vm.callBackAfterAddUpdateDelete = function(result) {
                
            };

           
            //implement
            vm.SetPassword = function(id) {
                var windowInstance = kendoWindowService.ShowKendoWindow({
                    modal: true,
                    title: 'SET PASSWORD',
                    width: 500,
                    height: 150,
                    url: '/User/SetPassword/',
                    controller: ['setPasswordController'],
                    resolve: {
                        userId: function () {
                            return id;
                        }
                    }
                });

                windowInstance.result.then(function (result) {
                    //alert(result);
                });
            };
        }
    });
}());