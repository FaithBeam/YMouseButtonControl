using System;
using System.Runtime.InteropServices;
using System.Text;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.CurrentWindow;

// a lot of this code was copied from https://stackoverflow.com/a/44669560 and https://github.com/isnowrain/CoreFoundation/blob/master/Project/CFType.cs
public partial class GetCurrentWindowOsx : IGetCurrentWindow
{
    public string ForegroundWindow => GetForegroundWindow();

    private string GetForegroundWindow()
    {
        var nsWorkspace = objc_getClass("NSWorkspace");
        var sharedWorkspace = objc_msgSend(nsWorkspace, GetSelector("sharedWorkspace"));
        var frontMostApplication = objc_msgSend(
            sharedWorkspace,
            GetSelector("frontmostApplication")
        );
        var localizedName = objc_msgSend(frontMostApplication, GetSelector("localizedName"));
        var localizedNameBuffer = new char[CFStringGetLength(localizedName) + 1];
        CfStringGetCStringWrapper(
            localizedName,
            localizedNameBuffer.AsSpan(),
            localizedNameBuffer.Length,
            KCfStringEncodingUtf8
        );
        return new string(localizedNameBuffer).TrimEnd('\0');
    }

    private const int KCfStringEncodingUtf8 = 0x08000100; // UTF-8 encoding

    private const string AppKitFramework = "/System/Library/Frameworks/AppKit.framework/AppKit";
    private const string FoundationFramework =
        "/System/Library/Frameworks/Foundation.framework/Foundation";
    private const string CoreFoundation =
        "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

    [LibraryImport(AppKitFramework, StringMarshalling = StringMarshalling.Utf8)]
    private static partial nint objc_getClass(string name);

    [LibraryImport(FoundationFramework)]
    private static partial nint objc_msgSend(nint target, nint selector);

    [LibraryImport(CoreFoundation)]
    private static partial long CFStringGetLength(nint theString);

    [LibraryImport(FoundationFramework)]
    private static partial void CFRelease(nint handle);

    [LibraryImport(AppKitFramework)]
    private static partial nint NSSelectorFromString(nint cfStr);

    [LibraryImport(CoreFoundation, EntryPoint = "CFStringCreateWithCString")]
    private static partial nint CfStringCreateWithCString(
        nint allocator,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string cStr,
        int encoding
    );

    [LibraryImport(CoreFoundation, EntryPoint = "CFStringGetCString")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CfStringGetCString(
        nint str,
        Span<byte> buffer,
        int bufferSize,
        int encoding
    );

    private static nint GetSelector(string name)
    {
        var cfStrSelector = CfStringCreateWithCString(nint.Zero, name, KCfStringEncodingUtf8);
        var selector = NSSelectorFromString(cfStrSelector);
        CFRelease(cfStrSelector);
        return selector;
    }

    private static bool CfStringGetCStringWrapper(
        nint str,
        Span<char> buffer,
        int bufferSize,
        int encoding
    )
    {
        var span = MemoryMarshal.Cast<char, byte>(buffer)[(buffer.Length - 1)..];
        if (!CfStringGetCString(str, span, bufferSize, encoding))
        {
            return false;
        }
        Encoding.ASCII.GetChars(span[..^1], buffer);
        return true;
    }
}
