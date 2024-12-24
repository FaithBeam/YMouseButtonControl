using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using YMouseButtonControl.Core.Services.Profiles;
using YMouseButtonControl.Core.ViewModels.ProfilesList.Models;

namespace YMouseButtonControl.Core.ViewModels.ProfilesList.Queries.Profiles;

public static class ListProfiles
{
    public sealed class Handler(IProfilesService profilesService) : IDisposable
    {
        private IDisposable? _disposable;

        public ReadOnlyObservableCollection<ProfilesListProfileModel> Execute()
        {
            _disposable = profilesService
                .ProfilesSc.Connect()
                .AutoRefresh()
                .Transform(x => new ProfilesListProfileModel(x, profilesService))
                .ObserveOn(RxApp.MainThreadScheduler)
                .SortAndBind(
                    out var profiles,
                    SortExpressionComparer<ProfilesListProfileModel>.Ascending(x =>
                        x.DisplayPriority
                    )
                )
                .DisposeMany()
                .Subscribe();
            return profiles;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
