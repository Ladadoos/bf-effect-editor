// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using System;
using BattleForgeEffectEditor.Models;

namespace BattleForgeEffectEditor.Application.ViewModel.Keyframe
{
    public class EnumKeyframeViewModel : KeyframeViewModel<FloatKeyFrame>
    {
        public Array Enums => Enum.GetValues(enumType);

        public object SelectedEnum
        {
            get => Enum.ToObject(enumType, (int)keyFrame.Data);
            set
            {
                keyFrame.Data = (float)(int)value;
                OnPropertyChanged();
            }
        }

        private Type enumType;

        public EnumKeyframeViewModel(FloatKeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor, Type enumType)
            : base(keyFrame, trackEditor)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException();

            this.enumType = enumType;
        }
    }
}
