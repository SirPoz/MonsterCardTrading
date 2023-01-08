// See https://aka.ms/new-console-template for more information

using FirstHttpServer;
using MonsterCardTrading.APIHandler;
using MonsterCardTrading.DAL;

Console.WriteLine("MonsterCardTrading start!");

var dbhandler = new DatabaseHandler();


PostgresRepository.setup(5);


var server = new HttpServer();
server.RegisterEndpoint("/users", new UserEndpoint());
server.RegisterEndpoint("/sessions",new SessionEndpoint());
server.RegisterEndpoint("/packages",new PackageEndpoint());
server.RegisterEndpoint("/reset", new ResetEndpoint());
new HttpServer().run();

/******** TO DO  ********/
//
//
//      rewrite Database handler to use correct connection type (IDatabaseconnection)... DONE
//      create a static postgres repository pool class DONE
//      create a static battle pool class
//      create a static trade pool class
//      
//      implement all api calls till battles 
//
//
//
//
//
//
//