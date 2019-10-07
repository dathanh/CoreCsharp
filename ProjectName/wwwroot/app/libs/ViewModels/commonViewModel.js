'use strict';

define([], function() {
    return {
        CommonViewModel: function () {
            return {
                FeedbackViewModel: feedbackViewModel,
                GridColumnViewModel: gridColumnViewModel,
                GridConfigViewModel: gridConfigViewModel
            };

            
            function gridColumnViewModel(text, name, hidden, width, columnOrder, mandatory) {
                var self = this;
                self.Text = text;
                self.Name = name;
                self.HideColumn = hidden;
                self.ColumnWidth = width;
                self.ColumnOrder = columnOrder;
                self.Mandatory = mandatory;
            }
            function feedbackViewModel(status, message, description, modelStateArray) {
                var self = this;
                self.Status = status; // This is values such as "success", error", "critical"
                self.Message = message; // Primary Message that gets displayed
                self.Description = description; // Secondary message which gets displayed, should contain the debug info.
                self.ModelState = modelStateArray; // Contains an array of validation errors on the current form.
            }
            function gridConfigViewModel(id, userId, documentTypeId, gridInternalName) {
                var self = this;
                self.Id = id;
                self.DocumentTypeId = documentTypeId;
                self.UserId = userId;
                self.GridInternalName = gridInternalName;
                self.ViewColumns = [];

                self.importColumnConfigs = function (columnConfigs) {
                    if (!_.isNull(columnConfigs) && !_.isUndefined(columnConfigs))
                        columnConfigs.forEach(function (columnConfig) {
                            var viewColumn = new GridColumnViewModel(columnConfig.Text, columnConfig.Name, columnConfig.HideColumn, columnConfig.ColumnWidth, columnConfig.ColumnOrder, columnConfig.Mandatory);
                            self.ViewColumns.push(viewColumn);
                        });
                };
                self.addColumn = function (column) {
                    var columnOrder = 0;
                    if (!_.isNull(self.ViewColumns)) {

                        columnOrder = self.ViewColumns.length;
                    }
                    var viewColumn = new GridColumnViewModel(column.title, column.field, column.hidden, column.width, columnOrder, column.mandatory);
                    self.ViewColumns.push(viewColumn);
                };

                self.findViewColumn = function (column) {
                    var col = _.find(self.ViewColumns, function (viewColumn) {
                        if (!_.isUndefined(column.field)) {
                            return viewColumn.Name == column.field;
                        } else {
                            return viewColumn.Text == column.title;
                        }
                    });

                    return col;

                };

                self.findViewColumnIndex = function (column) {
                    var col = self.findViewColumn(column);;

                    if (!_.isUndefined(col) && !_.isNull(col)) {
                        return col.ColumnOrder;
                    }
                    return -1;
                };

                self.changeColumnConfig = function (column) {
                    var col = self.findViewColumn(column);

                    if (!_.isUndefined(col)) {
                        col.ColumnWidth = column.width;
                        col.HideColumn = column.hidden;
                    }
                };

                self.changeColumnOrder = function (column) {
                    var col = self.findViewColumn(column);

                    if (!_.isUndefined(col)) {
                        col.ColumnOrder = column.index;
                    }
                };

                self.getMandatoryColumns = function () {
                    return _.filter(self.ViewColumns, function (col) { return col.Mandatory; });
                };
            }
        }
    }
});