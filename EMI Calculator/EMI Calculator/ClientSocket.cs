using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EMI_Calculator
{
    public class ClientSocket
    {
        TcpClient tcpclnt = null;
        Stream stm = null;
        Int32 PortNumber = 0;
        string IPAddress = null;

        public ClientSocket()
        {

            try
            {
                IPAddress = GetLocalIPAddress();
                PortNumber = 35642;
                tcpclnt = new TcpClient();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public bool EstablishedConnection()
        {
            try
            {
                tcpclnt.Connect(IPAddress, PortNumber);
                Console.WriteLine("Marvellous Web : Connection Successful");
                stm = tcpclnt.GetStream();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (stm != null)
                {
                    stm.Close();
                }
                return false;
            }

            return true;
        }

        public void SendMessage(string msg)
        {
            byte[] ba = Encoding.UTF8.GetBytes(msg);
            Console.WriteLine("Marvellous Web : Sending data ...");
            stm.Write(ba, 0, ba.Length);
        }

        public string ReceiveMessage(int size)
        {
            byte[] bb = new byte[size];
            int k = stm.Read(bb, 0, size);
            string receive = Encoding.UTF8.GetString(bb, 0, k);
            Console.WriteLine("Marvellous Web : Message received from server {0}…", receive);
            return receive;
        }
        
        public static string GetLocalIPAddress()
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

    }
}
