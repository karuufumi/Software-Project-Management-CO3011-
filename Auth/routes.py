from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from datetime import timedelta, datetime
from uuid import uuid4

from . import models, schemas, security, db, config

router = APIRouter(prefix="/auth", tags=["auth"])

@router.post("/register", response_model=schemas.UserResponse)
def register(user: schemas.UserCreate, db: Session = Depends(db.get_db)):
    if db.query(models.User).filter(models.User.email == user.email).first():
        raise HTTPException(status_code=400, detail="Email already registered")
    new_user = models.User(
        username=user.username,
        email=user.email,
        hashed_password=security.hash_password(user.password),
        role=user.role
    )
    db.add(new_user)
    db.commit()
    db.refresh(new_user)
    return new_user

@router.post("/login", response_model=schemas.Token)
def login(form: schemas.UserCreate, db: Session = Depends(db.get_db)):
    user = db.query(models.User).filter(models.User.username == form.username).first()
    if not user or not security.verify_password(form.password, user.hashed_password):
        raise HTTPException(status_code=401, detail="Invalid credentials")

    jti = str(uuid4())
    access_token = security.create_token(
        {"sub": user.username, "role": user.role, "jti": jti},
        timedelta(minutes=config.settings.ACCESS_TOKEN_EXPIRE_MINUTES),
    )
    refresh_token = security.create_token(
        {"sub": user.username, "jti": str(uuid4())},
        timedelta(days=config.settings.REFRESH_TOKEN_EXPIRE_DAYS),
        is_refresh=True
    )

    db.add(models.RefreshToken(
        token=refresh_token,
        user_id=user.id,
        expiry=datetime.utcnow() + timedelta(days=config.settings.REFRESH_TOKEN_EXPIRE_DAYS)
    ))
    db.commit()

    return {"access_token": access_token, "refresh_token": refresh_token, "token_type": "bearer"}

@router.post("/refresh", response_model=schemas.Token)
def refresh_token(token: str, db: Session = Depends(db.get_db)):
    payload = security.decode_token(token)
    if not payload or not payload.get("refresh"):
        raise HTTPException(status_code=401, detail="Invalid refresh token")

    db_token = db.query(models.RefreshToken).filter_by(token=token, revoked=False).first()
    #if not db_token or db_token.expiry < datetime.utcnow():
     #   raise HTTPException(status_code=401, detail="Expired or revoked refresh token")

    jti = str(uuid4())
    new_access = security.create_token(
        {"sub": payload["sub"], "jti": jti},
        timedelta(minutes=config.settings.ACCESS_TOKEN_EXPIRE_MINUTES)
    )
    return {"access_token": new_access, "refresh_token": token, "token_type": "bearer"}

@router.post("/logout")
def logout(token: str, db: Session = Depends(db.get_db)):
    payload = security.decode_token(token)
    if not payload:
        raise HTTPException(status_code=401, detail="Invalid token")
    jti = payload.get("jti")
    exp = payload.get("exp")
    #utils.blacklist_token(jti, exp)
    return {"msg": "Logged out"}