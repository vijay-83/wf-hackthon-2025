# wf-hackthon-2025
wf hackthon 20225
System Architecture
![image](https://github.com/user-attachments/assets/2609ae67-0f4f-42d8-9aee-eac92fc59914)


Vector Database Inetfcae for RAG
![ChatGPT Image May 24, 2025, 08_54_18 AM](https://github.com/user-attachments/assets/1c6230cc-447d-4a98-b513-c18622fae748)

Portal UI
![image](https://github.com/user-attachments/assets/abf1e87d-d442-4e19-97d2-a9aa96e8e507)
![image](https://github.com/user-attachments/assets/3bc4d2fa-afe7-43b9-9b55-cb04a3e4a0bc)

## 🧩 System Architecture (with Bug Fix Agent)

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
    E1[LangChain Summary Agent]
    E2[LangChain Suggestion Agent]
    E3[LangChain Bug Fix Agent]
    F[JIRA Service (future)]
    G[Auth Service (future)]
  end

  A -- "Get Incidents" --> B1
  A -- "Summarize Incident" --> B2
  A -- "Suggest Fixes" --> B2
  A -- "Generate Bug Fix" --> B2

  B1 -- GetIncidentsAsync --> C
  C -- JSON Incidents --> B1
  B1 -- IndexIncidents --> D
  D -- Vectors --> Qdrant[(Qdrant DB)]

  B2 -- POST summary --> E1
  B2 -- POST suggest --> E2
  B2 -- POST bugfix --> E3

  E1 -- Summary Text --> B2
  E2 -- Suggestions --> B2
  E3 -- Code Fix / Plan --> B2

  B2 -- (future) create Jira ticket --> F
  B -- (future) token auth --> G

  style Qdrant fill:#f9f,stroke:#333,stroke-width:2px
  style F fill:#9cf,stroke:#333,stroke-width:2px
  style E3 fill:#cfc,stroke:#333,stroke-width:2px




