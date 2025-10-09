using DocumentFormat.OpenXml.Bibliography;

using Microsoft.Extensions.Configuration;

using Repositories.Services.ArcGis.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repositories.Services.ArcGis
{
    public class ArcGisGeocodingService: IArcGisGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ArcGisGeocodingService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["ArcGIS:ApiKey"];
        }

        //public async Task<string> GetTokenAsync()
        //{
        //    var values = new Dictionary<string, string>
        //                {
        //                    { "client_id", "4MlIrI8cCIIqY2vD" },
        //                    { "client_secret", "1f9cc607e0424cfba02b10e316c3b4b2" },
        //                    { "grant_type", "client_credentials" }
        //                };

        //    var content = new FormUrlEncodedContent(values);
        //    var response = await _httpClient.PostAsync("https://www.arcgis.com/sharing/rest/oauth2/token", content);

        //    response.EnsureSuccessStatusCode();

        //    var json = await response.Content.ReadAsStringAsync();
        //    using var doc = JsonDocument.Parse(json);
        //    return doc.RootElement.GetProperty("access_token").GetString()!;
        //}


        // Autocomplete suggestions
        public async Task<List<string>> GetSuggestionsAsync(string text)
        {
            var url = $"https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/suggest" +
                      $"?f=json&text={Uri.EscapeDataString(text)}&maxSuggestions=5&apiKey={_apiKey}";

            var response = await _httpClient.GetStringAsync(url);

            using var doc = JsonDocument.Parse(response);
            var results = new List<string>();

            if (doc.RootElement.TryGetProperty("suggestions", out var arr))
            {
                foreach (var s in arr.EnumerateArray())
                {
                    results.Add(s.GetProperty("text").GetString()!);
                }
            }
            else if (doc.RootElement.TryGetProperty("error", out var err))
            {
                throw new Exception($"ArcGIS error: {err}");
            }

            return results;
        }


        // Convert a suggestion to lat/lon
        public async Task<(double lat, double lon, string address)?> GetCoordinatesAsync(string magicKey)
        {
            var url = $"https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/findAddressCandidates" +
                      $"?f=json&magicKey={Uri.EscapeDataString(magicKey)}&outFields=Match_addr&apiKey={_apiKey}";

            var response = await _httpClient.GetStringAsync(url);
            using var doc = JsonDocument.Parse(response);

            if (!doc.RootElement.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
                return null; // no candidates found

            var first = candidates[0];

            // Try getting location safely
            if (!first.TryGetProperty("location", out var location))
                return null; // invalid response

            double lat = 0, lon = 0;
            string address = string.Empty;

            if (location.TryGetProperty("y", out var yProp))
                lat = yProp.GetDouble();

            if (location.TryGetProperty("x", out var xProp))
                lon = xProp.GetDouble();

            if (first.TryGetProperty("address", out var addrProp))
                address = addrProp.GetString() ?? string.Empty;

            return (lat, lon, address);
        }

        public async Task<List<(string Text, string MagicKey)>> GetSuggestionsAsyncWithMagicKey(string text)
        {
            var suggestionsUrl = $"https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/suggest" +
                                 $"?f=json&text={Uri.EscapeDataString(text)}&maxSuggestions=5&apiKey={_apiKey}";

            var suggestionsResponse = await _httpClient.GetStringAsync(suggestionsUrl);
            using var doc = JsonDocument.Parse(suggestionsResponse);

            var results = new List<(string Text, string MagicKey)>();

            if (doc.RootElement.TryGetProperty("suggestions", out var arr))
            {
                foreach (var s in arr.EnumerateArray())
                {
                    var suggestionText = s.GetProperty("text").GetString();
                    var magicKey = s.GetProperty("magicKey").GetString();

                    results.Add((suggestionText!, magicKey!));
                }
            }
            else if (doc.RootElement.TryGetProperty("error", out var err))
            {
                throw new Exception($"ArcGIS error: {err}");
            }

            return results;
        }

    }
}
