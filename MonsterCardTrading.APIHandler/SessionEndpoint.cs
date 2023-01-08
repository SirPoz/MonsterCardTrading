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
                    
                    response.ResponseCode = 201;
                    response.ResponseContent = "application/json";
                    string description = "User login successful \n " + token;
                    response.ResponseContent += "\n" + JsonSerializer.Serialize(description);
                    response.ResponseText = "OK";
                }
                else
                {
                    response.ResponseCode = 400;
                    response.ResponseContent = "application/json";
                    string description = "Could not deserialize request";
                    response.ResponseContent += "\n" + JsonSerializer.Serialize(description);
                    response.ResponseText = "FAILED";
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.ResponseCode = 400;
                response.ResponseContent = "application/json";
                string description = e.Message;
                response.ResponseContent += "\n" + JsonSerializer.Serialize(description);
                response.ResponseText = "failed to deserialize request";
            }
        }
    }
}
