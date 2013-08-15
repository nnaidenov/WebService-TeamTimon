using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Chat.Models;

namespace Chat.Services.Controllers
{
    public class MessagesController : ApiController
    {
        private ChatEntities db = new ChatEntities();

        // GET api/Messages
        public IEnumerable<Message> GetMessages(int chatId)
        {
            var messages = (from m in db.Messages
                            where m.ChatID == chatId
                            select m);

            return messages.AsEnumerable();
        }

        // GET api/Messages/5
        //public Message GetMessage(int id)
        //{
        //    Message message = db.Messages.Find(id);
        //    if (message == null)
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        //    }

        //    return message;
        //}

        //// PUT api/Messages/5
        //public HttpResponseMessage PutMessage(int id, Message message)
        //{
        //    if (ModelState.IsValid && id == message.MessageID)
        //    {
        //        db.Entry(message).State = EntityState.Modified;

        //        try
        //        {
        //            db.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //}

        // POST api/Messages

        public HttpResponseMessage PostMessage(Message message)
        {
            var chatId = message.ChatID;
            var chat = db.Chats.Find(chatId);

            var userId = message.UserID;

            if (chatId <= 0 || userId <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            if (ModelState.IsValid)
            {
                chat.Messages.Add(message);
                db.Messages.Add(message);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, message);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = message.MessageID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Messages/5
        //public HttpResponseMessage DeleteMessage(int id)
        //{
        //    Message message = db.Messages.Find(id);
        //    if (message == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound);
        //    }

        //    db.Messages.Remove(message);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, message);
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
