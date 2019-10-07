using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class LikeCommentService : MasterFileService<LikeComment>, ILikeCommentService
    {
        private readonly ILikeCommentRepository _likeCommentRepository;
        private readonly ICommentRepository _commentRepository;

        public LikeCommentService(ITenantPersistenceService tenantPersistenceService, ILikeCommentRepository likeCommentRepository,
             ICommentRepository commentRepository,
            IBusinessRuleSet<LikeComment> businessRuleSet = null)
            : base(likeCommentRepository, likeCommentRepository, tenantPersistenceService, businessRuleSet)
        {
            _likeCommentRepository = likeCommentRepository;
            _commentRepository = commentRepository;
        }

        public CommentItem ProcessLikeComment(LikeComment likeInfo)
        {
            var likeExist = _likeCommentRepository.FirstOrDefault(o => o.CommentId == likeInfo.CommentId && o.CustomerId == likeInfo.CustomerId);
            if (likeExist == null)
            {
                _likeCommentRepository.Add(new LikeComment
                {
                    CommentId = likeInfo.CommentId,
                    CustomerId = likeInfo.CustomerId,
                    Type = likeInfo.Type,
                });
            }
            else
            {
                if (likeExist.Type == likeInfo.Type)
                {
                    _likeCommentRepository.Remove(likeExist);
                }
                else
                {
                    likeExist.Type = likeInfo.Type;
                }
            }
            _likeCommentRepository.Commit();

            return _commentRepository.GetCommentById(likeInfo.CommentId, likeInfo.CustomerId);
        }

    }
}

