from fastapi import FastAPI, HTTPException, Depends, Header
from pydantic import BaseModel, EmailStr
from controllers import authenticate_user, register_user, get_user_by_id
from models import Role
from utils import generate_token, verify_token, payload
import os
from typing import Optional

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

@app.get("/")
async def root():
    return {"message": "Authentication Microservice API", "version": "1.0.0"}

@app.post("/register", response_model=dict)
async def register(request: RegisterRequest):
    """Register a new user."""
    result = register_user(
        name=request.name,
        email=request.email,
        password=request.password,
        role=request.role or Role.USER
    )
    
    if not result["success"]:
        raise HTTPException(status_code=400, detail=result["error"])
    
    return {
        "message": "User registered successfully",
        "user": result["user"]
    }

@app.post("/login", response_model=TokenResponse)
async def login(request: LoginRequest):
    """Login endpoint that returns JWT token."""
    result = authenticate_user(request.email, request.password)
    
    if not result["success"]:
        raise HTTPException(status_code=401, detail=result["error"])
    
    user = result["user"]
    
    # Create payload and generate token
    token_payload = payload()
    token_payload.user_id = user.id
    token_payload.role = user.role.value
    
    secret_key = os.getenv("JWT_SECRET", "your-secret-key")
    token = generate_token(token_payload, secret_key)
    
    return {
        "token": token,
        "user": {
            "id": user.id,
            "name": user.name,
            "email": user.email,
            "role": user.role.value
        }
    }

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
        "role": user.role.value
    }

@app.post("/verify-token")
async def verify_token_endpoint(authorization: str = Header(None)):
    """Verify if token is valid."""
    if not authorization or not authorization.startswith("Bearer "):
        raise HTTPException(status_code=401, detail="Missing or invalid token")
    
    token = authorization.split(" ")[1]
    secret_key = os.getenv("JWT_SECRET", "your-secret-key")
    
    user_data = verify_token(token, secret_key)
    if not user_data:
        raise HTTPException(status_code=401, detail="Invalid or expired token")
    
    return {"valid": True, "user": user_data}

@app.get("/health")
async def health_check():
    """Health check endpoint."""
    return {"status": "healthy"}