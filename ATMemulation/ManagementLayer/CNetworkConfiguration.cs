﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ManagementLayer
{
    public sealed class CNetworkConfiguration
    {
        static readonly CNetworkConfiguration instance = new CNetworkConfiguration();

        Dictionary<int, String> nodesType = new Dictionary<int, string>() {{1, "client" } };
        private List<Data.CLink> LinkList = new List<Data.CLink>();
        
        CNetworkConfiguration()
        {
        }

        public static CNetworkConfiguration Instance
        {
            get
            {
                return instance;
            }
        }


        public List<Data.CLink> linkList
        {
            get { return LinkList; }
            set { LinkList = value; }
        }

        public void readConfig()
        {
            XmlTextReader textReader = new XmlTextReader("../../networkConfig.xml");
            try
            {
                while (textReader.Read())
                {
                    if (textReader.NodeType == XmlNodeType.Element && textReader.Name =="link")
                    {
                        //from
                        String[] fromArray = textReader.GetAttribute(0).Split(';');
                        String[] toArray = textReader.GetAttribute(1).Split(';');

                        if (fromArray[1] == "c") {fromArray[1] = "client";}
                        else if (fromArray[1] == "n") { fromArray[1] = "network"; }

                        if (toArray[1] == "c") { toArray[1] = "client"; }
                        else if (toArray[1] == "n") { toArray[1] = "network"; }

                        try
                        {
                            nodesType.Add(Convert.ToInt32(fromArray[0]), fromArray[1]);
                            nodesType.Add(Convert.ToInt32(toArray[0]), toArray[1]);
                        }
                        catch (Exception) { }


                        Data.CLinkInfo from = new Data.CLinkInfo(Convert.ToInt32(fromArray[0]), fromArray[1], Convert.ToInt32(fromArray[2]));
                        Data.CLinkInfo to = new Data.CLinkInfo(Convert.ToInt32(toArray[0]), toArray[1], Convert.ToInt32(toArray[2]));
                        //add
                        LinkList.Add(new Data.CLink(from, to, 1));


                        ConnectionsManager cm = ConnectionsManager.Instance;
                        foreach (Data.CLink l in LinkList) {

                        cm.setNetworkConnections(l.from.nodeNumber, l);
                         }
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void showNetworkConfiguration()
        {
            Console.WriteLine("Konfiguracja sieci : ");
            foreach (Data.CLink cl in LinkList)
                Console.WriteLine("node " + cl.from.nodeNumber + " port " + cl.from.portNumber + " type  " + cl.from.portType + " --> node " + cl.to.nodeNumber + " port " + cl.to.portNumber + " type " + cl.to.portType + " ");

        }

        public List<Data.CLink> getLinkList()
        {
            return LinkList;
        }


        public void addNodeToDictionary(int i, String type)
        {
            try
            {
                nodesType.Add(i, type);
                Console.WriteLine("dodałem " + i + " " + type);
            }
            catch (Exception e)
            {
                Console.WriteLine("Błąd przy dodawaniu węzła do słownika, węzeł o id = " + i + " juz isnieje " + e.Message);
            }
        }

        public void showNodes()
        {
            foreach(KeyValuePair<int, String> pair in nodesType)
            {
                Console.WriteLine("\t Wezly w sieci");
                Console.WriteLine("{0}, {1}",
                pair.Key,
                pair.Value);
            }
        }

        public bool checkFormula(int args)
        {
            // sprawdzanie czy takie połączenie już istnieje oraz czy istnieją nody
            bool warunek1 = false;
            //bool warunek2 = false;


            if (nodesType.ContainsKey(args)) 
            {
                warunek1 = true;
            }

            //Data.CLink cl = new Data.CLink(new Data.CLinkInfo(args[0], null, args[1]), new Data.CLinkInfo(args[2], null, args[3]));

            //if (!LinkList.Contains(cl)) 
            //{
            //    warunek2 = true;
            //}

            if(warunek1)
            { return true; }

            return false;
        }


    }
}