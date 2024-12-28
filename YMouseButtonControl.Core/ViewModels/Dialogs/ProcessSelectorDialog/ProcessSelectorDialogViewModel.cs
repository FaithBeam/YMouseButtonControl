using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Styling;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using YMouseButtonControl.Core.ViewModels.Dialogs.FindWindowDialog;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Processes;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Profiles;
using YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog.Queries.Themes;
using YMouseButtonControl.Core.ViewModels.Models;
using YMouseButtonControl.Domain.Models;

namespace YMouseButtonControl.Core.ViewModels.Dialogs.ProcessSelectorDialog;

public interface IProcessSelectorDialogViewModel
{
    ReactiveCommand<Unit, Unit> RefreshButtonCommand { get; }
}

public class ProcessSelectorDialogViewModel
    : DialogBase,
        IProcessSelectorDialogViewModel,
        IActivatableViewModel
{
    private readonly SourceList<Queries.Processes.Models.ProcessModel> _sourceProcessModels;
    private string? _processFilter;
    private readonly ReadOnlyObservableCollection<Queries.Processes.Models.ProcessModel> _filtered;
    private Queries.Processes.Models.ProcessModel? _processModel;
    private string? _selectedProcessModuleName;
    private string? _selectedProcessMainWindowTitle;

    public ProcessSelectorDialogViewModel(
        IListProcessesHandler listProcessesHandler,
        GetMaxProfileId.Handler getMaxProfileIdHandler,
        GetThemeVariant.Handler getThemeVariantHandler,
        string? selectedProcessModuleName,
        IFindWindowDialogVmFactory findWindowDialogVmFactory
    )
    {
        Activator = new ViewModelActivator();

        ThemeVariant = getThemeVariantHandler.Execute();
        _sourceProcessModels = new SourceList<Queries.Processes.Models.ProcessModel>();
        var dynamicFilter = this.WhenValueChanged(x => x.ProcessFilter)
            .Select(CreateProcessFilterPredicate);
        var filteredDisposable = _sourceProcessModels
            .Connect()
            .Filter(dynamicFilter)
            .Sort(
                SortExpressionComparer<Queries.Processes.Models.ProcessModel>.Ascending(x =>
                    x.Process.ProcessName
                )
            )
            .Bind(out _filtered)
            .Subscribe();

        ShowSpecificWindowInteraction = new Interaction<FindWindowDialogVm, Unit>();
        SpecificWindowCmd = ReactiveCommand.CreateFromTask(
            async () =>
                await ShowSpecificWindowInteraction.Handle(findWindowDialogVmFactory.Create())
        );

        RefreshButtonCommand = ReactiveCommand.Create(
            () => _sourceProcessModels.EditDiff(listProcessesHandler.Execute())
        );
        var canExecuteOkCommand = this.WhenAnyValue(
            x => x.SelectedProcessModel,
            selector: model => model is not null
        );
        OkCommand = ReactiveCommand.Create(
            () =>
            {
                var maxBmId = getMaxProfileIdHandler.Execute();
                var buttonMappings = new List<BaseButtonMappingVm>
                {
                    CreateButtonMappings(maxBmId, MouseButton.Mb1),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mb2),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mb3),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mb4),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mb5),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mwu),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mwd),
                    CreateButtonMappings(maxBmId += 4, MouseButton.Mwl),
                    CreateButtonMappings(maxBmId + 4, MouseButton.Mwr),
                };
                return new ProfileVm(buttonMappings)
                {
                    Name = SelectedProcessModuleName ?? string.Empty,
                    Description = SelectedProcessMainWindowTitle ?? string.Empty,
                    Process = SelectedProcessModuleName ?? string.Empty,
                    WindowCaption = "N/A",
                    WindowClass = "N/A",
                    ParentClass = "N/A",
                    MatchType = "N/A",
                };
            },
            canExecuteOkCommand
        );
        this.WhenAnyValue(x => x.SelectedProcessModel)
            .Subscribe(x =>
            {
                SelectedProcessModuleName = x?.Process.MainModule?.ModuleName;
                SelectedProcessMainWindowTitle = x?.Process.MainWindowTitle;
            });
        if (!string.IsNullOrWhiteSpace(selectedProcessModuleName))
        {
            var foundProc = _filtered.FirstOrDefault(x =>
                x.Process.MainModule?.ModuleName == selectedProcessModuleName
            );
            if (foundProc is not null)
            {
                SelectedProcessModel = foundProc;
            }
        }

        this.WhenActivated(disposables =>
        {
            _sourceProcessModels.EditDiff(listProcessesHandler.Execute());
            Disposable.Create(() => { }).DisposeWith(disposables);
        });
    }

    private static List<BaseButtonMappingVm> CreateButtonMappings(
        int id,
        MouseButton mouseButton
    ) =>
        [
            new NothingMappingVm
            {
                Id = ++id,
                MouseButton = mouseButton,
                Selected = true,
            },
            new DisabledMappingVm { Id = ++id, MouseButton = mouseButton },
            new SimulatedKeystrokeVm { Id = ++id, MouseButton = mouseButton },
            new RightClickVm { Id = ++id, MouseButton = mouseButton },
        ];

    private Func<Queries.Processes.Models.ProcessModel, bool> CreateProcessFilterPredicate(
        string? txt
    )
    {
        if (string.IsNullOrWhiteSpace(txt))
        {
            return _ => true;
        }

        return model =>
            !string.IsNullOrWhiteSpace(model.Process.ProcessName)
            && model.Process.ProcessName.Contains(txt, StringComparison.OrdinalIgnoreCase);
    }

    public string? ProcessFilter
    {
        get => _processFilter;
        set => this.RaiseAndSetIfChanged(ref _processFilter, value);
    }

    public ReactiveCommand<Unit, Unit> RefreshButtonCommand { get; }

    public ReactiveCommand<Unit, ProfileVm> OkCommand { get; }

    public IInteraction<FindWindowDialogVm, Unit> ShowSpecificWindowInteraction { get; }

    public ReactiveCommand<Unit, Unit> SpecificWindowCmd { get; }

    public ReadOnlyObservableCollection<Queries.Processes.Models.ProcessModel> Filtered =>
        _filtered;

    public Queries.Processes.Models.ProcessModel? SelectedProcessModel
    {
        get => _processModel;
        set => this.RaiseAndSetIfChanged(ref _processModel, value);
    }

    public string? SelectedProcessModuleName
    {
        get => _selectedProcessModuleName;
        set => this.RaiseAndSetIfChanged(ref _selectedProcessModuleName, value);
    }

    public string? SelectedProcessMainWindowTitle
    {
        get => _selectedProcessMainWindowTitle;
        set => this.RaiseAndSetIfChanged(ref _selectedProcessMainWindowTitle, value);
    }

    public ThemeVariant ThemeVariant { get; }

    public ViewModelActivator Activator { get; }
}
