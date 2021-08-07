// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleForgeEffectEditor.Application.ViewModel.StaticTrack
{
    public class EnumStaticTrackViewModel<T> : StaticTrackViewModel<FloatStaticTrack> where T : Enum
    {
        public IEnumerable<T> Enums => Enum.GetValues(typeof(T)).OfType<T>();

        public T SelectedEnum
        {
            get => (T)(object)(int)Track.Data;
            set
            {
                Track.Data = (float)(int)(object)value;
                OnPropertyChanged();
            }
        }

        public EnumStaticTrackViewModel(StaticTrackListViewModel staticTrackList, IStaticTrack track)
            : base(staticTrackList, track)
        {
        }
    }

    public class EffectTypeStaticTrackViewModel : EnumStaticTrackViewModel<EffecType>
    {
        public EffectTypeStaticTrackViewModel(StaticTrackListViewModel staticTrackList, IStaticTrack track)
            : base(staticTrackList, track)
        {
        }
    }

    public class PriorityStaticTrackViewModel : EnumStaticTrackViewModel<Priority>
    {
        public PriorityStaticTrackViewModel(StaticTrackListViewModel staticTrackList, IStaticTrack track)
            : base(staticTrackList, track)
        {
        }
    }

    public class SoundLoopStaticTrackViewModel : EnumStaticTrackViewModel<SoundLoop>
    {
        public SoundLoopStaticTrackViewModel(StaticTrackListViewModel staticTrackList, IStaticTrack track)
            : base(staticTrackList, track)
        {
        }
    }
}
