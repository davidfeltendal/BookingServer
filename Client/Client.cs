using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            TcpClient Client = new TcpClient();
            Console.WriteLine("Start serveren");
            Client.Connect("127.0.0.1",1234);
            NetworkStream stream = Client.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);
            
            string s = null;

            while (s != "q")
            {
                s = Console.ReadLine();
                writer.Write(s);

                string txt = reader.ReadString();
                Console.WriteLine(txt);
            }
            Client.Close();
        }
    }
}
