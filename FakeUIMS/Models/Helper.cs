using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FakeUIMS.Models
{
    public static class Helper
    {
        public static T ParseJson<T>(this string jsonString)
        {
            var json = new JsonSerializer();
            if (jsonString == "") throw new JsonReaderException();
            return json.Deserialize<T>(new JsonTextReader(new StringReader(jsonString)));
        }

        public static async Task<string> ReadBodyAsync(this HttpRequest req, string contentType)
        {
            if (req.ContentType != contentType) return null;
            if (req.ContentLength is null) return null;
            var bodyLength = (int)req.ContentLength.Value;
            if (bodyLength > 1024) return null;

            var bodyByte = new byte[bodyLength];
            for (var offset = 0;
                offset < bodyLength;
                offset += await req.Body.ReadAsync(bodyByte, offset, bodyLength - offset)) ;
            var bodyString = Encoding.UTF8.GetString(bodyByte);
            return bodyString;
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
