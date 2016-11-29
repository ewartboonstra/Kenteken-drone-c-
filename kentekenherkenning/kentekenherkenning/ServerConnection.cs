using System;
using System.Drawing;
using System.IO;
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
        /// <summary>
        /// Start a server which python can connect to
        /// </summary>
        public void SetConnection()
        {
            context = ZmqContext.Create();
            socket = context.CreateSocket(SocketType.PAIR);
            socket.Connect($"tcp://{Ip}:{Port}");
        }

        /// <summary>
        /// Close server
        /// </summary>
        public void Dispose()
        {
            socket.Close();
            context.Dispose();
            Console.WriteLine("Connection closed");
        }

        /// <summary>
        /// Keep server idle until image is received
        /// </summary>
        /// <returns>Image</returns>
        public Image WaitForImage()
        {
            string message = socket.Receive(Encoding.UTF8);
            return Base64ToImage(message);
        }

        /// <summary>
        /// Convert base64 string to image
        /// </summary>
        /// <param name="base64String">base64string to convert to image</param>
        /// <returns>Image</returns>
        public Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }
    }
}