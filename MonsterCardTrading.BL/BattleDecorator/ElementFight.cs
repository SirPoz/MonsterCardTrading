using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL.BattleDecorator
{
    public class ElementFight : BattleDecorator
    {
        override protected DamageCalculator specificTest(Card attacker, Card defender, DamageCalculator dmg)
        {
            if(attacker.Type != Species.Spell && defender.Type != Species.Spell)
            {
                
                dmg.SpecialWin.Add("The element type does not effect pure monster fights.");
                return dmg;
            }
            switch(attacker.Element)
            {
                case Element.Fire:
                    if(defender.Element == Element.Water)
                    {
                        dmg.Damage /= 2;
                        dmg.SpecialWin.Add("Fire is weak against Water");
                    }
                    if (defender.Element == Element.Normal)
                    {
                        dmg.Damage *= 2;
                        dmg.SpecialWin.Add("Fire is strong against Normal");
                    }
                    return dmg;



                case Element.Water:
                    if (defender.Element == Element.Normal)
                    {
                        dmg.Damage /= 2;
                        dmg.SpecialWin.Add("Water is weak against Normal");
                    }
                    if (defender.Element == Element.Fire)
                    {
                        dmg.Damage *= 2;
                        dmg.SpecialWin.Add("Water is strong against Fire");
                    }
                    return dmg;
                    


                case Element.Normal:
                    if (defender.Element == Element.Fire)
                    {
                        dmg.Damage /= 2;
                        dmg.SpecialWin.Add("Normal is weak against Fire");
                    }
                    if (defender.Element == Element.Water)
                    {
                        dmg.Damage *= 2;
                        dmg.SpecialWin.Add("Normal is strong against Water");
                    }
                    return dmg;
            }

            return dmg;
        }
    }
}
