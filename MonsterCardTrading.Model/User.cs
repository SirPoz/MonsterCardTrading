using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class User
    {
        public string ?Id { get; set; }
        public string ?Username { get; set; }
        public string ?Password { get; set; }

        public string ?hash { get; set; }
        public string ?Profile { get; set; }
        public int ?ELO { get; set; }
        public int ?Coins { get; set; }

    }
}
