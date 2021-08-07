// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Application.ViewModel.GenericControls;
using BattleForgeEffectEditor.Models;
using System.Collections.Generic;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class TrackListItemViewModel : ObservableObject
    {
        public string Name => Track.TrackType.ToString().Replace('_', ' ');

        public MultiStageButtonViewModel DeleteButton { get; private set; }

        public VisibilityButtonViewModel HideTracksButton { get; private set; }

        public Track Track { get; private set; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsTrackEnabled
        {
            get => Track.IsEnabled;
            set
            {
                Track.IsEnabled = value;
                HideTracksButton.Visible = value;
                OnPropertyChanged();
            }
        }

        public TrackListItemViewModel(TrackListViewModel trackList, Track track)
        {
            Track = track;

            DeleteButton = new MultiStageButtonViewModel(new List<ButtonStage>
            {
                new ButtonStage()
                {
                    Text = "X",
                },
                new ButtonStage()
                {
                    Text = "✓",
                    FallbackTimeSeconds = 1,
                    Command = new RelayCommand((_) =>
                    {
                        trackList.RemoveTrack(this);
                    })
                },
            });

            HideTracksButton = new VisibilityButtonViewModel(true, (visible) =>
            {
                IsTrackEnabled = visible;
            });
        }
    }
}
