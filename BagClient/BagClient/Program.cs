using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace ConsoleApplication7Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress address = ipHost.AddressList[0];

            // IPAddress address = IPAddress.Parse("172.27.216.225");//адрес сервера
            Socket socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


            EndPoint ep = new IPEndPoint(address, 2016);

            socket.Connect(ep);

            while (true)
            {

                string sText = Console.ReadLine();

                //byte[] bytes = Encoding.GetEncoding(1251).GetBytes(sText);

                Packet.Packet packet = new Packet.Packet();
                packet.packetType = Packet.PacketType.Message;
                packet.sText1 = "клиент 1";
                packet.sText2 = sText;
                byte[] bytes = packet.ToBytes();

                socket.Send(bytes);

            }

            socket.Close();
        }
    }
}

