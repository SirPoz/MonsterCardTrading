using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL
{
    public static class BattleHandler
    {
        private static List<User> battleContestants = new List<User>();
        
        public static void addContestant(User user)
        {
            battleContestants.Add(user);
            checkForBattle();
        }

        private static void checkForBattle()
        {
            
                //sort by elo
                battleContestants.OrderBy(p => p.ELO);
                while(battleContestants.Count >= 2)
                {
                    startBattle(battleContestants.ElementAt(0), battleContestants.ElementAt(1));
                    battleContestants.RemoveAt(1);
                    battleContestants.RemoveAt(0);
                }
            
        }

        private static void startBattle(User user1, User user2)
        {

        }
    }
}
