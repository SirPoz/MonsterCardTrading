using MonsterCardTrading.DAL;
using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL
{
    public class GameHandler
    {
        private DatabaseHandler db;

        public GameHandler()
        {
            db = new DatabaseHandler();
        }

        public void resetDatabase()
        {
            try
            {
                db.resetDatabase();
            }
            catch(Exception e)
            {
                throw new ResponseException(e.Message, 500);
            }
            
        }
    }
}
