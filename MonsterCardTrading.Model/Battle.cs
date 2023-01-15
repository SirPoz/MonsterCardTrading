using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class Battle
    {
        public string ?Id { get; set; }
        public User ?Winner { get; set; }
        public User ?Loser { get; set; }

        public string? Lobby { get; set; }
        public List<BattleLog> ?Rounds { get; set; }

        public bool Draw { get; set; }
    }
}
