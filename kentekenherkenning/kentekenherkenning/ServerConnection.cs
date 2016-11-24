using System;
using System.Text;
using ZeroMQ;

namespace kentekenherkenning
{
    public class ServerConnection
    {
        private ZmqContext context;
        private ZmqSocket socket;
        public string Ip { get; set; }
        public int Port { get; set; }
        public ServerConnection()
        {
            Ip = "127.0.0.1";
            Port = 9023;

        }
        public void SetConnection()
        {
            context = ZmqContext.Create();
            socket = context.CreateSocket(ZeroMQ.SocketType.REQ);

            socket.Connect($"tcp://{Ip}:{Port}");

            //test connection
//            GetMessage();


        }
        //test functie moet nog beter
        public string IsConnected()
        {
            string input = "test";

            socket.Send(input, Encoding.UTF8);
            string response = socket.Receive(Encoding.UTF8);
            Console.WriteLine(response);
            return response;
        }

        public void GetMessage()
        {
          ZmqMessage b64 =  socket.ReceiveMessage();
            Console.WriteLine(b64);
        }
    }
}