using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace ProjectName.Services.Email
{

    public class EmailHandler : IEmailHandler
    {
        private readonly SmtpClient _smtpClient;

        public EmailHandler(IConfiguration configuration)
        {
            _smtpClient = new SmtpClient
            {
                Host = configuration["EmailHost"],
                EnableSsl = Convert.ToBoolean(configuration["EnableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = Convert.ToInt32(configuration["EmailPort"]),
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(configuration["EmailUser"], configuration["EmailPassword"])
            };
        }

        public void SendEmail(string fromEmail, string[] toEmail, string emailSubject, string emailContent, bool isHtmlFormat,
            string displayName, Dictionary<string, string> listAttachmentFilename = null, string[] ccEmail = null, string[] bccEmail = null)
        {
            var message = new MailMessage { From = new MailAddress(fromEmail, displayName) };
            if (toEmail.Length > 0)
            {
                foreach (var item in toEmail.Where(item => !string.IsNullOrWhiteSpace(item)))
                {
                    message.To.Add(new MailAddress(item));
                }
            }
            if (ccEmail != null && ccEmail.Length > 0)
            {
                foreach (var item in ccEmail.Where(item => !string.IsNullOrWhiteSpace(item)))
                {
                    message.CC.Add(new MailAddress(item));
                }
            }
            if (bccEmail != null && bccEmail.Length > 0)
            {
                foreach (var item in bccEmail.Where(item => !string.IsNullOrWhiteSpace(item)))
                {
                    message.Bcc.Add(new MailAddress(item));
                }
            }
            message.Subject = emailSubject;
            message.Body = emailContent;
            message.IsBodyHtml = isHtmlFormat;
            message.BodyEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;
            if (listAttachmentFilename != null && listAttachmentFilename.Count > 0)
            {
                foreach (var attachmentFilename in listAttachmentFilename)
                {
                    var attachment = new Attachment(attachmentFilename.Value, MediaTypeNames.Application.Octet);
                    var disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename.Value);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename.Value);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename.Value);
                    disposition.FileName = attachmentFilename.Key;
                    disposition.Size = new FileInfo(attachmentFilename.Value).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    message.Attachments.Add(attachment);
                }
            }
            _smtpClient.Send(message);
        }
    }
}
