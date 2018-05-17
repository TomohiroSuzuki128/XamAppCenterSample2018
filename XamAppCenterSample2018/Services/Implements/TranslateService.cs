using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XamAppCenterSample2018.Services.Interfaces;
using XamAppCenterSample2018.Models;

namespace XamAppCenterSample2018.Services.Implements
{
    public class TranslateService : ITranslateService
    {
        static string host = "https://api.cognitive.microsofttranslator.com";
        static string path = "/translate?api-version=3.0";
        // Translate to English
        static string params_ = "&to=en";

        static string uri = host + path + params_;

        public async Task<string> Translate(string text)
        {
            var body = new System.Object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", Variables.ApiKey);

                var response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var deserialized = JsonConvert.DeserializeObject<List<TranslateResult>>(responseBody);
                    return deserialized.FirstOrDefault().Translations.FirstOrDefault().Text;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var deserialized = JsonConvert.DeserializeObject<ErrorResult>(responseBody);
                    return string.Format("エラーコード： {1}{0}{2}", Environment.NewLine, deserialized.Error.Code, deserialized.Error.Message);
                }
            }
        }

    }
}
