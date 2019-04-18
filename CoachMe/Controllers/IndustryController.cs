using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Web.Http;

namespace CoachMe.Controllers
{
    // Define the IndustryVacancy class to store the value retrieved from the backend MySQL
    // Database and return it as JSON to the frontend
    public class IndustryVacancy
    {
        public string anzsic_code { get; set; }
        public string industry_level { get; set; }
        public string industry_name { get; set; }
        public string vacancy_2018 { get; set; }
        public string vacancy_2023 { get; set; }
        public string five_year_growth { get; set; }
        public string five_year_growth_percentage { get; set; }
        public string error { get; set; }

        public IndustryVacancy(string anzsic_code, string industry_level
            , string industry_name, string vacancy_2018, string vacancy_2023
            , string five_year_growth, string five_year_growth_percentage, string error)
        {
            this.anzsic_code = anzsic_code;
            this.industry_level = industry_level;
            this.industry_name = industry_name;
            this.vacancy_2018 = vacancy_2018;
            this.vacancy_2023 = vacancy_2023;
            this.five_year_growth = five_year_growth;
            this.five_year_growth_percentage = five_year_growth_percentage;
            this.error = error;
        }
    }

    // Define the IndustrySalary class to store the value retrieved from the backend MySQL
    // Database and return it as JSON to the frontend
    public class IndustrySalary
    {
        public string anzsic_code { get; set; }
        public string industry_level { get; set; }
        public string industry_name { get; set; }
        public string year { get; set; }
        public string earning { get; set; }
        public string error { get; set; }

        public IndustrySalary(string anzsic_code, string industry_level
            , string industry_name, string year, string earning, string error)
        {
            this.anzsic_code = anzsic_code;
            this.industry_level = industry_level;
            this.industry_name = industry_name;
            this.year = year;
            this.earning = earning;
            this.error = error;
        }
    }

    public class IndustryController : ApiController
    {
        // GET api/industry/vacancy/{industry_level}
        [Route("api/industry/vacancy/{industry_level}")]
        public List<IndustryVacancy> Get(string industry_level)
        {
            // Create connection for MySQL database
            MySqlConnection conn = WebApiConfig.conn();

            // Define the MySQL Statement to query all the industry vacancy details
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = "SELECT i.anzsic_code, i.industry_level, i.industry_name, " +
                                " iv.2018_vacancy, iv.2023_vacancy, iv.five_year_growth" +
                                ", iv.five_year_growth_percentage " +
                                "FROM industry i LEFT JOIN industry_vacancy iv " +
                                "ON i.anzsic_code = iv.anzsic_code " +
                                "WHERE i.industry_level = @industry_level;";

            // Define parameters for the MySQL Statement
            query.Parameters.AddWithValue("@industry_level", industry_level);

            // Use prefined Result class to store the values returned
            var results = new List<IndustryVacancy>();

            // Error Handling 
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new IndustryVacancy(null, null, null, null, null, null, null, ex.ToString()));
                results.Add(new IndustryVacancy(null, null, null, null, null, null, null, "No Connection!"));
            }

            // Define the fetch query
            MySqlDataReader fetch_query = query.ExecuteReader();

            // Parse the query result and store in the Result list
            while (fetch_query.Read())
            {
                results.Add(new IndustryVacancy(fetch_query["anzsic_code"].ToString()
                    , fetch_query["industry_level"].ToString()
                    , fetch_query["industry_name"].ToString()
                    , fetch_query["2018_vacancy"].ToString()
                    , fetch_query["2023_vacancy"].ToString()
                    , fetch_query["five_year_growth"].ToString()
                    , fetch_query["five_year_growth_percentage"].ToString()
                    , null));
            }

            // Return results for the GET api
            return results;
        }

        // GET api/industry/salary/
        [Route("api/industry/salary/")]
        public List<IndustrySalary> Get()
        {
            // Create connection for MySQL database
            MySqlConnection conn = WebApiConfig.conn();

            // Define the MySQL Statement to query all the industry salary details
            MySqlCommand query = conn.CreateCommand();
            query.CommandText = "SELECT i.anzsic_code, i.industry_level, i.industry_name, " +
                                " ie.year, ie.earning " +
                                "FROM industry i LEFT JOIN industry_earning ie " +
                                "ON i.anzsic_code = ie.anzsic_code " +
                                "WHERE i.industry_level = 1;";

            // Use prefined Result class to store the values returned
            var results = new List<IndustrySalary>();

            // Error Handling 
            try
            {
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                results.Add(new IndustrySalary(null, null, null, null, null, ex.ToString()));
                results.Add(new IndustrySalary(null, null, null, null, null, "No Connection!"));
            }

            // Define the fetch query
            MySqlDataReader fetch_query = query.ExecuteReader();

            // Parse the query result and store in the Result list
            while (fetch_query.Read())
            {
                results.Add(new IndustrySalary(fetch_query["anzsic_code"].ToString()
                    , fetch_query["industry_level"].ToString()
                    , fetch_query["industry_name"].ToString()
                    , fetch_query["year"].ToString()
                    , fetch_query["earning"].ToString()
                    , null));
            }

            // Return results for the GET api
            return results;
        }
    }
}