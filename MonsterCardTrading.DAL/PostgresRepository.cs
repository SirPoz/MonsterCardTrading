using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Npgsql;
using Npgsql.Replication.PgOutput.Messages;
using NpgsqlTypes;

namespace MonsterCardTrading.DAL
{
    internal class PostgresRepository
    {
        private IDbConnection con;

        public PostgresRepository()
        {
            con = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=simpledatastore");
        }

        public void readDB(string command)
        {

        }

        public void writeDB(string command)
        {

        }


        
    }
}
