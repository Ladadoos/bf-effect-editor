// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;

namespace BattleForgeEffectEditor.Application.ViewModel.StaticTrack
{
    public class Vector3StaticTrackViewModel : StaticTrackViewModel<Vector3StaticTrack>
    {
        public float X
        {
            get => Track.Data.X;
            set
            {
                Track.Data = new Vector3(value, Track.Data.Y, Track.Data.Z);
                OnPropertyChanged();
            }
        }

        public float Y
        {
            get => Track.Data.Y;
            set
            {
                Track.Data = new Vector3(Track.Data.X, value, Track.Data.Z);
                OnPropertyChanged();
            }
        }

        public float Z
        {
            get => Track.Data.Z;
            set
            {
                Track.Data = new Vector3(Track.Data.X, Track.Data.Y, value);
                OnPropertyChanged();
            }
        }

        public Vector3StaticTrackViewModel(StaticTrackListViewModel staticTrackList, IStaticTrack track)
            : base(staticTrackList, track)
        {
        }
    }
}
