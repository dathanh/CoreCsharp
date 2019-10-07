using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Repositories
{
    public class EntityFrameworkUserRepository : EntityFrameworkTenantRepositoryBase<User>, IUserRepository
    {
        public EntityFrameworkUserRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("UserName");
            SearchColumns.Add("Role");
            SearchColumns.Add("FullName");
            SearchColumns.Add("Phone");
            SearchColumns.Add("Email");
            DisplayColumnForCombobox.Add("UserName");
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            return (from s in GetAll()
                    select new UserGridVo
                    {
                        Id = s.Id,
                        UserName = s.UserName,
                        FullName = s.FullName,
                        Role = s.UserRole != null ? s.UserRole.Name : "",
                        Email = s.Email,
                        Phone = s.Phone,
                        IsActive = s.IsActive,
                    }).OrderBy(queryInfo.SortString);
        }

        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "Id", Dir = "desc" } };
            }

            queryInfo.Sort.ForEach(x =>
            {
                if (x.Field == "PhoneInFormat")
                {
                    x.Field = "Phone";
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(x.Field))
                    {
                        x.Field = $"{x.Field}";
                    }
                    else
                    {
                        x.Field = "UserName";
                    }

                }
            });
        }

        protected override string BuildLookupCondition(LookupQuery query)
        {
            var where = new StringBuilder();
            @where.Append("(");
            var innerWhere = new List<string>();
            var queryDisplayName = String.Format("FullName.Contains(\"{0}\")", query.Query);
            innerWhere.Add(queryDisplayName);
            @where.Append(String.Join(" OR ", innerWhere.ToArray()));
            @where.Append(")");

            if (query.HierachyItems != null)
            {
                foreach (var parentItem in query.HierachyItems.Where(parentItem => parentItem.Value != string.Empty && parentItem.Value != "-1"
                                                                                    && parentItem.Value != "0" && !parentItem.IgnoredFilter))
                {
                    var filterValue = parentItem.Value.Replace(",", string.Format(" OR {0} = ", parentItem.Name));
                    @where.Append(string.Format(" AND ( {0} = {1})", parentItem.Name, filterValue));
                }
            }

            return @where.ToString();
        }

        public User GetUserByUserNameAndPass(string username, string password, string appRole = "")
        {
            var query = GetAll();
            var user = query.SingleOrDefault(o => o.UserName == username && o.Password == password);

            if (user != null)
            {
                var role = user.UserRole;
                if (role.AppRoleName == AppRole.GlobalAdmin.ToString())
                {
                    user.AppRole = AppRole.GlobalAdmin;
                }
                else
                {
                    user.AppRole = AppRole.None;
                }
            }

            return user;
        }

        public bool HasPermission(int userId, int documentTypeId, int operationAction)
        {
            var user = GetById(userId);

            if (user != null)
            {
                return DataContext.UserRoleFunctions.Any(
                    o => o.DocumentTypeId == documentTypeId && o.SecurityOperationId == operationAction && o.UserRoleId == user.UserRoleId);
            }

            return false;
        }

        public bool UpdatePassword(int id, string newPassword)
        {
            var user = GetById(id);

            if (user != null)
            {
                var hashPassword = PasswordHelper.HashString(newPassword, user.UserName);
                user.Password = hashPassword;
                Update(user);
                Commit();
                return true;
            }

            return false;
        }
    }
}
