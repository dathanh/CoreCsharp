using Framework.Service.Translation;
using Newtonsoft.Json;

namespace ProjectName.Models.Base
{
    public partial class ViewModelBase
    {
        [JsonIgnore]
        public string CreateText => SystemMessageLookup.GetMessage("CreateText");
        [JsonIgnore]
        public string UpdateText => SystemMessageLookup.GetMessage("UpdateText");
        [JsonIgnore]
        public string DirtyDialogMessageText => SystemMessageLookup.GetMessage("DirtyDialogMessageText");
    }
}