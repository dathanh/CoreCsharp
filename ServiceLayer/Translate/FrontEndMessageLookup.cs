using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using Framework.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Repositories.Interfaces;
using System;
using System.Linq;

namespace ServiceLayer.Translate
{
    public interface IFrontEndMessageLookup
    {
        string GetMessage(string resourceKey, int languageId = 0);
    }

    public class FrontEndMessageLookup : IFrontEndMessageLookup
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
        public FrontEndMessageLookup(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }
        public string GetMessage(string resourceKey, int languageId = 0)
        {

            if (languageId == (int)LanguageKey.English)
            {
                switch (resourceKey)
                {
                    case "InvalidData":
                        return "Invalid data";
                    case "PlaylistNameRequired":
                        return "You need a name to create a new playlist.";
                    case "PlaylistNameIsExists":
                        return "Playlist name is exists.";
                    case "PlayListNotNull":
                        return "Playlist not exists.";
                    case "VideoNotExists":
                        return "Video not exists.";
                    case "BannerNotExists":
                        return "Banner not exists.";
                    case "BackgroundNotExists":
                        return "Background not exists.";
                    case "FillInFullName":
                        return "Please enter your name.";
                    case "FillInEmail":
                        return "Fill in email.";
                    case "FillInGender":
                        return "Please select your gender.";
                    case "BirthDateIsInValid":
                        return "Birthdate is invalid.";
                    case "FillInCurrentPassword":
                        return "Fill in current password.";
                    case "FillInNewPassword":
                        return "Fill in new password.";
                    case "NewPassAndRetypePassNotMatch":
                        return "New password and re-type password are not match.";
                    case "PasswordMustBeComplex":
                        return "Oops, the password you’ve entered is not satisfied our requirement. Password needs to be at least 8 characters. Please try again.";
                    case "CurrentPasswordInvalid":
                        return "Current password is invalid.";
                    case "FillInUsername":
                        return "Fill in username.";
                    case "FillInMessage":
                        return "Fill in message.";
                    case "UserNameAndPasswordRequired":
                        return "You need to enter your email and password to log in.";
                    case "PasswordAndConfNotMatch":
                        return "Password and confirm your password are not match.";
                    case "UserNameIsExists":
                        return "The email address you’ve entered is already in use. Please try login using that email address or use another email address.";
                    case "UserNameIsNotAnValidEmail":
                        return "Oops, the email you’ve entered is not valid. Please try again or use another email address.";
                    case "UsernameOrPasswordIncorrect":
                        return "Oops, you’ve entered the wrong email or password. Please try again.";
                    case "CheckTermWhenSignUp":
                        return "You need to accept our terms & conditions to create an account.";
                    case "ActiveCodeNotValid":
                        return "Active Code Not Valid.";
                    case "ExpiredTime":
                        return "Overdue activation of account.";
                    case "YourAccountNotActive":
                        return "Your Account Not Active.";
                    case "EmailNotExists":
                        return "Oops, the email you’ve entered is not existing in our database. Please try again.";
                    case "EmailUnsubscribe":
                        return "Your email was unsubscribe";
                    case "SubjectToSendEmailForCreateUser":
                        return "[Ompalo] Please verify your email address.";
                    case "SubjectToSendEmailForForgotPassword":
                        return "[Ompalo] Forgot your password?";
                }

            }
            else
            {
                switch (resourceKey)
                {
                    case "InvalidData":
                        return "Dữ liệu không hợp lệ";
                    case "PlaylistNameRequired":
                        return "Bạn cần phải đặt tên cho danh sách phát mới";
                    case "PlaylistNameIsExists":
                        return "Tên danh sách phát đã tồn tại.";
                    case "PlayListNotNull":
                        return "Playlist không tồn tại.";
                    case "VideoNotExists":
                        return "Video không tồn tại.";
                    case "UrlLinkNotExists":
                        return "Banner không tồn tại.";
                    case "BackgroundNotExists":
                        return "Background không tồn tại.";
                    case "FillInFullName":
                        return "Vui lòng nhập họ và tên.";
                    case "FillInEmail":
                        return "Điền vào email.";
                    case "FillInGender":
                        return "Vui lòng chọn giới tính.";
                    case "BirthDateIsInValid":
                        return "Ngày sinh không hợp lệ.";
                    case "FillInCurrentPassword":
                        return "Điền vào mật khẩu hiện tại.";
                    case "FillInNewPassword":
                        return "Điền vào mật khẩu hiện mới.";
                    case "NewPassAndRetypePassNotMatch":
                        return "Mật khẩu mới và nhập lại mật khẩu không trùng.";
                    case "PasswordMustBeComplex":
                        return "Mật khẩu bạn nhập vừa rồi không hợp lệ. Mật khẩu cần có ít nhất 8 ký tự. Vui lòng thử lại.";
                    case "CurrentPasswordInvalid":
                        return "Mật khẩu hiện tại không hợp lệ";
                    case "FillInUsername":
                        return "Điền vào tài khoản";
                    case "FillInMessage":
                        return "Điền vào nội dung.";
                    case "UserNameAndPasswordRequired":
                        return "Bạn cần điền địa chỉ email và mật khẩu để đăng nhập.";
                    case "PasswordAndConfNotMatch":
                        return "Mật khẩu và xác nhận mật khẩu không trùng nhau.";
                    case "UserNameIsExists":
                        return "Địa chỉ email bạn vừa nhập đã được sử dụng. Vui lòng đăng nhập với địa chỉ email này hoặc sử dụng một địa chỉ email khác.";
                    case "UserNameIsNotAnValidEmail":
                        return "Địa chỉ email bạn vừa nhập không hợp lệ. Vui lòng thử lại lần nữa hoặc dùng một địa chỉ email khác.";
                    case "UsernameOrPasswordIncorrect":
                        return "Email và mật khẩu không hợp lệ. Vui lòng thử lại lần nữa.";
                    case "CheckTermWhenSignUp":
                        return "Bạn cần chấp nhận điều khoản sử dụng để tạo một tài khoản.";
                    case "ActiveCodeNotValid":
                        return "Mã xác nhận không chính xác.";
                    case "ExpiredTime":
                        return "Bạn đã hết hạn kích hoạt tài khoản.";
                    case "YourAccountNotActive":
                        return "Tài khoản của bạn chưa được kích hoạt.";
                    case "EmailNotExists":
                        return "Địa chỉ email bạn vừa nhập không tồn tại trong dữ liệu của chúng tôi. Vui lòng thử lại lần nữa.";
                    case "EmailUnsubscribe":
                        return "Email của bạn đã hủy đăng ký trước đây";
                    case "SubjectToSendEmailForCreateUser":
                        return "[Ompalo] Hãy xác nhận địa chỉ email của bạn.";
                    case "SubjectToSendEmailForForgotPassword":
                        return "[Ompalo] Quên mật khẩu của bạn?";
                }
            }
            return "";
        }

    }
}
