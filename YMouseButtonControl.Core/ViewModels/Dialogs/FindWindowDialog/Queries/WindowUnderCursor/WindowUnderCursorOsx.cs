using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor.Models;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog.Queries.WindowUnderCursor;

public static partial class WindowUnderCursorOsx
{
    public sealed partial class Handler : IWindowUnderCursorHandler
    {
        private sealed record MyRec(string WindowName, int X, int Y, int Width, int Height);

        public Response? Execute(Query q)
        {
            // KCgWindowListOptionOnScreenOnly causes the windows to be listed from front to back.
            // So when matching the window, choose the first one that matches the mouse coordinates
            var windowList = CGWindowListCopyWindowInfo(
                KCgWindowListOptionOnScreenOnly | ExcludeDesktopElements,
                KCgNullWindowId
            );
            var count = CFArrayGetCount(windowList);

            var myRecs = new List<MyRec>();

            for (var i = 0; i < count; i++)
            {
                var val = CFArrayGetValueAtIndex(windowList, i);
                var kCgWindowBounds = CFDictionaryGetValue(
                    val,
                    CfStringCreateWithCString(IntPtr.Zero, "kCGWindowBounds", KCfStringEncodingUtf8)
                );

                var ownerNamePtr = CFDictionaryGetValue(
                    val,
                    CfStringCreateWithCString(
                        IntPtr.Zero,
                        "kCGWindowOwnerName",
                        KCfStringEncodingUtf8
                    )
                );

                if (ownerNamePtr == IntPtr.Zero)
                {
                    continue;
                }

                var buffArr = new char[CFStringGetLength(ownerNamePtr) + 1];
                CfStringGetCStringWrapper(
                    ownerNamePtr,
                    buffArr.AsSpan(),
                    buffArr.Length,
                    KCfStringEncodingUtf8
                );
                var ownerName = new string(buffArr).TrimEnd('\0');

                if (kCgWindowBounds == IntPtr.Zero)
                {
                    continue;
                }

                var xPtr = CFDictionaryGetValue(
                    kCgWindowBounds,
                    CfStringCreateWithCString(IntPtr.Zero, "X", KCfStringEncodingUtf8)
                );
                var yPtr = CFDictionaryGetValue(
                    kCgWindowBounds,
                    CfStringCreateWithCString(IntPtr.Zero, "Y", KCfStringEncodingUtf8)
                );
                var widthPtr = CFDictionaryGetValue(
                    kCgWindowBounds,
                    CfStringCreateWithCString(IntPtr.Zero, "Width", KCfStringEncodingUtf8)
                );
                var heightPtr = CFDictionaryGetValue(
                    kCgWindowBounds,
                    CfStringCreateWithCString(IntPtr.Zero, "Height", KCfStringEncodingUtf8)
                );

                // Get values from CFNumber
                CfNumberGetValue(xPtr, KCfNumberIntType, out var x);
                CfNumberGetValue(yPtr, KCfNumberIntType, out var y);
                CfNumberGetValue(widthPtr, KCfNumberIntType, out var width);
                CfNumberGetValue(heightPtr, KCfNumberIntType, out var height);

                myRecs.Add(new MyRec(ownerName, x, y, width, height));
            }

            // get the first window that the input mouse coordinates is within
            var filtered = myRecs
                .Where(x => x is { X: > 0, Y: > 0, Width: > 0, Height: > 0 })
                .Where(x =>
                {
                    var windowCoord1 = new { x.X, x.Y };
                    var windowCoord2 = new { X = x.X + x.Width, Y = x.Y + x.Height };
                    var inputCoord = new { q.X, q.Y };
                    return inputCoord.X >= windowCoord1.X
                        && inputCoord.Y >= windowCoord1.Y
                        && inputCoord.X <= windowCoord2.X
                        && inputCoord.Y <= windowCoord2.Y;
                })
                .FirstOrDefault();
            return filtered is null ? null : new Response(filtered.WindowName);
        }

        private const int KCgWindowListOptionOnScreenOnly = 1;
        private const int ExcludeDesktopElements = 16;
        private const int KCgNullWindowId = 0;
        private const int KCfStringEncodingUtf8 = 0x08000100; // UTF-8 encoding
        private const int KCfNumberIntType = 3; // Type for CFNumber to int

        private const string CoreGraphics =
            "/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics";

        private const string CoreFoundation =
            "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

        [LibraryImport(CoreGraphics)]
        private static partial nint CGWindowListCopyWindowInfo(int option, int windowId);

        [LibraryImport(CoreFoundation)]
        private static partial nint CFArrayGetValueAtIndex(nint array, int index);

        [LibraryImport(CoreFoundation)]
        private static partial int CFArrayGetCount(nint array);

        [LibraryImport(CoreFoundation)]
        private static partial nint CFDictionaryGetValue(nint dict, nint key);

        [LibraryImport(CoreFoundation, EntryPoint = "CFStringCreateWithCString")]
        private static partial nint CfStringCreateWithCString(
            nint allocator,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string cStr,
            int encoding
        );

        [LibraryImport(CoreFoundation)]
        private static partial int CFStringGetLength(nint str);

        [LibraryImport(CoreFoundation, EntryPoint = "CFNumberGetValue")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool CfNumberGetValue(nint number, int type, out int value);

        [LibraryImport(CoreFoundation, EntryPoint = "CFStringGetCString")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool CfStringGetCString(
            nint str,
            Span<byte> buffer,
            int bufferSize,
            int encoding
        );

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
}
