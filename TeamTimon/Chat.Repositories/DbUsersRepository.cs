using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Chat.Models;

namespace Chat.Repositories
{
    public class DbUsersRepository : IRepository<User>
    {
        private const string SessionKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SessionKeyLen = 50;

        private readonly DbContext dbContext;
        private readonly DbSet<User> entitySet;

        public DbUsersRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = this.dbContext.Set<User>();
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

                this.entitySet.Add(dbUser);
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
            using (this.dbContext)
            {
                var user = this.entitySet.FirstOrDefault(u => u.SessionKey == sessionKey);
                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                this.DeleteAllChannels(user.UserID);

                user.SessionKey = null;
                this.dbContext.SaveChanges();
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

        private void DeleteAllChannels(int userId)
        {
            var result = this.dbContext.Set<Channel>().Where(u => u.UserID == userId || u.SecondUserID == userId).Select(c => c).ToList();

            foreach (var item in result)
            {
                this.dbContext.Set<Channel>().Remove(item);
            }

            this.dbContext.SaveChanges();
        }
    }
}