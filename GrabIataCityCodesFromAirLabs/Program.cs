using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GrabIataCityCodesFromAirLabs
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient webClient = new HttpClient();
            string uri = "https://airlabs.co/api/v6/cities?api_key=094caf8e-1acb-492e-8da6-b0d5ad8732ff";
            HttpResponseMessage httpResponseMessage= await webClient.GetAsync(uri);
            
            string responseString = string.Empty;
            if (httpResponseMessage.IsSuccessStatusCode)
                responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(responseString))
            {
                AirLabData oAirLabData = JsonConvert.DeserializeObject<AirLabData>(responseString);
                StringBuilder sb = new StringBuilder();
                if(oAirLabData != null)
                {
                    foreach (_Response response in oAirLabData.response)
                    {
                        sb.AppendLine($"INSERT INTO [dbo].[IataCityCodes] ([iataCode], [CityName], [CountryCode]) VALUES ('{response.code}', '{response.name.Replace("'","''")}', '{response.country_code}');");
                    }

                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "iataCityCodes.sql", sb.ToString());
                }
            }
            Console.ReadKey();
        }
    }

    public class AirLabData
    {
        public List<_Response> response { get; set; }
    }

    public class _Response
    {
        public string code { get; set; }
        public string country_code { get; set; }
        public string name { get; set; }
    }
}





