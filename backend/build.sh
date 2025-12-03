#!/bin/bash

echo "🔨 Building Docker image..."
docker build -t library-management-api:latest .

echo "✅ Build complete!"
echo "Run with: ./run.sh"