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
        private SessionHandler session;

        public UserHandler()
        {
            db = new DatabaseHandler();
            session = new SessionHandler();
        }

        public void createUser(string username, string password)
        {
            if(db.CheckUsernameExists(username))
            {
                throw new ResponseException("User with same username already registered", 409);
            }
            
            User user = new User();
            Guid id = Guid.NewGuid();
            user.Id = id.ToString();
            user.Username = username;
            user.Username = username;
            user.Password = password;
            user.ELO = 100;
            user.Coins = 20;
            
            try
            {
                db.AddUser(user);
            }
            catch (Exception e)
            {
                throw new ResponseException(e.Message, 500);
               
            }
           
        }

        public string loginUser(string username, string password)
        {
            if(db.CheckUserCredentials(username, password))
            {
                try
                {
                    User user = db.GetUser(username);
                    return session.addSession(user);
                }
                catch(Exception e)
                {
                    throw new ResponseException(e.Message,500);
                }
                                
            }
            throw new ResponseException("Invalid username/password provided",401);

            
        }

        public User userFromToken(string token)
        {
            string[] header = token.Split(' ');
            return session.getSession(header[2]);

        }

        public void updateUser(User current, User changes)
        {
            if(changes.Name != null)
            {
                current.Name = changes.Name;
            }
            if(changes.Profile != null)
            {
                current.Profile = changes.Profile;
            }
            if(changes.Picture != null)
            {
                current.Picture = changes.Picture;
            }

            db.UpdateUser(current);
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

        public List<ScoreEntry> scoreBoard()
        {
            return db.ScoreBoard();
        }

        
    }
}
