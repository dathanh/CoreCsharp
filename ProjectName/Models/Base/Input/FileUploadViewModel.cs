namespace ProjectName.Models.Base.Input
{
    public class FileUploadViewModel : ControlSharedViewModelBase
    {
        public bool Required { get; set; }

        public string RequiredAttribute => Required ? "required=\"required\"" : "";

        public string SaveUrl { get; set; }

        public string RemoveUrl { get; set; }

        public int PreviewHeight { get; set; }

        public int PreviewWidth { get; set; }

        public string AcceptType { get; set; }

        public string FileNameSave { get; set; }

        public bool IsMultiple { get; set; }
        public bool IsAvatar { get; set; }
        public bool IsUploadImage { get; set; }
        public bool IsChangeAvatar { get; set; }
        public bool IsAllowMultiFile { get; set; }
        public string SelectText { get; set; }
        public bool IsReturnObject { get; set; }
        public string OnsuccessFunction { get; set; }
        public bool IsUploadVideo { get; set; }
    }
}