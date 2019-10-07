(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisUpload', cisUploadDirective);
        cisUploadDirective.$inject = ['fileTypeValue'];
        function cisUploadDirective(fileTypeValue) {
            return {
                restrict: "E",
                scope: {
                    modelBinding: '=ngModel'
                },
                require: 'ngModel',
                transclude: true,
                bindToController: {
                    uploadId: '=',
                    saveUrl: '@',
                    removeUrl: '=',
                    previewHeight: '=',
                    previewWidth: '=',
                    isUploadImage: '=',
                    isUploadVideo: '=',
                    isChangeAvatar: '=',
                    isReturnObject: '=',
                    isAllowMultiFile: '=',
                    acceptType: '=',
                    selectText: '=',
                    onsuccessfuc: '=',
                },
                controller: cisUploadController,
                controllerAs: 'uploadVm',
                templateUrl: 'app/directive/template/Upload.html'
            }
        };

        cisUploadController.$inject = ['$rootScope', '$scope', 'common', 'commonViewModel', 'masterfileService', '$timeout'];

        function cisUploadController($rootScope, $scope, common, commonViewModelAmd, masterfileService, $timeout) {
            var ctrl = this;

            //declare default value

            //event
            ctrl.onSelect = function (e) {
                $rootScope.waitingModified = true;
                $rootScope.$apply();

                if (!ctrl.isAllowMultiFile && (e.files.length > 1 || ($scope.modelBinding != null && $scope.modelBinding.length >= 1))) {
                    //Yen
                    if (!ctrl.isChangeAvatar) {
                        common.showErrorModelState(new commonViewModelAmd.FeedbackViewModel("error", "Không cho phép upload nhiều hình", "", []));
                        e.preventDefault();
                        return false;
                    }
                }

                $.each(e.files, function (index, value) {
                    var ok = value.extension === ".JPG" || value.extension === ".jpg"
                        || value.extension === ".JPEG" || value.extension === ".jpeg"
                        || value.extension === ".PNG" || value.extension === ".png";

                    var okFile = value.extension === ".DOC" || value.extension === ".doc"
                        || value.extension === ".DOCX" || value.extension === ".docx"
                        || value.extension === ".PDF" || value.extension === ".pdf"
                        || value.extension === ".JPG" || value.extension === ".jpg"
                        || value.extension === ".JPEG" || value.extension === ".jpeg"
                        || value.extension === ".PNG" || value.extension === ".png"
                        || value.extension === ".TIFF" || value.extension === ".tiff"
                        || value.extension === ".TIF" || value.extension === ".tif"
                || value.extension === ".BACKUP" || value.extension === ".backup";

                    var excelFile = value.extension === ".xls" || value.extension === ".xlsx"
                        || value.extension === ".XLS" || value.extension === ".XLSX";

                    var okBackup = value.extension === ".BACKUP" || value.extension === ".backup";

                    if (ctrl.isUploadImage && value.size > 5 * 1024 * 1024) {
                        common.showErrorModelState(new commonViewModelAmd.FeedbackViewModel("error", "File size more than 5MB can not be uploaded", "", []));
                        e.preventDefault();
                        $rootScope.waitingModified = false;
                        $rootScope.$apply();
                        return false;
                    }
                    if (ctrl.isUploadImage && !ok) {
                        e.preventDefault();
                        common.showErrorModelState(new commonViewModelAmd.FeedbackViewModel("error", "Vui lòng upload jpg/png image file", "", []));
                        $rootScope.waitingModified = false;
                        $rootScope.$apply();
                        return false;
                    }
                    
                    if (!ctrl.isUploadVideo &&!ctrl.isUploadImage && !okFile && !excelFile) {
                        e.preventDefault();
                        common.showErrorModelState(new commonViewModelAmd.FeedbackViewModel("error", "Following business rules failed: <ol><li>Please upload word/pdf/png/jpg/tiff file.</li></ol>", "", []));
                        $rootScope.waitingModified = false;
                        $rootScope.$apply();
                        return false;
                    }
                });
            };

            ctrl.onSuccess = function (e) {
                if ($scope.modelBinding === undefined || $scope.modelBinding === null) {
                    $scope.modelBinding = [];
                }

                var file = e.response.file;
                var filePath = e.response.FilePath;
                var originalUrl = e.response.OriginalUrl;
                if (file !== undefined && file !== null && file.length > 0) {
                    e.files[0].name = file[0];
                    $scope.$apply(function () {
                        if (ctrl.isUploadVideo) {
                            var item = {
                                VideoLink: originalUrl + filePath + file[0],
                                FilePath: filePath + file[0],
                                //Thumnail: filePath + "Thumnail_" + file[0],
                                FileNameSaved: file[0],
                                FileNameOriginal: e.files[0].rawFile.name
                            }
                            var temp = angular.extend(item, $scope.modelBinding);
                            $scope.modelBinding = [];
                            $scope.modelBinding.push(temp);
                        }
                        else if (!ctrl.isAllowMultiFile) {
                            if (ctrl.isReturnObject === true) {
                                $scope.modelBinding = { fileName: e.files[0].rawFile.name, fileNameStore: file[0] };
                            } else {
                                $scope.modelBinding = originalUrl + filePath + file[0];
                            }

                        } else {
                            var item = {
                                ImageUrl: originalUrl + filePath + file[0],
                                FilePath: filePath + file[0],
                                //Thumnail: filePath + "Thumnail_" + file[0],
                                FileNameSaved: file[0],
                                FileNameOriginal: e.files[0].rawFile.name
                            }
                            var temp = angular.extend(item, $scope.modelBinding);
                            if ($scope.modelBinding == null) {
                                $scope.modelBinding = [];
                            }                            
                            $scope.modelBinding.push(temp);

                        }
                    });
                }
                $rootScope.waitingModified = false;
                $rootScope.$apply();
                if (typeof ctrl.onsuccessfuc == "function") {
                    ctrl.onsuccessfuc(e);
                }
            };

            ctrl.onRemove = function (e) {

                $scope.$apply(function () {

                    if (!ctrl.isAllowMultiFile) {
                        $scope.modelBinding = "";
                    } else {

                        $scope.modelBinding = _.without($scope.modelBinding, _.findWhere($scope.modelBinding, { FileNameSaved: e.files[0].name }));

                    }
                });
            };

            ctrl.deleteFile = function (fileName) {
                masterfileService.CallWithUrl(ctrl.removeUrl).perform({ fileNames: fileName }).$promise
                    .then(function (result) {
                        $timeout(function () {
                            if (!ctrl.isAllowMultiFile) {
                                $scope.modelBinding = "";
                            } else {

                                $scope.modelBinding = _.without($scope.modelBinding, _.findWhere($scope.modelBinding, { FilePath: fileName }));

                            }

                        });

                    })
                    .catch(function (reason) {
                    });
            };
        }
    });
}());