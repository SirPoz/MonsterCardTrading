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
        private PostgresRepository pr; 
        public DatabaseHandler()
        {
            pr = new PostgresRepository();
            setupDatabase();
        }

        public void AddUser(User user)
        {

        }

        public void UpdateUser(User user)
        {

        }

        public void DeleteUser(User user)
        {

        }

        public void AddCard(Card card)
        {

        }
        public void UpdateCard(Card card)
        {

        }
        public void DeleteCard(Card card)
        {

        }

        private void setupDatabase()
        {

        }



        public bool CheckUserCredentials(string username, string password)
        {
            return false;
        }


    }
}
