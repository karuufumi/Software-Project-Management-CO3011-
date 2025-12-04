#hashing with SHA256
import hashlib
import os
import jwt
from typing import Optional, Dict, Any

class payload:
    user_id:str
    role:str
    
    def to_dict(self):
        return {
            "user_id": self.user_id,
            "role": self.role
        }
    pass

def hash_string(input_string):
    """Returns the SHA256 hash of the input string."""
    sha256_hash = hashlib.sha256()
    sha256_hash.update(input_string.encode('utf-8'))
    return sha256_hash.hexdigest()   

def verify_hash(input_string, expected_hash):
    """Verifies if the SHA256 hash of the input string matches the expected hash."""
    return hash_string(input_string) == expected_hash

    

def generate_token(payload: payload, secret_key: str, algorithm: str = 'HS256') -> str:
    """Generates a JWT token with the given payload."""
    token = jwt.encode(payload.to_dict(), secret_key, algorithm=algorithm)
    return token



def verify_token(token: str, secret_key: str) -> Optional[Dict[str, Any]]:
    """Verify and decode JWT token."""
    try:
        decoded = jwt.decode(token, secret_key, algorithms=["HS256"])
        return decoded
    except jwt.ExpiredSignatureError:
        return None
    except jwt.InvalidTokenError:
        return None