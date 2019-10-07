namespace ProjectName.Models.Base.Input
{
    public class DualListBoxViewModel : ViewModelBase
    {
        public string ControlId { get; set; }
        public string ModelName { get; set; }
        public string GetAllUrl { get; set; }
        public string GetSelectedUrl { get; set; }
        public string HeaderText { get; set; }
    }
}