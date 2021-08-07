// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Application.ViewModel.GenericControls;
using BattleForgeEffectEditor.Models;
using System.Collections.Generic;

namespace BattleForgeEffectEditor.Application.ViewModel.StaticTrack
{
    public interface IStaticTrackViewModel
    {
    }

    public class StaticTrackViewModel<T> : ObservableObject, IStaticTrackViewModel where T : IStaticTrack
    {
        public string Name => Track.TrackType.ToString().Replace('_', ' ');

        public MultiStageButtonViewModel DeleteButton { get; private set; }

        public StaticTrackListViewModel StaticTrackList { get; set; }

        public T Track { get; protected set; }

        public StaticTrackViewModel(StaticTrackListViewModel staticTrackList, IStaticTrack track)
        {
            Track = (T)track;
            StaticTrackList = staticTrackList;

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
                        StaticTrackList.RemoveStaticTrack(this);
                    })
                },
            });
        }
    }
}
