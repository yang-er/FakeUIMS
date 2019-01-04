using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FakeUIMS.Controllers
{
    public class ServiceController : Controller
    {
        [Route("ntms/service/res.do")]
        public IActionResult ResDoDispatcher(string input)
        {
            return Content(input, "application/json");
        }
    }
}
