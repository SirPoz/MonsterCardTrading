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
        NpgsqlConnection con;

        public PostgresRepository()
        {
            con = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=swe1user");
            
        }

        public NpgsqlDataReader readDB(string command, List<NpgsqlParameter> parameters)
        {
            con.Open();
            var readCommand = new NpgsqlCommand(command, con);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    readCommand.Parameters.Add(parameter);
                }
            }

            NpgsqlDataReader result = readCommand.ExecuteReader();

            

            con.Close();

            return result;
        }

        public void writeDB(string command, List<NpgsqlParameter> parameters)
        {
            con.Open();
            var writeCommand = new NpgsqlCommand(command, con);
            
            if(parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    writeCommand.Parameters.Add(parameter);
                }
            }
           

            writeCommand.ExecuteNonQuery();

            con.Close();

           
        }


        
    }
}
