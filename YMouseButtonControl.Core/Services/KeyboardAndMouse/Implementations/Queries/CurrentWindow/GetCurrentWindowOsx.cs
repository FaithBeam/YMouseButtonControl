using System;
using System.Runtime.InteropServices;
using System.Text;
using CFIndex = long;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.CurrentWindow;

// a lot of this code was copied from https://stackoverflow.com/a/44669560 and https://github.com/isnowrain/CoreFoundation/blob/master/Project/CFType.cs
public partial class GetCurrentWindowOsx : IGetCurrentWindow
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

    [LibraryImport(AppKitFramework, StringMarshalling = StringMarshalling.Utf8)]
    internal static partial nint objc_getClass(string name);

    [LibraryImport(FoundationFramework, EntryPoint = "objc_msgSend")]
    internal static partial nint objc_msgSend_retIntPtr(nint target, nint selector);

    internal static nint GetSelector(string name)
    {
        nint cfstrSelector = CreateCfString(name);
        nint selector = NSSelectorFromString(cfstrSelector);
        CFRelease(cfstrSelector);
        return selector;
    }

    [LibraryImport(CoreFoundation)]
    internal static partial CFIndex CFStringGetLength(nint theString);

    [LibraryImport(CoreFoundation)]
    internal static partial void CFStringGetCharacters(nint theString, CfRange range, nint buffer);

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

    [LibraryImport(FoundationFramework)]
    internal static partial void CFRelease(nint handle);

    [LibraryImport(CoreFoundation)]
    internal static partial nint CFStringGetCharactersPtr(nint theString);

    [LibraryImport(AppKitFramework)]
    internal static partial nint NSSelectorFromString(nint cfstr);

    internal static unsafe nint CreateCfString(string aString)
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

    [LibraryImport(FoundationFramework)]
    internal static partial nint CFStringCreateWithBytes(
        nint allocator,
        nint buffer,
        long bufferLength,
        CfStringEncoding encoding,
        [MarshalAs(UnmanagedType.Bool)] bool isExternalRepresentation
    );

    public enum CfStringEncoding : uint
    {
        Utf16 = 0x0100,
        Utf16Be = 0x10000100,
        Utf16Le = 0x14000100,
        Ascii = 0x0600,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CfRange(CFIndex length, CFIndex location)
    {
        public CFIndex Length = length;
        public CFIndex Location = location;
    }
}
