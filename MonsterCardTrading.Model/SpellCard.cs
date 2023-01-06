using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class SpellCard : Card
    {
        public SpellCard(string id, string name, int damage, int type, int element) : base(id, name, damage, type, element)
        {
        }
    }
}
