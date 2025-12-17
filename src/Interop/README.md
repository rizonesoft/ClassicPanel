# ClassicPanel Interop Layer

This project provides interop support for C++ and native code in ClassicPanel.

## Purpose

The Interop layer enables:
- **C++/CLI Extensions**: Managed C++ extensions that can be loaded as .NET assemblies
- **Native C++ DLLs**: Pure C++ components loaded via P/Invoke or native library loading
- **C++/WinRT**: Modern C++ Windows Runtime support
- **Language Interop**: Bridge between C# and other languages

## Components

### NativeLibraryLoader
Loads native C++ DLLs dynamically at runtime.

### CppCliBridge
Bridge for C++/CLI components, allowing managed C++ code to interact with ClassicPanel.

### WinRTInterop
Windows Runtime interop for C++/WinRT components.

### ExtensionHost
Hosts extensions written in different languages (C++, etc.).

## Status

**Placeholder for future implementation** - This project structure is created but not yet implemented.

## Future Implementation

The Interop layer will provide:
- Dynamic loading of native DLLs
- C++/CLI bridge for managed C++ extensions
- C++/WinRT interop support
- Extension hosting for multi-language extensions

## Usage

Once implemented, native extensions can be loaded:

```csharp
var loader = new NativeLibraryLoader();
var nativeLib = loader.LoadLibrary("NativeExtension.dll");
```

