// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Internet.cs" company="geekgame">
//   All rights reserved
// </copyright>
// <summary>
//   The internet.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Drone.Core.Networking
{
    using System;
    using System.IO;
    using System.Net;

    using Drone.Core.utils;

    /// <summary>
    /// The internet.
    /// </summary>
    internal static class Internet
    {
        /// <summary>
        /// The get http.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetHttp(string url)
        {
            try
            {
                var r = WebRequest.Create(url);
                var wr = r.GetResponse();
                var data = wr.GetResponseStream();
                var html = string.Empty;

                if (data == null) return html; // Currently, html = string.Empty
                using (var sr = new StreamReader(data))
                {
                    html = sr.ReadToEnd();
                }

                return html;
            }
                // ReSharper disable once CatchAllClause
            catch (Exception)
            {
                Console2.WriteLine("erreur.", ConsoleColor.Red);
                return string.Empty;
            }
        }
    }
}