using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class Card
    {
        public string ?Id { set; get; }
        public string ?Name { set; get; }
        public float ?Damage { set; get; }

        public int ?packageid { set; get; }
        public Element ?Element { set; get; }

        public Type ?Type { set; get; }

    }

    public enum Element
    {
        Fire,
        Water,
        Normal

    };

    public enum Type
    {
        WaterSpell,
        FireSpell,
        NormalSpell,
        Goblin,
        Dragon,
        Wizzard,
        Ork,
        Knight,
        Kraken,
        FireElf
    };
}
