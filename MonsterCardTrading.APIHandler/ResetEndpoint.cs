using FirstHttpServer;
using MonsterCardTrading.BL;
using MonsterCardTrading.HttpServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterCardTrading.APIHandler
{
    public class ResetEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch (request.Method)
            {
                case "POST":
                    GameHandler gameHandler = new GameHandler();
                    gameHandler.resetDatabase();
                    response.ResponseCode = 201;
                    response.ResponseContent = "application/json";
                    string description = "Database reset";
                    response.ResponseContent += "\n" + JsonSerializer.Serialize(description);
                    response.ResponseText = "OK";
                    break;
            }
        }
    }
}
