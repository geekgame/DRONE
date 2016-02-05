// --------------------------------------------------------------------------------------------------------------------
// <copyright company="geekgame" file="PythonControl.cs">
//     All rights reserved
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

//UNIQUEMENT CONSERVE POUR HISTORIQUE
#pragma warning disable 219
namespace Drone.Core.LowLevel
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;

    using Drone.Properties;

    /// <summary>
    /// The python control.
    /// </summary>
    internal class PythonControl
    {
        /// <summary>
        /// The execute python.
        /// </summary>
        /// <param name="cmd">
        /// The cmd.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="InvalidOperationException">Aucun nom de fichier a été spécifié dans le <paramref name="startInfo" /> du paramètre <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> propriété.ou Le <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> propriété de le <paramref name="startInfo" /> paramètre est true et <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" />, <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" />, ou <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> propriété est également true.ouLe <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> propriété de le <paramref name="startInfo" /> paramètre est true et le <see cref="P:System.Diagnostics.ProcessStartInfo.UserName" /> propriété n'est pas null ou est vide ou <see cref="P:System.Diagnostics.ProcessStartInfo.Password" /> propriété n'est pas null.</exception>
        /// <exception cref="ArgumentNullException">Le paramètre <paramref name="startInfo" /> a la valeur null. </exception>
        /// <exception cref="ObjectDisposedException">L'objet de processus a déjà été supprimé. </exception>
        /// <exception cref="FileNotFoundException">Le fichier spécifié dans le <paramref name="startInfo" /> du paramètre <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> propriété introuvable.</exception>
        /// <exception cref="Win32Exception">Une erreur s'est produite lors de l'ouverture du fichier associé. ouLa somme de la longueur des arguments et de la longueur du chemin d'accès complet au processus dépasse 2080.Le message d'erreur associé à cette exception peut être une des valeurs suivantes: « la zone de données passée à un appel système est trop petit. » ou « Accès refusé ».</exception>
        public static void ExecutePython(string cmd, string args)
        {
            // http://stackoverflow.com/a/27801632
            var start = new ProcessStartInfo
                            {
                                FileName = cmd, 
                                Arguments = args, 
                                UseShellExecute = false, 
                                RedirectStandardOutput = true
                            };
            using (var process = Process.Start(start))
            {
                if (process != null)
                {
                    using (var reader = process.StandardOutput)
                    {
                        var result = reader.ReadToEnd();
                        Console.WriteLine(result);
                        if (string.CompareOrdinal(result, string.Empty) != 0)
                        {
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The init python files.
        /// </summary>
        /// <param name="pythonExePath">
        /// The python exe path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="IOException">Une erreur d'E/S s'est produite. </exception>
        /// <exception cref="OutOfMemoryException">Mémoire est insuffisante pour allouer un tampon pour la chaîne retournée. </exception>
        /// <exception cref="ArgumentOutOfRangeException">Le nombre de caractères dans la ligne suivante de caractères est supérieur à <see cref="F:System.Int32.MaxValue" />.</exception>
        /// <exception cref="FileNotFoundException">Le fichier spécifié dans le <paramref name="startInfo" /> du paramètre <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> propriété introuvable.</exception>
        /// <exception cref="InvalidOperationException">Aucun nom de fichier a été spécifié dans le <paramref name="startInfo" /> du paramètre <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> propriété.ou Le <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> propriété de le <paramref name="startInfo" /> paramètre est true et <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" />, <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" />, ou <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> propriété est également true.ouLe <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> propriété de le <paramref name="startInfo" /> paramètre est true et le <see cref="P:System.Diagnostics.ProcessStartInfo.UserName" /> propriété n'est pas null ou est vide ou <see cref="P:System.Diagnostics.ProcessStartInfo.Password" /> propriété n'est pas null.</exception>
        /// <exception cref="ArgumentNullException">Le paramètre <paramref name="startInfo" /> a la valeur null. </exception>
        /// <exception cref="ObjectDisposedException">L'objet de processus a déjà été supprimé. </exception>
        /// <exception cref="Win32Exception">Une erreur s'est produite lors de l'ouverture du fichier associé. ouLa somme de la longueur des arguments et de la longueur du chemin d'accès complet au processus dépasse 2080.Le message d'erreur associé à cette exception peut être une des valeurs suivantes: « la zone de données passée à un appel système est trop petit. » ou « Accès refusé ».</exception>
        public static bool InitPythonFiles(string pythonExePath = "")
        {
            debut:
            if (string.CompareOrdinal(pythonExePath, string.Empty) != 0 && File.Exists(pythonExePath))
            {
                return true;
            }

            // TODO : télécharger python
            // Sous linux uniquement
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var start = new ProcessStartInfo();
                start.FileName = "apt-get";
                start.Arguments = "install python3";
                start.UseShellExecute = true;
                Process.Start(start);
            }
            else
            {
                Console.WriteLine(Resources.NeedPython3);
                Console.WriteLine(Resources.PleaseInstallAndEnterPath);
                pythonExePath = Console.ReadLine();
                goto debut;
            }

            var servoL = "import sys, os; os.system(\"echo 1=\"+sys.argv[1]);";
            var servoR = "import sys, os; os.system(\"echo 2=\"+sys.argv[1]);";
            var engL = "import sys, os; os.system(\"echo 3=\"+sys.argv[1]);";
            var engR = "import sys, os; os.system(\"echo 4=\"+sys.argv[1]);";

            // Le moteur arrière est géré avec les relais, donc GPIO, donc pas dans ce fichier. Voir Core.LowLevel.GPIO
            return true;
        }
    }
}