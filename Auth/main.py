from db import get_conn, get_cursor, close_conn, close_cursor
import uvicorn

def init_database():
    conn = get_conn("auth.db")
    cursor = get_cursor(conn)
    
    cursor.execute("""
        CREATE TABLE IF NOT EXISTS users (
            id TEXT PRIMARY KEY,
            name TEXT NOT NULL,
            email TEXT UNIQUE NOT NULL,
            hashedpwd TEXT NOT NULL,
            role TEXT NOT NULL
        )
    """)
    
    conn.commit()
    close_cursor(cursor)
    close_conn(conn)

if __name__ == "__main__":
    # Initialize database
    init_database()
    print("Database initialized successfully!")
    
    # Run FastAPI app
    uvicorn.run("routes:app", host="127.0.0.1", port=8020, reload=True)