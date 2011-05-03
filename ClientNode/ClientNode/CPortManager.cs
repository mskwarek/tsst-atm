﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ClientNode
{
    // ta klasa ma za zadanie odczytać z pliku konfiguracyjnego i utworzyc odpowiednią liczbę portów wyjściowych klienckich
    class CPortManager
    {
        private List<CClientPortIn> InputClientPortList = new List<CClientPortIn>();
        private List<CClientPortOut> OutputClientPortList = new List<CClientPortOut>();
        private static  CPortManager instance = null;

        public static CPortManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CPortManager();
                }
                return instance; 
            }
        }


        private  CPortManager()
        {
            readConfig();
            showConfig();
            createPorts();
            showPorts();
        }

        public void readConfig() {
            XmlTextReader textReader = new XmlTextReader("../../config" + CConstrains.nodeNumber +".xml");
            try {
                while (textReader.Read()) {
                    switch (textReader.NodeType) {
                        case XmlNodeType.Element:
                            switch (textReader.Name) {
                                case "InputClientPort":
                                    CConstrains.inputPortNumber = Convert.ToInt16(textReader.ReadString());
                                    continue;
                                case "OutputClientPort":
                                    CConstrains.outputPortNumber = Convert.ToInt16(textReader.ReadString());
                                    continue;
                            }
                            break;
                    }
                }
            }
            catch(System.Exception e) {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void showConfig() {
            Console.WriteLine("in : " + CConstrains.inputPortNumber + " out : " + CConstrains.outputPortNumber); 
        }


        private void createPorts() {
            for (int i = 0; i < CConstrains.inputPortNumber; i++) {
                int systemPortNumber = 50000 + (CConstrains.nodeNumber * 100) +i;
                InputClientPortList.Add(new CClientPortIn(i, false, systemPortNumber));
            }

            for (int x = 0; x < CConstrains.outputPortNumber; x++) {
                OutputClientPortList.Add(new CClientPortOut(x, false));
            }
         }

        public void showPorts() {
            for (int i = 0; i < InputClientPortList.Count; i++) {
                Console.WriteLine("in " + InputClientPortList[i].ID + "  " + InputClientPortList[i].STATUS);
            }
            for (int i = 0; i < OutputClientPortList.Count; i++) {
                Console.WriteLine("out " + OutputClientPortList[i].ID + "  " + OutputClientPortList[i].STATUS);
            }
        }

        private CClientPortOut findFreePort() {   
            CClientPortOut t = OutputClientPortList.Find(delegate(CClientPortOut p) {  return p.STATUS == false; });
            return t;
        }
        // metoda odpowiedzialna za nadawanie wiadomości
        public void sendMsg(Data.CUserData data) {
            Console.WriteLine("wyszukuje port...");
            CClientPortOut free = findFreePort();
            if (free == null) { Console.WriteLine("Wszystkie porty zajete"); }
            else
            {
                Console.WriteLine("port o id= " + free.ID + " jest wolny");
                int index = OutputClientPortList.IndexOf(free);
                OutputClientPortList[index].send(data);
                OutputClientPortList[index].STATUS = true;
            }
        }

        public void stopSending(int i)
        {
            Console.WriteLine("Wstrzymywanie nadawnia na porcie : " + i);
            if (OutputClientPortList[i].STATUS == true)
            {
                //OutputClientPortList[i].stop();
                OutputClientPortList[i].STATUS = false;
            }
            else { Console.WriteLine("błędny numer portu" ); }
        }

        public void shutdownAllPorts()
        {
            foreach(CClientPortIn p in InputClientPortList) {
                p.shutdown();
            }

        }

    }
}