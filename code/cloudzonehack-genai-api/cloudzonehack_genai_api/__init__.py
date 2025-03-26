"""
The flask application package.
"""

from flask import Flask
app = Flask(__name__)

import cloudzonehack_genai_api.views
