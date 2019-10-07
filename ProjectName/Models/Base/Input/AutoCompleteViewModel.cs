namespace ProjectName.Models.Base.Input
{
    public class AutoCompleteViewModel : ControlSharedViewModelBase
    {
        public bool Required { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";
        public string ReadUrl { get; set; }

        public string MoreClass { get; set; }

        public string PlaceHolderText { get; set; }
        public string ParameterDependencies { get; set; }
        public string OnchangeFunction { get; set; }
        public string RequireMarkColor { get; set; }
        public string RequireMarkPosition { get; set; }
    }
}