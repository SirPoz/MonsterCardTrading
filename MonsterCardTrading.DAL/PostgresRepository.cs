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
    public static class PostgresRepository
    {
        private static Dictionary<int, IDbConnection> Connections = new Dictionary<int, IDbConnection>();
        private static Dictionary<int, bool> OpenConnections = new Dictionary<int, bool>();
        public static void setup(int connections)
        {
            IDbConnection connection;
            for (int i = 0; i < connections; i++)
            {
                connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=swe1user");
                connection.Open();
                Connections.Add(i, connection);
                OpenConnections.Add(i, false);
            }
        }

        public static Tuple<int, IDbConnection> getConnection()
        {
            Console.WriteLine("Openconnections: " + getOpenConnections());
            for(int i = 0; i < Connections.Count; i++)
            {
                if(!OpenConnections[i])
                {
                    OpenConnections[i] = true;
                    return new Tuple<int, IDbConnection>(i,Connections[i]);
                }
            }
            Connections.Add(Connections.Count+1, new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=swe1user"));
            OpenConnections.Add(Connections.Count + 1, true);
            return new Tuple<int, IDbConnection>(Connections.Last().Key, Connections.Last().Value);
        }

        public static void releaseConnections(int i)
        {
           
            if (Connections[i].State != ConnectionState.Open)
            {
                Connections[i].Open();
               
            }   
            OpenConnections[i] = false;
            
        }

        private static int getOpenConnections()
        {
            int count = 0;
            foreach(var con in OpenConnections)
            {
                if(!con.Value)
                {
                    count++;
                }
            }
            return count;
        }

        /*public NpgsqlDataReader readDB(string command, List<NpgsqlParameter> parameters)
        {
            con.Open();
            IDbCommand readCommand = con.CreateCommand();
            readCommand.CommandText = command;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    readCommand.Parameters.Add(parameter);
                }
            }

            NpgsqlDataReader result = (NpgsqlDataReader)readCommand.ExecuteReader();

            

            con.Close();

            return result;
        }

        public int writeDB(string command, List<NpgsqlParameter> parameters)
        {
            con.Open();
            IDbCommand writeCommand = con.CreateCommand();
            writeCommand.CommandText = command;
            
            
            if(parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    writeCommand.Parameters.Add(parameter);
                }
            }

            Console.WriteLine(command);
            int result;
            result = writeCommand.ExecuteNonQuery();
            
            
            con.Close();

            Console.WriteLine("Database done");
            return result;
           
        }*/


        
    }
}
