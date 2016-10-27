using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet
{
    enum PacketType { Login, Connect, Message }
    class Packet
    {
        public PacketType packetType;
        public string sText1, sText2;

        private static byte PacketType2Byte(PacketType pt)
        {
            switch (pt)
            {
                case PacketType.Login: return 1;
                case PacketType.Connect: return 2;
                case PacketType.Message: return 3;
                default: throw new Exception("unknown packet type");
            }
        }

        private static PacketType Byte2PacketType(byte b)
        {
            switch (b)
            {
                case 1: return PacketType.Login;
                case 2: return PacketType.Connect;
                case 3: return PacketType.Message;
                default: throw new Exception("unknown packet type");
            }
        }
        private static byte[] Str2Bytes(string s)
        {
            return Encoding.GetEncoding(1251).GetBytes(s);
        }
        private static string Bytes2Str(byte[] ab, int index, int count)
        {
            return Encoding.GetEncoding(1251).GetString(ab, index, count);
        }
        public byte[] ToBytes()
        {
            byte[] abText1 = Str2Bytes(sText1);//текст 1 в байтах
            Int16 nSize1 = (Int16)abText1.Length;
            //union позволяет представлять массив чаров в байтах, например, в # такого нет
            byte[] abSize1 = BitConverter.GetBytes(nSize1);
            
            byte[] abText2 = null;//текст 2 в байтах
            Int16 nSize2 = 0;

            if (sText2 != "")
            {
                abText2 = Str2Bytes(sText2);
                nSize2 = (Int16)abText2.Length;
            }
            byte[] abSize2 = BitConverter.GetBytes(nSize2);
            byte bType = PacketType2Byte(packetType);

            int nCount = 5 + nSize1 + nSize2;
            byte[] aTotal = new byte[nCount];
            
            aTotal[0] = bType;//тип пакета
            abSize1.CopyTo(aTotal, 1);//длина 1
            abSize2.CopyTo(aTotal, 3);//длина 2
            abText1.CopyTo(aTotal, 5);
            if (abText2 != null)
                abText2.CopyTo(aTotal, 5 + nSize1);

            return aTotal;

        }

        static public  Packet FromBytes(byte[] aTotal)
        {
            Packet packet = new Packet();

            packet.packetType = Byte2PacketType(aTotal[0]);
            Int16 nSize1 = BitConverter.ToInt16(aTotal, 1);
            Int16 nSize2 = BitConverter.ToInt16(aTotal, 3);

            packet.sText1 = Bytes2Str(aTotal, 5, nSize1);
            if (nSize2 > 0)
                packet.sText2 = Bytes2Str(aTotal, 5 + nSize1, nSize2);
            else packet.sText2 = "";

            return packet;
        }
    }
}
