
namespace ProjectName.Models.Base.Input
{
    public class InputNumericViewModel : ControlSharedViewModelBase
    {
        public int Width { get; set; }
        public string Format { get; set; }
        public dynamic MinimumValue { get; set; }
        public dynamic MaximumValue { get; set; }
        public double StepValue { get; set; }
        public string PlaceHolderText { get; set; }
        public int Decimals { get; set; }
        public bool Required { get; set; }
        public string RequiredAttribute => Required ? "required=\"required\"" : "";
    }
}