
using Framework.Utility;
using Newtonsoft.Json;
using System;

namespace Framework.DomainModel.ValueObject
{
    public class CommentItem
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        public string IdFrontEnd { get; set; }
        [JsonProperty(PropertyName = "parent")]
        public int? ParentId { get; set; }
        [JsonProperty(PropertyName = "creator")]
        public int CustomerId { get; set; }
        [JsonProperty(PropertyName = "upvote_count")]
        public int CountLike { get; set; }
        [JsonProperty(PropertyName = "downvote_count")]
        public int CountDislike { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "fullname")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "user_has_upvoted")]
        public bool HasLike { get; set; }
        [JsonProperty(PropertyName = "user_has_downvoted")]
        public bool HasDislike { get; set; }
        [JsonIgnore]
        public DateTime? CreatedValue { get; set; }
        [JsonProperty(PropertyName = "created")]
        public string Created
        {
            get
            {
                if (CreatedValue.GetValueOrDefault() == DateTime.MinValue)
                {
                    return "";
                }
                return CreatedValue.GetValueOrDefault().ToString(ConstantValue.DateFormatComment);
            }
        }
        [JsonIgnore]
        public byte[] AvatarValue { get; set; }
        [JsonProperty(PropertyName = "profile_picture_url")]
        public string Avatar => AvatarValue == null ? "" : "data:image/jpg;base64," + Convert.ToBase64String(AvatarValue);
        [JsonProperty(PropertyName = "child_count")]
        public int ChildCount { get; set; }
    }
}
