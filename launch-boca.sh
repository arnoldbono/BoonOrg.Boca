#!/bin/zsh

# Launch Boca application
# Get the directory where this script is located
SCRIPT_DIR="${0:A:h}"

# Path to the Boca executable
BOCA_EXECUTABLE="${SCRIPT_DIR}/Output/Boca"

# Check if the executable exists
if [[ ! -f "$BOCA_EXECUTABLE" ]]; then
    echo "Error: Boca executable not found at $BOCA_EXECUTABLE"
    echo "Please build the project first using: dotnet build"
    exit 1
fi

# Launch Boca with all passed arguments
"$BOCA_EXECUTABLE"  "$@" -f "~Development/BoonOrg.DEMO/Examples/glXML-Import.i" --application BoonOrg.DEMO --module BoonOrg.USD --module BoonOrg.Horn --module BoonOrg.Mandelbrot3D --module BoonOrg.PAT --module BoonOrg.DEMO
