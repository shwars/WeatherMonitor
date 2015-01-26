using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WeatherMonitorLib;

namespace WeatherMonitorWeb
{
    public class HumidityController : ApiController
    {
        // GET api/<controller>
        public WeatherRecord[] Get()
        {
            return WeatherDB.GetCurrentReading(ReadingType.Humidity);
        }

        // GET api/<controller>/5
        public async Task<string> Get(int id)
        {
            await WeatherDB.RecordSuccess(ReadingType.Humidity, WeatherInfoSource.Device, id);
            return DateTime.Now.ToString();
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}