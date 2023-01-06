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
        public string token;
        public void createUser(string username, string password)
        {
            User user = new User();
            user.Username = username;
            user.hash = password;
            user.ELO = 100;
            user.Profile = "Hello I am new!";
            user.Coins = 20;
        }

        public User loginUser(User user)
        {
            return null;
        }

        public User userFromToken(string token)
        {
            return null;
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
