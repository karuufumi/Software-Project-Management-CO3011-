import sqlite3

def get_conn(name:str):
    return sqlite3.connect(name)

def get_cursor(conn):
    return conn.cursor()

def close_conn(conn):
    conn.close()
    pass

def close_cursor(cursor):
    cursor.close()
    pass