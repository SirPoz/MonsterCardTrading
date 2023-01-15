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
                    GetUser(request,response);
                    break;
                case "PUT":
                    ChangeUser(request, response);
                    break;
                default:
                    response.setResponse(404, "FAILED", "{\"message\":\"No " + request.Method + " for " + request.Path + "\"}");
                    break;
            }
        }

        private void CreateUser(HttpRequest request, HttpResponse response)
        {
            
            try
            {
                var user = JsonSerializer.Deserialize<User>(request.Content);

                
                if (user != null)
                {
                    UserHandler userHandler = new UserHandler();
                    userHandler.createUser(user.Username, user.Password);
                    response.setResponse(201,"OK", "{\"message\":\"User successfully created\"}");                    
                }
                else
                {
                    throw new ResponseException("Could not deserialize request", 400); 
                }
                

            }
            catch (ResponseException e)
            {
                
                response.setResponse(e.ErrorCode, "Failed", "{\"message\":\""+e.Message+"\"}");
            }

            
            


         
            //do something
        }

        private void GetUser(HttpRequest request, HttpResponse response)
        {
            try
            {
                UserHandler userHandler = new UserHandler();
                if (request.Headers.TryGetValue("Authorization", out string token) && request.Headers.TryGetValue("PathParam", out string username))
                {
                    User user = userHandler.userFromToken(token);

                    if (user.Username == username)
                    {
                        response.setResponse(200, "OK", "{\"message\":\"Data successfully retrieved\",\"content\":[" + JsonSerializer.Serialize(user)+"]}");
                    }
                    else
                    {
                        throw new ResponseException("UnauthorizedError", 401);
                    }
                }
                else
                {
                    throw new ResponseException("UnauthorizedError", 401);
                }


            }
            catch (ResponseException e)
            {

                response.setResponse(e.ErrorCode, "Failed", "{\"message\":\"" + e.Message + "\"}");
            }
        }

        private void ChangeUser(HttpRequest request, HttpResponse response)
        {
            try
            {
                var userNew = JsonSerializer.Deserialize<User>(request.Content);

                if(userNew == null)
                {
                    throw new ResponseException("Could not deserialize request", 400);
                }
                UserHandler userHandler = new UserHandler();
                if (request.Headers.TryGetValue("Authorization", out string token) && request.Headers.TryGetValue("PathParam", out string username))
                {
                    User currentUser = userHandler.userFromToken(token);

                    if (currentUser.Username == username)
                    {
                        userHandler.updateUser(currentUser, userNew);
                        response.setResponse(200, "OK", "{\"message\":\"User sucessfully updated\"}");
                    }
                    else
                    {
                        throw new ResponseException("UnauthorizedError", 401);
                    }
                }
                else
                {
                    throw new ResponseException("UnauthorizedError", 401);
                }


            }
            catch (ResponseException e)
            {

                response.setResponse(e.ErrorCode, "Failed", "{\"message\":\"" + e.Message + "\"}");
            }
        }
    }
}
