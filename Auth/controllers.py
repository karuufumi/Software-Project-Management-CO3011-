from typing import Any, Dict
from models import User, Role
from utils import hash_string, verify_hash
from db import get_users_collection
import uuid
from datetime import datetime
from pymongo.errors import DuplicateKeyError

def create_user(id: str, name: str, email: str, hashedpwd: str, role: Role) -> User:
    return User(id=id, name=name, email=email, hashedpwd=hashedpwd, role=role)

def get_user_info(user: User) -> Dict[str, Any]:
    return {
        "id": user.id,
        "name": user.name,
        "email": user.email,
        "role": user.role.value
    }

def is_admin(user: User) -> bool:
    return user.role == Role.ADMIN

def register_user(name: str, email: str, password: str, role: Role = Role.USER) -> Dict[str, Any]:
    """Register a new user in MongoDB."""
    hashed_password = hash_string(password)
    user_id = str(uuid.uuid4())
    
    users_collection = get_users_collection()
    
    user_doc = {
        "id": user_id,
        "name": name,
        "email": email,
        "hashedpwd": hashed_password,
        "role": role.value,
        "created_at": datetime.utcnow()
    }
    
    try:
        users_collection.insert_one(user_doc)
        user = create_user(user_id, name, email, hashed_password, role)
        return {"success": True, "user": get_user_info(user)}
    except DuplicateKeyError:
        return {"success": False, "error": "Email already exists"}
    except Exception as e:
        return {"success": False, "error": str(e)}

def authenticate_user(email: str, password: str) -> Dict[str, Any]:
    """Authenticate user from MongoDB."""
    users_collection = get_users_collection()
    
    try:
        user_doc = users_collection.find_one({"email": email})
        
        if not user_doc:
            return {"success": False, "error": "User not found"}
        
        if verify_hash(password, user_doc["hashedpwd"]):
            user = User(
                id=user_doc["id"],
                name=user_doc["name"],
                email=user_doc["email"],
                hashedpwd=user_doc["hashedpwd"],
                role=Role(user_doc["role"])
            )
            return {"success": True, "user": user}
        else:
            return {"success": False, "error": "Invalid password"}
    except Exception as e:
        return {"success": False, "error": str(e)}

def get_user_by_id(user_id: str) -> User | None:
    """Fetch user by ID from MongoDB."""
    users_collection = get_users_collection()
    
    try:
        user_doc = users_collection.find_one({"id": user_id})
        
        if not user_doc:
            return None
        
        return User(
            id=user_doc["id"],
            name=user_doc["name"],
            email=user_doc["email"],
            hashedpwd=user_doc["hashedpwd"],
            role=Role(user_doc["role"])
        )
    except Exception as e:
        print(f"Error fetching user: {e}")
        return None

def get_user_by_email(email: str) -> User | None:
    """Fetch user by email from MongoDB."""
    users_collection = get_users_collection()
    
    try:
        user_doc = users_collection.find_one({"email": email})
        
        if not user_doc:
            return None
        
        return User(
            id=user_doc["id"],
            name=user_doc["name"],
            email=user_doc["email"],
            hashedpwd=user_doc["hashedpwd"],
            role=Role(user_doc["role"])
        )
    except Exception as e:
        print(f"Error fetching user: {e}")
        return None

def delete_user(user_id: str) -> Dict[str, Any]:
    """Delete user from MongoDB."""
    users_collection = get_users_collection()
    
    try:
        result = users_collection.delete_one({"id": user_id})
        
        if result.deleted_count > 0:
            return {"success": True, "message": "User deleted successfully"}
        else:
            return {"success": False, "error": "User not found"}
    except Exception as e:
        return {"success": False, "error": str(e)}

def update_user(user_id: str, updates: Dict[str, Any]) -> Dict[str, Any]:
    """Update user information in MongoDB."""
    users_collection = get_users_collection()
    
    # Don't allow updating id or hashedpwd directly
    if "id" in updates:
        del updates["id"]
    if "hashedpwd" in updates:
        del updates["hashedpwd"]
    
    try:
        result = users_collection.update_one(
            {"id": user_id},
            {"$set": updates}
        )
        
        if result.modified_count > 0:
            return {"success": True, "message": "User updated successfully"}
        else:
            return {"success": False, "error": "User not found or no changes made"}
    except Exception as e:
        return {"success": False, "error": str(e)}