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
            for(int i = 0; i < connections; i++)
            {
                IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=swe1user");
                connection.Open();
                Connections.Add(Connections.Count, connection);
                OpenConnections.Add(Connections.Count - 1, false);
            }
               
           
        }

        public static Tuple<int, IDbConnection> getConnection()
        {
            
            foreach(var con in OpenConnections)
            {
                if(!con.Value)
                {
                    OpenConnections[con.Key] = true;
                    return new Tuple<int, IDbConnection>(con.Key, Connections[con.Key]);
                }
            }

            IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=swe1user;Password=swe1pw;Database=swe1user");
            connection.Open();
            Connections.Add(Connections.Count+1, connection);
            OpenConnections.Add(Connections.Count, true);
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

        public static int getOpenConnections()
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

        public static void Close()
        {
            
            foreach(var con in Connections)
            {
                    OpenConnections[con.Key] = true;
                    Connections[con.Key].Close();
            }

            Connections.Clear();
            OpenConnections.Clear();
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
