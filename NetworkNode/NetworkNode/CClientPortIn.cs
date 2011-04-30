﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace NetworkNode
{
// Klasa portu wyjściowego dziedzicząca po CClientPort

    class CClientPortIn : CClientPort
    {
        private bool status;
        private IPAddress ip = IPAddress.Parse(CConstrains.ipAddress);     //adres serwera
        private TcpListener portListener;
        private TcpClient client;
        private NetworkStream clientStream;
        private StreamWriter serwerStream;
        private static String helloMessage = "Welcome to port : " ;

        public CClientPortIn(int id, Boolean busy, int systemPortNumber) : base(id, busy)
        {
            base.PORTNUMBER = systemPortNumber;
            base.PORTTYPE = "IN";
            base.PORTCLASS = "ClientPort";
            Console.WriteLine("Port kliencki o id = " + id + " będzie nasłuchiwał na porcie systemowym = " + base.PORTNUMBER);
        }



        public void init() //metoda uruchamiająca nasłuchiwanie na porcie. 
        {
            status = true;
            portListener = new TcpListener(ip, base.PORTNUMBER);  //tworzymy obiekt  nasłuchujący na podanym porcie
            portListener.Start();                      //uruchamiamy serwer

            client = portListener.AcceptTcpClient(); //akceptujemy żądanie połączenia
            clientStream = client.GetStream();  //pobieramy strumień do wymiany danych
            Console.WriteLine("connection accepted ");
            while (status) //uruchamiamy nasłuchiwanie
            {
                StreamReader sr = new StreamReader(clientStream);
                String dane = sr.ReadLine();
                Console.WriteLine(dane);
            }

        }

        public void shutdown()
        {
            status = false;
            client.Close();
            portListener.Stop();
        }


    
        

    
    }
}

