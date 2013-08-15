using Chat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chat.Repositories
{
    class DbChannelRepository : IRepository<Channel>
    {
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