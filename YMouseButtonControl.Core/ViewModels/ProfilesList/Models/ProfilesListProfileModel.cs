using System;
using System.Linq;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Models;

public sealed class ProfilesListProfileModel : ReactiveObject
{
    private int _id;
    private string _description;
    private bool _checked;
    private readonly bool _isDefault;
    private readonly int _displayPriority;
    private string? _process;
    private string? _name;

    public ProfilesListProfileModel(ProfileVm profileVm, IProfilesService profilesService)
    {
        _id = profileVm.Id;
        _description = profileVm.Description;
        _checked = profileVm.Checked;
        _isDefault = profileVm.IsDefault;
        _displayPriority = profileVm.DisplayPriority;
        _process = profileVm.Process;
        _name = profileVm.Name;

        this.WhenAnyValue(x => x.Id)
            .Subscribe(id =>
            {
                profilesService.ProfilesSc.Edit(inner =>
                {
                    var tgt = inner.Items.FirstOrDefault(x => x.Id == Id);
                    if (tgt is not null)
                    {
                        tgt.Id = id;
                    }
                });
            });
        this.WhenAnyValue(x => x.Description)
            .Subscribe(description =>
            {
                profilesService.ProfilesSc.Edit(inner =>
                {
                    var tgt = inner.Items.FirstOrDefault(x => x.Id == Id);
                    if (tgt is not null)
                    {
                        tgt.Description = description;
                    }
                });
            });
        this.WhenAnyValue(x => x.Checked)
            .Subscribe(@checked =>
            {
                profilesService.ProfilesSc.Edit(inner =>
                {
                    var tgt = inner.Items.FirstOrDefault(x => x.Id == Id);
                    if (tgt is not null)
                    {
                        tgt.Checked = @checked;
                    }
                });
            });
        this.WhenAnyValue(x => x.DisplayPriority)
            .Subscribe(displayPriority =>
            {
                profilesService.ProfilesSc.Edit(inner =>
                {
                    var tgt = inner.Items.FirstOrDefault(x => x.Id == Id);
                    if (tgt is not null)
                    {
                        tgt.DisplayPriority = displayPriority;
                    }
                });
            });
        this.WhenAnyValue(x => x.Process)
            .Subscribe(Process =>
            {
                profilesService.ProfilesSc.Edit(inner =>
                {
                    var tgt = inner.Items.FirstOrDefault(x => x.Id == Id);
                    if (tgt is not null)
                    {
                        tgt.Process = Process!;
                    }
                });
            });
        this.WhenAnyValue(x => x.Name)
            .Subscribe(Name =>
            {
                profilesService.ProfilesSc.Edit(inner =>
                {
                    var tgt = inner.Items.FirstOrDefault(x => x.Id == Id);
                    if (tgt is not null)
                    {
                        tgt.Name = Name!;
                    }
                });
            });
    }

    public int Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }
    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    public bool Checked
    {
        get => _checked;
        set => this.RaiseAndSetIfChanged(ref _checked, value);
    }
    public bool IsDefault => _isDefault;
    public int DisplayPriority => _displayPriority;
    public string? Process
    {
        get => _process;
        set => this.RaiseAndSetIfChanged(ref _process, value);
    }
    public string? Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
}
