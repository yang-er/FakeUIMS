using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
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
            if (!req.ContentType.StartsWith(contentType)) return null;
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

        public static string GetRandomChinese(int strlength)
        {
            var randomer = new RandomChinese();
            return randomer.GetRandomChinese(strlength);
        }

        public static DateTime GetRandomTime()
        {
            var random = Program.Random;
            int hour = random.Next(2, 5);
            int minute = random.Next(0, 60);
            int second = random.Next(0, 60);
            string tempStr = string.Format("{0} {1}:{2}:{3}", DateTime.Now.ToString("yyyy-MM-dd"), hour, minute, second);
            return Convert.ToDateTime(tempStr);
        }
        
        public static byte[] ToMD5(this byte[] source)
        {
            using (var MD5p = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                return MD5p.ComputeHash(source);
            }
        }

        public static string ToMD5(this string source, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            return encoding.GetBytes(source).ToMD5().ToHexDigest(true);
        }

        public static string ToHexDigest(this byte[] source, bool lower = false)
        {
            var sb = new StringBuilder();
            var chars = (lower ? "0123456789abcdef" : "0123456789ABCDEF").ToCharArray();

            foreach (var ch in source)
            {
                var bit = (ch & 0x0f0) >> 4;
                sb.Append(chars[bit]);
                bit = ch & 0x0f;
                sb.Append(chars[bit]);
            }

            return sb.ToString();
        }

        private class RandomChinese
        {
            public RandomChinese()
            {
            }

            public string GetRandomChinese(int strlength)
            {
                Encoding gb = Encoding.GetEncoding("GB2312");

                object[] bytes = this.CreateRegionCode(strlength);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < strlength; i++)
                {
                    string temp = gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[])));
                    sb.Append(temp);
                }

                return sb.ToString();
            }
            
            private object[] CreateRegionCode(int strlength)
            {
                string[] rBase = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

                Random rnd = new Random();
                object[] bytes = new object[strlength];
                
                for (int i = 0; i < strlength; i++)
                {
                    int r1 = rnd.Next(11, 14);
                    string str_r1 = rBase[r1].Trim();
                    
                    rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);
                    int r2;
                    if (r1 == 13)
                    {
                        r2 = rnd.Next(0, 7);
                    }
                    else
                    {
                        r2 = rnd.Next(0, 16);
                    }
                    string str_r2 = rBase[r2].Trim();
                    
                    rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                    int r3 = rnd.Next(10, 16);
                    string str_r3 = rBase[r3].Trim();
                    
                    rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                    int r4;

                    if (r3 == 10)
                        r4 = rnd.Next(1, 16);
                    else if (r3 == 15)
                        r4 = rnd.Next(0, 15);
                    else
                        r4 = rnd.Next(0, 16);

                    string str_r4 = rBase[r4].Trim();
                    byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                    byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                    byte[] str_r = new byte[] { byte1, byte2 };
                    bytes.SetValue(str_r, i);
                }

                return bytes;
            }
        }
    }
}
