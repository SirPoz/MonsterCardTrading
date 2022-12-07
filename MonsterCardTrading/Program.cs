// See https://aka.ms/new-console-template for more information

using FirstHttpServer;
using MonsterCardTrading.APIHandler;

Console.WriteLine("MonsterCardTrading start!");

var server = new HttpServer();
server.RegisterEndpoint("/users",new UserEndpoint());
server.RegisterEndpoint("/sessions",new SessionEndpoint());
server.RegisterEndpoint("/packages",new PackageEndpoint());
new HttpServer().run();