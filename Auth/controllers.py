from typing import Any, Dict
from models import User, Role
from utils import hash_string, verify_hash
from db import get_conn, get_cursor, close_conn, close_cursor
#for id
import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart
import os
import time 
import uuid
import pyotp
import random
import string
from datetime import datetime, timedelta

def create_user(id: str, name: str, email: str, hashedpwd: str, role: Role) -> User:
    return User(id=id, name=name, email=email, hashedpwd=hashedpwd, role=role)
def get_user_info(user: User) -> Dict[str, Any]:
    return {
        "id": user.id,
        "name": user.name,
        "email": user.email,
        "role": user.role
    }
def is_admin(user: User) -> bool:
    return user.role == Role.ADMIN

def register_user(name: str, email: str, password: str, role: Role = Role.USER) -> Dict[str, Any]:
  
    hashed_password = hash_string(password)
    
    # Generate unique ID
    user_id = str(uuid.uuid4())
    
    # Store in database
    conn = get_conn("auth.db")
    cursor = get_cursor(conn)
    
    try:
        cursor.execute("""
            INSERT INTO users (id, name, email, hashedpwd, role)
            VALUES (?, ?, ?, ?, ?)
        """, (user_id, name, email, hashed_password, role.value))
        conn.commit()
        
        user = create_user(user_id, name, email, hashed_password, role)
        return {"success": True, "user": get_user_info(user)}
    except Exception as e:
        return {"success": False, "error": str(e)}
    finally:
        close_cursor(cursor)
        close_conn(conn)


# Store verification codes temporarily (use Redis in production)
verification_codes = {}


def generate_verification_code(length: int = 6) -> str:
    """Generate a random verification code."""
    return ''.join(random.choices(string.digits, k=length))


def authenticate_user(email: str, password: str) -> Dict[str, Any]:
    """Authenticates user and returns user data if valid."""
    
    conn = get_conn("auth.db")
    cursor = get_cursor(conn)
    
    try:
        cursor.execute("SELECT * FROM users WHERE email = ?", (email,))
        result = cursor.fetchone()
        
        if not result:
            return {"success": False, "error": "User not found"}
        
        user_id, name, email, hashed_pwd, role = result
        
        if verify_hash(password, hashed_pwd):
            user = User(id=user_id, name=name, email=email, hashedpwd=hashed_pwd, role=Role(role))
            return {"success": True, "user": user}
        else:
            return {"success": False, "error": "Invalid password"}
    finally:
        close_cursor(cursor)
        close_conn(conn)



def get_user_by_id(user_id: str) -> User | None:
    """Fetch user by ID from database."""
    conn = get_conn("auth.db")
    cursor = get_cursor(conn)
    
    try:
        cursor.execute("SELECT * FROM users WHERE id = ?", (user_id,))
        result = cursor.fetchone()
        
        if not result:
            return None
        
        user_id, name, email, hashed_pwd, role = result
        return User(id=user_id, name=name, email=email, hashedpwd=hashed_pwd, role=Role(role))
    finally:
        close_cursor(cursor)
        close_conn(conn)