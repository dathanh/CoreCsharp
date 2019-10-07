namespace Framework.Service.Translation
{
    public class SystemMessageLookup
    {
        public static string GetMessage(string resourceKey)
        {
            switch (resourceKey)
            {
                case "ConcurrencyExceptionMessageText":
                    return "The data has been modified since you opened this record therefore you are no longer able to save the record.";
                case "UniqueConstraintErrorText":
                    return "{0} cannot contain value {1} as it is already in use";
                case "RecordInUseConstraintErrorText":
                    return "Unable to delete this record as it is referenced by {0}";
                case "UnAuthorizedAccessText":
                    return "Attempted to perform an unauthorized operation on this function.";
                case "GeneralExceptionMessageText":
                    return "We have encountered a problem with the system. Please contact ProjectName Support <support@startberry.com>.";
                case "RequiredTextResourceKey":
                    return "Field {0} is required.";
                case "FileUploadIsInValid":
                    return "File upload is not a valid format - please check the format and try again.";
                case "ExistsTextResourceKey":
                    return "Duplicate name: '{0}' already exists in the system - please enter a unique name.";
                case "InvalidField":
                    return "Data field {0} is invalid - please remove or update and try again.";
                case "InvalidData":
                    return "Data is invalid - please try again or contact ProjectName Support <support@startberry.com>.";
                case "BussinessGenericErrorMessageKey":
                    return "The following process is not valid:";
                case "CannotUploadFile":
                    return "Cannot upload file.";
                case "CannotConnectToAzureToUploadFile":
                    return "We are experiencing issues with our server connection. Please contact Interpris Support <support@interpris.com>.";
                case "ViewWithDataSourceAndTemplateAlreadyUse":
                    return "Duplicate view: you already have a view with this theme template and data source ({0})";
                case "NotHavePermissionToUpdateDashboard":
                    return "You don't have permission to update this dashboard.";
                case "UsernameOrPasswordIncorrect":
                    return "Username or Password is incorrect. Please try again.";
                case "VersionIncorrect":
                    return "Code version is incorrect.";
                case "DirtyDialogMessageText":
                    return "Để tránh việc mất dữ liệu, vui lòng lưu dữ liệu trước khi đóng.";
                case "NoText":
                    return "No";
                case "StayIn":
                    return "Remember me?";
                case "YesText":
                    return "Yes";
                case "CreateText":
                    return "Create";
                case "Row":
                    return "rows";
                case "All":
                    return "All";
                case "Export":
                    return "Export";
                case "CancelText":
                    return "Cancel";
                case "UpdateText":
                    return "Update";
                case "MaxLengthRequied":
                    return "The field {0} has max length is {1} characters.";
                case "InvalidAccessToken":
                    return "The access token is invalid.";
                case "ExpectationFailed":
                    return "Expectation failed";
                case "MustGreaterThan":
                    return "{0} must greater than {1}";
                case "ItemIsNotFound":
                    return "Item is not found!";
                case "CanDeleteAppRole":
                    return "You cannot delete the application role.";
                case "WeCannotFoundThisPage":
                    return "We cannot found this page";
                case "YourWebpageIsNotExists":
                    return "Your webpage is not exists";
                case "PersonalInfo":
                    return "Your personal information";
                case "MyPersonalInfo":
                    return "My personal information";
                case "Required":
                    return "Required";
                case "FullName":
                    return "Full name";
                case "Phone":
                    return "Phone";
                case "Avatar250":
                    return "Avatar (250*250)";
                case "BrowseFile":
                    return "Browse file";
                case "EnterKeyword":
                    return "Keywords...";
                case "DeleteText":
                    return "Delete";
                case "CurrentPassword":
                    return "Current password";
                case "NewPassword":
                    return "New password";
                case "ConfirmPassword":
                    return "Confirm password";
                case "PasswordRule":
                    return "Password must have at least 8 characters, include number, upper case, normal case and special characters";
                case "UserManagePage":
                    return "Manage users";
                case "CreateUserPage":
                    return "Create user";
                case "UpdateUserPage":
                    return "Update user";
                case "User":
                    return "User";
                case "UserInfo":
                    return "User information";
                case "UserRole":
                    return "User role";
                case "UserName":
                    return "User name";
                case "GeneralInfo":
                    return "General information";
                case "IsActive":
                    return "Active";
                case "UserRoleManagePage":
                    return "Manage user role";
                case "CreateUserRolePage":
                    return "Create user role";
                case "UpdateUserRolePage":
                    return "Update user role";
                case "ViewDetail":
                    return "View detail";
                case "Name":
                    return "Name";
                case "SelectAll":
                    return "Select all";
                case "DetailRole":
                    return "Role's information detail";
                case "InvalidUserAndPasswordText":
                    return "User name and password are invalid.";
                case "CannotDeleteYourself":
                    return "You cannot delete yourself.";
                case "LoginWithInacticeUser":
                    return "Your account is lock. Please contact with administrator to get more detail.";
                case "PasswordNotMatch":
                    return "Password is not match.";
                case "NewPasswordNotMatch":
                    return "New password and confirm password is not match.";
                case "CannotSelectParentWithSameCategory":
                    return "Cannot select parent same category.";
                case "DisplayNameEmail":
                    return "Ompalo";
                case "RequiredLengthOfName":
                    return "{0} limit is 65 chars";
            }
            return resourceKey;
        }
    }
}
