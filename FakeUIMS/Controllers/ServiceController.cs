using FakeUIMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public ServiceController(ILogger<ServiceController> logger)
        {
            Logger = logger;

            if (TeachingTerms is null)
            {
                TeachingTerms = TeachingTerm.GetRootObject().ParseJson<RootObject<TeachingTerm>>();
            }
        }
        
        const string serviceResource = "ntms/service/res.do";
        const string courseScoreStat = "ntms/score/course-score-stat.do";
        const string evalTeach = "ntms/action/eval/eval-with-answer.do";
        const string selectLesson = "ntms/action/select/select-lesson.do";
        const string messageFetch = "ntms/siteMessages/get-message-in-box.do";
        const string messageRead = "ntms/siteMessages/read-message.do";
        const string messageDelete = "ntms/siteMessages/delete-recv-message.do";
        
        private static SortedDictionary<string, string> CachedResult { get; }
            = new SortedDictionary<string, string>();

        private static RootObject<TeachingTerm> TeachingTerms { get; set; }

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

        [NonAction]
        public IActionResult SearchTeachingTerm(int t)
        {
            if (t == -1) return Json(TeachingTerms);

            var ro = new RootObject<TeachingTerm>
            {
                id = "termId",
                status = 0,
                resName = "teachingTerm",
                msg = "",
                value = new TeachingTerm[1],
            };

            ro.value[0] = System.Array.Find(TeachingTerms.value, s => s.termId == t.ToString());
            return Json(ro);
        }

        [NonAction]
        public IActionResult QueryCourseScore()
        {
            int terms = Program.Random.Next(0, 25);

            var ro = new RootObject<ArchiveScoreValue>
            {
                id = "asId",
                status = 0,
                value = new ArchiveScoreValue[terms],
                resName = "archiveScore",
                msg = ""
            };

            for (int i = 0; i < terms; i++)
            {
                var cn = Helper.GetRandomChinese(Program.Random.Next(3, 8));
                var tm = TeachingTerms.value[Program.Random.Next(0, 3)].termName;
                var score = (int)System.Math.Sqrt(Program.Random.Next(0, 10000));

                ro.value[i] = new ArchiveScoreValue
                {
                    xkkh = "(2017-2018-2)-ty13020001",
                    teachingTerm = new ArchiveScoreValue.TeachingTerm { termName = tm },
                    score = score.ToString(),
                    dateScore = Helper.GetRandomTime().AddDays(-2),
                    isPass = score >= 60 ? "Y" : "N",
                    course = new ArchiveScoreValue.Course { courName = cn },
                    isReselect = Program.Random.Next(0, 10) > 8 ? "Y" : "N",
                    credit = (Program.Random.Next(1, 8) / 2.0).ToString(),
                    gpoint = (Program.Random.Next(5, 40) / 10.0).ToString(),
                };
            }

            return Json(ro);
        }

        [NonAction]
        public IActionResult Schedule()
        {
            return Content("{\"id\":\"tcsId\",\"status\":0,\"value\":[{\"teachClassMaster\":{\"maxStudCnt\":31,\"lessonSchedules\":[{\"classroom\":{\"roomId\":294,\"fullName\":\"前卫-经信教学楼#D区104\"},\"timeBlock\":{\"classSet\":96,\"name\":\"周一第5,6节{第10-17周}\",\"endWeek\":17,\"beginWeek\":10,\"tmbId\":5838,\"dayOfWeek\":1},\"lsschId\":138975},{\"classroom\":{\"roomId\":294,\"fullName\":\"前卫-经信教学楼#D区104\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周三第3,4节{第1-8周}\",\"endWeek\":8,\"beginWeek\":1,\"tmbId\":1181,\"dayOfWeek\":3},\"lsschId\":138972},{\"classroom\":{\"roomId\":294,\"fullName\":\"前卫-经信教学楼#D区104\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周三第3,4节{第10-17周}\",\"endWeek\":17,\"beginWeek\":10,\"tmbId\":6077,\"dayOfWeek\":3},\"lsschId\":138973},{\"classroom\":{\"roomId\":294,\"fullName\":\"前卫-经信教学楼#D区104\"},\"timeBlock\":{\"classSet\":96,\"name\":\"周一第5,6节{第1-8周}\",\"endWeek\":8,\"beginWeek\":1,\"tmbId\":378,\"dayOfWeek\":1},\"lsschId\":138974}],\"studCnt\":31,\"lessonTeachers\":[{\"teacher\":{\"name\":\"金福寿\",\"teacherId\":16800}}],\"name\":\"(2018-2019-1)-ac13541107-500180-1\",\"tcmId\":94594,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"计算机原理与系统结构\"}},\"lssgId\":53058,\"fullName\":\"计算机原理与系统结构\"}},\"tcsId\":6994944,\"dateAccept\":\"2018-06-11T20:24:20\"},{\"teachClassMaster\":{\"maxStudCnt\":31,\"lessonSchedules\":[{\"classroom\":{\"roomId\":766,\"fullName\":\"前卫-经信教学楼#E区303\"},\"timeBlock\":{\"classSet\":6,\"name\":\"周五第1,2节{第1-8周}\",\"endWeek\":8,\"beginWeek\":1,\"tmbId\":39,\"dayOfWeek\":5},\"lsschId\":133911},{\"classroom\":{\"roomId\":766,\"fullName\":\"前卫-经信教学楼#E区303\"},\"timeBlock\":{\"classSet\":6,\"name\":\"周二第1,2节{第1-8周}\",\"endWeek\":8,\"beginWeek\":1,\"tmbId\":765,\"dayOfWeek\":2},\"lsschId\":133908},{\"classroom\":{\"roomId\":1567,\"fullName\":\"前卫-计算机新楼#B110\"},\"timeBlock\":{\"classSet\":480,\"name\":\"周三第5,6,7,8节{第10-13周}\",\"endWeek\":13,\"beginWeek\":10,\"tmbId\":10157,\"dayOfWeek\":3},\"lsschId\":134523}],\"studCnt\":31,\"lessonTeachers\":[{\"teacher\":{\"name\":\"王康平\",\"teacherId\":13680}}],\"name\":\"(2018-2019-1)-ac13542104-601240-1\",\"tcmId\":94600,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"JAVA程序设计\"}},\"lssgId\":53064,\"fullName\":\"JAVA程序设计\"}},\"tcsId\":7032632,\"dateAccept\":\"2018-06-11T20:27:31\"},{\"teachClassMaster\":{\"maxStudCnt\":58,\"lessonSchedules\":[{\"classroom\":{\"roomId\":758,\"fullName\":\"前卫-经信教学楼#E区203-B1静思课堂\"},\"timeBlock\":{\"classSet\":3584,\"name\":\"周一第9,10,11节{第1-10周}\",\"endWeek\":10,\"beginWeek\":1,\"tmbId\":2819,\"dayOfWeek\":1},\"lsschId\":128608}],\"studCnt\":58,\"lessonTeachers\":[{\"teacher\":{\"name\":\"陈松友\",\"teacherId\":4632}}],\"name\":\"(2018-2019-1)-ac13251009-240019-4\",\"tcmId\":95899,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"政治理论与思想教育专题Ⅲ\"}},\"lssgId\":53621,\"fullName\":\"政治理论与思想教育专题Ⅲ\"}},\"tcsId\":7043633,\"dateAccept\":\"2018-06-11T20:28:32\"},{\"teachClassMaster\":{\"maxStudCnt\":31,\"lessonSchedules\":[{\"classroom\":{\"roomId\":766,\"fullName\":\"前卫-经信教学楼#E区303\"},\"timeBlock\":{\"classSet\":6,\"name\":\"周四第1,2节{第1-19周}\",\"endWeek\":19,\"beginWeek\":1,\"tmbId\":6911,\"dayOfWeek\":4},\"lsschId\":128576},{\"classroom\":{\"roomId\":766,\"fullName\":\"前卫-经信教学楼#E区303\"},\"timeBlock\":{\"classSet\":6,\"name\":\"周一第1,2节{第1-19周}\",\"endWeek\":19,\"beginWeek\":1,\"tmbId\":6821,\"dayOfWeek\":1},\"lsschId\":128575}],\"studCnt\":31,\"lessonTeachers\":[{\"teacher\":{\"name\":\"赵晶旌\",\"teacherId\":2467}}],\"name\":\"(2018-2019-1)-ac13162003-600847-1\",\"tcmId\":95837,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"大学英语AⅢ\"}},\"lssgId\":53584,\"fullName\":\"大学英语AⅢ\"}},\"tcsId\":7063780,\"dateAccept\":\"2018-06-11T20:30:14\"},{\"teachClassMaster\":{\"maxStudCnt\":31,\"lessonSchedules\":[{\"classroom\":{\"roomId\":759,\"fullName\":\"前卫-经信教学楼#E区209-B2日新课堂\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周二第3,4节{第1-8周}\",\"endWeek\":8,\"beginWeek\":1,\"tmbId\":825,\"dayOfWeek\":2},\"lsschId\":134509},{\"classroom\":{\"roomId\":759,\"fullName\":\"前卫-经信教学楼#E区209-B2日新课堂\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周二第3,4节{第10-18周}\",\"endWeek\":18,\"beginWeek\":10,\"tmbId\":12884,\"dayOfWeek\":2},\"lsschId\":134511},{\"classroom\":{\"roomId\":759,\"fullName\":\"前卫-经信教学楼#E区209-B2日新课堂\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周五第3,4节{第10-18周|双周}\",\"weekOddEven\":\"E\",\"endWeek\":18,\"beginWeek\":10,\"tmbId\":13021,\"dayOfWeek\":5},\"lsschId\":134512},{\"classroom\":{\"roomId\":759,\"fullName\":\"前卫-经信教学楼#E区209-B2日新课堂\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周五第3,4节{第1-8周|双周}\",\"weekOddEven\":\"E\",\"endWeek\":8,\"beginWeek\":1,\"tmbId\":13020,\"dayOfWeek\":5},\"lsschId\":134510}],\"studCnt\":31,\"lessonTeachers\":[{\"teacher\":{\"name\":\"王生生\",\"teacherId\":13848}}],\"name\":\"(2018-2019-1)-ac13541106-100638-1\",\"tcmId\":95780,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"算法分析\"}},\"lssgId\":53431,\"fullName\":\"算法分析\"}},\"tcsId\":7066733,\"dateAccept\":\"2018-06-11T20:30:37\"},{\"teachClassMaster\":{\"maxStudCnt\":31,\"lessonSchedules\":[{\"classroom\":{\"roomId\":759,\"fullName\":\"前卫-经信教学楼#E区209-B2日新课堂\"},\"timeBlock\":{\"classSet\":96,\"name\":\"周二第5,6节{第10-18周}\",\"endWeek\":18,\"beginWeek\":10,\"tmbId\":11322,\"dayOfWeek\":2},\"lsschId\":134524},{\"classroom\":{\"roomId\":759,\"fullName\":\"前卫-经信教学楼#E区209-B2日新课堂\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周五第3,4节{第10-18周|单周}\",\"weekOddEven\":\"O\",\"endWeek\":18,\"beginWeek\":10,\"tmbId\":13017,\"dayOfWeek\":5},\"lsschId\":134518},{\"classroom\":{\"roomId\":759,\"fullName\":\"前卫-经信教学楼#E区209-B2日新课堂\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周五第3,4节{第1-8周|单周}\",\"weekOddEven\":\"O\",\"endWeek\":8,\"beginWeek\":1,\"tmbId\":13016,\"dayOfWeek\":5},\"lsschId\":134519},{\"classroom\":{\"roomId\":759,\"fullName\":\"前卫-经信教学楼#E区209-B2日新课堂\"},\"timeBlock\":{\"classSet\":96,\"name\":\"周二第5,6节{第1-8周}\",\"endWeek\":8,\"beginWeek\":1,\"tmbId\":1045,\"dayOfWeek\":2},\"lsschId\":134514}],\"studCnt\":31,\"lessonTeachers\":[{\"teacher\":{\"name\":\"张永刚\",\"teacherId\":12774}}],\"name\":\"(2018-2019-1)-ac13541105-600385-1\",\"tcmId\":95089,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"离散数学II\"}},\"lssgId\":53436,\"fullName\":\"离散数学II\"}},\"tcsId\":7125189,\"dateAccept\":\"2018-06-11T20:36:44\"},{\"teachClassMaster\":{\"maxStudCnt\":31,\"lessonSchedules\":[{\"classroom\":{\"roomId\":380,\"fullName\":\"前卫-经信教学楼#E区304\"},\"timeBlock\":{\"classSet\":6,\"name\":\"周三第1,2节{第1-16周|单周}\",\"weekOddEven\":\"O\",\"endWeek\":16,\"beginWeek\":1,\"tmbId\":7740,\"dayOfWeek\":3},\"lsschId\":129125},{\"classroom\":{\"roomId\":380,\"fullName\":\"前卫-经信教学楼#E区304\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周四第3,4节{第1-16周}\",\"endWeek\":16,\"beginWeek\":1,\"tmbId\":1894,\"dayOfWeek\":4},\"lsschId\":129073},{\"classroom\":{\"roomId\":380,\"fullName\":\"前卫-经信教学楼#E区304\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周一第3,4节{第1-16周}\",\"endWeek\":16,\"beginWeek\":1,\"tmbId\":1882,\"dayOfWeek\":1},\"lsschId\":129072}],\"studCnt\":31,\"lessonTeachers\":[{\"teacher\":{\"name\":\"孙鹏\",\"teacherId\":1954}}],\"name\":\"(2018-2019-1)-ac13931003-609514-1\",\"tcmId\":94956,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"高等数学AⅢ\"}},\"lssgId\":53331,\"fullName\":\"高等数学AⅢ\"}},\"tcsId\":7130313,\"dateAccept\":\"2018-06-12T08:30:08\"},{\"teachClassMaster\":{\"maxStudCnt\":60,\"lessonSchedules\":[{\"classroom\":{\"roomId\":758,\"fullName\":\"前卫-经信教学楼#E区203-B1静思课堂\"},\"timeBlock\":{\"classSet\":384,\"name\":\"周五第7,8节{第1-19周}\",\"endWeek\":19,\"beginWeek\":1,\"tmbId\":6230,\"dayOfWeek\":5},\"lsschId\":135894},{\"classroom\":{\"roomId\":758,\"fullName\":\"前卫-经信教学楼#E区203-B1静思课堂\"},\"timeBlock\":{\"classSet\":1536,\"name\":\"周二第9,10节{第1-19周}\",\"endWeek\":19,\"beginWeek\":1,\"tmbId\":11072,\"dayOfWeek\":2},\"lsschId\":135895}],\"studCnt\":60,\"lessonTeachers\":[{\"teacher\":{\"name\":\"徐向红\",\"teacherId\":17657}}],\"name\":\"(2018-2019-1)-ac13931013-900330-1\",\"tcmId\":94957,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"概率论与数理统计A\"}},\"lssgId\":53253,\"fullName\":\"概率论与数理统计A\"}},\"tcsId\":7130691,\"dateAccept\":\"2018-06-12T08:30:11\"},{\"teachClassMaster\":{\"maxStudCnt\":111,\"lessonSchedules\":[{\"classroom\":{\"roomId\":-1,\"fullName\":\"地点待定\"},\"timeBlock\":{\"classSet\":96,\"name\":\"周六第5,6节{第5-19周}\",\"endWeek\":19,\"beginWeek\":5,\"tmbId\":11801,\"dayOfWeek\":6},\"lsschId\":139528}],\"studCnt\":122,\"lessonTeachers\":[{\"teacher\":{\"name\":\"李丹\",\"teacherId\":7170}}],\"name\":\"(2018-2019-1)-ac13911013-600703-1\",\"tcmId\":100712,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"体育Ⅲ\"}},\"lssgId\":56356,\"fullName\":\"体育Ⅲ\"}},\"tcsId\":7361491,\"dateAccept\":\"2018-09-19T16:43:11\"},{\"teachClassMaster\":{\"maxStudCnt\":290,\"lessonSchedules\":[{\"classroom\":{\"roomId\":292,\"fullName\":\"前卫-经信教学楼#F区第六阶梯\"},\"timeBlock\":{\"classSet\":3584,\"name\":\"周四第9,10,11节{第5-15周}\",\"endWeek\":15,\"beginWeek\":5,\"tmbId\":2315,\"dayOfWeek\":4},\"lsschId\":139612}],\"studCnt\":286,\"lessonTeachers\":[{\"teacher\":{\"name\":\"金佳\",\"teacherId\":247465}}],\"name\":\"(2018-2019-1)-tg02900403-900635-1\",\"tcmId\":100532,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"在影视中学心理\"}},\"lssgId\":55451,\"fullName\":\"在影视中学心理\"}},\"tcsId\":7459905,\"dateAccept\":\"2018-09-20T08:31:09\"},{\"teachClassMaster\":{\"maxStudCnt\":200,\"lessonSchedules\":[{\"classroom\":{\"roomId\":661,\"fullName\":\"前卫-第三教学楼#第二阶梯\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周日第3,4节{第8-12周|双周}\",\"weekOddEven\":\"E\",\"endWeek\":12,\"beginWeek\":8,\"tmbId\":14938,\"dayOfWeek\":7},\"lsschId\":139668},{\"classroom\":{\"roomId\":661,\"fullName\":\"前卫-第三教学楼#第二阶梯\"},\"timeBlock\":{\"classSet\":24,\"name\":\"周日第3,4节{第15-15周}\",\"endWeek\":15,\"beginWeek\":15,\"tmbId\":14939,\"dayOfWeek\":7},\"lsschId\":139669}],\"studCnt\":88,\"lessonTeachers\":[{\"teacher\":{\"name\":\"李芳菲\",\"teacherId\":11435}}],\"name\":\"(2018-2019-1)-MOOCGX7001-604838-2\",\"tcmId\":100665,\"lessonSegment\":{\"lesson\":{\"courseInfo\":{\"courName\":\"材料学概论\"}},\"lssgId\":55941,\"fullName\":\"材料学概论\"}},\"tcsId\":7523037,\"dateAccept\":\"2018-09-21T12:55:41\"}],\"resName\":\"teachClassStud\",\"msg\":\"\"}", "application/json");
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

            if (resourceTag == "search@teachingTerm")
            {
                if ("default" == inputContent.branch)
                    return SearchTeachingTerm(-1);
                else return SearchTeachingTerm(inputContent.@params.termId);
            }
            else if (resourceTag == "stat-avg-gpoint")
            {
                return Content("{\"status\":0,\"value\":[{" +
                    "\"avgScoreBest\":89.11016949152542372881355932203389830508," +
                    "\"avgScoreFirst\":89.11016949152542372881355932203389830508," +
                    "\"gpaFirst\":3.58898305084745762711864406779661016949," +
                    "\"gpaBest\":3.58898305084745762711864406779661016949}]," +
                    "\"resName\":\"stat-avg-gpoint\",\"msg\":\"\"}", "application/json");
            }
            else if (resourceTag == "archiveScore@queryCourseScore")
            {
                return QueryCourseScore();
            }
            else if (resourceTag == "teachClassStud@schedule")
            {
                return Schedule();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route(courseScoreStat)]
        public IActionResult CourseScoreStat() => Json(new GradeDetails(true));

        [HttpPost]
        [Route(evalTeach)]
        public IActionResult EvalTeach() => Content("{}", "application/json");

        [HttpPost]
        [Route(selectLesson)]
        public IActionResult SelectLesson() => Content("{}", "application/json");

        [HttpPost]
        [Route(messageFetch)]
        public IActionResult MessageFetch() => Json(new MessageBox(true));

        [HttpPost]
        [Route(messageRead)]
        public IActionResult MessageRead() => Content("{}", "application/json");

        [HttpPost]
        [Route(messageDelete)]
        public IActionResult MessageDelete() => Content("{}", "application/json");
    }
}
