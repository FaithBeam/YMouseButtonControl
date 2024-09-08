using System;
using System.Runtime.InteropServices;
using System.Text;
using YMouseButtonControl.Core.Processes;
using CFIndex = long;

namespace YMouseButtonControl.Services.MacOS;

// a lot of this code was copied from https://stackoverflow.com/a/44669560 and https://github.com/isnowrain/CoreFoundation/blob/master/Project/CFType.cs
public class CurrentWindowService : ICurrentWindowService
{
    public string ForegroundWindow => GetForegroundWindow();

    private string GetForegroundWindow()
    {
        var nsWorkspace = objc_getClass("NSWorkspace");
        var sharedWorkspace = objc_msgSend_retIntPtr(nsWorkspace, GetSelector("sharedWorkspace"));
        var frontmostApplication = objc_msgSend_retIntPtr(
            sharedWorkspace,
            GetSelector("frontmostApplication")
        );
        var localizedName = objc_msgSend_retIntPtr(
            frontmostApplication,
            GetSelector("localizedName")
        );
        return GetStrFromCFString(localizedName);
    }

    private const string AppKitFramework = "/System/Library/Frameworks/AppKit.framework/AppKit";
    private const string FoundationFramework =
        "/System/Library/Frameworks/Foundation.framework/Foundation";
    private const string CoreFoundation =
        "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

    [DllImport(AppKitFramework, CharSet = CharSet.Ansi)]
    public static extern IntPtr objc_getClass(string name);

    [DllImport(FoundationFramework, EntryPoint = "objc_msgSend")]
    public static extern IntPtr objc_msgSend_retIntPtr(IntPtr target, IntPtr selector);

    public static IntPtr GetSelector(string name)
    {
        IntPtr cfstrSelector = CreateCFString(name);
        IntPtr selector = NSSelectorFromString(cfstrSelector);
        CFRelease(cfstrSelector);
        return selector;
    }

    [DllImport(CoreFoundation)]
    static extern CFIndex CFStringGetLength(nint theString);

    [DllImport(CoreFoundation)]
    static extern void CFStringGetCharacters(nint theString, CFRange range, nint buffer);

    private static unsafe string GetStrFromCFString(nint ptr)
    {
        var length = CFStringGetLength(ptr);
        var u = CFStringGetCharactersPtr(ptr);
        var buffer = IntPtr.Zero;
        if (u == IntPtr.Zero)
        {
            var range = new CFRange(0, length);
            buffer = Marshal.AllocCoTaskMem((int)(length * 2));
            CFStringGetCharacters(ptr, range, buffer);
            u = buffer;
        }

        var str = new string((char*)u, 0, (int)length);
        if (buffer != IntPtr.Zero)
            Marshal.FreeCoTaskMem(buffer);
        return str;
    }

    [DllImport(FoundationFramework)]
    public static extern void CFRelease(IntPtr handle);

    [DllImport(CoreFoundation)]
    static extern nint CFStringGetCharactersPtr(nint theString);

    [DllImport(AppKitFramework)]
    public static extern IntPtr NSSelectorFromString(IntPtr cfstr);

    public static unsafe IntPtr CreateCFString(string aString)
    {
        var bytes = Encoding.Unicode.GetBytes(aString);
        fixed (byte* b = bytes)
        {
            var cfStr = CFStringCreateWithBytes(
                IntPtr.Zero,
                (IntPtr)b,
                bytes.Length,
                CFStringEncoding.UTF16,
                false
            );
            return cfStr;
        }
    }

    [DllImport(FoundationFramework)]
    public static extern IntPtr CFStringCreateWithBytes(
        IntPtr allocator,
        IntPtr buffer,
        long bufferLength,
        CFStringEncoding encoding,
        bool isExternalRepresentation
    );

    public enum CFStringEncoding : uint
    {
        UTF16 = 0x0100,
        UTF16BE = 0x10000100,
        UTF16LE = 0x14000100,
        ASCII = 0x0600,
    }

    [StructLayout(LayoutKind.Sequential)]
    struct CFRange(CFIndex length, CFIndex location)
    {
        public CFIndex Length = length;
        public CFIndex Location = location;
    }
}
