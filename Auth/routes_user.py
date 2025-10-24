from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from fastapi.security import OAuth2PasswordBearer
from . import security, models, db, utils

router = APIRouter(prefix="/users", tags=["users"])
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="/auth/login")

def get_current_user(token: str = Depends(oauth2_scheme), db: Session = Depends(db.get_db)):
    payload = security.decode_token(token)
    if not payload:
        raise HTTPException(status_code=401, detail="Invalid token")
    #if utils.is_token_blacklisted(payload.get("jti")):
     #   raise HTTPException(status_code=401, detail="Token revoked")
    return db.query(models.User).filter(models.User.username == payload["sub"]).first()

def require_roles(roles: list[str]):
    def role_checker(user: models.User = Depends(get_current_user)):
        if user.role not in roles:
            raise HTTPException(status_code=403, detail="Forbidden")
        return user
    return role_checker

@router.get("/me")
def read_me(current_user: models.User = Depends(get_current_user)):
    return {"username": current_user.username, "role": current_user.role}

@router.get("/admin")
def admin_only(current_user: models.User = Depends(require_roles(["admin"]))):
    return {"msg": f"Hello admin {current_user.username}"}

@router.get("/staff-or-admin")
def staff_or_admin(current_user: models.User = Depends(require_roles(["admin", "staff"]))):
    return {"msg": f"Welcome {current_user.username}, role={current_user.role}"}