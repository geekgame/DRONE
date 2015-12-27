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
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using Drone.Core.LowLevel;
    using Drone.Core.nav;
    using Drone.Core.Networking;
    using Drone.Core.utils;
    using Drone.Properties;

    /// <summary>
    /// Class to start program, initialization etc
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// login used to identify to the server
        /// </summary>
        private static string login;

        /// <summary>
        /// Password used to identify to the server
        /// </summary>
        private static string pass;

        /// <summary>
        /// IP of the server
        /// </summary>
        private static string ip;

        /// <summary>
        /// Port open on the server
        /// </summary>
        private static int port;

        /// <summary>
        /// Booting the drone
        /// </summary>
        /// <returns>
        /// <see cref="bool"/> True if correctly booted.
        /// </returns>
        /// <exception cref="ThreadStateException">Le thread a déjà été démarré. </exception>
        /// <exception cref="ObjectDisposedException">L'objet de processus a déjà été supprimé. </exception>
        /// <exception cref="FileNotFoundException">Le fichier spécifié dans le <paramref name="startInfo" /> du paramètre <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> propriété introuvable.</exception>
        /// <exception cref="Win32Exception">Une erreur s'est produite lors de l'ouverture du fichier associé. ouLa somme de la longueur des arguments et de la longueur du chemin d'accès complet au processus dépasse 2080.Le message d'erreur associé à cette exception peut être une des valeurs suivantes: « la zone de données passée à un appel système est trop petit. » ou « Accès refusé ».</exception>
        /// <exception cref="ArgumentNullException">Le paramètre <paramref name="startInfo" /> a la valeur null. </exception>
        /// <exception cref="InvalidOperationException">Aucun nom de fichier a été spécifié dans le <paramref name="startInfo" /> du paramètre <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> propriété.ou Le <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> propriété de le <paramref name="startInfo" /> paramètre est true et <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" />, <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" />, ou <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> propriété est également true.ouLe <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> propriété de le <paramref name="startInfo" /> paramètre est true et le <see cref="P:System.Diagnostics.ProcessStartInfo.UserName" /> propriété n'est pas null ou est vide ou <see cref="P:System.Diagnostics.ProcessStartInfo.Password" /> propriété n'est pas null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">La valeur de délai d'attente est négative et n'est pas égale à <see cref="F:System.Threading.Timeout.Infinite" />. </exception>
        /// <exception cref="SecurityException">L'utilisateur n'est pas autorisé à effectuer cette action. </exception>
        /// <exception cref="IOException">Une erreur d'E/S s'est produite. </exception>
        public static bool Boot()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            DisplayMessage();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Resources.Startup_Boot_Debut_phase_de_démarrage);
            CheckFiles();
            GetCredentials();
            Console.WriteLine(Resources.Startup_Boot_Drone_en_ligne__Connexion_au_serveur);
            Sock.init(ip, port);
            Console.WriteLine(@"Connexion au serveur effectuée. Test des données");
            Thread.Sleep(1000);
            Sock.Receive(Sock.mySock);
            Thread.Sleep(1000);
            Sock.Send(Sock.mySock, "test<EOF>");
            Thread.Sleep(1000);
            Console.WriteLine(@"Envoi / réception de données OK");
            Console.WriteLine(@"Check des paramètres du drone.");
            if (!Settings.Default.isConfigured)
            {
                Console2.WriteLine("Non paramétré. Il est conseillé de régler cela au plus vite.", ConsoleColor.Blue);
                Sock.Send(Sock.mySock, "Config");
            }
            else
            {
                Console.WriteLine(@"Drone paramétré :)");
                Sock.Send(Sock.mySock, "okConfig");
            }

            Login1();

            while (!Program.Logged)
            {
            }

            Console2.WriteLine(@"Drone démarré. Bienvenue.", ConsoleColor.Green);
            Sock.Send(Sock.mySock, "on");

            Console2.WriteLine("Waiting", ConsoleColor.Blue);

            // Démarrer équilibrage
            Thread tBalance = new Thread(Flight.Balance);
            try
            {
                tBalance.Start();
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine(@"NOT ENOUGH MEMORY. RESTARTING...");
                System.Threading.Thread.Sleep(1000);
                var psi = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = "/sbin/shutdown",
                    Arguments = "-r now"
                };
                Process.Start(psi);
            }
            Thread.Sleep(1000);

            // balanceDrone = false;
            return true;                                                                           
        }

        /// <summary>
        /// Afficher message pour design
        /// </summary>
        private static void DisplayMessage()
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

        /// <summary>
        /// Check if all files needed are present at the good location
        /// </summary>
        private static void CheckFiles()
        {
            Console.WriteLine(@"Vérification des fichiers");

            // Check python
            // while(!Drone.Core.LowLevel.PythonControl.InitPythonFiles()) ;

            // TODO : CHECK FICHIERS
            if (!File.Exists("./dll.so"))
            {
                // créer la dll qui controle les servos avec servoblaster
                servoblasterDll.CreateDll();
            }

            Console.WriteLine(@"OK.");
        }

        /// <summary>
        /// Get credentials from a HTTP web server in order to authenticate to the socket server
        /// </summary>
        /// <returns>
        /// <see cref="bool"/> True if it worked.
        /// </returns>
        private static bool GetCredentials()
        {
            Console.WriteLine(@"Récupération des identifiants.");
            Console.WriteLine(@"Tentative de connexion au serveur...");

            string file = Internet.GetHttp(@"http://127.0.0.1/drone/config");

            string[] datas = file.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            Console.WriteLine(@"---------------------------------------");

            // 0 IP
            // 1 port
            // 2 login
            // 3 pass
            if (datas.Length > 3)
            {
                Console.WriteLine(datas[0] + "\n" + datas[1] + "\n" + datas[2] + "\n" + datas[3]);
            }
            login = datas[2];
            pass = datas[3];
            ip = datas[0];
            port = int.Parse(datas[1]);
            Console.WriteLine(@"---------------------------------------");
            return true;
        }

        /// <summary>
        /// Send login to server
        /// </summary>
        /// <exception cref="IOException">
        /// Une erreur d'E/S s'est produite. 
        /// </exception>
        private static void Login1()
        {
            Console.WriteLine(@"Authentification au serveur");
            Sock.Send(Sock.mySock, "login|" + login + "|" + pass + "<EOF>");
            Console.WriteLine(@"Requête envoyée. En attente de la réponse.");
        }

        /// <summary>
        /// Reconnect when the drone lost connection to the server
        /// </summary>
        /// <returns>
        /// <see cref="bool"/> True when ok.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">La valeur de délai d'attente est négative et n'est pas égale à <see cref="F:System.Threading.Timeout.Infinite" />. </exception>
        /// <exception cref="IOException">Une erreur d'E/S s'est produite. </exception>
        public static bool Reconnect()
        {
            if(!GetCredentials())
            {
                Console.WriteLine(@"Impossible de se reconnecter.");
                Console2.WriteLine(@"Mode automatique.", ConsoleColor.Yellow);
                return false;
            }

            Sock.init(ip, port);
            Sock.Receive(Sock.mySock);
            Thread.Sleep(1000);
            Sock.Send(Sock.mySock, "rc"); // reconnected

            return true;
        }

    }
}
