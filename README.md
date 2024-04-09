# YMouseButtonControl

This is an attempt at a cross-platform clone of X-Mouse-Button-Control.

![Image of YMouseButtonControl](https://i.imgur.com/PCMOXN0.png)

## Usage

1. Download the latest release from the releases page for your platform
2. Extract the archive
3. Run YMouseButtonControl

## Build

### Requirements

* [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks)

### Build Commands

1. `git clone https://github.com/FaithBeam/YMouseButtonControl`
2. `cd YMouseButtonControl`
3. `dotnet publish YMouseButtonControl/YMouseButtonControl.csproj -c Release -r YOUR_PLATFORM --self-contained true -p:PublishSingleFile=true -p:DebugType=embedded -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o bin`
    * YOUR_PLATFORM: win-x64, linux-x64, osx-x64, [more runtimes here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)
4. YMouseButtonControl executable is located in bin folder