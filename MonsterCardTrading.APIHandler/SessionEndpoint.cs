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
    public class SessionEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch (request.Method)
            {
                case "POST":
                    CreateSession(request, response);
                    break;
                default:
                    response.setResponse(404, "FAILED", "{\"message\":\"No " + request.Method + " for " + request.Path + "\"}");
                    break;
            }
        }

        private void CreateSession(HttpRequest request, HttpResponse response)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(request.Content);

                if (user != null)
                {
                    UserHandler userHandler = new UserHandler();
                    string token = userHandler.loginUser(user.Username, user.Password);
                    response.setResponse(200, "OK", "{\"message\":\"User login successful\",\"token\":\"" + token + "\"}");
                }
                else
                {
                    throw new ResponseException("Could not deserialize request", 400);
                }


            }
            catch (ResponseException e)
            {
                Console.WriteLine(e.Message);
                response.setResponse(e.ErrorCode, "FAILED", "{\"message\":\""+ e.Message + "\"}");
            }
        }
    }
}
