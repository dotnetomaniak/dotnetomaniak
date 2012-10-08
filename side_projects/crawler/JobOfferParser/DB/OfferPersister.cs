using System;
using System.Configuration;
using System.Data;
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


                using (SqlCommand command = new SqlCommand("INSERT INTO RawOffers (ID, Text, SHA1, Link, Date) VALUES ()", connection))
                {
                    AddOfferToQuery(offer, command);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void AddOfferToQuery(Offer offer, SqlCommand command)
        {
            command.Parameters.AddWithValue("@title", offer.Title);
            command.Parameters.AddWithValue("@link", offer.Link);
        }
    }
}