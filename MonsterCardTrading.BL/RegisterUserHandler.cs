using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardTrading.DAL;
using MonsterCardTrading.Model;

namespace MonsterCardTrading.BL
{
    public class RegisterUserHandler
    {
        public RegisterUserHandler()
        {
        }

        public void RegisterUser(User newUser)
        {
            DatabaseHandler dbConnection= new DatabaseHandler();
            dbConnection.CreateUser(newUser);
        }
    }
}
