using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace FakeUIMS.Models
{
    public static class Helper
    {
        public static T ParseJSON<T>(this string jsonString)
        {
            var json = new JsonSerializer();
            if (jsonString == "") throw new JsonReaderException();
            return json.Deserialize<T>(new JsonTextReader(new StringReader(jsonString)));
        }
        
        public static List<Cookie> GetAll(this CookieContainer cc)
        {
            const BindingFlags flag = BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance;
            var args = new object[] { };
            var lstCookies = new List<Cookie>();

            var table = (Hashtable)cc.GetType().InvokeMember("m_domainTable", flag, null, cc, args);

            foreach (var pathList in table.Values)
            {
                var lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list", flag, null, pathList, args);
                lstCookies.AddRange(from CookieCollection col in lstCookieCol.Values from Cookie c in col select c);
            }

            return lstCookies;
        }
    }
}
