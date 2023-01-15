using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class Stats
    {
        public Tuple<string,int> strongestCard { get; set; }
        public Tuple<string, int> weakestCard { get; set; }

        public int?totalBattles { get; set; }
        public int?lostBattles { get; set; }
        public int?wonBattles { get; set; }
        public int?drawBattles { get; set; }

        public int?eloWon { get; set; }
        public int?eloLost { get; set; }

        public int?roundsWon { get; set; }
        public int?roundsLost { get; set; }
        public int?roundsDraw { get; set; } 

        public Dictionary<string, int> cardElements { get; set; }
        public Dictionary<string, int> cardSpecies { get; set; }

        public Stats()
        {
            cardElements = new Dictionary<string, int>();
            cardSpecies = new Dictionary<string, int>();
            

        }
    }
}
