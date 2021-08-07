// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models;
using System.Windows.Media;

namespace BattleForgeEffectEditor.Application.ViewModel.Keyframe
{
    public class ColorKeyframeViewModel : KeyframeViewModel<Vector3KeyFrame>
    {
        private Color color;
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                keyFrame.Data = new Vector3(color.R / 255.0F, color.G / 255.0F, color.B / 255.0F);
                OnPropertyChanged();

                RaisePropertyChanged(() => R);
                RaisePropertyChanged(() => G);
                RaisePropertyChanged(() => B);
            }
        }

        public float R => keyFrame.Data.X;
        public float G => keyFrame.Data.Y;
        public float B => keyFrame.Data.Z;

        public ColorKeyframeViewModel(Vector3KeyFrame keyFrame, DynamicTrackEditorViewModel trackEditor)
            : base(keyFrame, trackEditor)
        {
            color = new Color
            {
                R = (byte)(keyFrame.Data.X * 255),
                G = (byte)(keyFrame.Data.Y * 255),
                B = (byte)(keyFrame.Data.Z * 255),
                A = 255
            };
        }
    }
}
