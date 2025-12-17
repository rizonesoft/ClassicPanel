# Frequently Asked Questions

## General Questions

### What is ClassicPanel?

ClassicPanel is a portable Windows application that replicates the classic Windows 7 Control Panel interface for Windows 10 and Windows 11.

### Why do I need ClassicPanel?

If you prefer the traditional Control Panel interface over the modern Settings app, ClassicPanel provides that familiar experience.

### Is ClassicPanel free?

Yes, ClassicPanel is free and open source.

### What operating systems are supported?

Windows 10 and Windows 11 (64-bit).

## Installation

### Do I need to install .NET?

No! ClassicPanel is compiled with Native AOT, so it's a standalone executable that doesn't require .NET to be installed.

### Can I run ClassicPanel from a USB drive?

Yes! ClassicPanel is fully portable and can run from any location, including USB drives.

### Where should I install ClassicPanel?

You can place ClassicPanel anywhere you prefer:
- `C:\Programs\ClassicPanel\` (portable)
- `C:\Users\YourName\AppData\Local\ClassicPanel\` (user-specific)
- A USB drive for portability

## Usage

### How do I add Control Panel applets?

1. Copy `.cpl` files from `C:\Windows\System32\` to the `system` folder next to `ClassicPanel.exe`
2. Refresh ClassicPanel (F5)

### Which .cpl files should I copy?

Common useful applets:
- `appwiz.cpl` - Programs and Features
- `desk.cpl` - Display Settings
- `main.cpl` - Mouse Properties
- `timedate.cpl` - Date and Time
- `sysdm.cpl` - System Properties

### Why don't some applets work?

Some applets may require:
- Administrator privileges
- Specific Windows components to be installed
- Network connectivity

### How do I change the view mode?

Use the **View** menu, toolbar buttons, or right-click context menu.

### Can I customize the icon size?

Yes, in Large Icons view, you can select icon sizes: 16, 24, 32, or 48 pixels.

## Troubleshooting

### ClassicPanel won't start

- Ensure you're running Windows 10 or 11 (64-bit)
- Check if the executable is blocked (right-click > Properties > Unblock)
- Run as administrator if needed

### Applets don't appear

- Check that `.cpl` files are in the `system` folder
- Press F5 to refresh
- Verify the files are valid `.cpl` files

### Applets won't run

- Some applets require administrator privileges (right-click ClassicPanel > Run as administrator)
- Ensure the .cpl file is not corrupted
- Check Windows Event Viewer for errors

### The interface looks wrong

- Ensure you have the latest version
- Try changing the view mode
- Check your display DPI settings

## Technical

### What technology is ClassicPanel built with?

- .NET 10
- C# 14
- Windows Forms
- Native AOT compilation

### Can I contribute to ClassicPanel?

Yes! ClassicPanel is open source. Check the repository for contribution guidelines.

### How do I report a bug?

File an issue on the GitHub repository with:
- Description of the problem
- Steps to reproduce
- Windows version
- Any error messages

## See Also

- [Troubleshooting Guide](troubleshooting.md)
- [User Manual](user-manual.md)
- [Installation Guide](installation.md)

