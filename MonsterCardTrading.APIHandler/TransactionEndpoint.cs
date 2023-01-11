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
                    response.setResponse(200, "OK", "{\"message\":\"A package has been successfully bought\",\"content\": \""+ JsonSerializer.Serialize(cards) + "\"}");
                }
                else
                {
                    throw new ResponseException("UnauthorizedError", 401);
                }



            }
            catch (ResponseException e)
            {
                Console.WriteLine(e.Message);
                response.setResponse(401, "FAILED", "{\"message\":\"" + e.Message + "\"}");
            }
        }
    }
}
