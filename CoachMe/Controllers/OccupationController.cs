using System.Collections.Generic;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace CoachMe.Controllers
{
    // Define the OccupationResult class to store the value retrieved from the backend MySQL
    // Database and return it as JSON to the frontend
    public class OccupationResult
    {
        public string occupation_code { get; set; }
        public string occupation_level { get; set; }
        public string occupation_name { get; set; }
        public string vacancy_2018 { get; set; }
        public string vacancy_2023 { get; set; }
        public string five_year_growth { get; set; }
        public string five_year_growth_percentage { get; set; }
        public string salary_2018 { get; set; }
        public string has_tafe { get; set; }
        public string error { get; set; }

        public OccupationResult(string occupation_code, string occupation_level
            , string occupation_name, string vacancy_2018, string vacancy_2023
            , string five_year_growth, string five_year_growth_percentage
            , string salary_2018, string has_tafe, string error)
        {
            this.occupation_code = occupation_code;
            this.occupation_level = occupation_level;
            this.occupation_name = occupation_name;
            this.vacancy_2018 = vacancy_2018;
            this.vacancy_2023 = vacancy_2023;
            this.five_year_growth = five_year_growth;
            this.five_year_growth_percentage = five_year_growth_percentage;
            this.salary_2018 = salary_2018;
            this.has_tafe = has_tafe;
            this.error = error;
        }
    }

    public class OccupationController : ApiController
    {
        // GET api/occupation/{occupation_level}
        [Route("api/occupation/{occupation_level}")]
        public List<OccupationResult> Get(string occupation_level)
        {
            // Create connection for MySQL database
            MySqlConnection conn = WebApiConfig.conn();

            // Define the MySQL Statement to query all the level one occupation details
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = "SELECT o.occupation_code, o.occupation_level, " +
                                "o.occupation_name, oc.2018_vacancy, oc.2023_vacancy, " +
                                "oc.five_year_growth, oc.five_year_growth_percentage, " +
                                "oc.2018_salary, o.has_tafe " +
                                "FROM occupation o JOIN occupation_vacancy oc " +
                                "ON o.occupation_code = oc.occupation_code " +
                                "WHERE o.occupation_level = @occupation_level;";

            // Define parameters for the MySQL Statement
            query.Parameters.AddWithValue("@occupation_level", occupation_level);

            // Use prefined Result class to store the values returned
            var results = new List<OccupationResult>();

            // Error Handling 
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new OccupationResult(null, null, null, null, null, null, null, null, null, ex.ToString()));
                results.Add(new OccupationResult(null, null, null, null, null, null, null, null, null, "No Connection!"));
            }

            // Define the fetch query
            MySqlDataReader fetch_query = query.ExecuteReader();

            // Parse the query result and store in the Result list
            while (fetch_query.Read())
            {
                results.Add(new OccupationResult(fetch_query["occupation_code"].ToString()
                    , fetch_query["occupation_level"].ToString()
                    , fetch_query["occupation_name"].ToString()
                    , fetch_query["2018_vacancy"].ToString()
                    , fetch_query["2023_vacancy"].ToString()
                    , fetch_query["five_year_growth"].ToString()
                    , fetch_query["five_year_growth_percentage"].ToString()
                    , fetch_query["2018_salary"].ToString()
                    , fetch_query["has_tafe"].ToString()
                    , null));
            }

            // Return results for the GET api
            return results;
        }

        // GET api/occupation
        [Route("api/occupation")]
        public List<OccupationResult> Get()
        {
            // Create connection for MySQL database
            MySqlConnection conn = WebApiConfig.conn();

            // Define the MySQL Statement to query all the occupation details
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = "SELECT o.occupation_code, o.occupation_level, " +
                                "o.occupation_name, oc.2018_vacancy, oc.2023_vacancy, " +
                                "oc.five_year_growth, oc.five_year_growth_percentage, " +
                                "oc.2018_salary, o.has_tafe " +
                                "FROM occupation o JOIN occupation_vacancy oc " +
                                "ON o.occupation_code = oc.occupation_code;";

            // Use prefined Result class to store the values returned
            var results = new List<OccupationResult>();

            // Error Handling 
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new OccupationResult(null, null, null, null, null, null, null, null, null, ex.ToString()));
                results.Add(new OccupationResult(null, null, null, null, null, null, null, null, null, "No Connection!"));
            }

            // Define the fetch query
            MySqlDataReader fetch_query = query.ExecuteReader();

            // Parse the query result and store in the Result list
            while (fetch_query.Read())
            {
                results.Add(new OccupationResult(fetch_query["occupation_code"].ToString()
                    , fetch_query["occupation_level"].ToString()
                    , fetch_query["occupation_name"].ToString()
                    , fetch_query["2018_vacancy"].ToString()
                    , fetch_query["2023_vacancy"].ToString()
                    , fetch_query["five_year_growth"].ToString()
                    , fetch_query["five_year_growth_percentage"].ToString()
                    , fetch_query["2018_salary"].ToString()
                    , fetch_query["has_tafe"].ToString()
                    , null));
            }

            // Return results for the GET api
            return results;
        }

        // GET api/occupation/{occupation_level}/{occupation_parent}
        [Route("api/occupation/{occupation_level}/{occupation_parent}")]
        public List<OccupationResult> Get(string occupation_level, string occupation_parent)
        {
            // Create connection for MySQL database
            MySqlConnection conn = WebApiConfig.conn();

            // Define the MySQL Statement to query all the occupation details under a 
            // certain occupation parent (e.g. if occupation_parent is 21, find all child
            // occupations, such as 211 & 213)
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = "SELECT o.occupation_code, o.occupation_level, " +
                                "o.occupation_name, oc.2018_vacancy, oc.2023_vacancy, " +
                                "oc.five_year_growth, oc.five_year_growth_percentage, " +
                                "oc.2018_salary, o.has_tafe " +
                                "FROM occupation o JOIN occupation_vacancy oc " +
                                "ON o.occupation_code = oc.occupation_code " +
                                "WHERE o.occupation_code LIKE CONCAT(@occupation_parent, '_');";

            // Define parameters for the MySQL Statement
            query.Parameters.AddWithValue("@occupation_parent", occupation_parent);

            // Use prefined Result class to store the values returned
            var results = new List<OccupationResult>();

            // Error Handling 
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new OccupationResult(null, null, null, null, null, null, null, null, null, ex.ToString()));
                results.Add(new OccupationResult(null, null, null, null, null, null, null, null, null, "No Connection!"));
            }

            // Define the fetch query
            MySqlDataReader fetch_query = query.ExecuteReader();

            // Parse the query result and store in the Result list
            while (fetch_query.Read())
            {
                results.Add(new OccupationResult(fetch_query["occupation_code"].ToString()
                    , fetch_query["occupation_level"].ToString()
                    , fetch_query["occupation_name"].ToString()
                    , fetch_query["2018_vacancy"].ToString()
                    , fetch_query["2023_vacancy"].ToString()
                    , fetch_query["five_year_growth"].ToString()
                    , fetch_query["five_year_growth_percentage"].ToString()
                    , fetch_query["2018_salary"].ToString()
                    , fetch_query["has_tafe"].ToString()
                    , null));
            }

            // Return results for the GET api
            return results;
        }
    }
}
