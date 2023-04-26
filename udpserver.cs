#define STM
//#define ESP

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Globalization;

namespace adcread2
{
    class UDPServer
    {
        private const string ip = "192.168.1.89";
        private const string senderIp = "192.168.1.193";
        private const int port = 1556;  //8081
        private const int senderPort = 1555;  //8081
        private IPEndPoint udpEndPoint;
        private EndPoint senderEndPoint;
        private Socket udpSocket;
        UdpClient receiver;

        public byte[] bufferData;
        public int count = 0;

        public delegate void AccountHandler(byte[] message);
       // public event AccountHandler? Notify;

        public UDPServer()
        {
            udpEndPoint = new IPEndPoint(IPAddress.Any, port); // создаем точку подключения  //  IPAddress.Any  IPAddress.Parse(ip)
            senderEndPoint = new IPEndPoint(IPAddress.Parse(senderIp), senderPort);  // экземпляр адреса

            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);// "дверь", через которую будет происходить соединение
            udpSocket.ReceiveBufferSize = 16777216;  // 16 МБ
            udpSocket.Bind(udpEndPoint);

            Thread currentThread = Thread.CurrentThread;
            currentThread.Priority = ThreadPriority.Highest;
        }

        public void ReceiveMessage() //private
        {
            byte[] buffer = new byte[1500];
            try
            {
                while (true)  // уложить длину в 1500 байт
                {
                    var size = udpSocket.Receive(buffer); //receiver.Receive(ref udpEndPoint); // получаем данные  
                    byte[] data = new byte[size];
                    Array.Copy(buffer, data, size);

                   // Notify?.Invoke(data);  // уведомляем подписанных на событие

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //receiver.Close();
                udpSocket.Close();
            }
        }

      public Span<ushort> SendMessage(ushort[] value) //private
        {
            int StopTime = 100; // 1000 ms
            byte[] buffer = new byte[1500];
            byte[] data= { 0 };
            try
            {
                udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, StopTime);
                var span = new Span<ushort>(value);
                var bytes = MemoryMarshal.AsBytes(span);
                var byteArray = bytes.ToArray(); //тут будет копирование.
                // send start paket
                udpSocket.SendTo(byteArray, senderEndPoint);
                // recive data
                var size = udpSocket.Receive(buffer); //receiver.Receive(ref udpEndPoint); // получаем данные  
                 data = new byte[size];
                Array.Copy(buffer, data, size);
               

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                udpSocket.Close();
            }
            finally
            {

            } 
            Span<ushort> massiveUShort = MemoryMarshal.Cast<byte, ushort>(data);
            return massiveUShort; 
        }

        public void Close()
        {
            udpSocket.Close();
        }
    }
}
