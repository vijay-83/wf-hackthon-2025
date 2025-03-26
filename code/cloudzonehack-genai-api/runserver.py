"""
This script runs the cloudzonehack_genai_api application using a development server.
"""
from fastapi import FastAPI
from flask import Flask
import uvicorn

from os import environ
from cloudzonehack_genai_api import app

if __name__ == '__main__':
    HOST = environ.get('SERVER_HOST', 'localhost')
    try:
        PORT = int(environ.get('SERVER_PORT', '5555'))
    except ValueError:
        PORT = 5555
    app.run(HOST, PORT)
