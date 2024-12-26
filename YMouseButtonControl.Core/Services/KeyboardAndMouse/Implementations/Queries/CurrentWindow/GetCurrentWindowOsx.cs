using System;
using System.Runtime.InteropServices;
using System.Text;
using CFIndex = long;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.CurrentWindow;

// a lot of this code was copied from https://stackoverflow.com/a/44669560 and https://github.com/isnowrain/CoreFoundation/blob/master/Project/CFType.cs
public class GetCurrentWindowOsx : IGetCurrentWindow
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
        return GetStrFromCfString(localizedName);
    }

    private const string AppKitFramework = "/System/Library/Frameworks/AppKit.framework/AppKit";
    private const string FoundationFramework =
        "/System/Library/Frameworks/Foundation.framework/Foundation";
    private const string CoreFoundation =
        "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

    [DllImport(AppKitFramework, CharSet = CharSet.Ansi)]
    public static extern nint objc_getClass(string name);

    [DllImport(FoundationFramework, EntryPoint = "objc_msgSend")]
    public static extern nint objc_msgSend_retIntPtr(nint target, nint selector);

    public static nint GetSelector(string name)
    {
        nint cfstrSelector = CreateCfString(name);
        nint selector = NSSelectorFromString(cfstrSelector);
        CFRelease(cfstrSelector);
        return selector;
    }

    [DllImport(CoreFoundation)]
    static extern CFIndex CFStringGetLength(nint theString);

    [DllImport(CoreFoundation)]
    static extern void CFStringGetCharacters(nint theString, CfRange range, nint buffer);

    private static unsafe string GetStrFromCfString(nint ptr)
    {
        var length = CFStringGetLength(ptr);
        var u = CFStringGetCharactersPtr(ptr);
        var buffer = nint.Zero;
        if (u == nint.Zero)
        {
            var range = new CfRange(0, length);
            buffer = Marshal.AllocCoTaskMem((int)(length * 2));
            CFStringGetCharacters(ptr, range, buffer);
            u = buffer;
        }

        var str = new string((char*)u, 0, (int)length);
        if (buffer != nint.Zero)
            Marshal.FreeCoTaskMem(buffer);
        return str;
    }

    [DllImport(FoundationFramework)]
    public static extern void CFRelease(nint handle);

    [DllImport(CoreFoundation)]
    static extern nint CFStringGetCharactersPtr(nint theString);

    [DllImport(AppKitFramework)]
    public static extern nint NSSelectorFromString(nint cfstr);

    public static unsafe nint CreateCfString(string aString)
    {
        var bytes = Encoding.Unicode.GetBytes(aString);
        fixed (byte* b = bytes)
        {
            var cfStr = CFStringCreateWithBytes(
                nint.Zero,
                (nint)b,
                bytes.Length,
                CfStringEncoding.Utf16,
                false
            );
            return cfStr;
        }
    }

    [DllImport(FoundationFramework)]
    public static extern nint CFStringCreateWithBytes(
        nint allocator,
        nint buffer,
        long bufferLength,
        CfStringEncoding encoding,
        bool isExternalRepresentation
    );

    public enum CfStringEncoding : uint
    {
        Utf16 = 0x0100,
        Utf16Be = 0x10000100,
        Utf16Le = 0x14000100,
        Ascii = 0x0600,
    }

    [StructLayout(LayoutKind.Sequential)]
    struct CfRange(CFIndex length, CFIndex location)
    {
        public CFIndex Length = length;
        public CFIndex Location = location;
    }
}
