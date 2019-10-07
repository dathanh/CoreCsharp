using Newtonsoft.Json;
using ProjectName.Models.Base;

namespace ProjectName.Models.MyProfile
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DashboardMyProfileShareViewModel : DashboardSharedViewModel
    {

        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string FullName { get; set; }
        [JsonProperty]
        public string Phone { get; set; }
        [JsonProperty]
        public string Avatar { get; set; }
        [JsonProperty]
        public string UserName { get; set; }

        [JsonProperty]
        public string Email { get; set; }

    }
}