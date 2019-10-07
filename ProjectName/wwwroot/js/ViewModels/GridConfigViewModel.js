function GridConfigViewModel(id, userId, documentTypeId, gridInternalName) {
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