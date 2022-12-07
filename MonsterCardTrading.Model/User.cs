using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class User
    {
        private string Id;
        private string Username;
        private string Password;

        
        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }



        
    }
}
