using System;
using DynamicData;
using DynamicData.Binding;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.Models;

namespace YMouseButtonControl.Core.Services.KeyboardAndMouse.Implementations.Queries.Profiles;

public static class ListProfiles
{
    public sealed class ButtonMappingForMouseListener(BaseButtonMappingVm vm)
    {
        public bool? BlockOriginalMouseInput = vm.BlockOriginalMouseInput;
    }

    public sealed class ProfileForMouseListener(ProfileVm vm)
    {
        public bool Checked { get; } = vm.Checked;
        public string Process { get; } = vm.Process;
        public int DisplayPriority = vm.DisplayPriority;
        public ButtonMappingForMouseListener MouseButton1 { get; } = new(vm.MouseButton1);
        public ButtonMappingForMouseListener MouseButton2 { get; } = new(vm.MouseButton2);
        public ButtonMappingForMouseListener MouseButton3 { get; } = new(vm.MouseButton3);
        public ButtonMappingForMouseListener MouseButton4 { get; } = new(vm.MouseButton4);
        public ButtonMappingForMouseListener MouseButton5 { get; } = new(vm.MouseButton5);
        public ButtonMappingForMouseListener MouseWheelUp { get; } = new(vm.MouseWheelUp);
        public ButtonMappingForMouseListener MouseWheelDown { get; } = new(vm.MouseWheelDown);
        public ButtonMappingForMouseListener MouseWheelLeft { get; } = new(vm.MouseWheelLeft);
        public ButtonMappingForMouseListener MouseWheelRight { get; } = new(vm.MouseWheelRight);
    }

    public sealed class Handler(IProfilesCache profilesCache) : IDisposable
    {
        private IDisposable? _disposable;

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public System.Collections.ObjectModel.ReadOnlyObservableCollection<ProfileForMouseListener> Execute()
        {
            _disposable = profilesCache
                .ProfilesSc.Connect()
                .AutoRefresh()
                .Transform(x => new ProfileForMouseListener(x))
                .SortAndBind(
                    out var profiles,
                    SortExpressionComparer<ProfileForMouseListener>.Ascending(x =>
                        x.DisplayPriority
                    )
                )
                .DisposeMany()
                .Subscribe();
            return profiles;
        }
    }
}
