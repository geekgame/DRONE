// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Geekgame">
//    Matthieu VANCAYZEELE, 2015
// </copyright>
// <summary>
//   Defines the Startup type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#define DEBUG

namespace Drone
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    using Drone.Core.nav;
    using Drone.Core.Networking;
    using Drone.Core.utils;
    using Drone.Properties;

    /// <summary>
    /// Class to start program, initialization etc
    /// </summary>
    class Startup
    {
        public static string Login;
        public static string pass;
        public static string ip;
        public static int port;

        /// <summary>
        /// The boot.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool Boot()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            displayMessage();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Resources.Startup_Boot_Debut_phase_de_démarrage);
            checkFiles();
            getCredentials();
            Console.WriteLine(Resources.Startup_Boot_Drone_en_ligne__Connexion_au_serveur);
            Sock.init(ip, port);
            Console.WriteLine("Connexion au serveur effectuée. Test des données");
            System.Threading.Thread.Sleep(1000);
            Sock.Receive(Sock.mySock);
            System.Threading.Thread.Sleep(1000);
            Sock.Send(Sock.mySock,"test<EOF>");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Envoi / réception de données OK");
            Console.WriteLine("Check des paramètres du drone.");
            if (!Properties.Settings.Default.isConfigured)
            {
                Console2.WriteLine("Non paramétré. Il est conseillé de régler cela au plus vite.", ConsoleColor.Blue);
                Sock.Send(Sock.mySock, "Config");
            }
            else
            {
                Console.WriteLine("Drone paramétré :)");
                Sock.Send(Sock.mySock, "okConfig");
            }

            login1();

            while (!Program.Logged) ;
            Console2.WriteLine("Drone démarré. Bienvenue.", ConsoleColor.Green);
            Sock.Send(Sock.mySock, "on");


            Drone.Core.utils.Console2.WriteLine("Waiting", ConsoleColor.Blue);

            // Démarrer équilibrage
            Thread t_Balance = new Thread(new ThreadStart(flight.Balance));
            t_Balance.Start();
            System.Threading.Thread.Sleep(1000);

            // balanceDrone = false;
            return true;                                                                           
        }

        /// <summary>
        /// Afficher message pour design
        /// </summary>
        public static void displayMessage()
        {
            Console.WriteLine(@"/$$$$$$$  /$$$$$$$   /$$$$$$  /$$   /$$ /$$$$$$$$  ");
            Console.WriteLine(@"| $$__  $$| $$__  $$ /$$__  $$| $$$ | $$| $$_____ /");
            Console.WriteLine(@"| $$  \ $$| $$  \ $$| $$  \ $$| $$$$| $$| $$       ");
            Console.WriteLine(@"| $$  | $$| $$$$$$$/| $$  | $$| $$ $$ $$| $$$$$    ");
            Console.WriteLine(@"| $$  | $$| $$__  $$| $$  | $$| $$  $$$$| $$__ /   ");
            Console.WriteLine(@"| $$  | $$| $$  \ $$| $$  | $$| $$\  $$$| $$       ");
            Console.WriteLine(@"| $$$$$$$/| $$  | $$|  $$$$$$/| $$ \  $$| $$$$$$$$ ");
            Console.WriteLine(@"| _______ / | __ /  | __ / \______ / | __ /  \__ /|");
            Console.WriteLine(string.Empty);
            Console.WriteLine(@"       /$$   /$$ /$$$$$$$  /$$    /$$");
            Console.WriteLine(@"      | $$  | $$| $$__  $$| $$   | $$              ");
            Console.WriteLine(@"      | $$  | $$| $$  \ $$| $$   | $$              ");
            Console.WriteLine(@"      | $$  | $$| $$  | $$|  $$ / $$/              ");
            Console.WriteLine(@"      | $$  | $$| $$  | $$ \  $$ $$/               ");
            Console.WriteLine(@"      | $$  | $$| $$  | $$  \  $$$/                ");
            Console.WriteLine(@"      |  $$$$$$/| $$$$$$$/   \  $/                 ");
            Console.WriteLine(@"       \______ / |_______/     \_/                 ");
                                                  
                                                  
                                                  
        }
        public static void checkFiles()
        {
            Console.WriteLine("Vérification des fichiers");
            //Check python
            //while(!Drone.Core.LowLevel.PythonControl.InitPythonFiles()) ;

            //TODO : CHECK FICHIERS
            if(!System.IO.File.Exists("./dll.so"))
            {
                // créer la dll qui controle les servos avec servoblaster
                Drone.Core.LowLevel.servoblasterDll.CreateDll();
            }
            Console.WriteLine("OK.");
        }
        public static bool getCredentials()
        {
            Console.WriteLine("Récupération des identifiants.");
            Console.WriteLine("Tentative de connexion au serveur...");

            string file = Internet.GetHttp("http://127.0.0.1/drone/config");

            string[] datas = file.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Console.WriteLine("---------------------------------------");

            // 0 IP
            // 1 port
            // 2 login
            // 3 pass
            Console.WriteLine(datas[0] + "\n" + datas[1] + "\n" + datas[2] + "\n" + datas[3]);
            Login = datas[2];
            pass = datas[3];
            ip = datas[0];
            port = int.Parse(datas[1]);
            Console.WriteLine("---------------------------------------");
            return true;
        }
        public static void login1()
        {
            Console.WriteLine("Authentification au serveur");
            Sock.Send(Sock.mySock, "login|" + Login + "|" + pass+"<EOF>");
            Console.WriteLine("Requête envoyée. En attente de la réponse.");
        }

        public static bool reconnect()
        {
            if(!getCredentials())
            {
                Console.WriteLine("Impossible de se reconnecter.");
                Console2.WriteLine("Mode automatique.", ConsoleColor.Yellow);
                return false;
            }

            Sock.init(ip, port);
            Sock.Receive(Sock.mySock);
            System.Threading.Thread.Sleep(1000);
            Sock.Send(Sock.mySock, "rc");//reconnected

            return true;
        }

    }
}
