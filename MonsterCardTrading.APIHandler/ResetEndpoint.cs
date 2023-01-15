using FirstHttpServer;
using MonsterCardTrading.BL;
using MonsterCardTrading.HttpServer;
using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
                    try
                    {
                        GameHandler gameHandler = new GameHandler();
                        gameHandler.resetDatabase();
                        response.setResponse(200, "OK", "{\"message\":\"Database reset\"}");
                    }
                    catch (ResponseException e)
                    {
                        response.setResponse(e.ErrorCode, "FAILED", "{\"message\":\""+e.Message+"\"}");
                    }
                    break;
                default:
                    response.setResponse(404, "FAILED", "{\"message\":\"No " + request.Method + " for " + request.Path + "\"}");
                    break;
            }
        }
    }
}
