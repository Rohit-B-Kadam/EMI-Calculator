using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ROIServer
{
    public class ServerSocket
    {
        Int32 PortNumber = 0;
        string IPAddress = null;
        Socket s = null;
        TcpListener myList = null;

        public ServerSocket()
        {
            IPAddress = GetLocalIPAddress();
            PortNumber = 35642;
        }

        public void StartServer()
        {
            try
            {

                IPAddress ipAd = System.Net.IPAddress.Parse(IPAddress);
                Console.WriteLine("Marvellous Web : Server started ... ");
                myList = new TcpListener(ipAd, PortNumber);
                myList.Start();
                
                //Console.WriteLine("Marvellous Web : Server started at port : " + MarvellousPort);
                //Console.WriteLine("Marvellous Web : The local End point is :" + myList.LocalEndpoint);
                Console.WriteLine("Marvellous Web : Server Waiting for a connection ....");
                s = myList.AcceptSocket();

                Console.WriteLine("Marvellous Web : Connection Established with client....");
                Console.WriteLine("Marvellous Web : Connection accepted from " + s.RemoteEndPoint);

                while (true)
                {
                    string receiveMsg = ReceiveMessage(256);
                    if(receiveMsg.Equals("GiveROI"))
                    {
                        
                        SendMessage(GetROI());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Marvellous Web : Exception - " + e.StackTrace);
            }
            finally
            {
                Console.WriteLine("\nMarvellous Web : Deallocating all resources ...");
                if (s != null)
                {
                    s.Close();
                }
                if (myList != null)
                {
                    myList.Stop();
                }
            }

        }

        public string GetLocalIPAddress()
        {
            Console.WriteLine("Marvellous Web : Host name - {0}", Dns.GetHostName());
            var Marvelloushost = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in Marvelloushost.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Marvellous Web :No network adapters with an IPv4 address in the system!");
        }

        public void SendMessage(string msg)
        {
            byte[] ba = Encoding.UTF8.GetBytes(msg);
            Console.WriteLine("Marvellous Web : Sending data ...");
            s.Send(ba);
        }

        public string ReceiveMessage(int size)
        {
            byte[] bb = new byte[size];
            int k = s.Receive(bb);
            string receive = Encoding.UTF8.GetString(bb, 0, k);
            Console.WriteLine("Message received from client {0}…", receive);
            return receive;
        }

        public string GetROI()
        {
            //Reading XML
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            try {

                doc.Load("Interest.xml");
                return doc.DocumentElement.InnerText;
            }
            catch (Exception)
            {

            }
                return "0";
        }
        
    }

    class Program
    {


        static void Main(string[] args)
        {
            ServerSocket ss = new ServerSocket();
            ss.StartServer();
        }
    }
}
