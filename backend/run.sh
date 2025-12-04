#!/bin/bash

echo "🚀 Starting Library Management API..."

# Create data directory if it doesn't exist
mkdir -p ./data

docker-compose up -d

echo ""
echo "✅ Application is running!"
echo "📍 Swagger UI: http://localhost:5000/swagger"
echo "📍 API Base: http://localhost:5000/api"
echo ""
echo "To view logs: docker-compose logs -f"
echo "To stop: docker-compose down"