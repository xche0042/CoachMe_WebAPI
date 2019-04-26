using System;
using System.Collections.Generic;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace CoachMe.Controllers
{
    // Define the Resuls class to store the value retrieved from the backend MySQL
    // Database and return it as JSON to the frontend
    public class Results
    {
        public string suburb_name { get; set; }
        public string suburb_lat { get; set; }
        public string suburb_long { get; set; }
        public string median_price { get; set; }
        public string error { get; set; }

        public Results(string suburb_name, string suburb_lat
            , string suburb_long, string median_price, string error)
        {
            this.suburb_name = suburb_name;
            this.suburb_lat = suburb_lat;
            this.suburb_long = suburb_long;
            this.median_price = median_price;
            this.error = error;
        }
    }

    public class ValuesController : ApiController
    {
        // GET api/values/{median_price}
        [Route("api/values/{median_price}")]
        public List<Results> Get(string median_price)
        {
            // Create connection for MySQL database
            MySqlConnection conn = WebApiConfig.conn();

            // Parse the median_price from String to Integer
            int median_price_double = Int32.Parse(median_price);

            // Define the MySQL Statement to query all the areas within the price range
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = "SELECT s.suburb_name, s.suburb_lat, s.suburb_long, " +
                "AVG(NULLIF(pr.median_price, 0)) AS median_price " +
                "FROM suburb s JOIN suburb_property sp ON s.suburb_id = sp.suburb_id " +
                "JOIN property p ON p.property_id = sp.property_id " +
                "JOIN property_rental pr ON sp.suburb_property_id = pr.suburb_property_id " +
                "GROUP BY s.suburb_name, s.suburb_lat, s.suburb_long " +
                "HAVING median_price <= @median_price;";

            // Define parameters for the MySQL Statement
            query.Parameters.AddWithValue("@median_price", median_price_double);

            // Use prefined Result class to store the values returned
            var results = new List<Results>();

            // Error Handling 
            try {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) {
                results.Add(new Results(null, null, null, null, ex.ToString()));
                results.Add(new Results(null, null, null, null, "No Connection!"));
            }

            // Define the fetch query
            MySqlDataReader fetch_query = query.ExecuteReader();

            // Parse the query result and store in the Result list
            while (fetch_query.Read())
            {
                results.Add(new Results(fetch_query["suburb_name"].ToString()
                    , fetch_query["suburb_lat"].ToString()
                    , fetch_query["suburb_long"].ToString()
                    , fetch_query["median_price"].ToString()
                    , null));
            }

            // Return results for the GET api
            return results;
        }

        // GET api/values/
        [Route("api/values")]
        public List<Results> Get()
        {
            // Create connection for MySQL database
            MySqlConnection conn = WebApiConfig.conn();

            // Define the MySQL Statement to query all the areas' average rental price
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = "SELECT s.suburb_name, s.suburb_lat, s.suburb_long, " +
                "AVG(NULLIF(pr.median_price, 0)) AS median_price " +
                "FROM suburb s JOIN suburb_property sp ON s.suburb_id = sp.suburb_id " +
                "JOIN property p ON p.property_id = sp.property_id " +
                "JOIN property_rental pr ON sp.suburb_property_id = pr.suburb_property_id " +
                "GROUP BY s.suburb_name, s.suburb_lat, s.suburb_long;"; 

            // Use prefined Result class to store the values returned
            var results = new List<Results>();

            // Error Handling 
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new Results(null, null, null, null, ex.ToString()));
                results.Add(new Results(null, null, null, null, "No Connection!"));
            }

            // Define the fetch query
            MySqlDataReader fetch_query = query.ExecuteReader();

            // Parse the query result and store in the Result list
            while (fetch_query.Read())
            {
                results.Add(new Results(fetch_query["suburb_name"].ToString()
                    , fetch_query["suburb_lat"].ToString()
                    , fetch_query["suburb_long"].ToString()
                    , fetch_query["median_price"].ToString()
                    , null));
            }

            // Return results for the GET api
            return results;
        }
    }
}
