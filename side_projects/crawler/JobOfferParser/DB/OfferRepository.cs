using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using JobOfferParser.Data;
using System.Data.SqlClient;

namespace JobOfferParser.DB
{
    public class OfferRepository : IOfferRepository
    {
        private readonly string _connectionString;

        public OfferRepository(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            _connectionString = connectionString;
        }

        public bool InsertOffer(Offer offer)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using(SqlCommand selectCommand = new SqlCommand("SELECT * FROM RawOffers WHERE SHA1='" + offer.Sha1 + "'", connection))
                {
                    if(selectCommand.ExecuteScalar() == null)
                    {
                        using (SqlCommand insertCommand = new SqlCommand("INSERT INTO RawOffers (Title, Text, Province, City, SHA1, Link, Date) VALUES (@title, @text, @province, @city, @sha1, @link, @date)", connection))
                        {
                            AddOfferToQuery(offer, insertCommand);
                            insertCommand.ExecuteNonQuery();
                            result = true;
                        }
                    }
                }
              
            }

            return result;
        }

        public IEnumerable<Keyword> GetAllKeywords()
        {
            var keywords = new List<Keyword>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (SqlCommand selectCommand = new SqlCommand("SELECT * FROM Keywords", connection))
                {
                    var reader = selectCommand.ExecuteReader();
                    
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var keywordId = (int) reader["Id"];
                            var name = (string)reader["Name"];

                            keywords.Add(new Keyword()
                                             {
                                                 Id = keywordId,
                                                 Name = name
                                             });
                        }
                    }
                }
            }

            return keywords;
        }

        public void InsertKeywordsForOffer(string offerSha1, IEnumerable<Keyword> keywords)
        {
            if (keywords.Any())
            {

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    using (SqlCommand selectCommand = new SqlCommand("SELECT Id FROM RawOffers WHERE SHA1='" + offerSha1 + "'", connection))
                    {
                        int offerId = (int)selectCommand.ExecuteScalar();

                        if (offerId > 0)
                        {
                            using (SqlCommand insertCommand = new SqlCommand("", connection))
                            {
                                foreach (var keyword in keywords)
                                {
                                    insertCommand.CommandText += string.Format("INSERT INTO KeywordsInOffers (KeywordId, OfferId) Values ({0}, {1});", keyword.Id, offerId);
                                }

                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }



        private void AddOfferToQuery(Offer offer, SqlCommand command)
        {           
            command.Parameters.AddWithValue("@title", offer.Title);
            command.Parameters.AddWithValue("@text", offer.Text);
            command.Parameters.AddWithValue("@province", offer.Province);
            command.Parameters.AddWithValue("@city", offer.City);
            command.Parameters.AddWithValue("@sha1", offer.Sha1);
            command.Parameters.AddWithValue("@link", offer.Link);
            command.Parameters.AddWithValue("@date", offer.Date);
        }
    }
}