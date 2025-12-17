# User Manual

Complete guide to using ClassicPanel.

## Table of Contents

1. [Getting Started](#getting-started)
2. [Interface Overview](#interface-overview)
3. [View Modes](#view-modes)
4. [Managing Applets](#managing-applets)
5. [Menu Reference](#menu-reference)
6. [Keyboard Shortcuts](#keyboard-shortcuts)
7. [Settings](#settings)

## Getting Started

### First Launch

When you first launch ClassicPanel:

1. The main window opens
2. A `system` folder is created next to the executable (if needed)
3. The ListView is empty until you add .cpl files

### Adding Applets

1. Copy `.cpl` files to the `system` folder
2. Refresh the view (F5 or File > Refresh)
3. Applets appear in the ListView

## Interface Overview

### Main Window Components

- **Menu Bar**: File, View, and Help menus
- **Toolbar**: Quick access buttons for view modes
- **ListView**: Displays Control Panel applets
- **Status Bar** (if enabled): Shows item count and status

### ListView Columns (Details View)

- **Icon**: Applet icon
- **Name**: Display name of the applet
- **Description**: Tooltip/description text
- **Path**: Full path to the .cpl file

## View Modes

ClassicPanel supports four view modes:

### Large Icons

- Displays large icons (32x32, 48x48, or custom size)
- Icons arranged in a grid
- Best for visual browsing

### Small Icons

- Displays small icons (16x16)
- Compact, classic Control Panel look
- Icons arranged horizontally

### List

- Small icons in a vertical list
- Single column
- Good for many items

### Details

- Shows icon, name, description, and path
- Sortable columns
- Most information visible

### Changing View Mode

**Via Menu:**
1. Go to **View** menu
2. Select desired view mode

**Via Toolbar:**
- Click the view mode button

**Via Context Menu:**
1. Right-click in ListView
2. Select view mode

## Managing Applets

### Opening an Applet

- Double-click the applet, or
- Right-click and select **Open**

### Refreshing the List

- Press `F5`, or
- Go to **File > Refresh**, or
- Click Refresh button

### Adding Applets

1. Copy `.cpl` files to the `system` folder
2. Refresh the view

### Removing Applets

1. Delete the `.cpl` file from the `system` folder
2. Refresh the view

## Menu Reference

### File Menu

- **Refresh** (F5): Reload applets from system folder
- **Exit** (Alt+F4): Close ClassicPanel

### View Menu

- **Large Icons**: Switch to large icon view
- **Small Icons**: Switch to small icon view
- **List**: Switch to list view
- **Details**: Switch to details view
- **Icon Size**: Choose icon size (16, 24, 32, 48) - for Large Icons view

### Help Menu

- **About ClassicPanel...**: Show version and information

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `F5` | Refresh applet list |
| `Alt+F4` | Exit application |
| `Ctrl+Q` | Exit application |
| `F1` | Show About dialog (coming soon) |

## Settings

### View Preferences

ClassicPanel remembers:
- Selected view mode
- Icon size preference
- Window position and size (coming soon)

Settings are saved automatically and restored on next launch.

### Configuration File

Settings are stored in a configuration file (location TBD):
- Automatically created on first run
- Can be edited manually if needed
- Resets if deleted

## Troubleshooting

See the [Troubleshooting Guide](troubleshooting.md) for solutions to common issues.

## Tips

- Keep your `system` folder organized
- Some applets require administrator privileges
- Refresh if applets don't appear after adding files
- Use Details view to see full paths

