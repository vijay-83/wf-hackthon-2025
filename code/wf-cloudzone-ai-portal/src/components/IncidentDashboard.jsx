import { useState, useEffect } from "react";
import { DataGrid } from "@mui/x-data-grid";
import "./IncidentDashboard.css";
import ChatBot from '../components/GenAIChatBoat';
function IncidentDashboard() {

    const [issues, setIssues] = useState([]);
    const [selectedStatus, setSelectedStatus] = useState("All");

    useEffect(() => {
        fetch("/data.json")
            .then(async (response) => {
                if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
                const contentType = response.headers.get("content-type");
                if (!contentType.includes("application/json")) {
                    throw new Error(`Expected JSON, but got: ${contentType}`);
                }
                return response.json();
            })
            .then((data) => setIssues(data))
            .catch((error) => console.error("Error fetching JSON:", error));
    }, []);

    //// Filter incidents based on status
    const filteredIssues = issues.filter((issue) =>
        selectedStatus === "All" ? true : issue.status === selectedStatus
    );

    const columns = [
        { field: "id", headerName: "Issue ID", width: 100 },
        { field: "description", headerName: "Description", width: 250 },
        { field: "summary", headerName: "Summary", width: 200 },
        { field: "priority", headerName: "Priority", width: 120 },
        { field: "status", headerName: "Status", width: 120 },
    ];
    const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 10 });
    const total = issues.length;
    const open = issues.filter((i) => i.status === "Open").length;
    const resolved = issues.filter((i) => i.status === "Resolved").length;
    const inProgress = issues.filter((i) => i.status === "In Progress").length;

    return (
        <div className="dashboard-container">
            <h1 className="dashboard-title">📍 Incident Dashboard</h1>

            <div className="dashboard-row">
                {/* Left Column: Incident List */}
                <div className="dashboard-col incident-list-col">
                    <div className="incident-list">
                        <h2 className="section-title">📝 Incident List</h2>
                        <DataGrid
                            rows={filteredIssues}
                            columns={columns}
                            getRowId={(row) => row.id}
                            pagination
                            paginationModel={paginationModel}
                            onPaginationModelChange={(newModel) => setPaginationModel(newModel)}
                        />
                    </div>
                </div>

                {/* Right Column: Summary + Filter + Chat */}
                <div className="dashboard-col summary-chat-col">
                    <div className="filter-section">
                        <label htmlFor="statusFilter">🔍 Filter by Status</label>
                        <select
                            id="statusFilter"
                            value={selectedStatus}
                            onChange={(e) => setSelectedStatus(e.target.value)}
                        >
                            <option value="">All</option>
                            <option value="Open">Open</option>
                            <option value="In Progress">In Progress</option>
                            <option value="Resolved">Resolved</option>
                            <option value="Closed">Closed</option>
                        </select>
                    </div>

                    <div className="incident-summary">
                        <h2 className="section-title">📊 Incident Summary</h2>
                        <div className="incident-item">
                            <span className="label">🧮 Total</span>
                            <span className="value total">{total}</span>
                        </div>
                        <div className="incident-item">
                            <span className="label">🚨 Open</span>
                            <span className="value open">{open}</span>
                        </div>
                        <div className="incident-item">
                            <span className="label">✅ Resolved</span>
                            <span className="value resolved">{resolved}</span>
                        </div>
                        <div className="incident-item">
                            <span className="label">🔄 In Progress</span>
                            <span className="value in-progress">{inProgress}</span>
                        </div>
                    </div>

                    <div className="chatbot-wrapper">
                        <h2 className="section-title">🤖 Platform Support Virtual Assistant</h2>
                        <ChatBot />
                    </div>
                </div>
            </div>
        </div>

    );
}

export default IncidentDashboard;