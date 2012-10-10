using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using HtmlAgilityPack;
using JobOfferParser.Crawlers;
using JobOfferParser.Data;
using JobOfferParser.Parsers;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using JobOfferParser.DB;

namespace JobOfferParser
{
    public class WorkerRole : RoleEntryPoint
    {
        private List<ICrawler> _crawlers;
        private OfferPersister _persister;
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("JobOfferParser entry point called", "Information");

            while (true)
            {
                try
                {
                    _crawlers.ForEach(x => x.Crawl());
                }
                catch (Exception ex)
                {
                    //var mailSend = new SmtpClient();
                    //var exceptionMessage = new MailMessage("crawl@octal.pl", "pawel@octal.pl")
                    //                           {Body = ex.ToString(), Subject = "Exception!"};
                    //mailSend.SendAsync(exceptionMessage, null);
                    Trace.WriteLine("Error crawling pages");
                }                

                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            string connectionString = ConfigurationManager.ConnectionStrings["Azure"].ConnectionString;
            _persister = new OfferPersister(connectionString);

            _crawlers = new List<ICrawler>
                            {
                                new GumtreeCrawler(_persister, new GumtreeParser()),
                                new PracujPlCrawler(_persister, new PracujPlParser())
                            };

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.);

            return base.OnStart();
        }
    }
}
