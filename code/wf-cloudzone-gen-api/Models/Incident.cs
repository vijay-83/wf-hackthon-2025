namespace wf_cloudzone_gen_api.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Incident
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("priority")]
        public string Priority { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; } = new List<string>();

        [JsonPropertyName("assignee")]
        public string Assignee { get; set; } = string.Empty;

        [JsonPropertyName("payload")]
        public Dictionary<string, string> Payload { get; set; } = new Dictionary<string, string>();
    }
}
