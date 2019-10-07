function LookupViewModel(id, modelName, hierarchyGroupName, urlReadData, isReadOnly, canEditTextLookup, heightLookup) {
    var self = this;

    self.ID = id;
    self.ModelName = modelName;
    var modelUrl = "";
    if (urlReadData != undefined && urlReadData != "") {
        modelUrl = urlReadData;
    } else {
        modelUrl = "/" + self.ModelName + "/GetLookup";
    }
    if (heightLookup == undefined) {
        heightLookup = 250;
    }
    self.ModelUrl = modelUrl;
    self.SelectedText = "";
    self.SelectedValue = 0;
    self.CurrentID = 0;
    self.OpenDropDown = false;
    self.Meassage = "";


    self.SelectedValue.subscribe(function (newValue) {
        //var notifiedData = { value: newValue, text: self.SelectedText(), clearChildren: self.OnChanged() };
        var notifiedData = { value: newValue, text: self.SelectedText(), clearChildren: false };
        if (hierarchyGroupName == undefined) hierarchyGroupName = "";
        Postbox.messages.notifySubscribers(notifiedData, self.ModelName() + "_" + hierarchyGroupName);
        self.OnChanged(false);
    });

    var parentItems = new Array();
    self.ParentItems = parentItems;
    self.FilterItem = "";
    self.OnChanged = false;

    self.FlagFirst = ko.observable(false);
    self.ValueFirst = ko.observable('');
    self.Lookup = function (container, options, kendoGridModel, notGetdataFromServer) {
        var clientDataSource = [{ KeyId: 0, DisplayName: "", Title: "" }];
        if (options != null) {
            if (options.dataSource != undefined && options.dataSource.length > 0)
                clientDataSource = options.dataSource;
        }
        var dataSource = undefined;
        var serverDataSource = new kendo.data.DataSource({
            serverFiltering: true,
            type: "json",
            schema: { model: { id: "KeyId", fields: { KeyId: {}, DisplayName: {}, Title: {} } } },
            transport: {
                read: {
                    url: self.ModelUrl(),
                    dataType: "json",
                    type: "GET"
                },
                parameterMap: function (options, type) {
                    var value = '';
                    var includeCurrentRecord = false;
                    if (options.filter !== undefined && options.filter.filters != undefined && options.filter.filters[0] != undefined) {
                        value = options.filter.filters[0].value;
                    }

                    if (!self.FlagFirst()) {
                        value = self.ValueFirst();
                        self.FlagFirst(true);
                    }

                    var currentID = (container.val() != '') ? container.val() : 0;

                    if (kendoGridModel != undefined) currentID = kendoGridModel.KeyId; // kendoeditor works a bit differently. 
                    if (self.CurrentID() == 0)
                        self.CurrentID(currentID);

                    if (self.OpenDropDown())
                        includeCurrentRecord = true;

                    return {
                        Id: currentID,
                        IncludeCurrentRecord: includeCurrentRecord,
                        Query: value,
                        ParentItems: JSON.stringify(self.ParentItems()),
                        Take: 150
                    };
                }
            }
        });

        dataSource = new kendo.data.DataSource({
            serverFiltering: true,
            data: clientDataSource
        });
        var strTemplate = "";
        if (isReadOnly == undefined || isReadOnly == null || isReadOnly == false) {
            strTemplate = "<span class=\"k-state-default\"";
            strTemplate += " title=\"#: data.Title #\"";
            strTemplate += ">#: data.DisplayName #</span><a href=\"javascript:ShowPopupWhenClickOnLookup(800, 600, '" + id + "','', '/" + modelName + "/Update?type=1&id=#: data.KeyId #')\"><span class=\"k-icon k-i-pencil right\" title=\"Update\"></span></a>";
        } else {
            strTemplate = "<span class=\"k-state-default\"";
            strTemplate += " title=\"#: data.Title #\"";
            strTemplate += ">#: data.DisplayName #</span>";
        }
        container
            .kendoComboBox({
                highlightFirst: false,
                dataTextField: "DisplayName",
                dataValueField: "KeyId",
                filter: "contains",
                autoBind: true,
                minLength: 0,
                suggest: true,
                height: heightLookup,
                //Nghiep Fix
                dataSource: dataSource,
                //dataSource: serverDataSource,
                template: strTemplate,

                // Needed to force the combo to refresh as transport.read.cache doesn't seem to work
                open: function (e) {
                    self.OpenDropDown(true);
                    self.OnChanged(false);
                    //self.SelectedText(this.text());                    
                    if (this.text() == '') { // When user clears the textbox                                    
                        $("#" + self.ID()).val('');
                    }

                    var combo = container.data("kendoComboBox");
                    if (notGetdataFromServer != true) {
                        self.ValueFirst(this.text());
                        //console.log(this.text());
                        combo.setDataSource(serverDataSource);
                    } else {
                        kendoGridModel.set(options.valueField, { Name: self.SelectedText(), KeyId: self.SelectedValue() });
                        kendoGridModel.set(options.keyIdField, self.SelectedValue());
                    }
                    // change the datasource to server whenever client clicks on the dropdown. This is for performance improvement.
                },

                change: function () {

                    $('#lookup-container-' + id).parents('form').addClass('dirty');
                    EnableCreateFooterButton(true);
                    if (this._data().length == 1 && this.text() !== '') {
                        var selected = this._data()[0];

                        this.text(selected.DisplayName);
                        this.value(selected.KeyId);
                    } else if (this.selectedIndex === -1) {
                        $("#" + self.ID()).val(0).trigger('change'); // for knockout
                    }

                    self.OnChanged(true);
                    self.SelectedText(this.text());
                    self.SelectedValue(this.value());
                    if (this.selectedIndex === -1) { // When user clears the textbox                        
                        this.value("");
                    }

                    // This is to handle binding back to editor of kendo grid if lookup is used in the kendo.
                    if (options != null && kendoGridModel != null && kendoGridModel != undefined) {
                        if (self.SelectedValue() == 0) {
                            self.SelectedText("");
                            this.value("");
                            this.text("");
                        }
                        kendoGridModel.set(options.valueField, { Name: self.SelectedText(), KeyId: self.SelectedValue() });
                        kendoGridModel.set(options.keyIdField, self.SelectedValue());
                        Postbox.messages.notifySubscribers(self.SelectedValue(), self.ModelName() + "_" + Postbox.CHANGE_VALUE_FOR_COMBOBOX_IN_GRID);
                    }
                    Postbox.messages.notifySubscribers(self.SelectedValue(), self.ModelName() + "_Parent");

                    $("#" + self.ID()).trigger('click'); // for knockout
                }
            });

        if (clientDataSource != null && clientDataSource.length > 0) {
            var containerValue = clientDataSource[0].KeyId;
            if (containerValue == 0)
                containerValue = '';
            container.data("kendoComboBox").value(containerValue);
            self.OnChanged(false);
            self.SelectedValue(containerValue);
        }
        $('#lookup-container-' + id + ' > span > span > span > span').attr('class', 'k-icon k-i-search');
        $('#' + id).parent().parent().find(" > span > span > span > span").attr('class', 'k-icon k-i-search');
        if ($('#' + id).attr('required') != undefined) {
            $('#' + id).parent().find('.k-input').attr('required', $('#' + id).attr('required'));
            $('#' + id).parent().find('.k-input').attr('data-required-msg', $('#' + id).attr('data-required-msg'));
        }
        // Post message for parent to notify bind data CHANGE_DATASOURCE_FOR_COMBOBOX
        Postbox.messages.notifySubscribers('', self.ModelName() + "_" + Postbox.CHANGE_DATASOURCE_FOR_COMBOBOX + "_Parent");
    };

    self.GetSelectedValue = function () {
        return self.SelectedValue();
    };
    self.UpdateSelection = function (keyId) {
        var kendoCombo = $("#" + self.ID()).data("kendoComboBox");
        if (kendoCombo == null) return;
        kendoCombo.value(keyId);
        $("#" + self.ID()).val(keyId).trigger('change'); // for knockout

        if (keyId == '')
            self.OnChanged(true);
        self.SelectedText(kendoCombo.text());
        self.SelectedValue(keyId);

    };

    self.UpdateParentFilter = function (parentItems, searchName, searchValue, searchTextValue) {
        var match = null;

        var parents = self.ParentItems();
        if (searchValue == "" || searchValue == undefined)
            searchValue = "0";
        for (var i = 0; i < parents.length; i++) {
            if (parents[i].name == searchName) {
                match = parents[i];

            }
        }
        if (searchTextValue == undefined) {
            searchTextValue = "";
        }
        if (match == null)
            parents.push({ name: searchName, value: searchValue, text: searchTextValue });
        else {
            match.value = searchValue;
            match.text = searchTextValue;
        }
    };

    self.UpdateSelectionBaseOnChild = function (filterItem) {

        if (filterItem.value == undefined || filterItem.value == "")
            return; // don't need to populate parent
        if (filterItem.returnPopup == undefined || filterItem.returnPopup == false) {
            if (self.SelectedValue() != 0 && self.SelectedValue() != '' && self.SelectedValue() != undefined) {
                return; // don't need to populate itself when it has the selected from 1st time binding
            }
        }
        // Lookup parent item
        var url = "/" + self.ModelName() + "/GetLookupItem";
        if (filterItem.modelName != null && filterItem.modelName != '') {
            url = "/" + filterItem.modelName + "/GetLookupItem";
        }

        $.ajax({
            type: 'GET',
            url: url,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: { FilterItem: JSON.stringify(filterItem) },
            async: false,
            success: function (result) {
                if (result != undefined) {
                    var kendoCombo = $("#" + self.ID()).data("kendoComboBox");
                    if (kendoCombo == undefined) {
                        return;
                    }
                    var selectedID = kendoCombo.value();
                    if (selectedID == result.KeyId) return;
                    var currentData = kendoCombo.dataSource.data();

                    var items = _.where(data, { KeyId: result.KeyId });
                    if (items.length == 0) // the populated item from child doesn't belong to current dataSource (because of record limitation).
                    {
                        var data = [{ KeyId: result.KeyId, DisplayName: result.DisplayName }];
                        dataSource = new kendo.data.DataSource({
                            serverFiltering: true,
                            data: data
                        });

                        kendoCombo.setDataSource(dataSource);
                    }
                    self.UpdateSelection(result.KeyId);
                    self.OnChanged(false);
                    if (filterItem.returnPopup != undefined && filterItem.returnPopup == true) {
                        self.SelectedValuePopup(result.KeyId);
                    }
                }
            }
        });
    };
    Postbox.messages.subscribe(function (returnViewModel) {
        if (returnViewModel == undefined) {
            return;
        }
        if (returnViewModel.TypeId != undefined && returnViewModel.TypeId == id && parseInt(returnViewModel.KeyId) > 0) {
            var kendoCombo = $("#" + self.ID()).data("kendoComboBox");
            var clientDataSource = [{ KeyId: returnViewModel.KeyId, DisplayName: returnViewModel.DisplayName }];
            var dataSource = new kendo.data.DataSource({
                serverFiltering: true,
                data: clientDataSource
            });
            kendoCombo.setDataSource(dataSource);
            self.UpdateSelection(returnViewModel.KeyId);
            self.SelectedValuePopup(returnViewModel.KeyId);
            if (parseInt(returnViewModel.KeyId) > 0) {
                $("#icon-pencil-" + id).removeClass("hide");
                $("#icon-plus-" + id).addClass("hide");

            } else {
                $("#icon-pencil-" + id).addClass("hide");
                $("#icon-plus-" + id).removeClass("hide");
            }
        }
        Postbox.messages.notifySubscribers(self.SelectedValue(), self.ModelName() + "_" + Postbox.RETURN_VALUE_TO_COMBOBOX);
    }, null, Postbox.RETURN_VALUE_TO_COMBOBOX);

    Postbox.messages.subscribe(function (value) {
        var kendoCombo = $("#" + self.ID()).data("kendoComboBox");
        kendoCombo.value("");
    }, null, id + "_" + Postbox.CLEAR_VALUE_TO_COMBOBOX);

    Postbox.messages.subscribe(function (returnViewModel) {
        var kendoCombo = $("#" + self.ID()).data("kendoComboBox");
        var clientDataSource = [{ KeyId: returnViewModel.KeyId, DisplayName: returnViewModel.DisplayName }];
        var dataSource = new kendo.data.DataSource({
            serverFiltering: true,
            data: clientDataSource
        });
        if (kendoCombo != undefined) {
            kendoCombo.setDataSource(dataSource);
        }
        self.UpdateSelection(returnViewModel.KeyId);
        //console.log(kendoCombo);
        //kendoCombo.enable(false);
    }, null, id + "_" + Postbox.CHANGE_DATASOURCE_FOR_COMBOBOX);
}