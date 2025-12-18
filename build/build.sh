#!/bin/bash
# ClassicPanel Build Script (Linux/macOS/Unix)
# Builds ClassicPanel with Debug or Release configuration
# Usage: ./build.sh [Debug|Release]
#   Debug: Build in Debug configuration (default)
#   Release: Build in Release configuration

set -e  # Exit on error

# Default to Release if no argument provided
CONFIG="Release"

# Parse arguments
if [ "$1" = "Debug" ] || [ "$1" = "debug" ]; then
    CONFIG="Debug"
elif [ "$1" = "Release" ] || [ "$1" = "release" ]; then
    CONFIG="Release"
fi

echo "========================================"
echo "ClassicPanel Build Script"
echo "========================================"
echo "Configuration: $CONFIG"
echo "========================================"
echo ""

# Check if .NET SDK is available
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK not found. Please install .NET 10 SDK."
    exit 1
fi

# Get script directory and change to project root
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
SRC_DIR="$PROJECT_ROOT/src"

if [ ! -d "$SRC_DIR" ]; then
    echo "ERROR: Source directory not found: $SRC_DIR"
    exit 1
fi

cd "$SRC_DIR"

# Clean previous build (optional - uncomment if needed)
# echo "Cleaning previous build..."
# dotnet clean -c "$CONFIG" -p:GenerateAssemblyInfo=false || echo "WARNING: Clean failed, continuing with build..."

# Build the project
echo "Building ClassicPanel ($CONFIG configuration)..."
echo ""
dotnet build -c "$CONFIG" -p:GenerateAssemblyInfo=false

if [ $? -ne 0 ]; then
    echo ""
    echo "========================================"
    echo "BUILD FAILED"
    echo "========================================"
    exit 1
fi

echo ""
echo "========================================"
echo "BUILD SUCCEEDED"
echo "========================================"
echo ""

# Determine output path
if [ "$CONFIG" = "Debug" ] || [ "$CONFIG" = "debug" ]; then
    OUTPUT_PATH="$PROJECT_ROOT/build/debug/ClassicPanel.exe"
else
    OUTPUT_PATH="$PROJECT_ROOT/build/release/ClassicPanel.exe"
fi

if [ -f "$OUTPUT_PATH" ]; then
    echo "Output: $OUTPUT_PATH"
    echo ""
else
    echo "WARNING: Expected output file not found at $OUTPUT_PATH"
    echo ""
fi


echo "========================================"
echo "Build Complete"
echo "========================================"
echo ""
echo "To run the application:"
if [ "$CONFIG" = "Debug" ] || [ "$CONFIG" = "debug" ]; then
    echo "  Debug:   $PROJECT_ROOT/build/debug/ClassicPanel.exe"
else
    echo "  Release: $PROJECT_ROOT/build/release/ClassicPanel.exe"
fi
echo ""
exit 0

