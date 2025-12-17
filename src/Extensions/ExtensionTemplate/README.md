# Extension Template

This is a template project for creating ClassicPanel extensions.

## Configuration

This extension project is automatically configured by `Directory.Build.props` to:
- Output to `system/` folder (build/debug/system/ or build/release/system/)
- Be framework-dependent (uses runtime from ClassicPanel.exe)
- Target .NET 10 Windows (x64)

## Creating a New Extension

1. Copy this template folder to create your extension
2. Rename the folder and .csproj file to your extension name
3. Update the project properties (Version, Authors, Description, etc.)
4. Implement your extension logic
5. Build - output will automatically go to `system/` folder

## Build Output

- **Debug**: `build/debug/system/YourExtension.dll`
- **Release**: `build/release/system/YourExtension.dll`
- **Published**: `build/publish/system/YourExtension.dll`

## Important Notes

- Extensions MUST be framework-dependent (NOT self-contained)
- Extensions use the runtime from ClassicPanel.exe
- See `docs/dev/extension-deployment.md` for detailed information

