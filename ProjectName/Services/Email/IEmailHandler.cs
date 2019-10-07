using System.Collections.Generic;

namespace ProjectName.Services.Email
{
    public interface IEmailHandler
    {
        void SendEmail(string fromEmail, string[] toEmail, string emailSubject, string emailContent, bool isHtmlFormat,
            string displayName, Dictionary<string, string> listAttachmentFilename = null, string[] ccEmail = null, string[] bccEmail = null);
    }
}
