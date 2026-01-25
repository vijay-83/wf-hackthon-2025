//import axios from "axios";
import { useState } from "react";
//import { FaCloudUploadAlt } from "react-icons/fa";
//import { BsCloudDownloadFill } from "react-icons/bs";
import IncidentDashboard from "./components/IncidentDashboard";
import backgroundImg from './assets/GenAIBackGround1.png';


function App() {

    const apiEndpoint = "https://cloudzoneresumeapi.azurewebsites.net"
    //const [userdata, setUserData] = useState([]);
    //const [initial, setInitial] = useState(true);
    //const [loading, setLoading] = useState(false);
    //const [URL, setURL] = useState("storageURl");
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    const handleLogin = () => {
        // TODO: Replace with real auth login
        setIsLoggedIn(true);
    };

    const handleLogout = () => {
        // TODO: Replace with real auth logout
        setIsLoggedIn(false);
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
    return (
        <div className="relative">
            <div
                className="p-8 px-20 relative"
                style={{
                    backgroundColor: '#d71e28', // Deep red
                    borderBottom: '4px solid #ffcd41', // Accent yellow border
                }}
            >
                <div className="flex items-center justify-between">
                    <div
                        className="text-3xl font-bold tracking-tight"
                        style={{ color: 'whitesmoke' }}
                    >
                        CloudZone: Hackthon2025 Gen AI For Platform Support - Integrated Platform Environment
                    </div>

                    <div>
                        {isLoggedIn ? (
                            <button
                                onClick={handleLogout}
                                className="bg-black text-white hover:bg-white hover:text-black border border-white transition px-4 py-2 rounded"
                            >
                                🔓 Logout
                            </button>
                        ) : (
                            <button
                                onClick={handleLogin}
                                className="bg-white text-black hover:bg-black hover:text-white border border-black transition px-4 py-2 rounded"
                            >
                                🔐 Login
                            </button>
                        )}
                    </div>
                </div>
            </div>

            {!isLoggedIn ? (
                <div
                    style={{
                        backgroundImage: `url(${backgroundImg})`,
                        backgroundRepeat: 'no-repeat',
                       /* backgroundSize: 'cover',*/       // Ensures full coverage
                        backgroundPosition: 'center',  // Centers the image
                       /* backgroundAttachment: 'fixed',*/ // Optional: gives a parallax-like effect
                        width: '100%',
                        height: 'calc(100vh - 120px)', // Adjust based on your header height
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        color: 'white',
                        textAlign: 'center',
                    }}
                />

            ) : (
                <IncidentDashboard />
            )}
        </div>
    );
}
export default App;
