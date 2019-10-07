using System;

namespace Framework.DomainModel.ValueObject
{
    [Serializable]
    public class Sort
    {
        public string Field { get; set; }

        public string Dir { get; set; }
    }
}