# wf-hackthon-2025

## 🧩 System Architecture
![image](https://github.com/user-attachments/assets/2609ae67-0f4f-42d8-9aee-eac92fc59914)

## 🧩Vector Database Interface for RAG
![api-interface](https://github.com/user-attachments/assets/0523a36d-6ed2-4525-99c8-8405697f0da5)

## 🧩 System Architecture (Full Interaction Flow with Vector Sync)

```mermaid
flowchart LR
  %% FRONTEND
  subgraph Frontend
    A[Vite React Chatbot]
  end

  %% API
  subgraph API
    B[.NET Core Web API]
    B1[IncidentController]
    B2[AgentController]
  end

  %% SERVICES
  subgraph Services
    C[Mock ServiceNow Client]
    D[Qdrant Vector Store]
    E1[LangChain Summary Agent]
    E2[LangChain Suggestion Agent]
    E3[LangChain Bug Fix Agent]
    F[JIRA Service]
    G[Auth Service ]
  end

  %% FRONTEND → API
  A -- "🔴 Incident Query" --> B1
  A -- "🟠 Summarize" --> B2
  A -- "🟡 Suggest Fixes" --> B2
  A -- "🟢 Bug Fix Agent" --> B2

  %% INCIDENT FLOW
  B1 -- "🔴 GetIncidentsAsync" --> C
  C -- "🟣 JSON Incidents" --> B1
  B1 -- "🟡 IndexIncidents" --> D
  D -- "🟣 Vectors" --> Qdrant[(Qdrant DB)]

  %% AGENT REQUESTS
  B2 -- "🟠 summary req" --> E1
  B2 -- "🟡 suggest req" --> E2
  B2 -- "🟢 bugfix req" --> E3

  %% AGENT RESPONSES
  E1 -- "🟠 summary" --> B2
  E2 -- "🟡 suggestions" --> B2
  E3 -- "🟢 fix/code" --> B2

  %% RE-INDEXING VECTOR STORE
  E2 -- "🔁 reindex suggestions" --> D
  E3 -- "🔁 reindex fixes" --> D

  %% API → CHATBOT RESPONSES
  B1 -- "📤 Incidents Response" --> A
  B2 -- "📤 Summary/Assist/Fix" --> A

  %% FUTURE
  B2 -- "🔵 create JIRA ticket" --> F
  B -- "🔒 auth token" --> G

  %% STYLES
  style Qdrant fill:#f9f,stroke:#333,stroke-width:2px
  style F fill:#9cf,stroke:#333,stroke-width:2px
  style E3 fill:#cfc,stroke:#333,stroke-width:2px
```

## 🧩 Portal UI Development 
**🟢Demo Work Completion** : 50%  **🟡Actual Integration Pending** : 30%   **🟠Business Logic and Bug Fix** : 20%  
![image](https://github.com/user-attachments/assets/abf1e87d-d442-4e19-97d2-a9aa96e8e507)
![image](https://github.com/user-attachments/assets/3bc4d2fa-afe7-43b9-9b55-cb04a3e4a0bc)


## 🧩 API Inetrface Development
**🟢Demo Work Completion** : 20%  **🔵Demo Work Inprogres** : 30%  **🟡Actual Integration Pending** : 30%   **🟠Business Logic and Bug Fix** : 20% 
![image](https://github.com/user-attachments/assets/9524f2de-cb1d-41a1-b5b7-0a8a04bfaa6c)

