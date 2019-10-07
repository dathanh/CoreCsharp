(function () {
    'use strict';

    define(['angularAMD'], function (angularAmd) {

        angularAmd.directive('cisGrid', cisGrid);

        function cisGrid() {
            return {
                restrict: "E",
                scope: {},
                replace: true,
                transclude: true,
                link: link,
                bindToController: {
                    gridId: '@',
                    userId: '@',
                    modelName: '@',
                    documentTypeId: '@',
                    gridInternalName: '@',
                    viewSchemaConfigData: '@',
                    viewColumnsConfigData: '@',
                    customHeaderTemplate: '@',
                    popupWidth: '@',
                    popupHeight: '@',
                    customParams: '@',
                    disableAutoBind: '@',
                    addFuc: '=',
                    cancelFuc: '=',
                    editFuc: '=',
                    deleteFuc: '=',
                    extFunc1: '=',
                    extFunc2: '=',
                    extFunc3: '=',
                    extFunc4: '=',
                    extFunc5: '=',
                    parentSearch: '=',
                    detailInitBind: '=',
                    isLazyPaging: '@',
                    boundFunc: '=',
                    detailTemplateVariable: '@',
                    numberLineHeader: '@'
                },
                controller: cisgridController,
                controllerAs: 'gridVm',
                template: '<div kendo-grid k-options="gridVm.mainGridOptions" id="{{gridVm.modelName}}"></div>'
            };
        }

        function link(scope, element, attrs) {
        }

        cisgridController.$inject = ['$rootScope', '$scope', '$http', 'angularKendoWindowService', 'masterfileService', 'common', 'logger', '$timeout', '$localStorage'];

        function cisgridController($rootScope, $scope, $http, kendoWindowService, masterfileService, common, logger, $timeout, $localStorage) {
            var getLogFn = logger.getLogFn;
            var ctrl = this;
            ctrl.parentController = $scope.$parent;
            var schemaFields = JSON.parse(ctrl.viewSchemaConfigData);
            var columns = JSON.parse(ctrl.viewColumnsConfigData);
            var customParams = [];
            if (ctrl.customParams != undefined && ctrl.customParams != null) {
                customParams = JSON.parse(ctrl.customParams);
            }

            //process gridConfig
            ctrl.gridConfigViewModel = new gridConfigViewModel(0, ctrl.userId, ctrl.documentTypeId, ctrl.gridInternalName);
            angular.forEach(columns, function (value, key) {
                var columnViewModel = new gridColumnViewModel(value.title, value.field, value.hidden, value.width, key, value.mandatory);
                ctrl.gridConfigViewModel.ViewColumns.push(columnViewModel);

                if (value.template != null && value.template != '') {
                    value.template = kendo.template($("#" + value.template).html());
                }
            });

            //process header template
            var customHeaderTemplate = "#templateHeader";
            if (ctrl.customHeaderTemplate != null && ctrl.customHeaderTemplate != undefined && ctrl.customHeaderTemplate !== "") {
                customHeaderTemplate = "#" + ctrl.customHeaderTemplate;
            }

            var getUrl = "/" + ctrl.modelName + "/" + "GetDataForGrid";
            ctrl.UserType = 0;
            ctrl.UserType = $scope.$parent.UserTypeId;
            ctrl.IsGetTotalForGrid = true;
            ctrl.TotalRecordCount = 0;
            var dataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: getUrl,
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options.models) {
                            return { models: kendo.stringify(options.models) };
                        } else if (operation === "read") {

                            // Limitation of nested json object; temporarily have to modify the json to
                            // pass in sort information correctly.
                            var result = {
                                pageSize: options.pageSize,
                                skip: options.skip,
                                take: options.take,
                                SearchString: ctrl.SearchTextEncoded,
                                AdditionalSearchString: ctrl.AdditionalSearchString,
                                IsLazyPaging: ctrl.isLazyPaging,
                                QueryId: $scope.$parent.$parent != null ? $scope.$parent.$parent.QueryId : "",
                                UserType: $scope.$parent.UserTypeId
                            };
                            if (options.sort) {
                                for (var i = 0; i < options.sort.length; i++) {
                                    result["sort[" + i + "].field"] = options.sort[i].field;
                                    result["sort[" + i + "].dir"] = options.sort[i].dir;
                                }
                            }

                            for (var i = 0; i < customParams.length; i++) {
                                result[customParams[i]] = $scope.$parent[customParams[i]];
                            }

                            return result;
                        }
                    }
                },
                requestStart: function () {

                    $('[data-toggle="tooltip"]').tooltip("destroy");
                    if (dataSource.totalPages() == dataSource.page()) {
                        ctrl.IsGetTotalForGrid = true;
                    }
                    if (requestGetTotalForGrid != null) {
                        requestGetTotalForGrid.abort();
                    }
                },
                serverPaging: true,
                serverSorting: true,
                pageSize: 50,
                batch: true,
                schema: {
                    model: {
                        id: "Id",
                        fields: schemaFields
                    },
                    data: "Data",
                    //total: "TotalRowCount",
                    total: function (response) {
                        if (ctrl.isLazyPaging == "true") {
                            if (ctrl.IsGetTotalForGrid) {
                                $timeout(function () {
                                    lazyPaging();
                                }, 500);
                                return response.Data.length;
                            } else {
                                return ctrl.TotalRecordCount > 0 ? ctrl.TotalRecordCount : (response.Data.length > 0 ? 1 : 0);
                            }
                        } else {
                            return response.TotalRowCount;
                        }
                    }
                },
            });
            var hGrid = $("#content-container").find(".content-tab-index").length > 0 && $("#content-container").find(".no-affect-content-tab-index").length == 0 ? $(window).height() - 183 : $(window).height() - 120;
            
            if (ctrl.numberLineHeader != null && ctrl.numberLineHeader != "" && ctrl.numberLineHeader != "1") {
                var numLine = parseInt(ctrl.numberLineHeader);
                hGrid = hGrid - (numLine - 1) * 30;
            }



            if (ctrl.detailTemplateVariable != null && ctrl.detailTemplateVariable != "") {
                ctrl.mainGridOptions = {
                    dataSource: dataSource,
                    toolbar: kendo.template($(customHeaderTemplate).html()),
                    pageable: {
                        refresh: true,
                        pageSizes: [10, 25, 50, 100, 1000],
                        buttonCount: 5
                    },
                    sortable: {
                        mode: "single",
                        allowUnsort: false,
                    },
                    filterable: false,
                    height: hGrid,
                    //autoBind: (ctrl.disableAutoBind == false || ctrl.disableAutoBind == "false"),
                    columns: columns,
                    editable: false,
                    selectable: false,
                    scrollable: { virtual: false },
                    navigatable: false,
                    resizable: true,
                    reorderable: true,
                    columnResize: changeColumnConfig,
                    columnHide: changeColumnConfig,
                    columnShow: changeColumnConfig,
                    columnReorder: changeColumnPosition,
                    columnMenu: false,
                    detailTemplate: kendo.template($("#" + ctrl.detailTemplateVariable).html()),
                    //detailInit: detailInitFunction,
                    dataBound: function (e) {
                        getGridConfig(ctrl.gridConfigViewModel);
                        //if (dataSource.total() === 0) {
                        //    $(".k-pager-wrap").hide();
                        //}
                        //scope.$emit("gridDataboundEvent", { gridId: scope.modelName });
                        $('#dropdown-export a').on('click', function (event) {
                            event.stopPropagation();
                        });
                        $('[data-toggle="tooltip"]').tooltip({
                            container: 'body',
                            html: true
                        });
                        //Set lai chieu cao cho grid khi bi loi nhan button login sau do click wa tab khac cua trinh duyet lien
                        var gridContent = $('#' + ctrl.modelName + ' .k-grid-content');
                        if (gridContent != undefined && gridContent.height() + 150 < hGrid) {
                            gridContent.height(hGrid - 128);
                        }
                       
                       
                        if (typeof ctrl.boundFunc == "function") {
                            ctrl.boundFunc().then(function (result) {
                                //console.log('editFuc result:' + result);
                            });
                        }
                    },
                    columnMenuInit: function (e) {
                        e.container.find('li.k-item.k-sort-asc[role="menuitem"]').remove();
                        e.container.find('li.k-item.k-sort-desc[role="menuitem"]').remove();
                        e.container.find('.k-columns-item .k-group').css({ 'width': '200px', 'max-height': '400px' });
                    },
                    change: function (e) {
                        e.preventDefault();
                        return;
                    },
                    detailExpand: function (e) {
                        ctrl.DetailInitGridBind(e);
                        var grid = this;
                        var currentRow = e.masterRow;
                        var masterRows = this.table.find("tr.k-master-row");
                        var mappedRows = $.map(masterRows, function (row, rowIndex) {
                            if ($(row).find("a.k-minus").length > 0 && !$(row).is(currentRow)) {
                                return $(row);
                            }
                            return null;
                        });
                        for (var i = 0; i < mappedRows.length; i++) {
                            grid.collapseRow(mappedRows[i]);
                        }
                    },
                };
            } else {
                ctrl.mainGridOptions = {
                    dataSource: dataSource,
                    toolbar: kendo.template($(customHeaderTemplate).html()),
                    pageable: {
                        refresh: true,
                        pageSizes: [10, 25, 50, 100, 1000],
                        buttonCount: 5
                    },
                    sortable: {
                        mode: "single",
                        allowUnsort: false,
                    },
                    filterable: false,
                    height: hGrid,
                    //autoBind: (ctrl.disableAutoBind == false || ctrl.disableAutoBind == "false"),
                    columns: columns,
                    editable: false,
                    selectable: false,
                    scrollable: { virtual: false },
                    navigatable: false,
                    resizable: true,
                    reorderable: true,
                    columnResize: changeColumnConfig,
                    columnHide: changeColumnConfig,
                    columnShow: changeColumnConfig,
                    columnReorder: changeColumnPosition,
                    columnMenu: false,
                    dataBound: function (e) {
                        getGridConfig(ctrl.gridConfigViewModel);
                        //if (dataSource.total() === 0) {
                        //    $(".k-pager-wrap").hide();
                        //}
                        //scope.$emit("gridDataboundEvent", { gridId: scope.modelName });
                        $('#dropdown-export a').on('click', function (event) {
                            event.stopPropagation();
                        });
                        $('[data-toggle="tooltip"]').tooltip({
                            container: 'body',
                            html: true
                        });
                        //Set lai chieu cao cho grid khi bi loi nhan button login sau do click wa tab khac cua trinh duyet lien
                        var gridContent = $('#' + ctrl.modelName + ' .k-grid-content');
                        if (gridContent != undefined && gridContent.height() + 150 < hGrid) {
                            gridContent.height(hGrid - 128);
                        }
                        var toolbarHeaderHeight = $(".k-grid-toolbar").height();
                        if (toolbarHeaderHeight > 40) {
                            var currentHeight = gridContent.height();
                            currentHeight = currentHeight - (toolbarHeaderHeight - 34);
                            gridContent.height(currentHeight);
                        }

                        if (typeof ctrl.boundFunc == "function") {
                            ctrl.boundFunc().then(function (result) {
                                //console.log('editFuc result:' + result);
                            });
                        }
                    },
                    columnMenuInit: function (e) {
                        e.container.find('li.k-item.k-sort-asc[role="menuitem"]').remove();
                        e.container.find('li.k-item.k-sort-desc[role="menuitem"]').remove();
                        e.container.find('.k-columns-item .k-group').css({ 'width': '200px', 'max-height': '400px' });
                    },
                    change: function (e) {
                        e.preventDefault();
                        return;
                    }
                };
            }
            var requestGetTotalForGrid;
            function lazyPaging() {
                $("#" + ctrl.modelName + " .k-pager-numbers").html("<li id='loadingPaging'><img src='/css/kendo/Bootstrap/loading-image.gif' alt='Loading paging...'/></li>");
                if (requestGetTotalForGrid != null) {
                    requestGetTotalForGrid.abort();
                }
                requestGetTotalForGrid = $.ajax({
                    url: "/" + ctrl.modelName + "/GetTotalForGrid",
                    data: {
                        SearchString: ctrl.searchTextEncode,
                        StringListStatusSelected: JSON.stringify(ctrl.listItemStatus)
                    },
                    async: true
                })
                    .success(function (total) {
                        ctrl.TotalRecordCount = total;
                        dataSource._total = parseInt(ctrl.TotalRecordCount);
                        var grid = $('#' + ctrl.modelName).data('kendoGrid');
                        if (grid != undefined && grid.pager != undefined) {
                            grid.pager.refresh();
                        }

                        $("#loadingPaging").remove();
                        ctrl.IsGetTotalForGrid = false;
                    })
                    .error(function () {
                        $("#loadingPaging").remove();
                    });
            };
            var bootboxConfirmObj;
            ctrl.Delete = function (id) {
                if ($localStorage.menuList["CanDelete" + ctrl.modelName] == false) {
                    var logError = getLogFn("", "error");
                    logError(common.getMessageFromSystemMessage('UnAuthorizedAccessText'));
                    return;
                }
                if (bootboxConfirmObj != null) {
                    common.hideAllBootboxConfirm();
                }
                var count = 0;
                bootboxConfirmObj = common.bootboxConfirm("Do you want to delete this item?", function () {
                    if (count == 0) {
                        count++;
                        masterfileService.DeleteById(ctrl.modelName).perform({ id: id }).$promise
                            .then(function (result) {
                                if (result.Error === undefined || result.Error === '') {
                                    var logSuccess = getLogFn("", "success");
                                    logSuccess(common.getMessageFromSystemMessage('DeleteSuccessfully', [ctrl.modelName]));

                                    if (ctrl.parentController.vm && ctrl.parentController.vm.callBackAfterAddUpdateDelete && typeof ctrl.parentController.vm.callBackAfterAddUpdateDelete === 'function') {
                                        ctrl.parentController.vm.callBackAfterAddUpdateDelete(result);
                                    }
                                    ctrl.IsGetTotalForGrid = true;
                                    dataSource.read();
                                }
                            })
                            .catch(function (reason) {
                            });
                    } else {
                        return;
                    }

                }, function () { }).modal('show');
            };
            ctrl.GridExtFunc1 = function (id) {
                if (typeof ctrl.extFunc1 == "function") {
                    $('[data-toggle="tooltip"]').tooltip('hide');
                    ctrl.extFunc1({ id: id }).then(function (result) {
                        //console.log('editFuc result:' + result);
                    });
                }
            }

            ctrl.GridExtFunc2 = function (id) {
                if (typeof ctrl.extFunc2 == "function") {
                    $('[data-toggle="tooltip"]').tooltip('hide');
                    ctrl.extFunc2({ id: id }).then(function (result) {
                        //console.log('editFuc result:' + result);
                    });
                }
            }

            ctrl.GridExtFunc3 = function (id) {
                if (typeof ctrl.extFunc3 == "function") {
                    $('[data-toggle="tooltip"]').tooltip('hide');
                    ctrl.extFunc3({ id: id }).then(function (result) {
                        //console.log('editFuc result:' + result);
                    });
                }
            }

            ctrl.GridExtFunc4 = function (id) {
                if (typeof ctrl.extFunc4 == "function") {
                    $('[data-toggle="tooltip"]').tooltip('hide');
                    ctrl.extFunc4({ id: id }).then(function (result) {
                        //console.log('editFuc result:' + result);
                    });
                }
            }

            ctrl.GridExtFunc5 = function (id) {
                if (typeof ctrl.extFunc5 == "function") {
                    $('[data-toggle="tooltip"]').tooltip('hide');
                    ctrl.extFunc5({ id: id }).then(function (result) {
                        //console.log('editFuc result:' + result);
                    });
                }
            }

            ctrl.DetailInitGridBind = function (e) {
                if (typeof ctrl.detailInitBind == "function") {
                    $('[data-toggle="tooltip"]').tooltip('hide');
                    ctrl.detailInitBind(e).then(function (result) {

                    });
                }
            }

            ctrl.Edit = function (id) {
                if ($localStorage.menuList["CanUpdate" + ctrl.modelName] == false) {
                    var logError = getLogFn("", "error");
                    logError(common.getMessageFromSystemMessage('UnAuthorizedAccessText'));
                    return;
                }
                if (typeof ctrl.editFuc == "function") {
                    $('[data-toggle="tooltip"]').tooltip('hide');
                    ctrl.editFuc({ id: id }).then(function (result) {
                        //console.log('editFuc result:' + result);
                    });

                } else {
                    var pw = parseInt(ctrl.popupWidth.toString().replace("px", ""));
                    ctrl.popupWidth = $(window).width() - 20 > pw ? pw + "px" : $(window).width() - 20 + "px";
                    if ($rootScope.IsLoadingPopup === false) {
                        $rootScope.IsLoadingPopup = true;
                        var title = ctrl.modelName.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) { return str.toUpperCase(); });
                        var windowInstance = kendoWindowService.ShowKendoWindow({
                            modal: true,
                            title: 'Update ' + title,
                            width: ctrl.popupWidth,
                            height: ctrl.popupHeight,
                            url: '/' + ctrl.modelName + '/Update/' + id,
                            controller: ['update' + ctrl.modelName + 'Controller', 'shared' + ctrl.modelName + 'Controller'],
                            resolve: {
                                modelName: function () {
                                    return ctrl.modelName;
                                }
                            }
                        });

                        windowInstance.result.then(function (result) {
                            if (ctrl.parentController.vm && ctrl.parentController.vm.callBackAfterAddUpdateDelete && typeof ctrl.parentController.vm.callBackAfterAddUpdateDelete === 'function') {
                                ctrl.parentController.vm.callBackAfterAddUpdateDelete(result);
                            }
                            dataSource.read();
                        });
                    }
                }
            };

            ctrl.Add = function () {
                if ($localStorage.menuList["CanAdd" + ctrl.modelName] == false) {
                    var logError = getLogFn("", "error");
                    logError(common.getMessageFromSystemMessage('UnAuthorizedAccessText'));
                    return;
                }
                if (typeof ctrl.addFuc == "function") {
                    $('[data-toggle="tooltip"]').tooltip('hide');
                    ctrl.addFuc({ id: 0 }).then(function (result) {
                        //console.log(result);
                    });

                } else {
                    var pw = parseInt(ctrl.popupWidth.toString().replace("px", ""));
                    ctrl.popupWidth = $(window).width() - 20 > pw ? pw + "px" : $(window).width() - 20 + "px";
                    if ($rootScope.IsLoadingPopup === false) {
                        $rootScope.IsLoadingPopup = true;
                        var title = ctrl.modelName.replace(/([A-Z])/g, ' $1').replace(/^./, function (str) { return str.toUpperCase(); });
                        var windowInstance = kendoWindowService.ShowKendoWindow({
                            modal: true,
                            title: 'Create ' + title,
                            width: ctrl.popupWidth,
                            height: ctrl.popupHeight,
                            url: '/' + ctrl.modelName + '/Create',
                            controller: ['create' + ctrl.modelName + 'Controller', 'shared' + ctrl.modelName + 'Controller'],
                            resolve: {
                                modelName: function () {
                                    return ctrl.modelName;
                                }
                            }
                        });

                        windowInstance.result.then(function (result) {
                            if (ctrl.parentController.vm && ctrl.parentController.vm.callBackAfterAddUpdateDelete && typeof ctrl.parentController.vm.callBackAfterAddUpdateDelete === 'function') {
                                ctrl.parentController.vm.callBackAfterAddUpdateDelete(result);
                            }
                            dataSource.read();
                        });
                    }
                }

            };

            ctrl.ExportToExcel = function () {
                var gridId = "#" + ctrl.modelName;
                var grid = $(gridId).data("kendoGrid");
                var gridColumnsConfig = _.filter(grid.columns, function (obj) {
                    return obj.field !== "Command" && typeof obj.template !== "function";
                });

                var gridColumns = _.map(gridColumnsConfig, function (obj) {
                    return { Field: obj.field, Title: obj.title };
                });

                var sort = grid.dataSource.sort();

                var searchText = ctrl.SearchText;
                searchText = "<SearchTerms>" + Encoder.htmlEncode(searchText) + "</SearchTerms>";
                searchText = Base64.encode('<AdvancedQueryParameters>' + searchText + '</AdvancedQueryParameters>');

                var total = grid.dataSource.total();
                var selected = $("#dropdown-export input[type='radio']:checked");
                if (selected.length > 0) {
                    if (parseInt(selected.val()) > 0) {
                        total = parseInt(selected.val());
                    }
                }

                var queryInfo = {
                    SearchString: searchText,
                    Sort: sort,
                    Take: total,
                    AdditionalSearchString: ctrl.AdditionalSearchString,
                };
                for (var i = 0; i < customParams.length; i++) {
                    queryInfo[customParams[i]] = $scope.$parent[customParams[i]];
                }
                var exportUrl = "/" + ctrl.modelName + "/" + "ExportExcel";
                var downloadExcelUrl = "/" + ctrl.modelName + "/" + "DownloadExcelFile";

                $http.post(exportUrl, { queryInfo: queryInfo, gridColumns: gridColumns }).success(function (result) {
                    $.fileDownload(downloadExcelUrl, {
                        httpMethod: "POST",
                        dataType: "json",
                        contentType: 'application/json; charset=utf-8',
                        data: { 'fileName': result.FileNameResult }
                    });
                }).error(function (err) {
                });
            };
            ctrl.SetSearchText = function () {

            }
            function SearchData() {
                var searchText = ctrl.SearchText;
                if (searchText == "null" || searchText == undefined || searchText == "undefined") {
                    searchText = "";
                }

                searchText = "<SearchTerms>" + searchText + "</SearchTerms>";
                searchText = Base64.encode('<AdvancedQueryParameters>' + searchText + '</AdvancedQueryParameters>');
                ctrl.SearchTextEncoded = searchText;

                dataSource._skip = 0;
                ctrl.IsGetTotalForGrid = true;
                dataSource.read();
            }

            ctrl.Search = function () {
                if (typeof ctrl.parentSearch == "function") {
                    ctrl.parentSearch().then(function (result) {
                        SearchData();
                    });
                }
                else {
                    SearchData();
                }
            };

            $scope.$on(ctrl.gridId + "_ReloadGrid", function () {
                dataSource._skip = 0;
                dataSource.read();
            });


            $scope.$on(ctrl.gridId + "_ReloadGridAndGetTotalForGrid", function () {
                ctrl.IsGetTotalForGrid = true;
                dataSource._skip = 0;
                dataSource.read();
            });

            ctrl.DischargeMember = function (id) {
                ctrl.parentController.$parent.vm.discharge(id);
            }


            function changeColumnConfig(event) {
                var columnChanged = event.column;

                var col = _.find(ctrl.gridConfigViewModel.ViewColumns, function (viewColumn) {
                    if (!_.isUndefined(columnChanged.field)) {
                        return viewColumn.Name === columnChanged.field;
                    } else {
                        return viewColumn.Text === columnChanged.title;
                    }
                });

                col.HideColumn = columnChanged.hidden;
                col.ColumnWidth = columnChanged.width;

                saveGridConfig();
            }

            function changeColumnPosition(event) {
                var oldIndex = event.oldIndex;
                var newIndex = event.newIndex;

                if (newIndex === columns.length - 1) {
                    setTimeout(function () {
                        $('#' + ctrl.modelName).data("kendoGrid").reorderColumn(oldIndex, event.column);
                    });
                }
            }

            function saveGridConfig() {
                var gridConfig = angular.toJson(ctrl.gridConfigViewModel);
                //Save grid's config
                $http.post('/GridConfig/Save', gridConfig)
                    .success(function (result) {
                        if (result.Error === undefined || result.Error === '') {
                            ctrl.gridConfigViewModel.Id = result.Data.Id;
                        }
                    }).error(function (error) {
                    });
            }

            function getGridConfig() {
                //processing on mandatory columns
                var mandatoryFields = _.filter(columns, function (col) { return col.mandatory; });

                mandatoryFields.forEach(function (col) {
                    $('#' + ctrl.modelName + ">.k-grid-header table>thead").find("[data-field='" + col.field + "']>.k-header-column-menu").remove();
                });

                $('#' + ctrl.modelName).data("kendoDraggable").bind("dragstart", function (e) {
                    var dataField = e.currentTarget[0].attributes['data-field'].value;
                    mandatoryFields.forEach(function (col) {
                        $('#' + ctrl.modelName + ">.k-grid-header table>thead").find("[data-field='" + col.field + "']>.k-header-column-menu").remove();
                        if (_.isUndefined(dataField) || dataField === "" || dataField === col.field) {
                            e.preventDefault();
                            return;
                        }
                    });
                });


                //get gridConfig on database
                $http.post('/GridConfig/Get', angular.toJson(ctrl.gridConfigViewModel))
                    .success(function (result) {
                        if (result.Error === undefined || result.Error === '') {
                            if (result.ViewColumns) {
                                ctrl.gridConfigViewModel.Id = result.Id;

                                _.each(columns, function (column) {
                                    var col = _.find(result.ViewColumns, function (viewColumn) {
                                        if (!_.isUndefined(column.field)) {
                                            return viewColumn.Name === column.field;
                                        } else {
                                            return viewColumn.Text === column.title;
                                        }
                                    });

                                    if (col) {
                                        column.width = col.ColumnWidth;
                                        column.hidden = col.HideColumn;
                                    }
                                });
                            }
                        }
                    }).error(function (error) {
                    });
            }

        }

        //object
        function gridColumnViewModel(text, name, hidden, width, columnOrder, mandatory) {
            var self = this;

            self.Text = text;
            self.Name = name;
            self.HideColumn = hidden;
            self.ColumnWidth = width;
            self.ColumnOrder = columnOrder;
            self.Mandatory = mandatory;
        }

        function gridConfigViewModel(id, userId, documentTypeId, gridInternalName) {
            var self = this;

            self.Id = id;
            self.DocumentTypeId = documentTypeId;
            self.UserId = userId;
            self.GridInternalName = gridInternalName;
            self.ViewColumns = [];
        }
    });
}());