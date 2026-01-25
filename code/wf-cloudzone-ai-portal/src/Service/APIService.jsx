class ApiService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
    }

    async fetchIncidents(itemCount) {
        return this.request(`/incidents/grid?itemCount=${itemCount}`);
    }

    async uploadIncidentJsonFile(file) {
        const formData = new FormData();
        formData.append('file', file);

        try {
                const response = await fetch(`${this.baseUrl}/incidentindexer/index/upload`, {
                    method: 'POST',
                    body: formData,
                });

                if (!response.ok) {
                    throw new Error(`Upload failed! Status: ${response.status}`);
                }

            return await response.json();
        } catch (error) {
            console.error("Upload failed:", error);
            throw error;
        }
    }

    async request(endpoint, options = {}) {
        try {
            const response = await fetch(`${this.baseUrl}${endpoint}`, {
                headers: { "Content-Type": "application/json" },
                ...options,
            });

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            const contentType = response.headers.get("content-type");
            if (!contentType || !contentType.includes("application/json")) {
                throw new Error(`Expected JSON, but got: ${contentType}`);
            }

            return response.json();
        } catch (error) {
            console.error("API request failed:", error);
            throw error;
        }
    }
}

// Create an instance for usage
const apiService = new ApiService("https://localhost:7045/api");

export default apiService;
