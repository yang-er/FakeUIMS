using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeUIMS.Models.JSON;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FakeUIMS.Controllers
{
    public class ScoreController : Controller
    {
        [Route("ntms/score/course-score-stat.do")]
        public IActionResult CourseScoreStat(string asId)
        {
            return new JsonResult(new GradeDetails());
        }
    }
}
