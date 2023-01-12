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
    public class ScoreboardEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch(request.Method)
            {
                case "GET":
                    getScoreboard(request, response);
                    break;
            }
        }

        private void getScoreboard(HttpRequest request, HttpResponse response)
        {
            try
            {
                UserHandler userHandler = new UserHandler();
                if (request.Headers.TryGetValue("Authorization", out string token))
                {

                    List<ScoreEntry> scoreboard = userHandler.scoreBoard();
                    response.setResponse(200, "OK", "{\"message\":\"The scoreboard could be retrieved successfully.\",\"content\":[" + JsonSerializer.Serialize(scoreboard) + "]}");
                    
                   
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
