﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Chat.Models;
using Chat.Repositories;

namespace Chat.Services.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly IRepository<Message> messageRepository;
        private readonly ChatEntities db = new ChatEntities();

        public MessagesController(IRepository<Message> repository)
        {
            this.messageRepository = repository;
        }

        // GET api/Messages/GetMessages?chatId={chatId}
        public IEnumerable<Message> GetMessages(int chatId)
        {
            var messages = (from m in this.db.Messages
                            where m.ChatID == chatId
                            select m);

            return messages.AsEnumerable();
        }

        // GET api/Messages/GetMessage?id={id}
        public Message GetMessage(int id)
        {
            Message message = this.messageRepository.Get(id);
            if (message == null)
            {
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return message;
        }

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

        //// PUT api/Messages/5
        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //}
        // POST api/Messages/PostMessage
        public HttpResponseMessage PostMessage(Message message)
        {
            var chatId = message.ChatID;
            var chat = this.db.Chats.Find(chatId);

            var userId = message.UserID;

            if (chatId <= 0 || userId <= 0)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
            }

            if (this.ModelState.IsValid)
            {
                chat.Messages.Add(message);
                this.db.Messages.Add(message);
                this.db.SaveChanges();

                HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.Created, message);
                response.Headers.Location = new Uri(this.Url.Link("DefaultApi", new { id = message.MessageID }));
                return response;
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);
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
            this.db.Dispose();
            base.Dispose(disposing);
        }
    }
}