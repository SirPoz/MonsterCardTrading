using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class User
    {
        private string _userid;
        private string _username;
        private string _password;

        private int coins;

        public int Coins { get; }
        public User(string username, string password)
        {
            this._username = username;
            this._password = password;
        }

        
    }
}
