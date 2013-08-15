using Chat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Repositories
{
    public class DbChatRepository : IRepository<Chat.Models.Chat>
    {
        private DbContext dbContext;
        private DbSet<Chat.Models.Chat> entitySet;

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
                string chatName = userFirst.Username + userSecond.Username;
                string chatNameSecond = userSecond.Username + userFirst.Username;

                var channel = entitySet.Where(c => c.ChannelName == chatName).FirstOrDefault();
                var channelSecond = entitySet.Where(c => c.ChannelName == chatNameSecond).FirstOrDefault();
                if (channel == null && channelSecond == null)
                {
                    var newChat = new Chat.Models.Chat();
                    newChat.ChannelName = chatName;
                    newChat.Users = new List<User> { userFirst, userSecond };

                    this.entitySet.Add(newChat);
                    this.dbContext.SaveChanges();

                    return newChat;
                }
                else
                {
                    var selectChat = entitySet.Where(c => c.ChannelName == chatNameSecond).FirstOrDefault();

                    if (selectChat == null)
                    {
                        selectChat = entitySet.Where(c => c.ChannelName == channelSecond.ChannelName).FirstOrDefault();
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
    }
}
