using System;
using System.Net;
using System.Web;

namespace PostMaping
{
    public class UrlAddres
    {
        public string QuestionUrl(string oldUrl)
        {
            string whereToSearch = oldUrl + " site:www.piotrludwiczuk.net";
            string newUrl = "https://www.google.pl/search?q=" + HttpUtility.UrlEncode(whereToSearch) + "&hl=pl";
            return newUrl;
        }

        public string SearchForNewUrl(string siteInString)
        {
            int biginOfString = siteInString.IndexOf("main");
            int endOfString = siteInString.IndexOf("/body>", biginOfString + 1);
            string halfSiteInString = siteInString.Substring(biginOfString, endOfString - biginOfString);

            biginOfString = halfSiteInString.IndexOf("http://www.piotrludwiczuk.net");
            endOfString = halfSiteInString.IndexOf(".aspx", biginOfString + 1);
            string encodedString = halfSiteInString.Substring(biginOfString, endOfString - biginOfString);
            return Uri.UnescapeDataString(HttpUtility.UrlDecode(encodedString)) + ".aspx";       
        }

        public bool SprawdzStrone(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.                
                request.Method = "HEAD";
                //Getting the Web Response.
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //Returns TRUE if the Status code == 200
                    return (response.StatusCode == HttpStatusCode.OK);
                }
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }        
    }
}