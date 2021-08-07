// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.Utility;

namespace BattleForgeEffectEditor.Application.ViewModel.StaticTrack
{
    public class StringStaticTrackViewModel : StaticTrackViewModel<StringStaticTrack>
    {
        public string Data
        {
            get => Track.Data.ToString();
            set
            {
                Track.Data = new BfString(value);
                OnPropertyChanged();
            }
        }

        public StringStaticTrackViewModel(StaticTrackListViewModel staticTrackList, IStaticTrack track)
            : base(staticTrackList, track)
        {
        }
    }
}
