using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Framework.Utility
{
    public class XmlDataHelper : IXmlDataHelpper
    {
        private readonly IHostingEnvironment _env;
        public XmlDataHelper(IHostingEnvironment env)
        {
            _env = env;
            PathFile = Path.Combine(env.WebRootPath, "ConfigData/ConfigData",
            "SystemData.xml");
            CalculateData();
        }
        private readonly string PathFile;

        private void CalculateData()
        {
            _listAllData = new Dictionary<string, Dictionary<string, string>>();
            var xmlDocument = XDocument.Load(PathFile);
            var root = xmlDocument.Root;
            if (root == null)
            {
                return;
            }

            foreach (var child in root.Elements())
            {
                var objAdd = new Dictionary<string, string>();
                foreach (var item in child.Elements())
                {
                    if (item.Attribute("value") != null)
                    {
                        objAdd.Add(item.Attribute("value").Value, item.Value);
                    }
                }

                _listAllData.Add(child.Name.ToString(), objAdd);
            }
        }

        private Dictionary<string, Dictionary<string, string>> _listAllData;

        public Dictionary<string, string> GetData(string type)
        {
            if (_listAllData == null || _listAllData.Count == 0 || string.IsNullOrWhiteSpace(type))
            {
                return new Dictionary<string, string>();
            }
            return !_listAllData.ContainsKey(type) ? new Dictionary<string, string>() : _listAllData[type];
        }

        public string GetValue(string type, string key)
        {
            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(key) || !_listAllData.ContainsKey(type))
            {
                return "";
            }

            var objListItem = _listAllData[type];
            if (objListItem != null && objListItem.ContainsKey(key))
            {
                return objListItem[key];
            }

            return "";
        }
    }
}
