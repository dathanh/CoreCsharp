namespace Framework.DomainModel.ValueObject
{

    public class MenuViewModel
    {
        public bool CanViewUser { get; set; }
        public bool CanViewRole { get; set; }
        public bool CanViewCategory { get; set; }
        public bool CanViewVideo { get; set; }
        public bool CanViewSeries { get; set; }
        public bool CanViewBanner { get; set; }
        public bool CanViewConfig { get; set; }
        public bool CanViewPlayList { get; set; }
        public bool CanViewCustomer { get; set; }
    }
}
