from fastapi import FastAPI, HTTPException, Request
from fastapi.middleware.cors import CORSMiddleware
from openai import OpenAI
import requests

app = FastAPI()

# Configure CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
)

# OpenAI API Key
OPENAI_API_KEY = ""  # Replace with your OpenAI key

# Hugging Face Dataset Endpoint
DATASET_ENDPOINT = "https://datasets-server.huggingface.co/rows?dataset=arsen-r-a%2Fincident-management-generative-test1&config=default&split=train&offset=0&length=100"  # Replace with actual Hugging Face endpoint

    
@app.post("/chat")
async def get_recommendation(request: Request):
    """Generate contextual recommendations using OpenAI."""
    try:
        data = await request.json()
        incident_details = data.get("incidentDetails")

        openai = OpenAI(api_key=OPENAI_API_KEY)
        completion = openai.Completion.create(
            model="text-davinci-003",
            prompt=f"Provide a contextual recommendation based on the following incident details: {incident_details}",
            max_tokens=200,
        )

        return {"recommendation": completion["choices"][0]["text"].strip()}
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error generating recommendation: {e}"
                            


# 1. Contextual Recommendation Endpoint
@app.post("/recommendation")
async def recommendation(request: Request):
    try:
        data = await request.json()
        incident_details = data.get("incidentDetails", "No details provided.")

        # Query OpenAI GPT model
        response = openai.Completion.create(
            model="text-davinci-003",
            prompt=f"Provide a contextual recommendation for the following incident: {incident_details}",
            max_tokens=200,
        )

        recommendation = response.choices[0].text.strip()
        return {"recommendation": recommendation}
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error generating recommendation: {e}")


# 2. Incident Resolution Endpoint
@app.post("/resolution")
async def resolution(request: Request):
    try:
        data = await request.json()
        incident_details = data.get("incidentDetails", "No details provided.")

        # Query OpenAI GPT model
        response = openai.Completion.create(
            model="text-davinci-003",
            prompt=f"Provide a step-by-step resolution plan for the following incident: {incident_details}",
            max_tokens=300,
        )

        resolution = response.choices[0].text.strip()
        return {"resolution": resolution}
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error generating resolution: {e}")


# 3. Telemetry Data Endpoint (Hugging Face Dataset)
@app.get("/telemetry")
async def telemetry():
    try:
        # Fetch data from Hugging Face dataset
        response = requests.get(HUGGING_FACE_DATASET_URL)
        response.raise_for_status()
        return response.json()
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error fetching telemetry data: {e}")
    
    

@app.get("/data")
async def get_testing_data():
    """Fetch testing data from Hugging Face."""
    try:
        response = requests.get(DATASET_ENDPOINT)
        response.raise_for_status()
        return response.json()
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Error fetching testing data: {e}")