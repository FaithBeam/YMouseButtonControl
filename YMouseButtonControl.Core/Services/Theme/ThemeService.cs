using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.DataAccess.Models;

namespace YMouseButtonControl.Core.Services.Theme;

public interface IThemeService
{
    IBrush HighlightLight { get; set; }
    IBrush HighlightDark { get; set; }
    IBrush BackgroundDark { get; set; }
    IBrush BackgroundLight { get; set; }
    IBrush CurBackground { get; set; }
    IBrush CurHighlight { get; set; }
    ThemeVariant ThemeVariant { get; }
}

public class ThemeService : ReactiveObject, IThemeService
{
    private readonly SettingIntVm _themeSetting;
    private IBrush _highlightLight = Brushes.Yellow;
    private IBrush _highlightDark = Brush.Parse("#3700b3");
    private IBrush _backgroundDark = Brushes.Black;
    private IBrush _backgroundLight = Brushes.White;
    private IBrush _curBackground;
    private IBrush _curHighlight;
    private ThemeVariant _themeVariant;

    public ThemeService(ISettingsService settingsService)
    {
        _themeSetting =
            settingsService.GetSetting("Theme") as SettingIntVm
            ?? throw new Exception("Error retrieving theme setting");
        _curBackground = GetCurrentThemeBackground();
        _curHighlight = GetCurrentThemeHighlight();
        _themeVariant = GetThemeVariant();
    }

    public IBrush HighlightLight
    {
        get => _highlightLight;
        set => this.RaiseAndSetIfChanged(ref _highlightLight, value);
    }

    public IBrush HighlightDark
    {
        get => _highlightDark;
        set => this.RaiseAndSetIfChanged(ref _highlightDark, value);
    }

    public IBrush BackgroundDark
    {
        get => _backgroundDark;
        set => this.RaiseAndSetIfChanged(ref _backgroundDark, value);
    }

    public IBrush BackgroundLight
    {
        get => _backgroundLight;
        set => this.RaiseAndSetIfChanged(ref _backgroundLight, value);
    }

    public IBrush CurBackground
    {
        get => _curBackground;
        set => this.RaiseAndSetIfChanged(ref _curBackground, value);
    }

    public IBrush CurHighlight
    {
        get => _curHighlight;
        set => this.RaiseAndSetIfChanged(ref _curHighlight, value);
    }

    public ThemeVariant ThemeVariant => _themeVariant;

    private ThemeVariant GetThemeVariant()
    {
        var theme = (ThemeEnum)_themeSetting.Value;
        return theme switch
        {
            ThemeEnum.Default => ThemeVariant.Default,
            ThemeEnum.Light => ThemeVariant.Light,
            ThemeEnum.Dark => ThemeVariant.Dark,
            _ => throw new ArgumentOutOfRangeException($"Invalid theme {theme}"),
        };
    }

    private IBrush GetCurrentThemeBackground()
    {
        var theme = (ThemeEnum)_themeSetting.Value;

        return theme switch
        {
            ThemeEnum.Default when Application.Current?.ActualThemeVariant == ThemeVariant.Light =>
                _backgroundLight,
            ThemeEnum.Default when Application.Current?.ActualThemeVariant == ThemeVariant.Dark =>
                _backgroundDark,
            ThemeEnum.Light => _backgroundLight,
            ThemeEnum.Dark => _backgroundDark,
            _ => throw new ArgumentOutOfRangeException($"Unknown theme {theme}"),
        };
    }

    private IBrush GetCurrentThemeHighlight()
    {
        var theme = (ThemeEnum)_themeSetting.Value;

        return theme switch
        {
            ThemeEnum.Default when Application.Current?.ActualThemeVariant == ThemeVariant.Light =>
                _highlightLight,
            ThemeEnum.Default when Application.Current?.ActualThemeVariant == ThemeVariant.Dark =>
                _highlightDark,
            ThemeEnum.Light => _highlightLight,
            ThemeEnum.Dark => _highlightDark,
            _ => throw new Exception($"Unknown theme {theme}"),
        };
    }
}
