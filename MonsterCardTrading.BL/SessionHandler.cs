using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL
{
    public static class SessionHandler
    {
        private static Dictionary<string, User> sessions = new Dictionary<string, User>();

        public static string addSession(User user)
        {
            string token = createUserToken(user.Username);
            sessions.Add(token, user);
            return token;
        }

        public static User getSession(string token)
        {
            
            sessions.TryGetValue(token, out User user);
            if(user == null)
            {
                throw new Exception("Unkown Token: " + token);
            }
            return user;
        }

        private static string createUserToken(string username)
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
