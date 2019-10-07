using System;
using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public interface IQueryInfo
    {
        int Take { get; set; }
        int Skip { get; set; }
        int QueryId { get; set; }
        //bool ActiveRecords { get; set; }
        //bool InactiveRecords { get; set; }
        int TotalRecords { get; set; }
        string SearchTerms { get; set; }
        DateTime? CreatedBefore { get; set; }
        DateTime? CreatedAfter { get; set; }
        int CreatedBy { get; set; }
        DateTime? ModifiedBefore { get; set; }
        DateTime? ModifiedAfter { get; set; }
        int ModifiedBy { get; set; }
        List<Sort> Sort { get; set; }

        string SortString { get; }
        string SearchString { get; set; }
        string AdditionalSearchString { get; set; }
        void ParseParameters(string xmlParams);
    }
}