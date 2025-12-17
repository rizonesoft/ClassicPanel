# Installation Guide

## Portable Installation (Recommended)

ClassicPanel is a portable application and doesn't require installation.

### Steps

1. **Download** ClassicPanel from the releases page
2. **Extract** the ZIP file to your desired location (e.g., `C:\Programs\ClassicPanel\`)
3. **Run** `ClassicPanel.exe`

That's it! ClassicPanel is ready to use.

### Directory Structure

After extraction, you'll have:

```
ClassicPanel/
├── ClassicPanel.exe    # Main executable
└── system/             # Place .cpl files here (created automatically)
```

## Installed Version

If you prefer an installed version:

1. **Download** the installer (`.exe` or `.msi`)
2. **Run** the installer
3. **Follow** the installation wizard
4. Launch ClassicPanel from the Start Menu

## Adding Control Panel Applets

1. **Locate** the `system` folder next to `ClassicPanel.exe`
2. **Copy** `.cpl` files from `C:\Windows\System32\` (e.g., `appwiz.cpl`, `desk.cpl`)
3. **Paste** them into the `system` folder
4. **Restart** ClassicPanel or click Refresh

**Note**: Some system `.cpl` files may require administrator privileges to execute.

## Updating

### Portable Version

1. Download the new version
2. Replace `ClassicPanel.exe` with the new version
3. Your `system` folder and settings are preserved

### Installed Version

1. Run the new installer
2. It will upgrade the existing installation
3. Your settings are preserved

## Uninstallation

### Portable Version

Simply delete the ClassicPanel folder.

### Installed Version

1. Open Settings > Apps
2. Find ClassicPanel
3. Click Uninstall

## Troubleshooting

See the [Troubleshooting Guide](troubleshooting.md) if you encounter issues during installation or use.

