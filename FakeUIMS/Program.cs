using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FakeUIMS
{
    public class Program
    {
        public static Random Random { get; } = new Random();

        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5000")
                .UseStartup<Startup>();

        internal const string ServerBaseUrl = "https://10.60.65.8/";
        
        public static HttpClientHandler ValidateRequestContext(HttpContext HttpContext, bool sessionRequested = true)
        {
            string alu, loginPage, pwdStrength, JSESSIONID;
            bool answer = HttpContext.Request.Cookies.TryGetValue(nameof(alu), out alu);
            answer = HttpContext.Request.Cookies.TryGetValue(nameof(loginPage), out loginPage) && answer;
            answer = HttpContext.Request.Cookies.TryGetValue(nameof(pwdStrength), out pwdStrength) && answer;
            answer = answer && alu == HttpContext.Session.GetString("startPersonAlu");
            if (!answer) throw new WebException("Session timed out.", WebExceptionStatus.Timeout);

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                UseCookies = true,
                CookieContainer = new CookieContainer(),
                ServerCertificateCustomValidationCallback = delegate { return true; },
            };

            handler.CookieContainer.Add(new Cookie(nameof(loginPage), loginPage, "/ntms/", "10.60.65.8"));
            handler.CookieContainer.Add(new Cookie(nameof(alu), alu, "/ntms/", "10.60.65.8"));
            handler.CookieContainer.Add(new Cookie(nameof(pwdStrength), pwdStrength, "/ntms/", "10.60.65.8"));

            if (HttpContext.Request.Cookies.TryGetValue(nameof(JSESSIONID), out JSESSIONID))
                handler.CookieContainer.Add(new Cookie(nameof(JSESSIONID), JSESSIONID, "/ntms/", "10.60.65.8"));
            else if (sessionRequested)
                throw new WebException("Session timed out.", WebExceptionStatus.Timeout);
            
            return handler;
        }
        
        public static async Task<IActionResult> ConnectUIMS(
            HttpContext HttpContext,
            bool sessionRequested,
            Func<HttpClient, CookieContainer, Task<IActionResult>> Process)
        {
            try
            {
                using (var handler = ValidateRequestContext(HttpContext, sessionRequested))
                {
                    using (var client = new HttpClient(handler))
                    {
                        client.BaseAddress = new Uri(ServerBaseUrl);
                        return await Process(client, handler.CookieContainer);
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status != WebExceptionStatus.Timeout) throw;
                return new RedirectResult("error/dispatch.jsp?reason=nologin", false);
            }
            catch (ArgumentException)
            {
                return new BadRequestResult();
            }
        }
    }
}
