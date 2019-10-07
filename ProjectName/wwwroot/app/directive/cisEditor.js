(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisEditor', cisEditor);

        function cisEditor() {
            return {
                restrict: "E",
                scope: {
                    modelBinding: '=ngModel'
                },
                require: 'ngModel',
                transclude: true,
                bindToController: {
                    editorId: '@',
                    urlRead: '@',
                    urlDestroy: '@',
                    urlImage: '@',
                    urlCreate: '@',
                    urlUpload: '@',
                    urlThumb: '@',
                    editorWidth: '@',
                    editorHeight: '@',
                    isBasic: '@'
                },
                controller: cisEditorController,
                controllerAs: 'editorVm',
                template: '<textarea id="{{editorVm.editorId}}" style="height:{{editorVm.editorHeight}}px;" kendo-editor k-ng-model="modelBinding" k-options="editorVm.options"></textarea>'
            };
        };

        cisEditorController.$inject = ['$rootScope', '$scope', 'common', 'commonViewModel', '$timeout'];

        function cisEditorController($rootScope, $scope, common, commonViewModelAmd, $timeout) {
            var ctrl = this;
            var toolOption = [];
            if (ctrl.isBasic) {
                toolOption = [
                    "bold",
                    "italic",
                    "underline",
                    "strikethrough",
                    "justifyLeft",
                    "justifyCenter",
                    "justifyRight",
                    "justifyFull",
                    "insertUnorderedList",
                    "insertOrderedList",
                    "indent",
                    "outdent",
                    "viewHtml",
                    "fontSize",
                ];
            }
            else {
                toolOption = [
                    "bold",
                    "italic",
                    "underline",
                    "strikethrough",
                    "justifyLeft",
                    "justifyCenter",
                    "justifyRight",
                    "justifyFull",
                    "insertUnorderedList",
                    "insertOrderedList",
                    "indent",
                    "outdent",
                    "createLink",
                    "unlink",
                    "insertImage",
                    "subscript",
                    "superscript",
                    "foreColor",
                    "createTable",
                    "addRowAbove",
                    "addRowBelow",
                    "addColumnLeft",
                    "addColumnRight",
                    "deleteRow",
                    "deleteColumn",
                    "viewHtml",
                    "fontSize",
                ];
            }
            ctrl.options = {
                tools: toolOption,
                imageBrowser: {
                    messages: {
                        dropFilesHere: "Drop files here"
                    },
                    transport: {
                        read: ctrl.urlRead,
                        destroy: {
                            url: ctrl.urlDestroy,
                            type: "POST"
                        },
                        create: {
                            url: ctrl.urlCreate,
                            type: "POST"
                        },
                        thumbnailUrl: ctrl.urlThumb,
                        uploadUrl: ctrl.urlUpload,
                        imageUrl: ctrl.urlImage
                    }
                },
                change: function (e) {
                },
                keydown: function (e) {
                    var content = this.value();
                    $scope.$apply(function () {
                        $scope.modelBinding = content;
                        $rootScope.popupFormModified = true;
                    });
                },
            };
            $timeout(function () {
                //var editor = $("#" + ctrl.editorId).data("kendoEditor");
                
                //if (editor != undefined && editor.body!=undefined) {
                //    if (editor.body.outerHTML.indexOf('url("/content/fonts/alice.ttf")') < 0) {
                //        $('<style>@font-face {font-family: "Alice_5";src: url("/content/fonts/alice.ttf") format("truetype");}</style>').appendTo(editor.body);
                //    }
                //    if (editor.body.outerHTML.indexOf('url("/content/fonts/futura-lt-bt-light.ttf")') < 0) {
                //        $('<style>@font-face {font-family: "Futura Lt BT",sans-serif;src: url("/content/fonts/futura-lt-bt-light.ttf") format("truetype");}</style>').appendTo(editor.body);
                //    }
                //    if (editor.body.outerHTML.indexOf('url("/content/fonts/futura-md-bt.ttf")') < 0) {
                //        $('<style>@font-face {font-family: "Futura Md BT",sans-serif;src: url("/content/fonts/futura-md-bt.ttf") format("truetype");}</style>').appendTo(editor.body);
                //    }
                //}


                var wrappers = $(".editor-wrap");
                wrappers.each(function (idx, element) {
                    var wrapper = $(element);
                    // add resize handle
                    var resizeHandle = $("<span class='k-icon k-resize-se' />").appendTo(wrapper);

                    resizeHandle.kendoDraggable({
                        dragstart: function (e) {
                            // overlay iframe to prevent event gap
                            wrapper.append("<div class='k-overlay' />");
                            // cache some offsets for later use
                            this.top = wrapper.offset().top - this.element.height();
                            this.left = wrapper.offset().left - this.element.width();
                            var win = $(window);
                            this.scrollTop = win.scrollTop();
                            this.scrollLeft = win.scrollLeft();
                        },
                        drag: function (e) {
                            // update wrapper height
                            wrapper.find(".k-editor").height((e.clientY || e.originalEvent.clientY) - this.top + this.scrollTop);
                            wrapper.width((e.clientX || e.originalEvent.clientX) - this.left + this.scrollLeft);
                        },
                        dragend: function (e) {
                            // remove overlay
                            wrapper.children(".k-overlay").remove();
                        }
                    });
                });
            });
        }
    });
}());