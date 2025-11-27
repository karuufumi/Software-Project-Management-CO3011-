from pydantic import BaseModel

from enum import Enum

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
