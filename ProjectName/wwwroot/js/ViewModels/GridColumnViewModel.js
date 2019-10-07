function GridColumnViewModel(text, name, hidden, width, columnOrder, mandatory) {
    var self = this;

    self.Text = text;
    self.Name = name;
    self.HideColumn = hidden;
    self.ColumnWidth = width;
    self.ColumnOrder = columnOrder;
    self.Mandatory = mandatory;
}