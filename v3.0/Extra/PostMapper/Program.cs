using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;


namespace PostMaping
{
    internal class Program
    {
        private static void Main(string[] args)
        {            
            const string connectionString = @"Data Source=sql2008.webio.pl,2401;Database=pawlos_dotnetomaniak;User Id=pawlos_dotnetomaniak;Password=p23q2M2pl!;MultipleActiveResultSets=true";

            using (var connection = new SqlConnection(connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var command = new SqlCommand(" SELECT [Title],[Url],[Id] FROM [pawlos_dotnetomaniak].[dbo].[Story] WHERE Url LIKE '%www.piotrosz.aspnet.pl%'", connection);
                using (var reader = command.ExecuteReader())
                {
                    var urlAddres = new UrlAddres();
                    var client = new WebClient();
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0) + "\n\n");
                        string fullSiteInString = client.DownloadString(urlAddres.QuestionUrl(reader.GetString(0)));
                        string url = urlAddres.SearchForNewUrl(fullSiteInString);

                        if (urlAddres.SprawdzStrone(url))
                        {
                            var updateCommand = new SqlCommand("UPDATE Story SET Url=@Url, UrlHash=@UrlHash WHERE Id=@Id", connection);
                            updateCommand.Parameters.AddWithValue("Url", url);
                            updateCommand.Parameters.AddWithValue("UrlHash", Hash.ComputeHash(url.ToUpperInvariant()));
                            updateCommand.Parameters.AddWithValue("Id", reader.GetGuid(reader.GetOrdinal("Id")));
                            updateCommand.ExecuteNonQuery();
                            Console.WriteLine("\nZamieniono: " + reader.GetString(reader.GetOrdinal("Url")) + " na: \n" + url + "\n");
                        }
                        else
                        {
                            Console.WriteLine("\nNie znaleziono odpowiednika dla:" + reader.GetString(reader.GetOrdinal("Title")) + "\n");
                        }
                    }
                }
            }
        }
    }
}
