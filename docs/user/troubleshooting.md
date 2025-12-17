# Troubleshooting Guide

Solutions to common issues with ClassicPanel.

## Application Won't Start

### Issue: "This app can't run on your PC"

**Solution:**
- Ensure you're running Windows 10 or 11 (64-bit)
- Download the correct version (x64) for your system

### Issue: Executable is blocked

**Solution:**
1. Right-click `ClassicPanel.exe`
2. Select **Properties**
3. Check **Unblock** at the bottom
4. Click **OK**
5. Try running again

### Issue: Application crashes on startup

**Solution:**
- Run as administrator
- Check Windows Event Viewer for error details
- Ensure you have the latest version
- Report the issue with error details

## Applets Don't Appear

### Issue: ListView is empty

**Solutions:**
1. **Check the system folder:**
   - Ensure `.cpl` files are in the `system` folder next to `ClassicPanel.exe`
   - The folder is created automatically on first run

2. **Refresh the view:**
   - Press `F5` or go to **File > Refresh**

3. **Verify file names:**
   - Files must have `.cpl` extension
   - Files must be valid Control Panel applets

4. **Check file permissions:**
   - Ensure ClassicPanel can read the files
   - Run as administrator if needed

### Issue: Some applets appear but not others

**Solutions:**
- Some `.cpl` files may be corrupted
- Some applets may require specific Windows components
- Try copying the file again from `C:\Windows\System32\`
- Check if the file loads in Windows' own Control Panel

## Applets Won't Execute

### Issue: Nothing happens when double-clicking

**Solutions:**
1. **Run as administrator:**
   - Right-click `ClassicPanel.exe`
   - Select **Run as administrator**
   - Some applets require elevated privileges

2. **Check the applet:**
   - Try running the applet directly from Windows Control Panel
   - Verify the `.cpl` file is not corrupted

3. **Check Windows Event Viewer:**
   - Look for errors related to the applet
   - Note the error code and message

### Issue: "Access Denied" or permission errors

**Solution:**
- Run ClassicPanel as administrator
- Some applets require administrator privileges to execute

### Issue: Applet starts but closes immediately

**Possible causes:**
- The applet may require specific parameters
- Missing Windows components
- Compatibility issue

**Solution:**
- Check if the applet works in Windows' Control Panel
- Try a different applet to isolate the issue

## Interface Issues

### Issue: Icons are missing

**Solutions:**
1. **Refresh the view:**
   - Press `F5` to reload icons

2. **Check icon extraction:**
   - Some .cpl files may not have extractable icons
   - Icons are loaded from the .cpl file's resources

3. **Try a different view mode:**
   - Switch to Details view to see if names appear

### Issue: Interface looks blurry or scaled incorrectly

**Solutions:**
1. **Check DPI settings:**
   - Right-click `ClassicPanel.exe` > Properties
   - Go to Compatibility tab
   - Check "Override high DPI scaling behavior"
   - Select "System" or "System (Enhanced)"

2. **Adjust Windows display scaling:**
   - Windows Settings > System > Display
   - Try different scaling percentages

### Issue: Window is too small/large

**Solution:**
- Resize the window manually
- Window size should be remembered (coming soon)
- Check if window is maximized/minimized

## Performance Issues

### Issue: Application is slow to start

**Possible causes:**
- Many .cpl files in system folder
- Slow disk (USB drive)
- Antivirus scanning

**Solutions:**
- Reduce the number of .cpl files
- Move ClassicPanel to a faster drive
- Add ClassicPanel to antivirus exclusions

### Issue: Icons load slowly

**Solution:**
- Icons are extracted on first load and cached
- Subsequent launches should be faster
- Reduce icon size in Large Icons view

## Settings Issues

### Issue: Settings are not saved

**Solutions:**
1. **Check write permissions:**
   - Ensure ClassicPanel can write to its directory
   - Run as administrator if needed

2. **Check settings file:**
   - Settings file should be created automatically
   - If corrupted, delete it and let ClassicPanel recreate it

### Issue: View mode doesn't persist

**Solution:**
- This feature may not be fully implemented yet
- Check TODO.md for implementation status
- Manually select view mode on each launch

## Getting Help

If you've tried the solutions above and still have issues:

1. **Check the version:**
   - Go to Help > About to see your version
   - Ensure you have the latest version

2. **Gather information:**
   - Windows version (Win+R > winver)
   - ClassicPanel version
   - Error messages (if any)
   - Steps to reproduce

3. **Report the issue:**
   - File an issue on GitHub
   - Include the information gathered above

## See Also

- [FAQ](faq.md)
- [User Manual](user-manual.md)
- [Installation Guide](installation.md)

