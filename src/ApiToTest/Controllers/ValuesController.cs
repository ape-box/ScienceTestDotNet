using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ScienceTest.ApiToTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            await Task.Delay(new Random(DateTimeOffset.UtcNow.Millisecond).Next(150, 1500));
            
            return DateTimeOffset.UtcNow.Second % 2 == 0
                ? new [] {"value1", "value3"}
                : new [] {"value1", "value2"};
        }
    }
}
