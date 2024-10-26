# YMouseButtonControl

This is an attempt at a cross-platform clone of X-Mouse-Button-Control.

![YMouseButtonControl 0 22 0](https://github.com/user-attachments/assets/a42266b8-dead-4dfd-b894-2deba1e9aa7e)

## Usage

1. Download the latest release from the [Releases](https://github.com/FaithBeam/YMouseButtonControl/releases) page for your platform
     * Alternatively, download the latest build from the [Actions tab](https://github.com/FaithBeam/YMouseButtonControl/actions)
2. Extract the archive
3. Run YMouseButtonControl

## OS Compatibility

Anything that can install .NET 8 should be able to run YMouseButtonControl

[.NET 8 Supported OS](https://github.com/dotnet/core/blob/main/release-notes/8.0/supported-os.md)

| **Operating System** | **Version** |
|----------------------|-------------|
| Windows 10           | 1607+       |
| Windows 11           | 22000+      |
| Ubuntu               | 20.04+      |
| macOS                | 12.0+       |

### Linux X11 vs. Wayland Considerations

* X11 is preferred when running this software: [More information](https://github.com/FaithBeam/YMouseButtonControl/wiki/Linux-X11-vs-Wayland-Considerations)

### Linux Issues

* [Linux Issues](https://github.com/FaithBeam/YMouseButtonControl/wiki/Linux-Issues)

## Build

### Requirements

* [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks)

### Build Commands

1. `git clone https://github.com/FaithBeam/YMouseButtonControl`
2. `cd YMouseButtonControl`
3. ```
   dotnet publish YMouseButtonControl/YMouseButtonControl.csproj \
    -c Release \
    -r YOUR_PLATFORM \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:DebugType=embedded \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -p:IncludeAllContentForSelfExtract=true \
    -p:EnableCompressionInSingleFile=true \
    -o bin
   ```
    * YOUR_PLATFORM: win-x64, linux-x64, osx-x64, [more runtimes here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)
4. YMouseButtonControl executable is located in bin folder
