using System.Collections.Generic;

namespace Framework.Utility
{
    public interface IXmlDataHelpper
    {
        Dictionary<string, string> GetData(string type);
        string GetValue(string type, string key);
    }
}
