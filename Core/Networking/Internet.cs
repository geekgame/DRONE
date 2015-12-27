namespace Drone.Core.Networking
{
    using System;
    using System.IO;
    using System.Net;

    using Drone.Core.utils;

    internal class Internet
    {
        public static string GetHttp(string url)
        {
            try
            {
                var r = WebRequest.Create(url);
                var wr = r.GetResponse();
                var data = wr.GetResponseStream();
                var html = string.Empty;

                using (var sr = new StreamReader(data))
                {
                    html = sr.ReadToEnd();
                }

                return html;
            }
            catch (Exception e)
            {
                Console2.WriteLine("erreur.", ConsoleColor.Red);
                return string.Empty;
            }
        }
    }
}