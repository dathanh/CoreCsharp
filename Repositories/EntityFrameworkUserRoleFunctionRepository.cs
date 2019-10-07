using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class EntityFrameworkUserRoleFunctionRepository : EntityFrameworkTenantRepositoryBase<UserRoleFunction>, IUserRoleFunctionRepository
    {

        public EntityFrameworkUserRoleFunctionRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {

        }

        public List<UserRoleFunction> LoadUserSecurityRoleFunction(int userRoleId, int documentTypeId)
        {
            var result = (from urf in DataContext.UserRoleFunctions.AsQueryable().AsNoTracking()
                          join document in DataContext.DocumentTypes.AsQueryable().AsNoTracking()
                              on urf.DocumentTypeId equals document.Id into temp
                          from docType in temp
                          where docType.Id == documentTypeId
                          where urf.UserRoleId == userRoleId
                          select urf);
            return result.ToList();
        }
    }
}