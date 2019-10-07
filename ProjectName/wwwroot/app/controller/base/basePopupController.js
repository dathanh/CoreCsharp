(function () {
    'use strict';

    define(['angularAMD', 'masterfileService'], function (angularAmd) {

        var controllerId = "basePopupController";

        angularAmd.controller(controllerId, basePopupController);

        basePopupController.$inject = ['$rootScope', '$scope', 'logger', 'common', 'commonConfig', 'config', 'masterfileService', '$q', '$timeout', '$window', '$location'];

        function basePopupController($rootScope, $scope, logger, common, commonConfig, config, masterfileService, $q, $timeout, $window, $location) {
            var getLogFn = logger.getLogFn;
            var log = getLogFn(controllerId);
            var basePopup = this;
            basePopup.$popupData = $q.defer();
            basePopup.modelName = '';
            basePopup.sharedControllerId = '';

            //common function
            function getSharedData() {
                if (basePopup.sharedControllerId === '') {
                    throw new Error('basePopup.shareDataControllerId is required.');
                }

                var postData = {};
                if ($scope.vms) {
                    postData = $scope.vms.getShareViewData();
                }

                return postData;
            }

            //basePopup function
            basePopup.create = function () {

                $rootScope.waitingModified = true;
                if (basePopup.modelName === '') {
                    throw new Error('basePopup.modelName is required.');
                }
                var params = getSharedData();
                masterfileService.Create(basePopup.modelName).perform(params).$promise
                    .then(function (result) {

                        if (result.Error === undefined || result.Error === '') {
                            $rootScope.popupFormModified = false;
                            var logSuccess = getLogFn(controllerId, "success");
                            logSuccess(common.getMessageFromSystemMessage('CreateSuccessfully', [basePopup.modelName]));
                            if ($scope.vms.handelAfterCreate != undefined && typeof $scope.vms.handelAfterCreate == "function") {
                                $scope.vms.handelAfterCreate(result);
                            }
                            else {
                                closeNoPopup(false, params);
                            }
                        }
                        $rootScope.waitingModified = false;
                    })
                    .catch(function (reason) {
                        var logError = getLogFn(controllerId, "error");
                        logError(reason);
                        $rootScope.waitingModified = false;
                    });

            };
            basePopup.update = function () {
                $rootScope.waitingModified = true;
                if (basePopup.modelName === '') {
                    throw new Error('basePopup.modelName is required.');
                }
                var params = getSharedData();
                masterfileService.Update(basePopup.modelName).perform(params).$promise
                    .then(function (result) {
                        if (result.Error === undefined || result.Error === '') {
                            $rootScope.popupFormModified = false;
                            var logSuccess = getLogFn(controllerId, "success");
                            logSuccess(common.getMessageFromSystemMessage('UpdateSuccessfully', [basePopup.modelName]));
                            closeNoPopup(true, params);
                        }
                        $rootScope.waitingModified = false;
                    })
                    .catch(function (reason) {
                        var logError = getLogFn(controllerId, "error");
                        logError(reason);
                        $rootScope.waitingModified = false;
                    });
            };

            basePopup.cancel = function () {
                $rootScope.popupFormModified = false;
                if (basePopup.popupInstall) {
                    basePopup.popupInstall.actionCancelMode = "cancel";
                    basePopup.popupInstall.close();
                } else {
                    closeNoPopup();
                }
            };

            function closeNoPopup(isUpdate, shareData) {

                if ($scope.vm != undefined && typeof $scope.vm.setActionState == "function") {
                    // 1.Grid, 2.RefreshGrid, 3.Add, 4. Added, 5. Update, 6.Updated, 7. Delete, 8. Deleted
                    $window.history.pushState(null, 'Chefjob Corporation', $location.absUrl());
                    if (isUpdate == undefined) {
                        // When cancel button click, just close popup and show grid
                        $scope.vm.setActionState(1);
                    } else {
                        $scope.vm.setActionState(2);
                    }

                    if ($scope.vm.closePopupHandle)//addition funtion for close popup Insert|update
                        $scope.vm.closePopupHandle(isUpdate, shareData);
                }
            }

            function getIdFromResponseCreate(result) {
                var id = 0;
                var array = $.map(result.toJSON(), function (value, index) {
                    return [value];
                });
                if (array.length > 0) {
                    id = array.join().replace(",", "");
                }
                return id;
            }

            basePopup.close = function (mode, params, result) {
                if (basePopup.popupInstall) {
                    basePopup.popupInstall.shareModel = common.decodeObject(JSON.parse(params.SharedParameter));
                    console.log(result);
                    if (result && parseInt(result) > 0) {
                        basePopup.popupInstall.shareModel.Id = parseInt(result);
                    }
                    basePopup.popupInstall.actionCancelMode = mode;
                    basePopup.popupInstall.close();
                }
            }

            basePopup.showCancel = true;

            basePopup.styleCancel = {};
        }
    });
}());