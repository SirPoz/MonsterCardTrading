using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL.BattleDecorator
{
    public class MonsterFight : BattleDecorator
    {
        override protected DamageCalculator specificTest(Card attacker, Card defender, DamageCalculator dmg)
        {
            if(attacker.Type == Species.Goblin && defender.Type == Species.Dragon)
            {
                dmg.Damage = 0;
                dmg.SpecialWin = "Goblins are too afraid of Dragons to attack.";
                return dmg;
            }
            if (attacker.Type == Species.Ork && defender.Type == Species.Wizzard)
            {
                dmg.Damage = 0;
                dmg.SpecialWin = "Wizzard can control Orks so they are not able to damage them.";
                return dmg;
            }
            if (attacker.Type == Species.Spell && attacker.Element == Element.Water && defender.Type == Species.Knight)
            {
                dmg.Kill = true;
                dmg.SpecialWin = "The armor of Knights is so heavy that WaterSpells make them drown them instantly.";
                return dmg;
            }
            if (attacker.Type == Species.Spell && defender.Type == Species.Kraken)
            {
                dmg.Damage = 0;
                dmg.SpecialWin = "The Kraken is immune against spells.";
                return dmg;
            }
            if (attacker.Type == Species.Dragon && defender.Element == Element.Fire && defender.Type == Species.Elf)
            {
                dmg.Damage = 0;
                dmg.SpecialWin = "The FireElves know Dragons since they were little and can evade their attacks.";
                return dmg;
            }

            return dmg;

        }
    }
}
