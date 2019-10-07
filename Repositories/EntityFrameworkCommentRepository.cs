using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace Repositories
{
    public class EntityFrameworkCommentRepository : EntityFrameworkTenantRepositoryBase<Comment>, ICommentRepository
    {
        public EntityFrameworkCommentRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {

        }
        public CommentListResponse GetCommentByVideoId(int id, int currentCustomerId, int sort, int page)
        {
            var commentCondition = from c in DataContext.Comments.Where(c => (c.VideoId == id) && (c.ParentId == null)) select c;
            var skip = (page - 1) * ConstantValue.LimitComment;
            var lastComment = DataContext.Comments.LastOrDefault();
            var maxIdComment = lastComment?.Id ?? 0;

            var query = (from c in commentCondition
                         join cus in DataContext.Customers on c.CustomerId equals cus.Id
                         select new CommentItem
                         {
                             Id = c.Id,
                             Message = c.Message,
                             ParentId = c.ParentId,
                             CustomerId = c.CustomerId,
                             FullName = c.Customer.FullName == "" || c.Customer.FullName == null ? c.Customer.UserName : c.Customer.FullName,
                             CreatedValue = c.CreatedOn,
                             HasLike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Like && l.CustomerId == currentCustomerId) > 0,
                             HasDislike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Dislike && l.CustomerId == currentCustomerId) > 0,
                             CountLike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Like),
                             CountDislike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Dislike),
                             AvatarValue = cus.Avatar,
                             ChildCount = DataContext.Comments.Count(child => child.ParentId == c.Id) > 0 ? DataContext.Comments.Count(child => child.ParentId == c.Id) : 0
                         });
            query = sort == ConstantValue.SortCommentNewest ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id);

            var commentItems = query.Skip(skip).Take(ConstantValue.LimitComment).AsNoTracking().ToList();

            CommentListResponse result = new CommentListResponse
            {
                TotalRecord = commentCondition.Count(),
                CurrentPage = page,
                TotalPage = (int)Math.Ceiling((double)commentCondition.Count() / ConstantValue.LimitComment),
                CommentItems = commentItems
            };
            return result;
        }
        public CommentListResponse GetCommentByParentId(int parentId, int currentCustomerId, int skip)
        {
            var commentCondition = from c in DataContext.Comments.Where(c => (c.ParentId == parentId)) select c;
            var lastComment = DataContext.Comments.LastOrDefault();
            var maxIdComment = lastComment?.Id ?? 0;
            CommentListResponse result = new CommentListResponse
            {
                TotalRecord = commentCondition.Count(),
                TotalPage = (int)Math.Ceiling((double)commentCondition.Count() / ConstantValue.LimitChildComment),
                CommentItems = (from c in commentCondition
                                join cus in DataContext.Customers on c.CustomerId equals cus.Id
                                select new CommentItem
                                {
                                    Id = c.Id,
                                    Message = c.Message,
                                    ParentId = c.ParentId ,
                                    CustomerId = c.CustomerId,
                                    FullName = c.Customer.FullName == "" || c.Customer.FullName == null ? c.Customer.UserName : c.Customer.FullName,
                                    CreatedValue = c.CreatedOn,
                                    HasLike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Like && l.CustomerId == currentCustomerId) > 0,
                                    HasDislike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Dislike && l.CustomerId == currentCustomerId) > 0,
                                    CountLike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Like),
                                    CountDislike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Dislike),
                                    AvatarValue = cus.Avatar,
                                    ChildCount = 0
                                }).OrderByDescending(c => c.Id).Skip(skip).Take(ConstantValue.LimitChildComment).AsNoTracking().ToList()

            };
            return result;
        }

        public CommentItem GetCommentByInfo(CommentDto commentInfo)
        {
            var lastComment = DataContext.Comments.LastOrDefault();
            var maxIdComment = lastComment?.Id ?? 0;
            var commentRes = (from c in DataContext.Comments.Where(c => (c.VideoId == commentInfo.VideoId)
                              && (c.CustomerId == commentInfo.CustomerId)
                              && (c.Message == commentInfo.Message))
                              join cus in DataContext.Customers on c.CustomerId equals cus.Id
                              select new CommentItem
                              {
                                  Id = c.Id,
                                  Message = c.Message,
                                  ParentId = c.ParentId,
                                  CustomerId = c.CustomerId,
                                  FullName = c.Customer.FullName == "" || c.Customer.FullName == null ? c.Customer.UserName : c.Customer.FullName,
                                  CreatedValue = c.CreatedOn,
                                  HasLike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Like && l.CustomerId == commentInfo.CustomerId) > 0,
                                  HasDislike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Dislike && l.CustomerId == commentInfo.CustomerId) > 0,
                                  CountLike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Like),
                                  CountDislike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Dislike),
                                  AvatarValue = cus.Avatar,
                                  ChildCount = DataContext.Comments.Count(child => child.ParentId == c.Id) > 0 ? DataContext.Comments.Count(child => child.ParentId == c.Id) : 0
                              }).OrderByDescending(c => c.Id).AsNoTracking().FirstOrDefault();
            return commentRes;
        }

        public CommentItem GetCommentById(int id, int customerId)
        {
            var commentRes = (from c in DataContext.Comments.Where(c => (c.Id == id))
                              join cus in DataContext.Customers on c.CustomerId equals cus.Id
                              select new CommentItem
                              {
                                  HasLike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Like && l.CustomerId == customerId) > 0,
                                  HasDislike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Dislike && l.CustomerId == customerId) > 0,
                                  CountLike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Like),
                                  CountDislike = DataContext.LikeComments.Count(l => l.CommentId == c.Id && l.Type == ConstantValue.Dislike),
                                  ChildCount = DataContext.Comments.Count(child => child.ParentId == c.Id) > 0 ? DataContext.Comments.Count(child => child.ParentId == c.Id) : 0
                              }).OrderByDescending(c => c.Id).AsNoTracking().FirstOrDefault();
            return commentRes;
        }

    }
}