from fastapi import FastAPI, HTTPException, Depends, Header
from fastapi.responses import HTMLResponse
from pydantic import BaseModel, EmailStr
from controllers import (
    authenticate_user, register_user, get_user_by_id,
    enable_2fa, disable_2fa, verify_totp
)
from models import Role
from utils import generate_token, verify_token, payload
from typing import Optional
import os

from db import init_mongodb

app = FastAPI(title="Authentication Microservice", version="1.0.1")

# Initialize MongoDB on startup
@app.on_event("startup")
async def startup_event():
    try:
        init_mongodb()
        print("MongoDB initialized successfully!")
    except Exception as e:
        print(f"Warning: MongoDB initialization failed: {e}")

# Pydantic models for request validation
class LoginRequest(BaseModel):
    email: EmailStr
    password: str
    totp_code: Optional[str] = None  # Optional TOTP code

class RegisterRequest(BaseModel):
    name: str
    email: EmailStr
    password: str
    role: Role = Role.USER  # Changed from Optional[Role] to Role with default

class TokenResponse(BaseModel):
    token: str
    user: dict

class VerifyTOTPRequest(BaseModel):
    totp_code: str

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
            <p class="version">Version 1.0.1 with 2FA</p>
            <span class="status">✓ Service Running</span>
            
            <p class="description">
                A secure and scalable authentication service with 2FA support built with FastAPI and MongoDB. 
                Handles user registration, login, JWT tokens, and TOTP-based two-factor authentication.
            </p>
            
            <div class="features">
                <div class="feature">
                    <div class="feature-icon">🔒</div>
                    <div class="feature-title">Secure Auth</div>
                    <div class="feature-desc">JWT token-based authentication</div>
                </div>
                <div class="feature">
                    <div class="feature-icon">🔐</div>
                    <div class="feature-title">2FA/TOTP</div>
                    <div class="feature-desc">Google Authenticator support</div>
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
                    <span>User login (with optional TOTP)</span>
                </div>
                <div class="endpoint">
                    <span class="method post">POST</span>
                    <span class="path">/enable-2fa</span>
                    <span>Enable 2FA and get QR code</span>
                </div>
                <div class="endpoint">
                    <span class="method post">POST</span>
                    <span class="path">/disable-2fa</span>
                    <span>Disable 2FA</span>
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
                <p>Made with ❤️ using FastAPI, MongoDB, Docker & PyOTP</p>
                <p>© 2025 Authentication Microservice</p>
            </div>
        </div>
    </body>
    </html>
    """
    return HTMLResponse(content=html_content)

@app.post("/register", response_model=dict)
async def register(request: RegisterRequest):
    """Register a new user."""
    result = register_user(
        name=request.name,
        email=request.email,
        password=request.password,
        role=request.role  # Now always a Role, never None
    )
    
    if not result["success"]:
        raise HTTPException(status_code=400, detail=result["error"])
    
    return {
        "message": "User registered successfully",
        "user": result["user"]
    }

@app.post("/login", response_model=TokenResponse)
async def login(request: LoginRequest):
    """Login endpoint with optional 2FA verification."""
    result = authenticate_user(request.email, request.password)
    
    if not result["success"]:
        raise HTTPException(status_code=401, detail=result["error"])
    
    user = result["user"]
    
    # Check if 2FA is enabled
    if user.is_2fa_enabled:
        if not request.totp_code:
            raise HTTPException(
                status_code=403,
                detail="2FA is enabled. Please provide TOTP code."
            )
        
        # Verify TOTP code
        if not verify_totp(user, request.totp_code):
            raise HTTPException(status_code=401, detail="Invalid TOTP code")
    
    # Create payload and generate token
    token_payload = payload()
    token_payload.user_id = user.id
    token_payload.role = user.role.value
    
    secret_key = os.getenv("JWT_SECRET", "secret-key")
    token = generate_token(token_payload, secret_key)
    
    return {
        "token": token,
        "user": {
            "id": user.id,
            "name": user.name,
            "email": user.email,
            "role": user.role.value,
            "is_2fa_enabled": user.is_2fa_enabled
        }
    }

@app.post("/enable-2fa")
async def enable_two_factor_auth(current_user: dict = Depends(get_current_user)):
    """Enable 2FA and return QR code for scanning."""
    user_id = current_user["user_id"]
    
    result = enable_2fa(user_id)
    
    if not result["success"]:
        raise HTTPException(status_code=400, detail=result["error"])
    
    return {
        "message": result["message"],
        "secret": result["totp_secret"],
        "qr_code_base64": result["qr_code"]
    }

@app.post("/disable-2fa")
async def disable_two_factor_auth(
    request: VerifyTOTPRequest,
    current_user: dict = Depends(get_current_user)
):
    """Disable 2FA (requires TOTP verification)."""
    user_id = current_user["user_id"]
    user = get_user_by_id(user_id)
    
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    
    # Verify TOTP before disabling
    if not verify_totp(user, request.totp_code):
        raise HTTPException(status_code=401, detail="Invalid TOTP code")
    
    result = disable_2fa(user_id)
    
    if not result["success"]:
        raise HTTPException(status_code=400, detail=result["error"])
    
    return {"message": result["message"]}

@app.get("/me")
async def get_current_user_info(current_user: dict = Depends(get_current_user)):
    """Get current authenticated user information."""
    user = get_user_by_id(current_user["user_id"])
    
    if not user:
        raise HTTPException(status_code=404, detail="User not found")
    
    return {
        "id": user.id,
        "name": user.name,
        "email": user.email,
        "role": user.role.value,
        "is_2fa_enabled": user.is_2fa_enabled
    }

@app.post("/verify-token")
async def verify_token_endpoint(authorization: str = Header(None)):
    """Verify if token is valid."""
    if not authorization or not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Missing or invalid token")
    
    token = authorization.split(" ")[1]
    secret_key = os.getenv("JWT_SECRET", "secret-key")
    
    user_data = verify_token(token, secret_key)
    if not user_data:
        raise HTTPException(status_code=401, detail="Invalid or expired token")
    
    return {"valid": True, "user": user_data}

@app.get("/health")
async def health_check():
    """Health check endpoint."""
    return {"status": "healthy", "service": "authentication", "version": "1.0.1", "2fa": "enabled"}