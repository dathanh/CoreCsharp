using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using System.Collections.Generic;

namespace ServiceLayer
{
    public class UserRoleService : MasterFileService<UserRole>, IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(ITenantPersistenceService tenantPersistenceService, IUserRoleRepository userRoleRepository,
            IBusinessRuleSet<UserRole> businessRuleSet = null)
            : base(userRoleRepository, userRoleRepository, tenantPersistenceService, businessRuleSet)
        {
            _userRoleRepository = userRoleRepository;
        }

        public dynamic GetRoleFunction(int idRole)
        {
            return _userRoleRepository.GetRoleFunction(idRole);
        }


        public IEnumerable<int> GetAllDocumentTypeId()
        {
            return _userRoleRepository.GetAllDocumentTypeId();
        }

        public List<RoleDto> GetListRoles()
        {
            throw new System.NotImplementedException();
        }

        public List<UserRoleFunctionDto> GetListUserRoleFunction()
        {
            throw new System.NotImplementedException();
        }

        public List<DocumentTypeDto> GetListDocumentType()
        {
            throw new System.NotImplementedException();
        }
        public IList<UserRoleFunction> ProcessMappingFromUserRoleGrid(List<UserRoleFunctionGridVo> userRoleFunctionData)
        {
            var objResult = new List<UserRoleFunction>();
            var listUpdate = userRoleFunctionData;

            if (listUpdate != null && listUpdate.Count != 0)
            {
                foreach (var userRoleFunctionGridVo in listUpdate)
                {
                    var objView = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.View,
                        IsDeleted = !userRoleFunctionGridVo.IsView
                    };

                    objResult.Add(objView);

                    //Implement View insert
                    var objInsert = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.Add,
                        IsDeleted = !userRoleFunctionGridVo.IsInsert
                    };

                    objResult.Add(objInsert);

                    //Implement View update
                    var objUpdate = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.Update,
                        IsDeleted = !userRoleFunctionGridVo.IsUpdate
                    };

                    objResult.Add(objUpdate);

                    //Implement View delete
                    var objDelete = new UserRoleFunction
                    {
                        DocumentTypeId = userRoleFunctionGridVo.Id,
                        SecurityOperationId = (int)OperationAction.Delete,
                        IsDeleted = !userRoleFunctionGridVo.IsDelete
                    };

                    objResult.Add(objDelete);
                }
            }
            return objResult;
        }
    }
}
