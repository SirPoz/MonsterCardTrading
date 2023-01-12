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
            int lobbySize = db.GetContestants();
            Console.WriteLine("lobbySize: " + lobbySize);
            if (lobbySize != 0)
            {
                Lobby oldestLobby = db.GetOldestContestant();
                User fighter = db.GetUserFromId(oldestLobby.Fighter.Id);

                return startBattle(user, fighter, oldestLobby.Id);
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
            Guid guid = new Guid();
            Battle battle = new Battle();
            battle.Id = guid.ToString();
            battle.Lobby = lobbyid;
            battle.Rounds = new List<BattleLog>();
            List<Card> newDeck = db.GetDeck(newUser);
            List<Card> lobbyDeck = db.GetDeck(lobbyUser);

            BattleDecoratorFactory referee = new BattleDecoratorFactory();

            int round = 1;
            BattleLog log;
            Random rnd = new Random();
            Random tiebreaker = new Random();
            string msg;
            while(newDeck.Count > 0 && lobbyDeck.Count > 0)
            {
                Console.WriteLine("Round " + round + " started");
                msg = "";
                //prepare log
                log = new BattleLog();
                Guid Logguid = new Guid();
                log.Id = Logguid.ToString();
                log.Round = round;
                round++;

                int newCard = rnd.Next(0, newDeck.Count);
                int lobbyCard = rnd.Next(0,lobbyDeck.Count);

                DamageCalculator newDmg = new DamageCalculator(newDeck[newCard]);
                DamageCalculator lobbyDmg = new DamageCalculator(lobbyDeck[lobbyCard]);

                newDmg = referee.GetDecorator().fight(newDeck[newCard], lobbyDeck[lobbyCard], newDmg);
                lobbyDmg = referee.GetDecorator().fight(lobbyDeck[lobbyCard], newDeck[newCard], lobbyDmg);

                

                if(lobbyDmg.Kill && newDmg.Kill)
                {
                    Console.WriteLine("Both Kill");
                   
                    int tie = tiebreaker.Next(0, 2);
                    if(tie == 1)
                    {
                        lobbyDmg.Kill = false;
                        msg = "Won by tiebreaker. ";
                     }
                     else
                     {
                        newDmg.Kill = false;
                        msg = "Won by tiebreaker. ";
                     }
                  
                    
                }

                if (newDmg.Damage == lobbyDmg.Damage && newDmg.Kill == lobbyDmg.Kill)
                {
                    Console.WriteLine("Both Equal");
                    
                    int tie = tiebreaker.Next(0, 2);
                    if (tie == 1)
                    {
                        lobbyDmg.Kill = false;
                        msg = "Won by tiebreaker. " ;
                    }
                    else
                    {
                        newDmg.Kill = false;
                        msg = "Won by tiebreaker. ";
                    }
                    
                   
                }



                if (lobbyDmg.Kill || (lobbyDmg.Damage > newDmg.Damage && !newDmg.Kill))
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
                        msg = "More than 20 rounds, 2 cards are exchanged each round. ";
                    }
                }
                if (newDmg.Kill || (newDmg.Damage > lobbyDmg.Damage && !lobbyDmg.Kill))
                {
                    Console.WriteLine("New wins");
                    log.WinningCard = lobbyDeck[lobbyCard];
                    log.RoundWinner = lobbyUser;
                    log.WinningDamage = (int)lobbyDmg.Damage;

                    log.LosingCard = newDeck[newCard];
                    log.RoundLoser = newUser;
                    log.LosingDamage = (int)newDmg.Damage;

                    newDeck.Add(lobbyDeck[lobbyCard]);
                    lobbyDeck.Remove(lobbyDeck[lobbyCard]);
                    if (round > 20 && lobbyDeck.Count > 0)
                    {
                            lobbyCard = rnd.Next(0, lobbyDeck.Count);
                            newDeck.Add(lobbyDeck[lobbyCard]);
                            lobbyDeck.Remove(lobbyDeck[lobbyCard]);
                            msg = "More than 20 rounds, 2 cards are exchanged each round. ";

                    }
                }




                log.SpecialWinCondition = msg;

                log.SpecialWinCondition += (lobbyDmg.SpecialWin == newDmg.SpecialWin)? lobbyDmg.SpecialWin : lobbyDmg.SpecialWin + " " + newDmg.SpecialWin;

                Console.WriteLine(log.SpecialWinCondition);
                Console.WriteLine("---------------------------------------------");

                battle.Rounds.Add(log);
               
            }

            if(lobbyDeck.Count == 0)
            {
                battle.Winner = newUser;
                battle.Loser = lobbyUser;
            }
            else
            {
                battle.Winner = lobbyUser;
                battle.Loser = newUser;
            }

            return battle;

        }

       
    }
}
