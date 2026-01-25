import React, { useState, useRef, useEffect } from "react";
import "./GenAIChatBoat.css";

export default function ChatBot() {
    const [messages, setMessages] = useState([
        { sender: "bot", text: "👋 Hi, I'm OpsBot. How can I help with your JIRA incidents today?" },
        { sender: "user", text: "Hey, we had a spike at 3AM. Any idea why?" },
        { sender: "bot", text: "It looks like it was caused by a misconfigured deployment on the auth service." }
    ]);

    const [input, setInput] = useState("");
    const [typing, setTyping] = useState(false);
    const chatEndRef = useRef(null);
    const [isMinimized, setIsMinimized] = useState(false);
    const [isFullscreen, setIsFullscreen] = useState(false);
    const [selectedAgent, setSelectedAgent] = useState(null);

    const toggleMinimize = () => {
        setIsMinimized(!isMinimized);
        if (isFullscreen) setIsFullscreen(false); // exit fullscreen if minimized
    };

    const toggleFullscreen = () => {
        setIsFullscreen(!isFullscreen);
        setIsMinimized(false); // ensure it's expanded when maximizing
    };


    useEffect(() => {
        chatEndRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [messages, typing]);

    const mockAgentResponse = async (prompt) => {
        setTyping(true);
        await new Promise((r) => setTimeout(r, 1500)); // simulate delay

        const mockReplies = [
            "Acknowledged. I’ve logged this for the SRE team to investigate.",
            "It might be related to a DNS misconfiguration. Shall I escalate?",
            "I've checked related incidents — 2 similar issues found in the last 7 days.",
            "A fix was deployed to staging, and it's being monitored.",
            "No alerts triggered yet, but logs show connection timeout anomalies."
        ];

        const reply = mockReplies[Math.floor(Math.random() * mockReplies.length)];
        setTyping(false);
        return reply;
    };

    const getAgentLabel = (agent) => {
        switch (agent) {
            case "suggestion": return "Incident Suggestion Agent";
            case "summary": return "Incident Summary Agent";
            case "bugfix": return "Bug Fix Agent";
            default: return "OpsBot";
        }
    };

    const handleClear = () => {
        setMessages([]);
    };


    useEffect(() => {
        if (messages.length > 0) {
            const last = messages[messages.length - 1];
            if (last.text.toLowerCase().includes("goodbye") || last.text.toLowerCase().includes("end of session")) {
                setTimeout(() => setSelectedAgent(null), 2000); // reset in 2 seconds
            }
        }
    }, [messages]);


    const handleSend = async () => {
        if (!input.trim()) return;

        const userMessage = { text: input, sender: "user" };
        setMessages((prev) => [...prev, userMessage]);
        setTyping(true);
        setInput("");

        const systemPrefix = `[Agent:${selectedAgent}]`;
        const fullQuery = `${systemPrefix} ${input}`;

        // send fullQuery to your API
        const reply = await mockAgentResponse(input);
        const botMessage = { sender: "bot", text: reply };
        setMessages((prev) => [...prev, botMessage]);
    };

    // Toggle to show/hide chat UI entirely if needed
    const [chatVisible, setChatVisible] = useState(true);

    const handleEndChat = () => {
        setMessages([]);
        setSelectedAgent(null);
        setIsFullscreen(false);
        setIsMinimized(true);
        setChatVisible(false);
        setTimeout(() => setChatVisible(true), 300); // simulate reset effect
    }

    useEffect(() => {
        console.log("Selected Agent Changed:", selectedAgent);
    }, [selectedAgent]);

    return (

        <div className={`chatbot-container ${isMinimized ? "minimized" : ""} ${isFullscreen ? "fullscreen" : ""}`}>
            {!selectedAgent ? (
                <div className="agent-selector">
                    <h3>🧠 Choose Your Support Agent</h3>
                    <button onClick={() => setSelectedAgent("suggestion")}>🤔 Incident Suggestion</button>
                    <button onClick={() => setSelectedAgent("summary")}>📋 Incident Summary</button>
                    <button onClick={() => setSelectedAgent("bugfix")}>🛠️ Bug Fix Assistant</button>
                </div>
            ) : (
                <>
                    <div className="chatbot-header">
                        <h3>🤖 {getAgentLabel(selectedAgent)}</h3>
                        <div className="chatbot-actions">
                                <button onClick={toggleFullscreen} title="Toggle Fullscreen" style={{fontSize:"25px"}}>
                                {isFullscreen ? "🗗" : "🗖"}
                            </button>
                            {/*<button onClick={toggleMinimize} title="Minimize">*/}
                            {/*    {isMinimized ? "🔼" : "🔽"}*/}
                                {/*</button>*/}
                               < button onClick={handleClear} title="Clear Chat">🗑️</button>
                            <button className="end-chat-button" onClick={handleEndChat}>
                               ❌
                            </button>
                        </div>
                    </div>

                    <div className="chat-input-container" style={{ display: selectedAgent ? "block" : "none" }}>
                        <input
                            type="text"
                            placeholder={`Ask something to ${selectedAgent}...`}
                            value={input}
                            onChange={(e) => setInput(e.target.value)}
                            onKeyDown={(e) => e.key === "Enter" && handleSend()}
                        />
                        <button onClick={handleSend}>Send</button>
                    </div>
                    <div className="chat-input-container" style={{ display: selectedAgent ? "block" : "none" }}>
                        {messages.map((msg, i) => (
                            <div key={i} className={`chat-row ${msg.sender}`}>
                                {msg.sender === "bot" && <span className="avatar">🤖</span>}
                                <div className={`chat-bubble ${msg.sender}`}>{msg.text}</div>
                            </div>
                        ))}
                        {typing && (
                            <div className="chat-row bot">
                                <span className="avatar">🤖</span>
                                <div className="chat-bubble bot">Typing...</div>
                            </div>
                        )}
                        <div ref={chatEndRef} />
                    </div>


                </>
            )}
        </div>


        //<div className={`chatbot-container ${isMinimized ? "minimized" : ""} ${isFullscreen ? "fullscreen" : ""}`}>
        //    <div className="chatbot-header">
        //        <h3>🤖 OpsBot</h3>
        //        <div className="chatbot-actions">
        //            <button onClick={handleClear} title="Clear Chat">🗑️</button>
        //            <button onClick={toggleFullscreen} title="Toggle Fullscreen">
        //                {isFullscreen ? "🗗" : "🗖"}
        //            </button>
        //            <button onClick={toggleMinimize} title="Minimize">
        //                {isMinimized ? "🔼" : "🔽"}
        //            </button>
        //        </div>
        //    </div>

        //    {!isMinimized && (
        //        <>
        //            {/* Input at top */}
        //            <div className="chat-input-container">
        //                <input
        //                    type="text"
        //                    placeholder="Type your JIRA question..."
        //                    value={input}
        //                    onChange={(e) => setInput(e.target.value)}
        //                    onKeyDown={(e) => e.key === "Enter" && handleSend()}
        //                />
        //                <button onClick={handleSend}>Send</button>
        //            </div>

        //            {/* Messages */}
        //            <div className="chat-window">
        //                <div className="chat-messages">
        //                    {messages.map((msg, i) => (
        //                        <div key={i} className={`chat-row ${msg.sender}`}>
        //                            {msg.sender === "bot" && <span className="avatar">🤖</span>}
        //                            <div className={`chat-bubble ${msg.sender}`}>{msg.text}</div>
        //                        </div>
        //                    ))}
        //                    {typing && (
        //                        <div className="chat-row bot">
        //                            <span className="avatar">🤖</span>
        //                            <div className="chat-bubble bot">Typing...</div>
        //                        </div>
        //                    )}
        //                    <div ref={chatEndRef} />
        //                </div>
        //            </div>
        //        </>
        //    )}
        //</div>

    );
}
