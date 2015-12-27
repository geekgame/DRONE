#define DEBUG

namespace Drone.Core.Networking
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class StateObject
    {
        public const int BufferSize = 256;

        public byte[] buffer = new byte[BufferSize];

        public StringBuilder sb = new StringBuilder();

        public Socket workSocket;
    }

    public class Sock
    {
        public static volatile Socket mySock;

        public static volatile string response;

        public static void Connect(EndPoint remoteEP, Socket client)
        {
            Console.WriteLine("Connexion");
            client.BeginConnect(remoteEP, ConnectCallback, client);

            // connectDone.WaitOne();
        }

        public static void init(string ServerIp, int ServerPort)
        {
            Console.WriteLine("Recherche de l'adresse IP...");
            Console.WriteLine("Résolution DNS");
            var iph = Dns.Resolve(Dns.GetHostName());

            var ret = IPAddress.None;
            foreach (var ip in iph.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ret = ip;
                    Console.WriteLine("OK");
                    break;
                }
            }

            Console.WriteLine("IP = " + ret);

            var ipServer = IPAddress.Parse(ServerIp);
            var remoteEP = new IPEndPoint(ipServer, ServerPort);
            var sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            mySock = sender;

            Connect(remoteEP, sender);
        }

        /// <summary>
        ///     La fonction attend et renvoie la valeur du socket lu
        ///     A utiliser uniquement dans un thread (car fonction bloquante)
        /// </summary>
        /// <returns>REnvoie le texte lu</returns>
        public static string Listen()
        {
            return string.Empty;
        }

        public static void Receive(Socket client)
        {
            try
            {
                var state = new StateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
                Console.WriteLine("ready to receive");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void Send(Socket client, string data)
        {
            var byteData = Encoding.ASCII.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, SendCallback, client);
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                var client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                Console.WriteLine("Socket connecté " + client.RemoteEndPoint);

                // connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                var state = (StateObject)ar.AsyncState;
                var client = state.workSocket;

                var bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);

                    foreach (char a in state.buffer)
                    {
                        if (a != 0)
                        {
                            Console.Write(a);
                        }
                    }

                    Perform.PerformSock(Encoding.ASCII.GetString(state.buffer));

                    for (var i = 0; i <= 255; i++)
                    {
                        state.buffer[i] = new byte();
                    }

                    Console.WriteLine(string.Empty);
                }
                else
                {
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("server disconnected. Last msg : \"" + response + "\"");
                        Console.ForegroundColor = ConsoleColor.White;

                        // Send(mySock, "a");
                        // receiveDone.Set();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                var client = (Socket)ar.AsyncState;

                var bytesSent = client.EndSend(ar);
                Console.WriteLine("Envoyé " + bytesSent + " bytes au serveur");

                // sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}