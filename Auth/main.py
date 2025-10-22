from typing import Union

from fastapi import FastAPI

import model
import controller
import jwt

'''
This is a microservice for authentication and authorization of the Library Management System.
It provides endpoints for user registration, login, and token validation.
We use the robust JWT (JSON Web Token) standard for secure token-based authentication.
'''


app = FastAPI()



@app.get("/")
def read_root():
    return {"Hello": "World 2332122"}

@app.get("/users/{user_id}")
def read_user(user_id: int):
    return {"user_id": user_id}
#    pass

@app.post("/users/")
def create_user(name: str, email: str):
    return {"name": name, "email": email}


