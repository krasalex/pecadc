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

    class Program
    {
       
         //Thread receiveThread;
        //Thread textThread; 
        


        static void Main(string[] args)
        {
            ushort[] impWidth = { 300, 1, 10500, 1 };
             

            Console.WriteLine("Hello World!");
           // udpServer = new UDPServer();
           UDPServer udpServer= new UDPServer();
            Span<ushort> data =udpServer.SendMessage(impWidth);

            //receiveThread = new Thread(new ThreadStart(udpServer.ReceiveMessage));
            //receiveThread.Start();
            udpServer.Close();
            int k = 1;

        }
    }



}
