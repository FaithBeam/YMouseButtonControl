using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using ReactiveUI;
using YMouseButtonControl.Core.Repositories;
using YMouseButtonControl.Core.Services.Settings;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.Theme;

public interface IThemeService
{
    public IBrush Background { get; }
    public IBrush Highlight { get; }
    ThemeVariant ThemeVariant { get; }
    List<ThemeVm> Themes { get; }
}

public class ThemeService : ReactiveObject, IThemeService
{
    private readonly IRepository<DataAccess.Models.Theme, ThemeVm> _themeRepo;
    private readonly SettingIntVm _themeSetting;
    private readonly ThemeVm _themeVm;
    private IBrush _background;
    private IBrush _highlight;
    private readonly ThemeVariant _themeVariant;

    public ThemeService(
        IRepository<DataAccess.Models.Theme, ThemeVm> themeRepo,
        ISettingsService settingsService
    )
    {
        _themeRepo = themeRepo;
        _themeSetting =
            settingsService.GetSetting("Theme") as SettingIntVm
            ?? throw new Exception("Error retrieving theme setting");
        _themeVm =
            _themeRepo.GetById(_themeSetting.IntValue)
            ?? throw new Exception($"Error retrieving theme with id: {_themeSetting.IntValue}");
        _themeVariant = GetThemeVariant();
        _background = GetBackground();
        _highlight = GetHighlight();
    }

    public List<ThemeVm> Themes => [.. _themeRepo.GetAll().OrderBy(x => x.Id)];

    public ThemeVariant ThemeVariant => _themeVariant;

    public IBrush Background
    {
        get => _background;
        set => this.RaiseAndSetIfChanged(ref _background, value);
    }

    public IBrush Highlight
    {
        get => _highlight;
        set => this.RaiseAndSetIfChanged(ref _highlight, value);
    }

    private IBrush GetBackground()
    {
        // Background is of the form #aarrggbb
        if (_themeVm.Background.StartsWith('#'))
        {
            return Brush.Parse(_themeVm.Background);
        }

        // Background is an avalonia resource like SystemAltHighColor
        if (
            Application.Current!.TryGetResource(
                _themeVm.Background,
                Application.Current.ActualThemeVariant,
                out var backgroundBrush
            )
        )
        {
            if (backgroundBrush is null)
            {
                throw new Exception("Error retrieving background brush");
            }
            var bbStr = backgroundBrush.ToString();
            if (string.IsNullOrWhiteSpace(bbStr))
            {
                throw new Exception("Error retrieving background brush");
            }
            var brush = Brush.Parse(bbStr);
            return brush;
        }

        // Background may be a color like White, Black, etc.
        return Brush.Parse(_themeVm.Background);
    }

    private IBrush GetHighlight()
    {
        // Highlight is of the form #aarrggbb
        if (_themeVm.Highlight.StartsWith('#'))
        {
            return Brush.Parse(_themeVm.Highlight);
        }

        // Highlight is an avalonia resource like SystemAltHighColor
        if (
            Application.Current!.TryGetResource(
                _themeVm.Highlight,
                Application.Current.ActualThemeVariant,
                out var highlightBrush
            )
        )
        {
            if (highlightBrush is null)
            {
                throw new Exception("Error retrieving highlight brush");
            }
            var bbStr = highlightBrush.ToString();
            if (string.IsNullOrWhiteSpace(bbStr))
            {
                throw new Exception("Error retrieving highlight brush");
            }
            var brush = Brush.Parse(bbStr);
            return brush;
        }

        // Highlight may be a color like White, Black, etc.
        return Brush.Parse(_themeVm.Highlight);
    }

    private ThemeVariant GetThemeVariant()
    {
        return _themeSetting.IntValue switch
        {
            1 => Application.Current!.ActualThemeVariant == ThemeVariant.Light
                ? ThemeVariant.Light
                : ThemeVariant.Dark,
            2 => ThemeVariant.Light,
            3 => ThemeVariant.Dark,
            _ => throw new ArgumentOutOfRangeException(
                $"Invalid theme id: {_themeSetting.IntValue}"
            ),
        };
    }
}
