using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;
using ServiceLayer.Interfaces.Common;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Common
{
    public class MenuExtractData : IMenuExtractData
    {
        private readonly IUserRoleFunctionRepository _userRoleFunctionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private IList<DocumentType> _listDocumentType = new List<DocumentType>();
        private IList<UserRoleFunction> _listUserRoleFunction = new List<UserRoleFunction>();
        private IList<UserRole> _listUserRoles = new List<UserRole>();

        public MenuExtractData(IUserRoleFunctionRepository userRoleFunctionRepository, IUserRoleRepository userRoleRepository)
        {
            _userRoleFunctionRepository = userRoleFunctionRepository;
            _userRoleRepository = userRoleRepository;
            RefreshListData();
        }

        public List<UserRoleFunction> LoadUserSecurityRoleFunction(long userRoleId, long documentTypeId)
        {
            if (_listDocumentType.Count == 0)
            {
                _listDocumentType = _userRoleRepository.GetAllDocumentType();
            }

            if (_listUserRoleFunction.Count == 0)
            {
                _listUserRoleFunction = _userRoleFunctionRepository.ListAll();
            }

            var result = (from urf in _listUserRoleFunction
                          join document in _listDocumentType
                              on urf.DocumentTypeId equals document.Id into temp
                          from docType in temp
                          where docType.Id == documentTypeId
                          && urf.UserRoleId == userRoleId
                          select urf);

            return result.ToList();
        }

        public void RefreshListData()
        {
            _listDocumentType = new List<DocumentType>();
            _listUserRoleFunction = new List<UserRoleFunction>();
            _listUserRoles = new List<UserRole>();
            if (_listDocumentType.Count == 0)
            {
                _listDocumentType = _userRoleRepository.GetAllDocumentType();
            }
            if (_listUserRoleFunction.Count == 0)
            {
                _listUserRoleFunction = _userRoleFunctionRepository.ListAll();
            }
            if (_listUserRoles.Count == 0)
            {
                _listUserRoles = _userRoleRepository.ListAll();
            }
        }

        public bool CheckUserRoleForDocumentType(int idRole, DocumentTypeKey documentType, OperationAction action)
        {
            if (_listDocumentType.Count == 0 || _listUserRoleFunction.Count == 0)
            {
                _listUserRoleFunction = _userRoleFunctionRepository.ListAll();
                _listDocumentType = _userRoleRepository.GetAllDocumentType();
            }

            return (from urf in _listUserRoleFunction
                    join document in _listDocumentType on urf.DocumentTypeId equals document.Id into temp
                    from docType in temp
                    where docType.Id == (int)documentType && urf.UserRoleId == idRole && urf.SecurityOperationId == (int)action
                    select urf).Any();
        }

        public MenuViewModel GetMenuViewModel(int idRole)
        {
            var objResult = new MenuViewModel();

            //Get data for host
            if (_listUserRoles != null)
            {
                objResult.CanViewUser = CheckUserRoleForDocumentType(idRole, DocumentTypeKey.User, OperationAction.ShowMenu);
                objResult.CanViewRole = CheckUserRoleForDocumentType(idRole, DocumentTypeKey.UserRole, OperationAction.ShowMenu);
                //objResult.CanViewConfig = CheckUserRoleForDocumentType(idRole, DocumentTypeKey.Config, OperationAction.ShowMenu);
                //========Import Menu Start========//
                //========Import Menu End========//
            }

            return objResult;
        }
    }
}
