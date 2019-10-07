using System;

namespace Framework.DomainModel.ValueObject
{
    public class CustomerQueryInfo : QueryInfo
    {
        public DateTime? RegisterStartDate { get; set; }
        public DateTime? RegisterEndDate { get; set; }
        public int? Type { get; set; }
    }
}