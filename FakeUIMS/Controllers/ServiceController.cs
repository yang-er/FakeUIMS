using FakeUIMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FakeUIMS.Controllers
{
    public class ServiceController : Controller
    {
        private ILogger Logger { get; }

        const string serviceResource = "ntms/service/res.do";
        const string courseScoreStat = "ntms/score/course-score-stat.do";
        const string evalTeach = "ntms/action/eval/eval-with-answer.do";
        const string selectLesson = "ntms/action/select/select-lesson.do";
        const string messageFetch = "ntms/siteMessages/get-message-in-box.do";
        const string messageRead = "ntms/siteMessages/read-message.do";
        const string messageDelete = "ntms/siteMessages/delete-recv-message.do";
        
        private static SortedDictionary<string, string> CachedResult { get; }
            = new SortedDictionary<string, string>();

        private static ISet<string> AvaliTag { get; } = new SortedSet<string>
        {
            "roomIdle@roomUsage",
            "search@teachingTerm",
            "school@schoolSearch",
            "lesson@globalStore",
            "teachClassMaster@selectResultAdjust",
            "programMaster@programsQuery",
            "programDetail@common",
            "tcmAdcAdvice@dep_recommandT",
        };

        private static ISet<string> AvaliOther { get; } = new SortedSet<string>
        {
            "teachClassStud@schedule",
            "stat-avg-gpoint",
            "archiveScore@queryCourseScore",
            "student@evalItem",
            "lessonSelectLogTcm@selectGlobalStore",
            "lessonSelectLog@selectStore",
            "query-splan-by-stud",
        };

        public ServiceController(ILogger<ServiceController> logger)
        {
            Logger = logger;
        }
        
        [NonAction]
        private async Task<IActionResult> Forwarder(string reqUrl, Func<string, string> outBadList)
        {
            var requestContent = await HttpContext.Request.ReadBodyAsync("application/json");
            if (requestContent is null) return BadRequest();
            var reqType = outBadList(requestContent);
            if (reqType is null) return BadRequest();
            Logger.LogWarning(reqType + " was requested by " + HttpContext.Session.GetString("startPersonAlu"));

            return await Program.ConnectUIMS(HttpContext, true, async (client, cc) =>
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, reqUrl);
                var httpContent = new StringContent(requestContent, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpRequest.Content = httpContent;
                var result = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead);

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return Redirect(result.Headers.Location.OriginalString.Replace(Program.ServerBaseUrl + "ntms/", ""));
                }
                else
                {
                    var body = await result.Content.ReadAsStringAsync();
                    return Content(body, "application/json");
                }
            });
        }

        [HttpPost]
        [Route(serviceResource)]
        public async Task<IActionResult> ServiceResource()
        {
            var requestContent = await HttpContext.Request.ReadBodyAsync("application/json");
            if (requestContent is null) return BadRequest();
            var inputContent = requestContent.ParseJson<InputBase>();
            if (inputContent.res is null) inputContent.res = "";
            if (inputContent.tag is null) inputContent.tag = "";
            var resourceTag = inputContent.res + inputContent.tag;
            var reqType = resourceTag == "" ? null : resourceTag;
            if (reqType is null) return BadRequest();

            var cacheRequest = AvaliTag.Contains(resourceTag);
            var allowedRequest = cacheRequest || AvaliOther.Contains(resourceTag);
            if (!allowedRequest) return BadRequest();
            if (cacheRequest && CachedResult.ContainsKey(requestContent))
                return Content(CachedResult[requestContent], "application/json");

            Logger.LogWarning(reqType + " was requested by " + HttpContext.Session.GetString("startPersonAlu"));

            return await Program.ConnectUIMS(HttpContext, true, async (client, cc) =>
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, serviceResource);
                var httpContent = new StringContent(requestContent, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpRequest.Content = httpContent;
                var result = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead);

                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return Redirect(result.Headers.Location.OriginalString.Replace(Program.ServerBaseUrl + "ntms/", ""));
                }
                else
                {
                    var body = await result.Content.ReadAsStringAsync();
                    if (cacheRequest) lock (CachedResult) CachedResult[requestContent] = body;
                    return Content(body, "application/json");
                }
            });
        }

        [HttpPost]
        [Route(courseScoreStat)]
        public Task<IActionResult> CourseScoreStat() => Forwarder(courseScoreStat, s => nameof(courseScoreStat));

        [HttpPost]
        [Route(evalTeach)]
        public Task<IActionResult> EvalTeach() => Forwarder(evalTeach, s => nameof(evalTeach));
        
        [HttpPost]
        [Route(selectLesson)]
        public Task<IActionResult> SelectLesson() => Forwarder(selectLesson, s => nameof(selectLesson));

        [HttpPost]
        [Route(messageFetch)]
        public Task<IActionResult> MessageFetch() => Forwarder(messageFetch, s => nameof(messageFetch));
        
        [HttpPost]
        [Route(messageRead)]
        public Task<IActionResult> MessageRead() => Forwarder(messageRead, s => nameof(messageRead));
        
        [HttpPost]
        [Route(messageDelete)]
        public Task<IActionResult> MessageDelete() => Forwarder(messageDelete, s => nameof(messageDelete));
    }
}
