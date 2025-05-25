//import axios from "axios";
//import { useState } from "react";
//import { FaCloudUploadAlt } from "react-icons/fa";
//import { BsCloudDownloadFill } from "react-icons/bs";
import IncidentDashboard from "./components/IncidentDashboard";

function App() {

    const apiEndpoint = "https://cloudzoneresumeapi.azurewebsites.net"
    //const [userdata, setUserData] = useState([]);
    //const [initial, setInitial] = useState(true);
    //const [loading, setLoading] = useState(false);
    //const [URL, setURL] = useState("storageURl");
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
    return (
        <div className="relative">
            <div className="bg-[#D7DFDC] p-8 px-20 relative" style={{ backgroundColor: '#d71e28', borderBottom: '4px solid #ffcd41' }} >
                <div className="flex items-center flex-row justify-between">
                    <div className="text-3xl text-blue-700 font-bold tracking-tight" style={{ color: 'whitesmoke' }}>
                        CloudZone: Hackthon2025 Gen AI For Platform Support - Integrated Platform Environment
                    </div>
                </div>
            </div>
            <IncidentDashboard></IncidentDashboard>
        </div>
    );
}
export default App;
