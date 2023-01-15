using MonsterCardTrading.BL;
using MonsterCardTrading.BL.BattleDecorator;
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
    public class BLTest
    {
        [SetUp]
        
        public void Setup()
        {
            PostgresRepository.Close();
            PostgresRepository.setup(1);
            
        }

        [Test, Order(1)]
        public void TestUserCreation()
        {
            UserHandler userHandler = new UserHandler();
            

            //create User
            userHandler.createUser("TestUser", "TestPass");


            //get User
            string token = userHandler.loginUser("TestUser", "TestPass");


            User user = userHandler.userFromToken(token);

            Assert.That(user.Username, Is.EqualTo("TestUser"));
            Assert.That(user.Coins, Is.EqualTo(20));


            
        }

        [Test, Order(2)]

        public void TestUserUpdate()
        {
            UserHandler userHandler = new UserHandler();
        
            string token = userHandler.loginUser("TestUser", "TestPass");

            User user = userHandler.userFromToken(token);

            User changes = new User();
            changes.Name = "Test33";
            changes.Username = "Should not change";

            userHandler.updateUser(user, changes);

            user = userHandler.userFromToken(token);

            Assert.That(user.Username, Is.EqualTo("TestUser"));
            Assert.That(user.Name, Is.EqualTo("Test33"));

        }

        [Test, Order(30)]

        public void TestUserDelete()
        {
            UserHandler userHandler = new UserHandler();

            string token = userHandler.loginUser("TestUser", "TestPass");

            User user = userHandler.userFromToken(token);

            userHandler.deleteUser(user);

    

            Assert.Throws(typeof(ResponseException),() => userHandler.userFromToken(token));
    

        }




        [Test]

        public void TestPackageCreationException()
        {
            Stack newCard = new Stack();
            CardHandler cardHandler = new CardHandler();

            User user = new User();
            user.Username = "Test";

            newCard.Cards = new List<Card>();

            Assert.Throws(typeof(ResponseException), () => cardHandler.createPackage(user, newCard));

        }



        [Test]

        public void TestBattleElementAdvantage()
        {
            Card winner = new Card();
            Card loser = new Card();

            BattleDecoratorFactory fact = new BattleDecoratorFactory();

            winner.Element = Element.Fire;
            winner.Damage = 100;
            winner.Type = Species.Dragon;

            loser.Element = Element.Normal;
            loser.Damage = 150;
            loser.Type = Species.Spell;

            DamageCalculator win = new DamageCalculator(winner);
            DamageCalculator lose = new DamageCalculator(loser);

            win = fact.GetDecorator().fight(winner, loser, win);
            lose = fact.GetDecorator().fight(loser, winner, lose);

            Assert.That(win.Damage, Is.EqualTo(winner.Damage * 2));
            Assert.That(lose.Damage, Is.EqualTo(loser.Damage / 2));

        }

        [Test]

        public void TestBattleSpeciesAdvantage()
        {
            Card winner = new Card();
            Card loser = new Card();

            BattleDecoratorFactory fact = new BattleDecoratorFactory();

            winner.Element = Element.Fire;
            winner.Damage = 100;
            winner.Type = Species.Elf;

            loser.Element = Element.Water;
            loser.Damage = 150;
            loser.Type = Species.Dragon;

            DamageCalculator win = new DamageCalculator(winner);
            DamageCalculator lose = new DamageCalculator(loser);

            win = fact.GetDecorator().fight(winner, loser, win);
            lose = fact.GetDecorator().fight(loser, winner, lose);

            Assert.That(lose.Damage, Is.EqualTo(0));
            Assert.That(win.Damage, Is.EqualTo(winner.Damage));
            

        }

        [Test]
        public void TestInstantKill()
        {
            Card winner = new Card();
            Card loser = new Card();

            BattleDecoratorFactory fact = new BattleDecoratorFactory();

            winner.Element = Element.Water;
            winner.Damage = 10;
            winner.Type = Species.Spell;

            loser.Element = Element.Normal;
            loser.Damage = 800;
            loser.Type = Species.Knight;

            DamageCalculator win = new DamageCalculator(winner);
            DamageCalculator lose = new DamageCalculator(loser);

            win = fact.GetDecorator().fight(winner, loser, win);
            lose = fact.GetDecorator().fight(loser, winner, lose);

            Assert.That(win.Kill, Is.True);
            Assert.That(lose.Damage, Is.EqualTo(loser.Damage * 2));
        }


        [Test]
        public void TestPackageAdminPrivilege()
        {
            CardHandler card = new CardHandler();

            Stack stack = new Stack();
            User user = new User();

            user.Username = "Test";

            try
            {
                card.createPackage(user, stack);
            }
            catch (ResponseException e)
            {
                Assert.That(e.Message, Is.EqualTo("Provided user is not \"admin\""));
            }

            user.Username = "admin";

            try
            {
                card.createPackage(user, stack);
            }
            catch (ResponseException e)
            {
                Assert.That(e.Message, Is.EqualTo("Provided package has not the correct amount of cards"));
            }

        }




    }
}
