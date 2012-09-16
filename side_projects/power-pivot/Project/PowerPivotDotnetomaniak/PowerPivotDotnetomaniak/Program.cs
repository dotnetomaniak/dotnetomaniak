using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;
using System.Web;
using System.Net;
using System.IO;
using System.Threading;

namespace PowerPivotDotnetomaniak
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ManiakDataContext())
            {
                int id = 0;
                XDocument doc = new XDocument();
                XElement items = new XElement("Items");
                //items.SetAttributeValue("ImgBase", @"maniak_deepzoom\maniak.dzc");
                doc.Add(items);
                foreach (Story story
                    in context.Stories.Where(f=>f.PublishedAt.HasValue).OrderBy(f => f.PublishedAt))
                {                    
                    XElement item = new XElement("Item");
                    item.SetAttributeValue("Id", id);
                    item.SetAttributeValue("Name", story.Title);
                    item.SetAttributeValue("Href", story.Url);

                    string name = Path.Combine("Images", id + ".png");
                    //item.SetAttributeValue("Img", "#" + id);
                    item.SetAttributeValue("Img", name);

                    item.Add(new XElement("Facets",
                                          new XElement("Facet", new XAttribute("Name", "Użytkownik"),
                                                       new XElement("String", 
                                                                    new XAttribute("Value", story.User.UserName))),

                                          new XElement("Facet", new XAttribute("Name", "dotnetomaniaki"),
                                                       new XElement("Number",
                                                                    new XAttribute("Value", story.StoryVotes.Count))),
                                          new XElement("Facet", new XAttribute("Name", "Komentarze"),
                                                       new XElement("Number",
                                                                    new XAttribute("Value", story.StoryComments.Count))),
                                          new XElement("Facet", new XAttribute("Name", "Wyświetlenia"),
                                                       new XElement("Number",
                                                                    new XAttribute("Value", story.StoryViews.Count))),
                                         new XElement("Facet", new XAttribute("Name", "Strona"),
                                                       new XElement("String",
                                                                    new XAttribute("Value", new Uri(story.Url).Host))),
                                         new XElement("Facet", new XAttribute("Name", "Kategoria"),
                                                       new XElement("String",
                                                                    new XAttribute("Value", story.Category.Name))),
                                         new XElement("Facet", new XAttribute("Name", "Data"),
                                                       new XElement("DateTime", 
                                                                    new XAttribute("Value", story.PublishedAt.Value.ToShortDateString()))),
                                        new XElement("Facet", new XAttribute("Name", "Opis"),
                                                       new XElement("LongString",
                                                                    new XAttribute("Value", story.TextDescription)))
                                                                    ));
                    
                    if (File.Exists(name) == false)
                    {
                        var p = new P {Name = name, Story = story.Url};
                        DownloadImage(p);
                        //ThreadPool.QueueUserWorkItem(DownloadImage, p);
                    }
                    Console.WriteLine("Processed id: " + id);
                    //add to the collection
                    items.Add(item);
                    id++;
                }
                doc.Save("facets.cxml");
            }            
            Console.WriteLine("Done");
            Console.ReadLine();
        }        

        static void DownloadImage(object o)
        {
            //Thread.Sleep(200);
            P p = (P)o;
            HttpWebRequest request =
                (HttpWebRequest)
                WebRequest.Create(
                    string.Format(
                        @"http://images.pageglimpse.com/v1/thumbnails?devkey=135ea18cdc2784d8d634a31830020a29&url={0}&size=large&root=no&nothumb=http://dotnetomaniak.pl/Assets/Images/pg-preview-na.jpg",
                        p.Story));

            var response = request.GetResponse();            
            using (BinaryReader wr = new BinaryReader(response.GetResponseStream()))
            {
                File.WriteAllBytes(p.Name, wr.ReadBytes((int)response.ContentLength));
            }
            Console.WriteLine("File saved: {0}", p.Name);
        }
    }

    class P
    {
        public string Name;
        public string Story;
    }
}
