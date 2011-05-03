﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace NetworkNode
{
    public sealed class CManagementAgent
    {

       static readonly CManagementAgent instance = new CManagementAgent();

       
       private bool status;
       private IPAddress ip = IPAddress.Parse(CConstrains.ipAddress);     //adres serwera
       private int portNum;
       private TcpListener portListener;
       private TcpClient client;
       private NetworkStream clientStream;

       private CManagementAgent()
       {
           portNum = 50000 + CConstrains.nodeNumber * 100;
       }

       public static CManagementAgent Instance
       {
           get
           {
               return instance;
           }
       }      

        public void setCommutationTable(Dictionary<Data.PortInfo, Data.PortInfo> commutationTable)
        {
            CCommutationTable.Instance.setCommutationTable(commutationTable);
        }

        public void resetCommutationTable()
        {
            CCommutationTable.Instance.resetCommutationTable();
        }

        public void removeConnection(CNetworkPortIn portIn, CNetworkPortOut portOut) //metoda rozlaczajaca polaczenie w polu komutacyjnym danego wezla
        {
            portIn.STATUS = false;
            portOut.STATUS = false;
            //CCommutationTable.Instance.removeConnection(portIn);
        }

        public void showConnections() //metoda wyswietlajaca zestawione polaczenia
        {
            
            CCommutationTable.Instance.showAll();   
        }

        public void mlConnectionListener()
        {
            portListener = new TcpListener(ip, portNum);  //listener na porcie danego węzła
            portListener.Start();

            client = portListener.AcceptTcpClient(); 
            clientStream = client.GetStream();  
            Console.WriteLine("connection with ML ");

            status = true;

            while (status) //uruchamiamy nasłuchiwanie
            {
                StreamReader sr = new StreamReader(clientStream);
                String dane = sr.ReadLine();
                
                //obsługa tego co przyśle ML
            }
        }
   }
}
