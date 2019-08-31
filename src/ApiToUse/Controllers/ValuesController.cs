using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ScienceTest.ApiToUse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            await Task.Delay(new Random(DateTimeOffset.UtcNow.Millisecond).Next(50, 500));
            
            return new [] {"value1", "value2"};
        }
    }
}
