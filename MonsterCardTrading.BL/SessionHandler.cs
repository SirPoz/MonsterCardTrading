using MonsterCardTrading.DAL;
using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL
{
    public class SessionHandler
    {
        private DatabaseHandler db;
        public SessionHandler()
        {
            db = new DatabaseHandler();
        }
        public string addSession(User user)
        {

            string token = createUserToken(user.Username);
            db.AddToken(user, token);
            return token;
        }

        public User getSession(string token)
        {

            User user = db.GetUserFromToken(token);
            if(user == null)
            {
                throw new Exception("Unkown Token: " + token);
            }
            return user;
        }

        private string createUserToken(string username)
        {
            //switch only for the curl script
            switch (username)
            {
                case "kienboec":
                    return "kienboec-mtcgToken";


                case "altenhof":
                    return "altenhof-mtcgToken";

                case "admin":
                    return "admin-mtcgToken";

                default:
                    Guid guid = Guid.NewGuid();
                    return guid.ToString();

            }
        }
    }
}
