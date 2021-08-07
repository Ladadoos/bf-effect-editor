// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;

namespace BattleForgeEffectEditor.Application.ViewModel.StaticTrack
{
    public class FloatStaticTrackViewModel : StaticTrackViewModel<FloatStaticTrack>
    {
        public float Data
        {
            get => Track.Data;
            set
            {
                Track.Data = value;
                OnPropertyChanged();
            }
        }

        public FloatStaticTrackViewModel(StaticTrackListViewModel staticTrackList, IStaticTrack track)
            : base(staticTrackList, track)
        {
        }
    }
}
