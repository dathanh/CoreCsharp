using Framework.DomainModel.ValueObject;
using Framework.Utility;
using Newtonsoft.Json;
using ServiceLayer.Interfaces;
using ServiceLayer.Interfaces.Common;
using System;
using System.Collections.ObjectModel;

namespace ProjectName.Models.Base
{
    public partial class ViewModelBase
    {
        public ViewModelBase()
        {
        }

        [JsonIgnore]
        public virtual string PageTitle
        {
            get;
            set;
        }

        [JsonProperty]
        public int Id { get; set; }
        public Guid RowGuid { get; set; }
        public bool IsDeleted { get; set; }
        [JsonIgnore]
        private UserDto _currentUser;
        [JsonIgnore]
        public UserDto CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    var authService = AppDependencyResolver.Current.GetService<IAuthenticationService>();
                    if (authService != null)
                    {
                        var taskGetCurrentUser = authService.GetCurrentUser();
                        _currentUser = taskGetCurrentUser.Result;
                    }
                }
                return _currentUser;
            }
            set => _currentUser = value;
        }
        public int CurrentUserId
        {
            get
            {
                if (CurrentUser != null)
                {
                    return CurrentUser.Id;
                }
                return 0;
            }
        }
        [JsonIgnore]
        public MenuViewModel MenuViewModel
        {
            get
            {
                var menuExtractDataService = AppDependencyResolver.Current.GetService<IMenuExtractData>();
                if (menuExtractDataService != null)
                {
                    return menuExtractDataService.GetMenuViewModel(CurrentUser.UserRoleId.GetValueOrDefault());
                }
                return new MenuViewModel();
            }
        }
        [JsonIgnore]
        public int DocumentTypeId { get; set; }
        [JsonIgnore]
        public Collection<FooterAction> FooterActions { get; set; }
        //public List<UserRoleFunction> SecurityActionPermissions { get; private set; }
        public object AddFooterAction(string text, string icon, FooterActionEnum function, string controllerScriptId, bool ignoreDirty = true, OperationAction securityMapAction = OperationAction.None)
        {
            var permission = true;
            if (FooterActions == null)
            {
                FooterActions = new Collection<FooterAction>();
            }
            //if (securityMapAction != OperationAction.None && SecurityActionPermissions != null)
            //{
            //    permission = SecurityActionPermissions.Any(s => s.SecurityOperationId == (int)securityMapAction);
            //}
            FooterActions.Add(new FooterAction
            {
                Action = function,
                Icon = icon,
                Text = text,
                //Text = text.ToUpperInvariant(),
                IgnoreDirty = ignoreDirty,
                Permission = permission,
                ControllerScripId = controllerScriptId
            });
            return null;
        }
    }

    public class FooterAction
    {
        public string Icon { get; set; }
        public string Text { get; set; }
        public FooterActionEnum Action { get; set; }
        public bool IgnoreDirty { get; set; }
        public bool Permission { get; set; }
        public string ActionMethod
        {
            get
            {
                switch (Action)
                {
                    case FooterActionEnum.Cancel:
                        return "Cancel('" + ControllerScripId + "');";
                    case FooterActionEnum.Save:
                        return "Save('" + ControllerScripId + "');";
                }
                return "";
            }
        }
        public string ActionMethodKey
        {
            get
            {
                switch (Action)
                {
                    case FooterActionEnum.Cancel:
                        return "FOOTER_ACTION_CANCEl";
                    case FooterActionEnum.Save:
                        return "FOOTER_ACTION_SAVE";
                }
                return "";
            }
        }
        public string ClassType
        {
            get
            {
                switch (Action)
                {
                    case FooterActionEnum.Cancel:
                        return "btn-default";
                    case FooterActionEnum.Save:
                        return "btn-primary";
                }
                return "";
            }
        }
        public string ControllerScripId { get; set; }
    }

    public enum FooterActionEnum
    {
        Save,
        Cancel
    }
}