using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Repositories
{
    public class EntityFrameworkUserRoleRepository : EntityFrameworkTenantRepositoryBase<UserRole>, IUserRoleRepository
    {
        public EntityFrameworkUserRoleRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            DisplayColumnForCombobox.Add("Name");
        }

        public override IQueryable<ReadOnlyGridVo> BuildQueryToGetDataForGrid(IQueryInfo queryInfo)
        {
            var queryResult = (from entity in GetAll().AsNoTracking()
                               select new { entity }).Select(s => new UserRoleGridVo
                               {
                                   Id = s.entity.Id,
                                   Name = s.entity.Name,
                                   AppRoleName = s.entity.AppRoleName
                               }).OrderBy(queryInfo.SortString);
            return queryResult;
        }

        protected override void BuildSortExpression(IQueryInfo queryInfo)
        {
            if (queryInfo.Sort == null || queryInfo.Sort.Count == 0)
            {
                queryInfo.Sort = new List<Sort> { new Sort { Field = "Id", Dir = "desc" } };
            }
            queryInfo.Sort.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.Field))
                {
                    x.Field = $"{x.Field}";
                }
                else
                {
                    x.Field = "Name";
                }
            });
        }
        public dynamic GetRoleFunction(int idRole)
        {
            var objListResult = new List<UserRoleFunctionGridVo>();
            var objListDocumentType =
                PersistenceService.CurrentWorkspace.Context.DocumentTypes.AsNoTracking().OrderBy(o => o.Order).ThenByDescending(o => o.Id).ToList();

            if (idRole > 0)
            {
                var objListUserRoleFunction = PersistenceService.CurrentWorkspace.Context.UserRoleFunctions.AsNoTracking().Where(o => o.UserRoleId == idRole).ToList();
                foreach (var documentType in objListDocumentType)
                {
                    var objAdd = new UserRoleFunctionGridVo { Id = documentType.Id, Name = documentType.Title };
                    // Check for isView
                    var isView = objListUserRoleFunction.Any(o => o.SecurityOperationId == (int)OperationAction.View && o.DocumentTypeId == documentType.Id);
                    objAdd.IsView = isView;
                    // Check for isDelete
                    var isDelete = objListUserRoleFunction.Any(o => o.SecurityOperationId == (int)OperationAction.Delete && o.DocumentTypeId == documentType.Id);
                    objAdd.IsDelete = isDelete;
                    // Check for isUpdate
                    var isUpdate = objListUserRoleFunction.Any(o => o.SecurityOperationId == (int)OperationAction.Update && o.DocumentTypeId == documentType.Id);
                    objAdd.IsUpdate = isUpdate;
                    // Check for isAdd
                    var isAdd = objListUserRoleFunction.Any(o => o.SecurityOperationId == (int)OperationAction.Add && o.DocumentTypeId == documentType.Id);
                    objAdd.IsInsert = isAdd;
                    objListResult.Add(objAdd);
                    var isShowMenu =
                   objListUserRoleFunction.Any(
                       o => o.SecurityOperationId == (int)OperationAction.ShowMenu && o.DocumentTypeId == documentType.Id);
                    objAdd.IsShowMenu = isShowMenu;
                }
            }
            else
            {
                foreach (var documentType in objListDocumentType)
                {
                    var objAdd = new UserRoleFunctionGridVo
                    {
                        Id = documentType.Id,
                        Name = documentType.Title,
                        IsView = false,
                        IsDelete = false,
                        IsUpdate = false,
                        IsInsert = false,
                        IsShowMenu = false,
                    };
                    objListResult.Add(objAdd);
                }
            }

            return new { Data = objListResult, TotalRowCount = objListResult.Count };
        }

        public IEnumerable<int> GetAllDocumentTypeId()
        {
            return PersistenceService.CurrentWorkspace.Context.DocumentTypes.Select(o => o.Id).ToList();
        }

        public List<DocumentType> GetAllDocumentType()
        {
            return PersistenceService.CurrentWorkspace.Context.DocumentTypes.ToList();
        }

        public List<RoleDto> GetListRoles()
        {
            var data = GetAll()
                .Select(o => new RoleDto
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();
            return data;
        }

        public List<UserRoleFunctionDto> GetListUserRoleFunction()
        {
            return PersistenceService.CurrentWorkspace.Context.UserRoleFunctions.AsNoTracking()
                .Select(o => new UserRoleFunctionDto
                {
                    Id = o.Id,
                    DocumentTypeId = o.DocumentTypeId,
                    SecurityOperationId = o.SecurityOperationId,
                    UserRoleId = o.UserRoleId
                }).ToList();
        }

        public List<DocumentTypeDto> GetListDocumentType()
        {
            return PersistenceService.CurrentWorkspace.Context.DocumentTypes.AsNoTracking()
                .Select(o => new DocumentTypeDto
                {
                    Id = o.Id,
                    Name = o.Name
                }).ToList();
        }
    }
}