using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class CustomerPlaylistForMenu : DtoBase
    {
        public string Name { get; set; }
        public string NameUrl => Name.GetUrlViaName();
    }
}

