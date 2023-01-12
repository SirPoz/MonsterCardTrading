using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL.BattleDecorator
{
    public abstract class BattleDecorator
    {
        private BattleDecorator next = null;
        public DamageCalculator fight(Card attacker, Card defender, DamageCalculator dmg)
        {

            dmg = specificTest(attacker, defender, dmg);
            if(next != null)
            {
                return next.fight(attacker, defender, dmg);
            }
            return dmg;
        }

        public void setNext (BattleDecorator battleDecorator)
        {
            next = battleDecorator;
        }

        protected virtual DamageCalculator specificTest(Card attacker, Card defender, DamageCalculator dmg)
        {
            return null;
        }
    }
}
