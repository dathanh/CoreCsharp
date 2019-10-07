using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;

namespace Framework.Utility
{
    public class TemplateHelper
    {
        public static string FormatTemplateWithContentTemplate(string contentTempalte, object data)
        {
            var template = Template.Parse(contentTempalte);
            var dict = data as IDictionary<string, object>;
            if (dict != null)
            {
                //phan nay xu ly du lieu tu PCST report
                return template.Render(Hash.FromDictionary(dict));
            }

            return template.Render(Hash.FromAnonymousObject(data));
        }
        public static string ReadFile(string fileName)
        {
            try
            {
                using (StreamReader reader = File.OpenText(fileName))
                {
                    var fileContent = reader.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(fileContent))
                    {
                        return fileContent;
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                //Log
            }
            return null;
        }
        public static string CreateUserEmailTemplateEN
        {
            get
            {
                return "wwwroot/ConfigData/ConfigTemplate/CreateUserEmailTemplateEN.html";
            }
        }
        public static string CreateUserEmailTemplateVN
        {
            get
            {
                return "wwwroot/ConfigData/ConfigTemplate/CreateUserEmailTemplateVN.html";
            }
        }

        public static string ForgotPasswordEmailTemplateEN
        {
            get
            {
                return "wwwroot/ConfigData/ConfigTemplate/ForgotPasswordEmailTemplateEN.html";
            }
        }

        public static string ForgotPasswordEmailTemplateVN
        {
            get
            {
                return "wwwroot/ConfigData/ConfigTemplate/ForgotPasswordEmailTemplateVN.html";
            }
        }
    }
}
