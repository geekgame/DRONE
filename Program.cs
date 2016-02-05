// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="geekgame">
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using Drone.Core;
using Drone.Properties;
using SharpDX;

namespace Drone
{
    using Action = enums.action;
    using Mode = enums.mode;
    using Objective = enums.objective;

    /// <summary>
    ///     The program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///     Does the drone have to stay balanced ?
        /// </summary>
        public static volatile bool BalanceDrone = false;

        /// <summary>
        ///     What does the drone do ?
        /// </summary>
        public static Action CurAction;

        /// <summary>
        ///     The current mode (vertical or horizontal)
        /// </summary>
        public static Mode CurMode = Mode.modeVertical; // le mode de vol actuel

        /// <summary>
        ///     All nav points remaining. Stack so everytime a point is reached, it's deleted and next point pop.
        /// </summary>
        public static Stack<Vector3> NavPoints = new Stack<Vector3>();

        /// <summary>
        ///     What does the drone want ?
        /// </summary>
        public static Objective ObjCurObjective = new Objective();

        /// <summary>
        ///     The position of current objective
        /// </summary>
        public static Vector3 PosCurObjective = new Vector3();

        /// <summary>
        ///     Gets or sets a value indicating whether the user can control the drone or not
        /// </summary>
        public static bool Logged { get; set; }

        /// <summary>
        ///     Gets or sets the socket used to speak with server.
        /// </summary>
        public static Socket Sock { get; set; }

        /// <summary>
        ///     The main.
        /// </summary>
        /// <param name="args">
        ///     The args passed by using -XXX after the name of the app (not used in this app).
        /// </param>
        /// <exception cref="ObjectDisposedException">L'objet de processus a déjà été supprimé. </exception>
        /// <exception cref="Win32Exception">
        ///     Une erreur s'est produite lors de l'ouverture du fichier associé. ouLa somme de la
        ///     longueur des arguments et de la longueur du chemin d'accès complet au processus dépasse 2080.Le message d'erreur
        ///     associé à cette exception peut être une des valeurs suivantes: « la zone de données passée à un appel système est
        ///     trop petit. » ou « Accès refusé ».
        /// </exception>
        public static void Main(string[] args)
        {
            while (true)
            {
                switch (CurAction)
                {
                    case Action.ActionBooting:
                            CurAction = Startup.Boot() ? Action.ActionWaiting : Action.ActionProblem;

                        Sock = Core.Networking.Sock.mySock;
                        break;

                    case Action.ActionWaiting:

                        Thread.Sleep(1000);

                        // Démarrer check serveur
                        // Démarrer attente ordres
                        break;
                    default:
                        Console.WriteLine(Resources.Program_Main_boot_fail_new_try_in_3_seconds);
                        Thread.Sleep(3000);
                        Console.Clear();
                        CurAction = Action.ActionBooting;
                        break;
                }
            }
        }
    }
}