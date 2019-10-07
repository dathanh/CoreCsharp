using Newtonsoft.Json;

namespace Framework.DomainModel.ValueObject
{
    public class LookupItem
    {
        public int Id { get; set; }

        public LookupHierarchyItem HierachyItem
        {
            get
            {
                try
                {
                    return FilterItem == null ? null : JsonConvert.DeserializeObject<LookupHierarchyItem>(FilterItem);
                }
                catch
                {
                    return null;
                }
            }
        }

        public string FilterItem { get; set; }
    }
}