 #define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;


namespace Drone.Core.Networking
{
    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 256;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }

    public class Sock
    {
        public static volatile string response;

        public static volatile Socket mySock;

        public static void init(string ServerIp, int ServerPort)
        {

            Console.WriteLine("Recherche de l'adresse IP...");
            Console.WriteLine("Résolution DNS");
            IPHostEntry iph = Dns.Resolve(Dns.GetHostName());

            IPAddress ret = IPAddress.None;
            foreach(IPAddress ip in iph.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ret = ip;
                    Console.WriteLine("OK");
                    break;
                }
            }

            Console.WriteLine("IP = " + ret.ToString());

            IPAddress ipServer = IPAddress.Parse(ServerIp);
            IPEndPoint remoteEP = new IPEndPoint(ipServer, ServerPort);
            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            mySock = sender;

            Connect(remoteEP, sender);
        }

        public static void Connect(EndPoint remoteEP, Socket client)
        {
            Console.WriteLine("Connexion");
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
            //connectDone.WaitOne();
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                Console.WriteLine("Socket connecté " + client.RemoteEndPoint.ToString());

                //connectDone.Set();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Send(Socket client, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Envoyé " + bytesSent + " bytes au serveur");

                //sendDone.Set();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                Console.WriteLine("ready to receive");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesRead = client.EndReceive(ar);
                if(bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

                    foreach(char a in state.buffer)
                    {
                        if (a != 0)
                            Console.Write(a);
                    }

                    Perform.PerformSock(Encoding.ASCII.GetString(state.buffer));
                    
                    for(int i = 0;i<=255;i++)
                    {
                        state.buffer[i] = new byte();
                    }

                    Console.WriteLine("");
                }
                else
                {
                    if(state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("server disconnected. Last msg : \"" + response + "\"");
                        Console.ForegroundColor = ConsoleColor.White;
                        //Send(mySock, "a");
                        //receiveDone.Set();
                    }
                }                                            
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

         /// <summary>
         /// La fonction attend et renvoie la valeur du socket lu
         /// A utiliser uniquement dans un thread (car fonction bloquante)
         /// </summary>
         /// <returns>REnvoie le texte lu</returns>
        public static string Listen()
        {
            return "";
        }
    }
}
