from pydantic import BaseModel
from enum import Enum
from typing import Optional

class Role(str, Enum):
    ADMIN = "admin"
    USER = "user"
    GUEST = "guest"
    MEMB = "member"

class User(BaseModel):
    id: str
    name: str
    email: str
    hashedpwd: str
    role: Role
    totp_secret: Optional[str] = None  # TOTP secret key
    is_2fa_enabled: bool = False  # Whether 2FA is enabled