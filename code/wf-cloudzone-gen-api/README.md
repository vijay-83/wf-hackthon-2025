
# QdrantRAGDemo (Mocked Embeddings)

This version uses randomly generated vectors instead of OpenAI embeddings. Great for local testing without internet or API keys.

## Setup

1. Run:
```bash
docker compose up --build
docker run -p 6333:6333 -p 6334:6334 qdrant/qdrant
```

2. Open Swagger at:
```
http://localhost:5000/swagger
```

## Endpoints

- `POST /api/index` - Index sample documents
- `GET /api/chat?question=...` - Ask a question using RAG

*Note: Embeddings are mocked, so search results are not semantically accurate.*
