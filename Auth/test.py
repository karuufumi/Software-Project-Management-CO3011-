import requests
import json

BASE_URL = "http://localhost:8000"

def test_health():
    """Test health endpoint"""
    response = requests.get(f"{BASE_URL}/health")
    print("Health Check:", response.json())
    assert response.status_code == 200

def test_register():
    """Test user registration"""
    data = {
        "name": "Test User",
        "email": "test@example.com",
        "password": "testpass123",
        "role": "user"
    }
    response = requests.post(f"{BASE_URL}/register", json=data)
    print("\nRegister Response:", response.json())
    return response.status_code in [200, 400]  # 400 if user already exists

def test_login():
    """Test user login"""
    data = {
        "email": "test@example.com",
        "password": "testpass123"
    }
    response = requests.post(f"{BASE_URL}/login", json=data)
    print("\nLogin Response:", response.json())
    
    if response.status_code == 200:
        return response.json()["token"]
    return None

def test_protected_route(token):
    """Test protected route with token"""
    headers = {"Authorization": f"Bearer {token}"}
    response = requests.get(f"{BASE_URL}/me", headers=headers)
    print("\nProtected Route Response:", response.json())
    assert response.status_code == 200

def test_invalid_login():
    """Test login with wrong password"""
    data = {
        "email": "test@example.com",
        "password": "wrongpassword"
    }
    response = requests.post(f"{BASE_URL}/login", json=data)
    print("\nInvalid Login Response:", response.json())
    assert response.status_code == 401

def test_verify_token(token):
    """Test token verification"""
    headers = {"Authorization": f"Bearer {token}"}
    response = requests.post(f"{BASE_URL}/verify-token", headers=headers)
    print("\nVerify Token Response:", response.json())
    assert response.status_code == 200

if __name__ == "__main__":
    print("Starting Authentication Tests...\n")
    
    try:
        # Test 1: Health check
        print("=" * 50)
        print("TEST 1: Health Check")
        print("=" * 50)
        test_health()
        
        # Test 2: Register
        print("\n" + "=" * 50)
        print("TEST 2: User Registration")
        print("=" * 50)
        test_register()
        
        # Test 3: Login
        print("\n" + "=" * 50)
        print("TEST 3: User Login")
        print("=" * 50)
        token = test_login()
        
        if token:
            # Test 4: Protected route
            print("\n" + "=" * 50)
            print("TEST 4: Protected Route (Get Current User)")
            print("=" * 50)
            test_protected_route(token)
            
            # Test 5: Verify token
            print("\n" + "=" * 50)
            print("TEST 5: Verify Token")
            print("=" * 50)
            test_verify_token(token)
        
        # Test 6: Invalid login
        print("\n" + "=" * 50)
        print("TEST 6: Invalid Login")
        print("=" * 50)
        test_invalid_login()
        
        print("\n" + "=" * 50)
        print("✅ All tests completed!")
        print("=" * 50)
        
    except AssertionError as e:
        print(f"\n❌ Test failed: {e}")
    except Exception as e:
        print(f"\n❌ Error: {e}")