using Microsoft.Extensions.Logging;
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
            command.CommandText = @"INSERT INTO users (id, username, password,  elo, coins, name) Values (@userid, @username, @password, @elo, @coins, @name);";


            NpgsqlCommand ?c = command as NpgsqlCommand;

            c.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("password", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("elo", NpgsqlDbType.Integer);
            c.Parameters.Add("coins", NpgsqlDbType.Integer);

            
            c.Parameters["userid"].Value = user.Id;
            c.Parameters["username"].Value = user.Username;
            c.Parameters["password"].Value = user.Password;
            c.Parameters["name"].Value = user.Username;
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
            command.CommandText = @"UPDATE users SET profile_text = @profile,  picture = @picture,  elo = @elo, coins = @coins,  name = @name WHERE id = @userid;";

            NpgsqlCommand ?c = command as NpgsqlCommand;

            c.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("profile", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("picture", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("elo", NpgsqlDbType.Integer);
            c.Parameters.Add("coins", NpgsqlDbType.Integer);

            c.Parameters["userid"].Value = user.Id;
            c.Parameters["picture"].Value = user.Picture;
            c.Parameters["profile"].Value = user.Profile;
            c.Parameters["elo"].Value = user.ELO;
            c.Parameters["coins"].Value = user.Coins;
            c.Parameters["name"].Value = user.Name;

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
            command.CommandText = @"SELECT id, username, password, profile_text, elo, coins, picture, name FROM users WHERE username = @username;";

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
                user.Picture = result.GetString(6);
                user.Name = result.GetString(7);

                result.Close();
                PostgresRepository.releaseConnections(conID);

                return user;
            }

            result.Close();
            PostgresRepository.releaseConnections(conID);
            throw new Exception("User not found");
        }

        public User GetUserFromId(string id)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"SELECT id, username, password, profile_text, elo, coins, picture, name FROM users WHERE id = @id;";

            NpgsqlCommand? c = command as NpgsqlCommand;

            c.Parameters.Add("id", NpgsqlDbType.Varchar, 255);
            c.Parameters["id"].Value = id;

            IDataReader result = command.ExecuteReader();


            if (result.Read())
            {
                User user = new User();
                user.Id = result.GetString(0);
                user.Username = result.GetString(1);
                user.Password = result.GetString(2);
                user.Profile = result.GetString(3);
                user.ELO = result.GetInt32(4);
                user.Coins = result.GetInt32(5);
                user.Picture = result.GetString(6);
                user.Name = result.GetString(7);

                result.Close();
                PostgresRepository.releaseConnections(conID);

                return user;
            }

            result.Close();
            PostgresRepository.releaseConnections(conID);
            throw new Exception("User not found");
        }

        public void AddToken(User user, string token)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"Update users SET token = @token WHERE username = @username;";

            NpgsqlCommand? c = command as NpgsqlCommand;


            c.Parameters.Add("token", NpgsqlDbType.Varchar, 255);
            c.Parameters["token"].Value = token;

            c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
            c.Parameters["username"].Value = user.Username;

            int result = command.ExecuteNonQuery();

            PostgresRepository.releaseConnections(conID);

            if (result != 1)
            { 
                throw new Exception("session could not be established");
            }

        
           
        }

        public User GetUserFromToken(string token)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"SELECT id, username, profile_text, elo, coins, picture, name FROM users WHERE token = @token;";

            NpgsqlCommand? c = command as NpgsqlCommand;

            c.Parameters.Add("token", NpgsqlDbType.Varchar, 255);
            c.Parameters["token"].Value = token;

            IDataReader result = command.ExecuteReader();


            if (result.Read())
            {

                User user = new User();

                user.Id = result.GetString(0);
                user.Username = result.GetString(1);
                user.Profile = result.GetString(2);
                user.ELO = result.GetInt32(3);
                user.Coins = result.GetInt32(4);
                user.Picture = result.GetString(5);
                user.Name = result.GetString(6);

                result.Close();
                PostgresRepository.releaseConnections(conID);
                return user;
            }

            result.Close();
            PostgresRepository.releaseConnections(conID);
            throw new ResponseException("Access token is missing or invalid",444);
        }

        public int GetMoneyFromUser(User user)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"SELECT coins FROM users WHERE username = @username;";


            NpgsqlCommand? c = command as NpgsqlCommand;

            c.Parameters.Add("username", NpgsqlDbType.Varchar, 255);
            c.Parameters["username"].Value = user.Username;

            IDataReader result = command.ExecuteReader();


            if (result.Read())
            {
                    int coins = result.GetInt32(0);
                    result.Close();
                    PostgresRepository.releaseConnections(conID);
                    return coins;
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
            throw new ResponseException("invalid username or password",401);
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
                throw new ResponseException("Package could not be generated",400);
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

        public int getAvailablePackage()
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;


            //get unaquired package
            IDbCommand command = con.CreateCommand();
            command.CommandText = "SELECT cards.packageid FROM cards INNER JOIN stacks ON cards.id = stacks.card_id WHERE stacks.user_id IS NULL;";

            var result = command.ExecuteReader();

            int packageid = 0;
            if (result.Read())
            {
                if (!result.IsDBNull(0))
                {
                    packageid = result.GetInt32(0);
                }
                else
                {
                    result.Close();
                    PostgresRepository.releaseConnections(conID);
                    throw new ResponseException("No card package available for buying",404);
                }

            }
            else
            {
                result.Close();
                PostgresRepository.releaseConnections(conID);
                throw new ResponseException("No card package available for buying",404);
            }

            result.Close();
            PostgresRepository.releaseConnections(conID);
            return packageid;
        }

        public List<Card> DrawPackage(User user)
        {
            //check money of user
            int money = GetMoneyFromUser(user);
            if (money < 5)
            {
                throw new ResponseException("Not enough money for buying a card package",403);
            }



            //check available packages
            int packageid = getAvailablePackage();


            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbTransaction trans = con.BeginTransaction();
            
            IDbCommand stackCommand = con.CreateCommand();
            
            stackCommand.CommandText = @"UPDATE stacks SET user_id = @userid WHERE user_id IS NULL and (card_id IN (SELECT s.card_id FROM stacks s INNER JOIN cards ON cards.id = s.card_id WHERE (cards.packageid = @packageid)));";

            NpgsqlCommand stack = stackCommand as NpgsqlCommand;

            stack.Parameters.Add("packageid", NpgsqlDbType.Integer);
            stack.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);

            stack.Parameters["packageid"].Value = packageid;
            stack.Parameters["userid"].Value = user.Id;

            var changedRows = stack.ExecuteNonQuery();

            if(changedRows != 5)
            {
                trans.Rollback();
                PostgresRepository.releaseConnections(conID);
                throw new ResponseException("cards could not be asigned",500);
            }



            IDbCommand cardCommand = con.CreateCommand();
            cardCommand.CommandText = "SELECT id, name,  type, element, damage FROM cards Where packageid = @packageid;";

            NpgsqlCommand card = cardCommand as NpgsqlCommand;

            card.Parameters.Add("packageid", NpgsqlDbType.Integer);

            card.Parameters["packageid"].Value = packageid;

            IDataReader cards = cardCommand.ExecuteReader();

            List<Card> package = new List<Card>();


            while(cards.Read())
            {
                Card packageCard = new Card();
                packageCard.Id = cards.GetString(0);
                packageCard.Name = cards.GetString(1);
                packageCard.Type = (Species)cards.GetInt32(2);
                packageCard.Element = (Element)cards.GetInt32(2);
                packageCard.Damage = cards.GetInt32(2);
                package.Add(packageCard);
            }

            if(package.Count != 5)
            {
                trans.Rollback();
                cards.Close();
                PostgresRepository.releaseConnections(conID);
                throw new ResponseException("cards could not be retrieved", 500);
            }

            cards.Close();

            IDbCommand userCommand = con.CreateCommand();
            userCommand.CommandText = "UPDATE users SET coins=@coins WHERE id = @userid;";

            NpgsqlCommand userC = userCommand as NpgsqlCommand;

            userC.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);
            userC.Parameters.Add("coins", NpgsqlDbType.Integer);

            userC.Parameters["userid"].Value = user.Id;
            userC.Parameters["coins"].Value = money - 5;

            int userResult = userCommand.ExecuteNonQuery();

            if(userResult != 1)
            {
                trans.Rollback();
                PostgresRepository.releaseConnections(conID);
                throw new ResponseException("coins could not be deducted", 500);
            }


            trans.Commit();
            cards.Close();
            PostgresRepository.releaseConnections(conID);
            return package;







        }

        public Card GetCard(string cardid)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;


            //get unaquired package
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"SELECT id, name, packageid,type, element, damage FROM cards WHERE id = @cardid;";

            NpgsqlCommand c = command as NpgsqlCommand;

            c.Parameters.Add("cardid", NpgsqlDbType.Varchar, 255);
            c.Parameters["cardid"].Value = cardid;

            var result = command.ExecuteReader();

            if(result.Read())
            {
                Card card = new Card();
                card.Id = result.GetString(0);
                card.Name = result.GetString(1);
                card.packageid = result.GetInt32(2);
                card.Type = (Species)result.GetInt32(3);
                card.Element = (Element)result.GetInt32(4);
                card.Damage = result.GetInt32(5);
                result.Close();
                PostgresRepository.releaseConnections(conID);
                return card;

            }

            result.Close();
            PostgresRepository.releaseConnections(conID);
            return null;
        }

        public List<Card> GetCards(User user)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;


            //get unaquired package
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"SELECT cards.id, name, packageid, type, element, damage FROM cards JOIN stacks on stacks.card_id = cards.id WHERE stacks.user_id = @userid;";

            NpgsqlCommand c = command as NpgsqlCommand;

            c.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);
            c.Parameters["userid"].Value = user.Id;

            var result = command.ExecuteReader();

            List<Card> cardList = new List<Card>();

            while(result.Read())
            {
                Card card = new Card();
                card.Id = result.GetString(0);
                card.Name = result.GetString(1);
                card.packageid = result.GetInt32(2);
                card.Type = (Species)result.GetInt32(3);
                card.Element = (Element)result.GetInt32(4);
                card.Damage = result.GetInt32(5);
                cardList.Add(card);

            }

            result.Close();
            PostgresRepository.releaseConnections(conID);
            return cardList;
        }

        public List<Card> GetDeck(User user)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;


            //get unaquired package
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"SELECT cards.id, name, packageid, type, element, damage FROM cards JOIN stacks on stacks.card_id = cards.id WHERE stacks.user_id = @userid and stacks.in_deck = 1;";

            NpgsqlCommand c = command as NpgsqlCommand;

            c.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);
            c.Parameters["userid"].Value = user.Id;
            
            var result = command.ExecuteReader();

            List<Card> cardList = new List<Card>();

            while (result.Read())
            {
                Card card = new Card();
                card.Id = result.GetString(0);
                card.Name = result.GetString(1);
                card.packageid = result.GetInt32(2);
                card.Type = (Species)result.GetInt32(3);
                card.Element = (Element)result.GetInt32(4);
                card.Damage = result.GetInt32(5);
                cardList.Add(card);

            }

            result.Close();
            PostgresRepository.releaseConnections(conID);
            return cardList;
        }

        public void SetDeck(User user,string[] cardis)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
            IDbTransaction trans = con.BeginTransaction();

            //reset current deck
            IDbCommand resetCommand = con.CreateCommand();
            resetCommand.CommandText = @"UPDATE stacks SET in_deck = 0 WHERE user_id = @userid;";
            NpgsqlCommand r = resetCommand as NpgsqlCommand;

           
            r.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);
            r.Parameters["userid"].Value = user.Id;

            resetCommand.ExecuteNonQuery();


            IDbCommand command = con.CreateCommand();
            command.CommandText = @"UPDATE stacks SET in_deck = 1 WHERE card_id = @cardid and user_id = @userid;";

            NpgsqlCommand c = command as NpgsqlCommand;

            c.Parameters.Add("cardid", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);

            c.Parameters["userid"].Value = user.Id;
            int result;
            foreach(string card in cardis)
            {
                c.Parameters["cardid"].Value = card;
                result = command.ExecuteNonQuery();
                if(result == 0)
                {
                    trans.Rollback();
                    PostgresRepository.releaseConnections(conID);
                    throw new ResponseException("At least one of the provided cards does not belong to the user or is not available.", 403);
                }
            }
            trans.Commit();
            PostgresRepository.releaseConnections(conID);



        }

        public List<ScoreEntry> ScoreBoard()
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;
    
            IDbCommand command = con.CreateCommand();
            command.CommandText = @"SELECT id, name, profile_text, picture, elo FROM users ORDER BY ELO desc";

            IDataReader result = command.ExecuteReader();

            List<ScoreEntry> scores = new List<ScoreEntry>();
            int rank = 1;
            int lastELO = 0;
            while(result.Read())
            {
                ScoreEntry se = new ScoreEntry();
                se.Id = result.GetString(0);
                se.Name = result.GetString(1);
                se.Profile = result.GetString(2);
                se.Picture = result.GetString(3);
                se.ELO = result.GetInt32(4);

                if(se.ELO == lastELO)
                {
                    rank--;
                }
                se.Rank = rank;
                lastELO = result.GetInt32(4);
                rank++;

                scores.Add(se);
            }

            result.Close();
            PostgresRepository.releaseConnections(conID);

            return scores;
        }

        public string AddContestant(User user)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;

            IDbCommand command = con.CreateCommand();
            command.CommandText = @"INSERT INTO battlelobby (id, user_id) VALUES (@id, @userid)";
            NpgsqlCommand c = command as NpgsqlCommand;

            Guid guid = new Guid();
            string id = guid.ToString();

            c.Parameters.Add("id", NpgsqlDbType.Varchar, 255);
            c.Parameters.Add("userid", NpgsqlDbType.Varchar, 255);
            c.Parameters["id"].Value = id;
            c.Parameters["userid"].Value = user.Id;
            int result = command.ExecuteNonQuery();

            if(result == 0)
            {
                PostgresRepository.releaseConnections(conID);
                throw new ResponseException("Could not enter Lobby", 444);
            }

            PostgresRepository.releaseConnections(conID);
            return id;
        }

        public int GetContestants()
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;

            IDbCommand command = con.CreateCommand();
            command.CommandText = "SELECT COUNT(*) AS fighters FROM battlelobby";

            IDataReader result = command.ExecuteReader();

            int fighters = 0;
            if(result.Read())
            {
                fighters = result.GetInt32(0);
            }
            result.Close();
            PostgresRepository.releaseConnections(conID);
            return fighters;

        }

        public Lobby GetOldestContestant()
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;

            IDbCommand command = con.CreateCommand();
            command.CommandText = "SELECT id, user_id  FROM battlelobby Order By entry desc Limit 1";

            IDataReader result = command.ExecuteReader();

            Lobby lobby = new Lobby();
            lobby.Fighter = new User();
            if (result.Read())
            {
                lobby.Id = result.GetString(0);
                lobby.Fighter.Id = result.GetString(1);
            }
            else
            {
                result.Close();
                PostgresRepository.releaseConnections(conID);
                throw new ResponseException("No fighter available", 444);
            }

            result.Close();
            PostgresRepository.releaseConnections(conID);
            return lobby;
        }

        public Battle GetBattleFromLobby(string lobby)
        {
            Tuple<int, IDbConnection> pr = PostgresRepository.getConnection();

            int conID = pr.Item1;
            IDbConnection con = pr.Item2;

            IDbCommand battleCommand = con.CreateCommand();
            battleCommand.CommandText = "SELECT id, winner, loser FROM battles Where lobby_id = @lobbyid";
            NpgsqlCommand b = battleCommand as NpgsqlCommand;

            b.Parameters.Add("lobbyid", NpgsqlDbType.Varchar, 255);
            b.Parameters["lobbyid"].Value = lobby;

            IDataReader resultBattle = battleCommand.ExecuteReader();

            Battle battle;
            if(resultBattle.Read())
            {
                battle = new Battle();
                battle.Id = resultBattle.GetString(0);
                battle.Winner = GetUserFromId(resultBattle.GetString(1));
                battle.Loser = GetUserFromId(resultBattle.GetString(2));

            }
            else
            {
                resultBattle.Close();
                PostgresRepository.releaseConnections(conID);
                return null;
            }
            resultBattle.Close();

            IDbCommand logCommand = con.CreateCommand();
            logCommand.CommandText = "SELECT id, round,  winning_card_id, losing_card_id, winning_user_id, losing_user_id, winning_damage, losing_damage, win_condition FROM battles Where battle_id = @battleid Order by round asc";
            NpgsqlCommand l = logCommand as NpgsqlCommand;


            l.Parameters.Add("battleid", NpgsqlDbType.Varchar, 255);
            l.Parameters["battleid"].Value = battle.Id;

            IDataReader resultLog = logCommand.ExecuteReader();
            while(resultLog.Read())
            {
                BattleLog log = new BattleLog();
                log.Id = resultLog.GetString(0);
                log.Round = resultLog.GetInt32(1);
                log.WinningCard = GetCard(resultLog.GetString(2));
                log.LosingCard = GetCard(resultLog.GetString(3));
                log.RoundWinner = GetUserFromId(resultLog.GetString(4));
                log.RoundLoser = GetUserFromId(resultLog.GetString(5));
                log.WinningDamage = resultLog.GetInt32(6);
                log.LosingDamage = resultLog.GetInt32(7);
                log.SpecialWinCondition = resultLog.GetString(8);
                battle.Rounds.Add(log);
            }
            resultLog.Close();
            PostgresRepository.releaseConnections(conID);
            return battle;

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
