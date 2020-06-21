using System;
using Elect.Web.Swagger.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Goblin.Service_Resource.Controllers
{
    public class TestController : BaseController
    {
        /// <summary>
        ///     Test Model
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [ApiDocGroup("Test")]
        [HttpPost]
        [Route("/test")]
        public IActionResult Index([FromBody] TestModel content)
        {
            return Ok(content);
        }

        public class TestModel
        {
            public string Data { get; set; }

            public TestEnum Enum { get; set; }

            public DateTimeOffset DateTimeData { get; set; }
            
            public TimeSpan TimeSpanData { get; set; }

            public double DoubleData { get; set; }

            public decimal DecimalData { get; set; }
        }

        public enum TestEnum
        {
            Value1 = 1,

            Value2 = 2
        }
    }
}