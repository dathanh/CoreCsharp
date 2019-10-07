namespace Framework.DomainModel.ValueObject
{
    public class FileAttachmentUploadDto
    {
        public string FilePath { get; set; }
        public string FileNameSaved { get; set; }
        public string FileNameOriginal { get; set; }
        public string ImageUrl { get; set; }
    }
}
