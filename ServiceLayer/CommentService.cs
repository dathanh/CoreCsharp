using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class CommentService : MasterFileService<Comment>, ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ITenantPersistenceService tenantPersistenceService, ICommentRepository commentRepository,
            IBusinessRuleSet<Comment> businessRuleSet = null)
            : base(commentRepository, commentRepository, tenantPersistenceService, businessRuleSet)
        {
            _commentRepository = commentRepository;
        }
        public CommentItem PostComment(CommentDto commentInfo)
        {
            var newComment = new Comment
            {
                Message = commentInfo.Message,
                VideoId = commentInfo.VideoId,
                ParentId = commentInfo.ParentId,
                CustomerId = commentInfo.CustomerId,
                IsActive = true,
            };
            _commentRepository.Add(newComment);
            _commentRepository.Commit();

            return _commentRepository.GetCommentByInfo(commentInfo);

        }
        public CommentListResponse GetCommentByVideoId(int id, int currentCustomerId, int sort, int page)
        {
            return _commentRepository.GetCommentByVideoId(id, currentCustomerId, sort, page);
        }
        public CommentListResponse GetCommentByParentId(int parentId, int currentCustomerId, int skip)
        {
            return _commentRepository.GetCommentByParentId(parentId, currentCustomerId, skip);
        }


    }
}