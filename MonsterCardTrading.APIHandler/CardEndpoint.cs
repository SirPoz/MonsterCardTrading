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
    public class CardEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch (request.Method)
            {
                case "GET":
                    GetCards(request, response);
                    break;
                default:
                    response.setResponse(404, "FAILED", "{\"message\":\"No "+ request.Method+" for " + request.Path +"\"}");
                    break;
            }
        }

        private void GetCards(HttpRequest request, HttpResponse response)
        {
            try
            {

                CardHandler cardHandler = new CardHandler();
                UserHandler userHandler = new UserHandler();

                if (request.Headers.TryGetValue("Authorization", out string token))
                {

                    List<Card> cards = cardHandler.getCards(userHandler.userFromToken(token));
                    if(cards.Count == 0)
                    {
                        response.setResponse(204, "OK", "{\"message\":\"The request was fine, but the user doesn't have any cards\"}");
                    }
                    else
                    {
                        response.setResponse(200, "OK", "{\"message\":\"The user has cards\",\"content\": [" + JsonSerializer.Serialize(cards) + "]}");
                    }
                    
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
