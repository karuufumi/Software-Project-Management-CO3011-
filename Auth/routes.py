from fastapi import FastAPI, HTTPException, Depends, Header
from fastapi.responses import HTMLResponse
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel, EmailStr, Field
from controllers import (
    authenticate_user, register_user, get_user_by_id,
    enable_2fa, disable_2fa, verify_totp
)
from models import Role
from utils import generate_token, verify_token, payload
from typing import Optional
import os

from db import init_mongodb

# Enhanced FastAPI app with metadata
app = FastAPI(
    title="Authentication Microservice API",
    description="""
    🔐 **Secure Authentication Service with 2FA Support**
    
    This microservice provides comprehensive authentication and authorization features:
    
    ## Features
    
    * 🔑 **User Registration & Login** - Secure user account management
    * 🔒 **JWT Token Authentication** - Stateless token-based auth
    * 🔐 **Two-Factor Authentication (2FA)** - TOTP support with Google Authenticator
    * 👤 **User Profile Management** - Get and update user information
    * ✅ **Token Verification** - Validate JWT tokens
    * 🏥 **Health Checks** - Monitor service status
    
    ## Security
    
    * Passwords hashed with bcrypt
    * JWT tokens with configurable expiration
    * TOTP-based 2FA with QR code generation
    * Role-based access control (RBAC)
    
    ## Tech Stack
    
    * FastAPI - Modern Python web framework
    * MongoDB - NoSQL database
    * PyJWT - JSON Web Tokens
    * PyOTP - TOTP implementation
    * Docker - Containerization
    """,
    version="1.0.1",
    contact={
        "name": "API Support",
        "email": "support@example.com",
    },
    license_info={
        "name": "MIT",
    },
    docs_url="/docs",
    redoc_url="/redoc",
    openapi_tags=[
        {
            "name": "Authentication",
            "description": "Operations for user authentication and registration",
        },
        {
            "name": "2FA",
            "description": "Two-factor authentication management",
        },
        {
            "name": "User",
            "description": "User profile and information",
        },
        {
            "name": "System",
            "description": "System health and status",
        },
    ]
)

# Add CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # In production, replace with specific origins
    allow_credentials=True,
    allow_methods=["*"],  # Allow all methods (GET, POST, PUT, DELETE, etc.)
    allow_headers=["*"],  # Allow all headers
    expose_headers=["*"],
)

# Initialize MongoDB on startup
@app.on_event("startup")
async def startup_event():
    try:
        init_mongodb()
        print("MongoDB initialized successfully!")
    except Exception as e:
        print(f"Warning: MongoDB initialization failed: {e}")

# Pydantic models for request validation with examples
class LoginRequest(BaseModel):
    email: EmailStr = Field(
        ...,
        description="User email address",
        examples=["user@example.com"]
    )
    password: str = Field(
        ...,
        min_length=6,
        description="User password",
        examples=["password123"]
    )
    totp_code: Optional[str] = Field(
        None,
        description="TOTP code from authenticator app (required if 2FA is enabled)",
        examples=["123456"]
    )
    
    class Config:
        json_schema_extra = {
            "example": {
                "email": "user@example.com",
                "password": "password123",
                "totp_code": "123456"
            }
        }

class RegisterRequest(BaseModel):
    name: str = Field(
        ...,
        min_length=2,
        max_length=100,
        description="User full name",
        examples=["John Doe"]
    )
    email: EmailStr = Field(
        ...,
        description="User email address",
        examples=["john@example.com"]
    )
    password: str = Field(
        ...,
        min_length=6,
        description="User password",
        examples=["securepass123"]
    )
    role: Role = Field(
        default=Role.USER,
        description="User role"
    )
    
    class Config:
        json_schema_extra = {
            "example": {
                "name": "John Doe",
                "email": "john@example.com",
                "password": "securepass123",
                "role": "user"
            }
        }

class TokenResponse(BaseModel):
    token: str = Field(..., description="JWT authentication token")
    user: dict = Field(..., description="User information")
    
    class Config:
        json_schema_extra = {
            "example": {
                "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                "user": {
                    "id": "123e4567-e89b-12d3-a456-426614174000",
                    "name": "John Doe",
                    "email": "john@example.com",
                    "role": "user",
                    "is_2fa_enabled": False
                }
            }
        }

class VerifyTOTPRequest(BaseModel):
    totp_code: str = Field(
        ...,
        description="6-digit TOTP code from authenticator app",
        min_length=6,
        max_length=6,
        examples=["123456"]
    )
    
    class Config:
        json_schema_extra = {
            "example": {
                "totp_code": "123456"
            }
        }

class Enable2FAResponse(BaseModel):
    message: str = Field(..., description="Success message")
    secret: str = Field(..., description="TOTP secret key (backup)")
    qr_code_base64: str = Field(..., description="Base64 encoded QR code image")

class UserInfoResponse(BaseModel):
    id: str = Field(..., description="User unique identifier")
    name: str = Field(..., description="User full name")
    email: str = Field(..., description="User email address")
    role: str = Field(..., description="User role")
    is_2fa_enabled: bool = Field(..., description="Whether 2FA is enabled")

# Dependency to verify token
def get_current_user(authorization: str = Header(None, description="Bearer token")):
    """Verify JWT token and return user data"""
    if not authorization or not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Missing or invalid token")
    
    token = authorization.split(" ")[1]
    secret_key = os.getenv("JWT_SECRET", "secret-key")
    
    user_data = verify_token(token, secret_key)
    if not user_data:
        raise HTTPException(status_code=401, detail="Invalid or expired token")
    
    return user_data

@app.get("/", response_class=HTMLResponse, tags=["System"], include_in_schema=False)
async def root():
    """Root endpoint with HTML landing page"""
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
                max-width: 900px;
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
                display: grid;
                grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
                gap: 15px;
                margin-bottom: 30px;
            }
            
            .btn {
                padding: 15px 30px;
                border: none;
                border-radius: 10px;
                font-size: 1em;
                cursor: pointer;
                text-decoration: none;
                display: inline-flex;
                align-items: center;
                justify-content: center;
                gap: 8px;
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
            
            .btn-success {
                background: #49cc90;
                color: white;
            }
            
            .btn-info {
                background: #61affe;
                color: white;
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
            
            .docs-section {
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                color: white;
                padding: 30px;
                border-radius: 10px;
                margin-bottom: 30px;
            }
            
            .docs-section h2 {
                margin-bottom: 15px;
            }
            
            .docs-section p {
                margin-bottom: 20px;
                line-height: 1.6;
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
            <p class="version">Version 1.0.1 with 2FA Support</p>
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
            
            <div class="docs-section">
                <h2>📚 API Documentation</h2>
                <p>
                    Explore our interactive API documentation powered by Swagger UI and ReDoc. 
                    Test endpoints, view request/response schemas, and learn how to integrate with our service.
                </p>
            </div>
            
            <div class="buttons">
                <a href="/docs" class="btn btn-primary">
                    <span>📖</span>
                    <span>Swagger UI</span>
                </a>
                <a href="/redoc" class="btn btn-info">
                    <span>📘</span>
                    <span>ReDoc</span>
                </a>
                <a href="/health" class="btn btn-success">
                    <span>❤️</span>
                    <span>Health Check</span>
                </a>
                <a href="/openapi.json" class="btn btn-secondary">
                    <span>📄</span>
                    <span>OpenAPI Schema</span>
                </a>
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
            
            <div class="footer">
                <p>Made with ❤️ using FastAPI, MongoDB, Docker & PyOTP</p>
                <p>© 2025 Authentication Microservice</p>
            </div>
        </div>
    </body>
    </html>
    """
    return HTMLResponse(content=html_content)

@app.post(
    "/register",
    response_model=dict,
    tags=["Authentication"],
    summary="Register a new user",
    description="Create a new user account with email and password. Optionally specify a role (default: user).",
    responses={
        200: {
            "description": "User registered successfully",
            "content": {
                "application/json": {
                    "example": {
                        "message": "User registered successfully",
                        "user": {
                            "id": "123e4567-e89b-12d3-a456-426614174000",
                            "name": "John Doe",
                            "email": "john@example.com",
                            "role": "user",
                            "is_2fa_enabled": False
                        }
                    }
                }
            }
        },
        400: {
            "description": "Email already exists or validation error"
        }
    }
)
async def register(request: RegisterRequest):
    """
    Register a new user account.
    
    - **name**: User's full name (2-100 characters)
    - **email**: Valid email address (must be unique)
    - **password**: Password (minimum 6 characters)
    - **role**: User role (user, admin, guest, member) - defaults to 'user'
    """
    result = register_user(
        name=request.name,
        email=request.email,
        password=request.password,
        role=request.role
    )
    
    if not result["success"]:
        raise HTTPException(status_code=400, detail=result["error"])
    
    return {
        "message": "User registered successfully",
        "user": result["user"]
    }

@app.post(
    "/login",
    response_model=TokenResponse,
    tags=["Authentication"],
    summary="User login",
    description="Authenticate user with email and password. If 2FA is enabled, TOTP code is required.",
    responses={
        200: {
            "description": "Login successful",
        },
        401: {
            "description": "Invalid credentials or TOTP code"
        },
        403: {
            "description": "2FA enabled but TOTP code not provided"
        }
    }
)
async def login(request: LoginRequest):
    """
    Authenticate user and return JWT token.
    
    - **email**: User's email address
    - **password**: User's password
    - **totp_code**: 6-digit TOTP code (required if 2FA is enabled)
    
    Returns a JWT token for authenticated requests and user information.
    """
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

@app.post(
    "/enable-2fa",
    response_model=Enable2FAResponse,
    tags=["2FA"],
    summary="Enable two-factor authentication",
    description="Enable 2FA for the authenticated user and receive a QR code to scan with an authenticator app.",
    responses={
        200: {
            "description": "2FA enabled successfully with QR code",
        },
        401: {
            "description": "Unauthorized - invalid or missing token"
        }
    }
)
async def enable_two_factor_auth(current_user: dict = Depends(get_current_user)):
    """
    Enable two-factor authentication.
    
    Returns:
    - **message**: Success message
    - **secret**: TOTP secret key (save as backup)
    - **qr_code_base64**: Base64-encoded QR code image to scan with Google Authenticator or similar apps
    
    After enabling, you'll need to provide TOTP codes when logging in.
    """
    user_id = current_user["user_id"]
    
    result = enable_2fa(user_id)
    
    if not result["success"]:
        raise HTTPException(status_code=400, detail=result["error"])
    
    return {
        "message": result["message"],
        "secret": result["totp_secret"],
        "qr_code_base64": result["qr_code"]
    }

@app.post(
    "/disable-2fa",
    tags=["2FA"],
    summary="Disable two-factor authentication",
    description="Disable 2FA for the authenticated user. Requires TOTP verification.",
    responses={
        200: {
            "description": "2FA disabled successfully",
            "content": {
                "application/json": {
                    "example": {"message": "2FA disabled successfully"}
                }
            }
        },
        401: {
            "description": "Invalid TOTP code or unauthorized"
        }
    }
)
async def disable_two_factor_auth(
    request: VerifyTOTPRequest,
    current_user: dict = Depends(get_current_user)
):
    """
    Disable two-factor authentication.
    
    Requires verification with current TOTP code to ensure security.
    
    - **totp_code**: 6-digit code from authenticator app
    """
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

@app.get(
    "/me",
    response_model=UserInfoResponse,
    tags=["User"],
    summary="Get current user information",
    description="Retrieve information about the currently authenticated user.",
    responses={
        200: {
            "description": "User information retrieved successfully",
        },
        401: {
            "description": "Unauthorized - invalid or missing token"
        },
        404: {
            "description": "User not found"
        }
    }
)
async def get_current_user_info(current_user: dict = Depends(get_current_user)):
    """
    Get current authenticated user's information.
    
    Returns user profile including:
    - User ID
    - Name
    - Email
    - Role
    - 2FA status
    """
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

@app.post(
    "/verify-token",
    tags=["Authentication"],
    summary="Verify JWT token",
    description="Verify if a JWT token is valid and not expired.",
    responses={
        200: {
            "description": "Token is valid",
            "content": {
                "application/json": {
                    "example": {
                        "valid": True,
                        "user": {
                            "user_id": "123e4567-e89b-12d3-a456-426614174000",
                            "role": "user"
                        }
                    }
                }
            }
        },
        401: {
            "description": "Invalid or expired token"
        }
    }
)
async def verify_token_endpoint(authorization: str = Header(None, description="Bearer JWT token")):
    """
    Verify JWT token validity.
    
    Checks if the provided JWT token is:
    - Properly formatted
    - Not expired
    - Signed with correct secret
    
    Returns token payload if valid.
    """
    if not authorization or not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Missing or invalid token")
    
    token = authorization.split(" ")[1]
    secret_key = os.getenv("JWT_SECRET", "secret-key")
    
    user_data = verify_token(token, secret_key)
    if not user_data:
        raise HTTPException(status_code=401, detail="Invalid or expired token")
    
    return {"valid": True, "user": user_data}

@app.get(
    "/health",
    tags=["System"],
    summary="Health check",
    description="Check if the service is running and healthy.",
    responses={
        200: {
            "description": "Service is healthy",
            "content": {
                "application/json": {
                    "example": {
                        "status": "healthy",
                        "service": "authentication",
                        "version": "1.0.1",
                        "2fa": "enabled"
                    }
                }
            }
        }
    }
)
async def health_check():
    """
    Health check endpoint.
    
    Returns service status and version information.
    Useful for monitoring and load balancer health checks.
    """
    return {
        "status": "healthy",
        "service": "authentication",
        "version": "1.0.1",
        "2fa": "enabled"
    }