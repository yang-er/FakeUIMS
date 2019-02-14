using FakeUIMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;

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
        public IActionResult SecurityCheck(string j_username, string j_password, string mousePath)
        {
            if ($"UIMS{j_username}233333".ToMD5(Encoding.UTF8) != j_password)
            {
                return Redirect("error/dispatch.jsp?reason=loginError");
            }
            else
            {
                return Redirect("index.do");
            }
        }

        [HttpPost]
        [Route(getCurrentUserInfo)]
        public IActionResult GetCurrentUserInfo()
        {
            if (Cookies.TryGetValue("alu", out var alu))
            {
                return Json(new LoginValue(alu));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route(userLogin)]
        public IActionResult UserLogin(string reason)
        {
            if (reason == "loginError")
            {
                return Content(errorMessagePattern.Replace("(\\S+)", "这是一个测试网站。"), "text/html");
            }
            else
            {
                return Content("I really really really really really dislike you", "text/html");
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
