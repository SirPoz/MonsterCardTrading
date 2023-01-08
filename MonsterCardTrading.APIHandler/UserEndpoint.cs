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
    public class UserEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch (request.Method)
            {
                case "POST":
                    CreateUser(request,response);
                    break;
                case "GET":
                    GetUsers(request,response);
                    break;
            }
        }

        private void CreateUser(HttpRequest request, HttpResponse response)
        {
            
            try
            {
                var user = JsonSerializer.Deserialize<User>(request.Content);


                Console.WriteLine(user.Username);

                if (user != null)
                {
                    UserHandler userHandler = new UserHandler();
                    userHandler.createUser(user.Username, user.Password);
                    
                    response.ResponseCode = 201;
                    response.ResponseContent = "application/json";
                    string description = "User successfully created";
                    response.ResponseContent += "\n" + JsonSerializer.Serialize(description);
                    response.ResponseText = "OK";
                    
                }
                else
                {

                    response.ResponseCode = 509;
                    response.ResponseContent = "application/json";
                    string description = "Failed to write to database";
                    response.ResponseContent += "\n" + JsonSerializer.Serialize(description);
                    response.ResponseText = "FAILED";
                }
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.ResponseCode = 509;
                response.ResponseContent = "application/json";
                string description = e.Message;
                response.ResponseContent += "\n" + JsonSerializer.Serialize(description);
                response.ResponseText = "FAILED";
            }

            
            


         
            //do something
        }

        private void GetUsers(HttpRequest request, HttpResponse response)
        {
            try
            {
                JsonSerializer.Deserialize<User>(request.Content);

                response.ResponseCode = 200;
                response.ResponseContent = "application/json";

            }
            catch (Exception)
            {
                response.ResponseCode = 400;
                response.ResponseContent = "application/json";
                response.ResponseText = "failed to deserialize request";
            }
        }
    }
}
