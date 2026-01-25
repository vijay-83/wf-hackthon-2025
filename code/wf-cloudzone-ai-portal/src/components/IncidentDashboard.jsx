import { useState, useEffect } from "react";
import { DataGrid } from "@mui/x-data-grid";
import "./IncidentDashboard.css";
import ChatBot from '../components/GenAIChatBoat';
import apiService from "../Service/ApiService";
import Uploader from "../components/Uploader";

function IncidentDashboard() {

    const [incidents, setIncidents] = useState([]);
    const [selectedStatus, setSelectedStatus] = useState("All");

    useEffect(() => {
        apiService.fetchIncidents(50)
            .then((data) => setIncidents(data.data))
            .catch((error) => console.error("Error fetching incidents:", error));
    }, []);


    const filteredIncidents = selectedStatus === "All" ? (incidents || []) :
        (incidents || []).filter((incident) => incident.status === selectedStatus);

    const columns = [
        { field: "id", headerName: "Issue ID", width: 100 },
        { field: "description", headerName: "Description", width: 250 },
        { field: "summary", headerName: "Summary", width: 200 },
        { field: "priority", headerName: "Priority", width: 120 },
        { field: "status", headerName: "Status", width: 120 },
    ];
    const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 10 });
    const total = incidents.length;
    const open = incidents.filter((i) => i.status === "Open").length;
    const resolved = incidents.filter((i) => i.status === "Resolved").length;
    const inProgress = incidents.filter((i) => i.status === "In Progress").length;

    return (
        <div className="dashboard-container">
            <h1 className="dashboard-title">📍 Incident Dashboard</h1>

            <div className="dashboard-row">
                {/* Left Column: Incident List */}
                <div className="dashboard-col incident-list-col">
                    <div className="incident-list">
                        <h2 className="section-title">📝 Incident List</h2>
                        <DataGrid
                            rows={filteredIncidents}
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
                            <option value="All">All</option>
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
                        <Uploader/>

                    </div>
                </div>
            </div>
        </div>

    );
}

export default IncidentDashboard;