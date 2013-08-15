using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Chat.Models;
using Chat.Repositories;
using Chat.Services.Models;

namespace Chat.Services.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IRepository<User> userRepository;
        private ChatEntities db = new ChatEntities();

        public UsersController(IRepository<User> repository)
        {
            this.userRepository = repository;
        }

        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage RegisterUser(User user)
        {
            var rep = new DbUsersRepository(db);
            rep.CreateUser(user.Username, user.Password);

            var sessionKey = rep.LoginUser(user.Username, user.Password);

            var loggedUser = new UserLoggedModel()
            {
                UserID = user.UserID,
                Username = user.Username,
                SessionKey = sessionKey
            };

            return Request.CreateResponse(HttpStatusCode.Created, loggedUser);
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage LoginUser(UserModel user)
        {
            IQueryable<User> users = this.userRepository.GetAll();

            var result = from u in users
                         where u.Username == user.Username && u.Password == user.Password
                         select u;

            User newUser = result.FirstOrDefault();
            if (newUser != null)
            {
                var rep = new DbUsersRepository(db);
                var sessionKey = rep.LoginUser(user.Username, user.Password);
                UserLoggedModel userModel = new UserLoggedModel()
                {
                    UserID = newUser.UserID,
                    Username = newUser.Username,
                    SessionKey = sessionKey
                };
                var responseMsg = Request.CreateResponse(HttpStatusCode.OK, userModel);
                return responseMsg;
            }
            else
            {
                var responseMsg = Request.CreateResponse(HttpStatusCode.NotFound);
                return responseMsg;
            }
        }

        [HttpGet]
        [ActionName("logout")]
        public HttpResponseMessage LogoutUser(string sessionKey)
        {
            var rep = new DbUsersRepository(db);
            rep.LogoutUser(sessionKey);

            var responseMsg = Request.CreateResponse(HttpStatusCode.OK);
            return responseMsg;

        }





        // GET api/user
        public IEnumerable<UserLoggedModel> Get()
        {
            var users = this.userRepository.GetAll().ToList();
            var loggedUsers = new List<UserLoggedModel>();

            foreach (var user in users)
            {
                UserLoggedModel curLoggedUser = new UserLoggedModel
                {
                    SessionKey = user.SessionKey,
                    UserID = user.UserID,
                    Username = user.Username
                };

                loggedUsers.Add(curLoggedUser);
            }

            return loggedUsers;
        }

        // GET api/user/5
        public User Get(int id)
        {
            return this.userRepository.Get(id);
        }

        // POST api/user
        //public UserModel Post(User value)
        //{
        //    var newUser = this.userRepository.Add(value);
        //    UserModel user = new UserModel
        //    {
        //        UserID = newUser.UserID,
        //        Username = newUser.Username,
        //        SessionKey = newUser.SessionKey
        //    };

        //    return user;
        //}

        // PUT api/user/5
        public void Put(int id, [FromBody]
                        string value)
        {
        }

        // DELETE api/user/5
        public void Delete(int id)
        {
            this.userRepository.Delete(id);
        }



    }
}
