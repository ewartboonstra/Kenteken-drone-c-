using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using ZeroMQ;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public LicensePlate WaitForImage()
        {
            string message = socket.Receive(Encoding.UTF8);

            JObject obj = JObject.Parse(message);

            Console.WriteLine(obj["Picture"].ToString());
            Image<Bgr, Byte> picture = new Image<Bgr, byte>((Bitmap)Base64ToImage(obj["Picture"].ToString()));
            string timestamp = obj["Timestamp"].ToString();
            string gps = obj["Gps"].ToString();
            LicensePlate plate = new LicensePlate(gps,timestamp,picture);
            Console.WriteLine("succes");
            return plate;
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
            using (MemoryStream mStream = new MemoryStream(imageBytes))
            {
                return Image.FromStream(mStream);
            }
        }
    }
}