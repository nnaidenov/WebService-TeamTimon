using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Chat.Services.Controllers;
using Chat.Repositories;
using Chat.Models;
using System.Data.Entity;

namespace Chat.Services.DependencyResolver
{
    public class DbDependencyResolver : IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(UsersController))
            {
                return new UsersController(new DbUsersRepository(new ChatEntities()));
            }
            else if (serviceType == typeof(ChannelsController))
            {
                return new ChannelsController(new DbChannelRepository(new ChatEntities()));
            }
            else if (serviceType == typeof(UploadController))
            {
                return new UploadController(new DbUsersRepository(new ChatEntities()));
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {

        }
    }
}