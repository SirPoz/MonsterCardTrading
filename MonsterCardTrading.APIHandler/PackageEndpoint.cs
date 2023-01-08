﻿using System;
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
                var stack = JsonSerializer.Deserialize<List<Card>>(request.Content);


                if (stack == null)
                {
                    throw new Exception("Could not deserialize request");
                }
                CardHandler cardHandler = new CardHandler();
                UserHandler userHandler = new UserHandler();
                
                if(request.Headers.TryGetValue("Authorization", out string token))
                {
                    Stack package = new Stack();
                    package.Cards = stack;
                    cardHandler.createPackage(userHandler.userFromToken(token), package);
                    response.ResponseCode = 201;
                    response.ResponseContent = "application/json";
                    string description = "Package and cards successfully created";
                    response.ResponseContent += "\n" + JsonSerializer.Serialize(description);
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
                response.ResponseText = "failed to deserialize request";
            }


        }
    }
}
