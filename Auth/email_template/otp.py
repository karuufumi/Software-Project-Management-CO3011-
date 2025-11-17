
import pyotp
import qrcode
    
def generate_otp_secret() -> str:
    """Generates a base32 OTP secret."""
    return pyotp.random_base32()


secret = generate_otp_secret()
print("Generated OTP Secret:", secret)
    
    # Generate QR code for the OTP secret