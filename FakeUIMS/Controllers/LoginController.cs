using FakeUIMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FakeUIMS.Controllers
{
    public class LoginController : Controller
    {
        private ILogger Logger { get; }
        private IRequestCookieCollection Cookies => HttpContext.Request.Cookies;
        
        const string securityCheckUrl = "ntms/j_spring_security_check";
        const string getCurrentUserInfo = "ntms/action/getCurrentUserInfo.do";
        const string userLogin = "ntms/userLogin.jsp";
        const string indexDo = "ntms/index.do";
        const string ntmsDir = "ntms/";
        const string errorDispatch = "ntms/error/dispatch.jsp";
        const string errorMessagePattern = @"<span class=""error_message"" id=""error_message"">登录错误：(\S+)</span>";

        public LoginController(ILogger<LoginController> logger)
        {
            Logger = logger;
        }

        [HttpGet]
        [Route(indexDo)]
        public IActionResult Index() => Redirect("http://uims.jlu.edu.cn/");

        [HttpGet]
        [Route(ntmsDir)]
        public IActionResult Home()
        {
            if (Cookies.TryGetValue("alu", out var alu))
            {
                HttpContext.Session.SetString("startPersonAlu", alu);
                Logger.LogWarning(alu + " started a session : " + HttpContext.Session.Id);
                return new EmptyResult();
            }
            else
            {
                return Redirect("http://uims.jlu.edu.cn/");
            }
        }

        [HttpPost]
        [Route(securityCheckUrl)]
        public Task<IActionResult> SecurityCheck(string j_username, string j_password, string mousePath)
        {
            return Program.ConnectUIMS(HttpContext, false, async (client, cc) =>
            {
                var formUrlEncodedContent = new Dictionary<string, string>
                {
                    { nameof(j_username), j_username },
                    { nameof(j_password), j_password },
                    { nameof(mousePath), mousePath }
                };

                await client.GetAsync("ntms/", HttpCompletionOption.ResponseHeadersRead);
                var JSESSIONID = cc.GetAll().Find(c => c.Name == "JSESSIONID");

                HttpContext.Response.Cookies.Append(JSESSIONID.Name, JSESSIONID.Value, new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(15),
                    Path = "/ntms/",
                });

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, securityCheckUrl);
                httpRequest.Content = new FormUrlEncodedContent(formUrlEncodedContent);
                httpRequest.Headers.Referrer = new Uri(Program.ServerBaseUrl + "ntms/userLogin.jsp?reason=nologin");
                var result = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);

                if (result.Headers.Location is null)
                {
                    Logger.LogCritical("Error Request! " + result.ToString());
                    return new BadRequestResult();
                }

                return Redirect(result.Headers.Location.OriginalString.Replace(Program.ServerBaseUrl + "ntms/", ""));
            });
        }

        [HttpPost]
        [Route(getCurrentUserInfo)]
        public Task<IActionResult> GetCurrentUserInfo()
        {
            return Program.ConnectUIMS(HttpContext, true, async (client, cc) =>
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, getCurrentUserInfo);
                var httpContent = new StringContent("{}", Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead);
                var body = await result.Content.ReadAsStringAsync();
                return Content(body, "application/json");
            });
        }

        [HttpGet]
        [Route(userLogin)]
        public Task<IActionResult> UserLogin(string reason)
        {
            if (reason == "loginError")
            {
                return Program.ConnectUIMS(HttpContext, true, async (client, cc) =>
                {
                    var httpRequest = new HttpRequestMessage(HttpMethod.Get, userLogin + "?reason=loginError");
                    var result = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead);
                    var body = await result.Content.ReadAsStringAsync();
                    return Content(Regex.Match(body, errorMessagePattern).Groups[0].Value);
                });
            }
            else
            {
                return Task.FromResult<IActionResult>(Content("I really really really really really dislike you"));
            }
        }

        [HttpGet]
        [Route(errorDispatch)]
        public IActionResult ErrorDispatch(string reason)
        {
            Logger.LogWarning("Be careful. Somebody access this page. reason=" + reason);
            return new BadRequestResult();
        }
    }
}
