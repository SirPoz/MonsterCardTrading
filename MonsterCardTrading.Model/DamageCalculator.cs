using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class DamageCalculator
    {
        public float ?Damage { get; set; }
        public List<string> SpecialWin { get; set; }
        public bool Kill { get; set; }

        public DamageCalculator(Card card)
        {
            if(card.Damage != null)
            {
                Damage = card.Damage;
            }
            else
            {
                Damage = 0;
            }
            
            SpecialWin = new List<string>();
            Kill = false;
        }
    }
}
