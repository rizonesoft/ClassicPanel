# Quick Start Guide

Get up and running with ClassicPanel in minutes!

## First Launch

1. **Run** `ClassicPanel.exe`
2. The main window opens showing an empty ListView
3. A `system` folder is created next to the executable (if it doesn't exist)

## Adding Your First Applet

1. **Copy** a `.cpl` file from `C:\Windows\System32\`
   - Example: `appwiz.cpl` (Programs and Features)
   - Example: `desk.cpl` (Display Settings)

2. **Paste** it into the `system` folder next to `ClassicPanel.exe`

3. **Refresh** ClassicPanel:
   - Press `F5`, or
   - Go to **File > Refresh**, or
   - Click the Refresh button in the toolbar

4. Your applet should now appear in the ListView!

## Using ClassicPanel

### Opening an Applet

- **Double-click** an applet in the ListView, or
- **Right-click** and select **Open**

### Changing View Mode

- **Menu**: Go to **View** menu and select:
  - Large Icons
  - Small Icons
  - List
  - Details

- **Toolbar**: Click the view mode buttons

- **Context Menu**: Right-click in the ListView and select a view mode

### Keyboard Shortcuts

- `F5`: Refresh applet list
- `Ctrl+Q` or `Alt+F4`: Exit application
- `F1`: Show About dialog (coming soon)

## Next Steps

- Read the [User Manual](user-manual.md) for detailed information
- Check the [FAQ](faq.md) for common questions
- Explore different view modes and find your preference

## Tips

- You can copy multiple `.cpl` files to the `system` folder
- ClassicPanel automatically detects new files on refresh
- Some applets may require administrator privileges to run

