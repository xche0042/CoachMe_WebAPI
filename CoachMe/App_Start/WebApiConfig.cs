using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace CoachMe
{
    public static class WebApiConfig
    {
        // Define the connection string for connecting the MySQL Database
        // Server name, port number, database name, username and password are required for the connection
        public static MySqlConnection conn()
        {
            String conn_string = "server=enso-coachme-db.c9k8cl289q8t.ap-southeast-2.rds.amazonaws.com;port =3306;database=coachmedb;username=enso_coachme;password=ensohd123456;";
            MySqlConnection conn = new MySqlConnection(conn_string);

            return conn;
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "GetRentalPriceApi",
                routeTemplate: "api/{controller}/{property_type}/{median_price}"
            );

            var appXmlType = config.Formatters.XmlFormatter
                .SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            config.Formatters.JsonFormatter
                .SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("text/html"));
        }
    }
}