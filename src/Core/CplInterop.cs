using System.Runtime.InteropServices;

namespace ClassicPanel.Core;

/// <summary>
/// P/Invoke definitions for Windows Control Panel (.cpl) interop.
/// </summary>
public static class CplInterop
{
    // CPL Messages (from Cpl.h)
    public const uint CPL_INIT = 1;
    public const uint CPL_GETCOUNT = 2;
    public const uint CPL_INQUIRE = 3;
    public const uint CPL_NEWINQUIRE = 0x001A;
    public const uint CPL_DBLCLK = 5;
    public const uint CPL_EXIT = 6;
    public const uint CPL_STOP = 7;
    public const uint CPL_EXITCODE = 8;

    // The function signature every .cpl file exports
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int CPlAppletDelegate(nint hwndCPl, uint uMsg, nint lParam1, nint lParam2);

    // Structure for CPL_INQUIRE (Ancient Windows format)
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CPLINFO
    {
        public int idIcon;     // Resource ID of the icon
        public int idName;     // Resource ID of the name string
        public int idInfo;     // Resource ID of the description string
        public nint lData;     // User data
    }

    // Structure for CPL_NEWINQUIRE (Extended format, Unicode support)
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NEWCPLINFO
    {
        public uint dwSize;
        public uint dwFlags;
        public uint dwHelpContext;
        public nint lData;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szInfo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szHelpFile;
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern nint LoadIcon(nint hInstance, nint lpIconName);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern nint LoadImage(
        nint hinst,
        nint lpszName,
        uint uType,
        int cxDesired,
        int cyDesired,
        uint fuLoad);
}

