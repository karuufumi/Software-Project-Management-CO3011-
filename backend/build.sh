#!/bin/bash

echo "🔨 Building Docker image..."

# Create data directory
mkdir -p ./data

docker-compose build

echo ""
echo "✅ Build complete!"
echo "Run with: ./run.sh"