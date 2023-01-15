using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using NUnit;
using FirstHttpServer;
using MonsterCardTrading.APIHandler;
using MonsterCardTrading.HttpServer;
using MonsterCardTrading.Model;
using NUnit.Framework.Internal.Execution;

namespace MonsterCardTrading.Test
{
    public class HTTPServerTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void TestRequestFirstLine()
        {
            string RequestString = "POST /users HTTP/1.1";
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(RequestString);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader reader = new StreamReader(stream);
            HttpRequest request = new HttpRequest(reader);
            request.Parse();


            Assert.That(request.Method, Is.EqualTo("POST"));
            Assert.That(request.Path, Is.EqualTo("/users"));

        }
        [Test]

        public void TestRequestHeaders()
        {
            string RequestString = "POST /users?test=done HTTP/1.1\r\nHost: localhost: 10001\r\nUser - Agent: curl / 7.83.1\r\nAccept: */*\r\nContent-Type: application/json\r\nContent-Length: 44\r\nAuthorization: Bearer kienboec-mtcgToken";


            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(RequestString);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader reader = new StreamReader(stream);
            HttpRequest request = new HttpRequest(reader);
            request.Parse();

            request.Headers.TryGetValue("Content-Length", out string length);
            request.Headers.TryGetValue("Authorization", out string auth);
            request.Headers.TryGetValue("Content-Type", out string type);
            request.Headers.TryGetValue("test", out string test);

            Assert.That(length, Is.EqualTo(" 44"));
            Assert.That(auth,Is.EqualTo(" Bearer kienboec-mtcgToken"));
            Assert.That(type, Is.EqualTo(" application/json"));
            Assert.That(test, Is.EqualTo("done"));

        }

        [Test]
        public void TestRegisterEndpoint()
        {
            //setup
            FirstHttpServer.HttpServer serv = new FirstHttpServer.HttpServer();
            serv.RegisterEndpoint("/test", new DeckEndpoint());

            IHttpEndpoint test = serv.getEndpoint("/test");

            
            Assert.That(test, Is.InstanceOf<DeckEndpoint>());
        }

        [Test]

        public void TestResponse()
        {
            Stream testStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(testStream);
            HttpResponse response = new HttpResponse(writer);

            response.setResponse(202, "OK", "{\"message\":\"test\"}");
           

            response.Process();



            //Assert.That(response.headers.Count, Is.EqualTo(2));
            response.headers.TryGetValue("Content-Length", out string length);
            response.headers.TryGetValue("Content-Type", out string type);

            Assert.That(response.headers.Count, Is.EqualTo(2));
            Assert.That(length, Is.EqualTo("18"));
            Assert.That(type, Is.EqualTo("application/json"));

        }


    }
}