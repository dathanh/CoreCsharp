(function () {
    'use strict';

    define(['angularAMD', 'sharedConfigController'], function (angularAmd) {

        var controllerId = "configController";

        angularAmd.controller(controllerId, configController);
        configController.$inject = ['$scope', 'common',  '$q','$window', '$location'];

        
        function configController($scope, common, $q,  $window, $location) {
            activate();
            var vm = this;
            vm.Id = 0;
            vm.actionState = 1; // 1.Grid, 2.RefreshGrid, 3.Add, 4. Added, 5. Update, 6.Updated, 7. Delete, 8. Deleted
            vm.modelName = 'Config';
            vm.route = '#/config';
            function activate() {
                common.activateController(null, controllerId).then(function () { });
                
            }
            $scope.setActionState = function (val) {
                vm.setActionState(val);
            }
            vm.setActionState = function(val) {
                vm.actionState = val;
            };
            vm.callBackAfterAddUpdateDelete = function () {
            };
            vm.edit = function(item) {
                var deferred = $q.defer();
                vm.actionState = 5;
                vm.Id = item.id;
                deferred.resolve(1);
                return deferred.promise;
            };
            vm.BackToList = function() {
                vm.actionState = 1;
            }

            vm.closePopupHandle = function () {
                $window.history.pushState(null, 'StarBerry', $location.absUrl());
            }

        }
    });
}());