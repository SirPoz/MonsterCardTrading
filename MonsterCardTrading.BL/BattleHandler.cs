using MonsterCardTrading.BL.BattleDecorator;
using MonsterCardTrading.DAL;
using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL
{
    public class BattleHandler
    {       

        private DatabaseHandler db = new DatabaseHandler();
        public Battle createBattle(User user)
        {
            Battle battle;


            int lobbySize = db.GetContestants();
            Console.WriteLine("lobbySize: " + lobbySize);
            if (lobbySize != 0)
            {
                Lobby oldestLobby = db.GetOldestContestant();
                User fighter = db.GetUserFromId(oldestLobby.Fighter.Id);

                battle =  startBattle(user, fighter, oldestLobby.Id);
                db.CloseBattle(battle);
                return battle;
            }

            string lobby = db.AddContestant(user);
            return checkForBattle(lobby);
            

            

           
            
        }

        private Battle checkForBattle(string lobby)
        {
            Battle battle = null;
            while(true)
            {
                battle = db.GetBattleFromLobby(lobby);
                if (battle != null)
                {
                    return battle;
                }
                System.Threading.Thread.Sleep(5000);
            }
            
        }

        private Battle startBattle(User newUser, User lobbyUser, string lobbyid)
        {
            Console.WriteLine("Battle started");
            Guid guid = Guid.NewGuid();
            Battle battle = new Battle();
            battle.Id = guid.ToString();
            battle.Lobby = lobbyid;
            battle.Rounds = new List<BattleLog>();
            battle.Draw = false;

            List<Card> newDeck = db.GetDeck(newUser);
            List<Card> lobbyDeck = db.GetDeck(lobbyUser);

            BattleDecoratorFactory referee = new BattleDecoratorFactory();

            int round = 1;
            BattleLog log;
            
            Random rnd = new Random();
            Random tiebreaker = new Random();
            string msg;
            while(newDeck.Count > 0 && lobbyDeck.Count > 0 && round < 100)
            {
                Console.WriteLine("Round " + round + " started");
                
                
                //prepare log
                log = new BattleLog();
                Guid Logguid = Guid.NewGuid();
                log.Id = Logguid.ToString();
                log.Round = round;
                log.SpecialWinCondition = new List<string>();
                log.Draw = false;
                round++;

                int newCard = rnd.Next(0, newDeck.Count);
                int lobbyCard = rnd.Next(0,lobbyDeck.Count);
                Console.WriteLine(newCard + " - " + lobbyCard);

                DamageCalculator newDmg = new DamageCalculator(newDeck[newCard]);
                DamageCalculator lobbyDmg = new DamageCalculator(lobbyDeck[lobbyCard]);

                newDmg = referee.GetDecorator().fight(newDeck[newCard], lobbyDeck[lobbyCard], newDmg);
                lobbyDmg = referee.GetDecorator().fight(lobbyDeck[lobbyCard], newDeck[newCard], lobbyDmg);

                
               

                //both have the same values
                if (newDmg.Damage == lobbyDmg.Damage && newDmg.Kill == lobbyDmg.Kill)
                {
                    log.WinningCard = lobbyDeck[lobbyCard];
                    log.RoundWinner = lobbyUser;
                    log.WinningDamage = (int)lobbyDmg.Damage;

                    log.LosingCard = newDeck[newCard];
                    log.RoundLoser = newUser;
                    log.LosingDamage = (int)newDmg.Damage;

                    log.Draw = true;

                    log.SpecialWinCondition.Clear();
                    log.SpecialWinCondition.Add("Draw");
                    log.SpecialWinCondition.Add("No Cards were exchanged");
                }


                //lobby player kills instantly or has more damage and the new player does not have an instant kill
                else if (lobbyDmg.Kill)
                {
                    Console.WriteLine("Lobby kills");
                    log.WinningCard = lobbyDeck[lobbyCard];
                    log.RoundWinner = lobbyUser;
                    log.WinningDamage = (int)lobbyDmg.Damage;

                    log.LosingCard = newDeck[newCard];
                    log.RoundLoser = newUser;
                    log.LosingDamage = (int)newDmg.Damage;

                    lobbyDeck.Add(newDeck[newCard]);
                    newDeck.Remove(newDeck[newCard]);
                    
                    if (round > 20 && newDeck.Count > 0)
                    {
                        newCard = rnd.Next(0, newDeck.Count);
                        lobbyDeck.Add(newDeck[newCard]);
                        newDeck.Remove(newDeck[newCard]);
                        log.SpecialWinCondition.Add("More than 20 rounds, 2 cards are exchanged each round");
                    }
                }
                //new player kills instantly or has more damage and the lobby player does not have an instant kill
                else if (newDmg.Kill)
                {
                    Console.WriteLine("New kills");
                    log.WinningCard = newDeck[newCard];
                    log.RoundWinner = newUser;
                    log.WinningDamage = (int)newDmg.Damage;

                    log.LosingCard = lobbyDeck[lobbyCard];
                    log.RoundLoser = lobbyUser;
                    log.LosingDamage = (int)lobbyDmg.Damage;

                    newDeck.Add(lobbyDeck[lobbyCard]);
                    lobbyDeck.Remove(lobbyDeck[lobbyCard]);
                   
                    if (round > 20 && lobbyDeck.Count > 0)
                    {
                            lobbyCard = rnd.Next(0, lobbyDeck.Count);
                            newDeck.Add(lobbyDeck[lobbyCard]);
                            lobbyDeck.Remove(lobbyDeck[lobbyCard]);
                            log.SpecialWinCondition.Add("More than 20 rounds, 2 cards are exchanged each round");

                    }
                }
                else if(lobbyDmg.Damage > newDmg.Damage)
                {
                    Console.WriteLine("Lobby wins");
                    
                 
                    log.WinningCard = lobbyDeck[lobbyCard];
                    log.RoundWinner = lobbyUser;
                    log.WinningDamage = (int)lobbyDmg.Damage;

                    log.LosingCard = newDeck[newCard];
                    log.RoundLoser = newUser;
                    log.LosingDamage = (int)newDmg.Damage;

                    lobbyDeck.Add(newDeck[newCard]);
                    newDeck.Remove(newDeck[newCard]);

                    if (round > 20 && newDeck.Count > 0)
                    {
                        newCard = rnd.Next(0, newDeck.Count);
                        lobbyDeck.Add(newDeck[newCard]);
                        newDeck.Remove(newDeck[newCard]);
                        log.SpecialWinCondition.Add("More than 20 rounds, 2 cards are exchanged each round");
                    }
                }
                else if(newDmg.Damage > lobbyDmg.Damage)
                {
                    Console.WriteLine("New wins");
                    
                   
                    log.WinningCard = newDeck[newCard];
                    log.RoundWinner =  newUser;
                    log.WinningDamage =  (int)newDmg.Damage;

                    log.LosingCard = lobbyDeck[lobbyCard];
                    log.RoundLoser = lobbyUser;
                    log.LosingDamage = (int)lobbyDmg.Damage;

                    newDeck.Add(lobbyDeck[lobbyCard]);
                    lobbyDeck.Remove(lobbyDeck[lobbyCard]);
                    if (round > 20 && lobbyDeck.Count > 0)
                    {
                        lobbyCard = rnd.Next(0, lobbyDeck.Count);
                        newDeck.Add(lobbyDeck[lobbyCard]);
                        lobbyDeck.Remove(lobbyDeck[lobbyCard]);
                        log.SpecialWinCondition.Add("More than 20 rounds, 2 cards are exchanged each round");
                    }
                }
                
                Console.WriteLine(log.WinningCard.Name + " won against " + log.LosingCard.Name);

                foreach(string lobWin in lobbyDmg.SpecialWin)
                { 
                    log.SpecialWinCondition.Add(lobWin);
                }

                foreach (string newWin in newDmg.SpecialWin)
                { 
                    log.SpecialWinCondition.Add(newWin);
                }

                // log.SpecialWinCondition.Add(lobbyDmg.SpecialWin);



        Console.WriteLine(log.SpecialWinCondition);
                Console.WriteLine("---------------------------------------------");

                battle.Rounds.Add(log);
               
            }

            if(lobbyDeck.Count == 0)
            {
                battle.Winner = newUser;
                battle.Loser = lobbyUser;
            }
            else if (newDeck.Count == 0)
            {
                battle.Winner = lobbyUser;
                battle.Loser = newUser;
            }
            else
            {
                battle.Winner = newUser;
                battle.Loser = lobbyUser;
                battle.Draw = true;
            }

            return battle;

        }

       
    }
}
