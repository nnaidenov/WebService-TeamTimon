using Chat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Repositories
{
    public class DbChannelRepository : IRepository<Channel>
    {
        private DbContext dbContext;
        private DbSet<Channel> entitySet;

        public DbChannelRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<Channel>();
        }

        public List<Channel> GetAllUnsubscribeChannels(int userId)
        {
            var dbUser = this.entitySet.Where(u => u.UserID == userId || u.SecondUserID == userId).Select(c => c).ToList();

            return dbUser;
        }

        public Channel Add(Channel item)
        {
            throw new NotImplementedException();
        }

        public Channel Update(int id, Channel item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Channel item)
        {
            throw new NotImplementedException();
        }

        public Channel Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Channel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}