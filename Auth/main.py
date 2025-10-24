from fastapi import FastAPI
from . import models,db 
from .routes import router as auth_router
from .routes_user import router as users_router

models.Base.metadata.create_all(bind=db.engine)

app = FastAPI(title="Advanced Auth Microservice with Roles")
app.include_router(auth_router)
app.include_router(users_router)