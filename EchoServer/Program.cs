using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Echo Server");

            TcpListener listener = new TcpListener(IPAddress.Any, 7);
            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Task.Run(() => HandleClient(socket));
            }
            listener.Stop();

            Console.ReadLine();
        }

        public static void HandleClient(TcpClient socket)
        {
            Console.WriteLine(socket.Client.RemoteEndPoint.ToString());

            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            string message = null;
            while (message == null || !message.Equals("end", StringComparison.OrdinalIgnoreCase))
            {
                message = reader.ReadLine();
                Console.WriteLine("Client sent: " + message);

                writer.WriteLine(message);
                writer.Flush();
            }
            socket.Close();
        }
    }
}
