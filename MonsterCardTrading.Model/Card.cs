using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class Card
    {
        private string _name;
        private int _damage;

        public int Damage { get; }
        public string Name { get;  }



        public Card(string name, int damage)
        {
            this._name = name;
            this._damage = damage;
        }
    }
}
