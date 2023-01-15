using FirstHttpServer;
using MonsterCardTrading.APIHandler;
using MonsterCardTrading.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Test
{
    public class EndpointTest
    {
        [SetUp]
        public void Setup()
        {
           
            
        }

        [Test]
        public void EndpointAuthorization()
        {
            string RequestString = "POST /transaction/package HTTP/1.1\r\nHost: localhost: 10001\r\nUser - Agent: curl / 7.83.1\r\nAccept: */*\r\nContent-Type: application/json\r\nContent-Length: 44";


            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(RequestString);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader reader = new StreamReader(stream);
            HttpRequest request = new HttpRequest(reader);
            request.Parse();

           

            Stream testStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(testStream);
            HttpResponse response = new HttpResponse(writer);

            

            TransactionEndpoint tran = new TransactionEndpoint();

            tran.HandleRequest(request, response);

            Assert.That(response.ResponseCode, Is.EqualTo(401));
            Assert.That(response.ResponseContent, Is.EqualTo("{\"message\":\"UnauthorizedError\"}"));




        }

        [Test]
        public void TestEndpointPath()
        {
            string RequestString = "PUT /cards HTTP/1.1\r\nHost: localhost: 10001\r\nUser - Agent: curl / 7.83.1\r\nAccept: */*\r\nContent-Type: application/json\r\nContent-Length: 44";


            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(RequestString);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader reader = new StreamReader(stream);
            HttpRequest request = new HttpRequest(reader);
            request.Parse();



            Stream testStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(testStream);
            HttpResponse response = new HttpResponse(writer);



            TransactionEndpoint tran = new TransactionEndpoint();

            tran.HandleRequest(request, response);

            Assert.That(response.ResponseCode, Is.EqualTo(404));
            Assert.That(response.ResponseContent, Is.EqualTo("{\"message\":\"No PUT for /cards\"}"));


        }

    }
}
