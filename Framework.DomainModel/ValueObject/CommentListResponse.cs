using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class CommentListResponse
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }
        public bool IsNext
        {
            get
            {
                if (CurrentPage < TotalPage)
                {
                    return true;
                }
                return false;
            }
        }
        public List<CommentItem> CommentItems;

    }
}
