(function () {
    'use strict';

    define(['angularAMD', 'sharedRoleController', 'viewDetailRoleController'], function (angularAmd) {

        var controllerId = "roleController";

        angularAmd.controller(controllerId, roleController);
        roleController.$inject = [ '$scope', 'common', '$q',  'storeHistoryObject', '$injector'];

        function roleController( $scope, common,$q,  storeHistoryObject, $injector) {
            activate();
            var vm = this;
            vm.Id = 0;
            vm.actionState = 1; // 1.Grid, 2.RefreshGrid, 3.Add, 4. Added, 5. Update, 6.Updated, 7. Delete, 8. Deleted, 9.Detail
            vm.modelName = 'UserRole';
            vm.route = '#/role';
            function activate() {
                common.activateController(null, controllerId).then(function () { });
                $scope.$watch(function () {
                    return storeHistoryObject.storeHistory;
                }, function (nval, oval) {
                    if (nval.Id != undefined && storeHistoryObject.isSelfUrl(vm.route, nval.url)) {
                        if (nval.Id == 0) {
                            vm.actionState = 3;
                        } else {
                            vm.actionState = 5;
                        }
                        vm.Id = nval.Id;
                    }
                });
            }
            $scope.setActionState = function (val) {
                vm.setActionState(val);
            }
            vm.setActionState = function (val) {
                vm.actionState = val;
            }

            vm.add = function (item) {              
                var deferred = $q.defer();
                vm.actionState = 3;
                vm.Id = 0;
                deferred.resolve(1);
                return deferred.promise;
            }
            vm.edit = function (item) {
                var deferred = $q.defer();
                vm.actionState = 5;
                vm.Id = item.id;
                deferred.resolve(1);
                return deferred.promise;
            }

            vm.detail = function (item) {
                var deferred = $q.defer();
                $injector.get('$state').transitionTo('ViewDetailRole', { id: item.id });
                deferred.resolve(1);
                return deferred.promise;
            }
        }
    });
}());