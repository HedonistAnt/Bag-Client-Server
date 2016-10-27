using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApplication7
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress address = ipHost.AddressList[0];
            Socket socketServer = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // IPAddress address = IPAddress.Parse("192.168.1.165");
            EndPoint ep = new IPEndPoint(address, 2016);

            socketServer.Bind(ep);
            socketServer.Listen(100);

            while (true)
            {
                Socket handler = socketServer.Accept();
                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessClient), handler);
            }
            //IPendpoint epClient = (ipendpoint)socketClient.RemoteEndpoint; k komu podkluch
            /*        while (true)
                    {
               
                        byte[] bytes = new byte[2048];
                        int nReceive = handler.Receive(bytes);
                         
                        //преобразование  полученных данных (в известной кодировке)в строку
                        string sTextReceived = Encoding.GetEncoding(1251).GetString(bytes, 0, nReceive);

                        Console.WriteLine(sTextReceived);
                    }*/

            socketServer.Close();
        }

        static void ProcessClient(object ob)
        {
            Socket handler = (Socket)ob;

            byte[] bytes = new byte[2048];
            try
            {
                while (true)
                {
                    int nReceive = handler.Receive(bytes);
                    Packet.Packet packet = Packet.Packet.FromBytes(bytes);
                   // string s = Encoding.GetEncoding(1251).GetString(bytes, 0, nReceive);
                    Console.WriteLine(packet.sText1+":" +packet.sText2);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Клиент отключился");
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка " + e.Message);
            }
            handler.Close();
        }
    }
}
