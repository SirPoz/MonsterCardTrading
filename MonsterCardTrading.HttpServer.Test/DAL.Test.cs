using MonsterCardTrading.DAL;
using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Test
{
    public class DALTests
    {
        [SetUp]
        public void Setup()
        { 
            PostgresRepository.Close();
            PostgresRepository.setup(3);
        }

        [Test]
        public void TestDatabaseVersion()
        {
            DatabaseHandler db = new DatabaseHandler();
            string version = db.versionDatabase();
            string[] info = version.Split(" ");




            Assert.IsNotNull(version);
            Assert.That(info[0], Is.EqualTo("PostgreSQL")); 
            Assert.That(info[1], Is.EqualTo("15.1"));



        }

        [Test]
        public void TestPostgres()
        {
            List<int> conIds = new List<int>();
            Tuple<int, IDbConnection> pr;
            for (int i = 0; i < 3; i++)
            {
                pr = PostgresRepository.getConnection();
                conIds.Add(pr.Item1);
            }

            Assert.That(PostgresRepository.getOpenConnections(), Is.EqualTo(0));

            pr = PostgresRepository.getConnection();
            conIds.Add(pr.Item1);

            Assert.That(conIds.Count, Is.EqualTo(4));

            foreach(var con in conIds)
            {
                PostgresRepository.releaseConnections(con);
            }

            Assert.That(PostgresRepository.getOpenConnections(), Is.EqualTo(4));

            
        }

        [Test]

        public void TestDatabaseTables()
        {
            SchemaHandler sh = new SchemaHandler();
            DatabaseHandler db = new DatabaseHandler();

            foreach (var schema in sh._schema)
            {
                Console.WriteLine(schema.Key);
                Assert.That(db.checkTable(schema.Key), Is.True);
            }

            Assert.That(db.checkTable("random"), Is.False);
        }

        [Test]

        public void TestGetStats()
        {
            DatabaseHandler db = new DatabaseHandler();

            User user = new User();
            user.Id = "Nicht vorhanden";
            Stats stat = db.GetStats(user);
            
            Assert.That(stat.cardSpecies, Is.Null);
            Assert.That(stat.strongestCard, Is.Null);

            user.Id = db.GetUserIdWithCards();
            if(user.Id == null)
            {
                Console.WriteLine("No Battles fought yet");
                Assert.True(user.Id == null);
            }

            stat = db.GetStats(user);

            Assert.That(stat.cardSpecies, Is.Not.Null);
            Assert.That(stat.strongestCard, Is.Not.Null);
        }

        [Test]
        public void TestBattlelogPersistence()
        {
            DatabaseHandler db = new DatabaseHandler();
            string lobby = db.GetLobbyIdFromBattle();
            if(lobby == null)
            {
                Console.WriteLine("No Battles fought yet");
                Assert.True(lobby == null);
            }
            Battle battle = db.GetBattleFromLobby(lobby);

            string winner = battle.Winner.Id;
            int winCount = 0;
            string loser = battle.Loser.Id;
            int loseCount = 0;


            foreach (var log in battle.Rounds)
            {
                if(!log.Draw)
                {
                    if(log.RoundWinner.Id == winner)
                    {
                        winCount++;
                    }
                    else
                    {
                        loseCount++;
                    }
                }
            }

            Assert.That(winCount, Is.GreaterThan(loseCount));
        }


    }
}

