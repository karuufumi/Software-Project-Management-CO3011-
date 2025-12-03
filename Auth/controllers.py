from typing import Any, Dict, Optional
from models import User, Role
from utils import hash_string, verify_hash
from db import get_users_collection
import uuid
from datetime import datetime
from pymongo.errors import DuplicateKeyError
import pyotp
import qrcode
from io import BytesIO
import base64
from PIL import Image

def create_user(
    id: str, 
    name: str, 
    email: str, 
    hashedpwd: str, 
    role: Role, 
    totp_secret: Optional[str] = None, 
    is_2fa_enabled: bool = False
) -> User:
    return User(
        id=id, 
        name=name, 
        email=email, 
        hashedpwd=hashedpwd, 
        role=role,
        totp_secret=totp_secret,
        is_2fa_enabled=is_2fa_enabled
    )

def get_user_info(user: User) -> Dict[str, Any]:
    return {
        "id": user.id,
        "name": user.name,
        "email": user.email,
        "role": user.role.value,
        "is_2fa_enabled": user.is_2fa_enabled
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
        "totp_secret": None,
        "is_2fa_enabled": False,
        "created_at": datetime.utcnow()
    }
    
    try:
        users_collection.insert_one(user_doc)
        user = create_user(user_id, name, email, hashed_password, role, None, False)
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
                role=Role(user_doc["role"]),
                totp_secret=user_doc.get("totp_secret"),
                is_2fa_enabled=user_doc.get("is_2fa_enabled", False)
            )
            return {"success": True, "user": user}
        else:
            return {"success": False, "error": "Invalid password"}
    except Exception as e:
        return {"success": False, "error": str(e)}

def get_user_by_id(user_id: str) -> Optional[User]:
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
            role=Role(user_doc["role"]),
            totp_secret=user_doc.get("totp_secret"),
            is_2fa_enabled=user_doc.get("is_2fa_enabled", False)
        )
    except Exception as e:
        print(f"Error fetching user: {e}")
        return None

def get_user_by_email(email: str) -> Optional[User]:
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
            role=Role(user_doc["role"]),
            totp_secret=user_doc.get("totp_secret"),
            is_2fa_enabled=user_doc.get("is_2fa_enabled", False)
        )
    except Exception as e:
        print(f"Error fetching user: {e}")
        return None

# 2FA Functions

def generate_totp_secret() -> str:
    """Generate a new TOTP secret."""
    return pyotp.random_base32()

def generate_qr_code(user_email: str, totp_secret: str, issuer_name: str = "AuthService") -> str:
    """Generate QR code for TOTP setup."""
    totp = pyotp.TOTP(totp_secret)
    provisioning_uri = totp.provisioning_uri(
        name=user_email,
        issuer_name=issuer_name
    )
    
    # Generate QR code
    qr = qrcode.QRCode(version=1, box_size=10, border=5)
    qr.add_data(provisioning_uri)
    qr.make(fit=True)
    
    # Create image
    img = qr.make_image(fill_color="black", back_color="white")
    
    # Convert PIL image to bytes
    buffered = BytesIO()
    
    # Type check to ensure img is a PIL Image
    if isinstance(img, Image.Image):
        img.save(buffered, "PNG")  # Fixed: use "PNG" instead of format="PNG"
    else:
        # Fallback if it's not a PIL Image
        img.save(buffered)
    
    # Convert to base64
    img_str = base64.b64encode(buffered.getvalue()).decode("utf-8")
    
    return img_str

def enable_2fa(user_id: str) -> Dict[str, Any]:
    """Enable 2FA for a user and return QR code."""
    users_collection = get_users_collection()
    
    try:
        user_doc = users_collection.find_one({"id": user_id})
        
        if not user_doc:
            return {"success": False, "error": "User not found"}
        
        # Generate new TOTP secret
        totp_secret = generate_totp_secret()
        
        # Update user with TOTP secret
        users_collection.update_one(
            {"id": user_id},
            {"$set": {
                "totp_secret": totp_secret,
                "is_2fa_enabled": True
            }}
        )
        
        # Generate QR code
        qr_code = generate_qr_code(user_doc["email"], totp_secret)
        
        return {
            "success": True,
            "totp_secret": totp_secret,
            "qr_code": qr_code,
            "message": "Scan this QR code with Google Authenticator or similar app"
        }
    except Exception as e:
        return {"success": False, "error": str(e)}

def disable_2fa(user_id: str) -> Dict[str, Any]:
    """Disable 2FA for a user."""
    users_collection = get_users_collection()
    
    try:
        result = users_collection.update_one(
            {"id": user_id},
            {"$set": {
                "totp_secret": None,
                "is_2fa_enabled": False
            }}
        )
        
        if result.modified_count > 0:
            return {"success": True, "message": "2FA disabled successfully"}
        else:
            return {"success": False, "error": "User not found"}
    except Exception as e:
        return {"success": False, "error": str(e)}

def verify_totp(user: User, totp_code: str) -> bool:
    """Verify TOTP code."""
    if not user.totp_secret:
        return False
    
    totp = pyotp.TOTP(user.totp_secret)
    return totp.verify(totp_code, valid_window=1)  # Allow 1 step tolerance

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