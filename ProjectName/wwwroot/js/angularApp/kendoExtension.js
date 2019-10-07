kendo.data.DataSource.prototype.AcceptChanges = function () {
    var that = this,
        idx,
        length,
        data = that._flatData(that._data);

    for (idx = 0, length = data.length; idx < length; idx++) {
        if (data[idx].dirty) {
            data[idx].dirty = false;
        }
    }
    return true;
};
kendo.data.DataSource.prototype.GetUnsavedData = function () {
    var that = this,
        idx,
        length,
        created = [],
        updated = [],
        destroyed = that._destroyed,
        data = that._flatData(that._data);

    for (idx = 0, length = data.length; idx < length; idx++) {
        if (data[idx].dirty) {
            updated.push(data[idx]);
        } else if (data[idx].Id == 0 && data[idx].isNew()) {
            console.log(data[idx]);
            created.push(data[idx]);
        }
    }

    var unsavedData = JSON.stringify({ created: created, destroyed: destroyed, updated: updated });
    return unsavedData;
};

kendo.data.DataSource.prototype.GetAllData = function () {
    var that = this,
        idx,
        length,
        listAll = [],
        data = that._flatData(that._data);

    for (idx = 0, length = data.length; idx < length; idx++) {
        listAll.push(data[idx]);
    }

    var allData = JSON.stringify({ listAll: listAll });
    return allData;
};

kendo.data.DataSource.prototype.GetDualSelectUnsavedData = function () {
    var that = this,
        idx,
        length,
        created = [],
        updated = [],
        destroyed = that._destroyed,
        data = that._flatData(that._data);

    for (idx = 0, length = data.length; idx < length; idx++) {
        if (data[idx].dirty || data[idx].isNew()) {
            updated.push(data[idx]);
        }
    }

    var unsavedData = JSON.stringify({ created: created, destroyed: destroyed, updated: updated });
    return unsavedData;
};
function dirtyField(data, fieldName) {
    if (data === null || data === undefined || data.dirtyFields === undefined || data.dirtyFields === null)
        return "";

    if (data.dirty && data.dirtyFields[fieldName]) {
        return "<span class='k-dirty'></span>";
    } else {
        return "";
    }
}

// change from date to string so that kendo date format and mvc date format are consistent.
// otherwise, it includes utc and mvc date is normally moved backward one day.
Date.prototype.toJSON = function () {
    return kendo.toString(this, "u");
};
