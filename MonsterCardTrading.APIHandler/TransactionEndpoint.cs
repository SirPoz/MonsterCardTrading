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
    public class TransactionEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch (request.Method)
            {
                case "POST":
                    AquirePackage(request, response);
                    break;
            }
        }

        private void AquirePackage(HttpRequest request, HttpResponse response)
        {
            try
            {
                
                CardHandler cardHandler = new CardHandler();
                UserHandler userHandler = new UserHandler();

                if (request.Headers.TryGetValue("Authorization", out string token))
                {
                    
                    List<Card> cards = cardHandler.aquirePackage(userHandler.userFromToken(token));
                    response.ResponseCode = 201;
                    response.ResponseContent = "application/json";
                    
                    response.ResponseContent += "\n" + JsonSerializer.Serialize(cards);
                    response.ResponseText = "OK";
                }
                else
                {
                    throw new Exception("Unautherized access");
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.ResponseCode = 400;
                response.ResponseContent = "application/json";
                string description = e.Message;
                response.ResponseContent += "\n" + JsonSerializer.Serialize(description);
                response.ResponseText = "FAILED";
            }
        }
    }
}
