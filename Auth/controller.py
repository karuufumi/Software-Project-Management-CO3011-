class User:
    def __init__(self, user_id, name, email, hashed_password):
        self.user_id = user_id
        self.name = name
        self.email = email
        self.hashed_password = hashed_password

    def get_user_info(self):
        return {
            "user_id": self.user_id,
            "name": self.name,
            "email": self.email,
            
        }
from typing import Union
from fastapi import FastAPI
