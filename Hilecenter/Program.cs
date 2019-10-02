using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Hilecenter
{
    static class Program
    {
        public static bool isTrue = false;
        public static DateTime targetDate = new DateTime(2019, 8, 28, 21, 36, 0);//Bitis tarihi
        public static DateTime buyingDate = new DateTime(2019, 8, 28, 17, 35, 0);//Satin aldigi tarih
        public static string gameProcessName = "csgo";
        public static bool isUpdated = false;
        public static int versionNumber = 5;
        
        /// <summary>
        ///// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {   
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HileCenter());
        }
        public static DateTime GetTime()
        {
            DateTime dt;
            try
            {
                dt =
 DateTime.Parse(GetWebResponse("http://hilecim.net/versiyon/get_time.php")).AddHours(7);
            }
            catch (Exception)
            {
                dt = DateTime.Now;
            }
            return dt;
        }
        public static string GetWebResponse(string url)
        {
            string responseString = "";
            WebRequest client;
            WebResponse response;
            StreamReader streamReader;
            try
            {
                client = HttpWebRequest.Create(url);
           response  = client.GetResponse();

            streamReader = new StreamReader(response.GetResponseStream());
            responseString = streamReader.ReadToEnd();
                responseString = responseString.Remove(responseString.IndexOf("</body>"));
                responseString = responseString.Substring(responseString.IndexOf("<body>") + 8);
                response.Dispose();
            streamReader.Dispose();
            }
            catch (Exception)
            {

            }
            
            
            return responseString;
        }
       
    }
}
