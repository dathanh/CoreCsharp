using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface ICustomerService : IMasterFileService<Customer>
    {
        Customer RegisterByFacebook(SigninWithFbDto fbInfo);
        Customer RegisterByGoogle(SigninWithFbDto ggInfo);
        Customer GetCustomerByUserNameAndPass(string userName, string password);
        bool Register(SignupDto signUp);
        bool UpdateProfile(CustomerEditProfile data);
        bool ChangePassword(CustomerChangePasswordDto data);
        bool SendFeedback(Feedback data);
        bool SetActiveNewAccount(ActiveCustomer activeCustomerInfo);
        string ForgotPassword(EmailForgotPassword emailInfo);
        bool SetNewPassByForgotPassCode(SetNewPassWordModel setNewPassInfo);
        bool SetUnsubscribe(string unsubscribeCode, int languageId);
    }
}