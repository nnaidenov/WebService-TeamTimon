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

        public Models.Chat Add(Chat.Models.Chat item, string sessionKey)
        {
            var userId = this.dbContext.Set<User>().Where(u => u.SessionKey == sessionKey).Select(u => u.UserID).FirstOrDefault();
           if ((int)userId > 0)
           {
               this.entitySet.Add(item);
               this.dbContext.SaveChanges();
               return item;
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
