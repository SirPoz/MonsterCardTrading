using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FirstHttpServer;
using MonsterCardTrading.BL;
using MonsterCardTrading.HttpServer;
using MonsterCardTrading.Model;

namespace MonsterCardTrading.APIHandler
{
    public class PackageEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch (request.Method)
            {
                case "POST":
                    CreatePackage(request,response);
                    break;
            }
        }

        private void CreatePackage(HttpRequest request, HttpResponse response)
        {
            try
            {
                var stack = JsonSerializer.Deserialize<List<Card>>(request.Content);


                if (stack == null)
                {
                    throw new ResponseException("Could not deserialize request",400);
                }
                CardHandler cardHandler = new CardHandler();
                UserHandler userHandler = new UserHandler();
                
                if(request.Headers.TryGetValue("Authorization", out string token))
                {
                    Stack package = new Stack();
                    package.Cards = stack;
                    cardHandler.createPackage(userHandler.userFromToken(token), package);
                    response.setResponse(201, "OK", "{\"message\":\"Package and cards successfully created\"}");
                }
                else
                {
                    throw new ResponseException("UnauthorizedError",401);
                }
                
                
                
            }
            catch (ResponseException e)
            {
                Console.WriteLine(e.Message);
                response.setResponse(e.ErrorCode, "FAILED", "{\"message\":\""+e.Message+"\"}");
            }

        }
    }
}
