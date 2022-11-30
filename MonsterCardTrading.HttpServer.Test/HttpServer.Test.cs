using System.Text;
using FirstHttpServer;

namespace MonsterCardTrading.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestRequestFirstLine()
        {
            string RequestString = "POST /users HTTP/1.1\r\nHost: localhost:10001\r\nUser-Agent: curl/7.83.1\r\nAccept: */*\r\nContent-Type: application/json\r\nContent-Length: 44";
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(RequestString);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader reader = new StreamReader(stream);
            HttpRequest request = new HttpRequest(reader);
            request.Parse();

            Assert.That(request.Method, Is.EqualTo("POST"));
            Assert.That(request.Path,Is.EqualTo("/users"));


        }
    }
}