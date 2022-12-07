using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class Card
    {
        private string Id;
        private string Name;
        private float Damage;


        public Card(string name, int damage)
        {
            this.Name = name;
            this.Damage = damage;
        }
    }
}
