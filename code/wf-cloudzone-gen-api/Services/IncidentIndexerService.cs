using Qdrant.Client;
using Qdrant.Client.Grpc;
using QdrantRAGDemo.API.Controllers;
using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.Models;
using wf_cloudzone_gen_api.helper;
using System.Linq;

namespace wf_cloudzone_gen_api.Services
{
    public class IncidentIndexerService : IIncidentIndexerService
    {
        private readonly QdrantClient _qdrant;
        private readonly IEmbeddingService _embedding;

        public IncidentIndexerService(QdrantClient qdrant, IEmbeddingService embedding)
        {
            _qdrant = qdrant;
            _embedding = embedding;
        }

        public async Task IndexIncidentAsync(Incident incident)
        {
            var text = FormatIncident(incident);
            var vector = await _embedding.GetVectorAsync(text);

            var point = new PointStruct
            {
                Id = new PointId { Uuid = Guid.NewGuid().ToString() },
                Vectors = new Vectors { Vector = new Vector { Data = { vector } } },
                Payload =
                {
                        { "id", new Value { StringValue = incident.Id.ToString() } },
                        { "summary", new Value { StringValue = incident.Summary } },
                        { "description", new Value { StringValue = incident.Description } },
                        { "status", new Value { StringValue = incident.Status } },
                        { "priority", new Value { StringValue = incident.Priority } },
                        { "assignee", new Value { StringValue = incident.Assignee ?? "" } },
                        {
                            "labels",
                            new Value
                            {
                                ListValue = new ListValue
                                {
                                    Values = { incident.Labels.Select(l => new Value { StringValue = l }) }
                                }
                            }
                        }
                }
            };

            await EnsureIncidentIndexAsync();

            await _qdrant.UpsertAsync("incident-index", new[] { point });
        }

        public async Task IndexManyAsync(IEnumerable<Incident> incidents)
        {
            var points = new List<PointStruct>();

            foreach (var incident in incidents)
            {
                var text = FormatIncident(incident);
                var vectorData = await _embedding.GetVectorAsync(text);

                var point = new PointStruct
                {
                    Id = new PointId { Uuid = Guid.NewGuid().ToString() },
                    Vectors = new Vectors
                    {
                        Vector = new Vector { Data = { vectorData } }
                    },
                    Payload =
                    {
                        { "id", new Value { StringValue = incident.Id.ToString() } },
                        { "summary", new Value { StringValue = incident.Summary } },
                        { "description", new Value { StringValue = incident.Description } },
                        { "status", new Value { StringValue = incident.Status } },
                        { "priority", new Value { StringValue = incident.Priority } },
                        { "assignee", new Value { StringValue = incident.Assignee ?? "" } },
                        {
                            "labels",
                            new Value
                            {
                                ListValue = new ListValue
                                {
                                    Values = { incident.Labels.Select(label => new Value { StringValue = label }) }
                                }
                            }
                        }
                    }
                };

                points.Add(point);
            }
            await EnsureIncidentIndexAsync();
            await _qdrant.UpsertAsync("incident-index", points);
        }

        private string FormatIncident(Incident incident)
        {
            return $"[{incident.Id}] {incident.Summary}. {incident.Description} Status: {incident.Status} | Priority: {incident.Priority} | Labels: {string.Join(", ", incident.Labels)} | Assignee: {incident.Assignee}";
        }

        public async Task EnsureIncidentIndexAsync()
        {
            var exists = await _qdrant.CollectionExistsAsync("incident-index");
            if (!exists)
            {
                await _qdrant.CreateCollectionAsync("incident-index", new VectorParams
                {
                    Size = 1536,
                    Distance = Distance.Cosine
                });
            }
        }
    }

}
