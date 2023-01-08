using MonsterCardTrading.DAL;
using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL
{
    public class UserHandler
    {
        private DatabaseHandler db;

        public UserHandler()
        {
            db = new DatabaseHandler();    
        }

        public void createUser(string username, string password)
        {
            if(db.CheckUsernameExists(username))
            {
                throw new Exception("User with same username already registered");
            }
            
            User user = new User();
            Guid id = Guid.NewGuid();
            user.Id = id.ToString();
            user.Username = username;
            user.Password = password;
            user.hash = password;
            user.ELO = 100;
            user.Profile = "Hello I am new!";
            user.Coins = 20;
            
            try
            {
                db.AddUser(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
               
            }
           
        }

        public string loginUser(string username, string password)
        {
            if(db.CheckUserCredentials(username, password))
            {
                User user = db.GetUser(username);
                
                return SessionHandler.addSession(user);
            }
            throw new Exception(" Invalid username/password provided");

            
        }

        public User userFromToken(string token)
        {
            string[] header = token.Split(' ');
            return SessionHandler.getSession(header[2]);

        }

        public void deleteUser(User user)
        {

        }

        public void editUser(User user)
        {

        }

        public void showStats(User user)
        {

        }

        public void scoreBoard()
        {

        }

        
    }
}
