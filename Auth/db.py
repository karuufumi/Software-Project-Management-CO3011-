from pymongo import MongoClient
from pymongo.errors import ConnectionFailure
import os
from dotenv import load_dotenv

load_dotenv()

# MongoDB Configuration
MONGO_URI = os.getenv("uri", "mongodb://localhost:27017/")
MONGO_DB_NAME = os.getenv("MONGO_DB_NAME", "lms-auth")

_client = None
_db = None

def get_mongo_client():
    """Get MongoDB client singleton."""
    global _client
    if _client is None:
        _client = MongoClient(MONGO_URI, serverSelectionTimeoutMS=5000)
        try:
            # Verify connection
            _client.admin.command('ping')
            print("MongoDB connection successful!")
        except ConnectionFailure as e:
            print(f"MongoDB connection failed: {e}")
            raise
    return _client

def get_database():
    """Get MongoDB database."""
    global _db
    if _db is None:
        client = get_mongo_client()
        _db = client[MONGO_DB_NAME]
    return _db

def get_users_collection():
    """Get users collection."""
    db = get_database()
    return db['users']

def close_mongo_connection():
    """Close MongoDB connection."""
    global _client, _db
    if _client:
        _client.close()
        _client = None
        _db = None

def init_mongodb():
    """Initialize MongoDB indexes."""
    db = get_database()
    users = db['users']
    
    try:
        '''Create indexes if they don't exist'''
        # Create unique index on email
        users.create_index("email", unique=True)
        print("Created unique index on email")
        
        # Create index on id for faster lookups
        users.create_index("id", unique=True)
        print("Created unique index on id")
        
        print("MongoDB indexes created successfully!")
    except Exception as e:
        print(f"Error creating indexes: {e}")
        raise

    
if __name__ == "__main__":
    init_mongodb()