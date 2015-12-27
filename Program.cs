// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="geekgame">
//   
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Drone
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading;

    using Core.Networking;

    using SharpDX;

    using Action = Drone.Core.enums.action;
    using Mode = Drone.Core.enums.mode;
    using Objective = Drone.Core.enums.objective;

    /// <summary>
    /// The program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// All nav points remaining. Stack so everytime a point is reached, it's deleted and next point pop.
        /// </summary>
        public static Stack<Vector3> NavPoints = new Stack<Vector3>();

        /// <summary>
        /// The position of current objective
        /// </summary>
        public static Vector3 PosCurObjective = new Vector3();

        /// <summary>
        /// What does the drone want ?
        /// </summary>
        public static Objective ObjCurObjective = new Objective();

        /// <summary>
        /// What does the drone do ?
        /// </summary>
        public static Action CurAction;

        /// <summary>
        /// The current mode (vertical or horizontal)
        /// </summary>
        public static Mode CurMode = Mode.modeVertical;     //le mode de vol actuel

        /// <summary>
        /// Does the drone have to stay balanced ?
        /// </summary>
        public static volatile bool BalanceDrone = false;

        /// <summary>
        /// Gets or sets a value indicating whether the user can control the drone or not
        /// </summary>
        public static bool Logged { get; set; }

        /// <summary>
        /// Gets or sets the socket used to speak with server.
        /// </summary>
        public static Socket Sock { get; set; } = null;

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args passed by using -XXX after the name of the app (not used in this app).
        /// </param>
        /// <exception cref="ObjectDisposedException">L'objet de processus a déjà été supprimé. </exception>
        /// <exception cref="Win32Exception">Une erreur s'est produite lors de l'ouverture du fichier associé. ouLa somme de la longueur des arguments et de la longueur du chemin d'accès complet au processus dépasse 2080.Le message d'erreur associé à cette exception peut être une des valeurs suivantes: « la zone de données passée à un appel système est trop petit. » ou « Accès refusé ».</exception>

        public static void Main(string[] args)
        {
            while (true)
            {
                switch (CurAction)
                {
                    case Action.ActionBooting:
                        try
                        {
                            CurAction = Startup.Boot() ? Action.ActionWaiting : Action.ActionProblem;
                        }
                        catch (ThreadStateException threadStateException)
                        {
                            // TODO: Handle the threadStateException 
                        }
                        catch (FileNotFoundException fileNotFoundException)
                        {
                            // TODO: Handle the FileNotFoundException 
                        }
                        catch (Win32Exception win32Exception)
                        {
                            // TODO: Handle the Win32Exception 
                        }
                        Sock = Core.Networking.Sock.mySock;
                        break;

                    case Action.ActionWaiting:

                        System.Threading.Thread.Sleep(1000);

                        // Démarrer check serveur
                        // Démarrer attente ordres
                        break;

                    default:
                        CurAction = Action.ActionBooting;
                        break;
                }       
            }
        }
    }
}
