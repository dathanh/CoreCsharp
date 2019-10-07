using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class MasterfileGridDataVo
    {
        public IList<ReadOnlyGridVo> Data { get; set; }

        public int TotalRowCount { get; set; }
    }
}
