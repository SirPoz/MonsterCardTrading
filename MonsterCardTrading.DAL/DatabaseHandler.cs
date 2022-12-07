using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.DAL
{
    public class DatabaseHandler
    {
        PostgresRepository pr = new PostgresRepository();
        public DatabaseHandler()
        {

        }

        public bool CreateUser(User newUser)
        {
            return false;
        }

        public void UpdateUser(User newUser)
        {

        }

        public bool CheckUserCredentials(string username, string password)
        {
            return false;
        }


    }
}
