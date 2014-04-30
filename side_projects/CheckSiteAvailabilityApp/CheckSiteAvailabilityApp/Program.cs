using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CheckSiteAvailabilityApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int notResponse = 0;
            bool lastCheckedUrlAchieved = false;
            string lastCheckedUrl = "";
            int checkedUrl = 0;

            Console.WriteLine("If you want stop press q button...");
            string[] lines = File.ReadAllLines("alldata.txt");

            if (File.Exists("lastCheckedUrl.txt"))
            {
                lastCheckedUrl = File.ReadAllText("lastCheckedUrl.txt");
                if (Int32.Parse(lastCheckedUrl) != 0)
                    lastCheckedUrlAchieved = true;
            }
            else
            {                
                    lastCheckedUrlAchieved = true;
            }

            for (int iter = 0; iter < lines.Count(); iter++)
            {
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().KeyChar == 'q')
                        break;
                }
                if (!lastCheckedUrlAchieved)
                {                    
                    if (iter == Int32.Parse(lastCheckedUrl))
                        lastCheckedUrlAchieved = true;
                }
                else
                {
                    using (MyClient client = new MyClient())
                    {
                        client.HeadOnly = true;
                        try
                        {                            
                            client.DownloadData(lines[iter]);
                        }
                        catch
                        {                            
                            Console.WriteLine(lines[iter]);
                            notResponse++;
                        }
                    }
                }
                checkedUrl++;
            }            

            File.WriteAllText("lastCheckedUrl.txt", checkedUrl.ToString());
            Console.WriteLine(notResponse);           
        }
    }
    class MyClient : WebClient
    {
        public bool HeadOnly { get; set; }
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            if (HeadOnly && req.Method == "GET")
            {
                req.Method = "HEAD";
            }
            return req;
        }
    }
}