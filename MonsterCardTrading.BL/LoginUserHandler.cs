using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardTrading.DAL;

namespace MonsterCardTrading.BL
{
    public class LoginUserHandler
    {
        public LoginUserHandler()
        {
        }

        public User LoginUser(string username, string password)
        {
            DatabaseHandler dbconnection = new DatabaseHandler();
            if (dbconnection.CheckUserCredentials(username, password))
            {
                return new User(username, password);
            }
            else
            {
                return null;
            }
        }
    }
}
