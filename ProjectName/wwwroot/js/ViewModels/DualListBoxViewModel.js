

function DualListBoxViewModel(id,
                            getAllUrl,
                            getSelectedUrl,
                            modelName) {

    var self = this;
    self.$container = $("#dual_list_" + id);
    self.$select = $('#dual_list_select_' + id);
    var loadDataForSelectedDocumentTypes = function (url) {

        self.dualListbox.trigger('bootstrapduallistbox.refresh');
        var $selectedControlOptions = $("#dual_list_select_" + id + " > option");


        $.ajax(
            {
                url: url,
                success: function (result) {
                    _.each(result, function (value, key) {
                        var displayName = value.Name.replace('<', '&lt;').replace('>', '&gt;');
                        self.dualListbox.append("<option value=" + value.Id + " selected>" + displayName + "</option>");
                        $selectedControlOptions.each(function () {
                            if (this.value == value.Id) {
                                $(this).remove();
                            }
                        });

                    });
                    // add to selected box
                    
                    self.dualListbox.trigger('bootstrapduallistbox.refresh');
                },
                type: 'GET',
                dataType: 'json',
                async: false,
                cache: false
            });
    };

    self.dualListbox = self.$select.bootstrapDualListbox({
        moveonselect: false,
        bootstrap2compatible: true,
        modelName: modelName,
    });
    self.dualViewModel = new DualListAllViewModel(id, getAllUrl);
    loadDataForSelectedDocumentTypes(getSelectedUrl);
}


function DualListAllViewModel(id, url) {

    //var self = this;
    self.ID = ko.observable(id);
    self.url = ko.observable(url);
    self.values = ko.observableArray([]);
    $.ajax({
        url: url,
        cache: false,
        type: 'GET',
        data: {},
        datatype: 'json',
        async: false,
        success: function (data) {
            $.each(data, function (index, value) {
                self.values.push(value);
            });
        }
    });
}