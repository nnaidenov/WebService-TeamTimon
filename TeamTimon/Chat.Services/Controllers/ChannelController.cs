using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Chat.Services.Controllers
{
    public class ChannelController : ApiController
    {
        // GET api/channel
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/channel/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/channel
        public void Post([FromBody]string value)
        {
        }

        // PUT api/channel/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/channel/5
        public void Delete(int id)
        {
        }
    }
}
