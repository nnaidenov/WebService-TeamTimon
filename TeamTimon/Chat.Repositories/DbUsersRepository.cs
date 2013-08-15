using Chat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Repositories
{
    public class DbUsersRepository : IRepository<User>
    {
        private DbContext dbContext;
        private DbSet<User> entitySet;

        private const string SessionKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SessionKeyLen = 50;

        public DbUsersRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<User>();
        }

        private static string GenerateSessionKey()
        {
            StringBuilder keyChars = new StringBuilder(50);
            //keyChars.Append(userId.ToString());
            Random rand = new Random();
            while (keyChars.Length < SessionKeyLen)
            {
                int randomCharNum;
                lock (rand)
                {
                    randomCharNum = rand.Next(SessionKeyChars.Length);
                }
                char randomKeyChar = SessionKeyChars[randomCharNum];
                keyChars.Append(randomKeyChar);
            }
            string sessionKey = keyChars.ToString();
            return sessionKey;
        }

        public void CreateUser(string username, string password)
        {
            var usernameToLower = username.ToLower();
            var dbUser = this.entitySet.FirstOrDefault(u => u.Username == usernameToLower);
            if (dbUser == null)
            {
                dbUser = new User()
               {
                   Username = usernameToLower,
                   Password = password
               };

                entitySet.Add(dbUser);
                this.dbContext.SaveChanges();
            }
        }

        public string LoginUser(string username, string password)
        {
            var usernameToLower = username.ToLower();
            var user = this.entitySet.FirstOrDefault(u => u.Username.ToLower() == usernameToLower);
            if (user != null)
            {
                var sessionKey = GenerateSessionKey();
                user.SessionKey = sessionKey;
                this.dbContext.SaveChanges();

                return sessionKey;
            }

            return "Error";
        }

        public void LogoutUser(string sessionKey)
        {
            using (dbContext)
            {
                var user = entitySet.FirstOrDefault(u => u.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                user.SessionKey = null;
                dbContext.SaveChanges();
            }
        }
































        public User Add(User item)
        {
            item.SessionKey = GenerateSessionKey();
            this.entitySet.Add(item);
            this.dbContext.SaveChanges();

            return item;
        }

        public User Update(int id, User item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var item = this.entitySet.Find(id);
            this.entitySet.Remove(item);
            this.dbContext.SaveChanges();
        }

        public void Delete(User item)
        {
            throw new NotImplementedException();
        }

        public User Get(int id)
        {
            return this.entitySet.Find(id);
        }

        public IQueryable<User> GetAll()
        {
            return this.entitySet;
        }
    }
}
