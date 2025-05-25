# wf-hackthon-2025
wf hackthon 20225
System Architecture
![image](https://github.com/user-attachments/assets/2609ae67-0f4f-42d8-9aee-eac92fc59914)


Vector Database Inetfcae for RAG
![ChatGPT Image May 24, 2025, 08_54_18 AM](https://github.com/user-attachments/assets/1c6230cc-447d-4a98-b513-c18622fae748)

Portal UI
![image](https://github.com/user-attachments/assets/abf1e87d-d442-4e19-97d2-a9aa96e8e507)
![image](https://github.com/user-attachments/assets/3bc4d2fa-afe7-43b9-9b55-cb04a3e4a0bc)

## 🧩 GEN AI API Flow

```mermaid
flowchart LR
  subgraph Frontend
    A[Vite React Chatbot]
  end

  subgraph API
    B[.NET Core Web API]
    B1[IncidentController]
    B2[AgentController]
  end

  subgraph Services
    C[Mock ServiceNow Client]
    D[Qdrant Vector Store]
    E[LangChain Agent]
    F[JIRA Service]
    G[Auth Service]
  end

  A -- HTTPS JSON --> B
  B1 -- GetIncidentsAsync --> C
  C -- JSON List<IncidentDto> --> B1
  B1 -- IndexIncidentAsync --> D
  D -- upsert vectors --> Qdrant[(Qdrant DB)]
  A -- “summarize” / “suggest” --> B2
  B2 -- POST summary/request --> E
  E -- suggestion/summary --> B2
  B2 -- (future) create Jira ticket --> F
  B -- (future) token --> G

  style Qdrant fill:#f9f,stroke:#333,stroke-width:2px
  style JIRA Service fill:#9cf,stroke:#333,stroke-width:2px



