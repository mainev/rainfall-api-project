using Newtonsoft.Json;
using RainfallApiProject.Models;
using System.IO;

namespace RainfallApiProject.Services
{
    public class RainfallService
    {
        public RainfallService() { }

        /// <summary>
        /// Returns a list of all readings from a particular station using the public API
        /// </summary>
        public async Task<RainfallReadingResponse> GetRainfallMeasuresAsync(int count, string stationId = "1491TH")
        {

            RainfallReadingResponse rainfallReadingResponse = new RainfallReadingResponse();
            string publicApiUrl = $"http://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/measures";

            using HttpClient httpClient = new HttpClient();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(publicApiUrl);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                RainfallMeasure rainfallMeasure = JsonConvert.DeserializeObject<RainfallMeasure>(jsonResponse);

                foreach (var item in rainfallMeasure.Items.Take(count))
                {
                    rainfallReadingResponse.Readings.Add(new RainfallReading
                    {
                        DateMeasured = item.LatestReading.DateTime,
                        AmountMeasured = item.LatestReading.Value
                    });
                }

                return rainfallReadingResponse;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Error while fetching data from API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException($"Error while deserializing JSON response: {ex.Message}", ex);
            }
        }

        class RainfallMeasure
        {
            public List<RainfallMeasureItem> Items { get; set; } = new List<RainfallMeasureItem>();
        }

        class RainfallMeasureItem
        {
            public LatestReading LatestReading { get; set; }
        }

        class LatestReading
        {
            public string Date { get; set; }
            public DateTime DateTime { get; set; }
            public string Measure { get; set; }
            public decimal Value { get; set; }
        }
    }
}
