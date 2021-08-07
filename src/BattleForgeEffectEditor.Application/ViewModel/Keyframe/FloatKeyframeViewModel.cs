// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;

namespace BattleForgeEffectEditor.Application.ViewModel.Keyframe
{
    public class FloatKeyframeViewModel : KeyframeViewModel<FloatKeyFrame>
    {
        public float Value
        {
            get => keyFrame.Data;
            set
            {
                keyFrame.Data = value;
                OnPropertyChanged();
            }
        }

        public FloatKeyframeViewModel(FloatKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
            : base(keyFrame, trackEditor)
        {

        }
    }
}
