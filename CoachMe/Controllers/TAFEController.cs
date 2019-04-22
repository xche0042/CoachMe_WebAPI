using System.Collections.Generic;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace CoachMe.Controllers
{
    // Define the TAFEResult class to store the value retrieved from the backend MySQL
    // Database and return it as JSON to the frontend
    public class TAFEResult
    {
        public string occupation_code { get; set; }
        public string occupation_level { get; set; }
        public string occupation_name { get; set; }
        public string tafe_code { get; set; }
        public string tafe_name { get; set; }
        public string tafe_website { get; set; }
        public string five_year_growth_percentage { get; set; }
        public string salary_2018 { get; set; }
        public string error { get; set; }

        public TAFEResult(string occupation_code, string occupation_level
            , string occupation_name, string tafe_code, string tafe_name
            , string tafe_website, string five_year_growth_percentage
            , string salary_2018, string error)
        {
            this.occupation_code = occupation_code;
            this.occupation_level = occupation_level;
            this.occupation_name = occupation_name;
            this.tafe_code = tafe_code;
            this.tafe_name = tafe_name;
            this.tafe_website = tafe_website;
            this.five_year_growth_percentage = five_year_growth_percentage;
            this.salary_2018 = salary_2018;
            this.error = error;
        }
    }

    // Define the TAFEInstitute class to store the value retrieved from the backend MySQL
    // Database and return it as JSON to the frontend
    public class TAFEInstitute
    {
        public string tafe_code { get; set; }
        public string tafe_name { get; set; }
        public string tafe_website { get; set; }
        public string institute_name { get; set; }
        public string institute_address { get; set; }
        public string institute_lat { get; set; }
        public string institute_long { get; set; }
        public string error { get; set; }

        public TAFEInstitute(string tafe_code, string tafe_name
            , string tafe_website, string institute_name, string institute_address
            , string institute_lat, string institute_long, string error)
        {
            this.tafe_code = tafe_code;
            this.tafe_name = tafe_name;
            this.tafe_website = tafe_website;
            this.institute_name = institute_name;
            this.institute_address = institute_address;
            this.institute_lat = institute_lat;
            this.institute_long = institute_long;
            this.error = error;
        }
    }

    public class TAFEController : ApiController
    {
        // GET api/tafe/{occupation_code}
        [Route("api/tafe/{occupation_code}")]
        public List<TAFEResult> Get(string occupation_code)
        {
            // Create connection for MySQL database
            MySqlConnection conn = WebApiConfig.conn();

            // Define the MySQL Statement to query all the tafe details under a certain occupation
            // code (e.g. if occupation_code is 2, find all tafe courses under the code, such as
            // 21, 22, 211 & 213)
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = "SELECT o.occupation_code, o.occupation_level, " +
                                "o.occupation_name, t.tafe_code, t.tafe_name, " +
                                "t.tafe_website, oc.five_year_growth_percentage, oc.2018_salary " +
                                "FROM occupation o JOIN tafe_occupation tao " +
                                "ON o.occupation_code = tao.occupation_code " +
                                "JOIN tafe t ON tao.tafe_code = t.tafe_code " +
                                "JOIN occupation_vacancy oc ON o.occupation_code = oc.occupation_code " +
                                "WHERE o.occupation_code LIKE CONCAT(@occupation_code, '%');";

            // Define parameters for the MySQL Statement
            query.Parameters.AddWithValue("@occupation_code", occupation_code);

            // Use prefined Result class to store the values returned
            var results = new List<TAFEResult>();

            // Error Handling 
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new TAFEResult(null, null, null, null, null, null, null, null, ex.ToString()));
                results.Add(new TAFEResult(null, null, null, null, null, null, null, null, "No Connection!"));
            }

            // Define the fetch query
            MySqlDataReader fetch_query = query.ExecuteReader();

            // Parse the query result and store in the Result list
            while (fetch_query.Read())
            {
                results.Add(new TAFEResult(fetch_query["occupation_code"].ToString()
                    , fetch_query["occupation_level"].ToString()
                    , fetch_query["occupation_name"].ToString()
                    , fetch_query["tafe_code"].ToString()
                    , fetch_query["tafe_name"].ToString()
                    , fetch_query["tafe_website"].ToString()
                    , fetch_query["five_year_growth_percentage"].ToString()
                    , fetch_query["2018_salary"].ToString()
                    , null));
            }

            // Return results for the GET api
            return results;
        }

        // GET api/tafe/{institute}/{tafe_code}
        [Route("api/tafe/{institute}/{tafe_code}")]
        public List<TAFEInstitute> Get(string institute, string tafe_code)
        {
            // Create connection for MySQL database
            MySqlConnection conn = WebApiConfig.conn();

            // Define the MySQL Statement to query all the tafe institutes for a certain tafe code
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = "SELECT t.tafe_code, t.tafe_name, t.tafe_website, i.institute_name, " +
                                "i.institute_address, i.institute_lat, i.institute_long " +
                                "FROM tafe t JOIN tafe_institute ti " +
                                "ON t.tafe_code = ti.tafe_code " +
                                "JOIN institute i ON ti.institute_code = i.institute_code " +
                                "WHERE t.tafe_code = @tafe_code;";

            // Define parameters for the MySQL Statement
            query.Parameters.AddWithValue("@tafe_code", tafe_code);

            // Use prefined Result class to store the values returned
            var results = new List<TAFEInstitute>();

            // Error Handling 
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new TAFEInstitute(null, null, null, null, null, null, null, ex.ToString()));
                results.Add(new TAFEInstitute(null, null, null, null, null, null, null, "No Connection!"));
            }

            // Define the fetch query
            MySqlDataReader fetch_query = query.ExecuteReader();

            // Parse the query result and store in the Result list
            while (fetch_query.Read())
            {
                results.Add(new TAFEInstitute(fetch_query["tafe_code"].ToString()
                    , fetch_query["tafe_name"].ToString()
                    , fetch_query["tafe_website"].ToString()
                    , fetch_query["institute_name"].ToString()
                    , fetch_query["institute_address"].ToString()
                    , fetch_query["institute_lat"].ToString()
                    , fetch_query["institute_long"].ToString()
                    , null));
            }

            // Return results for the GET api
            return results;
        }
    }
}
