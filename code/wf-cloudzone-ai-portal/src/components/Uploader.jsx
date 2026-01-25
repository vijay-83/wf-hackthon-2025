import React from 'react';
import apiService from "../Service/ApiService";

export default function UploadIncidentFile() {
    const handleFileChange = async (event) => {
        const file = event.target.files[0];
        if (!file) return;

        try {
            const result = await apiService.uploadIncidentJsonFile(file);
            console.log("✅ Upload Success:", result);
        } catch (error) {
            console.error("❌ Upload Failed:", error.message);
        }
    };

    return (
        <div className="p-4">
            <label className="block mb-2 text-sm font-medium text-white">Upload JIRA Incidents JSON:</label>
            <input
                type="file"
                accept=".json"
                onChange={handleFileChange}
                className="block w-full text-sm text-white bg-red-700 border border-gray-300 rounded-lg cursor-pointer"
            />
        </div>
    );
}
