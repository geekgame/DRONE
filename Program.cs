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
    using System.Collections.Generic;
    using System.Net.Sockets;

    using Core.Networking;

    using SharpDX;

    using Action = Drone.Core.enums.action;
    using Mode = Drone.Core.enums.mode;
    using Objective = Drone.Core.enums.objective;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// All nav points remaining. Stack so everytime a point is reached, it's deleted and next point pop.
        /// </summary>
        public static Stack<Vector3> NavPoints = new Stack<Vector3>();

        /// <summary>
        /// The position of current objective
        /// </summary>
        public static Vector3 posCurObjective = new Vector3();

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
        public static Mode curMode = Mode.modeVertical;     //le mode de vol actuel

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
