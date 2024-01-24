using Newtonsoft.Json;
using RainfallApiProject.Models;
using System.IO;

namespace RainfallApiProject.Services
{
    public class RainfallService
    {

        public RainfallService() { }

        public async Task<RainfallReadingResponse> GetRainfallMeasuresAsync(string stationId = "1491TH")
        {

            RainfallReadingResponse rainfallReadingResponse = new RainfallReadingResponse();
            string url = @$"http://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/measures";


            using HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();

            RainfallMeasure rainfallMeasure = JsonConvert.DeserializeObject<RainfallMeasure>(jsonResponse);

            foreach (var item in rainfallMeasure.Items)
            {
                rainfallReadingResponse.Readings.Add(new RainfallReading
                {
                    DateMeasured = item.LatestReading.DateTime,
                    AmountMeasured = item.LatestReading.Value
                });
            }

            return rainfallReadingResponse;
        }

        public class RainfallMeasure
        {
            public List<RainfallMeasureItem> Items { get; set; }
        }

        public class RainfallMeasureItem
        {
            public LatestReading LatestReading { get; set; }
        }


        public class LatestReading
        {
            public string Date { get; set; }
            public DateTime DateTime { get; set; }
            public string Measure { get; set; }
            public decimal Value { get; set; }
        }
    }
}
