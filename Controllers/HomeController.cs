using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.LogService.Interface;
using Core.LogService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.LogService.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        public ILogService _logService;

        public HomeController(IConfiguration configuration)
        {
            string defaultLogMode = configuration.GetValue<string>("LogService:DefaultMode");

            if (defaultLogMode == "AtlasMongoDb")
                _logService = new MongoDbService(configuration);
            else
                _logService = new FileLogService(configuration);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Fetch([FromBody] Models.filterObj payload)
        {
            return Ok(await _logService.FindLog(payload.filter.ToString(), payload.collection));
        }

        [HttpPut]
        public async Task<ActionResult> Save([FromBody] Models.saveObj payload)
        {
            return Ok(await Task.Run(() => _logService.SaveLog(payload.document.ToString(), payload.collection).Result));

            //return Ok(await _mongoDbService.SaveLog(payload));
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] Models.updateObj payload)
        {
            return Ok(await Task.Run(() => _logService.UpdateLog(payload.filter.ToString(), payload.document.ToString(), payload.collection).Result));

            //return Ok(await _mongoDbService.SaveLog(payload));
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] Models.filterObj payload)
        {
            return Ok(await Task.Run(() => _logService.DeleteLog(payload.filter.ToString(), payload.collection).Result));
        }
    }
}
