using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class BattleLog
    {
        public string ?Id { get; set; }
        public int ?Round { get; set; }
        public Card ?WinningCard { get; set; }
        public Card ?LosingCard { get; set; }

        public int ?WinningDamage { get; set; }
        public int ?LosingDamage { get; set; }

        public User ?RoundWinner { get; set; }
        public User ?RoundLoser { get; set; }

        public string ?SpecialWinCondition { get; set; }

    }
}
