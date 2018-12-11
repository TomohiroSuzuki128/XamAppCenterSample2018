using System.Collections.Generic;
using Newtonsoft.Json;

namespace XamAppCenterSample2018.Models
{

    //[
    //  {
    //    "detectedLanguage": {
    //      "language": "ja",
    //      "score": 1.0
    //    },
    //    "translations": [
    //      {
    //        "text": "I'm sleepy",
    //        "to": "en"
    //      }
    //    ]
    //  }
    //]

    [JsonObject("translateResult")]
    public class TranslateResult
    {
        [JsonProperty("detectedLanguage")]
        public DetectedLanguage DetectedLanguage { get; set; }

        [JsonProperty("translations")]
        public List<Translation> Translations { get; set; }
    }

    public class DetectedLanguage
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class Translation
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }

}
