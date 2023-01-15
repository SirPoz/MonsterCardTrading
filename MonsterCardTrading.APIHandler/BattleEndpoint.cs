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
    public class BattleEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch(request.Method)
            {
                case "POST":
                    CreateBattleRequest(request, response);
                    break;
                default:
                    response.setResponse(404, "FAILED", "{\"message\":\"No " + request.Method + " for " + request.Path + "\"}");
                    break;
            }
        }

        private void CreateBattleRequest(HttpRequest request, HttpResponse response)
        {
            try
            {
                
                if (request.Headers.TryGetValue("Authorization", out string token))
                {
                    UserHandler userHandler = new UserHandler();
                    BattleHandler battleHander = new BattleHandler();
                    Battle battle = battleHander.createBattle(userHandler.userFromToken(token));
                    response.setResponse(200, "OK", "{\"message\":\"The battle has been carried out successfully.\",\"content\":[" + JsonSerializer.Serialize(battle) + "]}");
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
