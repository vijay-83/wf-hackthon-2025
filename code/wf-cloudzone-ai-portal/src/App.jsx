import axios from "axios";
import { useState } from "react";
import { FaCloudUploadAlt } from "react-icons/fa";
import { BsCloudDownloadFill } from "react-icons/bs";
import Card from "./components/card";

function App() {
    const experiences = ["1", "2", "3", "4", "5+"];
    const skills = ["Cloud", "GCP", "Database", "React", "Next.js", "Node.js", "Figma"];
    //const apiEndpoint = "http://127.0.0.1:8000/"
    const apiEndpoint = "https://cloudzoneresumeapi.azurewebsites.net"

    // Mock incident data
    const mockIncidentData = [
        { id: 1, status: "Open", description: "High CPU usage on server X", priority: "Critical", team: "DevOps" },
        { id: 2, status: "Resolved", description: "Database connection timeout", priority: "High", team: "Database Team" },
        { id: 3, status: "In Progress", description: "API latency above threshold", priority: "Medium", team: "Backend Team" },
        { id: 4, status: "Open", description: "Memory leak detected", priority: "Critical", team: "DevOps" },
    ];



    // Predefined chatbot responses
    const mockChatResponses = {
        "hello": "Hi there! How can I assist you today?",
        "recommendation": "Based on the incident, you might want to scale your servers or check for code inefficiencies.",
        "resolution": "I suggest restarting the affected services and verifying logs for errors.",
        "default": "I'm sorry, I didn't understand that. Could you please rephrase?"
    };


    const [selectedStatus, setSelectedStatus] = useState("All");
    const [chatMessages, setChatMessages] = useState([]);
    const [userInput, setUserInput] = useState("");

    // Filter incidents based on status
    const filteredIncidents = mockIncidentData.filter((incident) =>
        selectedStatus === "All" ? true : incident.status === selectedStatus
    );

    // Handle chat messages
    const handleSendMessage = () => {
        const userMessage = userInput.trim().toLowerCase();
        const botResponse = mockChatResponses[userMessage] || mockChatResponses["default"];
        setChatMessages([...chatMessages, { sender: "User", text: userInput }, { sender: "Bot", text: botResponse }]);
        setUserInput(""); // Clear input after sending
    };



    const [userdata, setUserData] = useState([]);
    const [initial, setInitial] = useState(true);
    const [loading, setLoading] = useState(false);

    const [candidates, setCandidates] = useState([]);
    const [additionalFilter, setAdditionalFilter] = useState([]);
    const [URL, setURL] = useState("storageURl");

    const add = async (mail) => {
        const ar = [...candidates, mail];
        console.log(ar, mail, "Sds");
        await setCandidates(ar);
        console.log(candidates, "Sd");
    };

    const remove = (id) => {
        const removed = candidates.filter((item) => {
            return item != id;
        });
        setCandidates([...removed]);
    };

    const [selected, setSelected] = useState({
        experience: [],
        institute: [],
        skills: [],
        languages: [],
    });

    // Options Handling

    const handleExperience = (item) => {
        if (selected.experience.includes(item)) {
            setSelected({
                ...selected,
                experience: selected.experience.filter((element) => element !== item),
            });
        } else {
            setSelected({
                ...selected,
                experience: [...selected.experience, item],
            });
        }
        console.log(selected);
    };


    const handleSkills = (item) => {
        console.log(item.target);
        if (selected.skills.includes(item)) {
            setSelected({
                ...selected,
                skills: selected.skills.filter((element) => element !== item),
            });
        } else {
            setSelected({
                ...selected,
                skills: [...selected.skills, item],
            });
        }
        console.log(selected);
    };


    const getData = async () => {
        console.log("Loading");
        fetch(apiEndpoint)
            .then((response) => response.json())
            .then((data) => {
                console.log(data);
                setLoading(true);
            });
    };

    const searchData = () => {
        let work = "";
        if (selected.experience.length > 0) {
            selected.experience.forEach((item) => (item + " or "));
            work += "\n ";
        }

        let skill = "";
        console.log(selected.skills);
        if (selected.skills.length > 0) {
            selected.skills.forEach((item) => (item + " and "));
            skill += "\n ";
        }

        let stringd = "";
        let k = 0;
        [additionalFilter, work, skill].forEach((item, i) => {
            if (item) {
                stringd += `${++k}.` + item;
            }
        });

        if (!stringd) {
            stringd = "The resume has atleast one character";
        }
        console.log(stringd);
        postData(stringd);
    };

    const postData = (promt) => {
        if (URL) {
        setLoading(true);
        const url = "";
        const split = url.split("/");
        const code = split[split.length - 1].split("?")[0];
        const query = {
            url: code,
            prompt: promt,
        };
        console.log(query);
        axios({
            method: "post",
            url: apiEndpoint+"/mockSerarch",
            data: query,
        }).then((e) => {
            let arr = [];
            for (let i = 0; i < Object.keys(e.data).length; i++) {
                console.log(e.data[i]); ``
                arr.push(e.data[i]);
            }
            //arr = parseObject(arr);
            filterData(arr);
            console.log("Parsed Data");
            console.log(arr);
        });
        }
    };

    const parseObject = (data) => {
        let arr = [];
        data.forEach((item) => {
            if (item) {
                let str = item;
                console.log(str);
                let obj = {};
                // remove '\n' from the string
                str = str.replace(/\\n/g, "");
                // remove '\' from the string
                str = str.replace(/\\/g, "");
                // replace 'False' with false
                str = str.replace(/False/g, "false");
                // replace 'True' with true
                str = str.replace(/True/g, "true");

                obj = JSON.parse(str);
                arr.push(obj);
            }
        });
        return arr;
    };

    const filterData = (data) => {
        let arr = [];
        let flag = true;
        data.forEach((item) => {
            const keys = Object.keys(item);
            keys.forEach((key) => {
                console.log(item[key], "key");
                if (!key.includes("name") && !key.includes("email") && !key.includes("score") && !key.includes("path")) {
                    if (!item[key]) {
                        flag = false;
                    }
                }
            });

            if (flag) {
                console.log("flag iteam!");
                console.log(item, "item");
                arr.push({
                    name: item.name,
                    email: item.email,
                    score: item.score,
                    path: item.path
                });
            }
        });
        console.log("Filtered Data");
        console.log(arr);
        setUserData(arr);
        setInitial(false);
        setLoading(false);
    };

    function download(filename, text) {
        var element = document.createElement("a");
        element.setAttribute(
            "href",
            "data:text/plain;charset=utf-8," + encodeURIComponent(text)
        );
        element.setAttribute("download", filename);

        element.style.display = "none";
        document.body.appendChild(element);

        element.click();

        document.body.removeChild(element);
    }


     

    // Start file download.
    const downloadCode = () => {
        let fileString = "";
        console.log(candidates);
        candidates.forEach((item, i) => {
            fileString += item;
            if (i != candidates.length) {
                fileString += ",";
            }
        });
        download("mail.txt", fileString);
    };
    return (
        <div className="relative">
            <div className="bg-[#D7DFDC] p-8 px-20 relative" style={{ backgroundColor: '#d71e28', borderBottom: '4px solid #ffcd41' }} >
                <div className="flex items-center flex-row justify-between">
                    <div className="text-3xl text-blue-700 font-bold tracking-tight" style={{ color: 'whitesmoke' }}>
                        CloudZone: Hackthon2025 Gen AI For Platform Support - Integrated Platform Environment
                    </div>
                </div>
                <div className="w-full">
                    {/* <h2 className="text-3xl font-medium" style={{color:'yellowgreen'}} ></h2> */}
                    <div className="mt-3 relative flex flex-row items-center bg-white w-3/5 p-3 pl-6 rounded-full ">
                        <input
                            onChange={(e) => {
                                setAdditionalFilter(e.target.value);
                            }}
                            placeholder="You can enter your keyword for chat assistance help!"
                            type="text"
                            className="transparent  r-0 w-4/5 h-full outline-none text-gray-700  pl-5"
                        />
                    </div>
                </div>
            </div>
            <div className="dashboard-container">
                <h1>Incident Dashboard</h1>

                {/* Filter Section */}
                <div className="filter-section">
                    <label htmlFor="statusFilter">Filter by Status: </label>
                    <select
                        id="statusFilter"
                        value={selectedStatus}
                        onChange={(e) => setSelectedStatus(e.target.value)}
                    >
                        <option value="All">All</option>
                        <option value="Open">Open</option>
                        <option value="Resolved">Resolved</option>
                        <option value="In Progress">In Progress</option>
                    </select>
                </div>

                {/* Incident Summary */}
                <div className="incident-summary">
                    <h2>Summary</h2>
                    <p>Total Incidents: {mockIncidentData.length}</p>
                    <p>Open: {mockIncidentData.filter((i) => i.status === "Open").length}</p>
                    <p>Resolved: {mockIncidentData.filter((i) => i.status === "Resolved").length}</p>
                    <p>In Progress: {mockIncidentData.filter((i) => i.status === "In Progress").length}</p>
                </div>

                {/* Incident List */}
                <div className="incident-list">
                    <h2>Incident List</h2>
                    <table>
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Status</th>
                                <th>Description</th>
                                <th>Priority</th>
                                <th>Team</th>
                            </tr>
                        </thead>
                        <tbody>
                            {filteredIncidents.map((incident) => (
                                <tr key={incident.id}>
                                    <td>{incident.id}</td>
                                    <td>{incident.status}</td>
                                    <td>{incident.description}</td>
                                    <td>{incident.priority}</td>
                                    <td>{incident.team}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>

                {/* Chat AI Bot Wizard */}
                <div className="chatbot-container">
                    <h2>AI Chat Assistant</h2>
                    <input
                        type="text"
                        value={userInput}
                        onChange={(e) => setUserInput(e.target.value)}
                        placeholder="Type your message..."
                    />
                    <button onClick={handleSendMessage}>Send</button>
                    <div className="chatbox">
                        {chatMessages.map((msg, index) => (
                            <div
                                key={index}
                                className={`chat-message ${msg.sender === "User" ? "user-message" : "bot-message"}`}
                            >
                                <strong>{msg.sender}:</strong> {msg.text}
                            </div>
                        ))}
                    </div>
                    
                </div>
            </div>



        </div>
    );
}

export default App;
