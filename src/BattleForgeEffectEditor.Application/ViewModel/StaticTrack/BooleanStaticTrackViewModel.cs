// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.Enums;

namespace BattleForgeEffectEditor.Application.ViewModel.StaticTrack
{
    public class BooleanStaticTrackViewModel : EnumStaticTrackViewModel<Bool>
    {
        public BooleanStaticTrackViewModel(StaticTrackListViewModel staticTrackList, IStaticTrack track)
            : base(staticTrackList, track)
        {
        }
    }
}
