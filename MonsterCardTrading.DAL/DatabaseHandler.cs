using Microsoft.Extensions.Logging;
using MonsterCardTrading.Model;
using Npgsql;
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
        private SchemaHandler schemaHandler;
      
        public DatabaseHandler()
        {
            pr = new PostgresRepository();
           
            dropDatabase();
            //checkDatabaseTables();

            setupDatabase();
            //checkDatabaseTables();
           
        }

        public void AddUser(User user)
        {
            string command = @"INSERT INTO users (id, username, password, profile_text, elo) Values (@userid, @username, @password, @profile_text, @elo);";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("@userid", user.Id));
            parameters.Add(new NpgsqlParameter("@username", user.Username));
            parameters.Add(new NpgsqlParameter("@password", user.Password));
            parameters.Add(new NpgsqlParameter("@profile", user.Profile));
            parameters.Add(new NpgsqlParameter("@elo", user.ELO));

            

            pr.writeDB(command, parameters);
        }

        public void UpdateUser(User user)
        {
            string command = @"UPDATE users 
                                SET username = @username,
                                SET password = @password,
                                SET profile_text = @profile,
                                SET elo = @elo
                              WHERE id = @userid;";


            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("@userid", user.Id));
            parameters.Add(new NpgsqlParameter("@username", user.Username));
            parameters.Add(new NpgsqlParameter("@password", user.Password));
            parameters.Add(new NpgsqlParameter("@profile", user.Profile));
            parameters.Add(new NpgsqlParameter("@elo", user.ELO));



            pr.writeDB(command, parameters);
        }

        public User GetUser(string username, string password)
        {
            return null;
        }

        

        public void DeleteUser(User user)
        {
            string command = @"DELETE FROM users Where id = @userid;";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("@userid", user.Id));

            pr.writeDB(command, parameters);
        }

        public void AddCard(Card card, User user)
        {
            //add Card to card table
            string command = @"INSERT INTO cards (id, name, damage, type, element) Values (@id, @name, @damage, @type, @element);";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("@cardid", card.Id));
            parameters.Add(new NpgsqlParameter("@name", card.Name));
            parameters.Add(new NpgsqlParameter("@damge", card.Damage));
            parameters.Add(new NpgsqlParameter("@type", card.Type));
            parameters.Add(new NpgsqlParameter("@element", card.Element));


            pr.writeDB(command, parameters);

            parameters.Clear();

            //add Card to stack of user
            command = @"INSERT INTO stacks (id,user_id, card_id, in_deck) Values (@id, @user_id, @card_id, 0);";
            parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("@id", card.Id));
            parameters.Add(new NpgsqlParameter("@user_id", card.Id));
            parameters.Add(new NpgsqlParameter("@card_id", user.Id));

            pr.writeDB(command, parameters);

        }
        public void UpdateDeck(User user, Card card, int in_deck)
        {


            string command = @"UPDATE stacks 
                        SET user_id = @userid1
                        SET in_deck = 0
                        WHERE user_id = @userid2 and card_id = @cardid2";
        }

        public void TransferCards(User user1, Card card1, User user2, Card card2)
        {
            //transfer first card
            string command = @"UPDATE stacks 
                        SET user_id = @userid1
                        SET in_deck = 0
                        WHERE user_id = @userid2 and card_id = @cardid2";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("@userid1", user1.Id));
            parameters.Add(new NpgsqlParameter("@userid2", user2.Id));
            parameters.Add(new NpgsqlParameter("@cardid2", card2.Id));

            pr.writeDB(command, parameters);

            //transfer second card
            command = @"UPDATE stacks 
                        SET user_id = @userid2
                        SET in_deck = 0
                        WHERE user_id = @userid1 and card_id = @cardid1";

            parameters.Clear();
            parameters.Add(new NpgsqlParameter("@userid1", user1.Id));
            parameters.Add(new NpgsqlParameter("@userid2", user2.Id));
            parameters.Add(new NpgsqlParameter("@cardid1", card1.Id));

            pr.writeDB(command, parameters);
        }


        public void DeleteCard(Card card)
        {

        }
        public Card GetCard(string cardid)
        {
            string command = @"SELECT id, name, damage, type, element FROM cards WHERE id = @id;";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            parameters.Add(new NpgsqlParameter("@id", cardid));

            NpgsqlDataReader result = pr.readDB(command, parameters);

            return new Card(
                result.GetString(0), //id
                result.GetString(1), //name
                result.GetInt32(2),  //damage
                result.GetInt32(3),  //type
                result.GetInt32(4)   //element
                );
        }

        public void GetDeck(string userid)
        {
            string command = @"Select card_id From stacks Where user_id = @userid and in_deck = 1;";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            parameters.Add(new NpgsqlParameter(@userid, userid));

            NpgsqlDataReader result = pr.readDB(command, parameters);


        }

        private void setupDatabase()
        {
            schemaHandler = new SchemaHandler();
            foreach(KeyValuePair<string, string> table in schemaHandler._schema)
            {
                pr.writeDB(table.Value,null);
            }

           
        }

        private void dropDatabase()
        {
            schemaHandler = new SchemaHandler();
            string command = "DROP TABLE IF EXISTS ";
            //List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            foreach (KeyValuePair<string, string> table in schemaHandler._schema)
            {
                //Console.WriteLine(table.Key, table.Value);
                //Console.WriteLine(table.Key, table.Value);
                //parameters.Add(new NpgsqlParameter("@table",table.Key));
                pr.writeDB(command + table.Key + ";", null);
                //parameters.Clear();


            }

            
        }

        private void checkDatabaseTables()
        {
            string command = "SELECT * FROM information_schema.tables;";

            
            Console.WriteLine("Tables:");

            NpgsqlDataReader result = pr.readDB(command, null);


        }

        public bool CheckUserCredentials(string username, string password)
        {
            string command = @"SELECT id, username, password, profile_text, elo FROM users WHERE username = @username;";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            parameters.Add(new NpgsqlParameter("@username", username));

            NpgsqlDataReader result = pr.readDB(command, parameters);

            if(result == null || result.Rows == 0)
            {
               
                return false;
            }

            if(password == result.GetString(2))
            {
               
                return true;
            }

           
            return false;
        }

        


    }
}
