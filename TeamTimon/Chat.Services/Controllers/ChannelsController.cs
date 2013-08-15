using Chat.Models;
using Chat.Repositories;
using Chat.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Chat.Services.Controllers
{
    public class ChannelsController : ApiController
    {
        private readonly IRepository<Channel> channelRepository;
        private ChatEntities db = new ChatEntities();

        public ChannelsController(IRepository<Channel> repository)
        {
            this.channelRepository = repository;
        }

        [HttpGet]
        [ActionName("allUnsuscribeChannels")]
        public HttpResponseMessage UnsubscribeChannels(string sessionKey)
        {
            var rep = new DbChannelRepository(db);
            int userId = db.Set<User>().Where(u => u.SessionKey == sessionKey).Select(u => u.UserID).FirstOrDefault();
            var result = rep.GetAllUnsubscribeChannels(userId);

            List<ChannelModel> all = new List<ChannelModel>();
            foreach (var res in result)
            {
                ChannelModel newChannel = new ChannelModel
                {
                    ChannelName = res.ChannelName,
                    FirstUserId = res.UserID,
                    SecondUserId = res.SecondUserID,
                    FirstUsername = db.Set<User>().Where(u=>u.UserID == res.UserID).Select(u=>u.Username).FirstOrDefault(),
                    SecondUsername = db.Set<User>().Where(u => u.UserID == res.SecondUserID).Select(u => u.Username).FirstOrDefault(),
                };

                all.Add(newChannel);
            }

            var responseMsg = Request.CreateResponse(HttpStatusCode.OK, all);

            return responseMsg;
        }

        // GET api/channels
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/channels/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/channels
        public void Post([FromBody]string value)
        {
        }

        // PUT api/channels/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/channels/5
        public void Delete(int id)
        {
        }
    }
}
