using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Chat.Models;
using Chat.Repositories;
using Chat.Services.Models;

namespace Chat.Services.Controllers
{
    public class ChatsController : ApiController
    {
        private ChatEntities db = new ChatEntities();

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage CreateGame(string sessionKey, [FromBody] CreateChatModel chatModel)
        {
            DbChatRepository rep = new DbChatRepository(db);
            var result = rep.CreateChat(chatModel.SecondUserId, sessionKey);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // GET api/Chats
        public IEnumerable<Chat.Models.Chat> GetChats()
        {
            return db.Chats.AsEnumerable();
        }

        // GET api/Chats/5
        public Chat.Models.Chat GetChat(int id)
        {
            Chat.Models.Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return chat;
        }

        // PUT api/Chats/5
        public HttpResponseMessage PutChat(int id, Chat.Models.Chat chat)
        {
            if (ModelState.IsValid && id == chat.ChatID)
            {
                db.Entry(chat).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Chats
        public HttpResponseMessage PostChat(Chat.Models.Chat chat)
        {
            if (ModelState.IsValid)
            {
                db.Chats.Add(chat);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, chat);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = chat.ChatID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Chats/5
        public HttpResponseMessage DeleteChat(int id)
        {
            Chat.Models.Chat chat = db.Chats.Find(id);
            if (chat == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Chats.Remove(chat);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, chat);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}