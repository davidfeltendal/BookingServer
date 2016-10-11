using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookingServer
{
    class Server
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int portnr = 1234;
            TcpListener listener = new TcpListener(ipAddress, portnr);
            Book b = new Book();
            Queue<string> buffer = new Queue<string>();
            try
            {
                listener.Start();

                Socket s = listener.AcceptSocket();
                Console.WriteLine(s.RemoteEndPoint);
                while (true)
                {
                    Console.WriteLine("Wait for client");


                    NetworkStream socketStream = new NetworkStream(s);
                    BinaryWriter writer = new BinaryWriter(socketStream);
                    BinaryReader reader = new BinaryReader(socketStream);
                    writer.Write("GEM/HENT");

                    string txt = reader.ReadString();
                    Console.WriteLine("Recieved: " + txt);

                    string[] parts = txt.Split(' ');
                    string opcode = parts[0];
                    string reply = "";

                    if (opcode.ToUpper() == "ECHO")
                    {
                        reply = "REPLY " + parts[1];
                    }
                    if (opcode.ToUpper() == "HENT")
                    {
                        //int fly = Convert.ToInt32(parts[1]);
                        //int seat = Convert.ToInt32(parts[2]);
                        reply = buffer.Dequeue();
                        //reply += b.Booking(fly, seat);
                    }
                    if (opcode.ToUpper() == "GEM")
                    {
                        //int fly = Convert.ToInt32(parts[1]);
                        buffer.Enqueue(parts[1]);
                        reply = parts[1] +" Gemt";
                        //reply += b.Read(fly);
                    }
                    writer.Write(reply);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Offline");
                listener.Stop();
            }
        }
    }

    class Book
    {
        public int[] booking = {100, 100, 100, 100, 100};
        public string Booking(int fly, int seat)
        {
            string reply = "Default";
            if (seat <= booking[fly])
            {
                booking[fly] = booking[fly] - seat;
                reply = "OK: Remaining seats on flight "+ fly + ": " + booking[fly];
            }  
            else reply = "Error!";
            return reply;
        }

        public string Read(int fly)
        {
            return "Available seats on flight " + fly + ": " + booking[fly];
        }
    }
}
