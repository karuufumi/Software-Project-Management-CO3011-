from fastapi import FastAPI, HTTPException, Depends, Header
from fastapi.responses import HTMLResponse
from pydantic import BaseModel, EmailStr
from controllers import authenticate_user, register_user, get_user_by_id
from models import Role
from utils import generate_token, verify_token, payload
from typing import Optional
import os

app = FastAPI(title="Authentication Microservice", version="1.0.1")

# Pydantic models for request validation
class LoginRequest(BaseModel):
    email: EmailStr
    password: str

class RegisterRequest(BaseModel):
    name: str
    email: EmailStr
    password: str
    role: Optional[Role] = Role.USER

class TokenResponse(BaseModel):
    token: str
    user: dict

# Dependency to verify token
def get_current_user(authorization: str = Header(None)):
    if not authorization or not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Missing or invalid token")
    
    token = authorization.split(" ")[1]
    secret_key = os.getenv("JWT_SECRET", "secret-key")
    
    user_data = verify_token(token, secret_key)
    if not user_data:
        raise HTTPException(status_code=401, detail="Invalid or expired token")
    
    return user_data

@app.get("/", response_class=HTMLResponse)
async def root():
    """Root endpoint with HTML page"""
    html_content = """
    <!DOCTYPE html>
    <html lang="en">
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>Authentication Microservice</title>
        <style>
            * {
                margin: 0;
                padding: 0;
                box-sizing: border-box;
            }
            
            body {
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                min-height: 100vh;
                display: flex;
                align-items: center;
                justify-content: center;
                padding: 20px;
            }
            
            .container {
                background: white;
                border-radius: 20px;
                box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
                max-width: 800px;
                width: 100%;
                padding: 50px;
                text-align: center;
            }
            
            .logo {
                width: 80px;
                height: 80px;
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                border-radius: 20px;
                display: flex;
                align-items: center;
                justify-content: center;
                margin: 0 auto 30px;
                font-size: 40px;
            }
            
            h1 {
                color: #333;
                font-size: 2.5em;
                margin-bottom: 10px;
            }
            
            .version {
                color: #667eea;
                font-size: 0.9em;
                margin-bottom: 30px;
            }
            
            .description {
                color: #666;
                line-height: 1.6;
                margin-bottom: 40px;
                font-size: 1.1em;
            }
            
            .features {
                display: grid;
                grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
                gap: 20px;
                margin-bottom: 40px;
            }
            
            .feature {
                background: #f8f9fa;
                padding: 20px;
                border-radius: 10px;
                transition: transform 0.3s ease;
            }
            
            .feature:hover {
                transform: translateY(-5px);
            }
            
            .feature-icon {
                font-size: 2em;
                margin-bottom: 10px;
            }
            
            .feature-title {
                color: #333;
                font-weight: bold;
                margin-bottom: 5px;
            }
            
            .feature-desc {
                color: #666;
                font-size: 0.9em;
            }
            
            .endpoints {
                background: #f8f9fa;
                border-radius: 10px;
                padding: 30px;
                margin-bottom: 30px;
                text-align: left;
            }
            
            .endpoints h2 {
                color: #333;
                margin-bottom: 20px;
                text-align: center;
            }
            
            .endpoint {
                background: white;
                padding: 15px;
                border-radius: 8px;
                margin-bottom: 10px;
                display: flex;
                align-items: center;
                gap: 15px;
            }
            
            .method {
                padding: 5px 15px;
                border-radius: 5px;
                font-weight: bold;
                font-size: 0.85em;
                min-width: 70px;
                text-align: center;
            }
            
            .get { background: #61affe; color: white; }
            .post { background: #49cc90; color: white; }
            
            .path {
                color: #333;
                font-family: 'Courier New', monospace;
                flex: 1;
            }
            
            .buttons {
                display: flex;
                gap: 15px;
                justify-content: center;
                flex-wrap: wrap;
            }
            
            .btn {
                padding: 15px 30px;
                border: none;
                border-radius: 10px;
                font-size: 1em;
                cursor: pointer;
                text-decoration: none;
                display: inline-block;
                transition: transform 0.2s ease, box-shadow 0.2s ease;
            }
            
            .btn:hover {
                transform: translateY(-2px);
                box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
            }
            
            .btn-primary {
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                color: white;
            }
            
            .btn-secondary {
                background: white;
                color: #667eea;
                border: 2px solid #667eea;
            }
            
            .status {
                display: inline-block;
                padding: 5px 15px;
                background: #49cc90;
                color: white;
                border-radius: 20px;
                font-size: 0.9em;
                margin-bottom: 20px;
            }
            
            .footer {
                margin-top: 40px;
                padding-top: 20px;
                border-top: 1px solid #eee;
                color: #999;
                font-size: 0.9em;
            }
        </style>
    </head>
    <body>
        <div class="container">
            <div class="logo">🔐</div>
            <h1>Authentication Microservice</h1>
            <p class="version">Version 1.0.1</p>
            <span class="status">✓ Service Running</span>
            
            <p class="description">
                A secure and scalable authentication service built with FastAPI and MongoDB. 
                Handles user registration, login, and JWT token management.
            </p>
            
            <div class="features">
                <div class="feature">
                    <div class="feature-icon">🔒</div>
                    <div class="feature-title">Secure Auth</div>
                    <div class="feature-desc">JWT token-based authentication</div>
                </div>
                <div class="feature">
                    <div class="feature-icon">🗄️</div>
                    <div class="feature-title">MongoDB</div>
                    <div class="feature-desc">Fast & scalable database</div>
                </div>
                <div class="feature">
                    <div class="feature-icon">⚡</div>
                    <div class="feature-title">High Performance</div>
                    <div class="feature-desc">Built with FastAPI</div>
                </div>
                <div class="feature">
                    <div class="feature-icon">🐳</div>
                    <div class="feature-title">Docker Ready</div>
                    <div class="feature-desc">Easy deployment</div>
                </div>
            </div>
            
            <div class="endpoints">
                <h2>📡 Available Endpoints</h2>
                <div class="endpoint">
                    <span class="method post">POST</span>
                    <span class="path">/register</span>
                    <span>Register new user</span>
                </div>
                <div class="endpoint">
                    <span class="method post">POST</span>
                    <span class="path">/login</span>
                    <span>User login</span>
                </div>
                <div class="endpoint">
                    <span class="method get">GET</span>
                    <span class="path">/me</span>
                    <span>Get current user</span>
                </div>
                <div class="endpoint">
                    <span class="method post">POST</span>
                    <span class="path">/verify-token</span>
                    <span>Verify JWT token</span>
                </div>
                <div class="endpoint">
                    <span class="method get">GET</span>
                    <span class="path">/health</span>
                    <span>Health check</span>
                </div>
            </div>
            
            <div class="buttons">
                <a href="/docs" class="btn btn-primary">📚 API Documentation</a>
                <a href="/health" class="btn btn-secondary">❤️ Health Check</a>
            </div>
            
            <div class="footer">
                <p>Made with ❤️ using FastAPI, MongoDB & Docker</p>
                <p>© 2025 Authentication Microservice</p>
            </div>
        </div>
    </body>
    </html>
    """
    return HTMLResponse(content=html_content)

@app.get("/health")
async def health_check():
    """Health check endpoint."""
    return {"status": "healthy", "service": "authentication", "version": "1.0.1"}
