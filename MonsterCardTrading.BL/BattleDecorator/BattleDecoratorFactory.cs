using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL.BattleDecorator
{
    public class BattleDecoratorFactory
    {
        public BattleDecorator GetDecorator()
        {
            BattleDecorator element = new ElementFight();
            element.setNext(new MonsterFight());

            return element;
        }
    }
}
