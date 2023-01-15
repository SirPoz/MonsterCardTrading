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
    public class DeckEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch (request.Method)
            {
                case "GET":
                    GetDeck(request, response);
                    break;

                case "PUT":
                    ConfigureDeck(request, response);
                    break;

                default:
                    response.setResponse(404, "FAILED", "{\"message\":\"No " + request.Method + " for " + request.Path + "\"}");
                    break;
            }
        }

        private void GetDeck(HttpRequest request, HttpResponse response)
        {
            try
            {

                CardHandler cardHandler = new CardHandler();
                UserHandler userHandler = new UserHandler();

                if (request.Headers.TryGetValue("Authorization", out string token))
                {

                    List<Card> deck = cardHandler.getDeck(userHandler.userFromToken(token));
                    if (deck.Count == 0)
                    {
                        response.setResponse(204, "OK", "{\"message\":\"The request was fine, but the deck doesn't have any cards\"}");
                    }
                    else
                    {
                        request.Headers.TryGetValue("format", out string format);
                        if(format == "plain")
                        {
                            string cards = "";
                            foreach(Card card in deck)
                            {
                                cards += card.Name + ", ";
                            }
                            response.setResponse(200, "OK", cards);
                        }
                        else
                        {
                            response.setResponse(200, "OK", "{\"message\":\"The deck has cards\",\"content\": " + JsonSerializer.Serialize(deck) + "}");
                        }
                        
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

        private void ConfigureDeck(HttpRequest request, HttpResponse response)
        {
            try
            {
                var deck = JsonSerializer.Deserialize<string[]>(request.Content);
                if(deck == null)
                {
                    throw new ResponseException("Could not deserialize request", 400);
                }

                CardHandler cardHandler = new CardHandler();
                UserHandler userHandler = new UserHandler();


                if(deck.Length != 4)
                {
                    throw new ResponseException("The provided deck did not include the required amount of cards", 400);
                }

                if (request.Headers.TryGetValue("Authorization", out string token))
                {

                    cardHandler.setDeck(userHandler.userFromToken(token),deck);
                    response.setResponse(200, "OK", "{\"message\":\"The deck has been successfully configured\"}");

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
