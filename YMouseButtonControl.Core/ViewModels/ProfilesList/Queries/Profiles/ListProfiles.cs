using System;
using System.Collections.ObjectModel;
using DynamicData;
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
                .Connect()
                .Transform(x => new ProfilesListProfileModel(x, profilesService))
                .Bind(out ReadOnlyObservableCollection<ProfilesListProfileModel> profiles)
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
