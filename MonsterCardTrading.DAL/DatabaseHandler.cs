﻿using Microsoft.Extensions.Logging;
using MonsterCardTrading.Model;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.DAL
{
    public class DatabaseHandler
    {

        private SchemaHandler ?schemaHandler;

        public void resetDatabase()
        {
            dropDatabase();
            setupDatabase();
        }

        public void AddUser(User user)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"INSERT INTO users (id, username, password, profile_text, elo, coins) Values (@userid, @username, @password, @profile, @elo, @coins);";


            NpgsqlCommand ?c = command as NpgsqlCommand;

            c.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("password", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("profile", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("elo", NpgsqlDbType.Integer);
            c.Parameters.Add("coins", NpgsqlDbType.Integer);

            c.Parameters["userid"].Value = user.Id;
            c.Parameters["username"].Value = user.Username;
            c.Parameters["password"].Value = user.Password;
            c.Parameters["profile"].Value = user.Profile;
            c.Parameters["elo"].Value = user.ELO;
            c.Parameters["coins"].Value = user.Coins;

           
            try
            {
                int result = c.ExecuteNonQuery();
                if (result != 1)
                {
                    throw new Exception("User could not be created");
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            

            PostgresRepository.releaseConnections(conID);


            

        }

        public void UpdateUser(User user)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"UPDATE users SET username = @username, SET password = @password, SET profile_text = @profile, SET elo = @elo, SET coins = @coins WHERE id = @userid;";

            NpgsqlCommand ?c = command as NpgsqlCommand;

            c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("password", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("profile", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("elo", NpgsqlDbType.Integer);
            c.Parameters.Add("coins", NpgsqlDbType.Integer);

            c.Parameters["username"].Value = user.Username;
            c.Parameters["password"].Value = user.Password;
            c.Parameters["profile"].Value = user.Profile;
            c.Parameters["elo"].Value = user.ELO;
            c.Parameters["coins"].Value = user.Coins;

            int result = command.ExecuteNonQuery();
            PostgresRepository.releaseConnections(conID);

            if (result != 1)
            {
                throw new Exception("User could not be updated");
            }

        }

        public User GetUser(string username)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"SELECT id, username, password, profile_text, elo, coins FROM users WHERE username = @username;";

            NpgsqlCommand ?c = command as NpgsqlCommand;

            c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
            c.Parameters["username"].Value = username;

            IDataReader result = command.ExecuteReader();


            if(result.Read())
            {
                User user = new User();
                user.Id = result.GetString(0);
                user.Username = username;
                user.Password = result.GetString(2);
                user.Profile = result.GetString(3);
                user.ELO = result.GetInt32(4);
                user.Coins = result.GetInt32(5);

                result.Close();
                PostgresRepository.releaseConnections(conID);

                return user;
            }

            result.Close();
            PostgresRepository.releaseConnections(conID);
            throw new Exception("User not found");
        }

        public bool CheckUserCredentials(string username, string password)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"SELECT id, username, password, profile_text, elo FROM users WHERE username = @username;";

            NpgsqlCommand ?c = command as NpgsqlCommand;

            c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
            c.Parameters["username"].Value = username;

            IDataReader result = command.ExecuteReader();

           
            if(result.Read())
            {
                
                if (password == result.GetString(2))
                {
                    Console.WriteLine(password + ":" + result.GetString(2));
                    result.Close();
                    PostgresRepository.releaseConnections(conID);
                    return true;
                }
            }

            

            result.Close();
            PostgresRepository.releaseConnections(conID);
            throw new Exception("invalid username or password");
        }

        public bool CheckUsernameExists(string username)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"SELECT username FROM users WHERE username = @username;";


            NpgsqlCommand ?c = command as NpgsqlCommand;

            c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
            c.Parameters["username"].Value = username;

            var result = command.ExecuteReader();

            bool check = false;
            if (result.Read())
            {
                check = true;
            }

            result.Close();
            PostgresRepository.releaseConnections(conID);

            return check;

        }


        public void DeleteUser(User user)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"DELETE FROM users Where id = @userid;";

            NpgsqlCommand ?c = command as NpgsqlCommand;

            c.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);
            c.Parameters["userid"].Value = user.Id;

            int result = command.ExecuteNonQuery();
            PostgresRepository.releaseConnections(conID);
            if (result != 1)
            {
               
                
                throw new Exception("User could not be deleted");
            }

        }
        
        public void AddPackage(Stack package)
        {
            //add Card to card table
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbTransaction trans = con.BeginTransaction();


            IDbCommand commandCards = con.CreateCommand();
            commandCards.CommandText = @"INSERT INTO cards (id, name, damage, type, element, packageid) Values (@cardid, @name, @damage, @type, @element, @packageid);";

            NpgsqlCommand? c = commandCards as NpgsqlCommand;

            c.Parameters.Add("cardid", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("damage", NpgsqlDbType.Integer);
            c.Parameters.Add("type", NpgsqlDbType.Integer);
            c.Parameters.Add("element", NpgsqlDbType.Integer);
            c.Parameters.Add("packageid", NpgsqlDbType.Integer);


            IDbCommand commandStack = con.CreateCommand();
            commandStack.CommandText = @"INSERT INTO stacks (card_id, in_deck) Values (@cardid, 0);";

            NpgsqlCommand? s = commandStack as NpgsqlCommand;

            
            s.Parameters.Add("cardid", NpgsqlDbType.Varchar, 255);
            try
            {
                foreach (Card card in package.Cards)
                {
                    c.Parameters["cardid"].Value = card.Id;
                    c.Parameters["name"].Value = card.Name;
                    c.Parameters["damage"].Value = card.Damage;
                    c.Parameters["type"].Value = (int)card.Type;
                    c.Parameters["element"].Value = (int)card.Element;
                    c.Parameters["packageid"].Value = card.packageid;

                    commandCards.ExecuteNonQuery();

                    
                    s.Parameters["cardid"].Value = card.Id;

                    commandStack.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                trans.Rollback();
                Console.WriteLine(e.Message);
                PostgresRepository.releaseConnections(conID);
                throw new Exception("Package could not be generated");
            }

            trans.Commit();

            PostgresRepository.releaseConnections(conID);

        }


        public int getMaxPackageId()
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
           


            IDbCommand command = con.CreateCommand();
            command.CommandText = "SELECT MAX(packageid) FROM cards;";

            var result = command.ExecuteReader();

            int packageid = 0;
            if (result.Read())
            {
                if(!result.IsDBNull(0))
                {
                    packageid = result.GetInt32(0);
                }
                
            }
            result.Close();
            PostgresRepository.releaseConnections(conID);

            return packageid;


        }
        /*public void UpdateDeck(User user, Card card, int in_deck)
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

            //pr.writeDB(command, parameters);

            //transfer second card
            command = @"UPDATE stacks 
                        SET user_id = @userid2
                        SET in_deck = 0
                        WHERE user_id = @userid1 and card_id = @cardid1";

            parameters.Clear();
            parameters.Add(new NpgsqlParameter("@userid1", user1.Id));
            parameters.Add(new NpgsqlParameter("@userid2", user2.Id));
            parameters.Add(new NpgsqlParameter("@cardid1", card1.Id));

            //pr.writeDB(command, parameters);
        }


        public void DeleteCard(Card card)
        {

        }
        public Card GetCard(string cardid)
        {
            string command = @"SELECT id, name, damage, type, element FROM cards WHERE id = @id;";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            parameters.Add(new NpgsqlParameter("@id", cardid));

            

            Card card = new Card();
            card.Id = result.GetString(0);
            card.Name = result.GetString(1);
            card.Damage = result.GetInt32(2);
            card.Element = (MonsterCardTrading.Model.Element)result.GetInt32(3);
            card.Type = (MonsterCardTrading.Model.Type)result.GetInt32(4);
            return card;
        }

        public void GetDeck(string userid)
        {
            string command = @"Select card_id From stacks Where user_id = @userid and in_deck = 1;";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            parameters.Add(new NpgsqlParameter(@userid, userid));

            NpgsqlDataReader result = //pr.readDB(command, parameters);


        }

        

            
        }

        private void checkDatabaseTables()
        {
            string command = "SELECT * FROM information_schema.tables;";

            
            Console.WriteLine("Tables:");

            NpgsqlDataReader result = //pr.readDB(command, null);


        }*/


        private void setupDatabase()
        {
            schemaHandler = new SchemaHandler();

            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;

            IDbCommand command = con.CreateCommand();
           
            

            foreach (KeyValuePair<string, string> table in schemaHandler._schema)
            {
                command.CommandText = table.Value;
                command.ExecuteNonQuery();
            }

            PostgresRepository.releaseConnections(conID);
        }

        private void dropDatabase()
        {
            schemaHandler = new SchemaHandler();
             
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;

            IDbCommand command = con.CreateCommand();
            string baseCommand = "DROP TABLE IF EXISTS ";
            


            foreach (KeyValuePair<string, string> table in schemaHandler._schema)
            {
                command.CommandText = baseCommand + table.Key;
                command.ExecuteNonQuery();
            }

            PostgresRepository.releaseConnections(conID);

        }
    }
}
