namespace ProjectName.Models.Base
{
    public class ControlSharedViewModelBase
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public int Length { get; set; }
        public bool ReadOnly { get; set; }
        public string ClassCol { get; set; }
        public string ClassLabel { get; set; }
        public string ClassField { get; set; }

        public string ReadOnlyAttr => ReadOnly ? "readonly" : "";
        public bool Enabled { get; set; }
        public string DataBindingValue { get; set; }
    }
}