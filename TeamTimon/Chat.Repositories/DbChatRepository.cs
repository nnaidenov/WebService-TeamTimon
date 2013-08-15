using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Chat.Models;

namespace Chat.Repositories
{
    public class DbChatRepository : IRepository<Chat.Models.Chat>
    {
        private readonly DbContext dbContext;
        private readonly DbSet<Chat.Models.Chat> entitySet;

        public DbChatRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<Chat.Models.Chat>();
        }

        public Models.Chat CreateChat(int id, string sessionKey)
        {
            var userFirst = this.dbContext.Set<User>().Where(u => u.SessionKey == sessionKey).Select(u => u).FirstOrDefault();
            var userSecond = this.dbContext.Set<User>().Where(u => u.UserID == id).Select(u => u).FirstOrDefault();

            if (userFirst.UserID > 0 && id > 0)
            {
                string chatName = string.Format("{0}{1}", userFirst.Username, userSecond.Username);
                string chatNameSecond = string.Format("{0}{1}", userSecond.Username, userFirst.Username);

                var channel = this.entitySet.Where(c => c.Channel.ChannelName == chatName).FirstOrDefault();
                var channelSecond = this.entitySet.Where(c => c.Channel.ChannelName == chatNameSecond).FirstOrDefault();
                if (channel == null && channelSecond == null)
                {
                    var newChat = new Chat.Models.Chat();
                    var newChannel = this.CreateChannel(userFirst, userSecond, newChat, this.dbContext);
                    newChat.ChannelID = newChannel.ChannelID;

                    newChat.Users = new List<User> { userFirst, userSecond };

                    this.entitySet.Add(newChat);
                    this.dbContext.SaveChanges();

                    return newChat;
                }
                else
                {
                    var selectChat = this.entitySet.Where(c => c.Channel.ChannelName == chatNameSecond).FirstOrDefault();

                    if (selectChat == null)
                    {
                        selectChat = this.entitySet.Where(c => c.Channel.ChannelName == channel.Channel.ChannelName).FirstOrDefault();
                    }

                    return selectChat;
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public Models.Chat Update(int id, Chat.Models.Chat item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            this.entitySet.Find(id);
        }

        public void Delete(Chat.Models.Chat item)
        {
            throw new NotImplementedException();
        }

        public Models.Chat Get(int id)
        {
            return this.entitySet.Find(id);
        }

        public IQueryable<Chat.Models.Chat> GetAll()
        {
            return this.entitySet;
        }

        public Models.Chat Add(Models.Chat item)
        {
            throw new NotImplementedException();
        }

        private Channel CreateChannel(User userFirst, User userSecond, Models.Chat newChat, DbContext dbContext)
        {
            Channel newChannel = new Channel
            {
                ChannelName = string.Format("{0}{1}", userFirst.Username, userSecond.Username),
                UserID = userFirst.UserID,
                SecondUserID = userSecond.UserID
            };

            dbContext.Set<Channel>().Add(newChannel);
            dbContext.SaveChanges();

            return newChannel;
        }
    }
}