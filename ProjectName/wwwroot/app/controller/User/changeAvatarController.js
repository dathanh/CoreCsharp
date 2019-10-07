(function() {
    'use strict';
    define(['angularAMD', 'masterfileService'], function(angularAmd) {

        var controllerId = "changeAvatarController";

        angularAmd.controller(controllerId, changeAvatarController);

        changeAvatarController.$inject = ['$rootScope', '$scope', '$state', 'masterfileService', 'common', 'config', 'messageLanguage', '$http', 'logger'];

        function changeAvatarController($rootScope, $scope, $state, masterfileService, common, config, messageLanguage, $http, logger) {
            $scope.controllerId = "changeAvatarController";
            var getLogFn = common.logger.getLogFn;

            $rootScope.close = function() {
                common.formCancelDataEvent('changeAvatarController');
            };

            $scope.User = new ChangeAvatarViewModel();
            $scope.Cancel = function() {
                var popup = $("#popupWindow").data("kendoWindow");
                popup.close();
            };
            $scope.Save = function() {

                var data = $scope.User;

                var url = angular.element("#avatarProfilePage")[0].src;
                if (url == undefined || url == "") {
                    logger.logError('Bạn cần phải kiểm tra lại các thông tin sau: <br /> 1. Bạn phải chọn hình ảnh.', null, null, 'error');
                    return;
                }
                var result = url.substring(url.lastIndexOf("/") + 1, url.length);
                if (result.length === url.length) {
                    result = url.substring(url.lastIndexOf("\\") + 1, url.length);
                }
                data.FileNameSaved = result;
                $http.post("User/ChangeAvatar", { SharedParameter: JSON.stringify(data) }, {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).success(function(response) {
                    if (response.Error === undefined || response.Error === '') {
                        var logSuccess = getLogFn("changeAvatarController", "success");
                        logSuccess("Thay đổi avatar thành công");
                        setTimeout(function() {
                            $state.reload();
                        }, 3500);
                        var popup = $("#popupWindow").data("kendoWindow");
                        popup.close();
                    }
                }).error(function(response) {
                });
            };
            $scope.cancelChanges = function() {
                $state.reload();
            };

            function callBackAfterUpdateSuccess() {
                $state.reload();
            }

            $scope.getData = function() {
                return { SharedParameter: JSON.stringify($scope.User) };
            }
            //Call back directive upload file
            $scope.callUploading = function() {
            };
            //Call back directive upload file
            $scope.callCompleted = function() {
                $scope.editUploadCompleted = false;
                $scope.$digest();
            };
            $scope.callCancelled = function() {
                $scope.editUploadCompleted = true;
                $scope.$digest();
                $state.reload();
            };

        };
    });
}());