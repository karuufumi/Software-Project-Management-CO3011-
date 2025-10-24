import redis
from .config import settings

#redis_client = redis.from_url(settings.REDIS_URL, decode_responses=True)

#def blacklist_token(jti: str, exp: int):
 #   ttl = exp - int(__import__("time").time())
  #  redis_client.setex(f"bl_{jti}", ttl, "revoked")

#def is_token_blacklisted(jti: str):
 #   return redis_client.exists(f"bl_{jti}")