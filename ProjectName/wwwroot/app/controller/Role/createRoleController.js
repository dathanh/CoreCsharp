(function () {
    'use strict';

    define(['angularAMD', 'masterfileService'], function (angularAmd) {

        var controllerId = "createRoleController";

        angularAmd.controller(controllerId, createRoleController);

        createRoleController.$inject = ['$rootScope', '$scope', 'logger', 'common', '$controller', '$timeout'];

        function createRoleController($rootScope, $scope, logger, common, $controller, $timeout) {

            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);
            var vm = this;
            vm.basePopup = $controller('basePopupController', { $scope: $scope });
            vm.deRegisterEvent = $rootScope.$on('BroadcastDataFromKendoWindow_' + controllerId, function (event, data) {
                vm.basePopup.$popupData.resolve(data);
                vm.basePopup.modelName = data.modelName;
                vm.deRegisterEvent();
            });
            vm.basePopup.sharedControllerId = 'sharedRoleController';
            
            
            activate();

            function activate() {
                common.activateController(null, controllerId).then(function () {  });
            }

            //implement
            
        }
    });
}());