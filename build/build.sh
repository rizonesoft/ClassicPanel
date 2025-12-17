#!/bin/bash
# ClassicPanel Build Script (Linux/macOS/Unix)
# Builds ClassicPanel with Debug or Release configuration
# Usage: ./build.sh [Debug|Release] [--publish]
#   Debug: Build in Debug configuration (default)
#   Release: Build in Release configuration
#   --publish: Also publish self-contained executable after successful build

set -e  # Exit on error

# Default to Release if no argument provided
CONFIG="Release"
PUBLISH=false

# Parse arguments
if [ "$1" = "Debug" ] || [ "$1" = "debug" ]; then
    CONFIG="Debug"
elif [ "$1" = "Release" ] || [ "$1" = "release" ]; then
    CONFIG="Release"
fi

if [ "$1" = "--publish" ] || [ "$2" = "--publish" ]; then
    PUBLISH=true
fi

if [ "$2" = "Debug" ] || [ "$2" = "debug" ]; then
    CONFIG="Debug"
elif [ "$2" = "Release" ] || [ "$2" = "release" ]; then
    CONFIG="Release"
fi

echo "========================================"
echo "ClassicPanel Build Script"
echo "========================================"
echo "Configuration: $CONFIG"
echo "Publish: $PUBLISH"
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
    OUTPUT_PATH="$PROJECT_ROOT/build/debug/net10.0-windows/win-x64/ClassicPanel.exe"
else
    OUTPUT_PATH="$PROJECT_ROOT/build/release/net10.0-windows/win-x64/ClassicPanel.exe"
fi

if [ -f "$OUTPUT_PATH" ]; then
    echo "Output: $OUTPUT_PATH"
    echo ""
else
    echo "WARNING: Expected output file not found at $OUTPUT_PATH"
    echo ""
fi

# Publish if requested or if Release build
if [ "$PUBLISH" = true ] || [ "$CONFIG" = "Release" ] || [ "$CONFIG" = "release" ]; then
    echo "========================================"
    echo "Publishing self-contained executable..."
    echo "========================================"
    echo ""
    
    dotnet publish -c "$CONFIG" \
        -p:PublishSingleFile=true \
        -p:PublishReadyToRun=true \
        -p:SelfContained=true \
        -p:RuntimeIdentifier=win-x64 \
        -p:GenerateAssemblyInfo=false
    
    if [ $? -ne 0 ]; then
        echo ""
        echo "========================================"
        echo "PUBLISH FAILED"
        echo "========================================"
        exit 1
    fi
    
    echo ""
    echo "========================================"
    echo "PUBLISH SUCCEEDED"
    echo "========================================"
    echo ""
    
    PUBLISH_PATH="$PROJECT_ROOT/build/publish/ClassicPanel.exe"
    if [ -f "$PUBLISH_PATH" ]; then
        echo "Published executable: $PUBLISH_PATH"
        echo ""
        # Get file size
        if command -v stat &> /dev/null; then
            if [[ "$OSTYPE" == "darwin"* ]]; then
                # macOS
                SIZE=$(stat -f%z "$PUBLISH_PATH")
            else
                # Linux
                SIZE=$(stat -c%s "$PUBLISH_PATH")
            fi
            SIZE_MB=$((SIZE / 1048576))
            echo "Size: ${SIZE_MB} MB (self-contained with ReadyToRun)"
            echo ""
        fi
    else
        echo "WARNING: Published executable not found at $PUBLISH_PATH"
        echo ""
    fi
fi

echo "========================================"
echo "Build Complete"
echo "========================================"
echo ""
echo "To run the application:"
if [ "$CONFIG" = "Debug" ] || [ "$CONFIG" = "debug" ]; then
    echo "  Debug:   $PROJECT_ROOT/build/debug/net10.0-windows/win-x64/ClassicPanel.exe"
else
    echo "  Release: $PROJECT_ROOT/build/release/net10.0-windows/win-x64/ClassicPanel.exe"
fi
if [ "$PUBLISH" = true ] || [ "$CONFIG" = "Release" ] || [ "$CONFIG" = "release" ]; then
    echo "  Published: $PROJECT_ROOT/build/publish/ClassicPanel.exe"
fi
echo ""
exit 0

