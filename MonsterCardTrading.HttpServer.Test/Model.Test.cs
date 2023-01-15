using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Test
{
    public class ModelTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDamageCalculator()
        {
            Card one = new Card();
            Card two = new Card();

            one.Damage = 100;
            two.Damage = null;

            DamageCalculator dmg = new DamageCalculator(one);
            DamageCalculator dmg2 = new DamageCalculator(two);

            Assert.That(dmg.Damage, Is.EqualTo(one.Damage));
            Assert.That(dmg2.Damage, Is.EqualTo(0));
        }

      
    }
}
