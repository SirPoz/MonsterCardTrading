using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.DAL
{
    internal class SchemaHandler
    {
        public Dictionary<string, string> _schema = new Dictionary<string, string>();
        public SchemaHandler()
        {
            _schema.Add("cards", "CREATE TABLE IF NOT EXISTS cards (id varchar(255) NOT NULL, name varchar(255) NOT NULL, packageid varchar(255) NOT NULL, type integer NOT NULL,  element integer NOT NULL,   damage integer NOT NULL , PRIMARY KEY (id))");
            _schema.Add("users", "CREATE TABLE IF NOT EXISTS users (id varchar(255) NOT NULL, username varchar(255) NOT NULL,  password varchar(255) NOT NULL,  profile_text varchar(255) NOT NULL,   elo integer NOT NULL, coins integer NOT NULL, PRIMARY KEY (id))");
            _schema.Add("stacks", "CREATE TABLE IF NOT EXISTS stacks (id varchar(255) NOT NULL, user_id varchar(255) NOT NULL,  card_id varchar(255) NOT NULL,  in_deck integer NOT NULL Default '0', PRIMARY KEY (id))");
        }

        //Card: 
        //id, name, type, element, damage,  

        //User:
        //id, username, password, profile_text, elo, coins

        //Stack:
        //id, user_id, card_id, in_deck
    }
}
