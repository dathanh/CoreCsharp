(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisPopup', cisPopupDirective);
        
        function cisPopupDirective() {
            return {
                restrict: "E",
                scope: true,
                transclude: true,
                bindToController: {
                    popupId: '@',
                    popupOptions: '=',
                    popup: '=',
                    popupView: '@',
                },
                controller: cisPopupController,
                controllerAs: 'popupVm',
                template: function (elem, attr) {
                    var template = '<div ' +
                        'id="' + attr.popupId + '" ' +
                        'kendo-window="' + attr.popup + '" ' +
                        'k-options="' + attr.popupOptions + '" ' +
                        ' k-visible="false" k-modal="true" ' +
                        '></div>';
                    return template;
                }
            };
        }

        cisPopupController.$inject = ['$rootScope', '$scope', '$controller', '$attrs', '$timeout', '$window', 'common'];

        function cisPopupController($rootScope, $scope, $controller, $attrs, $timeout, $window, common) {
            var ctrl = this;
            // Set center Screen.

            ctrl.open = function (e) {
                ctrl.popup.actionCancelMode = 'close';
                $(".k-window").find("form").css({ height: ctrl.popupOptions.noFooter == undefined || ctrl.popupOptions.noFooter == false ? ctrl.popupOptions.height - 65 : ctrl.popupOptions.height, "overflow-y": "auto", "overflow-x": "hidden" });
            }
            ctrl.close = function (e) {
                if (ctrl.popupOptions && typeof ctrl.popupOptions.closePopup == "function") {
                    ctrl.popupOptions.closePopup(ctrl.popup.shareModel, ctrl.popup.actionCancelMode);
                }
                if (ctrl.popupOptions && typeof ctrl.popupOptions.closePopupWithData == "function") {
                    ctrl.popupOptions.closePopupWithData(ctrl.popup.data);
                }
                if (ctrl.popupOptions && typeof ctrl.popupOptions.closeAndBackPage == "function") {
                    ctrl.popupOptions.closeAndBackPage(ctrl.popup.actionBackMode);
                }
            }
            ctrl.resize = function (e) {
                $(".k-window").find("form").css({ height: ctrl.popupOptions.noFooter == undefined || ctrl.popupOptions.noFooter == false ? parseInt(($(".k-window").css("height")).replace("px", "")) - 65 : parseInt(($(".k-window").css("height")).replace("px", "")), "overflow-y": "auto", "overflow-x": "hidden" });
            }
            angular.element($window).bind('orientationchange', function () {
                ctrl.popup.center();
            });

            $scope.$on("kendoWidgetCreated", function (event, widget) {
                // the event is emitted for every widget; if we have multiple
                // widgets in this controller, we need to check that the event
                // is for the one we're interested in.
                //When this kendo generate control, this event will set center sreen for kendo windows
                $timeout(function() {
                    if (widget === ctrl.popup) {
                        ctrl.popup.actionCancelMode = null;
                        ctrl.popup.actionBackMode = null;
                        ctrl.popup.bind("resize", ctrl.resize);
                        ctrl.popup.bind("open", ctrl.open);
                        ctrl.popup.bind("close", ctrl.close);
                        ctrl.popup.center();
                        //Mode action close: 1 click x buton, 2 click cancel button
                        //bind 2 function openPopup and closePopup
                        ctrl.popup.openPopup = function (data) {
                            ctrl.popup.shareModel = data;
                            common.checkAndSetWidthPopup(ctrl.popup);
                            ctrl.popup.center().open();
                        }

                        ctrl.popup.closePopup = function () {
                            ctrl.popup.close();
                            return { shareModel: ctrl.popup.shareModel, mode: ctrl.popup.actionCancelMode };
                        }
                        ctrl.popup.closePopupWithData = function () {
                            ctrl.popup.close();
                            return { data: ctrl.popup.data };
                        }
                        ctrl.popup.closeAndBackPage = function () {
                            ctrl.popup.close();
                            return { mode: ctrl.popup.actionBackMode };
                        }
                        $(document).on("click", ".k-overlay", function () {
                            ctrl.popup.close();
                        });

                    }
                });
            });
        }
    });
}());