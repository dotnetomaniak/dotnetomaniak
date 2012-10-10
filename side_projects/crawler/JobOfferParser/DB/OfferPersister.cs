using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using JobOfferParser.Data;
using System.Data.SqlClient;

namespace JobOfferParser.DB
{
    public class OfferPersister : IOfferPersister
    {
        private readonly string _connectionString;

        public OfferPersister(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            _connectionString = connectionString;
        }

        public void Persist(Offer offer)
        {
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