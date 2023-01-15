using FirstHttpServer;
using MonsterCardTrading.BL;
using MonsterCardTrading.HttpServer;
using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterCardTrading.APIHandler
{
    public class StatEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch(request.Method)
            {
                case "GET":
                    getStats(request, response);
                    break;
                default:
                    response.setResponse(404, "FAILED", "{\"message\":\"No " + request.Method + " for " + request.Path + "\"}");
                    break;
            }
            
           
        }



        private void getStats(HttpRequest request, HttpResponse response)
        {
            try
            {

                UserHandler userHandler = new UserHandler();

                if (request.Headers.TryGetValue("Authorization", out string token))
                {

                        Stats portfolio = userHandler.getStats(userHandler.userFromToken(token));       
                        
                        response.setResponse(200, "OK", "{\"message\":\"Stats could be loaded\",\"content\": " + JsonSerializer.Serialize(portfolio) + "}");

                }
                else
                {
                    throw new ResponseException("UnauthorizedError", 401);
                }



            }
            catch (ResponseException e)
            {
                Console.WriteLine(e.Message);
                response.setResponse(e.ErrorCode, "FAILED", "{\"message\":\"" + e.Message + "\"}");
            }
        }
    }
}
