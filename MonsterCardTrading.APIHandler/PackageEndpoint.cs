using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FirstHttpServer;
using MonsterCardTrading.HttpServer;
using MonsterCardTrading.Model;

namespace MonsterCardTrading.APIHandler
{
    public class PackageEndpoint : IHttpEndpoint
    {
        public void HandleRequest(HttpRequest request, HttpResponse response)
        {
            switch (request.Method)
            {
                case "POST":
                    CreatePackage(request,response);
                    break;
            }
        }

        private void CreatePackage(HttpRequest request, HttpResponse response)
        {
            try
            {
                JsonSerializer.Deserialize<Card>(request.Content);

                response.ResponseCode = 200;
                response.ResponseContent = "application/json";
            }
            catch (Exception)
            {
                response.ResponseCode = 400;
                response.ResponseContent = "application/json";
            }
            

        }
    }
}
