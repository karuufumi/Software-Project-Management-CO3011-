from db import init_mongodb, close_mongo_connection
import uvicorn
import os
from dotenv import load_dotenv
import atexit

# Load environment variables
load_dotenv()

if __name__ == "__main__":
    # Initialize MongoDB
    try:
        init_mongodb()
        print("MongoDB initialized successfully!")
    except Exception as e:
        print(f"Failed to initialize MongoDB: {e}")
        exit(1)
    
    # Register cleanup function
    atexit.register(close_mongo_connection)
    
    # Validate JWT_SECRET
    jwt_secret = os.getenv("JWT_SECRET")
    if not jwt_secret or jwt_secret == "your-secret-key":
        print("WARNING: Using default JWT_SECRET. Set JWT_SECRET environment variable!")
    
    # Get configuration from environment
    host = os.getenv("APP_HOST", "0.0.0.0")
    port = int(os.getenv("APP_PORT", "8000"))
    
    # Run FastAPI app
    print(f"Starting server on {host}:{port}")
    uvicorn.run(
        "routes:app",
        host=host,
        port=port,
        reload=False,
        log_level="info"
    )