// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;

namespace BattleForgeEffectEditor.Application.ViewModel.Keyframe
{
    public class Vector3KeyframeViewModel : KeyframeViewModel<Vector3KeyFrame>
    {
        public float X
        {
            get => keyFrame.Data.X;
            set
            {
                keyFrame.Data = new Vector3(value, keyFrame.Data.Y, keyFrame.Data.Z);
                OnPropertyChanged();
            }
        }

        public float Y
        {
            get => keyFrame.Data.Y;
            set
            {
                keyFrame.Data = new Vector3(keyFrame.Data.X, value, keyFrame.Data.Z);
                OnPropertyChanged();
            }
        }

        public float Z
        {
            get => keyFrame.Data.Z;
            set
            {
                keyFrame.Data = new Vector3(keyFrame.Data.X, keyFrame.Data.Y, value);
                OnPropertyChanged();
            }
        }

        public Vector3KeyframeViewModel(Vector3KeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
            : base(keyFrame, trackEditor)
        {

        }
    }
}
