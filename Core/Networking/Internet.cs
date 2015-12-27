using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;

namespace Drone.Core.Networking
{
    class Internet
    {
        public static string GetHttp(string url)
        {
            debut:
            try
            {
                WebRequest r = WebRequest.Create(url);
                WebResponse wr = r.GetResponse();
                Stream data = wr.GetResponseStream();
                string html = String.Empty;

                using (StreamReader sr = new StreamReader(data))
                {
                    html = sr.ReadToEnd();
                }
                return html;
            }
            catch(Exception e)
            {
                Drone.Core.utils.Console2.WriteLine("erreur.", ConsoleColor.Red);
                return "";
            }
        }

    }
}
