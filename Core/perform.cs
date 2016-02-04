// --------------------------------------------------------------------------------------------------------------------
// <copyright file="perform.cs" company="geekgame">
//   geekgame
// </copyright>
// <summary>
//   actions executed when receiving commands
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Drone.Core.utils;

namespace Drone.Core
{
    using System;
    using System.Security;
    using System.Threading;

    using Drone.Core.LowLevel.servo;
    using Drone.Core.nav;
    using Drone.Core.Networking;
    using Drone.Properties;

    /// <summary>
    ///     actions executed when receiving commands
    /// </summary>
    public static class Perform
    {
        /// <summary>
        ///     Interpretation of command (from socket's message)
        /// </summary>
        /// <param name="command">(string) command</param>
        /// <returns>return message, or String.Empty()</returns>
        /// <exception cref="ThreadStateException">Le thread a déjà été démarré. </exception>
        /// <exception cref="SecurityException">
        ///     L'appelant n'a pas l'autorisation de sécurité appropriée pour effectuer cette
        ///     fonction.
        /// </exception>
        public static string PerformSock(string command)
        {
            Console.WriteLine(Resources.Perform_PerformSock_traitement_msg);

            if (string.CompareOrdinal(command.Split('|')[0], "login") == 0)
            {
                if (string.CompareOrdinal(command.Split('|')[1], "a") == 0       // TODO : change these values  to edit login and pass to connect to the drone.
                    && string.CompareOrdinal(command.Split('|')[2], "b") == 0)    //
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(
                        Resources.Perform_PerformSock_Connexion_autorisée__Utilisateur + command.Split('|')[1]);
                    Sock.Send(Sock.mySock, "Connected");
                    Console.WriteLine(Resources.Perform_PerformSock_Connecté);
                    Program.Logged = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        Resources.Perform_PerformSock_Mauvais_identifiants + command.Split('|')[1] + " "
                        + command.Split('|')[2]);
                    Console.ForegroundColor = ConsoleColor.White;
                    return "CoNO";
                }

                Console.ForegroundColor = ConsoleColor.White;
                return "CoOK";
            }

            if (!Program.Logged)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Resources.Perform_PerformSock_Denied_);
                Sock.Send(Sock.mySock, "DeniedNotLoggedIn");
                Sock.Send(Sock.mySock, command.Split('|')[1]);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                if (command.StartsWith("mode"))
                {
                    // Changer de mode de vol (vertical / horizontal)
                    if (command.StartsWith("modeVertical"))
                    {
                        Flight.SwitchMode(enums.mode.modeVertical);
                    }
                    else
                    {
                        Flight.SwitchMode(enums.mode.modeHorizontal);
                    }
                    Console2.WriteLine("Changing flying mode", ConsoleColor.Blue);
                }

                if (command.Contains("SHUT"))
                {
                    // emergency shutdown
                    ServoBlaster.setValue(2, 100);
                    ServoBlaster.setValue(4, 100);
                }

                if (command.Contains("CFG"))
                {
                    if (command.Contains("-GO"))
                    {
                        // Lancer, en thread, la configuration du drone.
                        try
                        {
                            new Thread(Configure.Config).Start();
                        }
                        catch (OutOfMemoryException)
                        {
                            Console.WriteLine(Resources.Perform_PerformSock_NOT_ENOUGH_MEMORY_TO_START_CONFIG_THREAD);
                            Sock.Send(Sock.mySock, "nemfct"); // NotEnoughMemoryForConfigThread
                            Environment.Exit(1);
                        }
                    }

                    if (command.Contains("PLAT"))
                    {
                        Configure.isDroneAPlat = true;
                    }

                    if (command.Contains("RIGHT"))
                    {
                        Configure.isDroneTurned = true;
                    }

                    if (command.Contains("UP"))
                    {
                        Configure.isDroneUp = true;
                    }
                }

                if (command.Contains("balance"))
                {
                    Program.BalanceDrone = true;
                }
            }

            return ".";
        }
    }
}